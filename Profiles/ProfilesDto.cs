using AutoMapper;
using ContabilidaMarket.Models;
using ContabilidaMarket.Models.DTOs;

namespace ContabilidaMarket.Profiles
{
    public class ProfilesDto : Profile
    {
        public ProfilesDto()
        {
            CreateMap<Rol, RolDto>().ReverseMap();
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<EstadoPedido,EstadoPedidoDto>().ReverseMap();
            CreateMap<Producto, ProductoDto>().ReverseMap();
            CreateMap<Pedido,PedidosDto>().ReverseMap();
            CreateMap<Pedido,PedidoPostDto>().ReverseMap();
            CreateMap<PedidoProducto,PedidoProductoDto>().ReverseMap();

        }


    }
}
