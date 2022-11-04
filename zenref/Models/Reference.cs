namespace zenref.Models
{
    public class Reference
    {
        //constructor with named arguments, where some properties are null by default.
        public Reference(
            KeyValuePair<_typeOfId, string> _UID, 
            string _Author = "", 
            string _Title = "", 
            string _PubType = "",
            string _Publisher = "",
            int _YearRef = 0,
            int _ID = 0,
            string _Edu = "",
            string _Location = "",
            string _Semester = "",
            string _Language = "",
            int _YearReport = 0,
            double _Match = 0.0,
            string _Commentary = "",
            string _Syllabus = "",
            string _Season = "",
            string _ExamEvent = "",
            string _Source = "",
            int _Pages = 0,
            string _Volume = "",
            string _Chapters = "",
            string _BookTitle = ""
            )
        {
            UID = _UID;
            Author = _Author;
            Title = _Title;
            PubType = _PubType;
            Publisher = _Publisher;
            YearRef = _YearRef;
            ID = _ID;
            Edu = _Edu;
            Location = _Location;
            Semester = _Semester;
            Language = _Language;
            YearReport = _YearReport;
            Match = _Match;
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

        public enum _typeOfId
        {
            Unknown,
            DOI,
            ISBN,
            ISSN,
        }
        public KeyValuePair<_typeOfId, string> UID;
        public string? Author { get; set; }
        public string? Title { get; set; }
        public string? PubType { get; set; }
        public string? Publisher { get; set; }
        public int? YearRef { get; set; }
        public int? ID { get; set; }
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


        
        //Based on Levenshteins distance
        public static int Fuzzy(string test, string test2)
        {
            int levenshteinDistance = Fastenshtein.Levenshtein.Distance(test, test2);
            return levenshteinDistance;
            /*
            /// <summary>
            /// Compute the distance between two strings.
            /// This is done through the processes of Replacing, Inserting or Deleting.
            /// The lower the integer, the closer the strings are to each other.
            /// </summary>
            
                int n = test.Length;
                int m = test2.Length;
                int[,] d = new int[n + 1, m + 1];

                // Step 1
                if (n == 0)
                {
                    return m;
                }

                if (m == 0)
                {
                    return n;
                }

                // Step 2 Initialization of a matrix
                for (int i = 0; i <= n; d[i, 0] = i++)
                {
                }

                for (int j = 0; j <= m; d[0, j] = j++)
                {
                }

                // Step 3
                for (int i = 1; i <= n; i++)
                {
                    //Step 4
                    for (int j = 1; j <= m; j++)
                    {
                        // Step 5
                        int cost = (test2[j - 1] == test[i - 1]) ? 0 : 1;

                        // Step 6 finds the lowest number of operations to change the string
                        // throughout the string comparisons
                        d[i, j] = Math.Min(
                            Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                            d[i - 1, j - 1] + cost);
                    }
                }
                // Step 7 
                return d[n, m];*/
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
    }
}