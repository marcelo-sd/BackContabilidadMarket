using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContabilidaMarket.Models.DTOs
{
    public class PedidoPostDto
    {

       // [Required]
        public int idUsuario { get; set; }

    //    [Required]
        public int EstadoPedidoId { get; set; }
    }
}
