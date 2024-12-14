using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Appli_EcoPartage.Data
{
    public class AnnoncesGeoSector
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int IdAnnoncesGeoSector { get; set; }
        [ForeignKey("GeographicalSector")]
        public int IdGeographicalSector { get; set; }
        public required virtual GeographicalSector GeographicalSector { get; set; }
        [ForeignKey("Annonce")]
        public int IdAnnonce { get; set; }
        public required virtual Annonces Annonce { get; set; }
    }
}
