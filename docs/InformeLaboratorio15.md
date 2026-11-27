# Laboratorio 15: gateway y microservicios

## Tema y servicios

TallerMecanico conserva la API de clientes y órdenes de la semana 14. Añadí un servicio pequeño de repuestos para consultar piezas comunes del taller, como filtros de aceite, pastillas de freno y bujías. El catálogo usa datos en memoria y no comparte base de datos ni clases con la API principal.

El cliente envía sus solicitudes al único punto de entrada local, `GatewayApi`, en el puerto 5055. Ocelot las reenvía por HTTPS a `TallerMecanico.Api` en el puerto 7163 o por HTTP a `TallerMecanico.Repuestos.Api` en el 5056.

## Tabla de rutas locales

| Upstream (cliente → gateway) | Método | Downstream (gateway → servicio) | Token |
|---|---|---|---|
| `/gateway/auth/{everything}` | POST | `https://localhost:7163/api/auth/{everything}` | No |
| `/gateway/auth/{everything}` | GET | `https://localhost:7163/api/auth/{everything}` | No en Ocelot; el perfil de S14 requiere su JWT |
| `/gateway/clientes{everything}` | GET | `https://localhost:7163/api/clientes{everything}` | No en Ocelot; la API S14 sí lo exige y revisa el rol |
| `/gateway/clientes{everything}` | POST, PUT, DELETE | `https://localhost:7163/api/clientes{everything}` | Sí en Ocelot y la API S14 |
| `/gateway/ordenesservicio{everything}` | GET | `https://localhost:7163/api/ordenesservicio{everything}` | No en Ocelot; la API S14 sí lo exige y revisa el rol |
| `/gateway/ordenesservicio{everything}` | POST, PUT, DELETE | `https://localhost:7163/api/ordenesservicio{everything}` | Sí en Ocelot y la API S14 |
| `/gateway/repuestos{everything}` | GET | `http://localhost:5056/api/repuestos{everything}` | Sí en Ocelot y en Repuestos.Api |

`{everything}` permite usar una sola regla para la lista y los detalles. Por ejemplo, `GET /gateway/repuestos/2` llega a `GET /api/repuestos/2` en el servicio 2. El filtro de estado de órdenes también se conserva al reenviar la consulta.

La guía pide dejar públicos los GET en el gateway y también conservar intacta la API S14. En esta solución Ocelot deja pasar esos GET sin exigir token, pero la API S14 original aún requiere token y rol. Así no se cambia su lógica ni se debilita su autorización; para obtener una respuesta de lectura de S14, el cliente envía el JWT que esta API solicita.

## Autenticación

El gateway valida firma, emisor, audiencia y vencimiento en las rutas de escritura y en las rutas de Repuestos.Api. La API S14 mantiene su propia validación y aplica sus roles; Repuestos.Api exige un token en sus dos GET. Las tres aplicaciones leen la misma clave JWT local desde User Secrets o `JWT__KEY`. La clave no se incluye en el repositorio.

Validar también en cada servicio protege sus endpoints si alguien intenta saltarse el gateway y llamar directamente al puerto interno. La contrapartida es repetir configuración y mantener iguales la clave, el emisor y la audiencia.

## Preguntas de sustentación

**¿Qué cambia para el cliente al usar el gateway?** El cliente usa una dirección y un prefijo común, sin necesitar conocer los puertos ni las rutas internas de cada servicio. El gateway selecciona el servicio y conserva el contrato público definido en `ocelot.json`.

**¿Cómo se traduce una ruta?** En `GET /gateway/repuestos/2`, `/gateway/repuestos/2` es el upstream que conoce el cliente. Ocelot lo convierte en `/api/repuestos/2` y lo envía a `http://localhost:5056`, el downstream. Cambian el prefijo y el destino; el identificador se conserva gracias a `{everything}`.

**¿Dónde se valida el token y por qué dos veces?** Ocelot comprueba el token antes de reenviar las rutas protegidas. Además, cada servicio protegido lo vuelve a comprobar; la API S14 también verifica el rol. La verificación doble evita que eludir el gateway permita entrar directamente al servicio, aunque obliga a sincronizar la configuración de seguridad.

**¿Qué ocurre cuando Repuestos.Api está detenido?** Ocelot no puede conectar con ese downstream y responde `502 Bad Gateway` para las rutas de repuestos. Las rutas de la API S14 pueden seguir funcionando porque usan otro servicio. La prueba local se debe realizar deteniendo Repuestos.Api y enviando la solicitud por el gateway.

Una dificultad observada en una arquitectura distribuida es que cada servicio puede fallar o configurarse de forma independiente. También hay más direcciones, procesos y parámetros de autenticación que mantener que en una sola API.

## Pruebas y entregables

Se ejecutaron 19 solicitudes locales contra el gateway. Se verificaron `200`, `201` con `Location` reescrito al gateway, `204`, `401`, `403`, `404` y `502`. También se llamó directamente a Repuestos.Api sin token y respondió `401`. El detalle está en `evidencias/pruebas-lab15.txt`; las solicitudes se pueden repetir desde `GatewayApi/TallerMecanico.Lab15.http`.

El archivo Ocelot completo es `GatewayApi/ocelot.json`; sus direcciones corresponden al entorno local. Para crear cuentas de prueba, usa el registro por `/gateway/auth/registro`; el registro asigna el rol Cliente. Si necesitas una cuenta de Recepcionista o Mecanico, cambia su rol en la base local y vuelve a iniciar sesión para recibir un token actualizado. La consulta de roles está en `docs/InformeLaboratorio14.md`.

Las capturas de pantalla de Visual Studio no se simularon. Para completar esa parte local, toma una captura del `ocelot.json` completo, otra donde la salida muestre los tres puertos en ejecución y capturas de las respuestas HTTP. La publicación en la nube y el video quedan fuera del alcance indicado.

Repositorio: [Lab13DesAEA](https://github.com/carodev02/Lab13DesAEA).

Esta entrega cubre la solución y las pruebas en el equipo local. La guía también solicita publicación en la nube, pruebas sobre direcciones públicas y un video de sustentación; esos puntos quedan fuera del alcance indicado para esta entrega y no se presentan como realizados.
