using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using Zoo.Models;

namespace Zoo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Mammal" },
                new Category { CategoryId = 2, Name = "Reptile" },
                new Category { CategoryId = 3, Name = "Aquatic" },
                new Category { CategoryId = 4, Name = "Bird" }
            );

            // Seed Animals
            modelBuilder.Entity<Animal>().HasData(
                new Animal
                {
                    AnimalId = 1,
                    Name = "Lion",
                    Age = 5,
                    CategoryId = 1, // Mammal
                    Description = "King of the jungle.",
                    ImageUrl = "/images/lion.jpg"
                },
                new Animal
                {
                    AnimalId = 2,
                    Name = "Elephant",
                    Age = 10,
                    CategoryId = 1, // Mammal
                    Description = "The largest land animal.",
                    ImageUrl = "/images/elephant.jpg"
                },
                new Animal
                {
                    AnimalId = 3,
                    Name = "Crocodile",
                    Age = 7,
                    CategoryId = 2, // Reptile
                    Description = "A large aquatic reptile.",
                    ImageUrl = "/images/crocodile.jpg"
                },
                new Animal
                {
                    AnimalId = 4,
                    Name = "Dolphin",
                    Age = 8,
                    CategoryId = 3, // Aquatic
                    Description = "An intelligent aquatic mammal.",
                    ImageUrl = "/images/dolphin.jpg"
                }
            );

            // Seed Comments
            modelBuilder.Entity<Comment>().HasData(
                new Comment
                {
                    CommentId = 1,
                    AnimalId = 1, // Lion
                    Content = "The lion looks majestic!",
                },
                new Comment
                {
                    CommentId = 2,
                    AnimalId = 2, // Elephant
                    Content = "Elephants are so intelligent.",
                },
                new Comment
                {
                    CommentId = 3,
                    AnimalId = 4, // Dolphin
                    Content = "Dolphins are my favorite!",
                },
                new Comment
                {
                    CommentId = 4,
                    AnimalId = 1, // Lion
                    Content = "I love lions!",
                }
            );
        }
    }
}
