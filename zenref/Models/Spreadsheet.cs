using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace zenref.Models.Spreadsheet
{
    public class Spreadsheet
    {
        private string FileName { get; }
        /// <summary>
        /// Represents the total amount of rows in a spreadsheet
        /// </summary>
        public int ReferenceCount { get; }
        public bool State { get; }
        /// <summary>
        /// Represents the path that the file should be read to.
        /// </summary>
        /// <remarks>Should only be used if new Excel files are to be created</remarks>
        public string FilePath { get; private set; }
        /// <summary>
        /// Represents whether or not <c>_workbook</c> exists or not
        /// </summary>
        public bool DoesExcelFileExist { get => _workbook is not null; }

        private XLWorkbook? _workbook
        {
            get => _workbook ?? throw new FileNotFoundException($"{nameof(_workbook)} is null, use import() to fill this property");
            set => _workbook = value;
        }
        private int _activeSheet { get; set; } = 1;
        private int _currentRow { get; set; } = 2;

        #region ManyFields
        //Reference field positioning
        //These fields represent the column positioning of each Reference field in the Worksheet
        private int AuthorPos       { get; set; } = 1;
        private int TitlePos        { get; set; } = 2;
        private int PubTypePos      { get; set; } = 3;
        private int PublisherPos    { get; set; } = 4;
        private int YearRefPos      { get; set; } = 5;
        private int IDRefPos        { get; set; } = 6;
        private int EduPos          { get; set; } = 7;
        private int LocationPos     { get; set; } = 8;
        private int SemesterPos     { get; set; } = 9;
        private int LanguagePos     { get; set; } = 10;
        private int YearReportPos   { get; set; } = 11;
        private int OriginalRefPos  { get; set; } = 12;
        private int MatchPos        { get; set; } = 13;
        private int CommentaryPos   { get; set; } = 14;
        private int SyllabusPos     { get; set; } = 15;
        private int SeasonPos       { get; set; } = 16;
        private int ExamEventPos    { get; set; } = 17;
        private int SourcePos       { get; set; } = 18;
        private int PagesPos        { get; set; } = 19;
        private int VolumePos       { get; set; } = 20;
        private int ChaptersPos     { get; set; } = 21;
        private int BookTitlePos    { get; set; } = 22;
        //TODO Sikkert en bedre måde at gøre dette, måske en dictionary til at holde styr på det
        #endregion ManyFields

        public Spreadsheet(string fileName)
        {
            FileName = fileName;
            FilePath = "";
        }
        public Spreadsheet(string fileName, string filepath)
        {
            FileName = fileName;
            FilePath = filepath;
        }

        /// <summary>
        /// Gets the position and names of all the worksheets in the workbook
        /// </summary>
        /// <returns>A <c>Dictionary</c> where the key represents the position of the sheet in the book and the value is the name of the sheet.</returns>
        /// <exception cref="FileNotFoundException">Throws if a spreadsheet has not been imported</exception>
        public Dictionary<int,string> GetWorksheets()
        {
            Dictionary<int, string> resDic = new Dictionary<int, string>();

            IXLWorksheet worksheet;

            for (int i = 1; i < _workbook.Worksheets.Count; i++)
            {
                worksheet = _workbook.Worksheet(i);
                resDic.Add(worksheet.Position, worksheet.Name);
            }
            return resDic;
        }

        /// <summary>
        /// Sets the active worksheet to read from or write to. If the sheet does not exist, it creates one.
        /// </summary>
        /// <param name="position">The position of the sheet</param>
        /// <exception cref="ArgumentException">Throws if position is below 0</exception>
        public void SetActiveSheet(int position)
        {
            if (_workbook.Worksheets.Count > position && position != 0)
            {
                _activeSheet = position;
            }
            else if (position <=0)
            {
                throw new ArgumentException("Position of worksheet must be 1 or greater");
            }
            else
            {
                _workbook.Worksheets.Add(DateTime.Now.ToString(), position);
            }
        }
        ///<inheritdoc cref="SetActiveSheet(int)"/>
        /// <param name="sheetname">The name of a sheet</param>
        public void SetActiveSheet(string sheetname)
        {
            Dictionary<int, string> dic = GetWorksheets();
            int actualSheetPos = -1;

            if (dic.ContainsValue(sheetname))
            {
                foreach (KeyValuePair<int,string> pair in dic)
                {
                    if (pair.Value == sheetname)
                    {
                        actualSheetPos = pair.Key;
                    }
                }
            }
            else
            {
                actualSheetPos = _workbook.Worksheets.Count + 1;
                _workbook.AddWorksheet(sheetname, actualSheetPos);
            }
            _activeSheet = actualSheetPos;
        }

        /// <summary>
        /// Reads the next reference in the spreadsheet
        /// </summary>
        /// <returns>The next reference</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Reference ReadRef()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the next references in the spreadsheet, delimited by <paramref name="amount"/>
        /// </summary>
        /// <param name="amount">The amount of references to read, if <paramref name="amount"/> is 0, reads all references in the worksheet.</param>
        /// <returns>List of references from spreadsheet</returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<Reference> ReadRef(uint amount)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if <c>FileName</c> is an Excelfile
        /// </summary>
        /// <returns><c>true</c> if <c>FileName</c>suffix is .xlsx, <c>false</c> otherwise </returns>
        public bool IsFileExcel()
        {
            return (Path.GetExtension(FileName) != ".xlsx");
        }
        /// <summary>
        /// Creates a new, empty workbook, primarily for creating a new spreadsheet with <c>Reference</c>s
        /// </summary>
        public void Create()
        {
            _workbook = new XLWorkbook();
        }

        /// <summary>
        /// Appends <c>reference</c> to the the next row of the first worksheet
        /// </summary>
        /// <param name="reference">The reference to be added</param>
        public void AddReference(Reference reference)
        {
            throw new NotImplementedException("Fuck dig ikke implementeret");
        }

        /// <summary>
        /// Appends multiple <c>references</c> to the next rows of the first worksheet
        /// </summary>
        /// <param name="references">Collection of references to be added</param>
        public void AddReference(IEnumerable<Reference> references)
        {
            foreach (Reference reference in references)
            {
                //AddReference(reference);
                throw new NotImplementedException("Fuck dig ikke implementeret");
            }
        }

        /// <summary>
        /// Reads an Excel file corresponding to <c>FileName</c> and assigns it to <c>Workbook</c>
        /// </summary>
        /// <exception cref="FileNotFoundException">Throws if the file cannot be found</exception>
        public void Import()
        {
            //throw new NotImplementedException();
            try
            {
            _workbook = new XLWorkbook(FileName);
            }
            catch (ArgumentException ex)
            {
                throw new FileNotFoundException("File not found\n" + ex.Message);
            }

        }

        /// <summary>
        /// Saves the spreadsheet as an Excel file with the given name.
        /// </summary>
        /// <param name="filename">The name of the Excelfile. If <paramref name="filename"/> already exists an error is thrown</param>
        /// <exception cref="ArgumentNullException">If Workbook is null, this exception is thrown</exception>
        /// <exception cref="ArgumentException">If a file with <c>filename</c> already exists</exception>
        public void Export(string filename)
        {
            if (_workbook is null)
            {
                throw new ArgumentNullException("Workbook cannot be saved when it is null");
            }
            if (File.Exists(filename))
            {
                throw new ArgumentException("File with this name already exists");
            }
            else
            {
            _workbook.SaveAs(Path.Join(FilePath + filename));
            }
            //throw new NotImplementedException();
            // Export spreadsheet....
            // Window.close(); 
        }
    }
}
