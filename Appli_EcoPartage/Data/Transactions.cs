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

        [ForeignKey("UserSeller")] //Utilisateur qui donne le service
        public int UserIdSeller { get; set; }
        public required virtual Users UserSeller { get; set; }

        [ForeignKey("UserBuyer")] //Utilisateur qui reçoit le service
        public int UserIdBuyer{ get; set; }
        public required virtual Users UserBuyer { get; set; }

        [ForeignKey("Annonce")]
        public int IdAnnonce { get; set; }
        public required virtual Annonces Annonce { get; set; }

        [Required]
        public string Status { get; set; } = "Pending"; //Etat de la transaction : Pending, Accepted, Refused

        [Required]
        public DateTime DateTransaction { get; set; }

    }
}
