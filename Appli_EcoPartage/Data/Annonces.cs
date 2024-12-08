using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Appli_EcoPartage.Data
{
    public class Annonces
    {
        [Key]
        public int IdAnnonce { get; set; }
        [Required]
        public string? Titre { get; set; }
        [Required]
        public string? Description { get; set; }
        public int Points { get; set; }
        public DateTime Date { get; set; }
        public bool Active { get; set; }

        [ForeignKey("User")]
        public int IdUser { get; set; }
        [Required]
        public virtual Users? User { get; set; }
    }
}
