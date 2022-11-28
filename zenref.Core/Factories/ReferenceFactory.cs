using zenref.Core.Models;

namespace zenref.Core.Factories;

public sealed class ReferenceFactory
{
    /// <summary>
    /// Creates a new reference with specified values.
    /// </summary>
    /// <returns></returns>
    public Reference CreateReference()
    {
        return new Reference
        {
            Title = "P3 gods",
            Author = "John Doe",
        };
    }
}