using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Appli_EcoPartage.Data
{
    public class Comments
    {
        [Key]
        public int IdComment { get; set; }
        public DateTime Date { get; set; }
        public string? Notice { get; set; }
        [Required]
        [ForeignKey("Giver")]
        public int? IdUserGiven { get; set; }
        public virtual Users? Giver { get; set; }

        [ForeignKey("Recipient")]
        public int? IdUserRecipient { get; set; }
        public virtual Users? Recipient { get; set; }
    }
}
