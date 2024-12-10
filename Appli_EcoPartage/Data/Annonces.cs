using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Appli_EcoPartage.Data
{
    public class Annonces
    {
        [Key]
        public int IdAnnonce { get; set; }
        [Required]
        public required string Titre { get; set; }
        [Required]
        public required string Description { get; set; }
        public int Points { get; set; }
        public DateTime Date { get; set; }
        public bool Active { get; set; }

        [ForeignKey("User")]
        public int IdUser { get; set; }
        [Required]
        public required virtual Users User { get; set; }

        public required virtual ICollection<AnnoncesTags> AnnoncesTags { get; set; }
        public required virtual ICollection<GeographicalSector> GeographicalSectors { get; set; } 
    }
}
