namespace Zenref.Ava.Models;

/// <summary>
/// Represents a reference from Raw-data. All properties are required and immutable. 
/// </summary>
public class RawReference
{
    /// <summary>
    /// The field of study/education.
    /// </summary>
    public string Education { get; } 
    /// <summary>
    /// The campus/location.
    /// </summary>
    public string Location { get; }
    /// <summary>
    /// The semester
    /// </summary>
    /// <remarks>Can be represented in numbers or a string</remarks>
    public string Semester { get; }
    /// <summary>
    /// The identifier of a given reference
    /// </summary>
    /// <remarks>Mostly contains numbers, but can contain chars</remarks>
    public string Id { get; }
    /// <summary>
    /// The original reference from a report, contains all data needed to enrich a reference.
    /// </summary>
    public string OriReference { get; }

    /// <summary>
    /// Initializes a RawReference where all properties are required
    /// </summary>
    public RawReference(string education, string location, string semester, string id, string oriReference)
    {
        Education = education;
        Location = location;
        Semester = semester;
        Id = id;
        OriReference = oriReference;
    }

    protected RawReference(RawReference rawReference)
        : this(rawReference.Education, rawReference.Location,
            rawReference.Semester, rawReference.Id, rawReference.OriReference)
    {
        
    }
            
}