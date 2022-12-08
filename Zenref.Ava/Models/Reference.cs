using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Zenref.Ava.Models
{
    public class Reference : RawReference, IEquatable<Reference>
    {
        public Reference(RawReference rawReference, DateTimeOffset? time = null ):base(rawReference)
        {
            if (time is not null)
            {
                TimeOfCreation = (DateTimeOffset)time;
            }
        }
        
        public Reference()
        {

        }

        public Reference(RawReference rawReference,
            // int id,
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
            // Id = id;
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
        /// The time at which this object returned by API response if present.
        /// </summary>
        [Key]
        public int Id { get; set; }
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

        /// <summary>
        /// Represents the minimum percentage match for a reference to be considered properly identified.
        /// </summary>
        private const double MINIMUMMATCHTHRESHOLD = 0.60;
        
        [Flags]
        private enum identificationState
        {
            None = 0,
            Raw = 1,
            NotFound = 1 << 1, // 2
            LowMatchThreshold = 1 << 2, // 4
            HighMatchThreshold = 1 << 3, // 8
            FoundInApi = 1 << 4, // 16
            FoundInDataBase = 1 << 5, // 32
            
            // ManualReview = Raw | NotFound | (FoundInApi & LowMatchThreshold),
            // Identified = (FoundInApi & HighMatchThreshold) | FoundInDataBase,
        }

        
        /// <summary>
        /// Determines whether or not a Reference is considered identified
        /// </summary>
        /// <returns>True if the Reference is identified, false if not and needs to be reviewed manually</returns>
        private bool isIdentified()
        {
            identificationState state = identificationState.None;
            identificationState identified = (identificationState.FoundInApi & identificationState.HighMatchThreshold) |
                                             identificationState.FoundInDataBase;
            if (TimeOfCreation.HasValue || TimeOfCreation.Value != null)
            {
                state |= identificationState.FoundInApi;
                state |= Match >= MINIMUMMATCHTHRESHOLD
                    ? identificationState.HighMatchThreshold
                    : identificationState.LowMatchThreshold;
            }


            return state == identified;
            // return ((state & identificationState.Identified) == identificationState.Identified);

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
            bool IdEquals = this.Id == other.Id;
            bool AuthorEquals = this.Author == other.Author;
            bool TitleEquals = this.Title == other.Title;
            bool PubTypeEquals = this.PubType == other.PubType;
            bool PublisherEquals = this.Publisher == other.Publisher;
            bool YearRefEquals = this.YearRef == other.YearRef;
            bool RefIdEquals = this.RefId == other.RefId;
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

            isEqual = IdEquals
                      && AuthorEquals
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



    }
}