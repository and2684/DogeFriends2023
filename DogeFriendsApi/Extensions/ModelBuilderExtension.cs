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
                    new Coat { Id = 1, Name = "Короткая" },
                    new Coat { Id = 2, Name = "Средняя" },
                    new Coat { Id = 3, Name = "Длинная" });

            // Размеры пород
            modelBuilder.Entity<Size>()
                .HasData(
                    new Size { Id = 1, Name = "Маленькая" },
                    new Size { Id = 2, Name = "Средняя" },
                    new Size { Id = 3, Name = "Большая" });
        }
    }
}