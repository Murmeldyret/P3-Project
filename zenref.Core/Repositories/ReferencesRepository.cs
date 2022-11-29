using Microsoft.EntityFrameworkCore;
using zenref.Core.Models;

namespace zenref.Core.Repositories;

public class ReferencesRepository : Repository<Reference>, IReferencesRepository
{
    public ReferencesRepository(ApplicationContext context) : base(context) {}

    /// <summary>
    /// Using GetAll to get the referencnes, then using FirstOrDefaultAsync to get the first reference that matches the given id.
    /// If the reference is null, an exception will be thrown.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Reference> GetReferenceByIdAsync(int id)
    {
        return await GetAll().FirstOrDefaultAsync(x => x.Id == id) ?? throw new InvalidOperationException();
    }
    
    /// <summary>
    /// Using GetAll to get the references, then using ToListAsync to get a list of all the references.
    /// ToListAsync is an extentions method from EFCore to convert IQueryable to List asynchronously.
    /// </summary>
    /// <returns></returns>
    public async Task<List<Reference>> GetAllReferencesAsync()
    {
        return await GetAll().ToListAsync();
    }
    
    public async Task<Reference> AddReferenceAsync(Reference reference)
    {
        return await AddAsync(reference);
    }
    
    public async Task<Reference> UpdateReferenceAsync(Reference reference)
    {
        return await UpdateAsync(reference);
    }
    
    public async Task<Reference> DeleteReferenceAsync(Reference reference)
    {
        return await DeleteAsync(reference);
    }
}