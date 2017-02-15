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
 * EDDiscovery is not affiliated with Fronter Developments plc.
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

        bool FromString(string input, out string msg, out string caption)
        {
            StringParser sp = new StringParser(input);
            msg = sp.NextQuotedWord(", ");
            caption = null;

            if (msg != null)
            {
                msg.ReplaceEscapeControlChars();
                sp.IsCharMoveOn(',');
                caption = sp.NextQuotedWord(", ");

                if (caption != null)
                    return true;
            }

            msg = caption = "";
            return false;
        }

        string ToString(string msg, string caption)
        {
            return msg.EscapeControlChars().QuoteString(comma: true) + "," + caption.QuoteString(comma: true);
        }

        public override string VerifyActionCorrect()
        {
            string msg, caption;
            return FromString(userdata, out msg, out caption) ? null : "MessageBox command line not in correct format";
        }

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            string msg, caption;
            FromString(UserData, out msg, out caption);
            Tuple<string,string> promptValue = PromptDoubleLine.ShowDialog(parent, discoveryform.theme, "Message", "Caption",msg,caption, "Configure MessageBox Dialog");
            if (promptValue != null)
            {
                userdata = ToString(promptValue.Item1.EscapeControlChars().QuoteString(true), promptValue.Item2.QuoteString(true));
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string msg, caption;
            if (FromString(UserData, out msg, out caption))
            {
                string msgres, captionres;
                if (ap.functions.ExpandString(msg, ap.currentvars, out msgres) != ConditionLists.ExpandResult.Failed)
                {
                    if (ap.functions.ExpandString(caption, ap.currentvars, out captionres) != ConditionLists.ExpandResult.Failed)
                    {
                        if (captionres.Length == 0)
                            captionres = "EDDiscovery Program Message";

                        MessageBox.Show(ap.actioncontroller.DiscoveryForm, msgres.ReplaceEscapeControlChars(), captionres);
                    }
                    else
                        ap.ReportError(captionres);
                }
                else
                    ap.ReportError(msgres);
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
            string promptValue = PromptSingleLine.ShowDialog(parent, discoveryform.theme, "Options", UserData, "Configure File Dialog");
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

                        System.Diagnostics.Debug.WriteLine("Init dir" + fd.InitialDirectory);

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
}
