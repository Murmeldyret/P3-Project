using zenref.Core.Models;

namespace zenref.Core.Repositories;

public class Repository<TEntitiy> : IRepository<TEntitiy> where TEntitiy : class, new()
{
    private readonly ApplicationContext _context;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    protected Repository(ApplicationContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Generic method to get all of the entities. Method returns a IQueryable which is a collection of entities.
    /// The method retrieves the entities from the database using the DbSet property of the context.
    /// </summary>
    /// <returns></returns>
    public IQueryable<TEntitiy> GetAll()
    {
        try
        {
            // Returns an IQueryable collection of entities.
            return _context.Set<TEntitiy>();
        }
        catch (Exception e)
        {
            throw new Exception($"Could not retrieve entities: {e.Message}");
        }
    }

    /// <summary>
    /// Generic method that accepts an entity of type TEntitiy and returns a Task of type TEntitiy.
    /// Used to add an entity to the database.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<TEntitiy> AddAsync(TEntitiy entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException($"{nameof(AddAsync)} the entity cannot be null");
        }

        try
        {
            // await _context.Set<TEntitiy>().AddAsync(entity);
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            
            return entity;
        }
        catch (Exception e)
        {
            throw new ArgumentNullException($"{nameof(AddAsync)} It could not be saved to the database: {e.Message}");
        }
    }
    
    /// <summary>
    /// Generic method that accepts an entity of type TEntitiy and returns a Task of type TEntitiy.
    /// Used to update an entity in the database.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<TEntitiy> UpdateAsync(TEntitiy entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException($"{nameof(UpdateAsync)} the entity cannot be null");
        }

        try
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            
            return entity;
        }
        catch (Exception e)
        {
            throw new ArgumentNullException($"{nameof(UpdateAsync)} Could not be updated in the database : {e.Message}");
        }
    }
}