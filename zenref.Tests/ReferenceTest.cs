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
    }
}
