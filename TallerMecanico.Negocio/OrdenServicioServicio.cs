using AutoMapper;
using TallerMecanico.Datos;
using TallerMecanico.Entidades;

namespace TallerMecanico.Negocio;

public sealed class OrdenServicioServicio(IOrdenServicioRepositorio repo, IClienteRepositorio clientes, IMapper mapper) : IOrdenServicioServicio
{
    public async Task<IReadOnlyList<OrdenServicioLecturaDto>> ListarAsync(string? estado, CancellationToken ct = default) => mapper.Map<List<OrdenServicioLecturaDto>>(await repo.ListarAsync(estado, ct));
    public async Task<OrdenServicioLecturaDto?> ObtenerAsync(int id, CancellationToken ct = default) => mapper.Map<OrdenServicioLecturaDto?>(await repo.ObtenerAsync(id, ct));
    public async Task<OrdenServicioLecturaDto?> CrearAsync(OrdenServicioCrearDto dto, CancellationToken ct = default)
    {
        if (await clientes.ObtenerAsync(dto.ClienteId, ct) is null) return null;
        if (dto.Estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase) && await repo.ExistePendienteParaPlacaAsync(dto.Placa, ct: ct))
            throw new ReglaNegocioException($"Ya existe una orden pendiente para la placa {dto.Placa}.");
        var entity = mapper.Map<OrdenServicio>(dto);
        await repo.AgregarAsync(entity, ct);
        await repo.GuardarAsync(ct);
        var guardada = await repo.ObtenerAsync(entity.Id, ct);
        return mapper.Map<OrdenServicioLecturaDto>(guardada);
    }
    public async Task<bool> ActualizarAsync(int id, OrdenServicioActualizarDto dto, CancellationToken ct = default)
    {
        var entity = await repo.ObtenerAsync(id, ct);
        if (entity is null) return false;
        if (await clientes.ObtenerAsync(dto.ClienteId, ct) is null) return false;
        if (dto.Estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase) && await repo.ExistePendienteParaPlacaAsync(dto.Placa, id, ct))
            throw new ReglaNegocioException($"Ya existe otra orden pendiente para la placa {dto.Placa}.");
        mapper.Map(dto, entity);
        await repo.GuardarAsync(ct);
        return true;
    }
    public async Task<bool> EliminarAsync(int id, CancellationToken ct = default)
    {
        var entity = await repo.ObtenerAsync(id, ct);
        if (entity is null) return false;
        repo.Eliminar(entity);
        await repo.GuardarAsync(ct);
        return true;
    }
}
