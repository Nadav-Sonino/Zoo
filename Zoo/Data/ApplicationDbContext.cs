using Microsoft.EntityFrameworkCore;
using System;
using Zoo.Models;

namespace Zoo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Admin>().HasData(
                new Admin { AdminId = 1, UserName = "Admin", HashPassword = BCrypt.Net.BCrypt.HashPassword("Admin@1234")}
            );

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Mammal" },
                new Category { CategoryId = 2, Name = "Reptile" },
                new Category { CategoryId = 3, Name = "Aquatic" },
                new Category { CategoryId = 4, Name = "Bird" },
                new Category { CategoryId = 5, Name = "Amphibian" },
                new Category { CategoryId = 6, Name = "Insect" }
            );

            // Seed Animals
            modelBuilder.Entity<Animal>().HasData(
                // Existing Animals
                new Animal
                {
                    AnimalId = 1,
                    Name = "Lion",
                    Age = 5,
                    CategoryId = 1, // Mammal
                    Description = "King of the jungle.",
                    ImageUrl = "/images/lion1.jpg"
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
                },
                // New Animals
                new Animal
                {
                    AnimalId = 5,
                    Name = "Tiger",
                    Age = 4,
                    CategoryId = 1, // Mammal
                    Description = "A large cat species native to Asia.",
                    ImageUrl = "/images/tiger.jpg"
                },
                new Animal
                {
                    AnimalId = 6,
                    Name = "Giraffe",
                    Age = 6,
                    CategoryId = 1, // Mammal
                    Description = "The tallest living terrestrial animal.",
                    ImageUrl = "/images/giraffe.jpg"
                },
                new Animal
                {
                    AnimalId = 7,
                    Name = "Python",
                    Age = 3,
                    CategoryId = 2, // Reptile
                    Description = "A large nonvenomous snake.",
                    ImageUrl = "/images/python.jpg"
                },
                new Animal
                {
                    AnimalId = 8,
                    Name = "Eagle",
                    Age = 5,
                    CategoryId = 4, // Bird
                    Description = "A bird of prey with excellent vision.",
                    ImageUrl = "/images/eagle.jpg"
                },
                new Animal
                {
                    AnimalId = 9,
                    Name = "Penguin",
                    Age = 2,
                    CategoryId = 4, // Bird
                    Description = "A flightless bird adapted to life in the water.",
                    ImageUrl = "/images/penguin.jpg"
                },
                new Animal
                {
                    AnimalId = 10,
                    Name = "Shark",
                    Age = 12,
                    CategoryId = 3, // Aquatic
                    Description = "A large fish known for its sharp teeth.",
                    ImageUrl = "/images/shark.jpg"
                },
                new Animal
                {
                    AnimalId = 11,
                    Name = "Frog",
                    Age = 1,
                    CategoryId = 5, // Amphibian
                    Description = "A small animal that can live both on land and in water.",
                    ImageUrl = "/images/frog.jpg"
                },
                new Animal
                {
                    AnimalId = 12,
                    Name = "Butterfly",
                    Age = 0,
                    CategoryId = 6, // Insect
                    Description = "An insect with colorful wings.",
                    ImageUrl = "/images/butterfly.jpg"
                }
            );

            // Seed Comments
            modelBuilder.Entity<Comment>().HasData(
                // Existing Comments
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
                },
                // New Comments
                new Comment
                {
                    CommentId = 5,
                    AnimalId = 5, // Tiger
                    Content = "Tigers are so fierce!",
                },
                new Comment
                {
                    CommentId = 6,
                    AnimalId = 6, // Giraffe
                    Content = "Giraffes have such long necks.",
                },
                new Comment
                {
                    CommentId = 7,
                    AnimalId = 7, // Python
                    Content = "Snakes are fascinating creatures.",
                },
                new Comment
                {
                    CommentId = 8,
                    AnimalId = 8, // Eagle
                    Content = "Eagles symbolize freedom.",
                },
                new Comment
                {
                    CommentId = 9,
                    AnimalId = 9, // Penguin
                    Content = "Penguins are so cute!",
                },
                new Comment
                {
                    CommentId = 10,
                    AnimalId = 10, // Shark
                    Content = "Sharks are misunderstood predators.",
                },
                new Comment
                {
                    CommentId = 11,
                    AnimalId = 11, // Frog
                    Content = "Frogs can jump really high!",
                },
                new Comment
                {
                    CommentId = 12,
                    AnimalId = 12, // Butterfly
                    Content = "Butterflies are so colorful.",
                },
                new Comment
                {
                    CommentId = 13,
                    AnimalId = 5, // Tiger
                    Content = "I love the stripes on tigers.",
                },
                new Comment
                {
                    CommentId = 14,
                    AnimalId = 8, // Eagle
                    Content = "Watching eagles soar is amazing.",
                }
            );
        }
    }
}
