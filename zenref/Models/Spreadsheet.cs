﻿using System;
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
        public bool DoesExcelFileExist { get => _workbook is not null; }
        // public string Worksheet { get; } // Egen klasse...
        protected XLWorkbook? _workbook
        {
            get => _workbook ?? throw new FileNotFoundException($"{nameof(_workbook)} is null, use import() to fill this property");
            set => _workbook = value;
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

        /// <summary>
        /// Checks if <c>FileName</c> is an Excelfile
        /// </summary>
        /// <returns><c>true</c> if <c>FileName</c>suffix is .xlsx, <c>false</c> otherwise </returns>
        public bool IsFileExcel()
        {
            throw new NotImplementedException();
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
        /// <param name="filename">The name of the Excelfile</param>
        /// <exception cref="ArgumentNullException">If Workbook is null, this exception is thrown</exception>
        public void Export(string filename)
        {
            if (_workbook is null)
            {
                throw new ArgumentNullException("Workbook cannot be saved when it is null");
            }
            else
            {
            _workbook.SaveAs(filename);
            }
            //throw new NotImplementedException();
            // Export spreadsheet....
            // Window.close(); 
        }
    }
}