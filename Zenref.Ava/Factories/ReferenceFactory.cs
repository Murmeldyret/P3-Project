using Zenref.Ava.Models;

namespace Zenref.Ava.Factories;

/// <summary>
/// The ReferenceFactory class is used to create new Reference objects
/// </summary>
public class ReferenceFactory
{
    /// <summary>
    ///  Creates a new Reference object and sets its default values
    /// </summary>
    /// <returns></returns>
    public Reference CreateReference()
    {
        var rawReference = CreateRawReference();
        
        // Create a new Reference object
        Reference reference = new Reference(rawReference, 
            author: "author",
            title: "title",
            pubType: "pubType",
            publisher: "Publisher",
            yearRef: 14,
            language: "Language",
            yearReport: 414,
            match: 41,
            comment: "Commentary",
            syllabus: "Syllabus",
            season: "Season",
            examEvent: "ok",
            source: "Source",
            pages: 14,
            volume: "Volume",
            chapters: "14",
            bookTitle: "BookTitle"
        );

        // Return the Reference object
        return reference;
    }

    public Reference CreateReference(string author, string title, string pubType, string publisher, int yearRef,
        string language, int yearReport, int match, string comment, string syllabus, string season, string examEvent,
        string source, int pages, string volume, string chapters, string bookTitle)
    {
        var rawReference = CreateRawReference();

        // Create a new Reference object
        Reference reference = new Reference(rawReference, author: author, title: title, pubType: pubType,
            publisher: publisher, yearRef: yearRef, language: language, yearReport: yearReport, match: match,
            comment: comment, syllabus: syllabus, season: season, examEvent: examEvent, source: source, pages: pages,
            volume: volume, chapters: chapters, bookTitle: bookTitle);
        
        // Return the Reference object
        return reference;
    }

    /// <summary>
    /// creates a new RawReference object and sets its default values
    /// </summary>
    /// <returns></returns>
    public RawReference CreateRawReference()
    {
        // Create a new RawReference object
        RawReference rawReference = new RawReference
        {
            // Set the default values for the properties of the RawReference object
            Education = "Education",
            Location = "Location",
            Semester = "Semester",
            RefId = "RefId",
            OriReference = "OriReference"
        };

        // Return the RawReference object
        return rawReference;
    }
    
    public RawReference CreateRawReference(string education, string location, string semester, string refId, string oriReference)
    {
        // Create a new RawReference object
        RawReference rawReference = new RawReference
        {
            // Set the default values for the properties of the RawReference object
            Education = education,
            Location = location,
            Semester = semester,
            RefId = refId,
            OriReference = oriReference
        };

        // Return the RawReference object
        return rawReference;
    }
}
