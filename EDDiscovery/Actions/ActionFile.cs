using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// A file holds a set of conditions and programs associated with them

namespace EDDiscovery.Actions
{
    public class ActionFile
    {
        public ActionFile(ConditionLists c, ActionProgramList p , string f , string n, bool e )
        {
            actionfieldfilter = c;
            actionprogramlist = p;
            filepath = f;
            name = n;
            enabled = e;
        }

        public ConditionLists actionfieldfilter;
        public ActionProgramList actionprogramlist;
        public string filepath;
        public string name;
        public bool enabled;
    }

    public class ActionFileList
    {
        private List<ActionFile> actionfiles = new List<ActionFile>();
        private int current = 0;

        public ConditionLists CurConditions { get { return actionfiles[current].actionfieldfilter; } }
        public ActionProgramList CurPrograms { get { return actionfiles[current].actionprogramlist; } }
        public string CurName { get { return actionfiles[current].name; } }
        public bool CurEnabled { get { return actionfiles[current].enabled; } }

        public List<string> GetList { get { return (from af in actionfiles select af.name).ToList(); } }

        public void UpdateCurrentCL(ConditionLists cl) { actionfiles[current].actionfieldfilter = cl; }
        public void UpdateCurrentEnabled(bool v) { actionfiles[current].enabled = v; }

        public bool SelectCurrent( string s )
        {
            int indexof = actionfiles.FindIndex(x => x.name.Equals(s));
            if (indexof >= 0)
                current = indexof;
            return (indexof >= 0);
        }

        public void CreateSet(string s)
        {
            string appfolder = Path.Combine(Tools.GetAppDataDirectory(), "Actions");
            ActionFile af = new ActionFile(new ConditionLists(), new ActionProgramList(), appfolder + "\\\\" + s + ".act", s, true);
            actionfiles.Add(af);
        }

        public class MatchingSets
        {
            public ActionFile af;                           // file it came from
            public List<ConditionLists.Condition> cl;       // list of matching events..
            public List<ConditionLists.Condition> passed;   // list of passed events after condition checked.
        }

        // get actions in system matching eventname
        // if eventjson = null meaning not associated with an event.
        // se if passed enabled string expansion of arguments in condition of event..
        public List<MatchingSets> GetActions(string eventname, string eventjson, ConditionVariables othervars, 
                                            ConditionLists.ExpandString se = null)
        {
            ConditionVariables valuesneeded = new ConditionVariables();

            List<MatchingSets> apl = new List<MatchingSets>();

            foreach (ActionFile af in actionfiles)
            {
                if (af.enabled)         // only enabled files are checked
                {
                    List<ConditionLists.Condition> events = af.actionfieldfilter.GetConditionListByEventName(eventname);

                    if (events != null)     // and if we have matching event..
                    {
                        if (eventjson!=null)
                        {
                            foreach (ConditionLists.Condition fe in events)        // find all values needed
                                fe.IndicateValuesNeeded(ref valuesneeded);
                        }

                        apl.Add(new MatchingSets() { af = af, cl = events });
                    }
                }
            }

            try
            {
                if (apl.Count > 0)          // we have matching events in files
                {
                    if (eventjson!=null)
                        valuesneeded.GetJSONFieldValuesIndicated(eventjson);     // get the values needed for the conditions

                    valuesneeded.Add(othervars);

                    foreach ( MatchingSets ae in apl )       // for all files
                    {
                        string errlist = null;
                        ae.passed = new List<ConditionLists.Condition>();

                        System.Diagnostics.Debug.WriteLine("Check `" + ae.af.name + ae.af.actionfieldfilter.ToString() + "`");
                        //ActionData.DumpVars(valuesneeded, " Test var:");

                        ae.af.actionfieldfilter.CheckConditions(ae.cl, valuesneeded, out errlist, ae.passed, se);   // indicate which ones passed
                    }

                    return apl;
                }
            }
            catch ( Exception )
            {
            }

            return null;
        }

        public void RunActions(List<Actions.ActionFileList.MatchingSets> ale, List<ConditionVariables> outervarsin, 
                                      ActionRun run, HistoryList hle, HistoryEntry he )
        {
            foreach (Actions.ActionFileList.MatchingSets ae in ale)          // for every file which passed..
            {
                foreach (ConditionLists.Condition fe in ae.passed)          // and every condition..
                {
                    Tuple<ActionFile, ActionProgram> ap = FindProgram(fe.action, ae.af);          // find program using this name, prefer this action file first

                    if (ap != null)     // program got,
                    {
                        ConditionVariables inputparas = new ConditionVariables();
                        string flags;
                        inputparas.FromActionDataString(fe.actiondata, out flags);

                        inputparas.Add(outervarsin);

                        run.Add(ap.Item1, ap.Item2, hle, he, inputparas);
                    }
                }
            }
        }

        public Tuple<ActionFile, ActionProgram> FindProgram(string req, ActionFile preferred = null)        // find a program 
        {
            Actions.ActionProgram ap = null;

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

        public void LoadAllActionFiles()
        {
            string appfolder = Path.Combine(Tools.GetAppDataDirectory(), "Actions");

            if (!Directory.Exists(appfolder))
                Directory.CreateDirectory(appfolder);

            FileInfo[] allFiles = Directory.EnumerateFiles(appfolder, "*.act", SearchOption.AllDirectories).Select(f => new FileInfo(f)).OrderBy(p => p.LastWriteTime).ToArray();

            foreach (FileInfo f in allFiles)
            {
                using (StreamReader sr = new StreamReader(f.FullName))         // read directly from file..
                {
                    string json = sr.ReadToEnd();
                    sr.Close();

                    try
                    {
                        JObject jo = (JObject)JObject.Parse(json);

                        JObject jcond = (JObject)jo["Conditions"];
                        JObject jprog = (JObject)jo["Programs"];
                        bool en = (bool)jo["Enabled"];

                        ConditionLists cond = new ConditionLists();
                        ActionProgramList prog = new ActionProgramList();
                        if (cond.FromJSON(jcond) && prog.FromJSONObject(jprog))
                        {
                            ActionFile af = new ActionFile(cond, prog, f.FullName, Path.GetFileNameWithoutExtension(f.FullName), en);
                            actionfiles.Add(af);
                        }
                    }
                    catch { }   // ignore the file.. garbage.
                }
            }

            if (actionfiles.Count == 0)           // need a default
                CreateSet("Default");

            current = actionfiles.Count - 1;        // always the latest.
        }


        public void SaveCurrentActionFile()
        {
            ActionFile af = actionfiles[current];

            JObject jcond = af.actionfieldfilter.GetJSONObject();
            JObject jprog = af.actionprogramlist.ToJSONObject();

            JObject jo = new JObject();
            jo["Conditions"] = jcond;
            jo["Programs"] = jprog;
            jo["Enabled"] = af.enabled;

            string json = jo.ToString(Formatting.Indented);

            using (StreamWriter sr = new StreamWriter(af.filepath))      
            {
                sr.Write(json);
                sr.Close();
            }
        }
    }
}
