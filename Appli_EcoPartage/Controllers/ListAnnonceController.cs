using Appli_EcoPartage.Data;
using Appli_EcoPartage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Appli_EcoPartage.Controllers
{
    public class ListAnnonceController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ListAnnonceController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index(int id)
        {
            var annonce = _dbContext.Annonces
                .Include(a => a.AnnoncesTags)
                    .ThenInclude(at => at.Tag)
                .Include(a => a.GeographicalSectors)
                .Include(a => a.User)
                .FirstOrDefault(a => a.IdAnnonce == id);

            if (annonce == null)
            {
                return NotFound("Annonce not found.");
            }

            var viewModel = new AnnonceViewModel
            {
                IdAnnonce = annonce.IdAnnonce,
                Titre = annonce.Titre,
                Description = annonce.Description,
                Date = annonce.Date,
                IsActive = annonce.Active,
                UserName = annonce.User?.UserName ?? "Unknown User",
                Tags = annonce.AnnoncesTags.Select(at => at.Tag.CategoryName).ToList(),
                GeographicalSectors = annonce.GeographicalSectors.Select(gs => gs.FirstPlace.ToString()).ToList()
            };

            return View(viewModel);
        }
    }
}
