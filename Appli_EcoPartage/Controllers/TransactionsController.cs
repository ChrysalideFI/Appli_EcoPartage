using Appli_EcoPartage.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Appli_EcoPartage.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ApplicationDbContext context, ILogger<TransactionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
            {
                return RedirectToAction("Login", "Account");
            }

            var transactions = await _context.Transactions
                .Include(t => t.UserBuyer)
                .Include(t => t.UserSeller)
                .Include(t => t.Annonce)
                .Where(t => t.UserIdBuyer.ToString() == currentUserId || t.UserIdSeller.ToString() == currentUserId)
                .ToListAsync();

            return View(transactions);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.UserBuyer)
                .Include(t => t.UserSeller)
                .Include(t => t.Annonce)
                .FirstOrDefaultAsync(t => t.IdTransaction == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CreateTransaction(int annonceId)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
            {
                return RedirectToAction("permission denied");
            }

            var annonce = await _context.Annonces
                .Include(a => a.User) // seller
                .FirstOrDefaultAsync(a => a.IdAnnonce == annonceId);

            if (annonce == null)
            {
                return NotFound();
            }

            // check if the user is trying to create a transaction with himself
            if (annonce.IdUser.ToString() == currentUserId)
            {
                return BadRequest("You cannot create a transaction with yourself.");
            }

            // get the buyer info
            var buyer = await _context.Users.FindAsync(int.Parse(currentUserId));
            if (buyer == null)
            {
                return NotFound();
            }

            // check if the buyer has enough points
            if (buyer.Points < annonce.Points)
            {
                TempData["TransactionError"] = "Insufficient points to create this transaction.";
                return RedirectToAction("Index", "Annonces");
            }

            var transaction = new Transactions
            {
                UserIdBuyer = buyer.Id, // buyer
                UserBuyer = buyer,
                UserIdSeller = annonce.IdUser, // seller
                UserSeller = annonce.User,
                IdAnnonce = annonce.IdAnnonce,
                Annonce = annonce,
                Status = "Pending",
                DateTransaction = DateTime.Now
            };

            _context.Transactions.Add(transaction);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving transaction: {ex.Message}");
                return BadRequest("Failed to save transaction.");
            }

            TempData["Success"] = "Transaction created successfully.";
            return RedirectToAction("Index", "Annonces");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessTransaction(int id, string action)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Annonce)
                .Include(t => t.UserBuyer)
                .Include(t => t.UserSeller)
                .FirstOrDefaultAsync(t => t.IdTransaction == id);

            if (transaction == null)
            {
                return NotFound();
            }

            // check if the user is the seller
            var sellerId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (transaction.UserIdSeller.ToString() != sellerId)
            {
                return Unauthorized();
            }

            try
            {
                if (action == "Accept")
                {
                    if (transaction.UserBuyer.Points < transaction.Annonce.Points)
                    {
                        TempData["TransactionError"] = "Buyer's points are insufficient to complete the transaction.";
                        return RedirectToAction("Details", "Transactions", new { id = transaction.IdTransaction });
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
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing transaction: {ex.Message}");
                return BadRequest("Failed to process transaction.");
            }

            TempData["Success"] = "Transaction processed successfully.";
            return RedirectToAction("Index", "Transactions");
        }
    }
}


