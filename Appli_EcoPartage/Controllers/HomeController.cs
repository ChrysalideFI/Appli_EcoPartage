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


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Profile(int page = 1, int pageSize = 10)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                // Handle the case where the claim is not found
                return RedirectToAction("Error");
            }
            var userId = int.Parse(userIdClaim.Value);

            var user = _dbContext.Users
                .Include(u => u.CommentsRecived)
                .ThenInclude(c => c.Giver)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null) {
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

            var viewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Points = user.Points,
                Comments = comments
            };

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalComments / pageSize);
            ViewBag.CurrentPage = page;

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
