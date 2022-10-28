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
			ApiHelper.InitializeClient();
			Assert.NotNull(ApiHelper.ApiClient);
		}
		//Test that client accepts content types
		[Fact]
		public static void ClientAcceptsContentTypeJSON()
		{
			ApiHelper.InitializeClient();

			bool apiHelperAcceptsJSON = ApiHelper.ApiClient.DefaultRequestHeaders.Accept.Contains(new MediaTypeWithQualityHeaderValue("application/json"));

			Assert.True(apiHelperAcceptsJSON);
		}
		[Fact]
		public static void ClientAcceptsContentTypeXML()
		{
            ApiHelper.InitializeClient();

            bool apiHelperAcceptsXML = ApiHelper.ApiClient.DefaultRequestHeaders.Accept.Contains(new MediaTypeWithQualityHeaderValue("application/xml"));

			Assert.True(apiHelperAcceptsXML);
        }
		//TODO Test if class throws when ApiClient is null (Note that the property is static)
	}
}

