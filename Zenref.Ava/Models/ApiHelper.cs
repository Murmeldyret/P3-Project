using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace P3Project.API.APIHelper
{
    /// <summary>
    /// Represents a singleton where the HTTP client can be accessed.
    /// Class <c>ApiClient</c> is a partial class that can be extended with additional configuration
    public class ApiClient
    {
        // Private property to hold an instance of HttpClient for making API calls.
        private static HttpClient? _ApiClient { get; set; }
        
        // Private constructor to prevent direct instantitation of the class.
        private ApiClient()
        {
            // Initialize the HttpClient
            _ApiClient = new HttpClient();
            
            // Create a new instance of ApiClientConfiguration
            ApiClientConfiguration apiConfiguration = new ApiClientConfiguration();
            
            // Configure the client using the ApiClientConfiguration.
            apiConfiguration.initializeClient(_ApiClient);
        }

        /// <summary>
        /// Returns the HTTP client instance and makes sure that it is initialized.
        /// </summary>
        /// <returns>The Initialized HTTP client instance.</returns>
        public static HttpClient getInstance()
        {
            // Check if the _ApiClient is null, if it is, create a new instance of ApiClient
            if (_ApiClient == null)
            {
                new ApiClient();
            }
            
            // Return the _ApiClient property
            return _ApiClient;
        }
    }

    partial class ApiClientConfiguration
    {
        /// Represents a partial method that can be implemented to extend the default initialization of the HTTP client.
        partial void CustomInit(HttpClient _ApiClient);

        public void initializeClient(HttpClient _ApiClient)
        {
            _ApiClient.DefaultRequestHeaders.Accept.Clear();

            defaultInitialization(_ApiClient); // Can be overwriten by partial method by clearing the headers.
            CustomInit(_ApiClient); // The default initialization can be extended by implementing a partial method.
        }


        /// <summary>
        /// Represents the default initialization of the HTTP client.
        /// </summary>
        private void defaultInitialization(HttpClient _ApiClient)
        {
            _ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            _ApiClient.Timeout = TimeSpan.FromMinutes(2);
        }
    }
}