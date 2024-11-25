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
            var animal = await context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            context.Animals.Remove(animal);
            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Catalog");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int id, int cid)
        {
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
                };

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var result = await ProcessImageUpload(model.ImageFile);

                    animal.ImagePath = result.ImagePath;
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

        [HttpGet]
        public async Task<IActionResult> EditAnimal(int id)
        {
            var animal = await context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            var viewModel = new AnimalVM
            {
                AnimalId = animal.AnimalId,
                Name = animal.Name,
                Age = animal.Age,
                CategoryId = animal.CategoryId,
                Description = animal.Description,
                ImagePath = animal.ImagePath
            };

            ViewBag.Categories = await context.Categories.ToListAsync();
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAnimal(AnimalVM model)
        {
            if (model.ImageFile == null || model.ImageFile.Length == 0)
            {
                ModelState.Remove(nameof(model.ImageFile));
            }

            if (ModelState.IsValid)
            {
                var animalToUpdate = await context.Animals.FindAsync(model.AnimalId);
                if (animalToUpdate == null)
                {
                    return NotFound();
                }

                animalToUpdate.Name = model.Name;
                animalToUpdate.Age = model.Age;
                animalToUpdate.CategoryId = model.CategoryId;
                animalToUpdate.Description = model.Description;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    // Process image upload
                    var result = await ProcessImageUpload(model.ImageFile);
                    if (result.Success)
                    {
                        animalToUpdate.ImagePath = result.ImagePath;
                    }
                    else
                    {
                        ModelState.AddModelError("ImageFile", result.ErrorMessage);
                        ViewBag.Categories = await context.Categories.ToListAsync();
                        return View(model);
                    }
                }
                
                context.Update(animalToUpdate);
                await context.SaveChangesAsync();
                return RedirectToAction("Index", "Catalog");
            }

            ViewBag.Categories = await context.Categories.ToListAsync();
            return View(model);
        }

        private async Task<(bool Success, string ImagePath, string ErrorMessage)> ProcessImageUpload(IFormFile imageFile)
        {
            // Validate file size (max 2MB)
            if (imageFile.Length > 2097152)
            {
                return (false, null, "The image file is too large (max 2MB).");
            }

            // Validate file type
            var fileName = imageFile.FileName;
            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (extension != ".jpg" && extension != ".jpeg")
            {
                return (false, null, "Only JPG/JPEG images are allowed.");
            }


            // Define the path to save the file
            var filePath = Path.Combine("wwwroot/images/animals", fileName);

            // Ensure directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            // Resize and save the image
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ResizeImage(imageFile, stream, 1280, 854);
            }


            // Return the relative path to the new image
            var relativeImagePath = "/images/animals/" + fileName;

            return (true, relativeImagePath, null);
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
