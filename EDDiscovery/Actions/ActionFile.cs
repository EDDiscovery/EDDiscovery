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
            filevariables = new ConditionVariables();       // filevariables are only cleared on creation
            Clear();
        }

        public ActionFile(string f, string n)
        {
            filevariables = new ConditionVariables();
            Clear(f, n);
        }

        public void Clear(string f="", string n="")         // clear all data read from file
        {
            actioneventlist = new ConditionLists();
            actionprogramlist = new ActionProgramList();
            enabled = true;
            installationvariables = new ConditionVariables();
            filepath = f;
            name = n;
        }

        public ConditionLists actioneventlist { get; private set; }                        // note we use the list, but not the evaluate between conditions..
        public ActionProgramList actionprogramlist { get; private set;}                    // programs associated with this pack
        public ConditionVariables installationvariables { get; private set; }              // used to pass to the installer various options, such as disable other packs
        public ConditionVariables filevariables { get; private set; }                      // variables defined using the static.. private to this program.  Not persistent. 
        public string filepath { get; private set; }                                       // where it came from
        public string name { get; private set; }                                           // its logical name
        public bool enabled { get; private set; }                                          // if enabled.

        public void ChangeEventList(ConditionLists s)
        {
            actioneventlist = s;
        }

        public void ChangeInstallationVariables(ConditionVariables v)
        {
            installationvariables = v;
        }

        public void SetFileVariable(string n, string v)
        {
            filevariables[n] = v;
        }

        public void DeleteFileVariable(string n)
        {
            filevariables.Delete(n);
        }

        public string Read(JObject jo , out bool readenable)            // KEEP JSON reader for backwards compatibility.
        {
            string errlist = "";
            readenable = false;

            try
            {
                JArray ivarja = (JArray)jo["Install"];

                if (!JSONHelper.IsNullOrEmptyT(ivarja))
                {
                    installationvariables.FromJSONObject(ivarja);
                }

                JObject jcond = (JObject)jo["Conditions"];

                if (jcond != null)
                    actioneventlist.FromJSON(jcond);

                JObject jprog = (JObject)jo["Programs"];

                if (jprog != null)
                {
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
                }

                if (jo["Enabled"] != null)
                {
                    enabled = (bool)jo["Enabled"];
                    readenable = true;
                }

                //System.Diagnostics.Debug.WriteLine("JSON read enable " + enabled);

                return errlist;
            }
            catch
            {
                return errlist + " Also Missing JSON fields";
            }
        }

        public string ReadFile(string filename, out bool readenable)     // string, empty if no errors
        {
            readenable = false;

            Clear(filename, Path.GetFileNameWithoutExtension(filename));

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
                            return Read(jo,out readenable);
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

                        while ((line = sr.ReadLine()) != null)
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

                                readenable = true;
                            }
                            else if (line.StartsWith("PROGRAM", StringComparison.InvariantCultureIgnoreCase))
                            {
                                ActionProgram ap = new ActionProgram();
                                string err = ap.Read(sr, ref lineno);

                                if (err.Length > 0)
                                    return name + " " + err;

                                ap.Rename(line.Substring(7).Trim());        //MUST rename now, after read, as read clears the name expecting PROGRAM
                                actionprogramlist.Add(ap);
                            }
                            else if (line.StartsWith("INCLUDE", StringComparison.InvariantCultureIgnoreCase))
                            {
                                string incfilename = line.Substring(7).Trim();
                                if (!incfilename.Contains("/") && !incfilename.Contains("\\"))
                                    incfilename = Path.Combine(Path.GetDirectoryName(filename), incfilename);

                                ActionProgram ap = new ActionProgram("",incfilename);   // NAME will be filled in by PROGRAM statement in file

                                string err = ap.ReadFile(incfilename);

                                if (err.Length > 0)
                                    return name + " " + err;

                                actionprogramlist.Add(ap);
                            }
                            else if (line.StartsWith("EVENT", StringComparison.InvariantCultureIgnoreCase))
                            {
                                Condition c = new Condition();
                                string err = c.Read(line.Substring(5).Trim(), true);
                                if (err.Length > 0)
                                    return name + " " + lineno + " " + err + Environment.NewLine;
                                else if (c.action.Length == 0 || c.eventname.Length == 0)
                                    return name + " " + lineno + " EVENT Missing event name or action" + Environment.NewLine;

                                actioneventlist.Add(c);
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
                            else if (line.StartsWith("//") || line.StartsWith("REM", StringComparison.InvariantCultureIgnoreCase) || line.Length == 0)
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

        public void WriteJSON(StreamWriter sr)          // kept for reference and debugging..  files are now written in ASCII
        {
            JObject jo = new JObject();
            jo["Conditions"] = actioneventlist.GetJSONObject();

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

        public bool WriteFile()
        {
            try
            {
                using (StreamWriter sr = new StreamWriter(filepath))
                {
                    string rootpath = Path.GetDirectoryName(filepath) + "\\";

                    sr.WriteLine("ACTIONFILE V4");
                    sr.WriteLine();
                    sr.WriteLine("ENABLED " + enabled);
                    sr.WriteLine();

                    if (installationvariables.Count > 0)
                    {
                        sr.WriteLine(installationvariables.ToString(prefix: "INSTALL ", separ: Environment.NewLine));
                        sr.WriteLine();
                    }

                    if (actioneventlist.Count > 0)
                    {
                        for (int i = 0; i < actioneventlist.Count; i++)
                            sr.WriteLine("EVENT " + actioneventlist.Get(i).ToString(includeaction: true));

                        sr.WriteLine();
                    }

                    if (actionprogramlist.Count > 0)
                    {
                        for (int i = 0; i < actionprogramlist.Count; i++)
                        {
                            ActionProgram f = actionprogramlist.Get(i);

                            sr.WriteLine("//*************************************************************");
                            sr.WriteLine("// " + f.Name);
                            string evl = "";

                            for (int ic = 0; ic < actioneventlist.Count; ic++)
                            {
                                Condition c = actioneventlist.Get(ic);
                                if (c.action.Equals(f.Name))
                                {
                                    string e = c.eventname;

                                    if (!c.AlwaysTrue())
                                    {
                                        e += "?(" + c.ToString() + ")";
                                    }

                                    if (c.actiondata.Length > 0)
                                        e += "(" + c.actiondata + ")";

                                    e += ", ";

                                    if (evl.Length + e.Length > 100)
                                    {
                                        sr.WriteLine("// Events: " + evl);
                                        evl = "";
                                    }

                                    evl += e;
                                }
                            }

                            if (evl.Length > 0)
                            {
                                evl = evl.Substring(0, evl.Length - 2);
                                sr.WriteLine("// Events: " + evl);
                            }

                            sr.WriteLine("//*************************************************************");


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

        static public bool SetEnableFlag(string file, bool enable)              // change the enable flag. Read in,write out.
        {                                                                       // true if managed to change it..
            try
            {
                ActionFile f = new ActionFile();

                bool readenable;
                if (f.ReadFile(file, out readenable).Length == 0)        // read it in..
                {
                    f.enabled = enable;
                    f.WriteFile();                                // write it out.
                  //  System.Diagnostics.Debug.WriteLine("Set Enable " + file + " " + enable );
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        static public ConditionVariables ReadVarsAndEnableFromFile(string file, out bool? enable)
        {
            ActionFile f = new ActionFile();
            enable = null;

            bool readenable;
            if (f.ReadFile(file,out readenable).Length == 0)        // read it in..
            {
                if (readenable)
                    enable = f.enabled;
                //System.Diagnostics.Debug.WriteLine("Enable vars read " + file + " " + enable);
                return f.installationvariables;
            }
            else
                return null;
        }

    }
}
