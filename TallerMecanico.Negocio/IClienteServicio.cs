using TallerMecanico.Entidades;

namespace TallerMecanico.Negocio;

public interface IClienteServicio
{
    Task<IReadOnlyList<ClienteLecturaDto>> ListarAsync(CancellationToken ct = default);
    Task<ClienteLecturaDto?> ObtenerAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<OrdenServicioLecturaDto>?> OrdenesAsync(int id, CancellationToken ct = default);
    Task<ClienteLecturaDto> CrearAsync(ClienteCrearDto dto, CancellationToken ct = default);
    Task<bool> ActualizarAsync(int id, ClienteActualizarDto dto, CancellationToken ct = default);
    Task<bool> EliminarAsync(int id, CancellationToken ct = default);
}
