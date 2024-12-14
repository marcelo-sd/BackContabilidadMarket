using ContabilidaMarket.Context;
using ContabilidaMarket.Models.DTOs;

namespace ContabilidaMarket.clasesUtiles
{
    public class ModeloVenta
    {
       
        public int idUsuario { get; set; }

        
        public int EstadoPedidoId { get; set; }
        public List<PedidoProductoDto> ListaProductosEnElPedido { get; set; }

    }
}
