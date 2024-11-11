using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zoo.Data;

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
            var animal = await context.Animals.Where(a => a.AnimalId == id).Include(a => a.Comments).Include(a => a.Category).SingleAsync();
            return View(animal);
        }
    }
}
