using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Actions
{
    public class ActionProgram
    {
        // name and program stored in memory.
        private string name;
        private List<Action> programsteps;

        // used during execution.. filled in on program objects associated with an execution
        public EDDiscoveryForm discoveryform;
        public HistoryEntry historyentry;
        public JSONHelper.JSONFields vars;
        public bool allowpause;

        private enum IfState { On, Off, OffForGood }
        private IfState[] ifstate = new IfState[50];
        private int iflevel = 0;

        private string errlist = "";

        public ActionProgram(string n)
        {
            name = n;
            programsteps = new List<Action>();
        }

        public ActionProgram( ActionProgram r, EDDiscoveryForm ed = null, HistoryEntry h = null , JSONHelper.JSONFields v = null , bool allowp = false )        // make a copy of the program..
        {
            name = r.name;
            discoveryform = ed;
            historyentry = h;
            vars = v;
            allowpause = allowp;
            iflevel = 0;
            ifstate[iflevel] = IfState.On;

            programsteps = new List<Action>();
            foreach ( Action ap in r.programsteps )
            {
                programsteps.Add(Action.CreateCopy(ap));
            }
        }

        public string Name { get { return name; } }

        static public string flagRunAtRefresh = "RunAtRefresh;";            // ACTION DATA Flags, stored with action program name in events to configure it

        public void Add(Action ap)
        {
            programsteps.Add(ap);
        }

        public void Clear()
        {
            programsteps.Clear();
        }

        public int Count { get { return programsteps.Count; } }

        public Action GetStep(int a )
        {
            if (a < programsteps.Count)
                return programsteps[a];
            else
                return null;
        }

        #region Conditional control

        public bool DoExecute(Action ac)      // execute if control state
        {
            return ifstate[iflevel] == IfState.On || ac.Type == Action.ActionType.If || ac.Type == Action.ActionType.ElseIf;
        }

        public bool IsLevelOff { get { return ifstate[iflevel] == IfState.Off; } }

        public void PushIf(bool execstate)
        {
            iflevel++;
            ifstate[iflevel] = execstate ? IfState.On : IfState.Off;
        }

        public void ElseIf(bool execstate)
        {
            ifstate[iflevel] = execstate ? IfState.On : IfState.Off;
        }

        public void ElseIfOff()
        {
            ifstate[iflevel] = IfState.OffForGood;
        }

        public void LevelUp(int n)
        {
            iflevel = Math.Max(iflevel - n, 0);
        }

        public int IfLevel { get { return iflevel;  } }

        #endregion

        #region Run time errors
        public void ReportError(string s)
        {
            if ( errlist.Length>0)
                errlist += Environment.NewLine;
            errlist += s;
        }

        public string GetErrorList { get { return errlist; } }

        #endregion

        #region JSON in and out

        public JObject GetJSON()
        {
            JArray jf = new JArray();

            foreach( Action ac in programsteps )
            {
                JObject step = new JObject();
                step["StepName"] = ac.Name;

                JArray flags = new JArray();
                foreach (string s in ac.Flags)
                    flags.Add(s);

                step["StepAC"] = flags;
                step["StepUC"] = ac.UserData;
                step["StepLevelUp"] = ac.LevelUp;

                jf.Add(step);
            }

            JObject prog = new JObject();
            prog["Name"] = Name;
            prog["Steps"] = jf;

            return prog;
        }

        static public ActionProgram FromJSON(JObject j )
        {
            ActionProgram ap = new ActionProgram((string)j["Name"]);

            JArray steps = (JArray)j["Steps"];

            foreach (JObject js in steps)
            {
                string stepname = (string)js["StepName"];

                List<string> stepAC = new List<string>();

                JArray flags = (JArray)js["StepAC"];

                foreach (JToken jf in flags)
                {
                    stepAC.Add((string)jf);
                }

                string stepUC = (string)js["StepUC"];

                int stepLU = (int)js["StepLevelUp"];

                Action cmd = Action.CreateAction(stepname, stepAC, stepUC, stepLU);

                if ( cmd != null )                  // throw away ones with bad names
                    ap.programsteps.Add(cmd);
            }

            return ap;
        }

        #endregion
    }

    public class ActionProgramList
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

        public string[] GetActionProgramList()
        {
            string[] ret = new string[programs.Count];
            for (int i = 0; i < programs.Count; i++)
                ret[i] = programs[i].Name;

            return ret;
        }

        public void Clear()
        {
            programs = new List<ActionProgram>();
        }

        public string GetJSON()
        {
            JObject evt = new JObject();

            JArray jf = new JArray();

            foreach (ActionProgram ap in programs)
            {
                JObject j1 = ap.GetJSON();
                jf.Add(j1);
            }

            evt["ProgramSet"] = jf;

            return evt.ToString();
        }

        public bool FromJSON(string s)
        {
            Clear();

            try
            {
                JObject jo = (JObject)JObject.Parse(s);

                JArray jf = (JArray)jo["ProgramSet"];

                foreach (JObject j in jf)
                {
                    ActionProgram ap = ActionProgram.FromJSON(j);
                    programs.Add(ap);
                }

                return true;
            }
            catch { }

            return false;
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
