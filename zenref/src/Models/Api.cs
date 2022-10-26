namespace P3Project.API
{
    /// <summary>
    /// Represents a RESTful webApi
    /// </summary>
    public abstract class Api
    {
        protected string _apiKey { get; init; }
        protected Uri _baseURL { get; init; }
        protected bool _isApiKeyValid { get; set; } = true;

        // A function that returns processed data, using delegates to parse data. The API returns a reference
        public Reference ReferenceFetch(string SearchQuery, Func<HttpResponseMessage, Reference> referenceParser)
        {
            throw new NotImplementedException();
            return new Reference();
        }


        
        // A function that uses fuzzy matching to determine the accuracy of the response.

        // A function that handles status codes. Should be private.
        protected bool _isHTTPResponseCodeSuccess()
        {
            throw new NotImplementedException("Fuck dig ikke implementeret");
        }

        // A function that returns what fileformat the response is in.

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

        // 

    }

    
}