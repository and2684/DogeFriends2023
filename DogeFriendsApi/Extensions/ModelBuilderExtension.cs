using DogeFriendsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DogeFriendsApi.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void SeedGroupsCoatsAndSizes(this ModelBuilder modelBuilder)
        {
            // Группы пород
            modelBuilder.Entity<BreedGroup>()
                .HasData(
                    new BreedGroup { Id = 1, Name = "Компаньоны" },
                    new BreedGroup { Id = 2, Name = "Декоративные" },
                    new BreedGroup { Id = 3, Name = "Охотничьи" },
                    new BreedGroup { Id = 4, Name = "Рабочие и служебные" },
                    new BreedGroup { Id = 5, Name = "Пастушьи" },
                    new BreedGroup { Id = 6, Name = "Гончая" },
                    new BreedGroup { Id = 7, Name = "Сторожевые" }
                );

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