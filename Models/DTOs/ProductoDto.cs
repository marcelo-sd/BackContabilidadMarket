using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ContabilidaMarket.Models.DTOs
{
    public class ProductoDto
    {
       
        [Required(ErrorMessage ="El nombre es requerido")]
        public string Nombre { get; set; }
        [Required]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "El código de barras debe tener 12 dígitos")]
        public string CodigoBarra { get; set; }
        public string Marca { get; set; }
        [Required]
        public double Precioo { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]+$",ErrorMessage ="el stock se debe ingresar en numeros enteros de 0 a 9")]
        public int Stock { get; set; }
    }
}
