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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class ActionProgram
    {
        // name and program stored in memory.
        protected List<Action> programsteps;

        public ActionProgram(string n = "")
        {
            Name = n;
            programsteps = new List<Action>();
        }

        public ActionProgram(string n , List<Action> steps)
        {
            Name = n;
            programsteps = steps;
        }

        public string Name { get; set; }
        public int Count { get { return programsteps.Count; } }

        public void Add(Action ap)
        {
            programsteps.Add(ap);
        }

        public void Insert(int pos, Action ap)
        {
            programsteps.Insert(pos,ap);
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

        public void SetStep(int step, Action act)
        {
            while (step >= programsteps.Count)
                programsteps.Add(null);

            programsteps[step] = act;
        }

        public void MoveUp(int step)
        {
            Action act = programsteps[step];
            programsteps.RemoveAt(step);
            programsteps.Insert(step - 1, act);
        }

        public void Delete(int step)
        {
            if (step < programsteps.Count)
                programsteps.RemoveAt(step);
        }


        #region to/from JSON

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

        #endregion

        #region to/from File

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
                            System.Diagnostics.Debug.WriteLine(indentpos + ":" + structlevel + ": Cmd " + cmd + " : " + line);

                            if (indentpos == -1)
                                indentpos = curindent;
                            else if (curindent > indentpos)        // more indented, up one structure
                            {
                                structlevel++;
                                indentpos = curindent;
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

                                if (tolevel >= 0)       // if found, we have a statement to hook to..
                                {
                                    if ((a.Type == Action.ActionType.While && prog[tolevel].Type == Action.ActionType.Do) ||
                                        ((a.Type == Action.ActionType.Else || a.Type == Action.ActionType.ElseIf) && prog[tolevel].Type == Action.ActionType.If))
                                    {
                                        int reallevel = level[tolevel] + 1;     // else, while are dedented, but they really are on level+1
                                        a.LevelUp = structlevel - reallevel;
                                        structlevel = reallevel;
                                        indentpos = indents[tolevel] + 4;       // and our indent should continue 4 in, so we don't match against this when we do indent
                                    }
                                    else
                                    { 
                                        a.LevelUp = structlevel - level[tolevel];
                                        structlevel = level[tolevel];   // if found, we are at that.. except..
                                        indentpos = indents[tolevel]; // and back to this level
                                    }
                                }
                            }

                            System.Diagnostics.Debug.WriteLine("    >>>> " + indentpos + ":" + structlevel);

                            indents.Add(indentpos);
                            level.Add(structlevel);
                            prog.Add(a);
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

        public bool SaveText(string file)
        {
            CalculateLevels();

            try
            {
                using (System.IO.StreamWriter sr = new System.IO.StreamWriter(file))
                {
                    sr.WriteLine("NAME " + Name + Environment.NewLine);

                    foreach (Action act in programsteps)
                    {
                        if (act != null)    // don't include ones not set..
                        {
                            sr.Write(new String(' ', act.calcDisplayLevel * 4));
                            sr.WriteLine(act.Name + " " + act.UserData);
                            if (act.Whitespace > 0)
                                sr.WriteLine("");
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Calculate Level data

        public string CalculateLevels()         // go thru the program, and calc some values for editing purposes. Also look for errors
        {
            string errlist = "";

            int structlevel = 0;
            int[] structcount = new int[50];
            Action.ActionType[] structtype = new Action.ActionType[50];

            System.Globalization.CultureInfo ct = System.Globalization.CultureInfo.InvariantCulture;
            int step = 1;
            bool lastwaswhileafterdo = false;

            foreach (Action act in programsteps)
            {
                if (act != null)
                {
                    act.calcAllowRight = act.calcAllowLeft = false;
                    bool indo = structtype[structlevel] == Action.ActionType.Do;        // if in a DO..WHILE, and we are a WHILE, we don't indent.

                    if (indo && lastwaswhileafterdo && act.LevelUp == 0)        // force back down if after do/while
                        act.LevelUp = 1;

                    if (act.LevelUp > 0)            // if backing up
                    {
                        if ( structcount[structlevel] == 0 )        // if we had nothing on this level, its wrong
                            errlist += "Step " + step.ToString(ct) + " no statements at indented level after " + structtype[structlevel].ToString() + " statement" + Environment.NewLine;

                        if (act.LevelUp > structlevel)            // ensure its not too big.. this may happen due to copying
                            act.LevelUp = structlevel;

                        structlevel -= act.LevelUp;                 // back up
                        act.calcAllowRight = act.LevelUp>1 || !lastwaswhileafterdo;      // normally we can go right, but can't if its directly after do..while and only 1 level of detent
                    }

                    lastwaswhileafterdo = false;        // records if last entry was while after do

                    structcount[structlevel]++;         // 1 more on this level

                    if (structlevel > 0 && structcount[structlevel] > 1)        // second further on can be moved back..
                        act.calcAllowLeft = true;

                    act.calcStructLevel = act.calcDisplayLevel = structlevel;
                    
                    if (act.Type == Action.ActionType.ElseIf)
                    {
                        if (structtype[structlevel] == Action.ActionType.Else)
                            errlist += "Step " + step.ToString(ct) + " ElseIf after Else found" + Environment.NewLine;
                        else if (structtype[structlevel] != Action.ActionType.If && structtype[structlevel] != Action.ActionType.ElseIf)
                            errlist += "Step " + step.ToString(ct) + " ElseIf without IF found" + Environment.NewLine;
                    }
                    else if (act.Type == Action.ActionType.Else)
                    {
                        if (structtype[structlevel] == Action.ActionType.Else)
                            errlist += "Step " + step.ToString(ct) + " Else after Else found" + Environment.NewLine;
                        else if (structtype[structlevel] != Action.ActionType.If && structtype[structlevel] != Action.ActionType.ElseIf)
                            errlist += "Step " + step.ToString(ct) + " Else without IF found" + Environment.NewLine;
                    }

                    if (act.Type == Action.ActionType.ElseIf || act.Type == Action.ActionType.Else)
                    {
                        structtype[structlevel] = act.Type;

                        if (structlevel == 1)
                            act.calcAllowLeft = false;         // can't move an ELSE back to level 0

                        if (structlevel > 0)      // display else artifically indented.. display only
                            act.calcDisplayLevel--;

                        structcount[structlevel] = 0;   // restart count so we don't allow a left on next one..
                    }
                    else if ( act.Type == Action.ActionType.While && indo )     // do..while
                    {
                        if (structlevel > 0)      // display else artifically indented.. display only
                            act.calcDisplayLevel--;

                        lastwaswhileafterdo = true;     // be careful backing up
                    }
                    else if (act.Type == Action.ActionType.If || (act.Type == Action.ActionType.While && !indo) ||
                                act.Type == Action.ActionType.Do || act.Type == Action.ActionType.Loop)
                    {
                        structlevel++;
                        structcount[structlevel] = 0;
                        structtype[structlevel] = act.Type;
                    }

                    string vmsg = act.VerifyActionCorrect();
                    if (vmsg != null)
                        errlist += "Step " + step.ToString(ct) + " " + vmsg + Environment.NewLine;

                }
                else
                {
                    errlist += "Step " + step.ToString(ct) + " not defined" + Environment.NewLine;
                }

                step++;
            }

            if ( structlevel > 0 && structcount[structlevel] == 0 )
            {
                errlist += "At End of program, no statements present after " + structtype[structlevel].ToString() + " statement" + Environment.NewLine;
            }

            return errlist;
        }

        #endregion

        #region Editor

        public bool EditInEditor()          // edit in editor, swap to this
        {
            try
            {
                string prog = Tools.AssocQueryString(Tools.AssocStr.Executable, ".txt");

                string filename = Name.Length > 0 ? Name : "Default";

                string editingloc = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Tools.SafeFileString(filename) + ".atf");

                if (SaveText(editingloc))
                {
                    while (true)
                    {
                        System.Diagnostics.Process p = new System.Diagnostics.Process();
                        p.StartInfo.FileName = prog;
                        p.StartInfo.Arguments = editingloc.QuoteString();
                        p.Start();
                        p.WaitForExit();

                        string err;
                        ActionProgram apin = ActionProgram.FromFile(editingloc, filename, out err);
                        if (apin == null)
                        {
                            DialogResult dr = MessageBox.Show("Editing produced the following errors" + Environment.NewLine + Environment.NewLine + err + Environment.NewLine +
                                                "Click Retry to correct errors, Cancel to abort editing",
                                                "Warning", MessageBoxButtons.RetryCancel);

                            if (dr == DialogResult.Cancel)
                                return false;
                        }
                        else
                        {
                            programsteps = apin.programsteps;
                            return true;
                        }
                    }
                }
            }
            catch { }

            MessageBox.Show("Unable to run text editor - check association for .txt files");
            return false;
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
