using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Appli_EcoPartage.Data;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Annonces/Create
        [Authorize]
        public IActionResult Create()
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
            {
                return RedirectToAction("AccessDenied");
            }

            ViewBag.IdUser = new SelectList(
                 _context.Users.Where(u => u.Id.ToString() == currentUserId),
                 "Id",
                 "Id",
                 currentUserId
             );

            ViewBag.Tags = new MultiSelectList(_context.Tags, "IdTag", "CategoryName");
            ViewBag.Sectors = new MultiSelectList(_context.GeographicalSectors, "IdGeographicalSector", "Place");
            return View();
        }

        // POST: Annonces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(
            [Bind("IdAnnonce,Titre,Description,Points,Date,Active,IdUser")] Annonces annonces,
            List<int> selectedTags,
            List<int> selectedSectors)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId)) {
                return RedirectToAction("AccessDenied");
            }

            annonces.IdUser = int.Parse(currentUserId);

            if (string.IsNullOrEmpty(currentUserId))
            {
                return RedirectToAction("AccessDenied");
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
                return RedirectToAction("Index", "Home");
            }

            ViewBag.IdUser = new SelectList(
                _context.Users.Where(u => u.Id.ToString() == currentUserId),
                "Id",
                "Id",
                annonces.IdUser
            );
            ViewBag.Tags = new MultiSelectList(_context.Tags, "IdTag", "CategoryName");
            ViewBag.Sectors = new MultiSelectList(_context.GeographicalSectors, "IdGeographicalSector", "Place");

            return View(annonces);
        }
    }
}
