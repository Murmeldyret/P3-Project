using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;

using Zenref.Ava.Models;
using P3Project.API.APIHelper;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace P3Project.API
{
    /// <summary>
    /// Represents a RESTful WebApi
    /// </summary>
    public abstract class Api
    {
        public List<string> ParametersName { get; set; } // Not implemented
        public List<string> ParametersValue { get; set; } // Not implemented
        protected string _apiKey { get; init; } // Protected cuz nogle dele af programmet ikke skal bruge APIKey. Men dem som nedarever skal have den.
        protected Uri _baseURL { get; init; } // Start url for API, så Scopus
        protected bool _isApiKeyValid { get; set; } = true; // Never used..
       
        /// Represents the minimum number of milliseconds that has to pass before another call to this api can be made.
        public uint RateLimitInMsecs { get; init; } // not in msecs, scopus håndtere hvor mange requests der har været hen over en måned. UCN has 20k calls in a month, which isn't quite a lot.
        
        /// Empty constructor for testing purposes. Not meant for use in the program.
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

        /// <summary>
        /// The method responsible for fetching data from a specific Api connection. Implementations should be async
        /// </summary>
        /// <param name="inputReference">The Reference that is to be looked up (usually unidentified)</param>
        /// <returns>A reference with correctly filled fields</returns>
        public virtual async Task<(Reference, RawReference)> ReferenceFetch(RawReference inputReference, Func<RawReference, HttpResponseMessage, Reference> referenceParser) // Virtual if they add more APIs in the future
        {
            Uri apiUri = BuildUri(inputReference);

            // Make an HTTP GET request to the specified API URI
            // using the singleton instance of the ApiClient class
            HttpResponseMessage response = await ApiClient.getInstance().GetAsync(apiUri);       // Request API for ressource.

            // Validation
            if (!_isHTTPResponseCodeSuccess(response))
            {
                Debug.WriteLine(response.StatusCode);
                throw new HttpRequestException("Was not able to get ressource from server.");
            }

            // Validate whether the response is empty


            // Parse into deligate
            Reference parsed_reference = referenceParser(inputReference, response);

            return (parsed_reference, inputReference);

        }
        
        // Scopus har tegn som er reseveret, så de bliver nød til at blive erstattet med andre ting.
        private string queryCleaner(string? oriReference)
        {
            if (oriReference == null)
            {
                throw new ArgumentNullException("The original reference is null");
            }
            oriReference = oriReference.Replace("(", " ");
            oriReference = oriReference.Replace(")", " ");
            oriReference = oriReference.Replace("*", " ");
            oriReference = oriReference.Replace("&", " ");
            oriReference = oriReference.Replace("#", " ");
            oriReference = oriReference.Replace("+", "%2b");
            oriReference = oriReference.Replace("/", "%2f");



            return oriReference;
        }


        protected Uri BuildUri(RawReference inputReference)
        {
            UriBuilder uriBuilder = new UriBuilder(_baseURL);

            // Convert RawReference to a Reference
            Reference convertedReference = inputReference.ExtractData();

            string query = "query=" + queryCleaner(convertedReference.Author!) + " " + queryCleaner(convertedReference.Title);


            // Add all parameters to the query - Implemented, but never really used. Use the APIs parameters...
            if (ParametersName != null && ParametersValue != null && ParametersName.Count == ParametersValue.Count)
            {
                uriBuilder.Query = ParametersName[0] + "=" + ParametersValue[0];
                for (int i = 1; i < ParametersName.Count; i++)
                {
                    uriBuilder.Query += $"&{ParametersName[i]}={ParametersValue[i]}";
                }
                uriBuilder.Query += "&";

            }

            uriBuilder.Query += query;

            // Add API key to query
            uriBuilder.Query += $"&apikey={_apiKey}";

            return uriBuilder.Uri;
        }
        
        // 
        protected void CacheReference(Reference CacheableReference)
        {
            throw new NotImplementedException("No code that cache references");
        }


        // A function that handles status codes. Should be protected.
        protected bool _isHTTPResponseCodeSuccess(HttpResponseMessage message)
        {
            switch (message.StatusCode)
            {
                case HttpStatusCode.OK:
                    return true;
                case HttpStatusCode.Created:
                    return true;
                case HttpStatusCode.Accepted:
                    return true;
                case HttpStatusCode.BadRequest:
                    return false;
                case HttpStatusCode.Unauthorized:
                    return false;
                case HttpStatusCode.NotFound:
                    return false;
                default:
                    return false;
            }
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
                throw new NotImplementedException("This isn't implemented");
            }
        }
    }

    public class ApiSearching
    {
        public (List<Reference>, List<RawReference>) SearchReferences(List<RawReference> rawReferences)
        {
            // This should have been done in a better way, however, there is no time for it.
            // Initialize the Scopus object
            Scopus scopus = InitializeScopus();
            
            // Lists to store the search results and the raw references that could not be matched
            List<Reference> references = new List<Reference>();
            List<RawReference> leftOverReferences = new List<RawReference>();
            
            // Iterate over each raw reference
            foreach (RawReference rawReference in rawReferences)
            {
                
                // Search for the reference using the ReferenceFetch method on the Scopus object and the ReferenceParser property
                (Reference reference, RawReference OriReference) = Task.Run(() => scopus.ReferenceFetch(rawReference, scopus.ReferenceParser)).Result;
                
                // Check if the reference was found
                if (reference.Title != null)
                {
                    // If the reference was found, add it to the references list
                    references.Add(reference);
                }
                else
                {
                    // If the reference was not found, add the raw reference to the leftOverReferences list
                    leftOverReferences.Add(OriReference);
                }
            }
            
            // Return the tuple containing the references and leftOverReferences lists
            return (references, leftOverReferences);
        }

        private Scopus InitializeScopus()
        {
            // Read the apikey from the file
            string apiKey = File.ReadAllText("./ApiKeys/scopusApiKey.txt");
            
            // Initialize the api
            Scopus scopus = new Scopus(apiKey, new Uri("https://api.elsevier.com/content/search/scopus"));
            return scopus;
        }
    }
}
