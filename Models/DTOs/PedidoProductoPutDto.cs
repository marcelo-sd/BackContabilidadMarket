using System.ComponentModel.DataAnnotations.Schema;

namespace ContabilidaMarket.Models.DTOs
{
    public class PedidoProductoPutDto
    {

        public int Id { get; set; } 
        [ForeignKey(nameof(Pedido))]
        public int? IdPedido { get; set; }

 

        [ForeignKey(nameof(Producto))]
        public int? IdProducto { get; set; }


    }
}
