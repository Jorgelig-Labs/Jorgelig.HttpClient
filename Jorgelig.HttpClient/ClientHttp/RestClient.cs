using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Jorgelig.HttpClient.Utils;
using Newtonsoft.Json;
using System.Net.Http;

namespace Jorgelig.HttpClient.ClientHttp
{
    public class RestClient
    {
        private static System.Net.Http.HttpClient _client;
        private static string ApiBaseUrl;
        //private static ILogger _logger => Log.ForContext(typeof(RestClient));


        public RestClient(System.Net.Http.HttpClient client, string? apiBaseUrl)
        {
            if (string.IsNullOrWhiteSpace(apiBaseUrl))
                throw new ArgumentNullException($"{nameof(apiBaseUrl)} is required");

            ApiBaseUrl = apiBaseUrl;
            _client = client;
        }

        public async Task<TResult?> ExecuteApi<TResult>(HttpMethod method, string resourcePath,
            AuthenticationHeaderValue? authenticationHeader = null,
            object? data = null) where TResult : class
        {
            var url = GetUrl(resourcePath);
            var httpRequestMessage = new HttpRequestMessage(method, url);

            try
            {
                httpRequestMessage = AddDefaultHeaders(httpRequestMessage);
                httpRequestMessage = AddStringContent(httpRequestMessage, data);
                httpRequestMessage = AddAuthenticationHeader(httpRequestMessage, authenticationHeader);


                var httpResponse = await _client.SendAsync(httpRequestMessage);
                var resultContent = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    if (string.IsNullOrWhiteSpace(resultContent)) return new ErrorResult { IsSuccessStatusCode = false } as TResult;

                    var result =
                        JsonConvert.DeserializeObject<TResult?>(resultContent, JsonUtils.StringEnumJsonSerializerSettings);

                    return result;
                }

                var error = new ErrorResult
                {
                    IsSuccessStatusCode = false,
                    Content = resultContent
                };

                return error as TResult;
            }
            catch (Exception e)
            {
                //_log.Error(exception: e, $"[{method}] {resourcePath}");

                return new ErrorResult
                {
                    IsSuccessStatusCode = false,
                    Content = e.InnerException?.Message ?? e.Message
                } as TResult;
            }


            return default;
        }

        private string? GetUrl(string? resourcePath)
        {
            if (string.IsNullOrWhiteSpace(ApiBaseUrl))
                throw new ArgumentNullException(nameof(ApiBaseUrl));

            if (string.IsNullOrWhiteSpace(resourcePath))
                throw new ArgumentNullException(nameof(resourcePath));

            var baseUrl = ApiBaseUrl.EndsWith("/") ? ApiBaseUrl.TrimEnd(ApiBaseUrl[ApiBaseUrl.Length - 1]) : $"{ApiBaseUrl}";
            var path = resourcePath.StartsWith("/") ? resourcePath : $"/{resourcePath}";
            var url = $"{baseUrl}{path}";

            return url;
        }

        private HttpRequestMessage? AddStringContent(HttpRequestMessage? requestMessage, object? data)
        {
            if (requestMessage != null && data != null)
            {
                var json = JsonConvert.SerializeObject(data, JsonUtils.StringEnumJsonSerializerSettings);
                requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return requestMessage;
        }

        private HttpRequestMessage? AddAuthenticationHeader(HttpRequestMessage? requestMessage,
            AuthenticationHeaderValue? authenticationHeader)
        {
            if (authenticationHeader != null)
                requestMessage.Headers.Authorization = authenticationHeader;

            return requestMessage;

            return requestMessage;
        }

        private HttpRequestMessage? AddDefaultHeaders(HttpRequestMessage? requestMessage)
        {
            return requestMessage;
        }

    }

    
    public class ErrorResult
    {
        public bool? IsSuccessStatusCode { get; set; }
        public int? StatusCode { get; set; }
        public string? Error { get; set; }
        public string? Content { get; set; }
    }
}
