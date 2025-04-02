using Microsoft.EntityFrameworkCore;
using MyUserApi.Models;

namespace MyUserApi.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens { get; set; }

    }
}
