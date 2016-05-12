﻿using EDDiscovery;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using System.Configuration;

namespace EDDiscovery2.HTTP
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Web;
    using System.Windows.Forms;
    public class EDMaterizliaerCom : HttpCom
    {
        private NameValueCollection _authTokens = null;
        private readonly string _authPath = "auth";

        public EDMaterizliaerCom()
        {
            // JSON API Mimetype: http://jsonapi.org/
            MimeType = "application/vnd.api+json; charset=utf-8";
        }

        protected ResponseData RequestSecureGet(string action)
        {
            return ManagedRequest(null, action, RequestGetWrapper);
        }

        protected ResponseData RequestSecurePost(string json, string action)
        {
            return ManagedRequest(json, action, RequestPost);
        }

        protected ResponseData RequestSecurePatch(string json, string action)
        {
            return ManagedRequest(json, action, RequestPatch);
        }

        protected ResponseData RequestSecureDelete(string action)
        {
            return ManagedRequest(null, action, RequestDeleteWrapper);
        }


        private ResponseData SignIn()
        {
            _authTokens = null;
            var appSettings = ConfigurationManager.AppSettings;
#if DEBUG
            // Testing db username/password is public so other contributors
            // can work with it
            var username = "edmaterializer@gmail.com";
            var password = "Barnacles are delicious";
#else
            // This is for the production database, so we're keeping the
            // credentials hidden away
            var username = appSettings["EDMaterializerUsername"];
            var password = appSettings["EDMaterializerPassword"];
#endif
            ResponseData response = new ResponseData(HttpStatusCode.BadRequest);
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {
                MessageBox.Show("Unabled to login to the EdMaterializer server, the credentials file is missing from the installation",
                    "Unauthorized",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                var joSignIn = new JObject {
                    { "email", username },
                    { "password", password }
                };

                response = RequestPost(joSignIn.ToString(), $"{_authPath}/sign_in");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var headers = response.Headers;
                    var tokens = new NameValueCollection();
                    tokens["access-token"] = headers["access-token"];
                    tokens["client"] = headers["client"];
                    tokens["uid"] = headers["uid"]; ;
                    _authTokens = tokens;
                }
                else
                {
                    MessageBox.Show("Their was an error logging in to the EdMaterializer server.\nCheck the logs for details",
                        "Unauthorized",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            return response;
        }

        private ResponseData ManagedRequest(string json, 
                                            string action, 
                                            Func<string, string, NameValueCollection, ResponseData> requestMethod)
        {
            var commanderName = EDDiscoveryForm.EDDConfig.CurrentCommander.Name;
            JObject jo = JObject.Parse(json);
            jo["user"] = HttpUtility.UrlEncode(commanderName);
            json = jo.ToString();

            ResponseData response = new ResponseData(HttpStatusCode.BadRequest);
            // Attempt #1 with existing tokens
            if (_authTokens != null)
            {
                response = requestMethod(json, action, _authTokens);
                if (response.StatusCode == HttpStatusCode.Unauthorized ||
                    response.StatusCode == HttpStatusCode.BadRequest)
                {
                    _authTokens = null;
                }
                else
                {
                    return response;
                }
            }
            // Attempt #2 by logging in and obtaining fresh tokens
            if (_authTokens == null)
            {
                response = SignIn();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response = requestMethod(json, action, _authTokens);
                }
            }
            return response;
        }

        private ResponseData RequestGetWrapper(string json, string action, NameValueCollection headers)
        {
            return RequestGet(action, headers);
        }

        private ResponseData RequestDeleteWrapper(string json, string action, NameValueCollection headers)
        {
            return RequestDelete(action, headers);
        }


        private string AuthKeyToJson()
        {
            var json = new JavaScriptSerializer().Serialize(
                _authTokens.AllKeys.ToDictionary(k => k, k => _authTokens[k])
            );
            return json;
        }

    }
}
