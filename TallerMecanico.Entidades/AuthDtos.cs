using System.ComponentModel.DataAnnotations;

namespace TallerMecanico.Entidades;

public sealed class RegistroDto
{
    [Required, EmailAddress, StringLength(254)]
    public string Correo { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 10)]
    public string Clave { get; set; } = string.Empty;
}

public sealed class LoginDto
{
    [Required, EmailAddress, StringLength(254)]
    public string Correo { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 1)]
    public string Clave { get; set; } = string.Empty;
}

public sealed class TokenDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiraEnUtc { get; set; }
}

public sealed class PerfilDto
{
    public string Correo { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
}

public sealed class RegistroRespuestaDto
{
    public int Id { get; set; }
    public string Correo { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
}
