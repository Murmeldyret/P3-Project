using System;
using System.Net.Http;
using System.Threading.Tasks;
using P3Project.API.APIHelper;
using Zenref.Ava.Models;
using Newtonsoft.Json;

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

        public override async Task<Reference> ReferenceFetch(Reference inputReference, Func<HttpResponseMessage, Reference> referenceParser)
        {
            Uri apiUri = BuildUri($"&query={inputReference.OriReference}");

            HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(apiUri);

            // Validation
            if (!_isHTTPResponseCodeSuccess(response))
            {
                throw new HttpRequestException("Was not able to get ressource from server.");
            }

            // Parse into deligate
            Reference parsed_reference = referenceParser(response);

            return parsed_reference;
        }
        public Reference ReferenceParser(HttpResponseMessage response)
        {
            return new Reference();
        }
    }
}