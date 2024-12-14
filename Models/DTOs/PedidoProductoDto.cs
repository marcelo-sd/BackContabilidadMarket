using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ContabilidaMarket.Models.DTOs
{
    public class PedidoProductoDto
    {
    
        [Required]
        [JsonIgnore]
        public int IdPedido { get; set; }


        [Required]
        public int IdProducto { get; set; }

        public int Cantidad { get; set; }


    }
}
