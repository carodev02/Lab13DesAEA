using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Entidades;
using TallerMecanico.Negocio;

namespace TallerMecanico.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class OrdenesServicioController(IOrdenServicioServicio servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrdenServicioLecturaDto>>> Listar([FromQuery] string? estado, CancellationToken ct) => Ok(await servicio.ListarAsync(estado, ct));
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrdenServicioLecturaDto>> Obtener(int id, CancellationToken ct)
    {
        var dto = await servicio.ObtenerAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }
    [HttpPost]
    public async Task<ActionResult<OrdenServicioLecturaDto>> Crear(OrdenServicioCrearDto dto, CancellationToken ct)
    {
        try
        {
            var creado = await servicio.CrearAsync(dto, ct);
            return creado is null ? NotFound(new ProblemDetails { Title = "Cliente no encontrado", Status = 404 }) : CreatedAtAction(nameof(Obtener), new { id = creado.Id }, creado);
        }
        catch (ReglaNegocioException ex) { return Conflict(new ProblemDetails { Title = "Regla de negocio", Detail = ex.Message, Status = 409 }); }
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, OrdenServicioActualizarDto dto, CancellationToken ct)
    {
        try { return await servicio.ActualizarAsync(id, dto, ct) ? NoContent() : NotFound(); }
        catch (ReglaNegocioException ex) { return Conflict(new ProblemDetails { Title = "Regla de negocio", Detail = ex.Message, Status = 409 }); }
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id, CancellationToken ct) => await servicio.EliminarAsync(id, ct) ? NoContent() : NotFound();
}
