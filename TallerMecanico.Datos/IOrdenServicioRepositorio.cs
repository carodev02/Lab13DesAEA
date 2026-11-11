using TallerMecanico.Entidades;

namespace TallerMecanico.Datos;

public interface IOrdenServicioRepositorio
{
    Task<List<OrdenServicio>> ListarAsync(string? estado, CancellationToken ct = default);
    Task<List<OrdenServicio>> PorClienteAsync(int clienteId, CancellationToken ct = default);
    Task<OrdenServicio?> ObtenerAsync(int id, CancellationToken ct = default);
    Task<bool> ExistePendienteParaPlacaAsync(string placa, int? excluirId = null, CancellationToken ct = default);
    Task AgregarAsync(OrdenServicio orden, CancellationToken ct = default);
    void Eliminar(OrdenServicio orden);
    Task GuardarAsync(CancellationToken ct = default);
}
