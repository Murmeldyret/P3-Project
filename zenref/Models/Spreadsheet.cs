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

        //Reference field positioning   



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
            throw new NotImplementedException();
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
            else if (File.Exists(filename))
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
