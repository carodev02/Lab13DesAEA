using System.ComponentModel.DataAnnotations;

namespace TallerMecanico.Entidades;

public sealed class ClienteLecturaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
}

public sealed class ClienteCrearDto
{
    [Required, StringLength(100, MinimumLength = 2)]
    public string Nombre { get; set; } = string.Empty;
    [Required, StringLength(20, MinimumLength = 7)]
    public string Telefono { get; set; } = string.Empty;
}

public sealed class ClienteActualizarDto
{
    [Required, StringLength(100, MinimumLength = 2)]
    public string Nombre { get; set; } = string.Empty;
    [Required, StringLength(20, MinimumLength = 7)]
    public string Telefono { get; set; } = string.Empty;
}
