using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Appli_EcoPartage.Data
{
    public class Transactions
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int IdTransaction { get; set; }

        [ForeignKey("UserGiver")]
        public int UserIdGiver { get; set; }
        public virtual Users? UserGiver { get; set; }

        [ForeignKey("UserRecipient")]
        public int UserIdRecipient { get; set; }
        public virtual Users? UserRecipient { get; set; }

        [ForeignKey("Annonce")]
        public int IdAnnonce { get; set; }
        public virtual Annonces? Annonce { get; set; }

        [ForeignKey("AnnoncePoints")]
        public int AnnoncePoint { get; set; }
        public virtual Annonces? AnnoncePoints { get; set; }

        [Required]
        public DateTime DateTransaction { get; set; }

    }
}
