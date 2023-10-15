using DogeFriendsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DogeFriendsApi.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void SeedCoatsAndSizes(this ModelBuilder modelBuilder)
        {
            // Виды шерсти
            modelBuilder.Entity<Coat>()
                .HasData(
                    new Coat { Id = 1, Name = "Short" },
                    new Coat { Id = 2, Name = "Medium" },
                    new Coat { Id = 3, Name = "Long" });

            // Размеры собак
            modelBuilder.Entity<Size>()
                .HasData(
                    new Size { Id = 1, Name = "Small" },
                    new Size { Id = 2, Name = "Middle" },
                    new Size { Id = 3, Name = "Big" });
        }
    }
}