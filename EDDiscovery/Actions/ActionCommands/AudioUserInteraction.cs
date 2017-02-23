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

                if (ap.functions.ExpandStrings(ctrl, out exp, ap.currentvars) != ConditionLists.ExpandResult.Failed)
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

                    ap.currentvars["DialogResult"] = res.ToString();
                }
                else
                    ap.ReportError(exp[0]);
            }
            else
                ap.ReportError("MessageBox command line not in correct format");

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
            if (ap.functions.ExpandString(UserData, ap.currentvars, out res) != ConditionLists.ExpandResult.Failed)
            {
                StringParser sp = new StringParser(res);
                string cmdname = sp.NextWord(", ", true);

                if (cmdname.Equals("folder"))
                {
                    sp.IsCharMoveOn(',');

                    FolderBrowserDialog fbd = new FolderBrowserDialog();

                    string descr = sp.NextQuotedWord(", ");
                    if (descr != null)
                        fbd.Description = descr;

                    sp.IsCharMoveOn(',');
                    string rootfolder = sp.NextQuotedWord(", ");
                    if (rootfolder != null)
                    {
                        Environment.SpecialFolder sf;
                        if (Enum.TryParse<Environment.SpecialFolder>(rootfolder, out sf))
                            fbd.RootFolder = sf;
                        else
                            return ap.ReportError("FileDialog folder does not recognise folder location " + rootfolder);
                    }

                    string fileret = (fbd.ShowDialog(ap.actioncontroller.DiscoveryForm) == DialogResult.OK) ? fbd.SelectedPath : "";
                    ap.currentvars["FolderName"] = fileret;
                }
                else if (cmdname.Equals("openfile"))
                {
                    sp.IsCharMoveOn(',');

                    OpenFileDialog fd = new OpenFileDialog();
                    fd.Multiselect = false;

                    try
                    {
                        string rootfolder = sp.NextQuotedWord(", ");
                        if (rootfolder != null)
                            fd.InitialDirectory = rootfolder;

                        sp.IsCharMoveOn(',');
                        string filter = sp.NextQuotedWord(", ");
                        if (filter != null)
                            fd.Filter = filter;

                        sp.IsCharMoveOn(',');
                        string defext = sp.NextQuotedWord(", ");
                        if (defext != null)
                            fd.DefaultExt = defext;

                        sp.IsCharMoveOn(',');
                        string check = sp.NextQuotedWord(", ");
                        if (check != null && check.Equals("On", StringComparison.InvariantCultureIgnoreCase))
                            fd.CheckFileExists = fd.CheckPathExists = true;

                        string fileret = (fd.ShowDialog(ap.actioncontroller.DiscoveryForm) == DialogResult.OK) ? fd.FileName : "";
                        ap.currentvars["FileName"] = fileret;
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
            return (s != null && s.Count >= 3 && s.Count <= 4) ? s : null;
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

                if (ap.functions.ExpandStrings(ctrl, out exp, ap.currentvars) != ConditionLists.ExpandResult.Failed)
                {
                    if (!ap.actioncontroller.DiscoveryForm.AddNewMenuItemToAddOns(exp[1], exp[2], (exp.Count>=4) ? exp[3] : "None", exp[0], ap.actionfile.name))
                        ap.ReportError("MenuItem cannot add to menu, check menu");
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
                            new string[] { "Caption", "Prompt List", "Default List", "Features" , "ToolTips" }, l?.ToArray() ,
                            false,new string[] { "Enter name of menu", "List of entries, semicolon separated", "Default list, semicolon separated", "Feature list: Multiline", "List of tool tips, semocolon separated" });
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

                if (ap.functions.ExpandStrings(ctrl, out exp, ap.currentvars) != ConditionLists.ExpandResult.Failed)
                {
                    string[] prompts = exp[1].Split(';');
                    string[] def = (exp.Count >= 3) ? exp[2].Split(';') : null;
                    bool multiline = (exp.Count >= 4) ? (exp[3].IndexOf("Multiline", StringComparison.InvariantCultureIgnoreCase) >= 0) : false;
                    string [] tooltips = (exp.Count >= 5) ? exp[4].Split(';') : null;

                    List<string> r = Forms.PromptMultiLine.ShowDialog(ap.actioncontroller.DiscoveryForm, exp[0],
                                        prompts, def, multiline , tooltips);

                    ap.currentvars["InputBoxOK"] = (r != null) ? "1" : "0";
                    if ( r != null )
                    {
                        for (int i = 0; i < r.Count; i++)
                            ap.currentvars["InputBox" + (i+1).ToString()] = r[i];
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

}
