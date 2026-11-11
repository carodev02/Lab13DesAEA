using AutoMapper;
using TallerMecanico.Entidades;

namespace TallerMecanico.Negocio;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Cliente, ClienteLecturaDto>();
        CreateMap<ClienteCrearDto, Cliente>();
        CreateMap<ClienteActualizarDto, Cliente>();
        CreateMap<OrdenServicio, OrdenServicioLecturaDto>()
            .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.Cliente.Nombre));
        CreateMap<OrdenServicioCrearDto, OrdenServicio>();
        CreateMap<OrdenServicioActualizarDto, OrdenServicio>();
    }
}
