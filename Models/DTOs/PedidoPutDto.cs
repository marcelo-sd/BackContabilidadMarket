using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ContabilidaMarket.Models.DTOs
{
    public class PedidoPutDto
    {
        [Key]
      
        public int IdPedido { get; set; }


        [ForeignKey(nameof(Usuario))]
        public int? idUsuario { get; set; }


        [ForeignKey(nameof(EstadoPedido))]
        public int? EstadoPedidoId { get; set; }
    }
}
