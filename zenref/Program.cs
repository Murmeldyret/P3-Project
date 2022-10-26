using System;

namespace P3Project
{
    class Program
    {
        static void Main(string[] args)
        {

        }

    }

    public class Filter
    {
        
    }

    public class Reference
    {

        public static int FuzzyLocal(string test, string test2)
        {
            return 1;
        }

        public static int FuzzyOnline(string test, string test2)
        {
            return 1;
        }

        public static List<string> NGramiser(string text)
        {
            throw new NotImplementedException();
        }

        public string? Author { get; set; }
        public string? Title { get; set; }
        public string? PubType { get; set; }
        public string? Publisher { get; set; }
        public int? YearRef { get; set;}
        public int? ID { get; set; }
        public int? ISBN { get; set; }
        public int? DOI { get; set; }
        public string? Edu { get; set; }
        public string? Location { get; set; }
        public string? Semester { get; set; }
        public string? Language { get; set; }
        public int? YearReport { get; set; }
        //public string reference { get; set; }
        public int? Match { get; set; }
        public string? Commentary { get; set; }
        public string? Syllabus { get; set; }
        public string? Season { get; set; }
        public string? ExamEvent { get; set; }
        public string? Source { get; set; }
        public int? Pages { get; set; }
        public string? Volume { get; set; }
        public string? Chapters { get; set; }
        public string? BookTitle { get; set; }
            
    }

    public class Spreadsheet
    {

    }

    public class Api
    {
        
    }

}