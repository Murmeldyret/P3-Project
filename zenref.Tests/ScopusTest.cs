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
        public void ReferenceParserTest()
        {
            // Arrange
            HttpResponseMessage response = new HttpResponseMessage((HttpStatusCode)200);
            RawReference inputReference = new RawReference("lige meget",
                "lige meget",
                "lige meget",
                "lige meget",
                "Zhao, Nannan. (2024). Improvement of Cloud Computing Medical Data Protection Technology Based on Symmetric Encryption Algorithm");

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

        [Fact]
        public async void ReferenceFetchTest()
        {
            // New reference
            RawReference inputReference = new RawReference("lige meget",
                "lige meget",
                "lige meget",
                "lige meget",
                "Zhao, Nannan. (2024). Improvement of Cloud Computing Medical Data Protection Technology Based on Symmetric Encryption Algorithm");
            // Get secret key
            var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Settings>()
            .Build();


            Scopus scopus = new Scopus(configuration["ScopusApiKey"], new Uri("https://api.elsevier.com/content/search/scopus"));


            // Act
            Reference reference = await scopus.ReferenceFetch(inputReference, scopus.ReferenceParser);

            // Assert
            Assert.NotNull(reference);
        }
    }

    internal class Settings
    {
        string apiKey { get; set; }
    }
}

