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
        private RawReference rawReference = new RawReference("software", "Aalborg", "Tredje", "12345", "Din far ");
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
            List<string> returnList = Reference.NGramizer(someString);

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

            Assert.True(reference.Equals(other));
        }
        [Fact]
        public void ValueEqualsReflexiveProperty()
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

            Assert.True(reference.Equals(reference));
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

            Assert.True(reference.Equals(other) && other.Equals(reference));
        }
        [Fact]
        public void ValueEqualsWhenOtherIsNullReturnsFalse()
        {
            Reference reference = new Reference(rawReference,
                "Anders Rask",
                "Titel på noget",
                "bog",
                "AAU",
                2022,
                "Dansk",
                2021,
                0.9,
                "Smart kommentar",
                "ingen idé",
                "Efterår",
                "en god eksamen",
                "pure opspind",
                21,
                "20th",
                "16-21",
                "Very good book");
            Reference? other = null;

            Assert.False(reference.Equals(other));
        }
        [Fact]
        public void ValueEqualsTransitive()
        {
            Reference reference = new Reference(rawReference, 
                    "Anders Rask",
                    "Titel på noget",
                    "bog",
                    "AAU",
                    2022,
                    "Dansk",
                    2021,
                    0.9,
                    "Smart kommentar",
                    "ingen idé",
                    "Efterår",
                    "en god eksamen",
                    "pure opspind",
                    21,
                    "20th",
                    "16-21",
                    "Very good book"
                ),
                second = new Reference(rawReference, 
                    "Anders Rask",
                    "Titel på noget",
                    "bog",
                    "AAU",
                    2022,
                    "Dansk",
                    2021,
                    0.9,
                    "Smart kommentar",
                    "ingen idé",
                    "Efterår",
                    "en god eksamen",
                    "pure opspind",
                    21,
                    "20th",
                    "16-21",
                    "Very good book"),
                third = new Reference(rawReference, 
                    "Anders Rask",
                    "Titel på noget",
                    "bog",
                    "AAU",
                    2022,
                    "Dansk",
                    2021,
                    0.9,
                    "Smart kommentar",
                    "ingen idé",
                    "Efterår",
                    "en god eksamen",
                    "pure opspind",
                    21,
                    "20th",
                    "16-21",
                    "Very good book");
            //De første to kan bruges hvis testen fejler.
            bool firstAndSecondEqual = reference.Equals(second);
            bool secondAndThirdEqual = second.Equals(third);
            bool firstAndThirdEqual = reference.Equals(third);


            Assert.True(firstAndThirdEqual);

        }
        [Fact]
        public void ValueEqualsFalseWhenNotEqual()
        {
           Reference reference = new Reference(rawReference,
                    "Anders Rask",
                    "Titel på noget",
                    "bog",
                    "AAU",
                    2022,
                    "Dansk",
                    2021,
                    0.9,
                    "Smart kommentar",
                    "ingen idé",
                    "Efterår",
                    "en god eksamen",
                    "pure opspind",
                    21,
                    "20th",
                    "16-21",
                    "Very good book"
                ),
                notTheSameReference = new Reference(rawReference,
                    "Anders Rask",
                    "Titel på noget",
                    "bog",
                    "AU",
                    2023,
                    "Dansk",
                    2019,
                    0.59,
                    "dum kommentar",
                    "ingen idé",
                    "Forår",
                    "en god eksamen",
                    "Noget videnskabeligt",
                    21,
                    "20th",
                    "16-21",
                    "Very good book"
                );
                

            Assert.False(reference.Equals(notTheSameReference));
        }
        // [Fact]
        // public void Regextest()
        // {
        //     //Arrange
        //     string stringWithDOI = "Fleet A, Che M, MacKay-Lyons M, et. al. Examining the Use of Constraint-Induced Movement Therapy in Canadian Neurological Occupational and Physical Therapy 2014;66(1): 60-71. doi: 10.3138/ptc.2012-61";
        //     RawReference raw = new RawReference(null, null, null, null, stringWithDOI);
        //     //Act
        //     Reference result = raw.ExtractData();
        //     
        //     //Assert
        //     Assert.Equal("10.3138/ptc.2012-61", result);
        // }

        [Fact]
        public void MatchingStrings()
        {
            //Arrange
            const string CORRECT_TEXT = "Examining py";
            string badText = "Examining ergfsg";
        
            //Act
            double result = rawReference.MatchingStrings(badText, CORRECT_TEXT);

            //Assert
            Assert.Equal(0.5, result);
        }

        [Fact]
        public void RegexWebsiteRef()
        {
            //Arrange
            string text = "Sundhedsstyrelsen.Danskernes sundhed - Den nationale sundhedsprofil 2017[Internet]. [Kbh.]: Sundhedsstyrelsen; 2018. 131 p. [cited 2019 Oct 22].Available from: https://www.sst.dk/da/udgivelser/2018/danskernes-sundhed-den-nationale-sundhedsprofil-2017.";
            RawReference raw = new RawReference(null, null, null, null, text);
            (string? PubType, string? Source) expected = new();
            expected.PubType = "Website";
            expected.Source = "www.sst.dk/da/udgivelser/2018/danskernes-sundhed-den-nationale-sundhedsprofil-2017";
            //Act
            (string?, string?) actual;
            actual = raw.UCNRefLinks();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RegexPoorRef()
        {
            //Arrange
            string text = "Rasmussen K, Kampmann J, Warming H. Interview med børn. 1. udgave. Kbh.: Hans Rit-zels Forlag A/S; 2017. 240 s.";
            RawReference raw = new RawReference(null, null, null, null, text);
            (string Author, string Title, int? YearRef) expected = new();
            expected.Author = "Rasmussen K, Kampmann J, Warming H.";
            expected.Title = "Interview med børn.";
            expected.YearRef = 2017;

            //Act
            (string, string, int?) actual = raw.UCNRefAuthorTitleYearRef();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RegexCorrectAPAStyleRef()
        {
            //Arrange
            string text = "Austring, B. D. & Sørensen, M. (2006). Æstetik og læring: Grundbog om æstetiske læreprocesser. København: Hans Reitzels Forlag.";
            RawReference raw = new RawReference(null, null, null, null, text);
            (string Author, int? YearRef, string Title, string Source) Expected = new ();
            Expected.Author = "Austring, B. D. & Sørensen, M.";
            Expected.YearRef = 2006;
            Expected.Title = "Æstetik og læring: Grundbog om æstetiske læreprocesser.";
            Expected.Source = "København: Hans Reitzels Forlag.";

            //Act
            (string, int?, string, string) actual = raw.CorrectAPACategorizer();

            //Assert
            Assert.Equal(Expected, actual);
        }

        [Fact]
        public void MethodInsideRegexUCNRef()
        {
            //Arrange
            string text = "Andersen, F. B. (2000). Tegn er noget vi bestemmer: Evalerung, hvalitet og udviklinger i omegnen af SMTTE-tænkningen. Aarhus: Danmarks Lærerhøjskole.";
            RawReference raw = new RawReference(null, null, null, null, text);
            
            (string Author, string Title, int? YearRef) expected = new();
            expected.Author = "Andersen, F. B.";
            expected.Title = "Tegn er noget vi bestemmer: Evalerung, hvalitet og udviklinger i omegnen af SMTTE-tænkningen.";
            expected.YearRef = 2000;

            //Act
            (string, string, int?) actual = raw.UCNRefAuthorTitleYearRef();

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
