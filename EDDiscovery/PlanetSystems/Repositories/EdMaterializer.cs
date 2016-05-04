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
#else
            // Production
            _serverAddress = "http://api.edmaterializer.com/";
#endif
        }

        public String ApiNamespace { get; } = "api/v3";
    }
}
