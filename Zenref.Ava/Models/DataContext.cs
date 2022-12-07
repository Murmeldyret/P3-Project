using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Zenref.Ava.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Reference> References => Set<Reference>();

        //private string  DbPath { get; }

        //public DataContext()
        //{
            
        //}
        //DbPath = Path.Combine(Environment.CurrentDirectory, "Zenref1.db");
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={Path.Combine(Environment.CurrentDirectory, "Zenref1.db")}");
        
            // optionsBuilder.UseSqlite("FileName=zenref.db");
            //optionsBuilder.UseSqlite("FileName=zenref.db", optionsBuilder =>
            //{
            //    optionsBuilder.MigrationsAssembly("Zenref.Ava");
            //});

            //base.OnConfiguring(optionsBuilder);
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reference>().ToTable("Reference");
            base.OnModelCreating(modelBuilder);
        }




        /// <summary>
        /// Instead of reading the database for every instance of reference
        /// this method allows the database to transfer to memory for better
        /// performance
        /// </summary>
        /// <returns></returns>
        /// <exception cref="OutOfMemoryException"></exception>
        public static List<Reference> FromDBToMemory()
        {
            DataContext context = new();
            const int ONEGB = 1000000000;
            var BasePath = AppDomain.CurrentDomain.BaseDirectory;
            var dbPath = Path.Combine(BasePath, "zenref.db");
            FileInfo DB = new FileInfo(dbPath);
            long Size = DB.Length;

            if (Size > ONEGB)
            {
                List<Reference> MemRef = context.References.ToList();
                return MemRef;
            }
            else
            {
                throw new OutOfMemoryException("File too large to use in memory");
            }

        }
    }
}
