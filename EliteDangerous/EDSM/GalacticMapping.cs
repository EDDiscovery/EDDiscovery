/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */

using EliteDangerousCore.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace EliteDangerousCore.EDSM
{
    public class GalacticMapping
    {
        private string GalacticMappingFile { get { return Path.Combine(EliteConfigInstance.InstanceOptions.AppDataDirectory, "galacticmapping.json"); } }

        public List<GalacticMapObject> galacticMapObjects = null;
        public List<GalMapType> galacticMapTypes = null;

        public bool Loaded { get { return galacticMapObjects != null; } }

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
                string url = EDSMClass.ServerAddress + "en/galactic-mapping/json-edd";
                bool newfile;

                return BaseUtils.DownloadFile.HTTPDownloadFile(url, GalacticMappingFile, false, out newfile);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("DownloadFromEDSM exception:" + ex.Message);
            }

            return false;
        }

        public bool GalMapFilePresent()
        {
            return File.Exists(GalacticMappingFile);
        }

        public bool ParseData()
        {
            var gmobjects = new List<GalacticMapObject>();

            try
            {
                string json = BaseUtils.FileHelpers.TryReadAllTextFromFile(GalacticMappingFile);

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
                        gmobjects.Add(galobject);
                    }

                    galacticMapObjects = gmobjects;

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
