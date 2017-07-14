/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Net;

namespace BaseUtils
{
    public struct ResponseData
    {
        public ResponseData(HttpStatusCode statusCode, string content = null, NameValueCollection headers = null, bool? error = null)
        {
            StatusCode = statusCode;
            Body = content;
            Headers = headers;
            Error = error ?? ((int)statusCode >= 400);
        }

        public HttpStatusCode StatusCode; // Sometimes you need the status code if you're in a
                                          // converstation with the server
        public string Body;
        public NameValueCollection Headers;
        public bool Error;

        // Uses knowledge of the error object structure in JSON API to
        // output all the errors for human consumption. 
        // Reference http://jsonapi.org/format/#error-objects
        public string JsonApiErrorMessage()
        {
            string message = "";
            try
            {
                if (Body.Length > 0)
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
                else
                {
                    message = $"Server Error\nHTTP response: {StatusCode.ToString()}\n\nCheck the logs for more details";
                }
            }
            catch(Exception e)
            {
                message = $"Unknown JSON API error\n\nDetails: {e}\n\nCheck the logs for more details";
            }
            return message;
        }
    }
}
