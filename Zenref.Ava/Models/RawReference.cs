
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Zenref.Ava.Models;
//TODO Erik skal dokumentere metoder :)))))
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
        (string author, string title, int? yearRef) ucnRefAuthorTitleYearRef = UCNRefAuthorTitleYearRef();
        string doi = DoiSearch();
        (string pubType, string source) ucnRefLinks = UCNRefLinks();

        return new Reference(this,
            ucnRefAuthorTitleYearRef.author,
            ucnRefAuthorTitleYearRef.title,
            ucnRefLinks.source,
            "",
            ucnRefAuthorTitleYearRef.yearRef,
            "",
            null, 
            null, 
            "", 
            "", 
            "", 
            "", 
            ucnRefLinks.source);
    }
    
    /// <summary>
    /// Finds a doi identifier in a reference if one is present
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public string DoiSearch()
    {
        string[] textSplit = Regex.Split(OriReference, @"(?:doi: |DOI: |Doi: |doi:|doi.org/)");
        string[] result = textSplit[1].Split(" ");
        return result[0];
    }
    /// <summary>
    /// Splits a string of words into a list of strings with each element being a word.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public List<string> NGramizer(string text)
    {
        return text.Split(' ').ToList();
    }

    /// <summary>
    /// Method for manipulating a raw UCN reference string into smaller, but correct pieces the right places.
    /// This method is not for Antologies or Websites
    /// </summary>
    /// <returns></returns>
    public (string Author, string Title, int? YearRef) UCNRefAuthorTitleYearRef()
    {
        string author, title;
        int? yearRef;
        
        //Last "(?!.*[A-Z+l]\.)" part below takes the last instance in the search
        string[] textAuthor = Regex.Split(OriReference, @"(?:[A-Z+l]\.)(?! &)(?! et)(?!.*[A-Z+l]\.)");
        author = OriReference.Substring(0, textAuthor[0].Length + 2);
        
        //IF THE YEAR DISAPPEARS IT'S BECAUSE I'VE EXCLUDED THE "^" SYMBOL BEFORE THE REGEX STRING!!
        Match m = Regex.Match(textAuthor[1], @"^(?: \([0-9]\d{3}\))");
        if (m.Success)
        {
            (string Author, int? YearRef, string Title, string Source) reference1 = CorrectAPACategorizer();
            return (reference1.Author, reference1.Title, reference1.YearRef);
        }
        else
        {
            string[] textTitleB = Regex.Split(textAuthor[1], @"(?:\.)");
            title = textTitleB[0].Substring(1, textTitleB[0].Length - 1) + ".";
            //Excludes ISSN and year intervals, remove (?<!-) to undo this.
            Regex yearExpression = new Regex(@"(?<!-)(?:[1][9][5-9][0-9]|[2][0][0-3][0-9])(?!-)");
            MatchCollection yearFound = yearExpression.Matches(OriReference);
            yearRef = int.Parse(yearFound[0].Value);

            return (author, title, yearRef);
        }
    }

    /// <summary>
    /// Theoretical correct string manipulation APA style
    /// </summary>
    /// <returns></returns>
    public (string Author, int? yearRef, string Title, string Source) CorrectAPACategorizer()
    {
        // Reference reference = new();
        string author, title, source;
        int? yearRef;
        
        //first case = Correctly inserted reference APA style.
        string[] textAuthor = Regex.Split(OriReference, @"(?:\. \(|\.\()");

        author = textAuthor[0] + ".";
        string[] textYear = Regex.Split(textAuthor[1], @"(\)(.*))");
        //or (?:\)) but make sure to unite the string afterwards
        bool check = Int32.TryParse(textYear[0], out int year);
        if (check)
        {
            yearRef = year;
        } else 
        {
            yearRef = null;
        }
        string[] textTitle = Regex.Split(textYear[1], @"(?:\. )");
        title = textTitle[1] + ".";
        source = textTitle[2];
        //Should there be a need for more details a more advanced method needs to be called.

        return (author, yearRef, title, source);
    }

    /// <summary>
    /// Finds the links to the source.
    /// USE THIS METHOD BEFORE THE OTHER TWO
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public (string? pubType, string? source) UCNRefLinks()
    {
        string pubType, source;
        string[] textSource = Regex.Split(OriReference, @"(?:https://)|(?:http://)");
        if (textSource[1].EndsWith("."))
        {
            string textSourceNoDot = textSource[1].Substring(0, textSource[1].Length - 1);
            source = textSourceNoDot;
        } else
        {
            source = textSource[1];
        }
            
        if(!string.IsNullOrEmpty(source))
        {
            pubType = "Website";
        } else
        {
            pubType = null;
        }

        return (pubType, source);
    }
    private int Fuzzy(string test, string test2)
    {
        int levenshteinDistance = Fastenshtein.Levenshtein.Distance(test, test2);
        return levenshteinDistance;
    }
    
    /// <summary>
    /// Converts the number of operations needed to change one string into another,
    /// into a percentage and allows for better quantification.
    /// </summary>
    /// <param name="newText"></param>
    /// <param name="originalText"></param>
    /// <returns></returns>
    public double MatchingStrings(string newText, string originalText)
    {
        int fuzzy = Fuzzy(newText, originalText);   
        double result = 1 - ((double)fuzzy / (double)originalText.Length);
        return result;
    }
}