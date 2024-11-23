using System.ComponentModel.DataAnnotations;

namespace Zoo.Models
{
    public class AnimalViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(0, 100, ErrorMessage = "Age must be between 0 and 100")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please upload an image file.")]
        [Display(Name = "Upload Image")]
        public IFormFile ImageFile { get; set; }
    }
}
