/*
 * Copyright © 2016 - 2023 EDDiscovery development team
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
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{

    ///-------------------------------------------------------------------- List of forms

    public class UserControlFormList
    {
        public int Count { get { return forms.Count; } }
        public int CountOf(PanelInformation.PanelIDs p)
        {
            return forms.Where(x => x.PanelID == p).Count();
        }
        public UserControlForm this[int i] { get { return forms[i]; } }
        public UserControlForm Find(PanelInformation.PanelIDs p, int num)
        {
            int index = forms.FindIndex(x => x.PanelID == p && x.PopOutNumber == num);
            return index >= 0 ? forms[index] : null;
        }

        public UserControlFormList(EDDiscoveryForm ed)
        {
            forms = new List<UserControlForm>();
            discoveryform = ed;
        }

        public UserControlForm GetByWindowsRefName(string name)
        {
            foreach (UserControlForm u in forms)     // first complete name
            {
                if (u.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return u;
            }

            foreach (UserControlForm u in forms)     // then partial start name
            {
                if (u.Name.StartsWith(name, StringComparison.InvariantCultureIgnoreCase))
                    return u;
            }

            return null;
        }

        public UserControlForm Find(PanelInformation.PanelIDs p)
        {
            foreach (UserControlForm u in forms)
            {
                if (u.UserControl?.Find(p) != null)     // if found inside this form
                    return u;
            }

            return null;
        }

        public UserControlForm NewForm()                // a new form is needed
        {
            UserControlForm tcf = new UserControlForm();
            forms.Add(tcf);
            tcf.FormClosed += FormClosedCallback;
            return tcf;
        }

        private void FormClosedCallback(Object sender, FormClosedEventArgs e)       // called when form closes.. by user or by us.  Remove from list
        {
            UserControlForm tcf = (UserControlForm)sender;
            forms.Remove(tcf);
            discoveryform.ActionRun(Actions.ActionEventEDList.onPopDown, new BaseUtils.Variables(new string[] { "PopOutName", tcf.DBRefName.Substring(9), "PopOutTitle", tcf.WinTitle }));
        }


        public List<int> PopOutNumberList(PanelInformation.PanelIDs p)
        {
            List<int> list = new List<int>();
            foreach (UserControlForm tcf in forms)
            {
                if (tcf.PanelID == p)
                    list.Add(tcf.PopOutNumber);
            }
            list.Sort();    // may not be in order
            return list;
        }


        public void ShowAllInTaskBar()
        {
            foreach (UserControlForm ucf in forms)
            {
                if (ucf.IsLoaded)
                    ucf.SetShowInTaskBar(true);
            }
        }

        public void MakeAllOpaque()
        {
            foreach (UserControlForm ucf in forms)
            {
                if (ucf.IsLoaded)
                {
                    ucf.SetTransparency(UserControlForm.TransparencyMode.Off);
                    ucf.SetShowTitleInTransparency(true);
                }
            }
        }

        public UserControlCommonBase.PanelActionState PerformPanelOperation(UserControlCommonBase sender, object action)
        {
            //System.Diagnostics.Debug.WriteLine($"MTC RequestOp pop outs from {sender.PanelID} {action}");

            UserControlCommonBase.PanelActionState retstate = UserControlCommonBase.PanelActionState.NotHandled;

            foreach (UserControlForm ucf in forms)
            {
                //System.Diagnostics.Debug.WriteLine($"MTC PerformOp pop outs from {sender.PanelID} distribute to tab {ucf.Name}: {action}");

                var res = ucf.UserControl.CallPerformPanelOperation(sender, action);
                //System.Diagnostics.Debug.WriteLine($"..PerformOp pop outs result {res} panel");

                if (res != UserControlCommonBase.PanelActionState.NotHandled)
                {
                    retstate = res;
                    if (UserControlCommonBase.IsPASResult(res))
                    {
                        //System.Diagnostics.Debug.WriteLine($"..PerformOp pop outs terminated {res} panel {ucf.Name}");
                        return res;
                    }
                }
            }

            return retstate;
        }

        public void CloseAll()
        {
            List<UserControlForm> list = new List<UserControlForm>(forms);       // so, closing it ends up calling back to FormCloseCallBack
                                                                                 // and it changes tabforms. So we need a copy to safely do this
            foreach (UserControlForm ucf in list)
            {
                ucf.Close();        // don't change tabforms.. the FormCloseCallBack does this
            }
        }

        public void CloseAll(PanelInformation.PanelIDs p)
        {
            List<UserControlForm> list = new List<UserControlForm>(forms);       

            foreach (UserControlForm ucf in list.Where(f=>f.UserControl?.Find(p)!=null))        // if the UCCB inside the form can find the panel, close it
            {
                ucf.Close();        
            }
        }

        public bool AllowClose()
        {
            foreach (var f in forms)
            {
                if (!f.AllowClose())
                    return false;
            }
            return true;
        }

        private List<UserControlForm> forms;
        private EDDiscoveryForm discoveryform;
    }
}

