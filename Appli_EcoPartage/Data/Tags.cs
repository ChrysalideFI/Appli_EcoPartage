using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Appli_EcoPartage.Data
{
    public class Tags
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int IdTag { get; set; }
        [Required]
        public required string CategoryName { get; set; }
    }
}
