using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Appli_EcoPartage.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Appli_EcoPartage.Controllers
{
    public class AnnoncesController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor for AnnoncesController class
        public AnnoncesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Annonces
        // Cette méthode est utilisée pour afficher la liste des annonces pour l'utilisateur connecté
        [Authorize] // Cette annotation assure que seuls les utilisateurs authentifiés peuvent accéder
        public IActionResult Index()
        {
            // Récupère l'identifiant de l'utilisateur connecté
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                // Recherche l'utilisateur en utilisant id
                var user = _context.Users.Find(int.Parse(userId));
                if (user != null)
                {
                    // Stocke l'état de validation de l'utilisateur dans ViewBag
                    // pour l'utiliser dans la vue
                    ViewBag.IsValidated = user.IsValidated;
                }
            }
            return View();
        }

        // GET: Annonces/Create
        [Authorize]
        // Cette fonction est utilisée pour afficher le formulaire de création d'une annonce
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var user = _context.Users.Find(int.Parse(userId));
                if (user != null)
                {
                    // Stocke l'état de validation de l'utilisateur dans ViewBag
                    // pour l'utiliser dans la vue
                    ViewBag.IsValidated = user.IsValidated;
                }
            }
            // Récupère l'identifiant de l'utilisateur connecté
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

            // une liste déroulante pour sélectionner les tags
            ViewBag.Tags = new MultiSelectList(_context.Tags, "IdTag", "CategoryName");
            // une liste déroulante pour sélectionner les secteurs géographiques
            ViewBag.Sectors = new MultiSelectList(_context.GeographicalSectors, "IdGeographicalSector", "Place");
            return View();
        }

        // POST: Annonces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Cette méthode est utilisée pour créer une nouvelle annonce
        [HttpPost] // cette méthode répond aux requêtes HTTP POST
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
                // Ajoute l'annonce au contexte de la base de données
                _context.Add(annonces);
                await _context.SaveChangesAsync();

                // Ajoute les tags sélectionnés à l'annonce
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

                // Ajoute les secteurs géographiques à l'annonce
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

            // Prépare les listes déroulantes pour la vue
            // en cas d'erreur de validation
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
