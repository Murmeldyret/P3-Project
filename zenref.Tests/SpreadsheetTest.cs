using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Zenref.Ava.Models.Spreadsheet;
using ClosedXML.Excel;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;
using Zenref.Ava.Factories;
using Zenref.Ava.Models;


namespace zenref.Tests
{
    public class SpreadsheetTest
    {
        const string SPREADSHEETTESTNAME = "test.xlsx";
        const string FILLEDSPREADSHEETNAME = "ReadRefTest.xlsx";

        XLWorkbook workbook = new();
        Spreadsheet spreadsheet = new(SPREADSHEETTESTNAME);
        Spreadsheet sheet = new(FILLEDSPREADSHEETNAME);

        [Fact]
        public void CheckFilename()
        {
            //Arrange
            workbook.AddWorksheet("Temp");
            workbook.SaveAs(SPREADSHEETTESTNAME);

            //Act
            spreadsheet.Import();
            bool result = spreadsheet.IsFileExcel();

            //Assert
            Assert.True(result);
            File.Delete(SPREADSHEETTESTNAME);
        }

        [Fact]
        public void ImportTestWhenFileFound()
        {
            //Arrange
            workbook.AddWorksheet("testsheet");
            workbook.SaveAs(SPREADSHEETTESTNAME);
            //Act
            spreadsheet.Import();

            //Assert
            Assert.True(spreadsheet.DoesExcelFileExist);
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
            File.Delete(SPREADSHEETTESTNAME);

            //Act
            Action exportNullWorkbook = () => spreadsheet.Export(SPREADSHEETTESTNAME);

            //Assert
            Assert.Throws<FileNotFoundException>(exportNullWorkbook);
            File.Delete(SPREADSHEETTESTNAME);
        }
        [Fact]
        public void CreateEmptySheet()
        { 
            spreadsheet.Create();

            Assert.True(spreadsheet.DoesExcelFileExist);
        }
        [Fact]
        public void GetWorkSheetsDictionaryContainsTwoSheets()
        {
            const string TESTSHEET = "testsheet";
            const string SECONDTESTSHEET = "secondtestsheet";

            workbook.AddWorksheet(TESTSHEET);
            workbook.AddWorksheet(SECONDTESTSHEET);
            workbook.SaveAs(SPREADSHEETTESTNAME);

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
            workbook.AddWorksheet(SHEETNAME, 1);
            workbook.SaveAs(SPREADSHEETTESTNAME);
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
            workbook.AddWorksheet(1);
            workbook.SaveAs(SPREADSHEETTESTNAME);

            spreadsheet.Import();

            spreadsheet.SetActiveSheet(2);
            Dictionary<int, string> sheetdic = spreadsheet.GetWorksheets();
            string sheet2Name = sheetdic[1];

            Assert.False(sheetdic[2] == sheet2Name);
            File.Delete(SPREADSHEETTESTNAME);
        }
        [Fact]
        public void SetActiveSheetStringExistingSheet()
        {
            const string SHEETNAME = "test";

            File.Delete(SPREADSHEETTESTNAME);
            workbook.AddWorksheet(SHEETNAME);
            workbook.SaveAs(SPREADSHEETTESTNAME);

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
            workbook.AddWorksheet("A bad sheetname");
            workbook.SaveAs(SPREADSHEETTESTNAME);

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
            const string SHEETNAME = "test";

            File.Delete(FILLEDSPREADSHEETNAME);

            IXLWorksheet ws = workbook.AddWorksheet(SHEETNAME);
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
            secondrow.Cell(6).SetValue<string>("12345");
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
            workbook.SaveAs(FILLEDSPREADSHEETNAME);

            //Read reference should be equal to this
            RawReference reference = new RawReference("Software",
                "Aalborg",
                "Tredje",
                "12345",
                "lang tekst");

            sheet.Import();
            RawReference importedReference = sheet.GetRawReference(2);

            //Equivalent verifies that each public property is the same
            Assert.Equivalent(reference, importedReference);


        }
        [Fact]
        public void IListIndexerGetterWorks()
        {
            const string SHEETNAME = "test";

            File.Delete(FILLEDSPREADSHEETNAME);

            IXLWorksheet ws = workbook.AddWorksheet(SHEETNAME);
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
            workbook.SaveAs(FILLEDSPREADSHEETNAME);

            sheet.Import();

            Assert.Equal("Software", sheet[2].Education);

            File.Delete(FILLEDSPREADSHEETNAME);
        }
        [Fact] //ha to referencer og de ska være equivalente.
               //Check for value reference
        public void IndexOf()
        {
            // Create a new reference object
            //Arrange
            File.Delete(FILLEDSPREADSHEETNAME);
            
            var factory = new ReferenceFactory();
            var reference1 = factory.CreateRawReference("Software", "Aalborg", "Tredje", "12345", "lang tekst");
            var reference2 = factory.CreateRawReference("Noget andet end software", "Copenhagen", "First", "42069", "I made it up");
            var reference3 = factory.CreateRawReference("Noget kedeligt", "Mou", "Second", "42", "John");

            //Act
            sheet.Create();
            sheet.AddRawReference(new List<RawReference>() { reference1, reference2, reference3 });
            sheet.Export(FILLEDSPREADSHEETNAME);
            //Assert
            int index = sheet.IndexOf(reference2);

            Assert.Equal(2, index);
            File.Delete(FILLEDSPREADSHEETNAME);
        }

