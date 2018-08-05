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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseUtils;
using Conditions;

namespace ActionLanguage
{
    public class ActionMessageBox : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        List<string> FromString(string input)       // returns in raw esacped mode
        {
            StringParser sp = new StringParser(input);
            List<string> s = sp.NextQuotedWordList(replaceescape: true);
            return (s != null && s.Count >=1 && s.Count <= 4) ? s : null;
        }

        public override string VerifyActionCorrect()
        {
            return (FromString(userdata) != null) ? null : "MessageBox command line not in correct format";
        }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
        {
            List<string> l = FromString(userdata);
            List<string> r = ExtendedControls.PromptMultiLine.ShowDialog(parent, "Configure MessageBox Dialog", cp.Icon,
                            new string[] { "Message" , "Caption" , "Buttons", "Icon"}, l?.ToArray(), true);

            if (r != null)
                userdata = r.ToStringCommaList(1,true);     // and escape them back

            return (r != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            List<string> ctrl = FromString(UserData);

            if (ctrl != null)
            {
                List<string> exp;

                if (ap.functions.ExpandStrings(ctrl, out exp) != ConditionFunctions.ExpandResult.Failed)
                {
                    string caption = (exp.Count>=2) ? exp[1] : "EDDiscovery Program Message";

                    MessageBoxButtons but = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.None;
                
                    if (exp.Count >=3 && !Enum.TryParse<MessageBoxButtons>(exp[2], true, out but))
                    {
                        ap.ReportError("MessageBox button type not recognised");
                        return true;
                    }
                    if (exp.Count >= 4 && !Enum.TryParse<MessageBoxIcon>(exp[3], true, out icon))
                    {
                        ap.ReportError("MessageBox icon type not recognised");
                        return true;
                    }

                    DialogResult res = ExtendedControls.MessageBoxTheme.Show(ap.actioncontroller.Form, exp[0], caption, but, icon);

                    ap["DialogResult"] = res.ToString();
                }
                else
                    ap.ReportError(exp[0]);
            }
            else
                ap.ReportError("MessageBox command line not in correct format");

            return true;
        }
    }

    public class ActionInfoBox : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        List<string> FromString(string input)       // returns in raw esacped mode
        {
            StringParser sp = new StringParser(input);
            List<string> s = sp.NextQuotedWordList(replaceescape: true);
            return (s != null && s.Count == 2) ? s : null;
        }

        public override string VerifyActionCorrect()
        {
            return (FromString(userdata) != null) ? null : "InfoBox command line not in correct format";
        }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
        {
            List<string> l = FromString(userdata);
            List<string> r = ExtendedControls.PromptMultiLine.ShowDialog(parent, "Configure InfoBox Dialog", cp.Icon,
                            new string[] { "Message", "Caption" }, l?.ToArray(), true);

            if (r != null)
                userdata = r.ToStringCommaList(1, true);     // and escape them back

            return (r != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            List<string> ctrl = FromString(UserData);

            if (ctrl != null)
            {
                List<string> exp;

                if (ap.functions.ExpandStrings(ctrl, out exp) != ConditionFunctions.ExpandResult.Failed)
                {
                    string caption = (exp[1].Length>0) ? exp[1]: "EDDiscovery Program Message";

                    ExtendedControls.InfoForm ifrm = new ExtendedControls.InfoForm();
                    ifrm.Info(caption, ap.actioncontroller.Icon, exp[0]);
                    ifrm.Show(ap.actioncontroller.Form);
                }
                else
                    ap.ReportError(exp[0]);
            }
            else
                ap.ReportError("InfoBox command line not in correct format");

            return true;
        }
    }

