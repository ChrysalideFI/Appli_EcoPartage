using Appli_EcoPartage.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Appli_EcoPartage.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _DBcontext;

        // Constructor for AdminController class
        public AdminController(ApplicationDbContext context)
        {
            // Initialize the database context
            _DBcontext = context;
        }

        // GET: AdminController/Index
        public ActionResult Index()
        {
            return View();
        }

        // GET: AdminController/ValidatedMembers
        // Get all validated users
        public async Task<IActionResult> ValidatedMembers()
        {
            var validatedMembers = await _DBcontext.Users
                .Where(u => u.IsValidated)
                .ToListAsync();
            return View(validatedMembers);
        }

        // GET: AdminController/ValidateMembers
        // Get all invaidated users for admin to validate
        public async Task<IActionResult> ValidateMembers()
        {
            var pendingMembers = await _DBcontext.Users
                .Where(u => !u.IsValidated)
                .ToListAsync();
            return View(pendingMembers);
        }

        // POST: AdminController/ApproveMember/id
        // Admin can approve an invalidated user in the list
        [HttpPost]
        public async Task<IActionResult> ApproveMember(int memberId)
        {
            var member = await _DBcontext.Users.FindAsync(memberId);
            if (member != null)
            {
                member.IsValidated = true;
                member.Points += 100; // Welcome Points 
                await _DBcontext.SaveChangesAsync();
            }
            return RedirectToAction("ValidateMembers");
        }

        // POST: AdminController/UserBlocked/id
        // Admin can change the role of a user to UserBlocked
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserBlocked(int id)
        {
            var user = await _DBcontext.Users.FindAsync(id);
            if (user != null)
            {
                var userManager = HttpContext.RequestServices.GetRequiredService<UserManager<Users>>();
                var currentRoles = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, currentRoles);
                await userManager.AddToRoleAsync(user, "UserBlocked");
                await _DBcontext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Alluser));
        }

        // GET: AdminController/EditServicePoints/id
        // Get all annonces which were created by users
        public async Task<IActionResult> EditServicePoints(int id)
        {
            var service = await _DBcontext.Annonces.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        // POST: AdminController/EditServicePoints/id
        // Admin can edit the points of an annonce which is created by users in the list
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

        // GET: AdminController/EditUserPoints/id
        // Get all users in the list with a parameter id
        public async Task<IActionResult> EditUserPoints(int id)
        {
            var user = await _DBcontext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: AdminController/EditUserPoints/id
        // Admin can edit the points of a user in the list
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

        // GET: AdminController/Alluser
        // Get all existed users in the list
        public async Task<IActionResult> Alluser()
        {
            var alluser = await _DBcontext.Users.ToListAsync();
            return View(alluser);
        }

        // GET: AdminController/AllService
        // Get all existed annonces 
        public async Task<IActionResult> AllService()
        {
            var allService = await _DBcontext.Annonces.ToListAsync();
            return View(allService);
        }

        // GET: AdminController/ContactMessages
        // Get all message written by the user
        public async Task<IActionResult> ContactMessage()
        {
            var messages = await _DBcontext.ContactMessages.Include(cm => cm.User).ToListAsync();
            return View(messages);
        }

        // POST: AdminController/MarkAsRead/5
        // Admin can change the status of the recieved message
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var message = await _DBcontext.ContactMessages.FindAsync(id);
            if (message != null)
            {
                message.IsRead = true;
                await _DBcontext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ContactMessage));
        }
    }
}