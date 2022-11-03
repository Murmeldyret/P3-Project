namespace zenref.Models
{
    public class Reference
    {
        //constructor with named arguments, where some properties are null by default.
        public Reference(string _Author = "", string _Title = "", string _PubType = "", string _Publisher = "",
                        int _YearRef = 0, int _ID = 0, string _ISBN = "", string _DOI = "", string _Edu = "",
                        string _Location = "", string _Semester = "", string _Language = "", int _YearReport = 0,
                        double _Match = 0.0, string _Commentary = "", string _Syllabus = "", string _Season = "",
                        string _ExamEvent = "", string _Source = "", int _Pages = 0, string _Volume = "", 
                        string _Chapters = "", string _BookTitle = "")
        {
            Author = _Author;
            Title = _Title;
            PubType = _PubType;
            Publisher = _Publisher;
            YearRef = _YearRef;
            ID = _ID;
            ISBN = _ISBN;
            Commentary = _Commentary;
            Syllabus = _Syllabus;
            Season = _Season;
            ExamEvent = _ExamEvent;
            Source = _Source;
            Pages = _Pages;
            Volume = _Volume;
            Chapters = _Chapters;
            BookTitle = _BookTitle;
        }

        public string? Author { get; set; }
        public string? Title { get; set; }
        public string? PubType { get; set; }
        public string? Publisher { get; set; }
        public int? YearRef { get; set; }
        public int? ID { get; set; }
        public string? ISBN { get; set; }
        public string? DOI { get; set; }
        public string? Edu { get; set; }
        public string? Location { get; set; }
        public string? Semester { get; set; }
        public string? Language { get; set; }
        public int? YearReport { get; set; }
        //public string reference { get; set; }
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


        
        public static int FuzzyLocal(string test, string test2)
        {
            throw new NotImplementedException();
        }

        public static int FuzzyOnline(string test, string test2)
        {
            throw new NotImplementedException();
        }

        public static List<string> NGramiser(string text)
        {
            throw new NotImplementedException();
        }
    }
}