using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using zenref.Models.Spreadsheet;
using ClosedXML.Excel;
using System.IO;

namespace zenref.Tests
{
    public class SpreadsheetTest
    {
        [Fact]
        public void CheckFilename()
        {
            //Arrange
            Spreadsheet testSpreadsheet = new Spreadsheet("test.xlsx");

            //Act
            bool result = testSpreadsheet.IsFileExcel();

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void ReadRefTest()
        {
            //Arrange
            Spreadsheet testSpreadsheet = new Spreadsheet("test.xlsx");

            //Act
            bool result = testSpreadsheet.ReadRef();

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void ImportTestWhenFileFound()
        {
            //Arrange
            const string testFileName = "test.xlsx";
            XLWorkbook tempWorkbook = new XLWorkbook();
            tempWorkbook.AddWorksheet("testsheet");
            tempWorkbook.SaveAs(testFileName);
            Spreadsheet testSpreadsheet = new Spreadsheet(testFileName);
            //Act
            testSpreadsheet.Import();

            //Assert
            Assert.True(testSpreadsheet.DoesExcelFileExist);
            File.Delete(testFileName);
        }
        [Fact]
        public void ImportTestWhenFileNotFound()
        {
            const string NonExistentExcelFileName = "DoesNotExsist.xlsx";
            Spreadsheet TestUnFindableSpreadsheet = new Spreadsheet(NonExistentExcelFileName);

            Action importNonExistentFile = () => TestUnFindableSpreadsheet.Import();

            Assert.Throws<FileNotFoundException>(importNonExistentFile);

        }
        [Fact]
        public void ExportTest()
        {
            //Arrange
            Spreadsheet testSpreadsheet = new Spreadsheet("test.xlsx");

            //Act
            bool result = testSpreadsheet.Export();

            //Assert
            Assert.True(result);
        }
    }
}
