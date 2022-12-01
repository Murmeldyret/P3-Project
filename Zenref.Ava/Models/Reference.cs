using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace Zenref.Ava.Models
{
    public class Reference : RawReference, IEquatable<Reference>
    {
        //constructor with named arguments, where some properties are null by default.

        public Reference(RawReference rawReference, DateTimeOffset? time = null ):base(rawReference)
        {
            if (time is not null)
            {
                TimeOfCreation = (DateTimeOffset)time;
            }
        }

        public Reference(RawReference rawReference,
            string author = "",
            string title = "",
            string pubType = "",
            string publisher = "",
            int? yearRef = null,
            string language = "",
            int? yearReport = null,
            double? match = null,
            string comment = "",
            string syllabus = "",
            string season = "",
            string examEvent = "",
            string source = "",
            int? pages = null,
            string volume = "",
            string chapters = "",
            string bookTitle = "",
            DateTimeOffset? time = null
        ) : this(rawReference,time)
        {
            Author = author;
            Title = title;
            PubType = pubType;
            Publisher = publisher;
            YearRef = yearRef;
            Language = language;
            YearReport = yearReport;
            Match = match;
            Commentary = comment;
            Syllabus = syllabus;
            Season = season;
            ExamEvent = examEvent;
            Source = source;
            Pages = pages;
            Volume = volume;
            Chapters = chapters;
            BookTitle = bookTitle;
        }
        // public Reference() { }
        // public Reference(
        //     string _Author = "",
        //     string _Title = "",
        //     string _PubType = "",
        //     string _Publisher = "",
        //     int? _YearRef = null,
        //     int? _ID = null,
        //     string _Edu = "",
        //     string _Location = "",
        //     string _Semester = "",
        //     string _Language = "",
        //     int? _YearReport = null,
        //     double? _Match = null,
        //     string _Commentary = "",
        //     string _Syllabus = "",
        //     string _Season = "",
        //     string _ExamEvent = "",
        //     string _Source = "",
        //     int? _Pages = null,
        //     string _Volume = "",
        //     string _Chapters = "",
        //     string _BookTitle = "",
        //     string _OriReference = ""
        //     )
        // {
        //     Author = _Author;
        //     Title = _Title;
        //     PubType = _PubType;
        //     Publisher = _Publisher;
        //     YearRef = _YearRef;
        //     ID = _ID;
        //     Edu = _Edu;
        //     Language = _Language;
        //     YearReport = _YearReport;
        //     Match = _Match;
        //     Commentary = _Commentary;
        //     Syllabus = _Syllabus;
        //     Season = _Season;
        //     ExamEvent = _ExamEvent;
        //     Source = _Source;
        //     Pages = _Pages;
        //     Volume = _Volume;
        //     Chapters = _Chapters;
        //     BookTitle = _BookTitle;
        // }
        
        // [Obsolete("KeyValuePair is deprecated for now")]
        // public Reference(
        //     KeyValuePair<_typeOfId, string> _UID,
        //     string _Author = "",
        //     string _Title = "",
        //     string _PubType = "",
        //     string _Publisher = "",
        //     int? _YearRef = null,
        //     int? _ID = null,
        //     string _Edu = "",
        //     string _Location = "",
        //     string _Semester = "",
        //     string _Language = "",
        //     int? _YearReport = null,
        //     double? _Match = null,
        //     string _Commentary = "",
        //     string _Syllabus = "",
        //     string _Season = "",
        //     string _ExamEvent = "",
        //     string _Source = "",
        //     int? _Pages = null,
        //     string _Volume = "",
        //     string _Chapters = "",
        //     string _BookTitle = ""
        //     )
        // {
        //     UID = _UID;
        //     Author = _Author;
        //     Title = _Title;
        //     PubType = _PubType;
        //     Publisher = _Publisher;
        //     YearRef = _YearRef;
        //     ID = _ID;
        //     Edu = _Edu;
        //     Location = _Location;
        //     Semester = _Semester;
        //     Language = _Language;
        //     YearReport = _YearReport;
        //     Match = _Match;
        //     Commentary = _Commentary;
        //     Syllabus = _Syllabus;
        //     Season = _Season;
        //     ExamEvent = _ExamEvent;
        //     Source = _Source;
        //     Pages = _Pages;
        //     Volume = _Volume;
        //     Chapters = _Chapters;
        //     BookTitle = _BookTitle;
        // }
        [Obsolete("KeyValuePair is deprecated for now")]
        public enum _typeOfId
        {
            Unknown,
            DOI,
            ISBN,
            ISSN,
        }
        [Obsolete("KeyValuePair is deprecated for now")]
        public KeyValuePair<_typeOfId, string> UID;
        /// <summary>
        /// Contains a value if this reference has been returned by an API response.
        /// </summary>
        public Optional<DateTimeOffset> TimeOfCreation { get; }
        public string? Author { get; set; }
        public string? Title { get; set; }
        public string? PubType { get; set; }
        public string? Publisher { get; set; }
        public int? YearRef { get; set; }
        public string? Language { get; set; }
        public int? YearReport { get; set; }
        public double? Match { get; set; }
        public string? Commentary { get; set; }
        public string? Syllabus { get; set; }
        public string? Season { get; set; }
        public string? ExamEvent { get; set; }
        public string? Source { get; set; }
        public int? Pages { get; set; }
        public string? Volume { get; set; }
        public string? Chapters { get; set; }
        public string? BookTitle { get; set; }

        //Method searches through APA ref for DOI.
        /// <summary>
        /// Searches a string for the word "doi:" and isolate the doi number for it's own spot.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string DOISearch(string text)
        {
            string[] textsplit = Regex.Split(text, @"(?:doi: |DOI: |Doi: |doi:|doi.org/)");
            string[] result = textsplit[1].Split(" ");
            return result[0];
        }

        //Method for percentage of Fastenshtein.
        /// <summary>
        /// Converts the number of operations needed to change one string into another,
        /// into a percentage and allows for better quantification.
        /// </summary>
        /// <param name="Shtein"></param>
        /// <param name="OriginalText"></param>
        /// <returns></returns>
        public static double MatchingStrings(int Shtein, string OriginalText)
        {
            double result = 1 - ((double)Shtein / (double)OriginalText.Length);
            return result;
        }

        /// <summary>
        /// Compares each public property of two references and checks if their value is equal
        /// </summary>
        /// <param name="other">The other reference to compare</param>
        /// <returns><c>true</c> if all properties are the same, <c>false</c> otherwise.</returns>
        /// <remarks>If other public properties are added, this method will need to be updated.</remarks>
        public bool Equals(Reference? other)
        {
            bool isEqual = false;
            if (other is null)
            {
                return false;
            }

            bool AuthorEquals = this.Author == other.Author;
            bool TitleEquals = this.Title == other.Title;
            bool PubTypeEquals = this.PubType == other.PubType;
            bool PublisherEquals = this.Publisher == other.Publisher;
            bool YearRefEquals = this.YearRef == other.YearRef;
            bool IdEquals = this.Id == other.Id;
            bool EduEquals = this.Education == other.Education;
            bool LocationEquals = this.Location == other.Location;
            bool SemesterEquals = this.Semester == other.Semester;
            bool LanguageEquals = this.Language == other.Language;
            bool YearReportEquals = this.YearRef == other.YearRef;
            bool MatchEquals = this.Match == this.Match;
            bool CommentaryEquals = this.Commentary == other.Commentary;
            bool SyllabusEquals = this.Syllabus == other.Syllabus;
            bool SeasonEquals = this.Season == other.Season;
            bool ExamEventEquals = this.ExamEvent == other.ExamEvent;
            bool SourceEquals = this.Source == other.Source;
            bool PagesEquals = this.Pages == other.Pages;
            bool VolumeEquals = this.Volume == other.Volume;
            bool ChaptersEquals = this.Chapters == other.Chapters;
            bool BookTitleEquals = this.BookTitle == other.BookTitle;

            isEqual = AuthorEquals
                      && TitleEquals
                      && PubTypeEquals
                      && PublisherEquals
                      && YearRefEquals
                      && EduEquals
                      && LocationEquals
                      && SemesterEquals
                      && LanguageEquals
                      && YearReportEquals
                      && MatchEquals
                      && CommentaryEquals
                      && SyllabusEquals
                      && SeasonEquals
                      && ExamEventEquals
                      && SourceEquals
                      && PagesEquals
                      && VolumeEquals
                      && ChaptersEquals
                      && BookTitleEquals;

            return isEqual;
        }
        //split sentences into individual words. NOTICE IT ONLY TAKES 1 STRING AS INPUT. NOT A LIST!
        /// <summary>
        /// Splits a string of words into a list of strings with each element being a word.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static List<string> NGramiser(string text)
        {
            return text.Split(' ').ToList();
        }

        /// <summary>
        /// Method for manipulating a raw UCN reference string into smaller, but correct pieces the right places.
        /// This method is not for Antologies or Websites
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static (string Author, string Title, int? YearRef) UCNRefAuthorTitleYearRef(string text)
        {
            Reference reference = new();
            //Last "(?!.*[A-Z+l]\.)" part below takes the last instance in the search
            string[] TextAuthor = Regex.Split(text, @"(?:[A-Z+l]\.)(?! &)(?! et)(?!.*[A-Z+l]\.)");
            reference.Author = text.Substring(0, TextAuthor[0].Length + 2);

            //IF THE YEAR DISAPPEARS IT'S BECAUSE I'VE EXCLUDED THE "^" SYMBOL BEFORE THE REGEX STRING!!
            Match m = Regex.Match(TextAuthor[1], @"^(?: \([0-9]\d{3}\))");
            if (m.Success)
            {
                (string Author, int? YearRef, string Title, string Source) reference1 = CorrectAPACategorizer(text);
                return (reference1.Author, reference1.Title, reference1.YearRef);
            }
            else
            {
                string[] TextTitleB = Regex.Split(TextAuthor[1], @"(?:\.)");
                reference.Title = TextTitleB[0].Substring(1, TextTitleB[0].Length - 1) + ".";
                //Excludes ISSN and year intervals, remove (?<!-) to undo this.
                Regex YearExpression = new Regex(@"(?<!-)(?:[1][9][5-9][0-9]|[2][0][0-3][0-9])(?!-)");
                MatchCollection YearFound = YearExpression.Matches(text);
                reference.YearRef = Int32.Parse(YearFound[0].Value);

                return (reference.Author, reference.Title, reference.YearRef);
            }
        }

        /// <summary>
        /// Theoretical correct string manipulation APA style
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static (string, int?, string, string) CorrectAPACategorizer(string text)
        {
            Reference reference = new();
            //first case = Correctly inserted reference APA style.
            string[] TextAuthor = Regex.Split(text, @"(?:\. \(|\.\()");

            reference.Author = TextAuthor[0] + ".";
            string[] TextYear = Regex.Split(TextAuthor[1], @"(\)(.*))");
            //or (?:\)) but make sure to unite the string afterwards
            bool check = Int32.TryParse(TextYear[0], out int Year);
            if (check)
            {
                reference.YearRef = Year;
            } else 
            {
                reference.YearRef = null;
            }
            string[] TextTitle = Regex.Split(TextYear[1], @"(?:\. )");
            reference.Title = TextTitle[1] + ".";
            reference.Source = TextTitle[2];
            //Should there be a need for more details a more advanced method needs to be called.

            return (reference.Author, reference.YearRef, reference.Title, reference.Source);
        }
        /// <summary>
        /// Finds the links to the source.
        /// USE THIS METHOD BEFORE THE OTHER TWO
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static (string?, string?) UCNRefLinks(string text)
        {
            Reference reference = new();
            string[] TextSource = Regex.Split(text, @"(?:https://)|(?:http://)");
            if (TextSource[1].EndsWith("."))
            {
                string TextSourceNoDot = TextSource[1].Substring(0, TextSource[1].Length - 1);
                reference.Source = TextSourceNoDot;
            } else
            {
                reference.Source = TextSource[1];
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

        //Based on Levenshteins distance
        public static int Fuzzy(string test, string test2)
        {
            int levenshteinDistance = Fastenshtein.Levenshtein.Distance(test, test2);
            return levenshteinDistance;
        }
        
    }
}