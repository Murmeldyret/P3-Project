using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zenref.Models
{
    public class Spreadsheet
    {
        private string FileName { get; }
        // public string FilePath { get; }
        public int ReferenceCount { get; }
        public bool State { get; }
        // public string Worksheet { get; } // Egen klasse...

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
        public bool Open()
        {
            throw new NotImplementedException();
        }

        /**
         * Closes File and verifies if it closed
         * @param fileName
        */
        public bool Close()
        {
            throw new NotImplementedException();
        }

        /**
         * Create a new Excel file and verifies it
         * @param fileName
        */
        public bool Create()
        {
            throw new NotImplementedException();
        }
    }
}
