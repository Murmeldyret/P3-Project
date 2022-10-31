using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Zenref.Ava.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Reference> References { get; set; }
        public DbSet<Spreadsheet> Spreadsheets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=data/zenref.db");
            // optionsBuilder.UseSqlite("Data Source=zenref.db");
        }
    }
}
