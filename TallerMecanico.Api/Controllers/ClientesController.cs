using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Entidades;
using TallerMecanico.Negocio;

namespace TallerMecanico.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class ClientesController(IClienteServicio servicio) : ControllerBase
{
    [Authorize(Roles = "Recepcionista,Mecanico")]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ClienteLecturaDto>>> Listar(CancellationToken ct) => Ok(await servicio.ListarAsync(ct));

    [Authorize(Roles = "Recepcionista,Mecanico")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteLecturaDto>> Obtener(int id, CancellationToken ct)
    {
        var dto = await servicio.ObtenerAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }
    [Authorize(Roles = "Recepcionista,Mecanico")]
    [HttpGet("{id:int}/ordenes")]
    public async Task<ActionResult<IReadOnlyList<OrdenServicioLecturaDto>>> Ordenes(int id, CancellationToken ct)
    {
        var dto = await servicio.OrdenesAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }
    [Authorize(Roles = "Recepcionista")]
    [HttpPost]
    public async Task<ActionResult<ClienteLecturaDto>> Crear(ClienteCrearDto dto, CancellationToken ct)
    {
        var creado = await servicio.CrearAsync(dto, ct);
        return CreatedAtAction(nameof(Obtener), new { id = creado.Id }, creado);
    }
    [Authorize(Roles = "Recepcionista")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, ClienteActualizarDto dto, CancellationToken ct) => await servicio.ActualizarAsync(id, dto, ct) ? NoContent() : NotFound();
    [Authorize(Roles = "Recepcionista")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
    {
        try { return await servicio.EliminarAsync(id, ct) ? NoContent() : NotFound(); }
        catch (ReglaNegocioException ex) { return Conflict(new ProblemDetails { Title = "Regla de negocio", Detail = ex.Message, Status = 409 }); }
    }
}
