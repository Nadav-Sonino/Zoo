namespace Zoo.Models
{
    public class Animal
    {
        public int AnimalId { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
