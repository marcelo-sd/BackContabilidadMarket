using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContabilidaMarket.Models.DTOs
{
    public class RolPutDto
    {
        [Required]
        public int IdRol { get; set; }

        public string? RolNombre { get; set; }
    }
}
