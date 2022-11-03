using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using zenref.Models.Spreadsheet;
using ClosedXML.Excel;
using System.IO;
using zenref.Models;

namespace zenref.Tests
{
    public class SpreadsheetTest
    {
        const string SPREADSHEETTESTNAME = "test.xlsx";
        [Fact]
        public void CheckFilename()
        {
            //Arrange
            Spreadsheet testSpreadsheet = new Spreadsheet(SPREADSHEETTESTNAME);

            //Act
            bool result = testSpreadsheet.IsFileExcel();

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void ReadRefTest()
        {
            //Arrange
            Spreadsheet testSpreadsheet = new Spreadsheet(SPREADSHEETTESTNAME);

            //Act
            zenref.Models.Reference result = testSpreadsheet.ReadRef();

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ImportTestWhenFileFound()
        {
            //Arrange
            XLWorkbook tempWorkbook = new XLWorkbook();
            tempWorkbook.AddWorksheet("testsheet");
            tempWorkbook.SaveAs(SPREADSHEETTESTNAME);
            Spreadsheet testSpreadsheet = new Spreadsheet(SPREADSHEETTESTNAME);
            //Act
            testSpreadsheet.Import();

            //Assert
            Assert.True(testSpreadsheet.DoesExcelFileExist);
            File.Delete(SPREADSHEETTESTNAME);
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
        public void ExportTestWhenNotNull()
        {
            //Arrange
            Spreadsheet testSpreadsheet = new Spreadsheet(SPREADSHEETTESTNAME);
            File.Delete(SPREADSHEETTESTNAME);

            //Act
            Action exportNullWorkbook = () => testSpreadsheet.Export(SPREADSHEETTESTNAME);

            //Assert
            Assert.Throws<FileNotFoundException>(exportNullWorkbook);
            File.Delete(SPREADSHEETTESTNAME);
        }
        [Fact]
        public void CreateEmptySheet()
        {
            Spreadsheet spreadsheet = new Spreadsheet(SPREADSHEETTESTNAME);

            spreadsheet.Create();

            Assert.True(spreadsheet.DoesExcelFileExist);
        }
        [Fact]
        public void AddReferenceThrows()
        {
            Spreadsheet spreadsheet = new Spreadsheet(SPREADSHEETTESTNAME);

            Action addReference = () => { spreadsheet.AddReference(new Models.Reference()); };

            Assert.Throws<NotImplementedException>(addReference);
        }
        [Fact]
        public void AddReferenceIEnumberableThrows()
        {
            Spreadsheet spreadsheet = new Spreadsheet(SPREADSHEETTESTNAME);
            List<Models.Reference> listOfReferences = new List<Models.Reference>();


            listOfReferences.Add(new Models.Reference());
            listOfReferences.Add(new Models.Reference());

            Action addReference = () => { spreadsheet.AddReference(listOfReferences); };


            Assert.Throws<NotImplementedException>(addReference);
        }
        [Fact]
        public void GetWorkSheetsDictionaryContainsOneSheet()
        {
            const string TESTSHEET = "testsheet";
            const string SECONDTESTSHEET = "secondtestsheet";
            Spreadsheet spreadsheet = new Spreadsheet(SPREADSHEETTESTNAME);

            XLWorkbook tempWorkbook = new XLWorkbook();
            tempWorkbook.AddWorksheet(TESTSHEET);
            tempWorkbook.AddWorksheet(SECONDTESTSHEET);
            tempWorkbook.SaveAs(SPREADSHEETTESTNAME);

            spreadsheet.Import();

            Dictionary<int, string> sheetDic = spreadsheet.GetWorksheets();

            Assert.Equal<Dictionary<int, string>>(sheetDic, new Dictionary<int, string>()
            {
                { 1, TESTSHEET },
                {2, SECONDTESTSHEET },
            }
            );
            File.Delete(SPREADSHEETTESTNAME);
        }
    }
}
