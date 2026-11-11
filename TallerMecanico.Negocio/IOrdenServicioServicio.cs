using TallerMecanico.Entidades;

namespace TallerMecanico.Negocio;

public interface IOrdenServicioServicio
{
    Task<IReadOnlyList<OrdenServicioLecturaDto>> ListarAsync(string? estado, CancellationToken ct = default);
    Task<OrdenServicioLecturaDto?> ObtenerAsync(int id, CancellationToken ct = default);
    Task<OrdenServicioLecturaDto?> CrearAsync(OrdenServicioCrearDto dto, CancellationToken ct = default);
    Task<bool> ActualizarAsync(int id, OrdenServicioActualizarDto dto, CancellationToken ct = default);
    Task<bool> EliminarAsync(int id, CancellationToken ct = default);
}
