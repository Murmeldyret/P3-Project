using P3Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

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
            int result = Reference.FuzzyLocal(testString, testValue);

            //Assert
            Assert.True(result > 75);
        }

        [Fact]
        public void Fuzzy_search_online()
        {
            //Arrange
            string testString = "hello";
            string testValue = "hello2";

            //Act
            int result = Reference.FuzzyOnline(testString, testValue);

            //Assert
            Assert.True(result > 75);
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
