using zenref.Core.Models;

namespace zenref.Core.Repositories;

public interface IReferencesRepository 
{
    /// <summary>
    /// Gets all of the references.
    /// </summary>
    /// <returns></returns>
    Task<List<Reference>> GetAllReferencesAsync();
    
    /// <summary>
    /// Gets a reference by its id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Reference> GetReferenceByIdAsync(int id);
    
    Task<Reference> AddReferenceAsync(Reference reference);
    
    Task<Reference> UpdateReferenceAsync(Reference reference);
    
    Task<Reference>DeleteReferenceAsync(Reference reference);
}