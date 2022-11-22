using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using Xunit;
using Zenref.Ava.Models;

namespace zenref.Tests
{
    public class ReferenceTest
    {
        [Fact]
        public void Fuzzy_search_locally()
        {
            //Arrange
            string testString = "hello";
            string testValue = "hello2";

            //Act
            int result = Reference.Fuzzy(testString, testValue);

            //Assert
            Assert.True(result == 1);
        }

        [Fact]
        public void Fuzzy_search_online()
        {
            //Arrange
            string testString = "Examining py";
            string testValue =  "Examining ergfsg";

            //Act
            int result = Reference.Fuzzy(testString, testValue);

            //Assert
            Assert.True(result == 6);
        }

        [Fact]
        public void NGramiser_test()
        {
            //Arrange
            string someString = "This is how we party";

            List<string> resultList = new List<string>()
            {
                "This",
                "is",
                "how",
                "we",
                "party",
            };

            //Act
            List<string> returnList = Reference.NGramiser(someString);

            //Assert
            Assert.Equal(resultList, returnList);
        }
        // See https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/how-to-define-value-equality-for-a-type
        // for further info regarding value equality tests
        [Fact]
        public void ValueEqualityOnTwoEqualReferences()
        {
            Reference reference = new Reference(
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
                "Din far"),

                other = new Reference(new KeyValuePair<Reference._typeOfId, string>(Reference._typeOfId.Unknown, ""),
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

            Assert.True(reference.ValueEquals(other));
        }
        [Fact]
        public void ValueEqualsReflexsiveProperty()
        {
            Reference reference = new Reference(
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

            Assert.True(reference.ValueEquals(reference));
        }
        [Fact]
        public void ValueEqualsAlsoWorksInReverse()
        {
            Reference reference = new Reference(
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
                "Din far"),

                other = new Reference(
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

            Assert.True(reference.ValueEquals(other) && other.ValueEquals(reference));
        }
        [Fact]
        public void ValueEqualsWhenOtherIsNullReturnsFalse()
        {
            Reference reference = new Reference(
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
                "Din far"),
                other = null;

            Assert.False(reference.ValueEquals(other));
        }
        [Fact]
        public void ValueEqualsTransitive()
        {
            Reference reference = new Reference(
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
                "Din far"),
                second = new Reference(
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
                "Din far"),
                third = new Reference(
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
            //De første to kan bruges hvis testen fejler.
            bool firstAndSecondEqual = reference.ValueEquals(second);
            bool secondAndThirdEqual = second.ValueEquals(third);
            bool firstAndThirdEqual = reference.ValueEquals(third);


            Assert.True(firstAndThirdEqual);

        }
        [Fact]
        public void ValueEqualsFalseWhenNotEqual()
        {
            Reference reference = new Reference(
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
                "Din far"),
                notTheSameReference = new Reference(
                "Anders Rask",
                "Titel på noget andet",
                "bog",
                "AAU",
                2022,
                12345,
                "Software",
                "ålborg",
                "Tredje",
                "svensk",
                2021,
                0.9,
                "blank",
                "Det ved jeg ikke",
                "Efterår",
                "pas",
                "ved jeg heller ikke",
                420,
                "20th",
                "16-21",
                "Din mor");

            Assert.False(reference.ValueEquals(notTheSameReference));
        }
        [Fact]
        public void Regextest()
        {
            //Arrange
            string DOI = "Fleet A, Che M, MacKay-Lyons M, et. al. Examining the Use of Constraint-Induced Movement Therapy in Canadian Neurological Occupational and Physical Therapy 2014;66(1): 60-71. doi: 10.3138/ptc.2012-61";

            //Act
            string result = Reference.DOISearch(DOI);
            
            //Assert
            Assert.Equal("10.3138/ptc.2012-61", result);
        }

        [Fact]
        public void MatchingStrings()
        {
            //Arrange
            string CorrectText = "Examining py";
            int ShteinRes = 6;
        
            //Act
            double Result = Reference.MatchingStrings(ShteinRes, CorrectText);

            //Assert
            Assert.Equal(0.5, Result);
        }

        [Fact]
        public void RegexWebsiteRef()
        {
            //Arrange
            string text = "Sundhedsstyrelsen.Danskernes sundhed - Den nationale sundhedsprofil 2017[Internet]. [Kbh.]: Sundhedsstyrelsen; 2018. 131 p. [cited 2019 Oct 22].Available from: https://www.sst.dk/da/udgivelser/2018/danskernes-sundhed-den-nationale-sundhedsprofil-2017.";
            (string? PubType, string? Source) Expected = new();
            Expected.PubType = "Website";
            Expected.Source = "www.sst.dk/da/udgivelser/2018/danskernes-sundhed-den-nationale-sundhedsprofil-2017";
            //Act
            (string?, string?) Actual = Reference.UCNRefLinks(text);

            //Assert
            Assert.Equal(Expected, Actual);
        }

        [Fact]
        public void RegexPoorRef()
        {
            //Arrange
            string text = "Rasmussen K, Kampmann J, Warming H. Interview med børn. 1. udgave. Kbh.: Hans Rit-zels Forlag A/S; 2017. 240 s.";
            (string Author, string Title, int? YearRef) Expected = new();
            Expected.Author = "Rasmussen K, Kampmann J, Warming H.";
            Expected.Title = "Interview med børn.";
            Expected.YearRef = 2017;

            //Act
            (string, string, int?) Actual = Reference.UCNRefAuthorTitleYearRef(text);

            //Assert
            Assert.Equal(Expected, Actual);
        }

        [Fact]
        public void RegexCorrectAPAStyleRef()
        {
            //Arrange
            string text = "Austring, B. D. & Sørensen, M. (2006). Æstetik og læring: Grundbog om æstetiske læreprocesser. København: Hans Reitzels Forlag.";
            (string Author, int? YearRef, string Title, string Source) Expected = new ();
            Expected.Author = "Austring, B. D. & Sørensen, M.";
            Expected.YearRef = 2006;
            Expected.Title = "Æstetik og læring: Grundbog om æstetiske læreprocesser.";
            Expected.Source = "København: Hans Reitzels Forlag.";

            //Act
            (string, int?, string, string) Actual = Reference.CorrectAPACategorizer(text);

            //Assert
            Assert.Equal(Expected, Actual);
        }

        [Fact]
        public void MethodInsideRegexUCNRef()
        {
            //Arrange
            string text = "Andersen, F. B. (2000). Tegn er noget vi bestemmer: Evalerung, hvalitet og udviklinger i omegnen af SMTTE-tænkningen. Aarhus: Danmarks Lærerhøjskole.";
            (string Author, string Title, int? YearRef) Expected = new();
            Expected.Author = "Andersen, F. B.";
            Expected.Title = "Tegn er noget vi bestemmer: Evalerung, hvalitet og udviklinger i omegnen af SMTTE-tænkningen.";
            Expected.YearRef = 2000;

            //Act
            (string, string, int?) Actual = Reference.UCNRefAuthorTitleYearRef(text);

            //Assert
            Assert.Equal(Expected, Actual);
        }
    }
}
