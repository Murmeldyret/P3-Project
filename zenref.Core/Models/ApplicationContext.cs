using Microsoft.EntityFrameworkCore;

namespace zenref.Core.Models;

public sealed class ApplicationContext : DbContext
{
    public DbSet<Reference> References => Set<Reference>();

    private string DbPath { get; }
    
    /// <summary>
    /// Creates a new instance of the <see cref="ApplicationContext"/> class.
    /// </summary>
    public ApplicationContext()
    {
        var basePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        DbPath = Path.Combine(basePath, "references.db3");
    }
    
    /// <summary>
    /// Specifies the database and provider to use.
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data source={DbPath}");


    /// <summary>
    /// When the model is being created, this method is called.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reference>().ToTable("References");
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
        ApplicationContext context = new();
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