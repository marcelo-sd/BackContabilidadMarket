using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ContabilidaMarket.Models
{
    public class PedidoProducto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey(nameof(Pedido))]
        public int IdPedido { get; set; }

        [JsonIgnore]
        public virtual Pedido Pedido { get; set; }

        [Required]
        [ForeignKey(nameof(Producto))]
        public int IdProducto { get; set; }

        public int Cantidad { get; set; }

        [JsonIgnore]
        public virtual Producto Producto { get; set; }

    }
}
