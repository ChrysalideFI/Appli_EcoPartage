using System.Diagnostics;
using Appli_EcoPartage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Appli_EcoPartage.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

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

        // GET: Annonces
        [Authorize]
        public async Task<IActionResult> Index(string searchString)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var user = await _dbContext.Users.FindAsync(int.Parse(userId));
                if (user != null)
                {
                    ViewBag.IsValidated = user.IsValidated;
                }
            }

            ViewBag.CurrentFilter = searchString;
            var annonces = from a in _dbContext.Annonces
                      .Include(a => a.User)
                      .Include(a => a.AnnoncesTags)
                          .ThenInclude(at => at.Tag)
                           select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                annonces = annonces.Where(a => a.Titre.Contains(searchString) ||
                                               a.Description.Contains(searchString) ||
                                               a.AnnoncesTags.Any(at => at.Tag.CategoryName.Contains(searchString)));
            }
            // Trie par date de publication et sélectionne les tois dernières annonces postées
            annonces = annonces.OrderByDescending(a => a.Date).Take(3);

            return View(await annonces.ToListAsync());
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

        // GET: Annonces/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annonces = await _dbContext.Annonces
                .Include(a => a.AnnoncesTags)
                .ThenInclude(at => at.Tag)
                .Include(a => a.AnnoncesGeoSectors)
                .ThenInclude(ags => ags.GeographicalSector)
                .FirstOrDefaultAsync(a => a.IdAnnonce == id);

            if (annonces == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null || annonces.IdUser.ToString() != currentUserId)
            {
                return RedirectToAction("AccessDenied");
            }


            ViewBag.Tags = new MultiSelectList(_dbContext.Tags, "IdTag", "CategoryName", annonces.AnnoncesTags.Select(at => at.IdTag));
            ViewBag.Sectors = new MultiSelectList(_dbContext.GeographicalSectors, "IdGeographicalSector", "Place", annonces.AnnoncesGeoSectors.Select(ags => ags.IdGeographicalSector));
            ViewData["IdUser"] = new SelectList(_dbContext.Users, "Id", "Id", annonces.IdUser);
            return View(annonces);
        }

        // POST: Annonces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id,
                                               [Bind("IdAnnonce,Titre,Description,Points,Date,Active,IdUser")] Annonces annonces,
                                               List<int> selectedTags,
                                               List<int> selectedSectors)
        {
            if (id != annonces.IdAnnonce)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(annonces);
                    await _dbContext.SaveChangesAsync();

                    var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    var existingAnnonce = await _dbContext.Annonces.AsNoTracking().FirstOrDefaultAsync(a => a.IdAnnonce == id);
                    if (existingAnnonce == null || currentUserId == null || existingAnnonce.IdUser.ToString() != currentUserId)
                    {
                        return RedirectToAction("AccessDenied");
                    }

                    var existingTags = _dbContext.AnnoncesTags.Where(at => at.IdAnnonce == annonces.IdAnnonce).ToList();
                    _dbContext.AnnoncesTags.RemoveRange(existingTags);

                    if (selectedTags != null && selectedTags.Any())
                    {
                        foreach (var tagId in selectedTags)
                        {
                            var annoncesTag = new AnnoncesTags
                            {
                                IdAnnonce = annonces.IdAnnonce,
                                IdTag = tagId,
                                Annonce = annonces,
                                Tag = await _dbContext.Tags.FindAsync(tagId) ?? throw new InvalidOperationException("Tag not found")
                            };
                            _dbContext.AnnoncesTags.Add(annoncesTag);
                        }
                    }

                    var existingSectors = _dbContext.AnnoncesGeoSectors.Where(ags => ags.IdAnnonce == annonces.IdAnnonce).ToList();
                    _dbContext.AnnoncesGeoSectors.RemoveRange(existingSectors);
                    if (selectedSectors != null && selectedSectors.Any())
                    {
                        foreach (var sectorid in selectedSectors)
                        {
                            var annonceGeoSector = new AnnoncesGeoSector
                            {
                                IdAnnonce = annonces.IdAnnonce,
                                IdGeographicalSector = sectorid,
                                Annonce = annonces,
                                GeographicalSector = await _dbContext.GeographicalSectors.FindAsync(sectorid) ?? throw new InvalidOperationException("Sector not found")
                            };
                            _dbContext.AnnoncesGeoSectors.Add(annonceGeoSector);
                        }
                    }

                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnoncesExists(annonces.IdAnnonce))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Tags = new MultiSelectList(_dbContext.Tags, "IdTag", "CategoryName", selectedTags);
            ViewBag.Sectors = new MultiSelectList(_dbContext.GeographicalSectors, "IdGeographicalSector", "Place", selectedSectors);
            ViewData["IdUser"] = new SelectList(_dbContext.Users, "Id", "Id", annonces.IdUser);
            return View(annonces);
        }

        // GET: Annonces/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
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

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null || annonces.IdUser.ToString() != currentUserId)
            {
                return RedirectToAction("AccessDenied");
            }

            return View(annonces);
        }

        // POST: Annonces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var annonces = await _dbContext.Annonces
                .Include(a => a.AnnoncesTags)
                .Include(a => a.AnnoncesGeoSectors)
                .FirstOrDefaultAsync(a => a.IdAnnonce == id);

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var existingAnnonce = await _dbContext.Annonces.AsNoTracking().FirstOrDefaultAsync(a => a.IdAnnonce == id);
            if (existingAnnonce == null || currentUserId == null || existingAnnonce.IdUser.ToString() != currentUserId)
            {
                return RedirectToAction("AccessDenied");
            }

            if (annonces != null)
            {
                if (annonces.AnnoncesTags != null)
                {
                    _dbContext.AnnoncesTags.RemoveRange(annonces.AnnoncesTags);
                }

                if (annonces.AnnoncesGeoSectors != null)
                {
                    _dbContext.AnnoncesGeoSectors.RemoveRange(annonces.AnnoncesGeoSectors);
                }
                _dbContext.Annonces.Remove(annonces);
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnnoncesExists(int id)
        {
            return _dbContext.Annonces.Any(e => e.IdAnnonce == id);
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
