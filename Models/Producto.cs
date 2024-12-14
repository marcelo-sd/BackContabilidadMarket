using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ContabilidaMarket.Models
{
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProductos { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "El código de barras debe tener 12 dígitos")]
        public string CodigoBarra { get; set; }
        public double Precioo { get; set; }
        public string Marca { get; set; }
        public int Stock { get; set; }

        [JsonIgnore]
        public virtual ICollection<PedidoProducto> PedidoProductos { get; set; } // Relación con PedidoProducto
    }
}

