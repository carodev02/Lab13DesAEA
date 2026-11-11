using System.ComponentModel.DataAnnotations;

namespace TallerMecanico.Entidades;

public sealed class OrdenServicioLecturaDto
{
    public int Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string DescripcionTrabajo { get; set; } = string.Empty;
    public DateOnly FechaIngreso { get; set; }
    public string Estado { get; set; } = string.Empty;
    public int ClienteId { get; set; }
    public string NombreCliente { get; set; } = string.Empty;
}

public sealed class OrdenServicioCrearDto
{
    [Required, StringLength(10, MinimumLength = 5)]
    public string Placa { get; set; } = string.Empty;
    [Required, StringLength(300, MinimumLength = 5)]
    public string DescripcionTrabajo { get; set; } = string.Empty;
    [Required, Range(typeof(DateOnly), "2000-01-01", "2100-12-31")]
    public DateOnly FechaIngreso { get; set; }
    [Required, StringLength(20)]
    public string Estado { get; set; } = "Pendiente";
    [Range(1, int.MaxValue)]
    public int ClienteId { get; set; }
}

public sealed class OrdenServicioActualizarDto
{
    [Required, StringLength(10, MinimumLength = 5)]
    public string Placa { get; set; } = string.Empty;
    [Required, StringLength(300, MinimumLength = 5)]
    public string DescripcionTrabajo { get; set; } = string.Empty;
    [Required, Range(typeof(DateOnly), "2000-01-01", "2100-12-31")]
    public DateOnly FechaIngreso { get; set; }
    [Required, StringLength(20)]
    public string Estado { get; set; } = string.Empty;
    [Range(1, int.MaxValue)]
    public int ClienteId { get; set; }
}
