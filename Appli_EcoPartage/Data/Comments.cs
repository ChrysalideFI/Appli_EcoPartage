﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Appli_EcoPartage.Data
{
    public class Comments
    {
        [Key]
        public int IdComment { get; set; }
        public DateTime Date { get; set; }
        public required string Notice { get; set; }
        [Required]
        [ForeignKey("Giver")] //L'utilisateur qui a donné le commentaire
        public required int IdUserGiven { get; set; }
        public required virtual Users Giver { get; set; }

        [ForeignKey("Recipient")] //L'utilisateur qui a reçu le commentaire
        public required int IdUserRecipient { get; set; }
        public required virtual Users Recipient { get; set; }
    }
}
