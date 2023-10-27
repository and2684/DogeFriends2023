using IdentityService.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Data
{
    public class DataContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<DogeFriendsUser> DogeFriendsUsers => Set<DogeFriendsUser>();
    }
}
