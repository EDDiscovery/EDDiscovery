/*
 * Copyright © 2017 EDDiscovery development team
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// A file holds a set of conditions and programs associated with them

namespace EDDiscovery.Actions
{
    public class ActionFile         
    {
        public ActionFile(ConditionLists c, ActionProgramList p , string f , string n, bool e , ConditionVariables ivar = null )
        {
            actionfieldfilter = c;
            actionprogramlist = p;
            filepath = f;
            name = n;
            enabled = e;
            installationvariables = new ConditionVariables();
            if (ivar != null)
                installationvariables.Add(ivar);
        }

        public ConditionLists actionfieldfilter;
        public ActionProgramList actionprogramlist;
        public ConditionVariables installationvariables;                // used to pass to the installer various options, such as disable other packs
        public string filepath;
        public string name;
        public bool enabled;

        public static string ReadFile( string filename , out ActionFile af)
        {
            af = null;
            string errlist = "";

            using (StreamReader sr = new StreamReader(filename))         // read directly from file..
            {
                string json = sr.ReadToEnd();
                sr.Close();

                try
                {
                    JObject jo = (JObject)JObject.Parse(json);

                    JObject jcond = (JObject)jo["Conditions"];
                    JObject jprog = (JObject)jo["Programs"];
                    bool en = (bool)jo["Enabled"];

                    JArray ivarja = (JArray)jo["Install"];

                    ConditionVariables ivars = new ConditionVariables();
                    if ( !JSONHelper.IsNullOrEmptyT(ivarja) )
                    {
                        ivars.FromJSONObject(ivarja);
                    }

                    ConditionLists cond = new ConditionLists();
                    ActionProgramList prog = new ActionProgramList();

                    if (cond.FromJSON(jcond))
                    {
                        errlist = prog.FromJSONObject(jprog);

                        if (errlist.Length == 0)
                        {
                            af = new ActionFile(cond, prog, filename, Path.GetFileNameWithoutExtension(filename), en, ivars);
                        }
                    }
                    else
                        errlist = "Bad JSON in conditions";
                        
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Dump:" + ex.StackTrace);
                    errlist = "Bad JSON in File";
                }

                return errlist;
            }
        }

        public bool SaveFile()
        {
            JObject jo = new JObject();
            jo["Conditions"] = actionfieldfilter.GetJSONObject();
            jo["Programs"] = actionprogramlist.ToJSONObject();
            jo["Enabled"] = enabled;
            jo["Install"] = installationvariables.ToJSONObject();

            string json = jo.ToString(Formatting.Indented);

            try
            {
                using (StreamWriter sr = new StreamWriter(filepath))
                {
                    sr.Write(json);
                    sr.Close();
                }

                return true;
            }
            catch
            { }

            return false;
        }
    }

}
