using AutoHub.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoHub.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<ScheduledServiceType> ScheduledServiceType { get; set; }
        public DbSet<VehicleSchedule> VehicleSchedule { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurations
            modelBuilder.Entity<Vehicle>().ToTable("Vehicle");
            modelBuilder.Entity<ScheduledServiceType>().ToTable("ScheduledServiceType");
            modelBuilder.Entity<VehicleSchedule>().ToTable("VehicleSchedule");
        }
    }
}
