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
using System.Collections.Generic;

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

            //RawReference inputReference = new RawReference("lige meget",
            //    "lige meget",
            //    "lige meget",
            //    "lige meget",
            //    "Andersen, B. B. & Porse, L. H. (2021). TAP. Teori til Analyse og Praksis i billedkunst. Praxis.");
            // Get secret key
            var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Settings>()
            .Build();


            Scopus scopus = new Scopus(configuration["ScopusApiKey"], new Uri("https://api.elsevier.com/content/search/scopus"));


            // Act
            (Reference reference, RawReference rawReference) = await scopus.ReferenceFetch(inputReference, scopus.ReferenceParser);

            // Assert
            Assert.NotNull(reference.Title);
        }

        [Fact]
        public void ReferenceFetchWithApiSearch()
        {
            // New reference
            RawReference inputReference = new RawReference("lige meget",
                "lige meget",
                "lige meget",
                "lige meget",
                "Andersen, B. B. & Porse, L. H. (2021). TAP. Teori til Analyse og Praksis i billedkunst. Praxis.");

            // Get secret key
            var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Settings>()
            .Build();

            List<RawReference> references = new List<RawReference>();
            references.Add(inputReference);

            ApiSearching apiSearching = new ApiSearching();
            (List<Reference> listReferences, List<RawReference> listRawReferences) = apiSearching.SearchReferences(references);

            Assert.Null(listReferences[0].Title);

        }
    }

    internal class Settings
    {
        string apiKey { get; set; }
    }
}

