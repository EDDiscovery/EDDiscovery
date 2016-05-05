using EDDiscovery2.HTTP;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace EDDiscovery2.PlanetSystems.Repositories
{
    public class EdMaterializer : EDMaterizliaerCom
    {
        public EdMaterializer()
        {
#if DEBUG
            // Dev server. Mess with data as much as you like here
            _serverAddress = "https://ed-materializer.herokuapp.com/";

            // Want some to visuals on that thing you're working on?
            // Frontend app is at:
            // http://qa.edmaterializer.com

            // You'll need to register a commander account. Don't use the EDDiscovery credentials
#else
            // Production
            _serverAddress = "http://api.edmaterializer.com/";

            // Frontend app is at:
            // http://edmaterializer.com
#endif
        }

        public String ApiNamespace { get; } = "api/v4";
    }
}
