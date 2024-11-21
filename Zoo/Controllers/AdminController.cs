using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zoo.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext context;

        public AdminController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult CreateAnimal()
        {
            return View();
        }

        public IActionResult EditAnimal()
        {
            return View();
        }

        public IActionResult DeleteAnimal()
        {
            return View();
        }

        public async Task<IActionResult> DeleteComment(int id, int cid)
        {
            // Fetch the animal including its comments
            var animal = await context.Animals
                .Include(a => a.Comments)
                .FirstOrDefaultAsync(a => a.AnimalId == id);

            if (animal == null)
                return NotFound();

            // Ensure that Comments collection is not null
            if (animal.Comments == null)
                return NotFound();

            // Find the comment to delete
            var comment = animal.Comments.FirstOrDefault(c => c.CommentId == cid);

            if (comment == null)
                return NotFound();

            // Remove the comment
            animal.Comments.Remove(comment);

            // Save changes asynchronously
            await context.SaveChangesAsync();

            // Redirect to the Details action with the animal ID
            return RedirectToAction("Details", "Catalog", new { id });
        }
    }
}
