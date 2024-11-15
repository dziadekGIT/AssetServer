using Microsoft.EntityFrameworkCore;
using AssetServerAPI.Models;

namespace AssetServerAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
        
        public DbSet<Asset> Assets { get; set; }
    }
}