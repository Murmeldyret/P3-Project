using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace P3Project.API.APIHelper
{
    /// <summary>
    /// Represents a singleton where the HTTP client can be accessed.
    /// </summary>
    /// 
    public interface ISingleton
    {
        HttpClient getInstance();
    }

    /// <summary>
    /// Represents a singleton where the HTTP client can be accessed.
    /// Class <c>ApiClient</c> is a partial class that can be extended with additional configuration
    public partial class ApiClient : ISingleton
    {
        private HttpClient _ApiClient { get; set; }

        partial void CustomInit();
        private ApiClient()
        {
            _ApiClient = new HttpClient();
            initializeClient();
        }

        private void initializeClient()
        {
            _ApiClient.DefaultRequestHeaders.Accept.Clear();
            
            defaultInitialization();
            CustomInit();
        }

        private void defaultInitialization()
        {
            _ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            _ApiClient.Timeout = TimeSpan.FromMinutes(2);
        }

        public HttpClient getInstance()
        {
            if (_ApiClient == null)
            {
                initializeClient();
            }

            return _ApiClient;
        }
    }
}

