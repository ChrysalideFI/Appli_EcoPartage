using Appli_EcoPartage.Data;

namespace Appli_EcoPartage.Models
{
    public class ContactViewModel
    {
        public ContactMessage ContactMessage { get; set; }
        public IEnumerable<ContactMessage> ContactMessages { get; set; }
    }
}
