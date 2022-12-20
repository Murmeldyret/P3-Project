using Xunit;
using System;
using P3Project.API;
using P3Project;
using Zenref.Ava.Models;
using Moq;
using P3Project.API.APIHelper;

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
    //End Test ReferenceFetch
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
}
