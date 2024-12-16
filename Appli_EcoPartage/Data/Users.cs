using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Appli_EcoPartage.Data
{
    public class Users : IdentityUser<int>
    {
        public Users() : base()
        {
        }
        public int Points { get; set; }
        public bool IsValidated { get; set; } = false; //Validation de l'inscription par l'admin
        public virtual ICollection<Comments> CommentsGiven { get; set; }
        public virtual ICollection<Comments> CommentsRecived { get; set; }
        
        public virtual ICollection<Annonces> MyAnnonces { get; set; }

        public virtual ICollection<Transactions> TransactionsSeller { get; set; }
        public virtual ICollection<Transactions> TransactionsBuyer { get; set; }
    }
}
