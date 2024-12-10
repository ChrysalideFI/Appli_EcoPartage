using Appli_EcoPartage.Data;
using Appli_EcoPartage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Appli_EcoPartage.Controllers
{
    public class ListeFilmsController : Controller
    {
        private readonly ApplicationDbContext _contexte;
        private readonly UserManager<Users> _gestionnaire;

        public ListeFilmsController(ApplicationDbContext contexte,
           UserManager<Users> gestionnaire)
        {
            _contexte = contexte;
            _gestionnaire = gestionnaire;
        }
        private Task<Users> GetCurrentUserAsync() =>
           _gestionnaire.GetUserAsync(HttpContext.User);

        [HttpGet]
        public async Task<int> RecupererIdUtilisateurCourant()
        {
            Users utilisateur = await GetCurrentUserAsync();
            return utilisateur.Id;
        }
    }
}
