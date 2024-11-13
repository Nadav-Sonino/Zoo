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

        public async Task<IActionResult> Index(int? categoryId, string searchTerm, int page = 0)
        {
            int pageSize = 6;

            // Fetch categories
            var categories = await context.Categories.ToListAsync();
            ViewData["Categories"] = categories;

            // Fetch animals
            var animalsQuery = context.Animals.Include(a => a.Category).AsQueryable();

            // Apply category filter if provided
            if (categoryId.HasValue && categoryId.Value != 0)
            {
                animalsQuery = animalsQuery.Where(a => a.CategoryId == categoryId.Value);
                ViewData["SelectedCategoryId"] = categoryId.Value;
            }
            else
            {
                ViewData["SelectedCategoryId"] = 0;
            }

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                animalsQuery = animalsQuery.Where(a => a.Name.Contains(searchTerm) || a.Description.Contains(searchTerm));
                ViewData["SearchTerm"] = searchTerm;
            }
            else
            {
                ViewData["SearchTerm"] = string.Empty;
            }

            // Get total count after filters
            int totalAnimals = await animalsQuery.CountAsync();

            // Fetch animals for the current page
            var animals = await animalsQuery
                .OrderBy(a => a.AnimalId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalAnimals / pageSize);

            return View(animals);
        }



        public async Task<IActionResult> Details(int id, int page = 0)
        {
            const int PageSize = 5;
            var animal = await context.Animals.Include(a => a.Category)
                .Include(a => a.Comments)
                .FirstOrDefaultAsync(a => a.AnimalId == id);

            if (animal == null)
            {
                return NotFound();
            }

            ViewData["CurrentCommentsPage"] = page;
            var paginatedComments = animal.Comments
                .Skip(page * PageSize)
                .Take(PageSize)
                .ToList();

            ViewData["TotalCommentPages"] = (int)Math.Ceiling((double)animal.Comments.Count / PageSize);
            animal.Comments = paginatedComments;

            return View(animal);
        }


        public IActionResult NextCommentsPage(int AnimalId, int currentPage)
        {
            return RedirectToAction("Details", new { id = AnimalId, page = currentPage + 1 });
        }

        public IActionResult PreviousCommentsPage(int AnimalId, int currentPage)
        {
            return RedirectToAction("Details", new { id = AnimalId, page = Math.Max(currentPage - 1, 0) });
        }


        public async Task<IActionResult> AddComment(int AnimalId, string Content)
        {
            if (string.IsNullOrWhiteSpace(Content) || Content.Length > 80)
            {
                return RedirectToAction("Details", new { id = AnimalId });
            }

            var animal = await context.Animals.FindAsync(AnimalId);
            if (animal == null)
            {
                return NotFound();
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
