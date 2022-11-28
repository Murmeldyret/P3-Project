using zenref.Core.Models;

namespace zenref.Core.Services;

public interface IReferencesService
{
    Task<Reference> GetReferenceAsync(int id);
    Task<IEnumerable<Reference>> GetReferencesAsync();
    Task<Reference> AddReferenceAsync(Reference reference);
    Task<Reference> UpdateReferenceAsync(Reference reference);
    Task<Reference> DeleteReferenceAsync(int id);
}