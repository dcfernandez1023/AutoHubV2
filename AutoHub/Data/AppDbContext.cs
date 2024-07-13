using AutoHub.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoHub.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Vehicle> Vehicle { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Vehicle configuration
            modelBuilder.Entity<Vehicle>().ToTable("Vehicle");
        }
    }
}
