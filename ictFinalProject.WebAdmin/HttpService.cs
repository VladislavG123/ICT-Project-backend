using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ictFinalProject.WebAdmin
{
    public class HttpService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private IConfiguration _configuration;

        public HttpService(
            HttpClient httpClient,
            NavigationManager navigationManager,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _configuration = configuration;
        }


        public async Task<HttpResponseMessage> Get(string uri, string token = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            return await SendRequest(request, token);
        }

        public async Task<HttpResponseMessage> Post(string uri, object value = null, string token = null)
        {
            string json = null;
            if (value != null)
            {
                json = JsonSerializer.Serialize(value);
            }

            var request = !(json is null)
                ? new HttpRequestMessage(HttpMethod.Post, uri)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                }
                : new HttpRequestMessage(HttpMethod.Post, uri);

            return await SendRequest(request, token);
        }

        public async Task<HttpResponseMessage> Put(string uri, object value = null, string token = null)
        {
            string json = null;
            if (value != null)
            {
                json = JsonSerializer.Serialize(value);
            }

            var request = !(json is null)
                ? new HttpRequestMessage(HttpMethod.Put, uri)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                }
                : new HttpRequestMessage(HttpMethod.Put, uri);

            return await SendRequest(request, token);
        }

        public async Task<HttpResponseMessage> Patch(string uri, object value, string token = null)
        {
            string json = null;
            if (value != null)
            {
                json = JsonSerializer.Serialize(value);
            }

            var request = !(json is null)
                ? new HttpRequestMessage(HttpMethod.Patch, uri)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                }
                : new HttpRequestMessage(HttpMethod.Patch, uri);

            return await SendRequest(request, token);
        }

        public async Task<HttpResponseMessage> Delete(string uri, string token = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, uri);

            return await SendRequest(request, token);
        }

        // helper methods
        private async Task<HttpResponseMessage> SendRequest(HttpRequestMessage request, string token = null)
        {
            if (token != null)
                request.Headers.Add("Authorization", "Bearer " + token);

            var response = await _httpClient.SendAsync(request);

            return response;
        }
    }

}