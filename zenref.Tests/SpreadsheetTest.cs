using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using zenref.Ava.Models.Spreadsheet;
using ClosedXML.Excel;
using System.IO;
using zenref.Ava.Models;
using System.Collections;
using DeepEqual;
using DeepEqual.Syntax;

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
        [Fact]
        public void ReadRefReadsFieldsCorrectly()
        {
            const string FILLEDSPREADSHEETNAME = "ReadRefTest.xlsx";
            const string SHEETNAME = "test";

            File.Delete(FILLEDSPREADSHEETNAME);

            XLWorkbook wb = new XLWorkbook();
            Spreadsheet sheet = new Spreadsheet(FILLEDSPREADSHEETNAME);

            IXLWorksheet ws = wb.AddWorksheet(SHEETNAME);
            IXLRow firstrow = ws.Row(1);
            //IXLRange firstrow = ws.Range(1, 1, 1, 22);

            for (int i = 0; i < sheet.PositionOfReferencesInSheet.Count; i++)
            {
                firstrow.Cell(i + 1).SetValue<string>(sheet.PositionOfReferencesInSheet.ElementAt(i).Key.ToString());
            }
            IXLRow secondrow = ws.Row(2);
            secondrow.Cell(1).SetValue<string>("Anders Rask");
            secondrow.Cell(2).SetValue<string>("titel på noget");
            secondrow.Cell(3).SetValue<string>("bog");
            secondrow.Cell(4).SetValue<string>("AAU");
            secondrow.Cell(5).SetValue<int>(2022);
            secondrow.Cell(6).SetValue<int>(12345);
            secondrow.Cell(7).SetValue<string>("Software");
            secondrow.Cell(8).SetValue<string>("Aalborg");
            secondrow.Cell(9).SetValue<string>("Tredje");
            secondrow.Cell(10).SetValue<string>("Dansk");
            secondrow.Cell(11).SetValue<int>(2021);
            secondrow.Cell(12).SetValue<string>("lang tekst");
            secondrow.Cell(13).SetValue<double>(0.9);
            secondrow.Cell(14).SetValue<string>("blank");
            secondrow.Cell(15).SetValue<string>("Det ved jeg ikke");
            secondrow.Cell(16).SetValue<string>("Efterår");
            secondrow.Cell(17).SetValue<string>("pas");
            secondrow.Cell(18).SetValue<string>("ved jeg heller ikke");
            secondrow.Cell(19).SetValue<int>(69);
            secondrow.Cell(20).SetValue<string>("20th");
            secondrow.Cell(21).SetValue<string>("16-21");
            secondrow.Cell(22).SetValue<string>("Din far");
            wb.SaveAs(FILLEDSPREADSHEETNAME);

            //Read reference should be equal to this
            Reference reference = new Reference(new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, ""),
                "Anders Rask",
                "titel på noget",
                "bog",
                "AAU",
                2022,
                12345,
                "Software",
                "Aalborg",
                "Tredje",
                "Dansk",
                2021,
                0.9,
                "blank",
                "Det ved jeg ikke",
                "Efterår",
                "pas",
                "ved jeg heller ikke",
                69,
                "20th",
                "16-21",
                "Din far");

            sheet.Import();
            Reference importedReference = sheet.GetReference(2);

            //Equivalent verifies that each public property is the same
            Assert.Equivalent(reference, importedReference);


        }
        [Fact]
        public void IListIndexerGetterWorks()
        {
            const string FILLEDSPREADSHEETNAME = "ReadRefTest.xlsx";
            const string SHEETNAME = "test";

            File.Delete(FILLEDSPREADSHEETNAME);

            XLWorkbook wb = new XLWorkbook();
            Spreadsheet sheet = new Spreadsheet(FILLEDSPREADSHEETNAME);

            IXLWorksheet ws = wb.AddWorksheet(SHEETNAME);
            IXLRow firstrow = ws.Row(1);
            //IXLRange firstrow = ws.Range(1, 1, 1, 22);

            for (int i = 0; i < sheet.PositionOfReferencesInSheet.Count; i++)
            {
                firstrow.Cell(i + 1).SetValue<string>(sheet.PositionOfReferencesInSheet.ElementAt(i).Key.ToString());
            }
            IXLRow secondrow = ws.Row(2);
            secondrow.Cell(1).SetValue<string>("Anders Rask");
            secondrow.Cell(2).SetValue<string>("titel på noget");
            secondrow.Cell(3).SetValue<string>("bog");
            secondrow.Cell(4).SetValue<string>("AAU");
            secondrow.Cell(5).SetValue<int>(2022);
            secondrow.Cell(6).SetValue<int>(12345);
            secondrow.Cell(7).SetValue<string>("Software");
            secondrow.Cell(8).SetValue<string>("Aalborg");
            secondrow.Cell(9).SetValue<string>("Tredje");
            secondrow.Cell(10).SetValue<string>("Dansk");
            secondrow.Cell(11).SetValue<int>(2021);
            secondrow.Cell(12).SetValue<string>("lang tekst");
            secondrow.Cell(13).SetValue<double>(0.9);
            secondrow.Cell(14).SetValue<string>("blank");
            secondrow.Cell(15).SetValue<string>("Det ved jeg ikke");
            secondrow.Cell(16).SetValue<string>("Efterår");
            secondrow.Cell(17).SetValue<string>("pas");
            secondrow.Cell(18).SetValue<string>("ved jeg heller ikke");
            secondrow.Cell(19).SetValue<int>(69);
            secondrow.Cell(20).SetValue<string>("20th");
            secondrow.Cell(21).SetValue<string>("16-21");
            secondrow.Cell(22).SetValue<string>("Din far");
            wb.SaveAs(FILLEDSPREADSHEETNAME);


            sheet.Import();

            Assert.Equal("Software", sheet[2].Edu);

            File.Delete(FILLEDSPREADSHEETNAME);
        }
        [Fact] //ha to referencer og de ska være equivalente.
               //Check for value reference
        public void IndexOf()
        {
            //Arrange
            const string FILLEDSPREADSHEETNAME = "ReadRefTest.xlsx";


            File.Delete(FILLEDSPREADSHEETNAME);

            Spreadsheet sheet = new Spreadsheet(FILLEDSPREADSHEETNAME);


            //Act

            //Assert

            File.Delete(FILLEDSPREADSHEETNAME);
        }

        [Fact]  //Tar to værdier. index og reference som skal ind.
        public void InsertUnoccupiedRow()
        {
            //Arrange
            const string FILLEDSPREADSHEETNAME = "ReadRefTest.xlsx";

            File.Delete(FILLEDSPREADSHEETNAME);

            Spreadsheet sheet = new Spreadsheet(FILLEDSPREADSHEETNAME);

            Reference reference1 = new Reference(
                new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, ""),
                "Anders er ikke rask",
                "wololo"
                );

            Reference reference2 = new Reference(
                new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, ""),
                "True Winner"
                );
            Reference reference3 = new Reference(
                new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, ""),
                "True Winner"
                );

            //Act
            sheet.Create();
            sheet.Insert(1, reference1);
            sheet.Insert(2, reference2);
            sheet.Insert(3, reference3);

            //Assert
            sheet.Export(FILLEDSPREADSHEETNAME);
            Assert.Equal(3, sheet.Count);
            File.Delete(FILLEDSPREADSHEETNAME);
        }

        [Fact]  //Tar to værdier. index og reference som skal ind.
                //den overskriver hvis der allerede er en.
        public void InsertOnOccupiedRow()
        {
            //Arrange
            const string FILLEDSPREADSHEETNAME = "ReadRefTest.xlsx";

            File.Delete(FILLEDSPREADSHEETNAME);

            Spreadsheet sheet = new Spreadsheet(FILLEDSPREADSHEETNAME);

            Reference reference1 = new Reference(
                new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, ""),
                "Anders er ikke rask",
                "wololo"
                );

            Reference reference2 = new Reference(
                new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, ""),
                "True Winner"
                );
            Reference reference3 = new Reference(
                new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, ""),
                "True Winner"
                );

            //Act
            sheet.Create();
            sheet.Insert(1, reference1);
            sheet.Insert(2, reference2);
            sheet.Insert(1, reference3);
            bool result = sheet[1].IsDeepEqual(reference3);

            //Assert
            //Er reference 1 stadig på index 1?
            sheet.Export(FILLEDSPREADSHEETNAME);
            Assert.True(result);
            File.Delete(FILLEDSPREADSHEETNAME);
            
        }

        [Fact]  //Appendering af reference til spreadsheet
                // tager kun reference som input
        public void Add()
        {
            //Arrange
            const string FILLEDSPREADSHEETNAME = "ReadRefTest.xlsx";

            File.Delete(FILLEDSPREADSHEETNAME);

            Spreadsheet sheet = new Spreadsheet(FILLEDSPREADSHEETNAME);
            
            Reference reference1 = new Reference(
                new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, ""),
                "Anders er ikke rask",
                "wololo"
                );
            Reference reference2 = new Reference(
                new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, ""),
                "True Winner"
                );

            //Act
            sheet.Create();
            sheet.Add(reference1);
            sheet.Add(reference2);

            //Assert
            Assert.Equal(2, sheet.Count);
            File.Delete(FILLEDSPREADSHEETNAME);
        }

        [Fact]  //Tog et index som parameter og fjerne hvad
                //end der er på rækken uden at slette rækken
        public void RemoveAt()
        {
            //Arrange
            const string FILLEDSPREADSHEETNAME = "ReadRefTest.xlsx";

            File.Delete(FILLEDSPREADSHEETNAME);

            Spreadsheet sheet = new Spreadsheet(FILLEDSPREADSHEETNAME);
            Reference reference1 = new Reference(
                new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, ""),
                "Anders er ikke rask",
                "wololo"
                );

            Reference reference2 = new Reference(
                new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, ""),
                "True Winner"
                );
            Reference reference3 = new Reference(
                new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, ""),
                "True Winner"
                );

            //Act
            sheet.Create();
            sheet.AddReference(reference1, 1);
            sheet.AddReference(reference2, 2);
            sheet.AddReference(reference3, 3);
            sheet.RemoveAt(2);

            //Assert
            Assert.Equal(2, sheet.Count);
            File.Delete(FILLEDSPREADSHEETNAME);
        }
        [Fact]  //Clear worksheet
        public void ClearWorksheet()
        {
            //Arrange
            /*const string FILLEDSPREADSHEETNAME = "ReadRefTest.xlsx";
            const string SHEETNAME = "test";
            const string SHEETNAMETWO = "test1";

            File.Delete(FILLEDSPREADSHEETNAME);

            XLWorkbook wb = new XLWorkbook();
            Spreadsheet sheet = new Spreadsheet(FILLEDSPREADSHEETNAME);

            IXLWorksheet ws = wb.AddWorksheet(SHEETNAME);
            IXLWorksheet ws2 = wb.AddWorksheet(SHEETNAMETWO);

            Reference reference1 = new Reference(
                new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, ""),
                "Anders er ikke rask",
                "wololo"
                );

            //Act
            sheet.Create();
            sheet.AddReference(reference1, 1);
            ws.Clear();

            //Assert
            sheet.Export(FILLEDSPREADSHEETNAME);
            Assert.Empty(sheet);
            File.Delete(FILLEDSPREADSHEETNAME);*/
        }

        [Fact]  //TODO
        public void RemoveRowContent()
        {
            //Arrange
            const string FILLEDSPREADSHEETNAME = "ReadRefTest.xlsx";

            File.Delete(FILLEDSPREADSHEETNAME);
          
            Spreadsheet sheet = new Spreadsheet(FILLEDSPREADSHEETNAME);

            Reference reference1 = new Reference(
                new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, ""),
                "Anders er ikke rask",
                "wololo"
                );

            Reference reference2 = new Reference(
                new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, ""),
                "True Winner"
                );
            Reference reference3 = new Reference(
                new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, ""),
                "True Winner"
                );

            //Act
            sheet.Create();
            sheet.AddReference(reference1, 1);
            sheet.AddReference(reference2, 2);
            sheet.AddReference(reference3, 3);
            sheet.Remove(reference2);
            bool res = sheet[2].IsDeepEqual(reference2);

            //Assert
            sheet.Export(FILLEDSPREADSHEETNAME);
            //Assert.Equivalent(reff, reference2);
            Assert.False(res);
            //File.Delete(FILLEDSPREADSHEETNAME);
        }

        [Fact]
        public void CountNotNullRows()
        {
            //Arrange
            const string FILLEDSPREADSHEETNAME = "ReadRefTest.xlsx";

            File.Delete(FILLEDSPREADSHEETNAME);

            Spreadsheet sheet = new Spreadsheet(FILLEDSPREADSHEETNAME);

            Reference reference1 = new Reference(
                new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, ""),
                "Anders er ikke rask",
                "wololo"
                );

            Reference referencenull = new Reference(
                new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, "")
                );

            //Act
            sheet.Create();
            sheet.AddReference(reference1,1);
            sheet.AddReference(reference1,2);
            sheet.AddReference(referencenull,3);
            sheet.AddReference(referencenull,4);
            sheet.AddReference(reference1,5);

            //Assert
            Assert.Equal(3, sheet.Count);
            File.Delete(FILLEDSPREADSHEETNAME);
        }
        [Fact]
        public void AddReferenceWorksAtCorrectField()
        {
            const int ROWTOINSERTAT = 4;
            File.Delete(SPREADSHEETTESTNAME);
            Spreadsheet spreadsheet = new Spreadsheet(SPREADSHEETTESTNAME);

            spreadsheet.Create();

            spreadsheet.AddReference(new Reference(new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, ""),
                                     _Author: "Din far"),ROWTOINSERTAT);
            spreadsheet.Export(SPREADSHEETTESTNAME);

            XLWorkbook wb = new XLWorkbook(SPREADSHEETTESTNAME);
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow row = ws.Row(ROWTOINSERTAT);
            bool isEmpty = row.IsEmpty();

            Assert.False(isEmpty); ;


        }
    }
}
