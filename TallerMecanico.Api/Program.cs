using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TallerMecanico.Api;
using TallerMecanico.Datos;
using TallerMecanico.Negocio;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TallerMecanico")
    ?? Environment.GetEnvironmentVariable("TALLERMECANICO_CONNECTION")
    ?? throw new InvalidOperationException("Configura ConnectionStrings:TallerMecanico mediante User Secrets o TALLERMECANICO_CONNECTION.");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"]
    ?? throw new InvalidOperationException("Configura Jwt:Key con User Secrets o la variable JWT__KEY.");
var jwtIssuer = jwtSection["Issuer"]
    ?? throw new InvalidOperationException("Configura Jwt:Issuer.");
var jwtAudience = jwtSection["Audience"]
    ?? throw new InvalidOperationException("Configura Jwt:Audience.");
if (Encoding.UTF8.GetByteCount(jwtKey) < 32)
    throw new InvalidOperationException("Jwt:Key debe tener al menos 32 bytes UTF-8.");
if (!int.TryParse(jwtSection["ExpirationMinutes"], out var jwtMinutes) || jwtMinutes < 1)
    throw new InvalidOperationException("Configura Jwt:ExpirationMinutes con un valor entero positivo.");

var jwt = new JwtConfiguracion(jwtKey, jwtIssuer, jwtAudience, jwtMinutes);
builder.Services.AddSingleton(jwt);
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Pega solo el token obtenido en POST /api/auth/login."
    });
    options.OperationFilter<JwtSecurityOperationFilter>();
});
builder.Services.AddDbContext<TallerMecanicoContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<IOrdenServicioRepositorio, OrdenServicioRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IClienteServicio, ClienteServicio>();
builder.Services.AddScoped<IOrdenServicioServicio, OrdenServicioServicio>();
builder.Services.AddScoped<IAuthServicio, AuthServicio>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Emisor,
            ValidateAudience = true,
            ValidAudience = jwt.Audiencia,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Clave)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            NameClaimType = JwtRegisteredClaimNames.Email,
            RoleClaimType = "rol"
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    // Mantiene la documentación contraída al abrirla para que el navegador
    // no tenga que renderizar todos los esquemas de respuesta de inmediato.
    options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    options.DisplayRequestDuration();
});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }
