using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Appli_EcoPartage.Data
{
    public class AnnoncesTags
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int IdAnnonceTag { get; set; }
        [ForeignKey("Tag")]
        public int IdTag { get; set; }
        public required virtual Tags Tag { get; set; }
        [ForeignKey("Annonce")]
        public int IdAnnonce { get; set; }
        public required virtual Annonces Annonce { get; set; }
    }
}
