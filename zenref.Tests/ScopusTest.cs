using Xunit;
using P3Project.API;

using System;

using Zenref.Ava.Models;

using Microsoft.Extensions.Configuration;
using P3Project.API.APIHelper;
using Moq;
using System.Net.Http;

namespace zenref.Tests
{
    public class ScopusTests
    {
        [Fact]
        public async void ReferenceParserTest()
        {
            // Arrange
            // Get API Key from user secrets
            Mock mock = new Mock<HttpResponseMessage>();    
        }
    }
}