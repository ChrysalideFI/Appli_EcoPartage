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
        public required virtual Users UserGiver { get; set; }

        [ForeignKey("UserRecipient")]
        public int UserIdRecipient { get; set; }
        public required virtual Users UserRecipient { get; set; }

        [ForeignKey("Annonce")]
        public int IdAnnonce { get; set; }
        public required virtual Annonces Annonce { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";

        [Required]
        public DateTime DateTransaction { get; set; }

    }
}
