using RestSharp;
using System.Threading.Tasks;
using ApiTests.Utils;

namespace ApiTests.Core
{
    /// <summary>
    /// Central client for sending HTTP requests to Trello API using RestSharp.
    /// </summary>
    public class ApiClient
    {
        private readonly RestClient _client;

        /// <summary>
        /// Initializes a RestClient with base URL from config.
        /// </summary>
        public ApiClient()
        {
            var options = new RestClientOptions(ConfigHelper.BaseUrl);
            _client = new RestClient(options);
        }

        /// <summary>
        /// Sends a GET request to the given endpoint.
        /// </summary>
        public async Task<RestResponse> GetAsync(string endpoint)
        {
            var request = PrepareRequest(endpoint, Method.Get);
            return await _client.ExecuteAsync(request);
        }

        /// <summary>
        /// Sends a POST request with JSON body.
        /// </summary>
        public async Task<RestResponse> PostAsync(string endpoint, object body)
        {
            var request = PrepareRequest(endpoint, Method.Post);
            request.AddJsonBody(body);
            return await _client.ExecuteAsync(request);
        }

        /// <summary>
        /// Sends a PUT request with JSON body.
        /// </summary>
        public async Task<RestResponse> PutAsync(string endpoint, object body)
        {
            var request = PrepareRequest(endpoint, Method.Put);
            request.AddJsonBody(body);
            return await _client.ExecuteAsync(request);
        }

        /// <summary>
        /// Sends a DELETE request to the given endpoint.
        /// </summary>
        public async Task<RestResponse> DeleteAsync(string endpoint)
        {
            var request = PrepareRequest(endpoint, Method.Delete);
            return await _client.ExecuteAsync(request);
        }

        /// <summary>
        /// Prepares a request and appends authentication query params.
        /// </summary>
        private RestRequest PrepareRequest(string endpoint, Method method)
        {
            var request = new RestRequest(endpoint, method);
            request.AddQueryParameter("key", ConfigHelper.ApiKey);
            request.AddQueryParameter("token", ConfigHelper.Token);
            return request;
        }
    }
}