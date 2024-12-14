using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace ContabilidaMarket.Models
{
    public class Pedido
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPedido { get; set; }

        [ForeignKey(nameof(Usuario))]
        public int idUsuario { get; set; }

        [DataType(DataType.DateTime)] 
        public DateTime fecha { get; set; } = DateTime.UtcNow;


        [ForeignKey(nameof(EstadoPedido))]
        public int EstadoPedidoId { get; set; }


        [JsonIgnore] 
        public virtual Usuario Usuario { get; set; }
        [JsonIgnore] 
        public virtual EstadoPedido EstadoPedido { get; set; }
       // [JsonIgnore]
        public virtual ICollection<PedidoProducto> PedidoProductos { get; set; }
     
    }
}

