using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ContabilidaMarket.Models
{
    public class Rol
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRol { get; set; }

        public string RolNombre { get; set; }


        [JsonIgnore]
       public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
