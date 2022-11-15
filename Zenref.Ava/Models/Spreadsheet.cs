using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Zenref.Ava.Models.Spreadsheet
{
    /// <summary>
    /// Represents an Excel file containing an amount of worksheets
    /// </summary>
    public class Spreadsheet : IList<Reference>
    {
        /// <summary>
        /// The name of the Excel file
        /// </summary>
        private string FileName { get; }

        /// <summary>
        /// Represents the total amount of rows in a spreadsheet
        /// </summary>
        public int ReferenceCount { get; }

        /// <summary>
        /// Represents the path that the file should be read to.
        /// </summary>
        /// <remarks>Should only be used if new Excel files are to be created</remarks>
        public string FilePath { get; private set; }

        /// <summary>
        /// Represents whether or not <c>_workbook</c> exists or not
        /// </summary>
        public bool DoesExcelFileExist { get => _workbookProperty is not null; }

        private XLWorkbook? _workbook { get; set; }

        private XLWorkbook? _workbookProperty
        {
            get => _workbook ?? throw new FileNotFoundException($"{nameof(_workbook)} is null, use Spreadsheet.Import() or Spreadsheet.Create() to fill this property");
            set => _workbook = value;
        }
        /// <summary>
        /// The Position of the active worksheet in Excel.
        /// </summary>
        public int ActiveSheet { get; set; } = 1;
        private IXLWorksheet xLWorksheet { get => _workbook.Worksheet(ActiveSheet); }
        private int _currentRow { get; set; } = 2;
        private const int _MAXROWSINEXCEL = 1048576;
        private int _REFERENCEFIELDSCOUNT = Enum.GetValues(typeof(ReferenceFields)).Length;

        /// <summary>
        /// Returns the total number of references in the active Worksheet.
        /// </summary>
        /// <remarks>Empty rows between first and last row will not be included in the count</remarks>
        public int Count => xLWorksheet.RowsUsed().Count();
        /// <summary>
        /// Returns the number of rows to the last used row in the active worksheet.
        /// </summary>
        public int Length => xLWorksheet.LastRowUsed().RowNumber();

        public bool IsReadOnly => _workbook.IsProtected;

        /// <summary>
        /// Gets the reference at the given row index
        /// </summary>
        /// <param name="index">An integer greater than 1 (And almost always 2) and less than the total amount of references in the worksheet</param>
        /// <returns>The reference at the given index</returns>
        /// <remarks>Cannot insert individual Reference properties with the setter</remarks>
        public Reference this[int index] { get => GetReference(index); set => Insert(index, value); }

        /// <summary>
        /// Represents the different fields that a reference instance contains.
        /// </summary>
        public enum ReferenceFields
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
        }

        /// <summary>
        /// Represents the different fields in an Excel worksheet where the key is the column position and the value is the content
        /// </summary>
        /// <remarks>Note: The values should be unique as well, since one Excel cell can only contain one field</remarks>
        public SortedDictionary<ReferenceFields, int> PositionOfReferencesInSheet { get; private set; } = new SortedDictionary<ReferenceFields, int>()
        {
            {ReferenceFields.Author, 1},
            {ReferenceFields.Title, 2},
            {ReferenceFields.PublicationType, 3},
            {ReferenceFields.Publisher, 4},
            {ReferenceFields.YearRef, 5},
            {ReferenceFields.IdRef, 6},
            {ReferenceFields.Education, 7},
            {ReferenceFields.Location, 8},
            {ReferenceFields.Semester, 9},
            {ReferenceFields.Language, 10},
            {ReferenceFields.YearReport, 11},
            {ReferenceFields.OriginalRef, 12},
            {ReferenceFields.Match, 13},
            {ReferenceFields.Comment, 14},
            {ReferenceFields.Syllabus, 15},
            {ReferenceFields.Season, 16},
            {ReferenceFields.ExamEvent, 17},
            {ReferenceFields.Source, 18},
            {ReferenceFields.Pages, 19},
            {ReferenceFields.Volume, 20},
            {ReferenceFields.Chapters, 21},
            {ReferenceFields.BookTitle, 22},
        };

        /// <summary>
        /// Represents a spreadsheet existing in the same current working directory as the program
        /// </summary>
        /// <param name="fileName">The name of the Excelfile, with file extension .xlsx</param>
        public Spreadsheet(string fileName)
        {
            FileName = fileName;
            FilePath = "";
        }
        /// <summary>
        /// Represents a spreadsheet in a given directory
        /// </summary>
        /// <param name="fileName">The name of the Excelfile, with file extension .xlsx</param>
        /// <param name="filepath">The Absolute or relative path of the Excel file</param>
        public Spreadsheet(string fileName, string filepath)
        {
            FileName = fileName;
            FilePath = filepath;
        }

        //TODO test
        /// <summary>
        /// Sets the column position of reference properties as given by the input dictionary
        /// </summary>
        /// <param name="inputdic">The Sorted dictionary where the key is the reference property and the value is the column position associated with the property</param>
        /// <exception cref="ArgumentException"> If the size of input dictionary is not the same as the number of fields in a reference</exception>
        public void SetColumnPosition(SortedDictionary<ReferenceFields,int> inputdic)
        {
            if (inputdic.Count != _REFERENCEFIELDSCOUNT)
            {
                throw new ArgumentException("Parameter inputdic must be the same size as the current dictionary " + inputdic.Count + " " + _REFERENCEFIELDSCOUNT);
            }
            PositionOfReferencesInSheet = inputdic;
        }

        //TODO test
        /// <summary>
        /// Swaps the column position of two Reference properties
        /// </summary>
        public void SwapReferenceProperty(ReferenceFields first, ReferenceFields second)
        {
            int firstValue = PositionOfReferencesInSheet[first];
            PositionOfReferencesInSheet[first] = PositionOfReferencesInSheet[second];
            PositionOfReferencesInSheet[second] = firstValue;
        }

        /// <summary>
        /// Gets the position and names of all the worksheets in the workbook
        /// </summary>
        /// <returns>A <c>Dictionary</c> where the key represents the position of the sheet in the book and the value is the name of the sheet.</returns>
        /// <exception cref="FileNotFoundException">Throws if a spreadsheet has not been imported</exception>
        public Dictionary<int, string> GetWorksheets()
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
            else if (position <= 0)
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
                foreach (KeyValuePair<int, string> pair in dic)
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
        /// Gets a reference at the specified row
        /// </summary>
        /// <param name="index">The row index of the ReferenceNote that Excel is 1-indexed, and the 1st row is usually reserved for metadata</param>
        /// <returns>The Reference at the given row</returns>
        /// <remarks>Note that Excel is 1-indexed, and the 1st row is usually reserved for metadata</remarks>
        public Reference GetReference(int index)
        {
            _currentRow = index;
            IXLRow indexedRow = xLWorksheet.Row(index);

            return readRow(indexedRow);
        }

        /// <summary>
        /// Reads the contents of an Excel row and returns a Reference
        /// </summary>
        /// <param name="row">The Excel row containing a Reference</param>
        /// <returns>A Reference from the given row</returns>
        private Reference readRow(IXLRow row)
        {
            string author = getCell(row, ReferenceFields.Author).GetValue<string>();
            string title = getCell(row, ReferenceFields.Title).GetValue<string>();
            string pubType = getCell(row, ReferenceFields.PublicationType).GetValue<string>();
            string publisher = getCell(row, ReferenceFields.Publisher).GetValue<string>();
            int? yearOfRef = getCell(row, ReferenceFields.YearRef).GetValue<int?>();
            int? id = getCell(row, ReferenceFields.IdRef).GetValue<int?>();
            string edu = getCell(row, ReferenceFields.Education).GetValue<string>();
            string location = getCell(row, ReferenceFields.Location).GetValue<string>();
            string semester = getCell(row, ReferenceFields.Semester).GetValue<string>();
            string language = getCell(row, ReferenceFields.Language).GetValue<string>();
            int? yearOfReport = getCell(row, ReferenceFields.YearReport).GetValue<int?>();
            double? match = getCell(row, ReferenceFields.Match).GetValue<double?>();
            string comment = getCell(row, ReferenceFields.Comment).GetValue<string>();
            string syllabus = getCell(row, ReferenceFields.Syllabus).GetValue<string>();
            string season = getCell(row, ReferenceFields.Season).GetValue<string>();
            string examEvent = getCell(row, ReferenceFields.ExamEvent).GetValue<string>();
            string source = getCell(row, ReferenceFields.Source).GetValue<string>();
            int? pages = getCell(row, ReferenceFields.Pages).GetValue<int?>();
            string volume = getCell(row, ReferenceFields.Volume).GetValue<string>();
            string chapters = getCell(row, ReferenceFields.Chapters).GetValue<string>();
            string bookTitle = getCell(row, ReferenceFields.BookTitle).GetValue<string>();

            return new Reference(author,
                                 title,
                                 pubType,
                                 publisher,
                                 yearOfRef,
                                 id,
                                 edu,
                                 location,
                                 semester,
                                 language,
                                 yearOfReport,
                                 match,
                                 comment,
                                 syllabus,
                                 season,
                                 examEvent,
                                 source,
                                 pages,
                                 volume,
                                 chapters,
                                 bookTitle);
        }

        /// <summary>
        /// Gets the Excel cell at a given row and specific field
        /// </summary>
        /// <param name="row">The row of a worksheet</param>
        /// <param name="field">The field in a row</param>
        /// <returns></returns>
        private IXLCell getCell(IXLRow row, ReferenceFields field)
        {
            return row.Cell(PositionOfReferencesInSheet[field]);
        }
        /// <summary>
        /// Reads the next references in the spreadsheet, delimited by <paramref name="amount"/>
        /// </summary>
        /// <param name="amount">The amount of references to read, if <paramref name="amount"/> is 0, reads all references in the worksheet.</param>
        /// <returns>List of references from spreadsheet</returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<Reference> GetReference(uint amount)
        {
            //ReadRef() in loop with yield return statement
            int totalrows;
            if (amount + _currentRow >= _MAXROWSINEXCEL)
            {
                throw new ArgumentOutOfRangeException($"Excel does not support more than 1,048,576 rows, tried to read {amount + _currentRow} rows.  ");
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
                yield return GetReference(i);
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
        /// <param name="row">Optional, adds reference to a given row, possibly overwriting it</param>
        public void AddReference(Reference reference, int row = -1)
        {
            row = row != -1 ? row : xLWorksheet.RowsUsed().Count() + 1;
            IXLRow indexedRow = xLWorksheet.Row(row);
            indexedRow.Clear();
            setReference(reference, indexedRow);
        }

        private void setReference(Reference reference, IXLRow indexedRow)
        {

            getCell(indexedRow, ReferenceFields.Author).SetValue<string>(reference.Author ?? "");
            getCell(indexedRow, ReferenceFields.Title).SetValue<string>(reference.Title ?? "");
            getCell(indexedRow, ReferenceFields.PublicationType).SetValue<string>(reference.PubType ?? "");
            getCell(indexedRow, ReferenceFields.Publisher).SetValue<string>(reference.Publisher ?? "");
            getCell(indexedRow, ReferenceFields.YearRef).SetValue<int?>(reference.YearRef);
            getCell(indexedRow, ReferenceFields.IdRef).SetValue<int?>(reference.ID);
            getCell(indexedRow, ReferenceFields.Education).SetValue<string>(reference.Edu ?? "");
            getCell(indexedRow, ReferenceFields.Location).SetValue<string>(reference.Location ?? "");
            getCell(indexedRow, ReferenceFields.Semester).SetValue<string>(reference.Semester ?? "");
            getCell(indexedRow, ReferenceFields.Language).SetValue<string>(reference.Language ?? "");
            getCell(indexedRow, ReferenceFields.YearReport).SetValue<int?>(reference.YearReport);
            getCell(indexedRow, ReferenceFields.Match).SetValue<double?>(reference.Match);
            getCell(indexedRow, ReferenceFields.Comment).SetValue<string>(reference.Commentary ?? "");
            getCell(indexedRow, ReferenceFields.Syllabus).SetValue<string>(reference.Syllabus ?? "");
            getCell(indexedRow, ReferenceFields.Season).SetValue<string>(reference.Season ?? "");
            getCell(indexedRow, ReferenceFields.ExamEvent).SetValue<string>(reference.ExamEvent ?? "");
            getCell(indexedRow, ReferenceFields.Source).SetValue<string>(reference.Source ?? "");
            getCell(indexedRow, ReferenceFields.Pages).SetValue<int?>(reference.Pages);
            getCell(indexedRow, ReferenceFields.Volume).SetValue<string>(reference.Volume ?? "");
            getCell(indexedRow, ReferenceFields.Chapters).SetValue<string>(reference.Chapters ?? "");
            getCell(indexedRow, ReferenceFields.BookTitle).SetValue<string>(reference.BookTitle ?? "");

        }

        /// <summary>
        /// Appends multiple <c>references</c> to the next rows of the first worksheet
        /// </summary>
        /// <param name="references">Collection of references to be added</param>
        /// <param name="startRow">The start row from where the references should be inserted. If default, appends to end of list of references</param>
        public void AddReference(IEnumerable<Reference> references, int startRow = -1)
        {
            if (startRow == -1) startRow = xLWorksheet.RowsUsed().Count() + 1;
            foreach (Reference reference in references)
            {
                //AddReference(reference);
                AddReference(reference, startRow++);
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

        /// <summary>
        /// Gets the index of the reference matching the input reference.
        /// </summary>
        /// <param name="item">The Reference to find.</param>
        /// <returns>The index position of the reference, -1 if no such reference was found</returns>
        public int IndexOf(Reference item)
        {
            int indexof = -1;
            for (int i = 1; i < Count; i++)
            {
                if (this[i].ValueEquals(item))
                {
                    indexof = i;
                }
            }
            return indexof;
        }

        /// <summary>
        /// Inserts the reference at the given index position
        /// </summary>
        /// <param name="index">The index position to insert the reference</param>
        /// <param name="item">The reference to be inserted</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if the index position is less than 1 or higher than Count </exception>
        /// <remarks>To append a refernce to the list, use <c>Add</c> instead</remarks>
        public void Insert(int index, Reference item)
        {
            if (index > 0 && index <= _MAXROWSINEXCEL)
            {

                AddReference(item, index);
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }
        /// <summary>
        /// Removes the reference at the given index
        /// </summary>
        /// <param name="index">The index position</param>
        /// <exception cref="ArgumentException">Throws if the reference could not be deleted.</exception>
        /// <remarks>Deletes the row and shifts latter rows by 1 thus reducing Count by 1</remarks>
        public void RemoveAt(int index)
        {
            if (index > 0 && index <= Count)
            {
                //Reference itemToBeRemoved = this[index];
                //Remove(itemToBeRemoved);
                xLWorksheet.Row(index).Delete();
            }
            else
            {
                throw new ArgumentException("Error in deleting item");
            }
        }

        /// <summary>
        /// Appends a reference to the end of list
        /// </summary>
        /// <param name="item">The reference to append</param>
        public void Add(Reference item)
        {
            AddReference(item, Count + 1);
        }

        /// <summary>
        /// Deletes the active worksheet.
        /// </summary>
        public void Clear()
        {
            IXLRow lastrow = xLWorksheet.LastRowUsed();
            IXLColumn lastcolumn = xLWorksheet.LastColumnUsed();
            IXLRange allRows = xLWorksheet.Range(1, 1, lastrow.RowNumber(), lastcolumn.ColumnNumber());
            allRows.Clear();
        }

        /// <summary>
        /// Determines whether the reference exists in the list
        /// </summary>
        /// <param name="item">The reference to match</param>
        /// <returns>True </returns>
        public bool Contains(Reference item)
        {
            bool doesContain = false;
            foreach (Reference reference in this)
            {
                if (reference.ValueEquals(item))
                {
                    doesContain = true;
                }
            }
            return doesContain;
        }

        public void CopyTo(Reference[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
        ///<summary>
        ///Removes the Reference from the spreadsheet and shifts all rows below it up thus
        ///</summary>
        /// <remarks>
        /// Reduces Count by 1
        /// 
        /// Don't use in a loop, ClosedXML alleges poor performance consider using clear</remarks>
        public bool Remove(Reference item)
        {
            if (IndexOf(item) == -1)
            {
                return false;
            }
            IXLRow row = xLWorksheet.Row(IndexOf(item));
            row.Delete();
            return true;
        }

        public IEnumerator<Reference> GetEnumerator()
        {
            if (Count == 0)
            {
                //Hvis count er 0 så bliver GetEnumerator kaldt rekursivt uden en stop case, idk why
                yield break;
            }
            foreach (Reference item in this)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
