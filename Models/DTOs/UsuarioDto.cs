using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ContabilidaMarket.Models.DTOs
{
    public class UsuarioDto
    {
        [Required(ErrorMessage = "El Nombre es requerido")]
        [StringLength(20, ErrorMessage = "El nombre no debe tener mas de 30 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El Apellido es requerido")]
        [StringLength(30, ErrorMessage = "El nombre no debe tener mas de 30 caracteres")]

        public string Apellido { get; set; }

        [Required]
        //\d: Representa cualquier dígito (del 0 al 9).
        [RegularExpression(@"^\d+$", ErrorMessage = "solo se aceptan numeros")]
        [StringLength(9, ErrorMessage = "El telefono no debe tener mas de 9 digitos")]

        public string Telefono { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "El nombre no debe tener mas de 30 caracteres")]

        public string Password { get; set; }

        [Required]
        [ForeignKey(nameof(Rol))]
        public int rol_id { get; set; }

    }
}
