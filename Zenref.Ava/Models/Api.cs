using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using Zenref.Ava.Models;
using P3Project.API.APIHelper;

namespace P3Project.API
{
    /// <summary>
    /// Represents a RESTful WebApi
    /// </summary>
    public abstract class Api
    {
        public List<string> ParametersName { get; set; }
        public List<string> ParametersValue { get; set; }
        protected string _apiKey { get; init; }
        protected Uri _baseURL { get; init; }
        protected bool _isApiKeyValid { get; set; } = true;
        /// <summary>
        /// Represents the minimum number of milliseconds that has to pass before another call to this api can be made.
        /// </summary>
        public uint RateLimitInMsecs { get; init; }

        /// <summary>
        /// Empty constructor for testing purposes. Not meant for use in the program.
        /// </summary>
        protected Api()
        {
            _apiKey = "Not valid";
            _baseURL = new Uri("https://example.com");
        }
        /// <summary>
        /// Constructor <c>Api</c> that initialize with the apikey and the URI for calling the API.
        /// <example>
        /// For example:
        /// <code>
        /// Api("SomeApiKey", new Uri("https://example.com"));
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="apiKey">The key for getting access to the API</param>
        /// <param name="URI">The URI that the client should fetch data from.</param>
        public Api(string apiKey, Uri URI)
        {
            this._apiKey = apiKey;
            this._baseURL = URI;
        }

        /// <summary>
        /// Constructor <c>Api</c> with optional ratelimit parameter for limiting the amount of API calls that can be done.
        /// <example>
        /// For example:
        /// <code>
        /// Api("SomeApiKey", new Uri("https://example.com"), 100);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="apiKey">The key for getting access to the API</param>
        /// <param name="URI">The URI that the client should fetch data from.</param>
        /// <param name="RateLimitInMsecs">The rate limit entered in milliseconds, to prevent overloading the API. Must be 0 or greater.</param>
        public Api(string apiKey, Uri URI, uint RateLimitInMsecs)
        {
            this._apiKey = apiKey;
            this._baseURL = URI;
            this.RateLimitInMsecs = RateLimitInMsecs;
        }


        // A function that returns processed data, using delegates to parse data. The API returns a reference

        //public async Task<Reference> ReferenceFetch(string SearchQuery, Func<HttpResponseMessage, Reference> referenceParser)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// The method responsible for fetching data from a specific Api connection. Implementations should be async
        /// </summary>
        /// <param name="inputReference">The Reference that is to be looked up (usually unidentified)</param>
        /// <returns>A reference with correctly filled fields</returns>
        public virtual async Task<Reference> ReferenceFetch(Reference inputReference, Func<Reference, HttpResponseMessage, Reference> referenceParser)
        {
            Uri apiUri = BuildUri($"&query={inputReference.OriReference}");

            HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(apiUri);       // Request API for ressource.

            // Validation
            if (!_isHTTPResponseCodeSuccess(response))
            {
                throw new HttpRequestException("Was not able to get ressource from server.");
            }

            // Parse into deligate
            Reference parsed_reference = referenceParser(inputReference, response);

            return parsed_reference;

        }

        protected Uri BuildUri(string query)
        {
            UriBuilder uriBuilder = new UriBuilder(_baseURL);

            // Add all parameters to the query
            if (ParametersName != null && ParametersValue != null && ParametersName.Count == ParametersValue.Count)
            {
                uriBuilder.Query = ParametersName[0] + "=" + ParametersValue[0];
                for (int i = 1; i < ParametersName.Count; i++)
                {
                    uriBuilder.Query += $"&{ParametersName[i]}={ParametersValue[i]}";
                }
            }

            uriBuilder.Query += query;

            // Add API key to query
            uriBuilder.Query += $"&apikey={_apiKey}";

            return uriBuilder.Uri;
        }

        protected void CacheReference(Reference CacheableReference)
        {
            throw new NotImplementedException("No code that cache references");
        }


        // A function that handles status codes. Should be protected.
        protected bool _isHTTPResponseCodeSuccess(HttpResponseMessage message)
        {
            throw new NotImplementedException("Fuck dig ikke implementeret");
        }

        // A function that returns what fileformat the response is in.
        protected string fileformatResponse(HttpResponseMessage response)
        {
            throw new NotImplementedException();
        }



        // Key validation.
        public void IsApiKeyValid()
        {
            if (true)
            {
                //_isApiKeyValid = false;
                throw new NotImplementedException("Fuck dig ikke implementeret");
                //throw new ArgumentException($"{_baseURL}\nAPI key is not valid. Please update the key, or this site will not be available ");
            }
        }
    }
}