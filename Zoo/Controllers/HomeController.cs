using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zoo.Data;

namespace Zoo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;

        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var topAnimals = await context.Animals
            .Include(a => a.Comments)
            .Include(a => a.Category)
            .OrderByDescending(a => a.Comments.Count)
            .Take(2)
            .ToListAsync();

            return View(topAnimals);
        }
    }
}
