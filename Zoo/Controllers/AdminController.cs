using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;
using Zoo.Data;
using Zoo.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

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

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var animal = await context.Animals
                .FirstOrDefaultAsync(a => a.AnimalId == id);

            context.Animals.Remove(animal);
            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Catalog");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int id, int cid)
        {
            // Fetch the animal including its comments
            var animal = await context.Animals
                .Include(a => a.Comments)
                .FirstOrDefaultAsync(a => a.AnimalId == id);

            var comment = animal.Comments.FirstOrDefault(c => c.CommentId == cid);

            animal.Comments.Remove(comment);

            await context.SaveChangesAsync();

            return RedirectToAction("Details", "Catalog", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> CreateAnimal()
        {
            ViewData["Categories"] = await context.Categories.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAnimal(AnimalViewModel model)
        {
            if (ModelState.IsValid)
            {
                var animal = new Animal
                {
                    Name = model.Name,
                    Age = model.Age,
                    CategoryId = model.CategoryId,
                    Description = model.Description,
                    ImageFile = model.ImageFile
                };

                if (animal.ImageFile != null && animal.ImageFile.Length > 0)
                {
                    // Validate file size max 2MB
                    if (animal.ImageFile.Length > 2097152)
                    {
                        ModelState.AddModelError("ImageFile", "The image file is too large (max 2MB).");
                        ViewData["Categories"] = await context.Categories.ToListAsync();
                        return View(model);
                    }

                    // Validate file type
                    var extension = Path.GetExtension(animal.ImageFile.FileName).ToLowerInvariant();
                    if (extension != ".jpg" && extension != ".jpeg")
                    {
                        ModelState.AddModelError("ImageFile", "Only JPG/JPEG images are allowed.");
                        ViewData["Categories"] = await context.Categories.ToListAsync();
                        return View(model);
                    }

                    // Generate unique file name
                    var fileName = Path.GetRandomFileName() + extension;

                    // Define the path to save the file
                    var filePath = Path.Combine("wwwroot/images/animals", fileName);

                    // Ensure directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    // Resize and save the image
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ResizeImage(animal.ImageFile, stream, 1280, 854);
                    }

                    // Save the relative path to the database
                    animal.ImagePath = "/images/animals/" + fileName;
                }
                else
                {
                    ModelState.AddModelError("ImageFile", "Please upload an image file.");
                    ViewData["Categories"] = await context.Categories.ToListAsync();
                    return View(model);
                }

                context.Add(animal);
                await context.SaveChangesAsync();
                return RedirectToAction("Index", "Catalog");
            }

            ViewData["Categories"] = await context.Categories.ToListAsync();
            return View(model);
        }


        
        private bool AnimalExists(int id)
        {
            return context.Animals.Any(e => e.AnimalId == id);
        }



        private async Task ResizeImage(IFormFile imageFile, Stream outputStream, int width, int height)
        {
            using (var image = await Image.LoadAsync(imageFile.OpenReadStream()))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(width, height)
                }));

                var encoder = new JpegEncoder
                {
                    Quality = 90
                };

                await image.SaveAsJpegAsync(outputStream, encoder);
            }
        }
    }
}
