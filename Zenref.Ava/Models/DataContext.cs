using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Zenref.Ava.Models
{
    public class DataContext : DbContext
    {
        // DbSet for the Reference table
        public DbSet<Reference> References => Set<Reference>();
        
        // Configure the database connection
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={Path.Combine(Environment.CurrentDirectory, "Zenref1.db")}");

        // Configures the database model - The modelbuilder to configure
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map the Reference classs to the "Reference" table in the database.
            modelBuilder.Entity<Reference>().ToTable("Reference");
            base.OnModelCreating(modelBuilder);
        }
        
        // /// <summary>
        // /// Instead of reading the database for every instance of reference
        // /// this method allows the database to transfer to memory for better
        // /// performance
        // /// </summary>
        // /// <returns></returns>
        // /// <exception cref="OutOfMemoryException"></exception>
        // public static List<Reference> FromDBToMemory()
        // {
        //     DataContext context = new();
        //     const int ONEGB = 1000000000;
        //     var BasePath = AppDomain.CurrentDomain.BaseDirectory;
        //     var dbPath = Path.Combine(BasePath, "zenref.db");
        //     FileInfo DB = new FileInfo(dbPath);
        //     long Size = DB.Length;
        //
        //     if (Size > ONEGB)
        //     {
        //         List<Reference> MemRef = context.References.ToList();
        //         return MemRef;
        //     }
        //     else
        //     {
        //         throw new OutOfMemoryException("File too large to use in memory");
        //     }
        //
        // }
    }
}