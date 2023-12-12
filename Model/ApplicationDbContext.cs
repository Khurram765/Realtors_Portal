using Microsoft.EntityFrameworkCore;
using Realtors_Portal.Model.Locations;
using System.Diagnostics.Metrics;

namespace Realtors_Portal.Model
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Countries> Countries { get; set; }
        public DbSet<Provinces> Provinces { get; set; }
        public DbSet<Cities> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Provinces>()
                .HasOne(p => p.Countries)
                .WithMany(c => c.Provinces)
                .HasForeignKey(p => p.CountryId);

            modelBuilder.Entity<Cities>()
                .HasOne(c => c.Provinces)
                .WithMany(p => p.Cities)
                .HasForeignKey(c => c.ProvinceId);
        }
    }
}
