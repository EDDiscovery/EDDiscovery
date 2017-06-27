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
    public class ActionProgram              // HOLDS the program, can write it JSON or Text FILES
    {
        protected List<Action> programsteps = new List<Action>();
        public string Name { get; set; } = "";
        public string StoredInSubFile { get; set; } = null;       // if null, then its stored in its master ActionFile, else its stored in a subfile

        public int Count { get { return programsteps.Count; } }

        public ActionProgram(string n = null)
        {
            if (n != null)
                Name = n;
        }

        public void Set( ActionProgram other)
        {
            Name = other.Name;
            programsteps = other.programsteps;
        }

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

        #region Read

        public string Read(JObject j)       // empty string if ok
        {
            string errlist = "";

            string progname = (string)j["Name"];
            JArray steps = (JArray)j["Steps"];

            if (steps != null)
            {
                Name = progname;
                programsteps = new List<Action>();

                int lineno = 1;

                foreach (JObject js in steps)
                {
                    string stepname = (string)js["StepName"];
                    string stepUC = (string)js["StepUC"];
                    int stepLU = JSONHelper.GetInt(js["StepLevelUp"], 0);                // optional
                    int whitespace = JSONHelper.GetInt(js["StepWhitespace"], 0);        // was not in earlier version, optional
                    string comment = JSONHelper.GetStringDef(js["StepComment"], "");    // was not in earlier version, optional

                    Action cmd = Action.CreateAction(stepname, stepUC, comment, stepLU, whitespace);

                    if (cmd != null && cmd.VerifyActionCorrect() == null)                  // throw away ones with bad names
                    {
                        cmd.LineNumber = lineno++;
                        lineno += cmd.Whitespace;
                        programsteps.Add(cmd);
                    }
                    else
                        errlist += "Failed to create " + progname + " step: " + stepname + ":" + stepUC + Environment.NewLine;
                }
            }

            return errlist;
        }

        public string Read(System.IO.TextReader sr, ref int lineno)         // read from stream
        {
            string err = "";

            programsteps = new List<Action>();
            Name = "";

            List<int> indents = new List<int>();
            List<int> level = new List<int>();
            int indentpos = -1;
            int structlevel = 0;

            string completeline;

            int initiallineno = lineno;

            while (( completeline = sr.ReadLine() )!=null)
            {
                lineno++;

                completeline = completeline.Replace("\t", "    ");  // detab, to spaces, tabs are worth 4.
                StringParser p = new StringParser(completeline);

                if (!p.IsEOL)
                {
                    int curindent = p.Position;
                    string cmd = "";
                    if (p.IsStringMoveOn("//"))         // special, this is allowed to butt against text and still work
                        cmd = "//";
                    else
                        cmd = p.NextWord();

                    if (cmd.Equals("Else", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //System.Diagnostics.Debug.WriteLine("Else " + cmd + " " + p.LineLeft);
                        if (p.IsStringMoveOn("If", StringComparison.InvariantCultureIgnoreCase))   // if Else followed by IF 
                            cmd = "Else If";
                    }

                    string line = p.LineLeft;           // and the rest of the line..

                    int commentpos = line.LastIndexOf("//");
                    string comment = "";

                    if (cmd != "//" && commentpos >= 0 && !line.InQuotes(commentpos))       // if not // command, and we have one..
                    {
                        comment = line.Substring(commentpos + 2).Trim();
                        line = line.Substring(0, commentpos).TrimEnd();
                        //System.Diagnostics.Debug.WriteLine("Line <" + line + "> <" + comment + ">");
                    }

                    if (cmd.Equals("PROGRAM", StringComparison.InvariantCultureIgnoreCase))
                        Name = line;
                    else if (cmd.Equals("END", StringComparison.InvariantCultureIgnoreCase) && line.Equals("PROGRAM", StringComparison.InvariantCultureIgnoreCase))
                        break;
                    else
                    {
                        Action a = Action.CreateAction(cmd, line, comment);
                        string vmsg;

                        if (a == null)
                            err += lineno + " " + Name + " Unrecognised command " + cmd + Environment.NewLine;
                        else if ((vmsg = a.VerifyActionCorrect()) != null)
                            err += lineno + " " + Name + ":" + vmsg + Environment.NewLine + " " + completeline.Trim() + Environment.NewLine;
                        else
                        {
                            //System.Diagnostics.Debug.WriteLine(indentpos + ":" + structlevel + ": Cmd " + cmd + " : " + line);

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
                                    if ((a.Type == Action.ActionType.While && programsteps[tolevel].Type == Action.ActionType.Do) ||
                                        ((a.Type == Action.ActionType.Else || a.Type == Action.ActionType.ElseIf) && programsteps[tolevel].Type == Action.ActionType.If))
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

                            a.LineNumber = lineno;

                            //System.Diagnostics.Debug.WriteLine("    >>>> " + indentpos + ":" + structlevel);

                            indents.Add(indentpos);
                            level.Add(structlevel);
                            programsteps.Add(a);
                        }
                    }
                }
                else
                {
                    if (programsteps.Count > 0)
                        programsteps[programsteps.Count - 1].Whitespace = 1;
                }
            }

            if (programsteps.Count > 0 )
                programsteps[programsteps.Count - 1].Whitespace = 0;        // last cannot have whitespace..

            return err;
        }

        public string ReadFile(string file)               // Read from File the program
        {
            try
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(file))
                {
                    int lineno = 0;
                    return Read(sr, ref lineno);
                }
            }
            catch
            {
                return "File " + file + " missing or IO failure";
            }
        }


        #endregion

        #region Write

        public JObject WriteJSONObject()                         // write to JSON the program..
        {
            JObject prog = new JObject();
            prog["Name"] = Name;

            JArray jf = new JArray();

            foreach (Action ac in programsteps)
            {
                JObject step = new JObject();
                step["StepName"] = ac.Name;
                step["StepUC"] = ac.UserData;
                if (ac.LevelUp != 0)                      // reduces file size
                    step["StepLevelUp"] = ac.LevelUp;
                if (ac.Whitespace != 0)
                    step["StepWhitespace"] = ac.Whitespace;
                if (ac.Comment.Length > 0)
                    step["StepComment"] = ac.Comment;

                jf.Add(step);
            }

            prog["Steps"] = jf;

            return prog;
        }

        public override string ToString()
        {
            CalculateLevels();

            StringBuilder sb = new StringBuilder(256);

            sb.AppendLine("PROGRAM " + Name);
            sb.AppendLine("");

            foreach (Action act in programsteps)
            {
                if (act != null)    // don't include ones not set..
                {
                    string output = new String(' ', act.calcDisplayLevel * 4) + act.Name + " " + act.UserData;

                    if (act.Comment.Length > 0)
                        output += new string(' ', output.Length < 64 ? (64 - output.Length) : 4) + "// " + act.Comment;

                    sb.AppendLine(output);
                    if (act.Whitespace > 0)
                        sb.AppendLine("");
                }
            }

            sb.AppendLine("");
            sb.AppendLine("END PROGRAM");

            return sb.ToNullSafeString();
        }

        public void Write(System.IO.TextWriter sr)
        {
            sr.Write(ToString());
        }

        public bool WriteFile(string file)                       // write to file the program
        {
            try
            {
                using (System.IO.StreamWriter sr = new System.IO.StreamWriter(file))
                {
                    sr.Write(ToString());
                    return true;
                }
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

            int lineno = 1;

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

                    act.LineNumber = lineno;
                    lineno += act.Whitespace;
                }
                else
                {
                    errlist += "Step " + step.ToString(ct) + " not defined" + Environment.NewLine;
                }

                step++;
                lineno++;
            }

            if ( structlevel > 0 && structcount[structlevel] == 0 )
            {
                errlist += "At End of program, no statements present after " + structtype[structlevel].ToString() + " statement" + Environment.NewLine;
            }

            return errlist;
        }

        #endregion

        #region Editor

        public bool EditInEditor(string file = null )          // edit in editor, swap to this
        {
            try
            {
                if ( file == null)                              // if not associated directly with a file, save to a temp one
                {
                    string filename = Name.Length > 0 ? Name : "Default";
                    file = System.IO.Path.Combine(System.IO.Path.GetTempPath(), filename.SafeFileString() + ".atf");

                    if (!WriteFile(file))
                        return false;
                }

                while (true)
                {
                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = Tools.AssocQueryString(Tools.AssocStr.Executable, ".txt");
                    p.StartInfo.Arguments = file.QuoteString();
                    p.Start();
                    p.WaitForExit();

                    ActionProgram apin = new ActionProgram();
                    string err = apin.ReadFile(file);
                    if (err.Length>0)
                    {
                        DialogResult dr = Forms.MessageBoxTheme.Show("Editing produced the following errors" + Environment.NewLine + Environment.NewLine + err + Environment.NewLine +
                                            "Click Retry to correct errors, Cancel to abort editing",
                                            "Warning", MessageBoxButtons.RetryCancel);

                        if (dr == DialogResult.Cancel)
                            return false;
                    }
                    else
                    {
                        Set(apin);
                        return true;
                    }
                }
            }
            catch { }

            Forms.MessageBoxTheme.Show("Unable to run text editor - check association for .txt files");
            return false;
        }

        #endregion
    }
}
