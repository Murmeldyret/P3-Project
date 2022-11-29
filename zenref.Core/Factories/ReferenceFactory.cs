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
            Author = "John Doe",
            Title = "P3 gods",
            PubType = "Book",
            Publisher = "P3 Group",
            Year = 2015,
            IDRef = 2,
            Edu = "Software Engineering",
            Location = "Stockholm",
            Semester = "Master",
            Language = "English",
            YearReport = 2020,
            OriReference = "https://www.p3group.se/",
            Match = 0.0,
            Commentary = "ok",
            Syllabus = "ok",
            Season = "ok",
            ExamEvent = "Finale",
            Source = "P3",
            Pages = 200,
            ISBN = "25-25-25",
            Volume = "2",
            Chapters = "55",
            BookTitle = "P3",
        };
    }
}