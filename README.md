# TallerMecanico

API REST para registrar clientes y órdenes de servicio de un taller mecánico.

Repositorio de entrega: [Lab13DesAEA](https://github.com/carodev02/Lab13DesAEA). El proyecto incluye el [script SQL](database/TallerMecanico.sql).

## Inicio rápido

1. Ejecuta `database/TallerMecanico.sql` en SQL Server LocalDB.
2. Configura `ConnectionStrings:TallerMecanico` con User Secrets o define `TALLERMECANICO_CONNECTION`. No guardes la cadena en el repositorio.
3. Abre `TallerMecanico.slnx` en Visual Studio y compila.
4. Ejecuta `TallerMecanico.Api` como proyecto de inicio. Swagger queda en `/swagger`.
5. Para repetir solicitudes, abre `TallerMecanico.Api/TallerMecanico.http` y envía los bloques en orden.

Comandos para configurar User Secrets desde la raíz:

```powershell
dotnet user-secrets init --project TallerMecanico.Api
dotnet user-secrets set "ConnectionStrings:TallerMecanico" "Server=(localdb)\MSSQLLocalDB;Database=TallerMecanico;Trusted_Connection=True;TrustServerCertificate=True" --project TallerMecanico.Api
```

La explicación del laboratorio, las reglas de negocio y el sustento de AutoMapper están en [docs/InformeLaboratorio.md](docs/InformeLaboratorio.md). Resultados de las pruebas ejecutadas: [evidencias/pruebas-http.txt](evidencias/pruebas-http.txt).
