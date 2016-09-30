using EDDiscovery.DB;
using EDDiscovery2.EDSM;
using EDDiscovery2.HTTP;
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

        public List<GalacticMapObject> galacticMapObjects = null;
        public List<GalMapType> galacticMapTypes = null;

        public GalacticMapping()
        {
            galacticMapTypes = GalMapType.GetTypes();          // we always have the types.

            int sel = SQLiteDBClass.GetSettingInt("GalObjectsEnable", int.MaxValue);
            foreach (GalMapType tp in galacticMapTypes)
            {
                tp.Enabled = (sel & 1) != 0;
                sel >>= 1;
            }
        }

        public bool DownloadFromEDSM()
        {
            try
            {
                EDSMClass edsm = new EDSMClass();
                string url = EDSMClass.ServerAddress + "galactic-mapping/json-edd";
                bool newfile;

                return DownloadFileHandler.DownloadFile(url, GalacticMappingFile, out newfile);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("DownloadFromEDSM exception:" + ex.Message);
            }

            return false;
        }

        public bool ParseData()
        {
            galacticMapObjects = new List<GalacticMapObject>();

            try
            {
                string json = EDDiscoveryForm.LoadJsonFile(GalacticMappingFile);

                if (json != null)
                {
                    JArray galobjects = (JArray)JArray.Parse(json);
                    foreach (JObject jo in galobjects)
                    {
                        GalacticMapObject galobject = new GalacticMapObject(jo);

                        GalMapType ty = galacticMapTypes.Find(x => x.Typeid.Equals(galobject.type));

                        if (ty == null)
                        {
                            ty = galacticMapTypes[galacticMapTypes.Count - 1];      // last one is default..
                            Console.WriteLine("Unknown Gal Map object " + galobject.type);
                        }

                        galobject.galMapType = ty;
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

        public void ToggleEnable(GalMapType tpsel = null)
        {
            GalMapType tpon = galacticMapTypes.Find(x => x.Enabled == true);  // find if any are on

            foreach (GalMapType tp in galacticMapTypes)
            {
                if (tpsel == null)                              // if toggle all..
                    tp.Enabled = (tpon == null);                // enabled if all are OFF, else disabled if any are on
                else if (tpsel == tp)
                    tp.Enabled = !tp.Enabled;
            }
        }

        public void SaveSettings()
        {
            int index = 0;
            int sel = 0;

            foreach (GalMapType tp in galacticMapTypes)
            {
                sel |= (tp.Enabled ? 1 : 0) << index;
                index++;
            }

            SQLiteDBClass.PutSettingInt("GalObjectsEnable", sel);
        }

        public GalacticMapObject Find(string name, bool contains = false , bool disregardenable = false)
        {
            if (galacticMapObjects != null && name.Length>0)
            {
                foreach (GalacticMapObject gmo in galacticMapObjects)
                {
                    if ( gmo.name.Equals(name,StringComparison.InvariantCultureIgnoreCase) || (contains && gmo.name.IndexOf(name,StringComparison.InvariantCultureIgnoreCase)>=0))
                    {
                        if ( gmo.galMapType.Enabled || disregardenable )
                            return gmo;
                    }
                }
            }

            return null;
        }

        public List<string> GetGMONames()
        {
            List<string> ret = new List<string>();

            if (galacticMapObjects != null)
            {
                foreach (GalacticMapObject gmo in galacticMapObjects)
                {
                    ret.Add(gmo.name);
                }
            }

            return ret;
        }
            
    }
}
