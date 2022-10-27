using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using zenref.Models;

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
        public void ImportTest()
        {
            //Arrange
            Spreadsheet testSpreadsheet = new Spreadsheet("test.xlsx");

            //Act
            bool result = testSpreadsheet.Import();

            //Assert
            Assert.True(result);
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