    public class ActionFileDialog : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
        {
            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "Options", UserData, "Configure File Dialog", cp.Icon);
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string res;
            if (ap.functions.ExpandString(UserData, out res) != ConditionFunctions.ExpandResult.Failed)
            {
                StringParser sp = new StringParser(res);
                string cmdname = sp.NextWordLCInvariant(", ");

                if (cmdname.Equals("folder"))
                {
                    sp.IsCharMoveOn(',');

                    FolderBrowserDialog fbd = new FolderBrowserDialog();

                    string descr = sp.NextQuotedWordComma();
                    if (descr != null)
                        fbd.Description = descr;

                    string rootfolder = sp.NextQuotedWordComma();
                    if (rootfolder != null)
                    {
                        Environment.SpecialFolder sf;
                        if (Enum.TryParse<Environment.SpecialFolder>(rootfolder, out sf))
                            fbd.RootFolder = sf;
                        else
                            return ap.ReportError("FileDialog folder does not recognise folder location " + rootfolder);
                    }

                    string fileret = (fbd.ShowDialog(ap.actioncontroller.Form) == DialogResult.OK) ? fbd.SelectedPath : "";
                    ap["FolderName"] = fileret;
                }
                else if (cmdname.Equals("openfile"))
                {
                    sp.IsCharMoveOn(',');

                    OpenFileDialog fd = new OpenFileDialog();
                    fd.Multiselect = false;
                    fd.CheckPathExists = true;

                    try
                    {
                        string rootfolder = sp.NextQuotedWordComma();
                        if (rootfolder != null)
                            fd.InitialDirectory = rootfolder;

                        string filter = sp.NextQuotedWordComma();
                        if (filter != null)
                            fd.Filter = filter;

                        string defext = sp.NextQuotedWordComma();
                        if (defext != null)
                            fd.DefaultExt = defext;

                        string check = sp.NextQuotedWordComma();
                        if (check != null && check.Equals("On", StringComparison.InvariantCultureIgnoreCase))
                            fd.CheckFileExists = true;

                        string fileret = (fd.ShowDialog(ap.actioncontroller.Form) == DialogResult.OK) ? fd.FileName : "";
                        ap["FileName"] = fileret;
                    }
                    catch
                    {
                        ap.ReportError("FileDialog file failed to generate dialog, check options");
                    }
                }
                else if (cmdname.Equals("savefile"))
                {
                    sp.IsCharMoveOn(',');

                    SaveFileDialog fd = new SaveFileDialog();

                    try
                    {
                        string rootfolder = sp.NextQuotedWordComma();
                        if (rootfolder != null)
                            fd.InitialDirectory = rootfolder;

                        string filter = sp.NextQuotedWordComma();
                        if (filter != null)
                            fd.Filter = filter;

                        string defext = sp.NextQuotedWordComma();
                        if (defext != null)
                            fd.DefaultExt = defext;

                        string check = sp.NextQuotedWordComma();
                        if (check != null && check.Equals("On", StringComparison.InvariantCultureIgnoreCase))
                            fd.OverwritePrompt = true;

                        string fileret = (fd.ShowDialog(ap.actioncontroller.Form) == DialogResult.OK) ? fd.FileName : "";
                        ap["FileName"] = fileret;
                    }
                    catch
                    {
                        ap.ReportError("FileDialog file failed to generate dialog, check options");
                    }
                }
                else
                    ap.ReportError("FileDialog does not recognise command " + cmdname);
            }
            else
                ap.ReportError(res);

