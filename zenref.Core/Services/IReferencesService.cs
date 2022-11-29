using zenref.Core.Models;

namespace zenref.Core.Services;

public interface IReferencesService
{
    Task<Reference> GetReferenceByIdAsync(int id);
    Task<Reference> AddReferenceAsync(Reference reference);
    Task<Reference> UpdateReferenceAsync(Reference reference);
    Task<Reference> DeleteReferenceAsync(Reference reference);
}