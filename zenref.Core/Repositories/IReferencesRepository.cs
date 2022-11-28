using zenref.Core.Models;

namespace zenref.Core.Repositories;

public interface IReferencesRepository 
{
    /// <summary>
    /// Gets a reference by its id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Reference> GetReferenceByIdAsync(int id);

    /// <summary>
    /// Gets all of the references.
    /// </summary>
    /// <returns></returns>
    Task<List<Reference>> GetAllReferencesAsync();
}