using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Staytus.Api.Extensions;
using Staytus.Api.Models;

namespace Staytus.Api
{
    public partial class StaytusApiClient : IDisposable
    {
        protected static readonly Object EMPTY_DATA = new Object();

        private readonly ILogger Logger;

        protected HttpClient m_HttpClient;

        private bool m_IsDisposed = false;

        public StaytusApiClient(String apiBaseUrl, String apiToken, String apiSecret, ILogger logger)
        {
            this.Logger = logger;

            if (apiBaseUrl.EndsWith("/"))
            {
                apiBaseUrl = apiBaseUrl.TrimEnd('/');
            }

            this.BaseApiUrl = apiBaseUrl;
            this.Token = apiToken;
            this.Secret = apiSecret;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_IsDisposed)
            {
                if (disposing)
                {
                    if (m_HttpClient != null)
                    {
                        m_HttpClient.Dispose();
                        m_HttpClient = null;
                    }

                    m_IsDisposed = true;
                }
            }
        }

        public HttpClient HttpClient
        {
            get
            {
                if (m_HttpClient == null)
                {
                    m_HttpClient = new HttpClient();
                }
                return m_HttpClient;
            }
            set { m_HttpClient = value; }
        }

        public String BaseApiUrl { get; private set; }
        public String Token { get; private set; }
        public String Secret { get; private set; }

        protected static String GetServiceMethodPath(String service, params String[] paths)
        {
            service = service.TrimEnd('/');
            if (!service.StartsWith("/"))
            {
                service = "/" + service;
            }

            if (paths == null)
            {
                return service;
            }

            var sb = new StringBuilder(service);
            foreach (String path in paths)
            {
                var tempPath = path.TrimEnd('/');
                if (String.IsNullOrEmpty(tempPath))
                {
                    continue;
                }

                if (!tempPath.StartsWith("/"))
                {
                    sb.Append("/");
                }
                sb.Append(tempPath);
            }

            return sb.ToString();
        }


        protected StaytusResponseModel<TResponseData> DefaultParseResponse<TResponseData>(String responseStr)
        {
            return ParseResponse<TResponseData>(responseStr, null, null);
        }

        // allow custom deserialization delegate functions to handle odd differences
        protected StaytusResponseModel<TResponseData> ParseResponse<TResponseData>(String responseStr,
            Func<String, JObject, StaytusResponseModel<TResponseData>> okParseResponse = null,
            Func<String, JObject, StaytusResponseModel<TResponseData>> errorParseResponse = null)
        {
            var responseJObject = JObject.Parse(responseStr);

            var statusToken = responseJObject["status"];
            if (statusToken == null ||
                statusToken.Type != JTokenType.String)
            {
                // no status prop, should mean this is an error
                // invalid json
                Logger.LogWarning("Invalid Api Response. Status property is null or not a string. Response: {0}", responseStr);

                // check if value prop is there for error message, pretty much we have an error response though
                return new StaytusResponseModel<TResponseData>(SystemMessages.ERROR,
                    GetErrorMessage(responseStr, responseJObject), default(TResponseData));
            }

            String statusStr = statusToken.ToObject<String>();
            if (String.Equals(statusStr, SystemMessages.SUCCESS, StringComparison.CurrentCultureIgnoreCase))
            {
                // we should be fine at this point, we have a status that definitely says OK, attempt deserialization
                if (okParseResponse != null)
                {
                    return okParseResponse(responseStr, responseJObject);
                }
                else
                {
                    // default deserialization
                    return JsonNetExtensions.DeserializeObject<StaytusResponseModel<TResponseData>>(responseStr);
                }
            }
            else
            {
                if (errorParseResponse != null)
                {
                    return errorParseResponse(responseStr, responseJObject);
                }
                else
                {
                    // default create response object as whatever status is and try to get error value
                    return new StaytusResponseModel<TResponseData>(statusStr,
                        GetErrorMessage(responseStr, responseJObject), default(TResponseData));
                    // TODO handle pulling out time/flags/headers when parsing as well?
                }
            }
        }

        protected String GetErrorMessage(String originalResponseStr, JObject responseJObject, String defaultValueStr = SystemMessages.PARSE_ERROR)
        {
            var dataToken = responseJObject["data"];
            if (dataToken == null ||
                dataToken.Type != JTokenType.Object)
            {
                Logger.LogWarning("Invalid Api Error Response. data field is null or not an object. Response: {0}", originalResponseStr);
                return defaultValueStr;
            }

            // guessing this is how moonrope always formats its errors?
            var messageToken = dataToken["message"];
            if (messageToken == null ||
                messageToken.Type != JTokenType.String)
            {
                Logger.LogWarning("Invalid Api Error Response. data.message field is null or not a string. Response: {0}", originalResponseStr);
                return defaultValueStr;
            }
            
            return messageToken.ToObject<String>();
        }

