using System;
using System.Net.Http.Headers;

namespace P3Project.API.APIHelper
{
	/// <summary>
	/// Represents an HTTP client that accepts JSON and XML content
	/// </summary>
	public class ApiHelper
	{
		/// <summary>
		/// The actual client making HTTP requests
		/// </summary>
		/// <remarks>
		/// Remember to use the method <c>InitializeClient</c> Before using this property
		/// </remarks>
		public static HttpClient ApiClient { get; private set; }

		public static void InitializeClient()
		{
			ApiClient = new HttpClient();
			ApiClient.DefaultRequestHeaders.Accept.Clear();
			ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

        }
    }
}

