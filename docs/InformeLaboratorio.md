# Informe del laboratorio 13

## Problema y modelo

TallerMecanico permite registrar los vehículos que ingresan al taller y dar seguimiento al trabajo solicitado. La entidad `Cliente` contiene nombre y teléfono. La entidad `OrdenServicio` registra placa, descripción del trabajo, fecha de ingreso, estado y `ClienteId`.

La relación es uno a muchos: un cliente puede tener varias órdenes y cada orden pertenece a un cliente. Así se conserva el historial de trabajos sin repetir los datos del cliente.

## Reglas de negocio

1. No se elimina un cliente que tenga órdenes registradas, para proteger el historial.
2. No se permite otra orden `Pendiente` para la misma placa.

Los servicios lanzan `ReglaNegocioException` cuando se incumple una regla; la API responde `409 Conflict`.

## Arquitectura

La solución se divide en `TallerMecanico.Entidades`, `TallerMecanico.Datos`, `TallerMecanico.Negocio` y `TallerMecanico.Api`, con referencias `Api → Negocio → Datos → Entidades`. Hay un repositorio y un servicio con interfaz para cada entidad. Sus métodos usan operaciones asíncronas y consultas LINQ. Los controladores dependen de interfaces de servicio y reciben o devuelven DTOs.

La base SQL Server se crea con `database/TallerMecanico.sql`, que incluye tres clientes y tres órdenes de ejemplo. El modelo se generó con EF Core Scaffold desde LocalDB y las entidades se ubicaron en Entidades. La clase scaffold `OrdenesServicio` se ajustó al nombre `OrdenServicio`. No se agregaron `[JsonIgnore]` a las entidades.

## Sustento de los mapeos

| Origen → destino | Motivo | Configuración especial |
|---|---|---|
| `Cliente → ClienteLecturaDto` | La entidad se convierte al formato público de lectura sin exponer el modelo persistente. | Ninguna; coinciden `Id`, `Nombre` y `Telefono`. |
| `ClienteCrearDto → Cliente` | La entrada validada se convierte en la entidad que se guarda. | Ninguna; los nombres coinciden. El servicio usa `Map<T>`. |
| `ClienteActualizarDto → Cliente` | La entrada se aplica a la entidad existente. | Ninguna; `Map(origen, destino)` conserva el `Id`. |
| `OrdenServicio → OrdenServicioLecturaDto` | La entidad se convierte al DTO de lectura de la API. | `ForMember` y `MapFrom` asignan `NombreCliente` desde `Cliente.Nombre`. |
| `OrdenServicioCrearDto → OrdenServicio` | La entrada validada se convierte en una entidad nueva. | Ninguna; los nombres coinciden. El servicio usa `Map<T>`. |
| `OrdenServicioActualizarDto → OrdenServicio` | La entrada se aplica a la entidad cargada para actualizarla. | Ninguna; `Map(origen, destino)` conserva el `Id`. |

No se utiliza `ReverseMap()`: lectura y entrada tienen propósitos y validaciones distintos, por lo que cada dirección se declara explícitamente.

## Endpoints y resultados

Hay seis rutas de clientes y cinco de órdenes, incluido `GET /api/clientes/{id}/ordenes` y `GET /api/ordenesservicio?estado=Pendiente`. El archivo `TallerMecanico.Api/TallerMecanico.http` permite repetir las solicitudes.

Se compilaron los cuatro proyectos con `dotnet build TallerMecanico.slnx --no-restore -m:1`: cero errores y cero advertencias. Se ejecutaron 19 solicitudes HTTP y se comprobaron `200`, `201` con `Location`, `204`, `400`, `404` y `409`. También se probó Swagger UI, el JSON OpenAPI y que exponga seis DTOs sin esquemas de entidades. El registro está en `evidencias/pruebas-http.txt`.

## Observaciones y conclusiones

La separación por capas mantiene los controladores centrados en HTTP, los repositorios en persistencia y los servicios en reglas de negocio. Los DTOs conservan un contrato público claro y evitan devolver entidades o ciclos de navegación. Las reglas protegen el historial del cliente y evitan órdenes pendientes duplicadas por placa.

En desarrollo, AutoMapper puede mostrar una advertencia sobre licencia; su uso está permitido para desarrollo y pruebas. El repositorio indicado es [Lab13DesAEA](https://github.com/carodev02/Lab13DesAEA), pero GitHub reporta que está vacío y la cuenta conectada no tiene permiso de escritura. El script está incluido en este workspace, pero falta subir el código al repositorio. También faltan capturas desde Visual Studio del Explorador de soluciones, el `MappingProfile` completo y las pruebas; los resultados ejecutados están en el archivo de evidencias.
