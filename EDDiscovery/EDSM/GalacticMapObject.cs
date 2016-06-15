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
        public GalMapType Type;
        public string name;
        public string galMapSearch;
        public string galMapUrl;
        public string color;
        public List<PointData> points;
        public string description;
        public string descriptionhtml;

        public GalacticMapObject()
        {
            points = new List<PointData>();
        }

        public GalacticMapObject(JObject jo)
        {
            id = Tools.GetInt(jo["id"]);
            type = Tools.GetString(jo["type"]);
            name = Tools.GetString(jo["name"]);
            galMapSearch = Tools.GetString(jo["galMapSearch"]);
            galMapUrl = Tools.GetString(jo["galMapUrl"]);
            color = Tools.GetString(jo["color"]);
            description = Tools.GetString(jo["descriptionMardown"]);
            descriptionhtml = Tools.GetString(jo["descriptionHtml"]);

            points = new List<PointData>();

            try
            {

                if (type.Equals("regionQuadrants") || type.Equals("region") || type.Equals("travelRoute") || type.Equals("historicalRoute") || type.Equals("minorRoute"))
                {
                    JArray coords = (JArray)jo["coordinates"];

                    foreach (JArray ja in coords)
                    {
                        double x, y, z;
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

                    double x, y, z;
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

