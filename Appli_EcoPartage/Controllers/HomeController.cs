using System.Diagnostics;
using Appli_EcoPartage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Appli_EcoPartage.Data;

namespace Appli_EcoPartage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _dbContext.Annonces.Include(a => a.User);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annonces = await _dbContext.Annonces
            .Include(a => a.User)
            .Include(a => a.AnnoncesTags)
                .ThenInclude(at => at.Tag)
            .Include(a => a.AnnoncesGeoSectors)
                .ThenInclude(ags => ags.GeographicalSector)
            .FirstOrDefaultAsync(m => m.IdAnnonce == id);

            if (annonces == null)
            {
                return NotFound();
            }

            return View(annonces);
        }

        [Authorize]
        public IActionResult Profile(int page = 1, int pageSize = 10)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Error");
            }
            var userId = int.Parse(userIdClaim.Value);

            var user = _dbContext.Users
                .Include(u => u.CommentsRecived)
                .ThenInclude(c => c.Giver)
                .Include(u => u.MyAnnonces)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return RedirectToAction("Error");
            }

            var totalComments = user.CommentsRecived.Count;
            var comments = user.CommentsRecived
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new UserCommentDisplayModel
                {
                    GiverUserName = c.Giver?.UserName ?? "Anonymous",
                    Notice = c.Notice,
                    Date = c.Date
                }).ToList();

            var totalMesAnnonces = user.MyAnnonces.Count;
            var annonces = user.MyAnnonces
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new UserAnnonceDisplayModel
                {
                    Titre = a.Titre,
                    Description = a.Description,
                    Points = a.Points,
                    Date = a.Date,
                    IsActive = a.Active
                }).ToList();

            var viewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Points = user.Points,
                Comments = comments,
                Annonces = annonces
            };

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalComments / pageSize);
            ViewBag.CurrentPage = page;

            return View(viewModel);
        }

        [Authorize]
        public IActionResult LoadComments(int page = 1, int pageSize = 10)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return BadRequest("User not found.");
            }
            var userId = int.Parse(userIdClaim.Value);

            var user = _dbContext.Users
                .Include(u => u.CommentsRecived)
                .ThenInclude(c => c.Giver)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var comments = user.CommentsRecived
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new UserCommentDisplayModel
                {
                    GiverUserName = c.Giver?.UserName ?? "Anonymous",
                    Notice = c.Notice,
                    Date = c.Date
                }).ToList();

            return PartialView("_CommentsPartial", comments);
        }

        [Authorize]
        public IActionResult LoadAnnonces(int page = 1, int pageSize = 10)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return BadRequest("User not found.");
            }
            var userId = int.Parse(userIdClaim.Value);

            var user = _dbContext.Users
                .Include(u => u.MyAnnonces)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var annonces = user.MyAnnonces
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new UserAnnonceDisplayModel
                {
                    Titre = a.Titre,
                    Description = a.Description,
                    Points = a.Points,
                    Date = a.Date,
                    IsActive = a.Active
                }).ToList();

            return PartialView("_AnnoncesPartial", annonces);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
