using EDDiscovery;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace EDDiscovery2.HTTP
{
    public class EDMaterizliaerCom : HttpCom
    {
        private NameValueCollection _authTokens = null;

        protected void AddAuthHeaders(WebRequest request)
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
            //RequestGet($"{authPath}/validate_token/$uid=");
            // http://ed-materializer.heroku.com/api/v1/auth/validate_token/?uid=jenny@example.com&access-token=TOKEN_HERE&client=CLIENT_HERE
            throw new NotImplementedException();
        }

        private NameValueCollection TokensFromSignIn()
        {
            throw new NotImplementedException();
        }

    }
}
