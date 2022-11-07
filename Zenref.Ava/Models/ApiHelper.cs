﻿using System;
using System.Net.Http.Headers;

namespace P3Project.API.APIHelper
{
	/// <summary>
	/// Represents an HTTP client that accepts JSON and XML content
	/// </summary>
	public static class ApiHelper
	{
		private static HttpClient _ApiClient {get; set;}
		/// <summary>
		/// The actual client making HTTP requests
		/// </summary>
		/// <remarks>
		/// Remember to use the method <c>InitializeClient</c> Before using this property
		/// </remarks>
		public static HttpClient ApiClient
		{
			get => _ApiClient ?? throw new MemberAccessException("ApiClient not initialized");
			private set {_ApiClient = value;}
		}

		public static void InitializeClient()
		{
			ApiClient = new HttpClient();
			ApiClient.DefaultRequestHeaders.Accept.Clear();
			ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

        }
    }
}
