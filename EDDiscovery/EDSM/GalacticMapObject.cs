using EDDiscovery2._3DMap;
using Newtonsoft.Json.Linq;
using OpenTK;
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
        public List<Vector3> points;
        public string description;
        public string descriptionhtml;

        public GalMapType galMapType;

        public GalacticMapObject()
        {
            points = new List<Vector3>();
        }

        public GalacticMapObject(JObject jo)
        {
            id = JSONHelper.GetInt(jo["id"]);
            type = JSONHelper.GetStringDef(jo["type"],"Not Set");
            name = JSONHelper.GetStringDef(jo["name"],"No name set");
            galMapSearch = JSONHelper.GetStringDef(jo["galMapSearch"],"");
            galMapUrl = JSONHelper.GetStringDef(jo["galMapUrl"],"");
            colour = JSONHelper.GetStringDef(jo["color"],"Orange");
            description = JSONHelper.GetStringDef(jo["descriptionMardown"],"No description");
            descriptionhtml = JSONHelper.GetStringDef(jo["descriptionHtml"],"");
            
            points = new List<Vector3>();

            try
            {
                JArray coords = (JArray)jo["coordinates"];

                if (coords.Count > 0)
                {
                    if (coords[0].Type == JTokenType.Array)
                    {
                        foreach (JArray ja in coords)
                        {
                            float x, y, z;
                            x = ja[0].Value<float>();
                            y = ja[1].Value<float>();
                            z = ja[2].Value<float>();
                            points.Add(new Vector3(x, y, z));
                        }
                    }
                    else
                    {
                        JArray plist = coords;

                        float x, y, z;
                        x = plist[0].Value<float>();
                        y = plist[1].Value<float>();
                        z = plist[2].Value<float>();
                        points.Add(new Vector3(x, y, z));
                    }
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

