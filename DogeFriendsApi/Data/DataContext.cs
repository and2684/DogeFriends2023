using DogeFriendsApi.Interfaces;
using DogeFriendsApi.Models;
using DogeFriendsApi.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DogeFriendsApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Dog> Dogs => Set<Dog>();
        public DbSet<Breed> Breeds => Set<Breed>();
        public DbSet<Coat> Coats => Set<Coat>();
        public DbSet<Size> Sizes => Set<Size>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dog>()
                .HasOne(c => c.User)
                .WithMany(s => s.Dogs)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Breed>()
                .HasMany(c => c.Dogs)
                .WithOne(s => s.Breed)
                .HasForeignKey(k => k.BreedId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Coat>()
                .HasMany(c => c.Breeds)
                .WithOne(s => s.Coat)
                .HasForeignKey(k => k.CoatId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Size>()
                .HasMany(c => c.Breeds)
                .WithOne(s => s.Size)
                .HasForeignKey(k => k.SizeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.SeedCoatsAndSizes(); // Добавим размеры собак и виды шерсти по умолчанию
        }

        // Метод для конфигурации DbContext - лезем в сторонний сервис, где хранятся настройки и оттуда получаем строку подключения к БД
        public static void ConfigureDbContext(IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options =>
            {
                var settingsService = services.BuildServiceProvider().GetRequiredService<ISettingsService>();
                var connectionString = settingsService.GetConnectionStringAsync(config.GetSection("SettingsService").GetValue<string>("ConnectionStringSettingKey")!).GetAwaiter().GetResult();

                options.UseSqlServer(connectionString);
            });
        }
    }
}