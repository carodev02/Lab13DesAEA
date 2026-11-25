using Microsoft.EntityFrameworkCore;
using TallerMecanico.Entidades;

namespace TallerMecanico.Datos;

public sealed class UsuarioRepositorio(TallerMecanicoContext db) : IUsuarioRepositorio
{
    public Task<Usuario?> ObtenerPorCorreoAsync(string correo, CancellationToken ct = default) =>
        db.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo, ct);

    public async Task AgregarAsync(Usuario usuario, CancellationToken ct = default) =>
        await db.Usuarios.AddAsync(usuario, ct);

    public Task GuardarAsync(CancellationToken ct = default) => db.SaveChangesAsync(ct);
}
