# TallerMecanico

El proyecto registra clientes, órdenes de servicio y repuestos de un taller. El acceso se realiza por un gateway local, que dirige cada solicitud al servicio correspondiente.

## Ejecutar en Visual Studio

1. Ejecuta el script `database/TallerMecanico.sql` en SQL Server y configura la conexión local para `TallerMecanico.Api`.
2. Guarda la misma clave JWT (mínimo 32 bytes) en User Secrets de `TallerMecanico.Api`, `GatewayApi` y `TallerMecanico.Repuestos.Api`. No pongas claves ni cadenas de conexión en Git.
3. En Visual Studio configura los tres como proyectos de inicio: `TallerMecanico.Api` (HTTPS 7163), `GatewayApi` (5055) y `TallerMecanico.Repuestos.Api` (5056).
4. Envía las solicitudes de `GatewayApi/TallerMecanico.Lab15.http` al gateway local.

Los informes están en `docs/InformeLaboratorio14.md` y `docs/InformeLaboratorio15.md`. La entrega cubre el trabajo local; no incluye publicación en la nube.

Repositorio: [Lab13DesAEA](https://github.com/carodev02/Lab13DesAEA).
