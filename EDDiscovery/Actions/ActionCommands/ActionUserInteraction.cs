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

namespace EDDiscovery.Actions
{
    public class ActionMessageBox : Action
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

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            List<string> l = FromString(userdata);
            List<string> r = Forms.PromptMultiLine.ShowDialog(parent, "Configure MessageBox Dialog",
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

                    DialogResult res = Forms.MessageBoxTheme.Show(ap.actioncontroller.DiscoveryForm, exp[0], caption, but, icon);

                    // debug Forms.MessageBoxTheme.Show(exp[0], caption, but, icon);

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

    public class ActionInfoBox : Action
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

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            List<string> l = FromString(userdata);
            List<string> r = Forms.PromptMultiLine.ShowDialog(parent, "Configure InfoBox Dialog",
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

                    Forms.InfoForm ifrm = new Forms.InfoForm();
                    ifrm.Info(caption, exp[0], null, new int[] { 0, 100, 200, 300, 400, 500, 600 }, true);
                    ifrm.Show(ap.actioncontroller.DiscoveryForm);
                }
                else
                    ap.ReportError(exp[0]);
            }
            else
                ap.ReportError("InfoBox command line not in correct format");

            return true;
        }
    }

    public class ActionFileDialog : Action
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            string promptValue = Forms.PromptSingleLine.ShowDialog(parent, "Options", UserData, "Configure File Dialog");
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
                string cmdname = sp.NextWord(", ", true);

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

                    string fileret = (fbd.ShowDialog(ap.actioncontroller.DiscoveryForm) == DialogResult.OK) ? fbd.SelectedPath : "";
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

                        string fileret = (fd.ShowDialog(ap.actioncontroller.DiscoveryForm) == DialogResult.OK) ? fd.FileName : "";
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

                        string fileret = (fd.ShowDialog(ap.actioncontroller.DiscoveryForm) == DialogResult.OK) ? fd.FileName : "";
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

    class ActionMenuItem : Action
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        List<string> FromString(string input)
        {
            StringParser sp = new StringParser(input);
            List<string> s = sp.NextQuotedWordList();
            return (s != null && (s.Count == 1 || (s.Count >= 3 && s.Count <= 4))) ? s : null;
        }

        public override string VerifyActionCorrect()
        {
            return (FromString(userdata) != null) ? null : "MenuItem command line not in correct format";
        }

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            List<string> l = FromString(userdata);
            List<string> r = Forms.PromptMultiLine.ShowDialog(parent, "Configure MenuInput Dialog",
                            new string[] { "MenuName", "In Menu", "Menu Text", "Icon" }, l?.ToArray());
            if ( r != null)
            {
                if (r[1].Length == 0 && r[2].Length == 0 && r[3].Length == 0)
                    userdata = r[0];
                else
                    userdata = r.ToStringCommaList(); 
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
                    if (exp.Count == 1 )
                    {
                        ap["MenuPresent"] = ap.actioncontroller.DiscoveryForm.IsMenuItemInstalled(exp[0]) ? "1" : "0";
                    }
                    else
                    {
                        if (!ap.actioncontroller.DiscoveryForm.AddNewMenuItemToAddOns(exp[1], exp[2], (exp.Count >= 4) ? exp[3] : "None", exp[0], ap.actionfile.name))
                            ap.ReportError("MenuItem cannot add to menu, check menu");
                    }
                }
                else
                    ap.ReportError(exp[0]);
            }
            else
                ap.ReportError("MenuItem command line not in correct format");

            return true;
        }
    }

    public class ActionInputBox : Action
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

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            List<string> l = FromString(userdata);
            List<string> r = Forms.PromptMultiLine.ShowDialog(parent, "Configure InputBox Dialog",
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

                    List<string> r = Forms.PromptMultiLine.ShowDialog(ap.actioncontroller.DiscoveryForm, exp[0],
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

    public class ActionDialog : Action
    {
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

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            List<string> l = FromString(userdata);
            List<string> r = Forms.PromptMultiLine.ShowDialog(parent, "Configure Dialog",
                            new string[] { "Logical Name" , "Caption", "Size", "Var Prefix" }, l?.ToArray(),
                            false, new string[] { "Handle name of menu" , "Enter title of menu", "Size, as w,h (200,300)", "Variable Prefix" });
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

                    List<Forms.ConfigurableForm.Entry> entries = new List<Forms.ConfigurableForm.Entry>();

                    foreach( string k in cv.NameList )
                    {
                        Forms.ConfigurableForm.Entry entry;
                        string errmsg =Forms.ConfigurableForm.MakeEntry(cv[k], out entry);
                        if (errmsg != null)
                            return ap.ReportError(errmsg + " in " + k + " variable for Dialog");
                        entries.Add(entry);
                    }

                    StringParser sp2 = new StringParser(exp[2]);
                    int? dw = sp2.NextWordComma().InvariantParseIntNull();
                    int? dh = sp2.NextWord().InvariantParseIntNull();

                    if (dw != null && dh != null)
                    {
                        Forms.ConfigurableForm cd = new Forms.ConfigurableForm();
                        ap.dialogs[exp[0]] = cd;
                        cd.Trigger += Cd_Trigger;
                        cd.Show(ap.actioncontroller.DiscoveryForm, exp[0], new System.Drawing.Size(dw.Value, dh.Value), exp[1], entries.ToArray(), ap);
                        return false;       // STOP, wait input
                    }
                    else
                        ap.ReportError("Width/Height not specified in Dialog");
                }
                else
                    ap.ReportError(exp[0]);
            }
            else
                ap.ReportError("Dialog command line not in correct format");

            return true;
        }

        private void Cd_Trigger(string lname, string res, Object tag)
        {
            ActionProgramRun apr = tag as ActionProgramRun;
            apr[lname] = res;
            apr.ResumeAfterPause();
        }
    }


    public class ActionDialogControl : Action
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            string promptValue = Forms.PromptSingleLine.ShowDialog(parent, "DialogControl command", UserData, "Configure DialogControl Command");
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

                if (handle != null && ap.dialogs.ContainsKey(handle))
                {
                    Forms.ConfigurableForm f = ap.dialogs[handle];

                    string cmd = sp.NextWord(lowercase: true);

                    if (cmd == null)
                        ap.ReportError("Missing command in DialogControl");
                    else if (cmd.Equals("continue"))
                    {
                        return false;
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
                            if ( !f.Set(control, value) )
                                ap.ReportError("Cannot set control " + control + " in DialogControl set");
                        }
                        else
                            ap.ReportError("Missing or invalid dialog name and/or value in DialogControl set");
                    }
                    else if (cmd.Equals("close"))
                    {
                        f.Close();
                        ap.dialogs.Remove(handle);
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
