using Microsoft.EntityFrameworkCore;
using TallerMecanico.Entidades;

namespace TallerMecanico.Datos;

public sealed class OrdenServicioRepositorio(TallerMecanicoContext db) : IOrdenServicioRepositorio
{
    public Task<List<OrdenServicio>> ListarAsync(string? estado, CancellationToken ct = default) =>
        db.OrdenesServicio.AsNoTracking().Include(o => o.Cliente).Where(o => estado == null || o.Estado == estado).OrderByDescending(o => o.FechaIngreso).ToListAsync(ct);
    public Task<List<OrdenServicio>> PorClienteAsync(int clienteId, CancellationToken ct = default) =>
        db.OrdenesServicio.AsNoTracking().Include(o => o.Cliente).Where(o => o.ClienteId == clienteId).OrderByDescending(o => o.FechaIngreso).ToListAsync(ct);
    public Task<OrdenServicio?> ObtenerAsync(int id, CancellationToken ct = default) => db.OrdenesServicio.Include(o => o.Cliente).FirstOrDefaultAsync(o => o.Id == id, ct);
    public Task<bool> ExistePendienteParaPlacaAsync(string placa, int? excluirId = null, CancellationToken ct = default) =>
        db.OrdenesServicio.AnyAsync(o => o.Placa == placa && o.Estado == "Pendiente" && (!excluirId.HasValue || o.Id != excluirId.Value), ct);
    public async Task AgregarAsync(OrdenServicio orden, CancellationToken ct = default) => await db.OrdenesServicio.AddAsync(orden, ct);
    public void Eliminar(OrdenServicio orden) => db.OrdenesServicio.Remove(orden);
    public Task GuardarAsync(CancellationToken ct = default) => db.SaveChangesAsync(ct);
}
