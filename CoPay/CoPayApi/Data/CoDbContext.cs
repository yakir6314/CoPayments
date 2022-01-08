using CoPayApi.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoPayApi.Data
{
    public class CoDbContext: IdentityDbContext
    {
        public CoDbContext(DbContextOptions<CoDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
