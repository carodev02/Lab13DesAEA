using Microsoft.EntityFrameworkCore;
using TallerMecanico.Datos;
using TallerMecanico.Negocio;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TallerMecanico")
    ?? Environment.GetEnvironmentVariable("TALLERMECANICO_CONNECTION")
    ?? throw new InvalidOperationException("Configura ConnectionStrings:TallerMecanico mediante User Secrets o TALLERMECANICO_CONNECTION.");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TallerMecanicoContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<IOrdenServicioRepositorio, OrdenServicioRepositorio>();
builder.Services.AddScoped<IClienteServicio, ClienteServicio>();
builder.Services.AddScoped<IOrdenServicioServicio, OrdenServicioServicio>();

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
app.MapControllers();
app.Run();

public partial class Program { }
