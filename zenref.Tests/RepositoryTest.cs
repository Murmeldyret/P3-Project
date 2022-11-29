using System.Threading.Tasks;
using Xunit;
using zenref.Core.Factories;
using zenref.Core.Models;
using zenref.Core.Services;

namespace zenref.Tests;

public class RepositoryTest
{
    private readonly IReferencesService _referencesService;
    
    [Fact]
    public void CreateReference()
    {
        ReferenceFactory referenceFactory = new();
        Reference book = referenceFactory.CreateReference();
        
        _referencesService.AddReferenceAsync(book);
        
        Assert.True(book.Id > 0);
    }
}