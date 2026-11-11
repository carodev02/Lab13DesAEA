using Microsoft.EntityFrameworkCore;
using TallerMecanico.Entidades;

namespace TallerMecanico.Datos;

public sealed class ClienteRepositorio(TallerMecanicoContext db) : IClienteRepositorio
{
    public Task<List<Cliente>> ListarAsync(CancellationToken ct = default) => db.Clientes.AsNoTracking().OrderBy(c => c.Nombre).ToListAsync(ct);
    public Task<Cliente?> ObtenerAsync(int id, CancellationToken ct = default) => db.Clientes.FirstOrDefaultAsync(c => c.Id == id, ct);
    public Task<Cliente?> ObtenerConOrdenesAsync(int id, CancellationToken ct = default) => db.Clientes.Include(c => c.OrdenesServicio).FirstOrDefaultAsync(c => c.Id == id, ct);
    public async Task AgregarAsync(Cliente cliente, CancellationToken ct = default) => await db.Clientes.AddAsync(cliente, ct);
    public void Eliminar(Cliente cliente) => db.Clientes.Remove(cliente);
    public Task GuardarAsync(CancellationToken ct = default) => db.SaveChangesAsync(ct);
}
