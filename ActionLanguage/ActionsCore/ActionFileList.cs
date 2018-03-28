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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conditions;

namespace ActionLanguage
{
    public class ActionFileList
    {
        private List<ActionFile> actionfiles = new List<ActionFile>();

        public List<string> GetList { get { return (from af in actionfiles select af.name).ToList(); } }

        public IEnumerable<ActionFile> Enumerable { get { return actionfiles; } }

        // normally pack names are case sensitive, except when we are checking it can be written to a file.. then we would want a case insensitive version
        public ActionFile Get(string name, StringComparison c = StringComparison.InvariantCulture) { return actionfiles.Find(x => x.name.Equals(name,c)); }

        public void CreateSet(string s, string appfolder)
        {
            ActionFile af = new ActionFile(appfolder + "\\\\" + s + ".act", s);
            af.WriteFile();
            actionfiles.Add(af);
        }

        public class MatchingSets
        {
            public ActionFile af;                           // file it came from
            public List<Condition> cl;       // list of matching events..
            public List<Condition> passed;   // list of passed events after condition checked.
        }

        // any with refresh flag set?
        public bool IsConditionFlagSet(string flagstart)
        {
            foreach (ActionFile af in actionfiles)
            {
                if (af.actioneventlist.IsConditionFlagSet(flagstart))
                    return true;
            }
            return false;
        }

        // get actions in system matching eventname
        public List<MatchingSets> GetMatchingConditions(string eventname, string flagstart = null)        // flag start is compared with start of actiondata
        {
            List<MatchingSets> apl = new List<MatchingSets>();

            foreach (ActionFile af in actionfiles)
            {
                if (af.enabled)         // only enabled files are checked
                {
                    List<Condition> events = af.actioneventlist.GetConditionListByEventName(eventname, flagstart);

                    if (events != null)     // and if we have matching event..
                    {
                        apl.Add(new MatchingSets() { af = af, cl = events });
                    }
                }
            }

            return apl;
        }

        // triage found actions and see which ones are runnable
        // cls = object from which to get any needed values from, can be null
        // if se is set passed enabled string expansion of arguments in condition of event..

        public int CheckActions(List<ActionFileList.MatchingSets> ale, Object cls, ConditionVariables othervars,
                                            ConditionFunctions se = null)
        {
            ConditionVariables valuesneeded = new ConditionVariables();

            if (cls != null)
            {
                foreach (MatchingSets ae in ale)       // for all files
                {
                    foreach (Condition fe in ae.cl)        // find all values needed
                        fe.IndicateValuesNeeded(ref valuesneeded);
                }

                valuesneeded.GetValuesIndicated(cls);     // get the values needed for the conditions
            }

            valuesneeded.Add(othervars);

            int progs = 0;

            ConditionFunctions cf = new ConditionFunctions(valuesneeded, null);

            foreach (MatchingSets ae in ale)       // for all files
            {
                string errlist = null;
                ae.passed = new List<Condition>();

                //System.Diagnostics.Debug.WriteLine("Check `" + ae.af.name + ae.af.actionfieldfilter.ToString() + "`");
                //ActionData.DumpVars(valuesneeded, " Test var:");

                ae.af.actioneventlist.CheckConditions(ae.cl, valuesneeded, out errlist, ae.passed, cf);   // indicate which ones passed
                progs += ae.passed.Count;
            }

            return progs;
        }

        // now = true run immediately, else defer to current programs
        public void RunActions(bool now, List<ActionFileList.MatchingSets> ale, ActionRun run, ConditionVariables inputparas)
        {
            foreach (ActionFileList.MatchingSets ae in ale)          // for every file which passed..
            {
                foreach (Condition fe in ae.passed)          // and every condition..
                {
                    Tuple<ActionFile, ActionProgram> ap = FindProgram(fe.action, ae.af);          // find program using this name, prefer this action file first

                    if (ap != null)     // program got,
                    {
                        ConditionVariables adparas = new ConditionVariables();
                        string flags;
                        adparas.FromActionDataString(fe.actiondata, out flags);

                        inputparas.Add(adparas);

                        run.Run(now, ap.Item1, ap.Item2, inputparas);
                    }
                }
            }
        }

