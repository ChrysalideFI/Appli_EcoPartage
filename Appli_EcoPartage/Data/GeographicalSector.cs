using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Appli_EcoPartage.Data
{
    public class GeographicalSector
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int IdGeographicalSector { get; set; }
        public required string Place { get; set; }
    }
}
