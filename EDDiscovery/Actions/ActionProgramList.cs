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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Actions
{
    public class ActionProgramList                  // Holder of programs loaded into memory
    {
        public ActionProgramList()
        {
            Clear();
        }

        private List<ActionProgram> programs;

        public ActionProgram Get(string name)
        {
            int existing = programs.FindIndex(x => x.Name.Equals(name));

            return (existing >= 0) ? programs[existing] : null;
        }

        public string[] GetActionProgramList(bool markfileasext = false)
        {
            string[] ret = new string[programs.Count];
            for (int i = 0; i < programs.Count; i++)
            {
                ret[i] = programs[i].Name;
                if (markfileasext && programs[i].StoredInFile != null)
                    ret[i] += " (Ext)";
            }

            return ret;
        }

        public void Clear()
        {
            programs = new List<ActionProgram>();
        }

        public string ToJSON()
        {
            return ToJSONObject().ToString();
        }

        public JObject ToJSONObject()
        {
            JObject evt = new JObject();

            JArray jf = new JArray();

            foreach (ActionProgram ap in programs)
            {
                JObject j1 = ap.ToJSON();
                jf.Add(j1);
            }

            evt["ProgramSet"] = jf;

            return evt;
        }

        public string FromJSON(string s)
        {
            try
            {
                JObject jo = (JObject)JObject.Parse(s);
                return FromJSONObject(jo);
            }
            catch
            {
                return "Exception Bad JSON";
            }
        }

        public string FromJSONObject(JObject jo)
        {
            string errlist = "";

            try
            {
                Clear();

                JArray jf = (JArray)jo["ProgramSet"];

                foreach (JObject j in jf)
                {
                    ActionProgram ap;
                    string err = ActionProgram.FromJSON(j, out ap);

                    if (err.Length == 0 && ap != null)         // if can't load, we can't trust the pack, so indicate error so we can let the user manually sort it out
                        programs.Add(ap);
                    else
                        errlist += err;
                }

                return errlist;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Dump:" + ex.StackTrace);
                errlist = "Exception bad JSON";
            }

            return errlist;
        }

        public void AddOrChange(ActionProgram ap)
        {
            int existing = programs.FindIndex(x => x.Name.Equals(ap.Name));

            if (existing >= 0)
                programs[existing] = ap;
            else
                programs.Add(ap);
        }

        public void Delete(string name)
        {
            int existing = programs.FindIndex(x => x.Name.Equals(name));

            if (existing >= 0)
                programs.RemoveAt(existing);
        }
    }

}
