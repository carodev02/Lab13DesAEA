namespace TallerMecanico.Entidades;

public partial class OrdenServicio
{
    public int Id { get; set; }
    public string Placa { get; set; } = null!;
    public string DescripcionTrabajo { get; set; } = null!;
    public DateOnly FechaIngreso { get; set; }
    public string Estado { get; set; } = null!;
    public int ClienteId { get; set; }
    public virtual Cliente Cliente { get; set; } = null!;
}
