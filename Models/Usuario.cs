using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace ContabilidaMarket.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El Nombre es requerido")]
        [StringLength(30, ErrorMessage = "El nombre no debe tener mas de 30 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El Apellido es requerido")]
        [StringLength(30, ErrorMessage = "El nombre no debe tener mas de 30 caracteres")]
        public string Apellido { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        //\d: Representa cualquier dígito (del 0 al 9).
        [RegularExpression(@"^\d+$",ErrorMessage ="solo se aceptan numeros")]
        public string Telefono { get; set; }

        [Required]
        [ForeignKey(nameof(Rol))]
        public int Rol_id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime FechaRegistro { get; set; }= DateTime.Now;

        //esta representa la relacion con la tabla Roles
        [JsonIgnore]
       public virtual Rol Rol { get; set; }
    }
}
