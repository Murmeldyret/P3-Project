using Microsoft.EntityFrameworkCore;

namespace Zenref.Ava.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Reference> References { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlite("FileName=zenref.db");
            optionsBuilder.UseSqlite("FileName=zenref.db", optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly("Zenref.Ava");
            });

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reference>().ToTable("Reference");
            base.OnModelCreating(modelBuilder);
        }
    }
}
