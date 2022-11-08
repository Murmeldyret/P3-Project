using System.Collections.Generic;
using Xunit;
using zenref.Ava.Models;

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
            string testString = "hello";
            string testValue = "hello2";

            //Act
            int result = Reference.Fuzzy(testString, testValue);

            //Assert
            Assert.True(result == 1);
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
    }
}
