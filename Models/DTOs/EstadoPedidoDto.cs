using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContabilidaMarket.Models.DTOs
{
    public class EstadoPedidoDto
    {

      
        public int IdEstado { get; set; }

        [MaxLength(20)]
        [Required(ErrorMessage = "El Nombre es requerido")]
        public string Estado { get; set; }
    }
}
