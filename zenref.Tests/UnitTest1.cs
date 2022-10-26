using Xunit;
using System.IO;
using P3Project;


namespace zenref.Tests;

public class UnitTest1
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
}