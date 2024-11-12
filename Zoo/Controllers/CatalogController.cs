using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zoo.Data;
using Zoo.Models;

namespace Zoo.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ApplicationDbContext context;

        public CatalogController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var animals = await context.Animals.Include(a => a.Comments).ToListAsync();
            return View(animals);
        }

        public async Task<IActionResult> Details(int id)
        {
            var animal = await context.Animals.Include(a => a.Category)
            .Include(a => a.Comments)
            .FirstOrDefaultAsync(a => a.AnimalId == id);

            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        public async Task<IActionResult> AddComment(int AnimalId, string Content)
        {
            var animal = await context.Animals.FindAsync(AnimalId);
            if (animal == null)
            {
                return NotFound();
            }
            if (Content.Length > 80) 
            {
                return RedirectToAction("Details", new { id = AnimalId });
            }
            var comment = new Comment
            {
                AnimalId = AnimalId,
                Content = Content,
            };

            context.Comments.Add(comment);
            await context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = AnimalId });
        }
    }
}
