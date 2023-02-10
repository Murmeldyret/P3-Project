using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using P3Project.API.APIHelper;
using Zenref.Ava.Models;


namespace P3Project.API
{
    public class Scopus : Api
    {
        
        /// <summary>
        /// Initializes a new instance of the Scopus class with the given API key and base URL
        /// </summary>
        /// <param name="apiKey">The API key for Scopus</param>
        /// <param name="URI">The base URL for the Scopus API</param>
        public Scopus(string apiKey, Uri URI) : base(apiKey, URI)
        {
            this._apiKey = apiKey;
            this._baseURL = URI;
        }

        /// <summary>
        /// Initializes a new instance of the Scopus class with the given API key, base URL, and rate limit
        /// </summary>
        /// <param name="apiKey">The API key for Scopus</param>
        /// <param name="URI">The base URL for the Scopus API</param>
        /// <param name="RateLimitInMsecs">The rate limit for the Scopus API in milliseconds</param>
        public Scopus(string apiKey, Uri URI, uint RateLimitInMsecs) : base(apiKey, URI, RateLimitInMsecs)
        {
            this._apiKey = apiKey;
            this._baseURL = URI;
            this.RateLimitInMsecs = RateLimitInMsecs;
        }

        /// <summary>
        /// Parses the raw reference and returns the best matched reference
        /// </summary>
        /// <param name="inputReference">The raw reference that needs to be parsed</param>
        /// <param name="response">The response from the Scopus API</param>
        /// <returns>The best matched reference</returns>
        public Reference ReferenceParser(RawReference inputReference, HttpResponseMessage response)
        {
            // Read the response content as a string and assign it to responseContent variable
            string responseContent;
            try
            {
                responseContent = Task.Run(() => response.Content.ReadAsStringAsync()).Result;
            }
            catch (System.Exception)
            {
                // If the response cannot be read, return the raw reference as a reference
                return new Reference(inputReference, DateTimeOffset.Now);
            }

            // Parse json in object
            ScopusResponse scopusResponse;
            try
            {
                // If the response cannot be parsed, return the raw reference as a reference
                scopusResponse = ScopusResponse.FromJson(responseContent);
            }
            catch (System.Exception)
            {
                // If the response cannot be parsed, return the raw reference as a reference
                return new Reference(inputReference, DateTimeOffset.Now);
            }

            // If the response is empty, return the raw reference as a reference
            if (scopusResponse.SearchResults.OpensearchTotalResults == 0)
            {
                return new Reference(inputReference, DateTimeOffset.Now);
            }
            
            // Split inputReference into substrings
            string[] inputReferenceSplit = inputReference.OriReference.Split(". ");
            
            // Fuzzy match the substrings with the response
            // : Initialize the bestMatchIndex with -1 as no match has been found yet
            int bestMatchIndex = -1;
            
            // : Initialize the bestMatchScore with the maximum possible value of int
            int bestMatchScore = int.MaxValue;
            
            // : Initialize the bestMatchScorePercentage with 0 as no match has been found yet
            double bestMatchScorePercentage = 0;
            
            // : Loop through each entry in scopusResponse.SearchResults.Entry
            for (int i = 0; i < scopusResponse.SearchResults.Entry.Count; i++)
            {
                // : Loop through each word in inputReferenceSplit
                for (int j = 0; j < inputReferenceSplit.Length; j++)
                {
                    // Calculate the Levenshtein distance between the title in the response and the input reference
                    int distance = Fastenshtein.Levenshtein.Distance(scopusResponse.SearchResults.Entry[i].DcTitle.ToLower(), inputReferenceSplit[j].ToLower());
                    
                    // : If the calculated distance is smaller than the current best match score
                    if (distance < bestMatchScore)
                    {
                        // : Update the best match score with the new distance
                        bestMatchScore = distance;
                        
                        // : Update the best match index with the current index
                        bestMatchIndex = i;
                    }
                }
            }

            // : Check if a match was found by checking the value of bestMatchIndex
            if (bestMatchIndex == -1)
            {
                // : If no match was found, throw an exception
                throw new Exception("Could not find a match");
            }
            else
            {
                // : If a match was found, calculate the match score percentage
                bestMatchScorePercentage = 1 - ((double)bestMatchScore / (double)scopusResponse.SearchResults.Entry[bestMatchIndex].DcTitle.Length);
            }

            // : Create a new Reference object with the input reference and the current time
            Reference outputReference = new Reference(inputReference, DateTimeOffset.Now);

            try
            {
                // Set the best match as the reference
                outputReference.Author = scopusResponse.SearchResults.Entry[bestMatchIndex].DcCreator;
                outputReference.Title = scopusResponse.SearchResults.Entry[bestMatchIndex].DcTitle;
                outputReference.PubType = scopusResponse.SearchResults.Entry[bestMatchIndex].PrismAggregationType;
                outputReference.Publisher = scopusResponse.SearchResults.Entry[bestMatchIndex].Affiliation[0].Affilname;
                outputReference.YearRef = Int32.Parse(scopusResponse.SearchResults.Entry[bestMatchIndex].PrismCoverDisplayDate.Split(" ")[2]);
                outputReference.Source = scopusResponse.SearchResults.Entry[bestMatchIndex].Link[2].Href.ToString();
                outputReference.Match = bestMatchScorePercentage;
                outputReference.Volume = scopusResponse.SearchResults.Entry[bestMatchIndex].PrismVolume.ToString();
                outputReference.BookTitle = scopusResponse.SearchResults.Entry[bestMatchIndex].PrismPublicationName;
                if (scopusResponse.SearchResults.Entry[bestMatchIndex].PrismIssn is not null)
                {
                    outputReference.Commentary = scopusResponse.SearchResults.Entry[bestMatchIndex].PrismIssn.ToString();
                }
                else if (scopusResponse.SearchResults.Entry[bestMatchIndex].PrismDoi is not null)
                {
                    outputReference.Commentary = scopusResponse.SearchResults.Entry[bestMatchIndex].PrismDoi.ToString();
                }
            }
            catch (System.Exception)
            {
                return new Reference(inputReference, DateTimeOffset.Now);
            }


            return outputReference;
        }
    }
}
