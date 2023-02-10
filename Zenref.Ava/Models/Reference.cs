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
    /// <summary>
    /// A class representing a Reference, which inherits from RawReference and implements IEquatable interface.
    /// </summary>
    public class Reference : RawReference, IEquatable<Reference>
    {
        /// <summary>
        /// Constructor for the Reference class, it takes a RawReference object and an optional time of creation as arguments.
        /// </summary>
        /// <param name="rawReference">The RawReference object to be used in creating the Reference object.</param>
        /// <param name="time">The time of creation of the Reference object, it is optional and defaults to null</param>
        public Reference(RawReference rawReference, DateTimeOffset? time = null ):base(rawReference)
        {
            // if time is not null, then assign its value to the TimeOfCreation property
            if (time is not null)
            {
                TimeOfCreation = (DateTimeOffset)time;
            }
        }
        
        /// <summary>
        /// Default constructor for the Reference class
        /// </summary>
        public Reference()
        {

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
        private const double MINIMUMMATCHTHRESHOLD = 0.60; // Bliver vist ikke brugt..

        /// <summary>
        /// An enumeration used to represent the different states a Reference object can be in
        /// </summary>
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
        public bool isIdentified() // Ikke færdig og Bliver ikke brugt...
        {
            // Initialize the state to NotFound
            identificationState state = identificationState.NotFound;
            
            // Create a variable for the identified state in the API
            identificationState identifiedInApi =
                (identificationState.FoundInApi | identificationState.HighMatchThreshold);
            
            // Create a variable for the identified state in the database
            identificationState identifiedInDB = identificationState.FoundInDataBase;
            
            // Check if the TimeOfCreation property has a value
            if (TimeOfCreation.HasValue)
            {
                // Clear the NotFound state
                state &= ~identificationState.NotFound; //  "~" is the bitwise NOT operator. This effectively sets all bits of the "NotFound" state to 0, which means that the state is no longer set to "NotFound".
                
                // Set the state to FoundInApi
                state |= identificationState.FoundInApi;
                
                // Check if the match threshold is greater than or equal to the minimum threshold
                state |= Match >= MINIMUMMATCHTHRESHOLD
                    // If the match threshold is greater than or equal to the minimum threshold, set the state to HighMatchThreshold
                    ? identificationState.HighMatchThreshold
                    // If the match threshold is less than the minimum threshold, set the state to LowMatchThreshold
                    : identificationState.LowMatchThreshold;
            }
            
            // Return true if the state is equal to identified in the API
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