            return true;
        }
    }

    public class ActionInputBox : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        List<string> FromString(string input)
        {
            StringParser sp = new StringParser(input);
            List<string> s = sp.NextQuotedWordList();
            return (s != null && s.Count >= 2 && s.Count <= 5) ? s : null;
        }

        public override string VerifyActionCorrect()
        {
            return (FromString(userdata) != null) ? null : " command line not in correct format";
        }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
        {
            List<string> l = FromString(userdata);
            List<string> r = ExtendedControls.PromptMultiLine.ShowDialog(parent, "Configure InputBox Dialog", cp.Icon,
                            new string[] { "Caption", "Prompt List", "Default List", "Features", "ToolTips" }, l?.ToArray(),
                            false, new string[] { "Enter name of menu", "List of entries, semicolon separated", "Default list, semicolon separated", "Feature list: Multiline", "List of tool tips, semocolon separated" });
            if (r != null)
            {
                userdata = r.ToStringCommaList(2);
            }

            return (r != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            List<string> ctrl = FromString(UserData);

            if (ctrl != null)
            {
                List<string> exp;

                if (ap.functions.ExpandStrings(ctrl, out exp) != ConditionFunctions.ExpandResult.Failed)
                {
                    string[] prompts = exp[1].Split(';');
                    string[] def = (exp.Count >= 3) ? exp[2].Split(';') : null;
                    bool multiline = (exp.Count >= 4) ? (exp[3].IndexOf("Multiline", StringComparison.InvariantCultureIgnoreCase) >= 0) : false;
                    string[] tooltips = (exp.Count >= 5) ? exp[4].Split(';') : null;

                    List<string> r = ExtendedControls.PromptMultiLine.ShowDialog(ap.actioncontroller.Form, exp[0], ap.actioncontroller.Icon,
                                        prompts, def, multiline, tooltips);

                    ap["InputBoxOK"] = (r != null) ? "1" : "0";
                    if (r != null)
                    {
                        for (int i = 0; i < r.Count; i++)
                            ap["InputBox" + (i + 1).ToString()] = r[i];
                    }
                }
                else
                    ap.ReportError(exp[0]);
            }
            else
                ap.ReportError("MenuInput command line not in correct format");

            return true;
        }
    }

    public class ActionDialogBase : ActionBase
    {
        bool IsModalDialog() { return this.GetType().Equals(typeof(ActionDialog)); }

        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        List<string> FromString(string input)
        {
            StringParser sp = new StringParser(input);
            List<string> s = sp.NextQuotedWordList();
            return (s != null && s.Count == 4) ? s : null;
        }

        public override string VerifyActionCorrect()
        {
            return (FromString(userdata) != null) ? null : " command line not in correct format";
        }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
        {
            List<string> l = FromString(userdata);
            List<string> r = ExtendedControls.PromptMultiLine.ShowDialog(parent, "Configure Dialog", cp.Icon,
                            new string[] { "Logical Name" , "Caption", "Size [Pos]", "Var Prefix" }, l?.ToArray(),
                            false, new string[] { "Handle name of menu" , "Enter title of menu", "Size and optional Position, as w,h [,x,y] 200,300 or 200,300,500,100", "Variable Prefix" });
            if (r != null)
            {
                userdata = r.ToStringCommaList(2);
            }

            return (r != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            List<string> ctrl = FromString(UserData);

            if (ctrl != null)
            {
                List<string> exp;

                if (ap.functions.ExpandStrings(ctrl, out exp) != ConditionFunctions.ExpandResult.Failed)
                {
                    ConditionVariables cv = ap.variables.FilterVars(exp[3] + "*");

                    ExtendedControls.ConfigurableForm cd = new ExtendedControls.ConfigurableForm();

                    foreach ( string k in cv.NameList )
                    {
                        string errmsg = cd.Add(cv[k]);
                        if (errmsg != null)
                            return ap.ReportError(errmsg + " in " + k + " variable for Dialog");
                    }

                    StringParser sp2 = new StringParser(exp[2]);
                    int? dw = sp2.NextWordComma().InvariantParseIntNull();
                    int? dh = sp2.NextWordComma().InvariantParseIntNull();
                    int? x = sp2.NextWordComma().InvariantParseIntNull();
                    int? y = sp2.NextWord().InvariantParseIntNull();

                    if (dw != null && dh != null && ((x==null)==(y==null))) // need w/h, must have either no pos or both pos
                    {
                        if (IsModalDialog())
                            ap.dialogs[exp[0]] = cd;
                        else
                            ap.actionfile.dialogs[exp[0]] = cd;

                        System.Drawing.Point pos = new System.Drawing.Point(-999, -999);
                        if (x != null && y != null)
                            pos = new System.Drawing.Point(x.Value, y.Value);

                        cd.Trigger += Cd_Trigger;

                        cd.Show(ap.actioncontroller.Form, ap.actioncontroller.Icon,
                                            new System.Drawing.Size(dw.Value, dh.Value), pos , 
                                            exp[1],
                                            exp[0], new List<Object>() { ap, IsModalDialog() }  // logical name and tag
                                            );

                        return !IsModalDialog();       // modal, return false, STOP.  Non modal, continue
                    }
                    else
                        ap.ReportError("Width/Height and/or X/Y not specified correctly in Dialog");
                }
                else
                    ap.ReportError(exp[0]);
            }
            else
                ap.ReportError("Dialog command line not in correct format");

            return true;
        }

        static private void Cd_Trigger(string lname, string controlname, Object tag)
        {
            List<Object> tags = tag as List<Object>;        // object is a list of objects! lovely

            ActionProgramRun apr = tags[0] as ActionProgramRun;
            bool ismodal = (bool)tags[1];

            if (ismodal)
            {
                apr[lname] = controlname;
                apr.ResumeAfterPause();
            }
            else
            {
                apr.actioncontroller.ActionRun(ActionEvent.onNonModalDialog, new Conditions.ConditionVariables(new string[] { "Dialog", lname, "Control", controlname }));
            }
        }
    }

    public class ActionDialog : ActionDialogBase        // type of class determines Dialog action using IsModal
    {
    }

    public class ActionNonModalDialog : ActionDialogBase
    {
    }


    public class ActionDialogControl : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
        {
            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "DialogControl command", UserData, "Configure DialogControl Command", cp.Icon);
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string exp;
            if (ap.functions.ExpandString(UserData, out exp) != ConditionFunctions.ExpandResult.Failed)
            {
                StringParser sp = new StringParser(exp);
                string handle = sp.NextWordComma();

                if ( handle != null )
                { 
                    bool infile = ap.actionfile.dialogs.ContainsKey(handle);
                    bool inlocal = ap.dialogs.ContainsKey(handle);

                    ExtendedControls.ConfigurableForm f = infile ? ap.actionfile.dialogs[handle] : (inlocal ? ap.dialogs[handle] : null);

                    string cmd = sp.NextWordLCInvariant();

                    if (cmd == null)
                    {
                        ap.ReportError("Missing command in DialogControl");
                    }
                    else if (cmd.Equals("exists"))
                    {
                        ap["Exists"] = (f != null) ? "1" : "0";
                    }
                    else if (f == null)
                    {
                        ap.ReportError("No such dialog exists in DialogControl");
                    }
                    else if (cmd.Equals("continue"))
                    {
                        return (inlocal) ? false : true;    // if local, pause. else just ignore
                    }
                    else if (cmd.Equals("position"))
                    {
                        ap["X"] = f.Location.X.ToStringInvariant();
                        ap["Y"] = f.Location.Y.ToStringInvariant();
                    }
                    else if (cmd.Equals("get"))
                    {
                        string control = sp.NextWord();
                        string r;

                        if (control != null && (r = f.Get(control)) != null)
                        {
                            ap["DialogResult"] = r;
                        }
                        else
                            ap.ReportError("Missing or invalid dialog name in DialogControl get");
                    }
                    else if (cmd.Equals("set"))
                    {
                        string control = sp.NextWord(" =");
                        string value = sp.IsCharMoveOn('=') ? sp.NextQuotedWord() : null;
                        if (control != null && value != null)
                        {
                            if (!f.Set(control, value))
                                ap.ReportError("Cannot set control " + control + " in DialogControl set");
                        }
                        else
                            ap.ReportError("Missing or invalid dialog name and/or value in DialogControl set");
                    }
                    else if (cmd.Equals("close"))
                    {
                        f.Close();
                        if (inlocal)
                            ap.dialogs.Remove(handle);
                        else
                            ap.actionfile.dialogs.Remove(handle);
                    }
                    else
                        ap.ReportError("Unknown command in DialogControl");
                }
                else
                    ap.ReportError("Missing handle in DialogControl");
            }
            else
                ap.ReportError(exp);

            return true;
        }
    }
}
