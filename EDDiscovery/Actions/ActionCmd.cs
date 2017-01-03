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
        public string Name;
        public List<Action> programsteps;

        static public string flagRunAtRefresh = "RunAtRefresh;";            // ACTION DATA Flag

        public JObject GetJSON()
        {
            JArray jf = new JArray();

            foreach( Action ac in programsteps )
            {
                JObject step = new JObject();
                step["StepName"] = ac.ActionName;

                JArray flags = new JArray();
                foreach (string s in ac.ActionFlags)
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
            ActionProgram ap = new ActionProgram();
            ap.Name = (string)j["Name"];

            ap.programsteps = new List<Action>();

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

        public void Add(Action ap)
        {
            if ( programsteps == null )
                programsteps = new List<Action>();

            programsteps.Add(ap);
        }
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

    }
}
