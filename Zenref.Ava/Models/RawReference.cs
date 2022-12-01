
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
        
    }
    
    /// <summary>
    /// Finds a doi identifier in a reference if one is present
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public string DoiSearch(string text)
    {
        string[] textSplit = Regex.Split(text, @"(?:doi: |DOI: |Doi: |doi:|doi.org/)");
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
    /// <param name="text"></param>
    /// <returns></returns>
    public (string Author, string Title, int? YearRef) UCNRefAuthorTitleYearRef(string text)
    {
        Reference reference = new();
        //Last "(?!.*[A-Z+l]\.)" part below takes the last instance in the search
        string[] textAuthor = Regex.Split(text, @"(?:[A-Z+l]\.)(?! &)(?! et)(?!.*[A-Z+l]\.)");
        reference.Author = text.Substring(0, textAuthor[0].Length + 2);

        //IF THE YEAR DISAPPEARS IT'S BECAUSE I'VE EXCLUDED THE "^" SYMBOL BEFORE THE REGEX STRING!!
        Match m = Regex.Match(textAuthor[1], @"^(?: \([0-9]\d{3}\))");
        if (m.Success)
        {
            (string Author, int? YearRef, string Title, string Source) reference1 = CorrectAPACategorizer(text);
            return (reference1.Author, reference1.Title, reference1.YearRef);
        }
        else
        {
            string[] textTitleB = Regex.Split(textAuthor[1], @"(?:\.)");
            reference.Title = textTitleB[0].Substring(1, textTitleB[0].Length - 1) + ".";
            //Excludes ISSN and year intervals, remove (?<!-) to undo this.
            Regex yearExpression = new Regex(@"(?<!-)(?:[1][9][5-9][0-9]|[2][0][0-3][0-9])(?!-)");
            MatchCollection yearFound = yearExpression.Matches(text);
            reference.YearRef = int.Parse(yearFound[0].Value);

            return (reference.Author, reference.Title, reference.YearRef);
        }
    }

    /// <summary>
    /// Theoretical correct string manipulation APA style
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public (string, int?, string, string) CorrectAPACategorizer(string text)
    {
        // Reference reference = new();
        string author, title, source;
        int? yearRef;
        
        //first case = Correctly inserted reference APA style.
        string[] textAuthor = Regex.Split(text, @"(?:\. \(|\.\()");

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
    public (string?, string?) UCNRefLinks(string text)
    {
        Reference reference = new();
        string[] textSource = Regex.Split(text, @"(?:https://)|(?:http://)");
        if (textSource[1].EndsWith("."))
        {
            string textSourceNoDot = textSource[1].Substring(0, textSource[1].Length - 1);
            reference.Source = textSourceNoDot;
        } else
        {
            reference.Source = textSource[1];
        }
            
        if(reference.Source != null)
        {
            reference.PubType = "Website";
        } else
        {
            reference.PubType = null;
        }

        return (reference.PubType, reference.Source);
    }
    public int Fuzzy(string test, string test2)
    {
        int levenshteinDistance = Fastenshtein.Levenshtein.Distance(test, test2);
        return levenshteinDistance;
    }
    
    /// <summary>
    /// Converts the number of operations needed to change one string into another,
    /// into a percentage and allows for better quantification.
    /// </summary>
    /// <param name="shtein"></param>
    /// <param name="originalText"></param>
    /// <returns></returns>
    public double MatchingStrings(int shtein, string originalText)
    {
        double result = 1 - ((double)shtein / (double)originalText.Length);
        return result;
    }
}