/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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
using Newtonsoft.Json.Linq;

namespace EliteDangerousCore
{
    public class EliteDangerousTypesFromJSON
    {
        static public EDGovernment Government2ID(JToken jt)
        {

            if (jt == null)
                return EDGovernment.Unknown;


            if (jt.Type != JTokenType.String)
                return EDGovernment.Unknown;


            string str = jt.Value<string>();

            foreach (var govid in Enum.GetValues(typeof(EDGovernment)))
            {
                if (str.Equals(govid.ToString().Replace("_", " ")))
                    return (EDGovernment)govid;
                //System.Console.WriteLine(govid.ToString());
            }

            return EDGovernment.Unknown;
        }

        static public EDAllegiance Allegiance2ID(JToken jt)
        {

            if (jt == null)
                return EDAllegiance.Unknown;


            if (jt.Type != JTokenType.String)
                return EDAllegiance.Unknown;


            string str = jt.Value<string>();

            foreach (var govid in Enum.GetValues(typeof(EDAllegiance)))
            {
                if (str.Equals(govid.ToString().Replace("_", " ")))
                    return (EDAllegiance)govid;
                //System.Console.WriteLine(govid.ToString());
            }

            return EDAllegiance.Unknown;
        }


        static public EDState EDState2ID(JToken jt)
        {

            if (jt == null)
                return EDState.Unknown;


            if (jt.Type != JTokenType.String)
                return EDState.Unknown;


            string str = jt.Value<string>();

            foreach (var govid in Enum.GetValues(typeof(EDState)))
            {
                if (str.Equals(govid.ToString().Replace("_", " ")))
                    return (EDState)govid;
                //System.Console.WriteLine(govid.ToString());
            }

            return EDState.Unknown;
        }


        static public EDSecurity EDSecurity2ID(JToken jt)
        {

            if (jt == null)
                return EDSecurity.Unknown;


            if (jt.Type != JTokenType.String)
                return EDSecurity.Unknown;


            string str = jt.Value<string>();

            foreach (var govid in Enum.GetValues(typeof(EDSecurity)))
            {
                if (str.Equals(govid.ToString().Replace("_", " ")))
                    return (EDSecurity)govid;
                //System.Console.WriteLine(govid.ToString());
            }

            return EDSecurity.Unknown;
        }

        static public EDStationType EDStationType2ID(JToken jt)
        {

            if (jt == null)
                return EDStationType.Unknown;


            if (jt.Type != JTokenType.String)
                return EDStationType.Unknown;


            string str = jt.Value<string>();

            foreach (var govid in Enum.GetValues(typeof(EDStationType)))
            {
                if (str.Equals(govid.ToString().Replace("_", " ")))
                    return (EDStationType)govid;
                //System.Console.WriteLine(govid.ToString());
            }

            return EDStationType.Unknown;
        }

        static public EDEconomy EDEconomy2ID(JToken jt)
        {

            if (jt == null)
                return EDEconomy.Unknown;


            if (jt.Type != JTokenType.String)
                return EDEconomy.Unknown;


            string str = jt.Value<string>();

            return String2Economy(str);
        }

        static public EDEconomy String2Economy(string str)
        {
            foreach (var govid in Enum.GetValues(typeof(EDEconomy)))
            {
                if (str.Equals(govid.ToString().Replace("___", ".").Replace("__", "-").Replace("_", " ")))
                    return (EDEconomy)govid;
                //System.Console.WriteLine(govid.ToString());
            }

            return EDEconomy.Unknown;
        }


        static public List<EDEconomy> EDEconomies2ID(JArray ja)
        {
            List<EDEconomy> economies = new List<EDEconomy>();

            if (ja == null)
                return null;

            for (int ii = 0; ii < ja.Count; ii++)
            {
                string ecstr = ja[ii].Value<string>();
                economies.Add(String2Economy(ecstr));

            }
            return economies;
        }

    }
}
