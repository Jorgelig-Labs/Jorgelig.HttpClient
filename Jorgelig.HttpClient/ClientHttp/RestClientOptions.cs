using System;
using System.Collections.Generic;
using System.Text;

namespace Jorgelig.HttpClient.ClientHttp
{
    public class RestClientOptions
    {
        public string? ApiBaseUrl { get; set; }
        public string? ApiSecretKey { get; set; }
        public string? Secret { get; set; }
        public string? DefaultHost { get; set; }
    }
}