        [Fact]  //Tar to værdier. index og reference som skal ind.
        public void InsertUnoccupiedRow()
        {
            //Arrange
            File.Delete(FILLEDSPREADSHEETNAME);
            
            var factory = new ReferenceFactory();
            var reference1 = factory.CreateRawReference("Software", "Aalborg", "Tredje", "12345", "lang tekst");
            var reference2 = factory.CreateRawReference("Noget andet end software", "Copenhagen", "First", "42069", "I made it up");
            var reference3 = factory.CreateRawReference("Noget kedeligt", "Mou", "Second", "42", "John");

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
            File.Delete(FILLEDSPREADSHEETNAME);
            
            var factory = new ReferenceFactory();
            var reference1 = factory.CreateRawReference("Software", "Aalborg", "Tredje", "12345", "lang tekst");
            var reference2 = factory.CreateRawReference("Noget andet end software", "Copenhagen", "First", "42069", "I made it up");
            var reference3 = factory.CreateRawReference("Noget kedeligt", "Mou", "Second", "42", "John");

            //Act
            sheet.Create();
            sheet.Insert(1, reference1);
            sheet.Insert(2, reference2);
            sheet.Insert(1, reference3);
            bool result = sheet[1].Equals(reference3);

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
            File.Delete(FILLEDSPREADSHEETNAME);
            
            var factory = new ReferenceFactory();
            var reference1 = factory.CreateRawReference("Software", "Aalborg", "Tredje", "12345", "lang tekst");
            var reference2 = factory.CreateRawReference("Noget andet end software", "Copenhagen", "First", "42069", "I made it up");

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
            File.Delete(FILLEDSPREADSHEETNAME);
            
            var factory = new ReferenceFactory();
            var reference1 = factory.CreateRawReference("Software", "Aalborg", "Tredje", "12345", "lang tekst");
            var reference2 = factory.CreateRawReference("Noget andet end software", "Copenhagen", "First", "42069", "I made it up");
            var reference3 = factory.CreateRawReference("Noget kedeligt", "Mou", "Second", "42", "John");

            //Act
            sheet.Create();
            sheet.AddRawReference(reference1, 1);
            sheet.AddRawReference(reference2, 2);
            sheet.AddRawReference(reference3, 3);
            sheet.RemoveAt(2);

            //Assert
            Assert.Equal(2, sheet.Count);
            File.Delete(FILLEDSPREADSHEETNAME);
        }
        [Fact]  //Clear worksheet
        public void ClearWorksheet()
        {
            //Arrange
            const string SHEETNAME = "test";

            File.Delete(FILLEDSPREADSHEETNAME);

            IXLWorksheet ws = workbook.AddWorksheet(SHEETNAME);
            
            var factory = new ReferenceFactory();
            var reference1 = factory.CreateRawReference("Software", "Aalborg", "Tredje", "12345", "lang tekst");

            //Act
            sheet.Create();
            sheet.AddRawReference(reference1, 1);
            ws.Clear();
            sheet.Clear();

            //Assert
            sheet.Export(FILLEDSPREADSHEETNAME);
            Assert.Empty(sheet);

            File.Delete(FILLEDSPREADSHEETNAME);
        }

        [Fact]
        public void RemoveRowContent()
        {
            //Arrange
            File.Delete(FILLEDSPREADSHEETNAME);
            
            var factory = new ReferenceFactory();
            var reference1 = factory.CreateRawReference("Software", "Aalborg", "Tredje", "12345", "lang tekst");
            var reference2 = factory.CreateRawReference("Noget andet end software", "Copenhagen", "First", "42069", "I made it up");
            var reference3 = factory.CreateRawReference("Noget kedeligt", "Mou", "Second", "42", "John");

            //Act
            sheet.Create();
            sheet.AddRawReference(reference1, 1);
            sheet.AddRawReference(reference2, 2);
            sheet.AddRawReference(reference3, 3);
            sheet.Remove(reference2);
            bool res = sheet[2].Equals(reference2);

            //Assert
            sheet.Export(FILLEDSPREADSHEETNAME);
            Assert.False(res);
            File.Delete(FILLEDSPREADSHEETNAME);
        }

