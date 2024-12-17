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

        public required int Points { get; set; } //Points que coûte le service
        public required DateTime Date { get; set; }
        public bool Active { get; set; } //Anononce active ou archivée

        //// Identifiant de l'utilisateur qui a créé l'annonce
        [ForeignKey("User")]
        public int IdUser { get; set; }

        public virtual Users? User { get; set; }

        // Collections des tags et sécteurs géographiques associés à l'annonce

        public required virtual ICollection<AnnoncesTags> AnnoncesTags { get; set; } = new List<AnnoncesTags>();

        public required virtual ICollection<AnnoncesGeoSector> AnnoncesGeoSectors { get; set; } = new List<AnnoncesGeoSector>();

        //Ajout de la méthode Include utiliser notamment pour inclure les filtres de la barre de recherche 
       
        internal object Include(Func<object, object> value)
        {
            throw new NotImplementedException();
        }
    }

}
