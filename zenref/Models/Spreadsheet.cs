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
        public int ReferenceCount { get; }
        public bool State { get; }
        // public string Worksheet { get; } // Egen klasse...
        protected XLWorkbook? Workbook
        {
            get => Workbook ?? throw new FileNotFoundException();
            set => Workbook = value;
        }

        public Spreadsheet(string fileName)
        {
            FileName = fileName;
        }

        /**
         * Reads the content of the file
         * @param fileName
        */
        public bool ReadRef()
        {
            throw new NotImplementedException();
        }
        
        /**
         * Check the file if it is a valid file
         * @param fileName
        */
        public bool IsFileExcel()
        {
            throw new NotImplementedException();
        }

        /**
         * Opens File and verifies if it opened
         * @param fileName
        */
        public void Import()
        {
            //throw new NotImplementedException();
            try
            {
            Workbook = new XLWorkbook(FileName);
            }
            catch (ArgumentException ex)
            {
                throw new FileNotFoundException("File not found\n" + ex.Message);
            }

        }

        /**
         * Create a new Excel file and verifies it.
         * @param fileName
        */
        public bool Export()
        {
            throw new NotImplementedException();
            // Export spreadsheet....
            // Window.close(); 
        }
    }
}
