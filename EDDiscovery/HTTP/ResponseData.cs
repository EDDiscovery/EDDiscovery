using System.Collections.Specialized;
using System.Net;

namespace EDDiscovery2.HTTP
{
    public struct ResponseData
    {
        public ResponseData(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            Body = null;
            Headers = null;
        }

        public ResponseData(HttpStatusCode statusCode, string content, NameValueCollection headers)
        {
            StatusCode = statusCode;
            Body = content;
            Headers = headers;
        }

        public HttpStatusCode StatusCode; // Sometimes you need the status code if you're in a
                                          // converstation with the server
        public string Body;
        public NameValueCollection Headers;
    }

}
