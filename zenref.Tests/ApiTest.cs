using Xunit;
using System;
using P3Project.API;
using Moq;
namespace zenref.Tests;


public class ApiTest : Api
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
        //et eller andet
    }
    [Fact]
    public void ReferenceFetchSuccesful()
    {
        //
    }
    //End Test ReferenceFetch
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
    //Test fileformatResponse

    // End Test fileformatResponse
}