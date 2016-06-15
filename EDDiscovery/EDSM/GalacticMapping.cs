using EDDiscovery2.EDDB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EDDiscovery.EDSM
{
    public class GalacticMapping
    {
        readonly string GalacticMappingFile = Path.Combine(Tools.GetAppDataDirectory(), "galacticmapping.json");

        public List<GalacticMapObject> galacticMapObjects;
        public Dictionary<string , GalMapType > dictGalMapTypes;

        public bool DownloadFromEDSM()
        {
            try
            {
                string url = "https://www.edsm.net/galactic-mapping/json-edd";
                bool newfile;

                EDDBClass.DownloadFile(url, GalacticMappingFile, out newfile);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("DownloadFromEDSM exception:" + ex.Message);
                return false;
            }
        }

        public bool ParseData()
        {
            galacticMapObjects = new List<GalacticMapObject>();
            dictGalMapTypes = GalMapType.GetDictionary;

            try
            {
                string json = EDDiscoveryForm.LoadJsonFile(GalacticMappingFile);

                if (json != null)
                {
                    JArray galobjects = (JArray)JArray.Parse(json);
                    foreach (JObject jo in galobjects)
                    {
                        GalacticMapObject galobject = new GalacticMapObject(jo);

                        if (dictGalMapTypes.ContainsKey(galobject.type))
                            galobject.Type = dictGalMapTypes[galobject.type];

                        galacticMapObjects.Add(galobject);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("GalacticMapping.parsedata exception:" + ex.Message);
            }

            return false;
        }
    }
}
