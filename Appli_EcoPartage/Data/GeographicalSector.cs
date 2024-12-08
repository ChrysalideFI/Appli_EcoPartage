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
        [ForeignKey("Annonce")]
        public int IdAnnonce { get; set; }
        public virtual Annonces? Annonce { get; set; }
        [Required]
        public int FirstPlace { get; set; } // Le premier lieu est requis, les autres sont optionnels
        public int SecondPlace { get; set; }
        public int ThirdPlace { get; set; }
    }
}
