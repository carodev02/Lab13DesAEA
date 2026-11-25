# TallerMecanico

API REST de taller mecánico para clientes, órdenes de servicio y acceso con JWT.

## Ejecutar

1. Ejecuta `database/TallerMecanico.sql` en SQL Server LocalDB.
2. En PowerShell, desde la raíz, guarda tu cadena de conexión y una clave JWT en User Secrets. Sustituye el marcador por la conexión de tu SQL Server:

```powershell
dotnet user-secrets set "ConnectionStrings:TallerMecanico" "<tu-cadena-de-conexion-local>" --project .\TallerMecanico.Api
$bytes = New-Object byte[] 48
[Security.Cryptography.RandomNumberGenerator]::Fill($bytes)
$key = [Convert]::ToBase64String($bytes)
dotnet user-secrets set "Jwt:Key" $key --project .\TallerMecanico.Api
dotnet user-secrets set "Jwt:ExpirationMinutes" "30" --project .\TallerMecanico.Api
$key = $null; $bytes = $null
```

3. Abre `TallerMecanico.slnx` en Visual Studio, compila y ejecuta `TallerMecanico.Api`.
4. Abre `/swagger` en el navegador. Inicia sesión en `/api/auth/login`, copia el token y pulsa **Authorize** para probar rutas protegidas.

El proyecto ya tiene `UserSecretsId`; no vuelvas a ejecutar `user-secrets init`.
No guardes en Git claves JWT, contraseñas, tokens ni cadenas de conexión.
Para probar cada rol, registra tres cuentas localmente desde Swagger y cambia dos roles con el `UPDATE` del script SQL. El registro público solo asigna el rol `Cliente`.

## Pruebas y entrega

- Solicitudes repetibles: [TallerMecanico.Api/TallerMecanico.http](TallerMecanico.Api/TallerMecanico.http). Los pasos para configurar cuentas locales están en [el informe del laboratorio 14](docs/InformeLaboratorio14.md).
- Registro de pruebas ejecutadas: [evidencias/pruebas-lab14.txt](evidencias/pruebas-lab14.txt).
- Script SQL: [database/TallerMecanico.sql](database/TallerMecanico.sql).
- Repositorio indicado: [Lab13DesAEA](https://github.com/carodev02/Lab13DesAEA).
- Informe anterior: [Laboratorio 13](docs/InformeLaboratorio.md).
