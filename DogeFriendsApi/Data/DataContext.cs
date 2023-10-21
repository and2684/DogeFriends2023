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
        public DbSet<Friendship> Friendships => Set<Friendship>();
        public DbSet<BreedGroup> BreedGroups => Set<BreedGroup>();

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


            // Поработаем с дружбой
            modelBuilder.Entity<Friendship>()
                .HasOne(x => x.User)
                .WithMany(x => x.Friends)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>()
                .HasOne(x => x.Friend)
                .WithMany(x => x.FriendOf)
                .HasForeignKey(x => x.FriendId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Breed>()
                .HasMany(b => b.BreedGroups)
                .WithMany(bg => bg.Breeds);

            modelBuilder.SeedGroupsCoatsAndSizes(); // Добавим группы, размеры собак и виды шерсти по умолчанию
        }
    }
}