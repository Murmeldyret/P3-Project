using ClosedXML.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Zenref.Ava.Models.Spreadsheet
{
    // Spreadsheet class represents a spreadsheet and implements the IList interface
    public sealed class Spreadsheet : IList<RawReference>
    {
        // FileName property stores the name of the spreadsheet file
        private string FileName { get; }
        
        // FilePath property stores the path of the spreadsheet file
        private string FilePath { get; }
        
        // Property that indicates if the excel file exists
        public bool DoesExcelFileExist => WorkbookProperty is not null;
        
        // Property that returns the active worksheet in the spreadsheet
        private IXLWorksheet XlWorksheet => Workbook?.Worksheet(ActiveSheet) ?? throw new InvalidOperationException();
        
        // Property that stores the XLWorkbook object
        private XLWorkbook? Workbook { get; set; }

        // Property that returns the XLWorkbook object and throws an exception if the Workbook is null
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

        public bool IsReadOnly => Workbook!.IsProtected; // Not used..?

        /// <summary>
        /// Gets or sets the reference at the given row index
        /// </summary>
        /// <param name="index">An integer greater than 1 (And almost always 2) and less than the total amount of references in the worksheet</param>
        /// <returns>The reference at the given index</returns>
        /// <remarks>Note: Cannot insert individual Reference properties with the setter</remarks>
        public RawReference this[int index]
        {
            get
            {
                if (index <= 0) throw new ArgumentOutOfRangeException(nameof(index));
                return GetRawReference(index);
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
            RefId,
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
        public Dictionary<ReferenceFields, int> PositionOfReferencesInSheet { get; private set; } = new()
        {
            { ReferenceFields.Author, 1 },
            { ReferenceFields.Title, 2 },
            { ReferenceFields.PublicationType, 3 },
            { ReferenceFields.Publisher, 4 },
            { ReferenceFields.YearRef, 5 },
            { ReferenceFields.RefId, 6},
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
        public void SetColumnPosition(Dictionary<ReferenceFields, int> inputdic)
        {
                PositionOfReferencesInSheet = inputdic;
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
        public Dictionary<int, string> GetWorksheets() // int er ....
        {
            // : creates a new instance of a Dictionary<int, string> object and assigns it to the local variable resDic. 
            Dictionary<int, string> resDic = new Dictionary<int, string>();
            
            // : Loop through the worksheets in the workbook
            for (int i = 1; i <= Workbook!.Worksheets.Count; i++)
            {
                // : It uses the Worksheet method of Workbook object to retrieve the worksheet at the specified index.
                IXLWorksheet worksheet = Workbook.Worksheet(i);
                
                // : Add the worksheet position and name to the result dictionary
                resDic.Add(worksheet.Position, worksheet.Name);
            }
            
            // : Return the result dictionary
            return resDic;
        }

        /// <summary>
        /// Sets the active worksheet to read from or write to. If the sheet does not exist, it creates one.
        /// </summary>
        /// <param name="position">The position of the sheet</param>
        /// <exception cref="ArgumentException">Throws if position is below 0</exception>
        public void SetActiveSheet(int position)
        {
            // : Check if workbook is not null and if position is less than or equal to 0 or greater than the number of worksheets
            if (Workbook != null && (Workbook.Worksheets.Count < position || position <= 0))
            {
                // : If position is greater than 0, add a new worksheet with the current timestamp as its name at the specified position
                if (position > 0)
                    Workbook.Worksheets.Add(DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), position);
                // : If position is less than or equal to 0, throw an argument exception
                else
                    throw new ArgumentException("Position of worksheet must be 1 or greater");
            }
            else
            {
                // : Set the active sheet to the specified position
                ActiveSheet = position;
            }
        }

        ///<inheritdoc cref="SetActiveSheet(int)"/>
        /// <param name="sheetname">The name of a sheet</param>
        public void SetActiveSheet(string sheetname)
        {
            // : Retrieves the dictionary of worksheets, where the key is the sheet's position and the value is the sheet's name.
            Dictionary<int, string> dic = GetWorksheets();
            int outputSheetPos = -1;
            
            // : Check if the given sheet name is in the worksheets dictionary.
            if (dic.ContainsValue(sheetname))
            {
                // : Find the position of the sheet with the given name.
                foreach (KeyValuePair<int, string> pair in dic.Where(pair => pair.Value == sheetname))
                {
                    outputSheetPos = pair.Key;
                }
            }
            else
            {
                // : If the workbook exists, add a new worksheet with the given name and set its position to the next one after the last worksheet.
                if (Workbook != null)
                {
                    outputSheetPos = Workbook.Worksheets.Count + 1;
                    Workbook.AddWorksheet(sheetname, outputSheetPos);
                }
            }

            // : Set the active sheet with the determined position.
            ActiveSheet = outputSheetPos;
        }

        /// <summary>
        /// Reads the contents of an Excel row and returns a Reference
        /// </summary>
        /// <param name="row">The Excel row containing a Reference</param>
        /// <returns>A Reference from the given row</returns>
        public Reference GetReference(int index)
        {
            // : Assign the index to the current row
            CurrentRow = index;
            
            // : Get the row at the specified index
            IXLRow indexedRow = XlWorksheet.Row(index);
            
            // : Read and return the data in the indexed row
            return ReadRow(indexedRow);
        }

        /// </summary>
        /// <param name="index">The row index of the ReferenceNote that Excel is 1-indexed, and the first row is usually reserved for metadata</param>
        /// <returns>The Reference at the given row</returns>
        /// <remarks>Note that Excel is 1-indexed, and the 1st row is usually reserved for metadata</remarks>
        public RawReference GetRawReference(int index)
        {
            // : Set the current row to the index passed in
            CurrentRow = index;
            
            // : Get the XLWorksheet row at the index
            IXLRow indexedRow = XlWorksheet.Row(index);
            
            // : Return the result of calling ReadRawRow with the indexed row
            return ReadRawRow(indexedRow);
        }


        /// <summary>
        /// Reads the contents of an Excel row and returns a Reference
        /// </summary>
        /// <param name="row">The Excel row containing a Reference</param>
        /// <returns>A Reference from the given row</returns>
        // : This method reads raw data from a given row in an excel sheet and returns a RawReference object. 
        private RawReference ReadRawRow(IXLRow row)
        {
            // : Get the value of the "Education" column in the row.
            string education = getCell(row, ReferenceFields.Education).GetValue<string>();
            
            // : Get the value of the "Location" column in the row.
            string location = getCell(row, ReferenceFields.Location).GetValue<string>();
            
            // : Get the value of the "Semester" column in the row.
            string semester = getCell(row, ReferenceFields.Semester).GetValue<string>();
            
            // : Get the value of the "OriginalRef" column in the row.
            string oriReference = getCell(row, ReferenceFields.OriginalRef).GetValue<string>();
            
            // : Get the value of the "RefId" column in the row.
            string refId = getCell(row, ReferenceFields.RefId).GetValue<string>();
            
            // : Return a new RawReference object with the extracted values.
            return new RawReference(education, location, semester, refId, oriReference);

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
            
            string education = getCell(row, ReferenceFields.Education).GetValue<string>();
            string location = getCell(row, ReferenceFields.Location).GetValue<string>();
            string semester = getCell(row, ReferenceFields.Semester).GetValue<string>();
            string oriReference = getCell(row, ReferenceFields.OriginalRef).GetValue<string>();
            string refId = getCell(row, ReferenceFields.RefId).GetValue<string>();

            RawReference rawReference = new RawReference(education, location, semester, refId, oriReference);
            return new Reference( rawReference,
                author,
                title,
                pubType,
                publisher,
                yearOfRef,
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
        // : Get a specified number of references from the Excel file, starting from the current row.

        public IEnumerable<Reference> GetReference(uint amount)
        {
            // : Check if the requested number of rows exceeds the maximum number of rows supported in Excel 
            if (amount + CurrentRow >= MaxRowsInExcel)
            {
                // : Throw an exception if the requested number of rows exceeds the maximum number of rows in Excel 
                throw new ArgumentOutOfRangeException(
                    $"Excel does not support more than 1,048,576 rows, tried to read {amount + CurrentRow} rows.");
            }
            
            // : If the amount of rows is not specified, return all the rows, otherwise, return the requested amount of rows
            int totalrows = amount != 0 ? CurrentRow + (int)amount : Count;
            
            // : Iterate through the rows, starting from the current row, until total rows
            for (int i = CurrentRow; i <= totalrows; i++)
            {
                // : Return the reference of the current row
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
            // : Get the extension of the file
            // : And check if it's equal to ".xlsx"
            // : Return true if it's an excel file, otherwise false
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
        // : Method to add a RawReference object to the excel sheet
        public void AddRawReference(RawReference reference, int row = -1)
        {
            // : if row is not passed, then add the new reference to the next available row
            row = row != -1 ? row : Count + 1;
            
            // : get the row based on the given row number
            IXLRow indexedRow = XlWorksheet.Row(row);
            
            // : clear any existing content in the row
            indexedRow.Clear();
            
            // : add the content of the RawReference object to the row
            setRawReference(reference, indexedRow);
        }

        /// <summary>
        /// : AddReference method adds a new reference object to the Excel worksheet at the specified row
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="row"></param>
        public void AddReference(Reference reference, int row = -1)
        {
            // : If row is not specified, the new reference will be added to the next empty row.
            row = row != -1 ? row : Count + 1;
            
            // : Get the row object at the specified position
            IXLRow indexedRow = XlWorksheet.Row(row);
            
            // : Clear the contents of the row
            indexedRow.Clear();
            
            // : Call the setReference method to populate the contents of the row with the new reference
            setReference(reference, indexedRow);
        }

        private void setRawReference(RawReference reference, IXLRow indexedRow)
        {
            // getCell(indexedRow, ReferenceFields.Author).SetValue(reference.Author ?? "");
            // getCell(indexedRow, ReferenceFields.Title).SetValue(reference.Title ?? "");
            // getCell(indexedRow, ReferenceFields.PublicationType).SetValue(reference.PubType ?? "");
            // getCell(indexedRow, ReferenceFields.Publisher).SetValue(reference.Publisher ?? "");
            // getCell(indexedRow, ReferenceFields.YearRef).SetValue(reference.YearRef);
            // getCell(indexedRow, ReferenceFields.IdRef).SetValue(reference.ID);
            // getCell(indexedRow, ReferenceFields.Education).SetValue(reference.Edu ?? "");
            // getCell(indexedRow, ReferenceFields.Location).SetValue(reference.Location ?? "");
            // getCell(indexedRow, ReferenceFields.Semester).SetValue(reference.Semester ?? "");
            // getCell(indexedRow, ReferenceFields.Language).SetValue(reference.Language ?? "");
            // getCell(indexedRow, ReferenceFields.YearReport).SetValue(reference.YearReport);
            // getCell(indexedRow, ReferenceFields.Match).SetValue(reference.Match);
            // getCell(indexedRow, ReferenceFields.Comment).SetValue(reference.Commentary ?? "");
            // getCell(indexedRow, ReferenceFields.Syllabus).SetValue(reference.Syllabus ?? "");
            // getCell(indexedRow, ReferenceFields.Season).SetValue(reference.Season ?? "");
            // getCell(indexedRow, ReferenceFields.ExamEvent).SetValue(reference.ExamEvent ?? "");
            // getCell(indexedRow, ReferenceFields.Source).SetValue(reference.Source ?? "");
            // getCell(indexedRow, ReferenceFields.Pages).SetValue(reference.Pages);
            // getCell(indexedRow, ReferenceFields.Volume).SetValue(reference.Volume ?? "");
            // getCell(indexedRow, ReferenceFields.Chapters).SetValue(reference.Chapters ?? "");
            // getCell(indexedRow, ReferenceFields.BookTitle).SetValue(reference.BookTitle ?? "");
            getCell(indexedRow, ReferenceFields.Education).SetValue(reference.Education);
            getCell(indexedRow, ReferenceFields.Location).SetValue(reference.Location);
            getCell(indexedRow, ReferenceFields.Semester).SetValue(reference.Semester);
            getCell(indexedRow, ReferenceFields.RefId).SetValue(reference.RefId);
            getCell(indexedRow, ReferenceFields.OriginalRef).SetValue(reference.OriReference);
        }
        
        private void setReference(Reference reference, IXLRow indexedRow)
        {
            getCell(indexedRow, ReferenceFields.Author).SetValue(reference.Author ?? "");
            getCell(indexedRow, ReferenceFields.Title).SetValue(reference.Title ?? "");
            getCell(indexedRow, ReferenceFields.PublicationType).SetValue(reference.PubType ?? "");
            getCell(indexedRow, ReferenceFields.Publisher).SetValue(reference.Publisher ?? "");
            getCell(indexedRow, ReferenceFields.YearRef).SetValue(reference.YearRef);
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
            getCell(indexedRow, ReferenceFields.Education).SetValue(reference.Education);
            getCell(indexedRow, ReferenceFields.Location).SetValue(reference.Location);
            getCell(indexedRow, ReferenceFields.Semester).SetValue(reference.Semester);
            getCell(indexedRow, ReferenceFields.RefId).SetValue(reference.Id);
            getCell(indexedRow, ReferenceFields.OriginalRef).SetValue(reference.OriReference);
        }

        /// <summary>
        /// Appends multiple <c>references</c> to the next rows of the first worksheet
        /// </summary>
        /// <param name="references">Collection of references to be added</param>
        /// <param name="startRow">The start row from where the references should be inserted. If default, appends to end of list of references</param>
        /// <exception cref="ArgumentException">Throws if parameter startRow is 0 or less than -1</exception>
        public void AddRawReference(IEnumerable<RawReference> references, int startRow = -1)
        {
            // check if start row value is invalid, and throw an exception if it is
            if (startRow is 0 or < -1)
            {
                throw new ArgumentException("Start row cannot be 0 or less than -1");
            }
            
            // if start row is not specified, set it to the last row of the worksheet
            if (startRow == -1) startRow = Count + 1;
            
            // loop through the list of references and add each reference to the worksheet
            foreach (RawReference reference in references)
            {
                AddRawReference(reference, startRow++);
            }
        }
        
        /// <summary>
        /// : method to add a list of References to the Excel worksheet starting from a specified row
        /// </summary>
        /// <param name="references"></param>
        /// <param name="startRow"></param>
        /// <exception cref="ArgumentException"></exception>
        public void AddReference(IEnumerable<Reference> references, int startRow = -1)
        {
            // : check if start row value is invalid, and throw an exception if it is
            if (startRow is 0 or < -1)
            {
                throw new ArgumentException("Start row cannot be 0 or less than -1");
            }
            
            // : if start row is not specified, set it to the last row of the worksheet
            if (startRow == -1) startRow = Count + 1;
            
            // : loop through the list of references and add each reference to the worksheet
            foreach (Reference reference in references)
            {
                AddReference(reference, startRow++);
            }
        }

        /// <summary>
        /// Reads an Excel file corresponding to <c>FileName</c> and assigns it to <c>Workbook</c>
        /// </summary>
        /// <exception cref="FileNotFoundException">Throws if the file cannot be found</exception>
        // : Opens an excel workbook with the given file name and saves it in WorkbookProperty
        public void Import()
        {
            try
            {
                // : Open the excel workbook
                WorkbookProperty = new XLWorkbook(FileName); // : Note: It would be better to use a "using" statement so that it closes automatically and releases resources.
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
            // : If a file with the same name already exists, throw an IOException with a custom error message
            if (File.Exists(filename))
            {
                throw new IOException("File with this name already exists");
            }
            
            // : Save the workbook to the given file path
            WorkbookProperty?.SaveAs(Path.Join(FilePath + filename));
        }

        /// <summary>
        /// Gets the index of the reference matching the input reference.
        /// : Method to get the index of a RawReference object
        /// </summary>
        /// <param name="item">The Reference to find.</param>
        /// <returns>The index position of the reference, -1 if no such reference was found</returns>
        public int IndexOf(RawReference item)
        {
            int indexof = -1;
            
            // : iterating through the list to find the item
            for (int i = 1; i < Count; i++)
            {
                // : if the item is found, store the index in the variable 'indexof'
                if (this[i].Equals(item))
                {
                    indexof = i;
                }
            }
            
            // : return the index of the item
            return indexof;
        }

        /// <summary>
        /// Inserts the reference at the given index position
        /// : Method to insert a RawReference object at a specified index
        /// </summary>
        /// <param name="index">The index position to insert the reference</param>
        /// <param name="item">The reference to be inserted</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if the index position is less than 1 or higher than Count </exception>
        /// <remarks>To append a refernce to the list, use <c>Add</c> instead</remarks>
        public void Insert(int index, RawReference item)
        {
            // : check if the index is within the bounds (greater than 0 and less than or equal to MaxRowsInExcel)
            if (index is > 0 and <= MaxRowsInExcel)
            {
                // : add the item at the specified index
                AddRawReference(item, index);
            }
            else
            {
                // : if the index is out of bounds, throw an ArgumentOutOfRangeException
                throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Removes the reference at the given index
        /// : Method to remove an item at a specified index
        /// </summary>
        /// <param name="index">The index position</param>
        /// <exception cref="ArgumentException">Throws if the reference could not be deleted.</exception>
        /// <remarks>Deletes the row and shifts latter rows by 1 thus reducing Count by 1</remarks>
        public void RemoveAt(int index)
        {
            // : check if the index is within the bounds (greater than 0 and less than or equal to Count)
            if (index > 0 && index <= Count)
            {
                // : delete the row at the specified index
                XlWorksheet.Row(index).Delete();
            }
            else
            {
                // : if the index is out of bounds, throw an ArgumentException
                throw new ArgumentException("Error in deleting item");
            }
        }

        /// <summary>
        /// Appends a reference to the end of list
        /// : Method to add a RawReference object to the list
        /// </summary>
        /// <param name="item">The reference to append</param>
        public void Add(RawReference item)
        {
            // : add the item to the end of the list
            AddRawReference(item, Count + 1);
        }

        /// <summary>
        /// Deletes the active worksheet.
        /// : Method to clear the list
        /// </summary>
        public void Clear()
        {
            // : get the last row and column used in the worksheet
            IXLRow lastRow = XlWorksheet.LastRowUsed();
            IXLColumn lastColumn = XlWorksheet.LastColumnUsed();
            
            // : get the range of all rows in the worksheet
            IXLRange allRows = XlWorksheet.Range(1, 1, lastRow.RowNumber(), lastColumn.ColumnNumber());
            
            // : clear the range
            allRows.Clear();
        }

        /// <summary>
        /// Determines whether the reference exists in the list
        /// : Method to check if a RawReference object is present in the collection
        /// </summary>
        /// <param name="item">The reference to match</param>
        /// <returns>True </returns>
        public bool Contains(RawReference item)
        {
            bool doesContain = false;
            
            // : Loop through the collection to check for the item
            // Forloop giver bedre performance end foreach 
            for (int index = 0; index < this.Count; index++)
            {
                RawReference reference = this[index];
                if (reference.Equals(item))
                {
                    doesContain = true;
                }
            }
            // : Return if the item is present in the collection or not
            return doesContain;
        }

        [Obsolete]
        public void CopyTo(RawReference[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        /// Removes the Reference from the spreadsheet and shifts all rows below it up thus
        /// : Method to remove a RawReference object from the collection
        ///</summary>
        /// <remarks>
        /// Reduces Count by 1.
        /// Don't use in a loop, ClosedXML alleges poor performance consider using clear</remarks>
        public bool Remove(RawReference item)
        {
            // : Check if the item is not present in the collection
            if (IndexOf(item) == -1)
            {
                return false;
            }
            
            // : Delete the row containing the item
            IXLRow row = XlWorksheet.Row(IndexOf(item));
            row.Delete();
            return true;
        }

        /// <summary>
        /// : Method to get the enumerator for the collection
        /// </summary>
        /// <returns></returns>
        public IEnumerator<RawReference> GetEnumerator()
        {
            // : If the collection is empty, break the method
            if (Count == 0)
            {
                //Hvis count er 0 så bliver GetEnumerator kaldt rekursivt uden en stop case, idk why
                // : Note: If count is 0, the GetEnumerator method is called recursively without a stop case, idk why
                yield break;
            }
            
            // : Loop through the collection and return each item
            for (int index = 0; index < Count; index++)
            {
                RawReference item = this[index];
                yield return item;
            }
        }

        // : Method to get the enumerator for the collection
        IEnumerator IEnumerable.GetEnumerator()
        {
            // : Return the enumerator for the collection
            return GetEnumerator();
        }
    }
}
