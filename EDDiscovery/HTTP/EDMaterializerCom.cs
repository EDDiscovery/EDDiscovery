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

        protected override void AddAuthHeaders(WebRequest request)
        {
            if (_authTokens == null)
            {
                _authTokens = TokensFromSignIn();
            }

            if (_authTokens == null)
            {
                // TODO: Could potentially cut this step by checking the expiry date
                // token. Though watch out for scenario where tokens are expired by other
                // events, like a logout. It can be handled by validating the return value
                // of the actual main request, but of course that's extra effort. :)
                _authTokens = ValidatedTokens();
            }

            if (_authTokens != null)
            {
                request.Headers.Add(_authTokens);
            }
        }

        private NameValueCollection ValidatedTokens()
        {
            var queryParams = $"uid={_authTokens["uid"]}&access-token={_authTokens["access_token"]}&client=email";
            var response = RequestGet($"{_authPath}/validate_token/?{queryParams}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return _authTokens;
            }
            else
            { 
                return null;
            }
        }

        private NameValueCollection TokensFromSignIn()
        {
            var appSettings = ConfigurationManager.AppSettings;
            var username = appSettings["EDMaterializerUsername"];
            var password = appSettings["EDMaterializerPassword"];
            var json = $"{{\"email\": \"{username}\", \"password\": \"{password}\"}}";
            var response = RequestPost(json, $"{_authPath}/sign_in",false);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return HttpUtility.ParseQueryString(response.Body);
            }
            else
            {
                return null;
            }
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