        [Fact]
        public void CountNotNullRows()
        {
            //Arrange
            File.Delete(FILLEDSPREADSHEETNAME);
            RawReference referencenull = new RawReference(null,null,null,null,null);

            var factory = new ReferenceFactory();
            var reference1 = factory.CreateRawReference("Software", "Aalborg", "Tredje", "12345", "lang tekst");
            
            //Act
            sheet.Create();
            sheet.AddRawReference(reference1,1);
            sheet.AddRawReference(reference1,2);
            sheet.AddRawReference(referencenull,3);
            sheet.AddRawReference(referencenull,4);
            sheet.AddRawReference(reference1,5);

            //Assert
            Assert.Equal(3, sheet.Count);
            File.Delete(FILLEDSPREADSHEETNAME);
        }
        [Fact]
        public void AddReferenceWorksAtCorrectField()
        {
            const int ROWTOINSERTAT = 4;
            File.Delete(SPREADSHEETTESTNAME);

            spreadsheet.Create();

            spreadsheet.AddRawReference(new RawReference(null,null,null,null,"very cool"),ROWTOINSERTAT);
            spreadsheet.Export(SPREADSHEETTESTNAME);

            XLWorkbook wb = new XLWorkbook(SPREADSHEETTESTNAME);
            IXLWorksheet ws = wb.Worksheet(1);
            IXLRow row = ws.Row(ROWTOINSERTAT);
            bool isEmpty = row.IsEmpty();

            Assert.False(isEmpty);
        }
        [Fact]
        public void SwapReferencePropertySwaps()
        {
            //Dictionary needs to be copied because of the way reference types work :/
            Dictionary<Spreadsheet.ReferenceFields, int> originalDic = new Dictionary<Spreadsheet.ReferenceFields, int>(spreadsheet.PositionOfReferencesInSheet);
            spreadsheet.SwapReferenceProperty(Spreadsheet.ReferenceFields.Author,Spreadsheet.ReferenceFields.Title);

            Dictionary<Spreadsheet.ReferenceFields, int> newDic = spreadsheet.PositionOfReferencesInSheet;

            Tuple<int, int> oldDicValues = new Tuple<int, int>(originalDic[Spreadsheet.ReferenceFields.Author], originalDic[Spreadsheet.ReferenceFields.Title]);
            Tuple<int, int> newDicValues = new Tuple<int, int>(newDic[Spreadsheet.ReferenceFields.Author], newDic[Spreadsheet.ReferenceFields.Title]);

            Tuple<int, int> newDicReverse = new Tuple<int, int>(newDicValues.Item2, newDicValues.Item1);

            Assert.Equal(oldDicValues, newDicReverse);
        }
        [Fact]
        public void SetColumnPositionOverwrite()
        {
            Dictionary<Spreadsheet.ReferenceFields, int> originaldic = new(spreadsheet.PositionOfReferencesInSheet);

            Dictionary<Spreadsheet.ReferenceFields, int> newDic = new()
            {
                {Spreadsheet.ReferenceFields.Author, 22},
                {Spreadsheet.ReferenceFields.Title, 21},
                {Spreadsheet.ReferenceFields.PublicationType, 20},
                {Spreadsheet.ReferenceFields.Publisher, 19},
                {Spreadsheet.ReferenceFields.YearRef, 18},
                {Spreadsheet.ReferenceFields.RefId, 17},
                {Spreadsheet.ReferenceFields.Education, 16},
                {Spreadsheet.ReferenceFields.Location, 15},
                {Spreadsheet.ReferenceFields.Semester, 14},
                {Spreadsheet.ReferenceFields.Language, 13},
                {Spreadsheet.ReferenceFields.YearReport, 12},
                {Spreadsheet.ReferenceFields.OriginalRef, 11},
                {Spreadsheet.ReferenceFields.Match, 10},
                {Spreadsheet.ReferenceFields.Comment, 9},
                {Spreadsheet.ReferenceFields.Syllabus, 8},
                {Spreadsheet.ReferenceFields.Season, 7},
                {Spreadsheet.ReferenceFields.ExamEvent, 6},
                {Spreadsheet.ReferenceFields.Source, 5},
                {Spreadsheet.ReferenceFields.Pages, 4},
                {Spreadsheet.ReferenceFields.Volume, 3},
                {Spreadsheet.ReferenceFields.Chapters, 2},
                {Spreadsheet.ReferenceFields.BookTitle, 1},
            };
            spreadsheet.SetColumnPosition(newDic);

            Assert.Equivalent(newDic, spreadsheet.PositionOfReferencesInSheet, true);
        }
    }
}
