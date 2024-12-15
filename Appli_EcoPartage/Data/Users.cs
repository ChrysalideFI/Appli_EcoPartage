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
        public required virtual ICollection<Comments> CommentsGiven { get; set; }
        public required virtual ICollection<Comments> CommentsRecived { get; set; }
        
        public required virtual ICollection<Annonces> MyAnnonces { get; set; }

        public required virtual ICollection<Transactions> TransactionsSeller { get; set; }
        public required virtual ICollection<Transactions> TransactionsBuyer { get; set; }
    }
}
