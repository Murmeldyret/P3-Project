using Xunit;
using P3Project.API;

using System;
using System.IO;

using Zenref.Ava.Models;

using Microsoft.Extensions.Configuration;
using P3Project.API.APIHelper;
using Moq;
using System.Net.Http;
using System.Net;

namespace zenref.Tests
{
    public class ScopusTests
    {
        [Fact]
        public async void ReferenceParserTest()
        {
            // Arrange
            HttpResponseMessage response = new HttpResponseMessage((HttpStatusCode)200);
            Reference inputReference = new Reference(_OriReference: "Zhao, Nannan. (2024). Improvement of Cloud Computing Medical Data Protection Technology Based on Symmetric Encryption Algorithm");

            string path = System.IO.Directory.GetCurrentDirectory();

            // Load test response json file
            string testResponse = File.ReadAllText("../../../testResponse.json");

            // Set response content
            response.Content = new StringContent(testResponse);

            // Act
            Scopus scopus = new Scopus("test", new Uri("http://test.com"));

            Reference reference = scopus.ReferenceParser(inputReference, response);

            // Assert
            Assert.Equal("Improvement of Cloud Computing Medical Data Protection Technology Based on Symmetric Encryption Algorithm", reference.Title);
        }
    }
}

