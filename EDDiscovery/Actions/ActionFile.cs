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
        public ActionFile()
        {

        }

        public ActionFile(ConditionLists c, ActionProgramList p , string f , string n, bool e , ConditionVariables ivar = null )
        {
            actionfieldfilter = c;
            actionprogramlist = p;
            enabled = e;
            installationvariables = new ConditionVariables();
            filepath = f;
            name = n;
            if (ivar != null)
                installationvariables.Add(ivar);
        }

        public ConditionLists actionfieldfilter;
        public ActionProgramList actionprogramlist;
        public ConditionVariables installationvariables;                // used to pass to the installer various options, such as disable other packs
        public string filepath;
        public string name;
        public bool enabled;

        public string Read(JObject jo)
        {
            string errlist = "";

            try
            {
                JArray ivarja = (JArray)jo["Install"];

                if (!JSONHelper.IsNullOrEmptyT(ivarja))
                {
                    installationvariables.FromJSONObject(ivarja);
                }

                JObject jcond = (JObject)jo["Conditions"];

                if (actionfieldfilter.FromJSON(jcond))
                {
                    JObject jprog = (JObject)jo["Programs"];
                    JArray jf = (JArray)jprog["ProgramSet"];

                    foreach (JObject j in jf)
                    {
                        ActionProgram ap = new ActionProgram();
                        string lerr = ap.Read(j);

                        if (lerr.Length == 0)         // if can't load, we can't trust the pack, so indicate error so we can let the user manually sort it out
                            actionprogramlist.Add(ap);
                        else
                            errlist += lerr;
                    }

                    return errlist;
                }
                else
                    errlist = "Bad JSON in conditions";

                enabled = (bool)jo["Enabled"];

                return errlist;
            }
            catch
            {
                return errlist + " Also Missing JSON fields";
            }
        }

        public string ReadFile(string filename)     // string, empty if no errors
        {
            actionprogramlist = new ActionProgramList();
            actionfieldfilter = new ConditionLists();
            installationvariables = new ConditionVariables();
            enabled = false;
            filepath = filename;
            name = Path.GetFileNameWithoutExtension(filename);

            try
            {
                using (StreamReader sr = new StreamReader(filename))         // read directly from file..
                {
                    string firstline = sr.ReadLine();

                    if (firstline == "{")
                    {
                        string json = firstline + Environment.NewLine + sr.ReadToEnd();
                        sr.Close();

                        try
                        {
                            JObject jo = JObject.Parse(json);
                            return Read(jo);
                        }
                        catch
                        {
                            return "Invalid JSON" + Environment.NewLine;
                        }
                    }
                    else if (firstline == "ACTIONFILE V4")
                    {
                        string line;
                        int lineno = 1;     // on actionFILE V4

                        while( (line= sr.ReadLine()) != null)
                        {
                            lineno++;       // on line of read..

                            line.Trim();
                            if (line.StartsWith("ENABLED", StringComparison.InvariantCultureIgnoreCase))
                            {
                                line = line.Substring(7).Trim().ToLower();
                                if (line == "true")
                                    enabled = true;
                                else if (line == "false")
                                    enabled = false;
                                else
                                    return name + " " + lineno + " ENABLED is neither true or false" + Environment.NewLine;
                            }
                            else if (line.StartsWith("PROGRAM", StringComparison.InvariantCultureIgnoreCase))
                            {
                                ActionProgram ap = new ActionProgram();
                                string err = ap.Read(sr, ref lineno);

                                if (err.Length > 0)
                                    return name + " " + err;

                                ap.Name = line.Substring(7).Trim();

                                actionprogramlist.Add(ap);
                            }
                            else if (line.StartsWith("INCLUDE", StringComparison.InvariantCultureIgnoreCase))
                            {
                                ActionProgram ap = new ActionProgram();

                                string incfilename = line.Substring(7).Trim();
                                if (!incfilename.Contains("/") && !incfilename.Contains("\\"))
                                    incfilename = Path.Combine(Path.GetDirectoryName(filename), incfilename);

                                string err = ap.ReadFile(incfilename);

                                if (err.Length > 0)
                                    return name + " " + err;

                                ap.StoredInSubFile = incfilename;       // FULL path note..
                                actionprogramlist.Add(ap);
                            }
                            else if (line.StartsWith("EVENT", StringComparison.InvariantCultureIgnoreCase))
                            {
                                Condition c = new Condition();
                                string err = c.Read(line.Substring(5).Trim(),true);
                                if (err.Length > 0)
                                    return name + " " + lineno + " " + err + Environment.NewLine;
                                else if (c.action.Length == 0 || c.eventname.Length == 0)
                                    return name + " " + lineno + " EVENT Missing action" + Environment.NewLine;

                                actionfieldfilter.Add(c);
                            }
                            else if (line.StartsWith("INSTALL", StringComparison.InvariantCultureIgnoreCase))
                            {
                                ConditionVariables c = new ConditionVariables();
                                if (c.FromString(line.Substring(7).Trim(), ConditionVariables.FromMode.SingleEntry) && c.Count == 1)
                                {
                                    installationvariables.Add(c);
                                }
                                else
                                    return name + " " + lineno + " Incorrectly formatted INSTALL variable" + Environment.NewLine;
                            }
                            else if ( line.StartsWith("//") || line.Length == 0)
                            {
                            }
                            else
                                return name + " " + lineno + " Invalid command" + Environment.NewLine;
                        }

                        return "";
                    }
                    else
                    {
                        return name + " Header file type not recognised" + Environment.NewLine; 
                    }
                }
            }
            catch
            {
                return filename + " Not readable" + Environment.NewLine;
            }
        }

        public void WriteJSON(StreamWriter sr)     
        {
            JObject jo = new JObject();
            jo["Conditions"] = actionfieldfilter.GetJSONObject();

            JObject evt = new JObject();
            JArray jf = new JArray();

            ActionProgram ap = null;
            for (int i = 0; (ap = actionprogramlist.Get(i)) != null; i++)
            {
                JObject j1 = ap.WriteJSONObject();
                jf.Add(j1);
            }

            evt["ProgramSet"] = jf;
            jo["Programs"] = evt;
            jo["Enabled"] = enabled;
            jo["Install"] = installationvariables.ToJSONObject();

            string json = jo.ToString(Formatting.Indented);
            sr.Write(json);
        }

        public bool WriteFile(bool json = false)
        {
            try
            {
                using (StreamWriter sr = new StreamWriter(filepath))
                {
                    string rootpath = Path.GetDirectoryName(filepath) + "\\";

                    if (json)
                        WriteJSON(sr);
                    else
                    {
                        sr.WriteLine("ACTIONFILE V4");
                        sr.WriteLine();
                        sr.WriteLine("ENABLED " + enabled);
                        sr.WriteLine();
                        sr.WriteLine(installationvariables.ToString(prefix: "INSTALL ", separ: Environment.NewLine));
                        sr.WriteLine();

                        for (int i = 0; i < actionfieldfilter.Count; i++)
                            sr.WriteLine("EVENT " + actionfieldfilter.Get(i).ToString(includeaction: true));

                        sr.WriteLine();

                        for (int i = 0; i < actionprogramlist.Count; i++)
                        {
                            ActionProgram f = actionprogramlist.Get(i);

                            if (f.StoredInSubFile != null)
                            {
                                string full = f.StoredInSubFile;        // try and simplify the path here..
                                if (full.StartsWith(rootpath))
                                    full = full.Substring(rootpath.Length);

                                sr.WriteLine("INCLUDE " + full);
                                f.WriteFile(f.StoredInSubFile);
                            }
                            else
                                f.Write(sr);

                            sr.WriteLine();
                        }
                    }

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
