using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCryptHelper = BCrypt.Net.BCrypt;
using Microsoft.IdentityModel.Tokens;
using TallerMecanico.Datos;
using TallerMecanico.Entidades;

namespace TallerMecanico.Negocio;

public sealed class AuthServicio(IUsuarioRepositorio usuarios, JwtConfiguracion jwt) : IAuthServicio
{
    private const string RolPredeterminado = "Cliente";

    public async Task<RegistroRespuestaDto> RegistrarAsync(RegistroDto dto, CancellationToken ct = default)
    {
        var correo = NormalizarCorreo(dto.Correo);
        if (await usuarios.ObtenerPorCorreoAsync(correo, ct) is not null)
            throw new ReglaNegocioException("Ya existe una cuenta registrada con ese correo.");

        var usuario = new Usuario
        {
            Correo = correo,
            ClaveHash = BCryptHelper.HashPassword(dto.Clave, workFactor: 12),
            // El rol se asigna en el servidor. Nunca se acepta desde RegistroDto.
            Rol = RolPredeterminado
        };

        await usuarios.AgregarAsync(usuario, ct);
        await usuarios.GuardarAsync(ct);

        return new RegistroRespuestaDto { Id = usuario.Id, Correo = usuario.Correo, Rol = usuario.Rol };
    }

    public async Task<TokenDto?> IniciarSesionAsync(LoginDto dto, CancellationToken ct = default)
    {
        var correo = NormalizarCorreo(dto.Correo);
        var usuario = await usuarios.ObtenerPorCorreoAsync(correo, ct);

        // La misma respuesta cubre correo inexistente y clave incorrecta.
        if (usuario is null || !BCryptHelper.Verify(dto.Clave, usuario.ClaveHash))
            return null;

        var expiraEnUtc = DateTime.UtcNow.AddMinutes(jwt.MinutosVida);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
            new Claim("rol", usuario.Rol)
        };
        var credenciales = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Clave)),
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: jwt.Emisor,
            audience: jwt.Audiencia,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiraEnUtc,
            signingCredentials: credenciales);

        return new TokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiraEnUtc = expiraEnUtc
        };
    }

    private static string NormalizarCorreo(string correo) => correo.Trim().ToLowerInvariant();
}
