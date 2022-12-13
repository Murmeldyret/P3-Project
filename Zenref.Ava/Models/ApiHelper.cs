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
        private static HttpClient? _ApiClient { get; set; }

        private ApiClient()
        {
            _ApiClient = new HttpClient();

            ApiClientConfiguration apiConfiguration = new ApiClientConfiguration();
            apiConfiguration.initializeClient(_ApiClient);
        }

        /// <summary>
        /// Returns the HTTP client instance and makes sure that it is initialized.
        /// </summary>
        /// <returns>The Initialized HTTP client instance.</returns>
        public static HttpClient getInstance()
        {
            if (_ApiClient == null)
            {
                new ApiClient();
            }

            return _ApiClient;
        }
    }

    partial class ApiClientConfiguration
    {
        /// <summary>
        /// Represents a partial method that can be implemented to extend the default initialization of the HTTP client.
        /// </summary>
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