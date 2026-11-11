using TallerMecanico.Entidades;

namespace TallerMecanico.Datos;

public interface IClienteRepositorio
{
    Task<List<Cliente>> ListarAsync(CancellationToken ct = default);
    Task<Cliente?> ObtenerAsync(int id, CancellationToken ct = default);
    Task<Cliente?> ObtenerConOrdenesAsync(int id, CancellationToken ct = default);
    Task AgregarAsync(Cliente cliente, CancellationToken ct = default);
    void Eliminar(Cliente cliente);
    Task GuardarAsync(CancellationToken ct = default);
}
