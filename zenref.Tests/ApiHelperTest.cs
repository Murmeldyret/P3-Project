using System;
using Xunit;
using P3Project.API.APIHelper;
using System.Net.Http.Headers;

namespace zenref.Tests
{
	public class ApiHelperTest
	{
		//Test InitializeComponent
		[Fact]
		public static void InitializeComponentClientNotNull()
		{
			Assert.NotNull(ApiClient.getInstance());
		}
		//Test that client accepts content types
		[Fact]
		public static void ClientAcceptsContentTypeJSON()
		{
			bool apiHelperAcceptsJSON = ApiClient.getInstance().DefaultRequestHeaders.Accept.Contains(new MediaTypeWithQualityHeaderValue("application/json"));

			Assert.True(apiHelperAcceptsJSON);
		}
		[Fact]
		public static void ClientAcceptsContentTypeXML()
		{
            bool apiHelperAcceptsXML = ApiClient.getInstance().DefaultRequestHeaders.Accept.Contains(new MediaTypeWithQualityHeaderValue("application/xml"));

			Assert.True(apiHelperAcceptsXML);
        }
	}
}

