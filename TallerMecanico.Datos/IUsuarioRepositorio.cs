using TallerMecanico.Entidades;

namespace TallerMecanico.Datos;

public interface IUsuarioRepositorio
{
    Task<Usuario?> ObtenerPorCorreoAsync(string correo, CancellationToken ct = default);
    Task AgregarAsync(Usuario usuario, CancellationToken ct = default);
    Task GuardarAsync(CancellationToken ct = default);
}
