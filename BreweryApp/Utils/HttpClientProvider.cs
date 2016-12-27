using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace BreweryApp.Utils
{
    public static class HttpClientProvider
    {
        private static HttpClient _client;
        private static string _baseAddr = ConfigurationManager.AppSettings["BreweryDBBaseUrl"];
        private static string _apiKey = ConfigurationManager.AppSettings["BreweryDBKey"];
        private static NameValueCollection _parameters = new NameValueCollection();

        private static void _setDefautParameters()
        {
            if (_parameters.Count > 0)
            {
                _parameters.Clear();
            }
            _parameters.Add("key", _apiKey);
        }

        private static void _setDefaultHeaders()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            MediaTypeWithQualityHeaderValue jsonMediaType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(jsonMediaType);
        }

        private static void _initClient()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_baseAddr);

            _setDefautParameters();
            _setDefaultHeaders();
        }

        public static HttpClient Client
        {
            get
            {
                if(_client == null)
                {
                    _client = new HttpClient();
                }
                return _client;
            }
        }

        public static Uri BuildRequest(string[] segments, NameValueCollection extraParams = null)
        {
            _initClient();
            UriBuilder uriBuilder = new UriBuilder(_baseAddr);
            if (extraParams != null)
            {
                _parameters.Add(extraParams);
            }

            uriBuilder.Path += string.Join("/", segments);
            List<string> parametersString = new List<string>();
            foreach(string key in _parameters.Keys)
            {
                parametersString.Add( key + "=" + HttpUtility.UrlEncode(_parameters[key]) );
            }
            uriBuilder.Query = string.Join("&", parametersString);

            return uriBuilder.Uri;
        }
    }
}
