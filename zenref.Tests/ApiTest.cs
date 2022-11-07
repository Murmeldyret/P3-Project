using Xunit;
using System;
using P3Project.API;
using P3Project;
using zenref.Models;
using Moq;
using zenref.Models;

namespace zenref.Tests;

// For testing public methods
public class ApiTest
{
    //Test IsApiKeyValid
    [Fact]
    public void ApiIsApiKeyValidThrowsNotImplemented()
    {
        Mock<Api> api = new Mock<Api>();

        Assert.Throws<NotImplementedException>(api.Object.IsApiKeyValid);
    }

    //End Test IsApiKeyValid
    [Fact]
    //Test ReferenceFetch
    public void ReferenceFetchUnsuccesful()
    {
        string url = "https://averywrong.url";

        Reference response_reference = null;

        Assert.Null(response_reference);
    }
    [Fact]
    public void ReferenceFetchSuccesful()
    {
        //
    }
    //End Test ReferenceFetch
    //Test fileformatResponse

    // End Test fileformatResponse
}

// For testing protected methods.
public class ApiTest1 : Api
{
    //Test _isHTTPResponseCodeSuccess
    [Theory]
    [InlineData(400)]
    [InlineData(401)]
    [InlineData(404)]
    public void isHTTPResponseCodeSuccessFailOnCode4xx(int value)
    {
        // Arrange
        System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage((System.Net.HttpStatusCode)value);

        // Act
        bool response_failure = _isHTTPResponseCodeSuccess(response);

        // Assert
        Assert.Equal(false, response_failure);
    }


    [Theory]
    [InlineData(201)]
    [InlineData(200)]
    public void httpResponseCodeSuccessOn2xx(int value)
    {
        // Arrange
        System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage((System.Net.HttpStatusCode)value);

        // Act
        bool response_success = _isHTTPResponseCodeSuccess(response);

        // Assert
        Assert.Equal(true, response_success);

    }
    //End Test _isHTTPResponseCodeSuccess

    // Start test CacheReferenceTest
    /// <summary>
    /// This test will test whether the program can cache a reference in the database if it doesn't already exist.
    /// </summary>
    //public void CacheReferenceTestSuccess()
    //{
    //    Reference testReference = new Reference();
    //    testReference.Author = "Michaelis, Mark";
    //    testReference.Title = "Essential C# 7.0";
    //    testReference.YearRef = 2018;
    //    testReference.Publisher = "Addison-Wesley Professional";

    //    CacheReference(testReference);

    //    //! Missing class for database and therefore methods too.
    //}
    // End test CacheReferenceTest

    // Start test CacheReferenceTest
    /// <summary>
    /// This test should fail because the cache already includes the reference that the program wants to add.
    /// </summary>
    //public void CacheReferenceTestDuplicate()
    //{
    //    Reference testReference = new Reference();
    //    testReference.Author = "Michaelis, Mark";
    //    testReference.Title = "Essential C# 7.0";
    //    testReference.YearRef = 2018;
    //    testReference.Publisher = "Addison-Wesley Professional";

    //    Reference testReferenceDuplicate = new Reference();
    //    testReferenceDuplicate.Author = "Michaelis, Mark";
    //    testReferenceDuplicate.Title = "Essential C# 7.0";
    //    testReferenceDuplicate.YearRef = 2018;
    //    testReferenceDuplicate.Publisher = "Addison-Wesley Professional";

    //    CacheReference(testReference);
    //    CacheReference(testReferenceDuplicate);

    //    //! Missing class for database and therefore methods too.
    //}

    //public void CacheReferenceTestInvalid()
    //{
    //    Reference? testReference = new Reference();
    //    testReference = null;

    //    try
    //    {
    //        CacheReference(testReference);    //* Don't worry. It is supposed to be null.
    //    }
    //    catch (Exception e)
    //    {
    //        Assert.IsType<NullReferenceException>(e);
    //    }
    //}


    public override System.Threading.Tasks.Task<Reference> ReferenceFetch(Reference inputReference, Func<System.Net.Http.HttpResponseMessage, Reference> referenceParser)
    {
        throw new NotImplementedException();
    }

}
