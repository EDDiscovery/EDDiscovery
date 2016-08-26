using EDDiscovery2._3DMap;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EDSM
{
    public class GalacticMapObject
    {
        public int id;
        public string type;
        public string name;
        public string galMapSearch;
        public string galMapUrl;
        public string colour;
        public List<PointData> points;
        public string description;
        public string descriptionhtml;

        public GalMapType galMapType;

        public GalacticMapObject()
        {
            points = new List<PointData>();
        }

        public GalacticMapObject(JObject jo)
        {
            id = Tools.GetInt(jo["id"]);
            type = Tools.GetStringOrDefault(jo["type"],"Not Set");
            name = Tools.GetStringOrDefault(jo["name"],"No name set");
            galMapSearch = Tools.GetStringOrDefault(jo["galMapSearch"],"");
            galMapUrl = Tools.GetStringOrDefault(jo["galMapUrl"],"");
            colour = Tools.GetStringOrDefault(jo["color"],"Orange");
            description = Tools.GetStringOrDefault(jo["descriptionMardown"],"No description");
            descriptionhtml = Tools.GetStringOrDefault(jo["descriptionHtml"],"");
            
            points = new List<PointData>();

            try
            {

                if (type.Equals("regionQuadrants") || type.Equals("region") || type.Equals("travelRoute") || type.Equals("historicalRoute") || type.Equals("minorRoute"))
                {
                    JArray coords = (JArray)jo["coordinates"];

                    foreach (JArray ja in coords)
                    {
                        float x, y, z;
                        x = ja[0].Value<float>();
                        y = ja[1].Value<float>();
                        z = ja[2].Value<float>();
                        PointData point = new PointData(x, y, z);
                        points.Add(point);
                    }
                }
                else
                {
                    JArray plist = (JArray)jo["coordinates"];

                    float x, y, z;
                    x = plist[0].Value<float>();
                    y = plist[1].Value<float>();
                    z = plist[2].Value<float>();
                    PointData point = new PointData(x, y, z);
                    points.Add(point);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("GalacticMapObject parse coordinate error: type" + type + " " + ex.Message);
                points = null;
            }
        }

    }
}

