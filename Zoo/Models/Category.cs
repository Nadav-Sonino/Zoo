namespace Zoo.Models
{
    public class Category
    {
        
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public ICollection<Animal> Animals { get; set; }
    }
}
