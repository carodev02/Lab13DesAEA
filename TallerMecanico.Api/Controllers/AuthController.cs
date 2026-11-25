using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Entidades;
using TallerMecanico.Negocio;

namespace TallerMecanico.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IAuthServicio servicio) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("registro")]
    public async Task<ActionResult<RegistroRespuestaDto>> Registrar(RegistroDto dto, CancellationToken ct)
    {
        try
        {
            var creado = await servicio.RegistrarAsync(dto, ct);
            return StatusCode(StatusCodes.Status201Created, creado);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new ProblemDetails { Title = "Correo ya registrado", Detail = ex.Message, Status = 409 });
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> IniciarSesion(LoginDto dto, CancellationToken ct)
    {
        var respuesta = await servicio.IniciarSesionAsync(dto, ct);
        return respuesta is null
            ? Unauthorized(new ProblemDetails { Title = "Credenciales inválidas", Status = 401 })
            : Ok(respuesta);
    }

    [Authorize]
    [HttpGet("perfil")]
    public ActionResult<PerfilDto> Perfil()
    {
        // Se leen los valores del JWT validado; no se hace otra consulta a la base.
        var correo = User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
        var rol = User.FindFirst("rol")?.Value;
        if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(rol))
            return Unauthorized();

        return Ok(new PerfilDto { Correo = correo, Rol = rol });
    }
}
