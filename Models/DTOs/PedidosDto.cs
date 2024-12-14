using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ContabilidaMarket.Models.DTOs
{
    public class PedidosDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPedido { get; set; }


        [ForeignKey(nameof(Usuario))]
        public int idUsuario { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime fecha { get; set; }
        public int EstadoPedidoId { get; set; }
     
    }
}
