using System.ComponentModel.DataAnnotations;

namespace Zoo.Models
{
    public class AnimalVM
    {
        public int AnimalId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(0, 100, ErrorMessage = "Age must be between 0 and 100")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string ImagePath { get; set; }

        // Include ImageFile if you're handling image uploads
        [Display(Name = "Upload New Image")]
        public IFormFile ImageFile { get; set; }
    }
}
