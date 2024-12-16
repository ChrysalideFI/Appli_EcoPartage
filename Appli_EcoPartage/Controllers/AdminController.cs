using Appli_EcoPartage.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Appli_EcoPartage.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _DBcontext;


        public AdminController(ApplicationDbContext context)
        {
            _DBcontext = context;
        }

        // GET: AdminController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AdminController/ValidatedMembers
        public async Task<IActionResult> ValidatedMembers()
        {
            var validatedMembers = await _DBcontext.Users
                .Where(u => u.IsValidated)
                .ToListAsync();
            return View(validatedMembers);
        }

        // GET: AdminController/ValidateMembers
        public async Task<IActionResult> ValidateMembers()
        {
            var pendingMembers = await _DBcontext.Users
                .Where(u => !u.IsValidated)
                .ToListAsync();
            return View(pendingMembers);
        }

        // POST: AdminController/ApproveMember/5
        [HttpPost]
        public async Task<IActionResult> ApproveMember(int memberId)
        {
            var member = await _DBcontext.Users.FindAsync(memberId);
            if (member != null)
            {
                member.IsValidated = true;
                member.Points += 100; // Points de bienvenue
                await _DBcontext.SaveChangesAsync();
            }
            return RedirectToAction("ValidateMembers");
        }

        // POST: AdminController/RejectMember/5
        [HttpPost]
        public async Task<IActionResult> RejectMember(int memberId)
        {
            var member = await _DBcontext.Users.FindAsync(memberId);
            if (member != null)
            {
                _DBcontext.Users.Remove(member);
                await _DBcontext.SaveChangesAsync();
            }
            return RedirectToAction("ValidateMembers");
        }

        // GET: AdminController/ValidateServices
        public async Task<IActionResult> ValidateServices()
        {
            var pendingServices = await _DBcontext.Annonces
                .Where(a => !a.IsValidated)
                .ToListAsync();
            return View(pendingServices);
        }

        // POST: AdminController/ApproveService/5
        [HttpPost]
        public async Task<IActionResult> ApproveService(int serviceId)
        {
            var service = await _DBcontext.Annonces.FindAsync(serviceId);
            if (service != null)
            {
                service.IsValidated = true;
                service.Active = true;
                var user = await _DBcontext.Users.FindAsync(service.IdUser);
                if (user != null)
                {
                    user.Points += service.Points; // Points attribués pour le service
                }
                await _DBcontext.SaveChangesAsync();
            }
            return RedirectToAction("ValidateServices");
        }

        // POST: AdminController/RejectService/5
        [HttpPost]
        public async Task<IActionResult> RejectService(int serviceId)
        {
            var service = await _DBcontext.Annonces.FindAsync(serviceId);
            if (service != null)
            {
                _DBcontext.Annonces.Remove(service);
                await _DBcontext.SaveChangesAsync();
            }
            return RedirectToAction("ValidateServices");
        }
        
        // GET: AdminController/EditServicePoints/5
        public async Task<IActionResult> EditServicePoints(int id)
        {
            var service = await _DBcontext.Annonces.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        // POST: AdminController/EditServicePoints/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditServicePoints(int id, [Bind("IdAnnonce,Points")] Annonces service)
        {
            if (id != service.IdAnnonce)
            {
                Console.WriteLine("Service not found");
                return NotFound();
                
            }

            var existingService = await _DBcontext.Annonces.FindAsync(id);
            if (existingService != null)
            {
                existingService.Points = service.Points;
                await _DBcontext.SaveChangesAsync();
                return RedirectToAction("Allservice", "Admin");
            }
            return View(service);
        }

        // GET: AdminController/EditUserPoints/5
        public async Task<IActionResult> EditUserPoints(int id)
        {
            var user = await _DBcontext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: AdminController/EditUserPoints/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserPoints(int id, [Bind("Id,Points")] Users user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            var existingUser = await _DBcontext.Users.FindAsync(id);
            if (existingUser != null)
            {
                existingUser.Points = user.Points;
                await _DBcontext.SaveChangesAsync();
                return RedirectToAction("Alluser", "Admin");
            }
            return View(user);
        }

        public async Task<IActionResult> Alluser()
        {
            var alluser = await _DBcontext.Users.ToListAsync();
            return View(alluser);
        }

        public async Task<IActionResult> AllService()
        {
            var allService = await _DBcontext.Annonces.ToListAsync();
            return View(allService);
        }
    }
}