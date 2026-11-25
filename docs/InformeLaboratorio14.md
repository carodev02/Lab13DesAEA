# Laboratorio 14: autenticación y autorización

## Problema y solución

TallerMecanico registra a las personas que llevan su vehículo y los trabajos que dejan pendientes. La API ahora exige iniciar sesión para consultar o cambiar esos datos. Los permisos se separan entre Cliente, Recepcionista y Mecanico para que cada cuenta tenga solo el acceso necesario.

`Cliente` guarda nombre y teléfono. `OrdenServicio` guarda placa, descripción, fecha de ingreso, estado y `ClienteId`. Un cliente puede tener varias órdenes; cada orden pertenece a un solo cliente. `Usuario` guarda correo único, hash de contraseña y rol. El registro público no recibe el rol: el servidor crea la cuenta como `Cliente`.

## Roles y permisos

| Ruta o acción | Acceso |
|---|---|
| `POST /api/auth/registro`, `POST /api/auth/login` | Público |
| `GET /api/auth/perfil` | Cualquier rol autenticado |
| Consultar clientes, órdenes y órdenes de un cliente | Recepcionista o Mecanico |
| Crear, actualizar y borrar clientes; crear órdenes | Recepcionista |
| Actualizar y borrar órdenes | Mecanico |

El script incluye una fila de referencia por rol con hashes BCrypt de claves aleatorias desconocidas; esas filas no sirven como cuentas de acceso. Para probar, registra tres cuentas con contraseñas locales en `POST /api/auth/registro`. El endpoint siempre asigna el rol `Cliente`. Después cambia dos roles en SQL:

```sql
UPDATE dbo.Usuarios SET Rol = N'Recepcionista' WHERE Correo = N'<correo-de-recepcionista-local>';
UPDATE dbo.Usuarios SET Rol = N'Mecanico' WHERE Correo = N'<correo-de-mecanico-local>';
```

Las contraseñas y los tokens de prueba permanecen fuera de Git. El script guarda solo hashes BCrypt. El mismo ejemplo de actualización está comentado al final del script SQL.

## Autenticación y decisiones de seguridad

El inicio de sesión devuelve un JWT firmado con HMAC-SHA256. Contiene `sub` (Id del usuario), `email` y `rol`, junto con emisor, audiencia y expiración. No se incluyen la contraseña, el hash ni datos personales que no hagan falta; el contenido de un JWT se puede leer aunque su firma no se pueda modificar sin invalidarlo.

El secreto de firma y la duración del token se leen de User Secrets o del entorno (`JWT__KEY`, `JWT__EXPIRATIONMINUTES`). El secreto debe tener al menos 32 bytes y no se guarda en `appsettings.json`, el código ni Git. En local se puede generarlo con los pasos del README.

- **401 Unauthorized:** no se presentó un token válido, por ejemplo al consultar clientes sin iniciar sesión. El login responde igual si el correo no existe o si la contraseña no coincide, para no revelar qué cuentas están registradas.
- **403 Forbidden:** el token sí es válido, pero su rol no tiene ese permiso; por ejemplo, un Cliente intenta crear una orden.
- Guardar contraseñas en texto claro permitiría usarlas inmediatamente si se filtrara la base. BCrypt aplica una función de hash con costo computacional, de modo que no se recupera la contraseña original leyendo `ClaveHash`.
- El rol lo asigna el servidor y no el formulario público. Si una persona pudiera enviar `"rol": "Recepcionista"`, podría otorgarse permisos de personal. Los roles del token provienen del registro controlado y quedan protegidos por la firma.

`GET /api/auth/perfil` obtiene correo y rol de los claims del JWT ya validado; no consulta de nuevo la tabla. Swagger publica el esquema Bearer y permite probarlo mediante **Authorize**.

## Rutas de autenticación

| Método y ruta | Resultado esperado |
|---|---|
| `POST /api/auth/registro` | `201 Created`; `400` si falla la validación; `409` si el correo ya existe |
| `POST /api/auth/login` | `200 OK` y token; `400` si falla la validación; `401` si las credenciales no son válidas |
| `GET /api/auth/perfil` | `200 OK` con token; `401` sin token válido |

Las rutas de clientes y órdenes conservan el CRUD del laboratorio 13 y sus respuestas de `200`, `201` con `Location`, `204`, `400`, `404` y `409`. Las rutas de consulta y filtro de órdenes se limitan por rol, y se mantiene `GET /api/clientes/{id}/ordenes`. El detalle de endpoints y las solicitudes para los tres roles están en `TallerMecanico.Api/TallerMecanico.http`.

## Pruebas ejecutadas y capturas

Se ejecutaron 42 solicitudes HTTP contra la API conectada a LocalDB. Se crearon cuentas temporales por el registro, se asignaron roles para probar permisos y se borraron al terminar. `Location` se comprobó en las altas de clientes y órdenes. El detalle está en [evidencias/pruebas-lab14.txt](../evidencias/pruebas-lab14.txt).

| Resultado | Verificación ejecutada |
|---|---|
| `200` | Swagger/OpenAPI, logins por rol, perfil, listas, detalles y filtro |
| `201` | Registro; alta de cliente y orden con `Location` |
| `204` | Actualizaciones y eliminaciones autorizadas |
| `400` | DTO inválido en login, cliente y orden |
| `401` | Sin token y credenciales inválidas; las dos formas de login devolvieron el mismo cuerpo |
| `403` | Cliente consultando clientes y Mecanico intentando acciones de Recepcionista |
| `404` | Id inexistente, cliente asociado ausente y recursos eliminados |
| `409` | Correo duplicado, borrar cliente con órdenes y duplicar orden pendiente por placa |

Falta adjuntar las capturas de Visual Studio de tu equipo; no se incluyen imágenes simuladas. Para completarlas, ejecuta la API, inicia sesión con las tres cuentas locales en Swagger, pulsa **Authorize** con cada token y captura las respuestas reales `200`, `201`, `204`, `400`, `401`, `403`, `404` y `409`. En la captura del JWT decodificado muestra solo `sub`, `email`, `rol` y expiración; no publiques el token completo. El archivo `.http` contiene solicitudes repetibles y marcadores para las cuentas locales.

## Observaciones y conclusiones

La autenticación se agregó respetando las capas existentes: `Datos` busca y persiste usuarios, `Negocio` verifica claves y crea JWT, y `Api` valida el token y aplica permisos. Los DTOs de registro y login validan correo y clave. La relación Cliente–OrdenServicio y las reglas de negocio previas se conservan.

Los roles reducen el acceso accidental: Cliente consulta su perfil; Recepcionista gestiona clientes y registra trabajos; Mecanico consulta y actualiza su trabajo. Las comprobaciones de 401 y 403 deben mostrarse juntas para distinguir identidad de permiso. La expiración obliga a iniciar sesión otra vez cuando el token vence.

El repositorio de entrega indicado es [Lab13DesAEA](https://github.com/carodev02/Lab13DesAEA); contiene el script SQL dentro de este proyecto. Las claves JWT y la cadena de SQL Server son configuración local y no se guardan en el repositorio.
