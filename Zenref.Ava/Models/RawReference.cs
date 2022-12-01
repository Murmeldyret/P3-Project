
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Zenref.Ava.Models;

/// <summary>
/// Represents a reference from Raw-data. All properties are required and immutable. 
/// </summary>
public class RawReference : IEquatable<RawReference>
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

    
    public bool Equals(RawReference? other)
    {
        bool isEqual = false;
        if (other is null)
        {
            return isEqual;
        }

        bool educationEquals = this.Education == other.Education;
        bool locationEquals = this.Location == other.Location;
        bool semesterEquals = this.Semester == other.Semester;
        bool idEquals = this.Id == other.Id;
        bool oriReferenceEquals = this.OriReference == other.OriReference;

        isEqual = educationEquals
                  && locationEquals
                  && semesterEquals
                  && idEquals
                  && oriReferenceEquals;
        
        return isEqual;
    }

    /// <summary>
    /// Extracts data from the original reference and returns an enriched Reference allowing searching by several properties
    /// </summary>
    /// <returns>An enriched reference with filled fields</returns>
    public Reference ExtractData()
    {
        
    }
}