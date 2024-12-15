using Appli_EcoPartage.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Appli_EcoPartage.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Profile/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.CommentsRecived)
                    .ThenInclude(c => c.Giver)
                .Include(u => u.MyAnnonces)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.IsCurrentUser = currentUserId == user.Id.ToString();
            ViewBag.CurrentUserId = currentUserId;

            return View(user);
        }

        // POST: Profile/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddComment(int RecipientId, string Notice)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var giver = await _context.Users.FindAsync(int.Parse(currentUserId));
            var recipient = await _context.Users.FindAsync(RecipientId);

            if (giver == null || recipient == null)
            {
                return NotFound();
            }

            var comment = new Comments
            {
                IdUserGiven = int.Parse(currentUserId),
                IdUserRecipient = RecipientId,
                Notice = Notice,
                Date = DateTime.Now,
                Giver = giver,
                Recipient = recipient
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = RecipientId });
        }

        // POST: Profile/DeleteComment/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var comment = await _context.Comments
                .Include(c => c.Giver)
                .FirstOrDefaultAsync(c => c.IdComment == id);

            if (comment == null || comment.Giver.Id != int.Parse(currentUserId))
            {
                return Unauthorized();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = comment.IdUserRecipient });
        }

    }

}
