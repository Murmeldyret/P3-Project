using Xunit;
using System;
using P3Project.API;
using Moq;
namespace zenref.Tests;


public class ApiTest
{
    [Fact]
    public void ApiIsApiKeyValidThrowsNotImplemented()
    {
        Mock<Api> api = new Mock<Api>();

        Assert.Throws<NotImplementedException>(api.Object.IsApiKeyValid);
    }
}