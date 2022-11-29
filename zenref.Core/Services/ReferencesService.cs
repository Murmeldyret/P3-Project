using zenref.Core.Models;
using zenref.Core.Repositories;

namespace zenref.Core.Services;

public sealed class ReferencesService : IReferencesService
{
    private readonly IReferencesRepository _referencesRepository;
    
    public ReferencesService(IReferencesRepository referencesRepository)
    {
        _referencesRepository = referencesRepository;
    }
    
    public async Task<List<Reference>> GetAllReferencesAsync()
    {
        return await _referencesRepository.GetAllReferencesAsync();
    }

    public async Task<Reference> GetReferenceByIdAsync(int id)
    {
        return await _referencesRepository.GetReferenceByIdAsync(id);
    }

    public async Task<Reference> AddReferenceAsync(Reference newReference)
    {
        return await _referencesRepository.AddAsync(newReference);
    }
    
    // public async Task<Reference> GetReferenceByIdAsync(int id)
    // {
    //     return await _referencesRepository.GetReferenceByIdAsync(id);
    // }
    //
    // public async Task<Reference> AddReferenceAsync(Reference reference)
    // {
    //     return await _referencesRepository.AddReferenceAsync(reference);
    // }
    //
    // public async Task<Reference> UpdateReferenceAsync(Reference reference)
    // {
    //     return await _referencesRepository.UpdateReferenceAsync(reference);
    // }
    //
    // public async Task<Reference> DeleteReferenceAsync(Reference reference)
    // {
    //     return await _referencesRepository.DeleteReferenceAsync(reference);
    // }
}