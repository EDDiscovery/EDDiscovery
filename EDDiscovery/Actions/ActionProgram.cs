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
        protected string name;
        protected List<Action> programsteps;

        public ActionProgram(string n)
        {
            name = n;
            programsteps = new List<Action>();
        }

        public ActionProgram(string n , List<Action> steps)
        {
            name = n;
            programsteps = steps;
        }

        public string Name { get { return name; } }
        public int Count { get { return programsteps.Count; } }

        public void Add(Action ap)
        {
            programsteps.Add(ap);
        }

        public void Clear()
        {
            programsteps.Clear();
        }

        public Action GetStep(int a)
        {
            if (a < programsteps.Count)
                return programsteps[a];
            else
                return null;
        }

        #region JSON in and out

        public JObject ToJSON()
        {
            JArray jf = new JArray();

            foreach( Action ac in programsteps )
            {
                JObject step = new JObject();
                step["StepName"] = ac.Name;
                step["StepUC"] = ac.UserData;
                step["StepLevelUp"] = ac.LevelUp;
                step["StepWhitespace"] = ac.Whitespace;

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
                string stepUC = (string)js["StepUC"];
                int stepLU = (int)js["StepLevelUp"];
                int whitespace = JSONHelper.GetInt(js["StepWhitespace"],0);     // was not in earlier version, optional

                Action cmd = Action.CreateAction(stepname, stepUC, stepLU, whitespace);

                if ( cmd != null && cmd.VerifyActionCorrect() == null )                  // throw away ones with bad names
                    ap.programsteps.Add(cmd);
            }

            return ap;
        }

        static public ActionProgram FromFile(string file, string progname, out string err)
        {
            try
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(file))
                {
                    return FromTextReader(sr, progname, out err);
                }
            }
            catch
            {
                err = "File IO failure";
                return null;
            }
        }

        static public ActionProgram FromString(string text, string progname, out string err)
        {
            using (System.IO.TextReader sr = new System.IO.StringReader(text))
            {
                return FromTextReader(sr, progname, out err);
            }
        }

        static public ActionProgram FromTextReader(System.IO.TextReader sr, string progname, out string err)
        {
            err = "";
            List<Action> prog = new List<Action>();
            List<int> indents = new List<int>();
            List<int> level = new List<int>();
            int indentpos = -1;
            int structlevel = 0;

            string completeline;
            int lineno = 1;

            while (( completeline = sr.ReadLine() )!=null)
            {
                completeline = completeline.Replace("\t", "    ");  // detab, to spaces, tabs are worth 4.

                StringParser p = new StringParser(completeline);

                if (!p.IsEOL)
                {
                    int curindent = p.Position;
                    string cmd = p.NextWord();      // space separ
                    string line = p.LineLeft;       // and the rest of the line..

                    if (cmd.Equals("Name", StringComparison.InvariantCultureIgnoreCase))
                        progname = line;
                    else
                    {
                        Action a = Action.CreateAction(cmd, line, 0);
                        string vmsg;

                        if (a == null)
                            err += "Line " + lineno + " Unrecognised command " + cmd + Environment.NewLine;
                        else if ((vmsg = a.VerifyActionCorrect()) != null)
                            err += "Line " + lineno + " " + vmsg + Environment.NewLine + " " + completeline + Environment.NewLine;
                        else
                        {
                            if (indentpos == -1)
                                indentpos = curindent;
                            else if (curindent > indentpos)        // more indented, up one structure
                            {
                                structlevel++;
                            }
                            else if (curindent < indentpos)   // deindented
                            {
                                int tolevel = -1;
                                for (int i = indents.Count - 1; i >= 0; i--)            // search up and find the entry with the indent..
                                {
                                    if (indents[i] <= curindent)    
                                    {
                                        tolevel = i;
                                        break;
                                    }
                                }

                                int cl = structlevel;

                                if (tolevel != -1)                  // if found
                                {
                                    structlevel = level[tolevel];   // if found, we are at that.. except..

                                    if ((a.Type == Action.ActionType.While && prog[tolevel].Type == Action.ActionType.Do) ||
                                        ((a.Type == Action.ActionType.Else || a.Type == Action.ActionType.ElseIf) && prog[tolevel].Type == Action.ActionType.If))
                                    {
                                        structlevel++;      // thest are artifically indented (else/elseif after if, and while after do, these maintain level)
                                    }
                                }
                                else
                                    structlevel--;  // else we are just down 1.

                                a.LevelUp = cl - structlevel;       // how much to go up..
                            }

//                            System.Diagnostics.Debug.WriteLine(structlevel + ": Cmd " + cmd + " : " + line);
                            prog.Add(a);

                            indentpos = curindent;
                            indents.Add(indentpos);
                            level.Add(structlevel);
                        }
                    }
                }
                else
                {
                    if (prog.Count > 0)
                        prog[prog.Count - 1].Whitespace = 1;
                }

                lineno++;
            }

            if (prog.Count == 0)
                err += "No valid statements" + Environment.NewLine;

            return (err.Length == 0) ? new ActionProgram(progname, prog) : null;
        }

        #endregion
    }


    // holder of programs

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

        public bool FromJSON(string s)
        {
            try
            {
                JObject jo = (JObject)JObject.Parse(s);
                return FromJSONObject(jo);
            }
            catch
            {
                return false;
            }
        }

        public bool FromJSONObject(JObject jo)
        {
            try
            {
                Clear();

                JArray jf = (JArray)jo["ProgramSet"];

                foreach (JObject j in jf)
                {
                    ActionProgram ap = ActionProgram.FromJSON(j);
                    programs.Add(ap);
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Dump:" + ex.StackTrace);
            }

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
