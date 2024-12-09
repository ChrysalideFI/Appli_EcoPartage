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
        public virtual ICollection<Comments>? CommentsGiven { get; set; }
        public virtual ICollection<Comments>? CommentsRecived { get; set; }

        //public virtual ICollection<Transactions>? TransactionsGiven { get; set; }
        //public virtual ICollection<Transactions>? TransactionsRecived { get; set; }
    }
}
