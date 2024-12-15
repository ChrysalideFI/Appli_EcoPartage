using Appli_EcoPartage.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Appli_EcoPartage.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessTransaction(int id, string action)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Annonce)
                .Include(t => t.UserSeller)
                .Include(t => t.UserBuyer)
                .FirstOrDefaultAsync(t => t.IdTransaction == id);

            if (transaction == null)
            {
                return NotFound();
            }

            var sellerId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (transaction.UserIdBuyer.ToString() != sellerId)
            {
                return Unauthorized();
            }

            if (action == "Accept")
            {
                if (transaction.UserBuyer.Points < transaction.Annonce.Points)
                {
                    TempData["Error"] = "Buyer's points are insufficient to complete the transaction.";
                    return RedirectToAction("Details", new { id = transaction.IdTransaction });
                }

                transaction.UserBuyer.Points -= transaction.Annonce.Points;

                transaction.UserSeller.Points += transaction.Annonce.Points;

                transaction.Status = "Accepted";
            }
            else if (action == "Decline")
            {
                transaction.Status = "Declined";
            }
            else
            {
                return BadRequest();
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = transaction.IdTransaction });
        }

    }
}
