using EDDiscovery;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using System.Configuration;

namespace EDDiscovery2.HTTP
{
    using System.Web;

    public class EDMaterizliaerCom : HttpCom
    {
        private NameValueCollection _authTokens = null;
        private readonly string _authPath = "api/v1/auth";

        //{
        //    if (_authTokens == null)
        //    {
        //        _authTokens = TokensFromSignIn();
        //    }

        //    if (_authTokens == null)
        //    {
        //        // TODO: Could potentially cut this step by checking the expiry date
        //        // token. Though watch out for scenario where tokens are expired by other
        //        // events, like a logout. It can be handled by validating the return value
        //        // of the actual main request, but of course that's extra effort. :)
        //        _authTokens = ValidatedTokens();
        //    }

        //    if (_authTokens != null)
        //    {
        //        request.Headers.Add(_authTokens);
        //    }
        //}
        //protected void AddAuthHeaders(WebRequest request)

        //private NameValueCollection ValidatedTokens()
        //{
        //    var queryParams = $"uid={_authTokens["uid"]}&access-token={_authTokens["access_token"]}&client=email";
        //    var response = RequestGet($"{_authPath}/validate_token/?{queryParams}");
        //    if (response.StatusCode == HttpStatusCode.OK)
        //    {
        //        return _authTokens;
        //    }
        //    else
        //    { 
        //        return null;
        //    }
        //}
        protected ResponseData RequestSecurePost(string json, string action)
        {
            ResponseData response = new ResponseData(HttpStatusCode.BadRequest);
            // Attempt #1 with existing tokens
            if (_authTokens != null)
            {
                response = RequestPost(json, action, _authTokens);
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
                    response = RequestPost(json, action, _authTokens);
                }
            }
            return response;
        }

        private ResponseData SignIn()
        {
            _authTokens = null;
            var appSettings = ConfigurationManager.AppSettings;
            var username = appSettings["EDMaterializerUsername"];
            var password = appSettings["EDMaterializerPassword"];
            var json = $"{{\"email\": \"{username}\", \"password\": \"{password}\"}}";
            var response = RequestPost(json, $"{_authPath}/sign_in");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var headers = response.Headers;
                var tokens = new NameValueCollection();
                tokens["access-token"] = headers["access-token"];
                tokens["client"] = headers["client"];
                tokens["provider"] = headers["email"];
                _authTokens = tokens;
            }
            return response;
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
