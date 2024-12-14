using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContabilidaMarket.Models
{
    public class EstadoPedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEstado { get; set; }

        public string Estado { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; }

    }
}
