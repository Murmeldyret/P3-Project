using zenref.Core.Models;

namespace zenref.Core.Services;

public sealed class ReferencesService : IReferencesService
{
    public Task<Reference> GetReferenceAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Reference>> GetReferencesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Reference> AddReferenceAsync(Reference reference)
    {
        throw new NotImplementedException();
    }

    public Task<Reference> UpdateReferenceAsync(Reference reference)
    {
        throw new NotImplementedException();
    }

    public Task<Reference> DeleteReferenceAsync(int id)
    {
        throw new NotImplementedException();
    }
}