using TallerMecanico.Entidades;

namespace TallerMecanico.Negocio;

public interface IAuthServicio
{
    Task<RegistroRespuestaDto> RegistrarAsync(RegistroDto dto, CancellationToken ct = default);
    Task<TokenDto?> IniciarSesionAsync(LoginDto dto, CancellationToken ct = default);
}