        public Tuple<ActionFile, ActionProgram> FindProgram(string packname, string progname)
        {
            ActionFile f = actionfiles.Find(x => x.name.Equals(packname));

            if (f != null)
            {
                ActionProgram ap = f.actionprogramlist.Get(progname);   // get in local program list first

                if (ap != null)
                    return new Tuple<ActionFile, ActionProgram>(f, ap);
            }

            return null;
        }

        public Tuple<ActionFile, ActionProgram> FindProgram(string req, ActionFile preferred = null)        // find a program 
        {
            ActionProgram ap = null;

            string file = null, prog;
            int colon = req.IndexOf("::");

            if (colon != -1)
            {
                file = req.Substring(0, colon);
                prog = req.Substring(colon + 2);
            }
            else
                prog = req;

            if (file != null)                             // if file given, only search that
            {
                ActionFile f = actionfiles.Find(x => x.name.Equals(file));

                if (f != null)      // found file..
                {
                    ap = f.actionprogramlist.Get(prog);

                    return (ap != null) ? new Tuple<ActionFile, ActionProgram>(f, ap) : null;
                }
            }
            else
            {
                if (preferred != null)          // if no file stated, and we have a preferred
                {
                    ap = preferred.actionprogramlist.Get(prog);   // get in local program list first

                    if (ap != null)
                        return new Tuple<ActionFile, ActionProgram>(preferred, ap);
                }

                foreach (ActionFile f in actionfiles)
                {
                    ap = f.actionprogramlist.Get(prog);

                    if (ap != null)         // gotcha
                        return new Tuple<ActionFile, ActionProgram>(f, ap);
                }
            }

            return null;
        }

        public string LoadAllActionFiles(string appfolder)              // loads or reloads the actions, dep on if present in list
        {
            if (!Directory.Exists(appfolder))
                Directory.CreateDirectory(appfolder);

            FileInfo[] allFiles = Directory.EnumerateFiles(appfolder, "*.act", SearchOption.AllDirectories).Select(f => new FileInfo(f)).OrderBy(p => p.LastWriteTime).ToArray();

            string errlist = "";

            foreach (FileInfo f in allFiles)
            {
                int indexof = actionfiles.FindIndex(x => x.filepath.Equals(f.FullName));

                ActionFile af;

                if (indexof == -1)                  // if we don't have it, new it.. else overwrite it
                    af = new ActionFile();
                else
                    af = actionfiles[indexof];      // overwriting keeps any dynamic data action files have

                bool readenable;
                string err = af.ReadFile(f.FullName, out readenable);       // re-read it in.  Note it does not kill the fileaveriables

                if (err.Length == 0)
                {
                    if (indexof == -1)
                    {
                        System.Diagnostics.Debug.WriteLine("Add pack " + af.name);
                        actionfiles.Add(af);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Update Pack " + af.name);
                    }
                }
                else
                {
                    errlist += "File " + f.FullName + " failed to load: " + Environment.NewLine + err;
                    if (indexof != -1)
                    {
                        actionfiles.RemoveAt(indexof);          // remove dead packs
                        System.Diagnostics.Debug.WriteLine("Delete Pack " + af.name);
                    }
                }
            }

            return errlist;
        }

        #region special helpers

        // give back all conditions which match itemname and have a compatible matchtype across all files.. used for key presses/voice input to compile a list of condition data to check for

        public List<Tuple<string, ConditionEntry.MatchType>> ReturnValuesOfSpecificConditions(string conditions, List<ConditionEntry.MatchType> matchtypes)
        {
            List<Tuple<string, ConditionEntry.MatchType>> ret = new List<Tuple<string, ConditionEntry.MatchType>>();
            foreach (ActionFile f in actionfiles)
            {
                if (f.enabled)
                {
                    List<Tuple<string, ConditionEntry.MatchType>> fr = f.actioneventlist.ReturnValuesOfSpecificConditions(conditions, matchtypes);
                    if (fr != null)
                        ret.AddRange(fr);
                }
            }

            return ret;
        }

        #endregion

    }
}
