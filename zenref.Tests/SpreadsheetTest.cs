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
        public void OpenTest()
        {
            //Arrange
            Spreadsheet testSpreadsheet = new Spreadsheet("test.xlsx");

            //Act
            bool result = testSpreadsheet.Open();

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void CloseTest()
        {
            //Arrange
            Spreadsheet testSpreadsheet = new Spreadsheet("test.xlsx");

            //Act
            bool result = testSpreadsheet.Close();

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void CreateTest()
        {
            //Arrange
            Spreadsheet testSpreadsheet = new Spreadsheet("test.xlsx");

            //Act
            bool result = testSpreadsheet.Create();

            //Assert
            Assert.True(result);
        }
    }
}
