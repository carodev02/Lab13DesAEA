using AutoMapper;
using TallerMecanico.Datos;
using TallerMecanico.Entidades;

namespace TallerMecanico.Negocio;

public sealed class ClienteServicio(IClienteRepositorio repo, IMapper mapper) : IClienteServicio
{
    public async Task<IReadOnlyList<ClienteLecturaDto>> ListarAsync(CancellationToken ct = default) => mapper.Map<List<ClienteLecturaDto>>(await repo.ListarAsync(ct));
    public async Task<ClienteLecturaDto?> ObtenerAsync(int id, CancellationToken ct = default) => mapper.Map<ClienteLecturaDto?>(await repo.ObtenerAsync(id, ct));
    public async Task<IReadOnlyList<OrdenServicioLecturaDto>?> OrdenesAsync(int id, CancellationToken ct = default)
    {
        var cliente = await repo.ObtenerConOrdenesAsync(id, ct);
        return cliente is null ? null : mapper.Map<List<OrdenServicioLecturaDto>>(cliente.OrdenesServicio);
    }
    public async Task<ClienteLecturaDto> CrearAsync(ClienteCrearDto dto, CancellationToken ct = default)
    {
        var entity = mapper.Map<Cliente>(dto);
        await repo.AgregarAsync(entity, ct);
        await repo.GuardarAsync(ct);
        return mapper.Map<ClienteLecturaDto>(entity);
    }
    public async Task<bool> ActualizarAsync(int id, ClienteActualizarDto dto, CancellationToken ct = default)
    {
        var entity = await repo.ObtenerAsync(id, ct);
        if (entity is null) return false;
        mapper.Map(dto, entity);
        await repo.GuardarAsync(ct);
        return true;
    }
    public async Task<bool> EliminarAsync(int id, CancellationToken ct = default)
    {
        var entity = await repo.ObtenerConOrdenesAsync(id, ct);
        if (entity is null) return false;
        if (entity.OrdenesServicio.Count != 0) throw new ReglaNegocioException("No se puede eliminar un cliente que tiene órdenes de servicio registradas.");
        repo.Eliminar(entity);
        await repo.GuardarAsync(ct);
        return true;
    }
}