        protected async Task<StaytusResponseModel<TResponseData>> InternalSendAsync<TResponseData>(HttpRequestMessage httpRequest,
            Func<String, StaytusResponseModel<TResponseData>> deserializeResponse = null,
            CancellationToken cancelToken = default(CancellationToken))
        {
            if (deserializeResponse == null)
            {
                deserializeResponse = DefaultParseResponse<TResponseData>;
            }

            var requestUri = httpRequest.RequestUri;
            var requestVerb = httpRequest.Method;
            // backed up for printing purposes
            String httpContentStr = null;
            if (httpRequest.Content != null)
            {
                httpContentStr = await httpRequest.Content.ReadAsStringAsync();
            }

            httpRequest.Headers.Add("X-Auth-Token", this.Token);
            httpRequest.Headers.Add("X-Auth-Secret", this.Secret);

            String responseStr = null;
            using (var response = await this.HttpClient.SendAsync(httpRequest, cancelToken))
            {
                try
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        responseStr = await response.Content.ReadAsStringAsync();
                        Logger.LogError("RequestUri: {0} HttpVerb: {1} StatusCode: {2} ReasonPhrase: {3} HttpContent: {4} HttpResponse: {5}",
                            requestUri, requestVerb, response.StatusCode, response.ReasonPhrase, httpContentStr, responseStr);
                        return deserializeResponse(responseStr);
                    }

                    responseStr = await response.Content.ReadAsStringAsync();
                    Logger.LogDebug("RequestUri: {0} HttpVerb: {1} HttpContent: {2} HttpResponse: {3}",
                        requestUri, requestVerb, httpContentStr, responseStr);

                    return deserializeResponse(responseStr);
                }
                catch (Exception ex)
                {
                    Logger.LogError(0, ex, "Error parsing response");
                    return new StaytusResponseModel<TResponseData>(SystemMessages.ERROR, "Error handling response", default(TResponseData));
                }
            }
        }

        protected async Task<StaytusResponseModel<TResponseData>> InternalGetAsync<TResponseData>(String servicePath,
            IEnumerable<KeyValuePair<String, String>> parameters = null, Func<String, JObject, StaytusResponseModel<TResponseData>> okParseResponse = null,
            CancellationToken cancelToken = default(CancellationToken))
        {
            if (!servicePath.StartsWith("/"))
            {
                servicePath = "/" + servicePath;
            }

            var uriBuilder = new UriBuilder(BaseApiUrl + servicePath)
                {
                    Query = parameters.ToHttpString()
                };

            using (var httpReq = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri))
            {
                return await InternalSendAsync(httpReq,
                    (responseStr) => ParseResponse(responseStr, okParseResponse, null),
                    cancelToken);
            }
        }

        protected async Task<StaytusResponseModel<TResponseData>> InternalPostAsync<TContentData, TResponseData>(String servicePath,
            TContentData data = default(TContentData),
            Func<TContentData, String> serializeContentData = null,
            Func<String, JObject, StaytusResponseModel<TResponseData>> okParseResponse = null,
            CancellationToken cancelToken = default(CancellationToken))
        {
            if (!servicePath.StartsWith("/"))
            {
                servicePath = "/" + servicePath;
            }
            if (serializeContentData == null)
            {
                serializeContentData = (contentData) => JsonNetExtensions.SerializeObject(contentData);
            }

            using (var httpReq = new HttpRequestMessage(HttpMethod.Post, BaseApiUrl + servicePath))
            {
                httpReq.Content = new StringContent(serializeContentData(data), Encoding.UTF8, "application/json");
                // if the ;charset is appended to content-type I noticed certain calls just failing (??)
                httpReq.Content.Headers.ContentType.CharSet = null;
                return await InternalSendAsync(httpReq,
                    (responseStr) => ParseResponse(responseStr, okParseResponse, null),
                    cancelToken);
            }
        }

        protected async Task<StaytusResponseModel<TResponseData>> InternalPutAsync<TContentData, TResponseData>(String servicePath,
            TContentData data = default(TContentData),
            Func<TContentData, String> serializeContentData = null,
            Func<String, JObject, StaytusResponseModel<TResponseData>> okParseResponse = null,
            CancellationToken cancelToken = default(CancellationToken))
        {
            if (!servicePath.StartsWith("/"))
            {
                servicePath = "/" + servicePath;
            }
            if (serializeContentData == null)
            {
                serializeContentData = (contentData) => JsonNetExtensions.SerializeObject(contentData);
            }

            using (var httpReq = new HttpRequestMessage(HttpMethod.Put, BaseApiUrl + servicePath))
            {
                httpReq.Content = new StringContent(serializeContentData(data), Encoding.UTF8, "application/json");
                return await InternalSendAsync(httpReq,
                    (responseStr) => ParseResponse(responseStr, okParseResponse, null),
                    cancelToken);
            }
        }

        protected async Task<StaytusResponseModel<TResponseData>> InternalDeleteAsync<TResponseData>(String servicePath,
            IEnumerable<KeyValuePair<String, String>> parameters = null, Func<String, JObject, StaytusResponseModel<TResponseData>> okParseResponse = null,
            CancellationToken cancelToken = default(CancellationToken))
        {
            if (!servicePath.StartsWith("/"))
            {
                servicePath = "/" + servicePath;
            }

            var uriBuilder = new UriBuilder(BaseApiUrl + servicePath)
                {
                    Query = parameters.ToHttpString()
                };

            using (var httpReq = new HttpRequestMessage(HttpMethod.Delete, uriBuilder.Uri))
            {
                return await InternalSendAsync(httpReq,
                    (responseStr) => ParseResponse(responseStr, okParseResponse, null),
                    cancelToken);
            }
        }
    }
}
