using System.ComponentModel.DataAnnotations;

namespace ContabilidaMarket.Models.DTOs
{
    public class ProductoPutDto
    {
        public string? Nombre { get; set; }
        [Required]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "El código de barras debe tener 12 dígitos")]
        public string CodigoBarra { get; set; }
        public string? Marca { get; set; }
        public double? Precioo { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "el stock se debe ingresar en numeros enteros")]
        public int? Stock { get; set; }
    }
}
