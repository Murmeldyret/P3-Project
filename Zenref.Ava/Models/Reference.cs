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
            // Identified = (FoundInApi | HighMatchThreshold) | FoundInDataBase,
        }

        
        /// <summary>
        /// Determines whether or not a Reference is considered identified
        /// </summary>
        /// <returns>True if the Reference is identified, false if not and needs to be reviewed manually</returns>
        public bool isIdentified()
        {
            identificationState state = identificationState.NotFound;
            identificationState identifiedInApi =
                (identificationState.FoundInApi | identificationState.HighMatchThreshold);
            
            identificationState identifiedInDB = identificationState.FoundInDataBase;
            
            if (TimeOfCreation.HasValue)
            {
                state &= ~identificationState.NotFound;
                state |= identificationState.FoundInApi;
                state |= Match >= MINIMUMMATCHTHRESHOLD
                    ? identificationState.HighMatchThreshold
                    : identificationState.LowMatchThreshold;
            }

            return (state | identifiedInApi) == identifiedInApi;
            // return identified.HasFlag(~state & identified);
            // return ((state & identificationState.Identified) == identificationState.Identified);

        }
        
        /// <summary>
        /// This method compares the current Reference object to another Reference object
        /// It returns true if all of the properties of the two Reference objects are equal, otherwise it returns false
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Reference? other)
        {
            // Return false if the other Reference object is null
            if (other is null)
                return false;
            
            // Compare the properties of the current Reference object to the other Reference object
            // Return true if all of the properties are equal, otherwise return false
            return this.Author == other.Author
                   && this.Title == other.Title
                   && this.PubType == other.PubType
                   && this.Publisher == other.Publisher
                   && this.YearRef == other.YearRef
                   && this.RefId == other.RefId
                   && this.Education == other.Education
                   && this.Location == other.Location
                   && this.Semester == other.Semester
                   && this.Language == other.Language
                   && this.YearReport == other.YearReport
                   && this.Match == other.Match
                   && this.Commentary == other.Commentary
                   && this.Syllabus == other.Syllabus
                   && this.Season == other.Season
                   && this.ExamEvent == other.ExamEvent
                   && this.Source == other.Source
                   && this.Pages == other.Pages
                   && this.Volume == other.Volume
                   && this.Chapters == other.Chapters
                   && this.BookTitle == other.BookTitle;
        }
    }
}