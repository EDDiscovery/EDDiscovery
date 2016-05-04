using Newtonsoft.Json.Linq;
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

        // Uses knowledge of the error object structure in JSON API to
        // output all the errors for human consumption. 
        // Reference http://jsonapi.org/format/#error-objects
        public string JsonApiErrorMessage()
        {
            string message = "";
            try
            {
                JObject jbody = JObject.Parse(Body);
                JArray jerrors = (JArray)jbody["errors"];
                if (jerrors != null)
                {
                    foreach (JObject jerror in jerrors)
                    {
                        if (message.Length > 0)
                        {
                            message += "\n";
                        }
                        message += $"{jerror["title"]} - {jerror["detail"]}";
                    }
                }
            }
            catch
            {
                message = "Unknown JSON API error";
            }
            return message;
        }
    }

}
