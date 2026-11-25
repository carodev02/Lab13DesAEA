namespace TallerMecanico.Negocio;

public sealed record JwtConfiguracion(string Clave, string Emisor, string Audiencia, int MinutosVida);
