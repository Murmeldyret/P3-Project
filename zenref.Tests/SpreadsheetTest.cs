using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Zenref.Ava.Models.Spreadsheet;
using ClosedXML.Excel;
using System.IO;
using Zenref.Ava.Models;

namespace zenref.Tests
{
    public class SpreadsheetTest
    {
        const string SPREADSHEETTESTNAME = "test.xlsx";
        [Fact]
        public void CheckFilename()
        {
            //Arrange
            XLWorkbook workbook = new XLWorkbook();
            workbook.AddWorksheet("Temp");
            workbook.SaveAs(SPREADSHEETTESTNAME);
            Spreadsheet testSpreadsheet = new Spreadsheet(SPREADSHEETTESTNAME);

            //Act
            testSpreadsheet.Import();
            bool result = testSpreadsheet.IsFileExcel();

            //Assert
            Assert.True(result);
            File.Delete(SPREADSHEETTESTNAME);
        }

        //[Fact]
        //public void ReadRefTest()
        //{
        //    //Arrange
        //    Spreadsheet testSpreadsheet = new Spreadsheet(SPREADSHEETTESTNAME);

        //    //Act
        //    Reference result = testSpreadsheet.ReadRef();

        //    //Assert
        //    Assert.NotNull(result);
        //}

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

            Action importNonExistentFile = TestUnFindableSpreadsheet.Import;

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
        public void GetWorkSheetsDictionaryContainsTwoSheets()
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
                {1, TESTSHEET },
                {2, SECONDTESTSHEET },
            }
            );
            File.Delete(SPREADSHEETTESTNAME);
        }
        [Fact]
        public void SetActiveSheetIntExistingSheet()
        {
            const string SHEETNAME = "test";
            File.Delete(SPREADSHEETTESTNAME);
            XLWorkbook workbook = new XLWorkbook();
            workbook.AddWorksheet(SHEETNAME, 1);
            workbook.SaveAs(SPREADSHEETTESTNAME);
            Spreadsheet spreadsheet = new Spreadsheet(SPREADSHEETTESTNAME);
            spreadsheet.Import();
            Dictionary<int, string> sheetdic = spreadsheet.GetWorksheets();

            spreadsheet.SetActiveSheet(1);

            Assert.True(sheetdic[1] == SHEETNAME);
            File.Delete(SPREADSHEETTESTNAME);
        }
        [Fact]
        public void SetActiveSheetIntNonExistentSheet()
        {
            File.Delete(SPREADSHEETTESTNAME);
            XLWorkbook workbook = new XLWorkbook();
            workbook.AddWorksheet(1);
            workbook.SaveAs(SPREADSHEETTESTNAME);

            Spreadsheet spreadsheet = new Spreadsheet(SPREADSHEETTESTNAME);
            spreadsheet.Import();

            spreadsheet.SetActiveSheet(2);
            Dictionary<int, string> sheetdic = spreadsheet.GetWorksheets();
            string sheet2Name = sheetdic[2]; //FIXME Test giver ikke mening

            Assert.True(sheetdic[2] == sheet2Name);
            File.Delete(SPREADSHEETTESTNAME);

        }
        [Fact]
        public void SetActiveSheetStringExistingSheet()
        {
            const string SHEETNAME = "test";

            File.Delete(SPREADSHEETTESTNAME);
            XLWorkbook workbook = new XLWorkbook();
            workbook.AddWorksheet(SHEETNAME);
            workbook.SaveAs(SPREADSHEETTESTNAME);

            Spreadsheet spreadsheet = new Spreadsheet(SPREADSHEETTESTNAME);
            spreadsheet.Import();


            spreadsheet.SetActiveSheet(SHEETNAME);
            Dictionary<int, string> sheetdic = spreadsheet.GetWorksheets();
            string sheetname;
            sheetdic.TryGetValue(1, out sheetname!);

            Assert.True(sheetname == SHEETNAME);
            File.Delete(SPREADSHEETTESTNAME);

        }
        [Fact]
        public void SetActiveSheetStringNonExistentSheet()
        {
            const string SHEETNAME = "test";

            File.Delete(SPREADSHEETTESTNAME);
            XLWorkbook workbook = new XLWorkbook();
            workbook.AddWorksheet("A bad sheetname");
            workbook.SaveAs(SPREADSHEETTESTNAME);

            Spreadsheet spreadsheet = new Spreadsheet(SPREADSHEETTESTNAME);
            spreadsheet.Import();

            spreadsheet.SetActiveSheet(SHEETNAME);
            Dictionary<int, string> sheetdic = spreadsheet.GetWorksheets();
            string sheetname;
            sheetdic.TryGetValue(2, out sheetname!);

            Assert.True(sheetname == SHEETNAME);
            File.Delete(SPREADSHEETTESTNAME);
        }
    }
}
