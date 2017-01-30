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

        public static ActionFile ReadFile( string filename )
        {
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

                    if (cond.FromJSON(jcond) && prog.FromJSONObject(jprog))
                    {
                        return new ActionFile(cond, prog, filename, Path.GetFileNameWithoutExtension(filename), en, ivars );
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Dump:" + ex.StackTrace);
                }

                return null;
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

    public class ActionFileList
    {
        private List<ActionFile> actionfiles = new List<ActionFile>();
        private int current = 0;

        public ActionFile CurFile { get { return actionfiles[current]; } }
        public ConditionLists CurConditions { get { return actionfiles[current].actionfieldfilter; } }
        public ActionProgramList CurPrograms { get { return actionfiles[current].actionprogramlist; } }
        public ConditionVariables CurInstallationVariables { get { return actionfiles[current].installationvariables; } }
        public string CurName { get { return actionfiles[current].name; } }
        public bool CurEnabled { get { return actionfiles[current].enabled; } }

        public List<string> GetList { get { return (from af in actionfiles select af.name).ToList(); } }

        public void UpdateCurrentCL(ConditionLists cl) { actionfiles[current].actionfieldfilter = cl; }
        public void UpdateCurrentInstallationVariables(ConditionVariables v) { actionfiles[current].installationvariables = v; }
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

        // any with refresh flag set?
        public bool IsConditionFlagSet(string flagstart)
        {
            foreach (ActionFile af in actionfiles)
            {
                if (af.actionfieldfilter.IsConditionFlagSet(flagstart))
                    return true;
            }
            return false;
        }

        // get actions in system matching eventname
        public List<MatchingSets> GetMatchingConditions(string eventname , string flagstart = null )        // flag start is compared with start of actiondata
        {
            List<MatchingSets> apl = new List<MatchingSets>();

            foreach (ActionFile af in actionfiles)
            {
                if (af.enabled)         // only enabled files are checked
                {
                    List<ConditionLists.Condition> events = af.actionfieldfilter.GetConditionListByEventName(eventname , flagstart);

                    if (events != null)     // and if we have matching event..
                    {
                        apl.Add(new MatchingSets() { af = af, cl = events });
                    }
                }
            }

            return apl;
        }

        // if eventjson = null meaning not associated with an event.
        // work out which ones passed
        // if se is set passed enabled string expansion of arguments in condition of event..

        public int CheckActions(List<Actions.ActionFileList.MatchingSets> ale, string eventjson , ConditionVariables othervars,
                                            ConditionLists.ExpandString se = null)
        {
            ConditionVariables valuesneeded = new ConditionVariables();

            if ( eventjson != null )
            {
                foreach (MatchingSets ae in ale)       // for all files
                {
                    foreach (ConditionLists.Condition fe in ae.cl)        // find all values needed
                        fe.IndicateValuesNeeded(ref valuesneeded);
                }

                valuesneeded.GetJSONFieldValuesIndicated(eventjson);     // get the values needed for the conditions
            }

            valuesneeded.Add(othervars);

            int progs = 0;
            
            foreach (MatchingSets ae in ale)       // for all files
            {
                string errlist = null;
                ae.passed = new List<ConditionLists.Condition>();

                //System.Diagnostics.Debug.WriteLine("Check `" + ae.af.name + ae.af.actionfieldfilter.ToString() + "`");
                //ActionData.DumpVars(valuesneeded, " Test var:");

                ae.af.actionfieldfilter.CheckConditions(ae.cl, valuesneeded, out errlist, ae.passed, se);   // indicate which ones passed
                progs += ae.passed.Count;
            }

            return progs;
        }

        public void RunActions(List<Actions.ActionFileList.MatchingSets> ale, ActionRun run, ConditionVariables inputparas, HistoryList hle, HistoryEntry he )
        {
            foreach (Actions.ActionFileList.MatchingSets ae in ale)          // for every file which passed..
            {
                foreach (ConditionLists.Condition fe in ae.passed)          // and every condition..
                {
                    Tuple<ActionFile, ActionProgram> ap = FindProgram(fe.action, ae.af);          // find program using this name, prefer this action file first

                    if (ap != null)     // program got,
                    {
                        ConditionVariables adparas = new ConditionVariables();
                        string flags;
                        adparas.FromActionDataString(fe.actiondata, out flags);

                        inputparas.Add(adparas);

                        run.Add(ap.Item1, ap.Item2, inputparas, hle, he);
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
                ActionFile af = ActionFile.ReadFile(f.FullName);
                if (af != null)
                    actionfiles.Add(af);
            }

            if (actionfiles.Count == 0)           // need a default
                CreateSet("Default");

            current = actionfiles.Count - 1;        // always the latest.
        }

        public bool LoadFile(string filename)
        {
            ActionFile af = ActionFile.ReadFile(filename);
            if (af != null)
            {
                int indexof = actionfiles.FindIndex(x => x.name.Equals(af.name));

                if (indexof != -1)
                    actionfiles[indexof] = af;
                else
                    actionfiles.Add(af);

                return true;
            }
            else
                return false;
        }

        public void SaveCurrentActionFile()
        {
            actionfiles[current].SaveFile();
        }

        #region special helpers

        public List<Tuple<string, ConditionLists.MatchType>> ReturnValuesOfSpecificConditions(string conditions, List<ConditionLists.MatchType> matchtypes)
        {
            List<Tuple<string, ConditionLists.MatchType>> ret = new List<Tuple<string, ConditionLists.MatchType>>();
            foreach (ActionFile f in actionfiles)
            {
                if (f.enabled)
                {
                    List<Tuple<string, ConditionLists.MatchType>> fr = f.actionfieldfilter.ReturnValuesOfSpecificConditions(conditions, matchtypes);
                    if (fr != null)
                        ret.AddRange(fr);
                }
            }

            return ret;
        }

        #endregion

            #region Dialog helps

        public bool ImportDialog()
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.DefaultExt = "act";
            dlg.AddExtension = true;
            dlg.Filter = "Action Files (*.act)|*.act|All files (*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string importdir = System.IO.Path.Combine(Tools.GetAppDataDirectory(), "Actions");
                if (!System.IO.Directory.Exists(importdir))
                    System.IO.Directory.CreateDirectory(importdir);

                ActionFile af = ActionFile.ReadFile(dlg.FileName);

                if (af != null)
                {
                    if (GetList.Contains(af.name))
                    {
                        string acceptstr = "Already have an action file called " + af.name + Environment.NewLine + "Click Cancel to abort, OK to overwrite";

                        DialogResult dr = MessageBox.Show(acceptstr, "Duplicate File Warning", MessageBoxButtons.OKCancel);

                        if (dr == DialogResult.Cancel)
                            return false;
                    }

                    string destfile = System.IO.Path.Combine(importdir, af.name + ".act");

                    try
                    {
                        System.IO.File.Copy(dlg.FileName, destfile, true);

                        if (LoadFile(destfile))
                        {
                            MessageBox.Show("Action file " + af.name + " loaded.  Note if action file relies on start up events, you will need to quit and rerun EDDiscovery to make the file work correctly");
                            return true;
                        }
                        else
                            MessageBox.Show("Failed to load in");
                    }
                    catch
                    {
                        MessageBox.Show("File IO error copying file " + dlg.FileName + " to " + destfile + " check permissions");
                    }
                }
                else
                {
                    MessageBox.Show("Action file does not read - check file " + dlg.FileName);
                }
            }

            return false;
        }

        #endregion
    }
}
