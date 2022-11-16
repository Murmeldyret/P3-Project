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
        public Scopus(string apiKey, Uri URI) : base(apiKey, URI)
        {
            this._apiKey = apiKey;
            this._baseURL = URI;
        }

        public Scopus(string apiKey, Uri URI, uint RateLimitInMsecs) : base(apiKey, URI, RateLimitInMsecs)
        {
            this._apiKey = apiKey;
            this._baseURL = URI;
            this.RateLimitInMsecs = RateLimitInMsecs;
        }

/*
        public override async Task<Reference> ReferenceFetch(Reference inputReference, Func<Reference, HttpResponseMessage, Reference> referenceParser)
        {
            Uri apiUri = BuildUri($"&query={inputReference.OriReference}");

            HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(apiUri);

            // Validation
            if (!_isHTTPResponseCodeSuccess(response))
            {
                throw new HttpRequestException("Was not able to get ressource from server.");
            }

            // Parse into deligate
            Reference parsed_reference = referenceParser(inputReference, response);

            return parsed_reference;
        }*/
        public Reference ReferenceParser(Reference inputReference, HttpResponseMessage response)
        {
            string responseContent = response.Content.ReadAsStringAsync().Result;

            // Parse json in object
            ScopusResponse scopusResponse = ScopusResponse.FromJson(responseContent);


            // Split inputReference into substrings
            string[] inputReferenceSplit = inputReference.OriReference.Split(". ");


            // Fuzzy match the substrings with the response
            int bestMatchIndex = -1;
            int bestMatchScore = int.MaxValue;
            double bestMatchScorePercentage = 0;
            for (int i = 0; i < scopusResponse.SearchResults.Entry.Count; i++)
            {
                for (int j = 0; j < inputReferenceSplit.Length; j++)
                {
                    int distance = Fastenshtein.Levenshtein.Distance(scopusResponse.SearchResults.Entry[i].DcTitle.ToLower(), inputReferenceSplit[j].ToLower());

                    if (distance < bestMatchScore)
                    {
                        bestMatchScore = distance;
                        bestMatchIndex = i;
                    }
                }
            }

            if (bestMatchIndex == -1)
            {
                throw new Exception("Could not find a match");
            }
            else
            {
                bestMatchScorePercentage = 1 - ((double)bestMatchScore / (double)scopusResponse.SearchResults.Entry[bestMatchIndex].DcTitle.Length);
            }



            // Set the best match as the reference
                inputReference.Author = scopusResponse.SearchResults.Entry[bestMatchIndex].DcCreator;
                inputReference.Title = scopusResponse.SearchResults.Entry[bestMatchIndex].DcTitle;
                inputReference.PubType = scopusResponse.SearchResults.Entry[bestMatchIndex].PrismAggregationType;
                inputReference.Publisher = scopusResponse.SearchResults.Entry[bestMatchIndex].Affiliation[0].Affilname;
                inputReference.YearRef = Int32.Parse(scopusResponse.SearchResults.Entry[bestMatchIndex].PrismCoverDisplayDate.Split(" ")[2]);
                inputReference.Location = scopusResponse.SearchResults.Entry[bestMatchIndex].Link[2].Href.ToString();
                inputReference.Match = bestMatchScorePercentage;
                inputReference.Volume = scopusResponse.SearchResults.Entry[bestMatchIndex].PrismVolume.ToString();
                inputReference.BookTitle = scopusResponse.SearchResults.Entry[bestMatchIndex].PrismPublicationName;
            

            return inputReference;
        }
    }
}