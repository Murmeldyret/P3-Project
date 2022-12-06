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

        public Reference ReferenceParser(RawReference inputReference, HttpResponseMessage response)
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

            Reference outputReference = new Reference(inputReference, DateTimeOffset.Now);
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
            

            return outputReference;
        }
    }
}