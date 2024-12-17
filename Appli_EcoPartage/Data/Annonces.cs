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

        public required int Points { get; set; }
        public required DateTime Date { get; set; }
        public bool Active { get; set; } //Anononce active ou archivée

        [ForeignKey("User")]
        public int IdUser { get; set; }

        public virtual Users? User { get; set; }

        public required virtual ICollection<AnnoncesTags> AnnoncesTags { get; set; } = new List<AnnoncesTags>();

        public required virtual ICollection<AnnoncesGeoSector> AnnoncesGeoSectors { get; set; } = new List<AnnoncesGeoSector>();

        internal object Include(Func<object, object> value)
        {
            throw new NotImplementedException();
        }
    }

}
