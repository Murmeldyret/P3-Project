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
        //TODO slet
        public bool State { get; }

        /// <summary>
        /// Represents the path that the file should be read to.
        /// </summary>
        /// <remarks>Should only be used if new Excel files are to be created</remarks>
        public string FilePath { get; private set; }

        /// <summary>
        /// Represents whether or not <c>_workbook</c> exists or not
        /// </summary>
        public bool DoesExcelFileExist { get => _workbookProperty is not null; }

        private XLWorkbook? _workbook {get; set;}

        private XLWorkbook? _workbookProperty
        {
            get => _workbook ?? throw new FileNotFoundException($"{nameof(_workbook)} is null, use import() to fill this property");
            set => _workbook = value;
        }
        public int ActiveSheet { get; set; } = 1;
        private IXLWorksheet xLWorksheet { get => _workbook.Worksheet(ActiveSheet); }
        private int _currentRow { get; set; } = 2;

        /// <summary>
        /// Represents the different fields that a reference instance contains.
        /// </summary>
        public enum _referenceFields
        {
            Author,
            Title,
            PublicationType,
            Publisher,
            YearRef,
            IdRef,
            Education,
            Location,
            Semester,
            Language,
            YearReport,
            OriginalRef,
            Match,
            Comment,
            Syllabus,
            Season,
            ExamEvent,
            Source,
            Pages,
            Volume,
            Chapters,
            BookTitle,
            Isbn,
        }

        /// <summary>
        /// Represents the different fields in an Excel worksheet where the key is the column position and the value is the content
        /// </summary>
        public SortedDictionary<int, _referenceFields> _positionOfReferencesInSheet = new SortedDictionary<int,_referenceFields>()
        {
            {1,_referenceFields.Author },
            {2,_referenceFields.Title },
            {3,_referenceFields.PublicationType },
            {4,_referenceFields.Publisher },
            {5,_referenceFields.YearRef },
            {6,_referenceFields.IdRef },
            {7,_referenceFields.Education },
            {8,_referenceFields.Location },
            {9,_referenceFields.Semester },
            {10,_referenceFields.Language },
            {11,_referenceFields.YearReport },
            {12,_referenceFields.OriginalRef },
            {13,_referenceFields.Match },
            {14,_referenceFields.Comment },
            {15,_referenceFields.Syllabus },
            {16,_referenceFields.Season },
            {17,_referenceFields.ExamEvent },
            {18,_referenceFields.Source },
            {19,_referenceFields.Pages },
            {20,_referenceFields.Volume },
            {21,_referenceFields.Chapters },
            {22,_referenceFields.BookTitle },
        };

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

            for (int i = 1; i <= _workbook.Worksheets.Count; i++)
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
            if (_workbook.Worksheets.Count > position && position > 0)
            {
                ActiveSheet = position;
            }
            else if (position <=0)
            {
                throw new ArgumentException("Position of worksheet must be 1 or greater");
            }
            else
            {
                _workbook.Worksheets.Add(DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), position);
            }
        }
        ///<inheritdoc cref="SetActiveSheet(int)"/>
        /// <param name="sheetname">The name of a sheet</param>
        public void SetActiveSheet(string sheetname)
        {
            Dictionary<int, string> dic = GetWorksheets();
            int outputSheetPos = -1;

            if (dic.ContainsValue(sheetname))
            {
                foreach (KeyValuePair<int,string> pair in dic)
                {
                    if (pair.Value == sheetname)
                    {
                        outputSheetPos = pair.Key;
                    }
                }
            }
            else
            {
                outputSheetPos = _workbook.Worksheets.Count + 1;
                _workbook.AddWorksheet(sheetname, outputSheetPos);
            }
            ActiveSheet = outputSheetPos;
        }

        /// <summary>
        /// Reads the next reference in the spreadsheet
        /// </summary>
        /// <returns>The next reference</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Reference ReadRef()
        {
            IXLRange OneRow = xLWorksheet.Range(_currentRow, _positionOfReferencesInSheet.First().Key, _currentRow, _positionOfReferencesInSheet.Last().Key);
            //Read from the corresponding fields according to dict
            Reference FilledReference = new Reference(
                new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, ""),
                OneRow.Cell(_currentRow, _positionOfReferencesInSheet.ElementAt(2).Key).GetValue<string>(),
                OneRow.Cell(_currentRow, _positionOfReferencesInSheet.ElementAt(3).Key).GetValue<string>(),
                OneRow.Cell(_currentRow, _positionOfReferencesInSheet.ElementAt(4).Key).GetValue<string>(),
                OneRow.Cell(_currentRow, _positionOfReferencesInSheet.ElementAt(5).Key).GetValue<string>(),
                OneRow.Cell(_currentRow, _positionOfReferencesInSheet.ElementAt(6).Key).GetValue<int>(),
                OneRow.Cell(_currentRow, _positionOfReferencesInSheet.ElementAt(7).Key).GetValue<int>(),
                OneRow.Cell(_currentRow, _positionOfReferencesInSheet.ElementAt(8).Key).GetValue<string>(),
                OneRow.Cell(_currentRow, _positionOfReferencesInSheet.ElementAt(9).Key).GetValue<string>(),
                OneRow.Cell(_currentRow, _positionOfReferencesInSheet.ElementAt(10).Key).GetValue<string>(),
                OneRow.Cell(_currentRow, _positionOfReferencesInSheet.ElementAt(11).Key).GetValue<string>(),
                OneRow.Cell(_currentRow, _positionOfReferencesInSheet.ElementAt(12).Key).GetValue<int>(),
                OneRow.Cell(_currentRow, _positionOfReferencesInSheet.ElementAt(13).Key).GetValue<int>(),
                OneRow.Cell(_currentRow, _positionOfReferencesInSheet.ElementAt(14).Key).GetValue<string>(),
                OneRow.Cell(_currentRow, _positionOfReferencesInSheet.ElementAt(15).Key).GetValue<string>(),
                OneRow.Cell(_currentRow, _positionOfReferencesInSheet.ElementAt(16).Key).GetValue<string>(),
                OneRow.Cell(_currentRow, _positionOfReferencesInSheet.ElementAt(17).Key).GetValue<string>(),
                OneRow.Cell(_currentRow, _positionOfReferencesInSheet.ElementAt(18).Key).GetValue<string>(),
                OneRow.Cell(_currentRow, _positionOfReferencesInSheet.ElementAt(19).Key).GetValue<int>(),
                OneRow.Cell(_currentRow, _positionOfReferencesInSheet.ElementAt(20).Key).GetValue<string>(),
                OneRow.Cell(_currentRow, _positionOfReferencesInSheet.ElementAt(21).Key).GetValue<string>()
                );          
          
            //Make instance of class filled with said fields
             
            //increment _currentRow
            _currentRow++;
            //return reference
            return FilledReference;
        }

        /// <summary>
        /// Reads the next references in the spreadsheet, delimited by <paramref name="amount"/>
        /// </summary>
        /// <param name="amount">The amount of references to read, if <paramref name="amount"/> is 0, reads all references in the worksheet.</param>
        /// <returns>List of references from spreadsheet</returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<Reference> ReadRef(uint amount)
        {
            //ReadRef() in loop with yield return statement
            int totalrows;
            if (amount + _currentRow >= 1048576)
            {
                throw new ArgumentOutOfRangeException($"Excel does not support more than 1,048,576 rows, tried to read {amount+_currentRow} rows.  ");
            }
            if (amount != 0)
            {
                totalrows = _currentRow + (int)amount;
            }
            else
            {
                totalrows = xLWorksheet.RowsUsed().Count();
            }

            for (int i = _currentRow; i <= totalrows; i++)
            {
                yield return ReadRef();
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if <c>FileName</c> is an Excel file
        /// </summary>
        /// <returns><c>true</c> if <c>FileName</c>suffix is .xlsx, <c>false</c> otherwise </returns>
        public bool IsFileExcel()
        {
            return (Path.GetExtension(FileName) == ".xlsx");
        }
        /// <summary>
        /// Creates a new, empty workbook, primarily for creating a new spreadsheet with <c>Reference</c>s
        /// </summary>
        public void Create()
        {
            _workbookProperty = new XLWorkbook();
            _workbookProperty.AddWorksheet("Sheet1");
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
                _workbookProperty = new XLWorkbook(FileName);
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
            if (File.Exists(filename))
            {
                throw new IOException("File with this name already exists");
            }
            else
            {
                _workbookProperty.SaveAs(Path.Join(FilePath + filename));
            }
            //throw new NotImplementedException();
            // Export spreadsheet....
            // Window.close(); 
        }
    }
}
