using ClosedXML.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Zenref.Ava.Models.Spreadsheet
{
    public sealed class Spreadsheet : IList<Reference>
    {
        private string FileName { get; }
        private string FilePath { get; }
        public bool DoesExcelFileExist => WorkbookProperty is not null;
        private XLWorkbook? Workbook { get; set; }
        private IXLWorksheet XlWorksheet => Workbook?.Worksheet(ActiveSheet) ?? throw new InvalidOperationException();

        private XLWorkbook? WorkbookProperty
        {
            get => Workbook ?? throw new FileNotFoundException(
                $"{nameof(Workbook)} is null, use Spreadsheet.Import() or Spreadsheet.Create() to fill this property");
            set => Workbook = value;
        }

        private int ActiveSheet { get; set; } = 1;
        private int CurrentRow { get; set; } = 2;

        private const int MaxRowsInExcel = 1048576; // The max number of rows in 2007 Excel that can be filled
        private readonly int _referenceFieldsCount = Enum.GetValues(typeof(ReferenceFields)).Length;

        /// <summary>
        /// Returns the total number of references in the active Worksheet.
        /// </summary>
        /// <remarks>Empty rows between first and last row will not be included in the count</remarks>
        public int Count => XlWorksheet.RowsUsed().Count();

        /// <summary>
        /// Returns the number of rows to the last used row in the active worksheet.
        /// </summary>
        /// <remarks>Not the same as count, Count returns the total number of references in a spreadsheet whereas length returns the row position of the last reference</remarks>
        public int Length => XlWorksheet.LastRowUsed().RowNumber();

        public bool IsReadOnly => Workbook.IsProtected;

        /// <summary>
        /// Gets or sets the reference at the given row index
        /// </summary>
        /// <param name="index">An integer greater than 1 (And almost always 2) and less than the total amount of references in the worksheet</param>
        /// <returns>The reference at the given index</returns>
        /// <remarks>Note: Cannot insert individual Reference properties with the setter</remarks>
        public Reference this[int index]
        {
            get
            {
                if (index <= 0) throw new ArgumentOutOfRangeException(nameof(index));
                return GetReference(index);
            }
            set
            {
                if (index <= 0) throw new ArgumentOutOfRangeException(nameof(index));
                Insert(index, value);
            }
        }
        
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
        public SortedDictionary<ReferenceFields, int> PositionOfReferencesInSheet { get; private set; } = new()
        {
            { ReferenceFields.Author, 1 },
            { ReferenceFields.Title, 2 },
            { ReferenceFields.PublicationType, 3 },
            { ReferenceFields.Publisher, 4 },
            { ReferenceFields.YearRef, 5 },
            { ReferenceFields.IdRef, 6 },
            { ReferenceFields.Education, 7 },
            { ReferenceFields.Location, 8 },
            { ReferenceFields.Semester, 9 },
            { ReferenceFields.Language, 10 },
            { ReferenceFields.YearReport, 11 },
            { ReferenceFields.OriginalRef, 12 },
            { ReferenceFields.Match, 13 },
            { ReferenceFields.Comment, 14 },
            { ReferenceFields.Syllabus, 15 },
            { ReferenceFields.Season, 16 },
            { ReferenceFields.ExamEvent, 17 },
            { ReferenceFields.Source, 18 },
            { ReferenceFields.Pages, 19 },
            { ReferenceFields.Volume, 20 },
            { ReferenceFields.Chapters, 21 },
            { ReferenceFields.BookTitle, 22 },
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

        /// <summary>
        /// Sets the column position of reference properties as given by the input dictionary
        /// </summary>
        /// <param name="inputdic">The Sorted dictionary where the key is the reference property and the value is the column position associated with the property</param>
        /// <exception cref="ArgumentException"> If the size of input dictionary is not the same as the number of fields in a reference</exception>
        public void SetColumnPosition(SortedDictionary<ReferenceFields, int> inputdic)
        {
            if (inputdic.Count == _referenceFieldsCount)
                PositionOfReferencesInSheet = inputdic;
            else
                throw new ArgumentException(
                    $"Parameter inputdic must be the same size as the current dictionary. inputdic.Count =={inputdic.Count} !={_referenceFieldsCount}");
        }

        /// <summary>
        /// Swaps the column position of two Reference properties
        /// </summary>
        public void SwapReferenceProperty(ReferenceFields first, ReferenceFields second)
        {
            (PositionOfReferencesInSheet[second], PositionOfReferencesInSheet[first]) = (PositionOfReferencesInSheet[first], PositionOfReferencesInSheet[second]);
        }

        /// <summary>
        /// Gets the position and names of all the worksheets in the workbook
        /// </summary>
        /// <returns>A <c>Dictionary</c> where the key represents the position of the sheet in the book and the value is the name of the sheet.</returns>
        /// <exception cref="FileNotFoundException">Throws if a spreadsheet has not been imported</exception>
        public Dictionary<int, string> GetWorksheets()
        {
            Dictionary<int, string> resDic = new Dictionary<int, string>();
            
            for (int i = 1; i <= Workbook.Worksheets.Count; i++)
            {
                IXLWorksheet worksheet = Workbook.Worksheet(i);
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
            if (Workbook != null && (Workbook.Worksheets.Count <= position || position <= 0))
            {
                if (position > 0)
                    Workbook.Worksheets.Add(DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), position);
                else
                    throw new ArgumentException("Position of worksheet must be 1 or greater");
            }
            else
            {
                ActiveSheet = position;
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
                foreach (KeyValuePair<int, string> pair in dic.Where(pair => pair.Value == sheetname))
                {
                    outputSheetPos = pair.Key;
                }
            }
            else
            {
                if (Workbook != null)
                {
                    outputSheetPos = Workbook.Worksheets.Count + 1;
                    Workbook.AddWorksheet(sheetname, outputSheetPos);
                }
            }

            ActiveSheet = outputSheetPos;
        }

        /// <summary>
        /// Gets a reference at the specified row
        /// </summary>
        /// <param name="index">The row index of the ReferenceNote that Excel is 1-indexed, and the first row is usually reserved for metadata</param>
        /// <returns>The Reference at the given row</returns>
        /// <remarks>Note that Excel is 1-indexed, and the 1st row is usually reserved for metadata</remarks>
        public Reference GetReference(int index)
        {
            CurrentRow = index;
            IXLRow indexedRow = XlWorksheet.Row(index);
            return ReadRow(indexedRow);
        }

        /// <summary>
        /// Reads the contents of an Excel row and returns a Reference
        /// </summary>
        /// <param name="row">The Excel row containing a Reference</param>
        /// <returns>A Reference from the given row</returns>
        private Reference ReadRow(IXLRow row)
        {
            string author =         getCell(row, ReferenceFields.Author).GetValue<string>();
            string title =          getCell(row, ReferenceFields.Title).GetValue<string>();
            string pubType =        getCell(row, ReferenceFields.PublicationType).GetValue<string>();
            string publisher =      getCell(row, ReferenceFields.Publisher).GetValue<string>();
            int? yearOfRef =        getCell(row, ReferenceFields.YearRef).GetValue<int?>();
            int? id =               getCell(row, ReferenceFields.IdRef).GetValue<int?>();
            string edu =            getCell(row, ReferenceFields.Education).GetValue<string>();
            string location =       getCell(row, ReferenceFields.Location).GetValue<string>();
            string semester =       getCell(row, ReferenceFields.Semester).GetValue<string>();
            string language =       getCell(row, ReferenceFields.Language).GetValue<string>();
            int? yearOfReport =     getCell(row, ReferenceFields.YearReport).GetValue<int?>();
            double? match =         getCell(row, ReferenceFields.Match).GetValue<double?>();
            string comment =        getCell(row, ReferenceFields.Comment).GetValue<string>();
            string syllabus =       getCell(row, ReferenceFields.Syllabus).GetValue<string>();
            string season =         getCell(row, ReferenceFields.Season).GetValue<string>();
            string examEvent =      getCell(row, ReferenceFields.ExamEvent).GetValue<string>();
            string source =         getCell(row, ReferenceFields.Source).GetValue<string>();
            int? pages =            getCell(row, ReferenceFields.Pages).GetValue<int?>();
            string volume =         getCell(row, ReferenceFields.Volume).GetValue<string>();
            string chapters =       getCell(row, ReferenceFields.Chapters).GetValue<string>();
            string bookTitle =      getCell(row, ReferenceFields.BookTitle).GetValue<string>();

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
            if (amount + CurrentRow >= MaxRowsInExcel)
            {
                throw new ArgumentOutOfRangeException(
                    $"Excel does not support more than 1,048,576 rows, tried to read {amount + CurrentRow} rows.");
            }

            int totalrows = amount != 0 ? CurrentRow + (int)amount : Count;

            for (int i = CurrentRow; i <= totalrows; i++)
            {
                yield return GetReference(i);
            }
        }

        /// <summary>
        /// Checks if <c>FileName</c> is an Excel file
        /// </summary>
        /// <returns><c>true</c> if <c>FileName</c>suffix is .xlsx, <c>false</c> otherwise </returns>
        /// <remarks>.xlsx refers to 2007 Excel files, if file suffx is .xls, method will return false</remarks>
        public bool IsFileExcel()
        {
            return (Path.GetExtension(FileName) == ".xlsx");
        }

        /// <summary>
        /// Creates a new, empty workbook, primarily for creating a new spreadsheet with References. Also creates an empty worksheet called Sheet1
        /// </summary>
        public void Create()
        {
            WorkbookProperty = new XLWorkbook();
            WorkbookProperty.AddWorksheet("Sheet1");
        }

        /// <summary>
        /// Appends <c>reference</c> to the the next row of the first worksheet
        /// </summary>
        /// <param name="reference">The reference to be added</param>
        /// <param name="row">Optional, adds reference to a given row, possibly overwriting it</param>
        public void AddReference(Reference reference, int row = -1)
        {
            row = row != -1 ? row : Count + 1;
            IXLRow indexedRow = XlWorksheet.Row(row);
            indexedRow.Clear();
            setReference(reference, indexedRow);
        }

        private void setReference(Reference reference, IXLRow indexedRow)
        {
            getCell(indexedRow, ReferenceFields.Author).SetValue(reference.Author ?? "");
            getCell(indexedRow, ReferenceFields.Title).SetValue(reference.Title ?? "");
            getCell(indexedRow, ReferenceFields.PublicationType).SetValue(reference.PubType ?? "");
            getCell(indexedRow, ReferenceFields.Publisher).SetValue(reference.Publisher ?? "");
            getCell(indexedRow, ReferenceFields.YearRef).SetValue(reference.YearRef);
            getCell(indexedRow, ReferenceFields.IdRef).SetValue(reference.ID);
            getCell(indexedRow, ReferenceFields.Education).SetValue(reference.Edu ?? "");
            getCell(indexedRow, ReferenceFields.Location).SetValue(reference.Location ?? "");
            getCell(indexedRow, ReferenceFields.Semester).SetValue(reference.Semester ?? "");
            getCell(indexedRow, ReferenceFields.Language).SetValue(reference.Language ?? "");
            getCell(indexedRow, ReferenceFields.YearReport).SetValue(reference.YearReport);
            getCell(indexedRow, ReferenceFields.Match).SetValue(reference.Match);
            getCell(indexedRow, ReferenceFields.Comment).SetValue(reference.Commentary ?? "");
            getCell(indexedRow, ReferenceFields.Syllabus).SetValue(reference.Syllabus ?? "");
            getCell(indexedRow, ReferenceFields.Season).SetValue(reference.Season ?? "");
            getCell(indexedRow, ReferenceFields.ExamEvent).SetValue(reference.ExamEvent ?? "");
            getCell(indexedRow, ReferenceFields.Source).SetValue(reference.Source ?? "");
            getCell(indexedRow, ReferenceFields.Pages).SetValue(reference.Pages);
            getCell(indexedRow, ReferenceFields.Volume).SetValue(reference.Volume ?? "");
            getCell(indexedRow, ReferenceFields.Chapters).SetValue(reference.Chapters ?? "");
            getCell(indexedRow, ReferenceFields.BookTitle).SetValue(reference.BookTitle ?? "");
        }

        /// <summary>
        /// Appends multiple <c>references</c> to the next rows of the first worksheet
        /// </summary>
        /// <param name="references">Collection of references to be added</param>
        /// <param name="startRow">The start row from where the references should be inserted. If default, appends to end of list of references</param>
        /// <exception cref="ArgumentException">Throws if parameter startRow is 0 or less than -1</exception>
        public void AddReference(IEnumerable<Reference> references, int startRow = -1)
        {
            if (startRow is 0 or < -1)
            {
                throw new ArgumentException("Start row cannot be 0 or less than -1");
            }
            if (startRow == -1) startRow = Count + 1;
            foreach (Reference reference in references)
            {
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
                WorkbookProperty = new XLWorkbook(FileName);
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

            WorkbookProperty?.SaveAs(Path.Join(FilePath + filename));
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
            if (index is > 0 and <= MaxRowsInExcel)
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
                XlWorksheet.Row(index).Delete();
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
            IXLRow lastRow = XlWorksheet.LastRowUsed();
            IXLColumn lastColumn = XlWorksheet.LastColumnUsed();
            IXLRange allRows = XlWorksheet.Range(1, 1, lastRow.RowNumber(), lastColumn.ColumnNumber());
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
            
            // Forloop giver bedre performance end foreach 
            for (int index = 0; index < this.Count; index++)
            {
                Reference reference = this[index];
                if (reference.ValueEquals(item))
                {
                    doesContain = true;
                }
            }

            return doesContain;
        }

        [Obsolete]
        public void CopyTo(Reference[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        ///Removes the Reference from the spreadsheet and shifts all rows below it up thus
        ///</summary>
        /// <remarks>
        /// Reduces Count by 1.
        /// Don't use in a loop, ClosedXML alleges poor performance consider using clear</remarks>
        public bool Remove(Reference item)
        {
            if (IndexOf(item) == -1)
            {
                return false;
            }

            IXLRow row = XlWorksheet.Row(IndexOf(item));
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
            
            // Forloop giver bedre performance end foreach i NET6.0
            for (int index = 0; index < Count; index++)
            {
                Reference item = this[index];
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}