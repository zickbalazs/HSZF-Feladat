using L072NS_HSZF_2024251.Model;
using Microsoft.EntityFrameworkCore;

namespace L072NS_HSZF_2024251.Persistence.MsSql
{
    public class BusContext : DbContext
    {
        public DbSet<Region> Regions { get; set; }
        public DbSet<Route> Routes { get; set; }

        public BusContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Region>()
                .HasMany(e => e.Routes)
                .WithOne()
                .HasForeignKey(e => e.RegionId)
                .OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=NLB;Integrated Security=True");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
