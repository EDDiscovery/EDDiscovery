/*
 * Copyright © 2015 - 2017 EDDiscovery development team
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

using System;
using System.Collections.Generic;


namespace EliteDangerousCore
{
    public static class JournalFieldNaming
    {
        public static string FixCommodityName(string fdname)      // instances in log on mining and mission entries of commodities in this form, back into fd form
        {
            if (fdname.Length >= 8 && fdname.StartsWith("$") && fdname.EndsWith("_name;", System.StringComparison.InvariantCultureIgnoreCase))
                fdname = fdname.Substring(1, fdname.Length - 7); // 1 for '$' plus 6 for '_name;'

            return fdname;
        }

        static public string FDNameTranslation(string old)
        {
            return MaterialCommodityData.FDNameTranslation(old);
        }

        static public string NormaliseMaterialCategory(string cat)
        {
            switch (cat.ToLowerInvariant())
            {
                case "raw":
                case "encoded":
                case "manufactured":
                    return cat;
                case "$microresource_category_encoded;":
                    return "Encoded";
                case "$microresource_category_elements;":
                    return "Raw";
                case "$microresource_category_manufactured;":
                    return "Manufactured";
            }

            // Fallback decoding
            if (cat.Contains("$"))
            {
                int i = cat.LastIndexOf('_');
                if (i != -1 && i < cat.Length - 1)
                    cat = cat.Substring(i + 1).Replace(";", "");
            }

            return cat;
        }

        public static string RLat(double? lv)
        {
            if (lv.HasValue)
                return RLat(lv.Value);
            else
                return null;
        }


        public static string RLat(double lv)      
        {
            long arcsec = (long)(lv * 60 * 60);          // convert to arc seconds
            string marker = (arcsec < 0) ? "S" : "N";       // presume lat
            arcsec = Math.Abs(arcsec);
            return string.Format("{0}°{1} {2}'{3}\"", arcsec / 3600, marker, (arcsec / 60) % 60, arcsec % 60);
        }

        public static string RLong(double? lv)
        {
            if (lv.HasValue)
                return RLong(lv.Value);
            else
                return null;
        }

        public static string RLong(double lv)      
        {
            long arcsec = (long)(lv * 60 * 60);          // convert to arc seconds
            string marker = (arcsec < 0) ? "W" : "E";       // presume lat
            arcsec = Math.Abs(arcsec);
            return string.Format("{0}°{1} {2}'{3}\"", arcsec / 3600, marker, (arcsec / 60) % 60, arcsec % 60);
        }

        static public string GetBetterMissionName(string inname)
        {
            return inname.Replace("_name", "").SplitCapsWordFull();
        }

        static public string ShortenMissionName(string inname)
        {
            return inname.Replace("Mission ", "",StringComparison.InvariantCultureIgnoreCase).SplitCapsWordFull();
        }

        static Dictionary<string, string> replaceslots = new Dictionary<string, string>
        {
            {"Engines",     "Thrusters"},
        };

        static public string GetBetterSlotName(string s)
        {
            return s.SplitCapsWordFull(replaceslots);
        }

        static public string GetBetterItemName(string s)           
        {
            if (s.Length>0)         // accept empty string, some of the fields are purposely blank from the journal because they are not set for a particular transaction
            {
                ShipModuleData.ShipModule item = ShipModuleData.Instance.GetItemProperties(s);
                return item.ModName;
            }
            else
                return s;
        }

        static public string GetBetterShipName(string inname)
        {
            ShipModuleData.ShipInfo i = ShipModuleData.Instance.GetShipProperty(inname, ShipModuleData.ShipPropID.Name);

            if (i != null)
                return (i as ShipModuleData.ShipInfoString).Value;
            else
            {
                System.Diagnostics.Debug.WriteLine("Unknown FD ship ID" + inname);
                return inname.SplitCapsWordFull();
            }
        }

        static public string GetBetterTargetTypeName(string s)      // has to deal with $ and underscored
        {
            //string x = s;
            if (s.StartsWith("$"))
                s = s.Substring(1);
            return s.SplitCapsWordFull();
        }

        static public string NormaliseFDItemName(string s)      // has to deal with $int and $hpt.. This takes the FD name and keeps it, but turns it into the form
        {                                                       // used by Coriolis/Frontier API
            //string x = s;
            if (s.StartsWith("$int_"))
                s = s.Replace("$int_", "Int_");
            if (s.StartsWith("int_"))
                s = s.Replace("int_", "Int_");
            if (s.StartsWith("$hpt_"))
                s = s.Replace("$hpt_", "Hpt_");
            if (s.StartsWith("hpt_"))
                s = s.Replace("hpt_", "Hpt_");
            if (s.Contains("_armour_"))
                s = s.Replace("_armour_", "_Armour_");      // normalise to Armour upper cas.. its a bit over the place with case..
            if (s.EndsWith("_name;"))
                s = s.Substring(0, s.Length - 6);
            if (s.StartsWith("$"))                          // seen instances of $python_armour..
                s = s.Substring(1);

            return s;
        }

        static public string NormaliseFDSlotName(string s)            // FD slot name, anything to do.. leave in as there might be in the future
        {
            return s;
        }

        static public string NormaliseBodyType(string s)            // FD slot name, anything to do.. leave in as there might be in the future
        {
            if (s == null)
                return "Unknown";
            else if (s.Equals("Null",StringComparison.InvariantCultureIgnoreCase))
                s = "Barycentre";
            return s;
        }

        static public string NormaliseFDShipName(string inname)            // FD ship names.. tend to change case.. Fix
        {
            ShipModuleData.ShipInfo i = ShipModuleData.Instance.GetShipProperty(inname, ShipModuleData.ShipPropID.FDID);
            if (i != null)
                return (i as ShipModuleData.ShipInfoString).Value;
            else
            {
                System.Diagnostics.Debug.WriteLine("Unknown FD ship ID" + inname);
                return inname;
            }
        }

        static public string CheckLocalisation(string loc, string alt)      // instances of ! # $int in localisation strings, screen out
        {
            bool invalid = loc == null || loc.Length < 2 || loc.StartsWith("$int", StringComparison.InvariantCultureIgnoreCase) || loc.StartsWith("$hpt", StringComparison.InvariantCultureIgnoreCase);
            return invalid ? alt.SplitCapsWordFull() : loc;
        }

        static public string CheckLocalisationTranslation(string loc, string alt)      // instances of ! # $int in localisation strings, screen out
        {
            if (BaseUtils.Translator.Instance.Translating)          // if we are translating, use the alt name as its the most valid..
                return alt;
            else
                return CheckLocalisation(loc, alt);
        }
    }
}
