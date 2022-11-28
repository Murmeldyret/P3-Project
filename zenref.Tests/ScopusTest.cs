using Xunit;
using System;
using System.IO;
using System.Net;
using Zenref.Ava.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using zenref.Core.API;

namespace zenref.Tests
{
    public class ScopusTests
    {
        [Fact]
        public void ReferenceParserTest()
        {
            // Arrange
            HttpResponseMessage response = new HttpResponseMessage((HttpStatusCode)200);
            Reference inputReference = new Reference(_OriReference: "Zhao, Nannan. (2024). Improvement of Cloud Computing Medical Data Protection Technology Based on Symmetric Encryption Algorithm");

            // Load test response json file TODO: Move the tesresponse.json to the debug folder and load it from there
            string currentDirectory = Directory.GetCurrentDirectory();
            string testResponse = File.ReadAllText(currentDirectory + "\\testResponse.json");
            
            // string testResponse = File.ReadAllText("../../../testResponse.json");

            // Set response content
            response.Content = new StringContent(testResponse);

            // Act
            Scopus scopus = new Scopus("test", new Uri("http://test.com"));

            Reference reference = scopus.ReferenceParser(inputReference, response);

            // Assert
            Assert.Equal("Improvement of Cloud Computing Medical Data Protection Technology Based on Symmetric Encryption Algorithm", reference.Title);
        }

        [Fact]
        public async void ReferenceFetchTest()
        {
            // New reference
            Reference inputReference = new Reference(_OriReference: "Zhao, Nannan. (2024). Improvement of Cloud Computing Medical Data Protection Technology Based on Symmetric Encryption Algorithm");
            
            // Get secret key
            var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Settings>()
            .Build();
            
            Scopus scopus = new Scopus(configuration["ScopusApiKey"], new Uri("https://api.elsevier.com/content/search/scopus"));

            // Initialize client
            ApiHelper.InitializeClient();

            // Act
            Reference reference = await scopus.ReferenceFetch(inputReference, scopus.ReferenceParser);

            // Assert
            Assert.NotNull(reference);
        }
    }

    internal abstract class Settings
    {
        string apiKey { get; set; }
    }
}

