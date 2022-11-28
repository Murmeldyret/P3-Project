namespace zenref.Core.Repositories;

public interface IRepository<TEntity> where TEntity : class, new()
{
    /// <summary>
    /// Gets a reference by its id.
    /// </summary>
    /// <returns></returns>
    IQueryable<TEntity> GetAll();

    /// <summary>
    /// Takes one parameter of a generic type and returns a Task of the same type.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> AddAsync(TEntity entity);

    /// <summary>
    /// Takes one parameter of a generic type and returns a Task of the same type.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> UpdateAsync(TEntity entity);
}