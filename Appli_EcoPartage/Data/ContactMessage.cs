using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Appli_EcoPartage.Data
{
    public class ContactMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime DateSent { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;

        // utilisateur qui a envoyé le message
        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual Users User { get; set; }
    }
}
