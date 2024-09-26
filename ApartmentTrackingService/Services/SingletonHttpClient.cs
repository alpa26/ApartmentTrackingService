using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentTrackingService.Services
{
    public class SingletonHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _httpClient;

        public SingletonHttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public HttpClient Instance => _httpClient ??= _httpClientFactory.CreateClient("Host");
    }
}
