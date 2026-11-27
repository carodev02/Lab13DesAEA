using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var jwt = builder.Configuration.GetSection("Jwt");
var jwtKey = jwt["Key"]
    ?? throw new InvalidOperationException("Configura Jwt:Key mediante User Secrets o la variable JWT__KEY.");
var issuer = jwt["Issuer"]
    ?? throw new InvalidOperationException("Configura Jwt:Issuer.");
var audience = jwt["Audience"]
    ?? throw new InvalidOperationException("Configura Jwt:Audience.");

if (Encoding.UTF8.GetByteCount(jwtKey) < 32)
    throw new InvalidOperationException("Jwt:Key debe tener al menos 32 bytes UTF-8.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            NameClaimType = JwtRegisteredClaimNames.Email,
            RoleClaimType = "rol"
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

var repuestos = new[]
{
    new Repuesto(1, "Filtro de aceite", "Bosch", 12, 38.50m),
    new Repuesto(2, "Pastillas de freno", "Brembo", 8, 145.00m),
    new Repuesto(3, "Bujía", "NGK", 20, 24.90m)
};

app.MapGet("/api/repuestos", () => Results.Ok(repuestos))
    .RequireAuthorization();

app.MapGet("/api/repuestos/{id:int}", (int id) =>
{
    var repuesto = repuestos.FirstOrDefault(item => item.Id == id);
    return repuesto is null ? Results.NotFound() : Results.Ok(repuesto);
}).RequireAuthorization();

app.Run();

public sealed record Repuesto(int Id, string Nombre, string Marca, int Stock, decimal Precio);
public partial class Program { }
