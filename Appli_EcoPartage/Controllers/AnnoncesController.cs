using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Appli_EcoPartage.Data;

namespace Appli_EcoPartage.Controllers
{
    public class AnnoncesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnnoncesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Annonces
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Annonces.Include(a => a.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Annonces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annonces = await _context.Annonces
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.IdAnnonce == id);
            if (annonces == null)
            {
                return NotFound();
            }

            return View(annonces);
        }

        // GET: Annonces/Create
        public IActionResult Create()
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized();
            }

            ViewBag.IdUser = new SelectList(
                 _context.Users.Where(u => u.Id.ToString() == currentUserId),
                 "Id",
                 "Id",
                 currentUserId
             );

            ViewBag.Tags = new SelectList(_context.Tags, "IdTag", "CategoryName");
            ViewBag.Sectors = new SelectList(_context.GeographicalSectors, "IdGeographicalSector", "Place");
            return View();
        }

        // POST: Annonces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("IdAnnonce,Titre,Description,Points,Date,Active,IdUser")] Annonces annonces,
            List<int> selectedTags,
            List<int> selectedSectors)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId)) {
                return Unauthorized();
            }

            annonces.IdUser = int.Parse(currentUserId);

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                _context.Add(annonces);
                await _context.SaveChangesAsync();

                if (selectedTags != null && selectedTags.Any())
                {
                    foreach (var tagId in selectedTags)
                    {
                        var annoncesTag = new AnnoncesTags
                        {
                            IdAnnonce = annonces.IdAnnonce,
                            IdTag = tagId,
                            Annonce = annonces,
                            Tag = await _context.Tags.FindAsync(tagId) ?? throw new InvalidOperationException("Tag not found")
                        };
                        _context.AnnoncesTags.Add(annoncesTag);
                    }
                }

                if (selectedSectors != null && selectedSectors.Any())
                {
                    foreach (var sectorid in selectedSectors)
                    {
                        var annonceGeoSector = new AnnoncesGeoSector
                        {
                            IdAnnonce = annonces.IdAnnonce,
                            IdGeographicalSector = sectorid,
                            Annonce = annonces,
                            GeographicalSector = await _context.GeographicalSectors.FindAsync(sectorid) ?? throw new InvalidOperationException("Sector not found")
                        };
                        _context.AnnoncesGeoSectors.Add(annonceGeoSector);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    Console.WriteLine($"Key: {state.Key}");
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Error: {error.ErrorMessage}");
                    }
                }
            }

            ViewBag.IdUser = new SelectList(
                _context.Users.Where(u => u.Id.ToString() == currentUserId),
                "Id",
                "Id",
                annonces.IdUser
            );
            ViewBag.Tags = new SelectList(_context.Tags, "IdTag", "CategoryName");
            ViewBag.Sectors = new SelectList(_context.GeographicalSectors, "IdGeographicalSector", "Place");

            return View(annonces);
        }

        // GET: Annonces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annonces = await _context.Annonces.FindAsync(id);
            if (annonces == null)
            {
                return NotFound();
            }
            ViewData["IdUser"] = new SelectList(_context.Users, "Id", "Id", annonces.IdUser);
            return View(annonces);
        }

        // POST: Annonces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAnnonce,Titre,Description,Points,Date,Active,IdUser")] Annonces annonces)
        {
            if (id != annonces.IdAnnonce)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(annonces);
                    await _context.SaveChangesAsync();
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
            ViewData["IdUser"] = new SelectList(_context.Users, "Id", "Id", annonces.IdUser);
            return View(annonces);
        }

        // GET: Annonces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var annonces = await _context.Annonces
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.IdAnnonce == id);
            if (annonces == null)
            {
                return NotFound();
            }

            return View(annonces);
        }

        // POST: Annonces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var annonces = await _context.Annonces.FindAsync(id);
            if (annonces != null)
            {
                _context.Annonces.Remove(annonces);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnnoncesExists(int id)
        {
            return _context.Annonces.Any(e => e.IdAnnonce == id);
        }
    }
}
