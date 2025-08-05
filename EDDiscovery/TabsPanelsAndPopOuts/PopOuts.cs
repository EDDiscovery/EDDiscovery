/*
 * Copyright 2017-2025 EDDiscovery development team
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

using EDDiscovery.UserControls;
using System;

namespace EDDiscovery
{
    public class PopOutControl
    {
        public PopOutControl( EDDiscoveryForm ed )
        {
            discoveryform = ed;
            usercontrolsforms = new UserControlFormList(discoveryform);
        }

        public int Count { get { return usercontrolsforms.Count;  } }
        public UserControlForm GetByWindowsRefName(string name) { return usercontrolsforms.GetByWindowsRefName(name); }

        public UserControlForm Find(PanelInformation.PanelIDs p) { return usercontrolsforms.Find(p); }

        public UserControlForm this[int i] { get { return usercontrolsforms[i]; } }

        public Func<UserControlCommonBase, object, UserControlCommonBase.PanelActionState> RequestPanelOperation;        // Request other panel does work

        private static string PopOutSaveID(PanelInformation.PanelIDs p)
        {
            return EDDProfiles.Instance.UserControlsPrefix + "SavedPanelInformation.PopOuts:" + p.ToString();
        }

        public void ShowAllPopOutsInTaskBar()
        {
            usercontrolsforms.ShowAllInTaskBar();
        }

        public void MakeAllPopoutsOpaque()
        {
            usercontrolsforms.MakeAllOpaque();
        }

        public void CloseAllPopouts()
        {
            usercontrolsforms.CloseAll();
        }

        public void CloseAllPopouts(PanelInformation.PanelIDs p)
        {
            usercontrolsforms.CloseAll(p);
        }

        public void SaveCurrentPopouts()
        {
            PanelInformation.PanelInfo[] userselectablepanels = PanelInformation.GetUserSelectablePanelInfo(false);        // get list of panels in system

            foreach (var pi in userselectablepanels)
            {
                // new method saves the pop out number list
                var list = usercontrolsforms.PopOutNumberList(pi.PopoutID);
                EliteDangerousCore.DB.UserDatabase.Instance.PutSetting(PopOutSaveID(pi.PopoutID), list.ToString(","));
            }
        }

        public void LoadSavedPopouts()
        {
            PanelInformation.PanelInfo[] userselectablepanels = PanelInformation.GetUserSelectablePanelInfo(false);        // get list of panels in system

            foreach (var pi in userselectablepanels)
            {
                // lets try the old method, which is the number..
                int numtoopen = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting(PopOutSaveID(pi.PopoutID), 0);          // get number, from id

                for (int i = 0; usercontrolsforms.CountOf(pi.PopoutID) < numtoopen; i++)
                    PopOut(pi.PopoutID);

                // new method (nov 24) saves a list of pop out instances and puts them up. Don't say I don't every do things for you!

                string newlist = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting(PopOutSaveID(pi.PopoutID), "");     // get list
                if ( newlist.HasChars())
                {
                    var popoutlist = newlist.RestoreIntListFromString();
                    foreach( int x in popoutlist)
                    {
                        if (usercontrolsforms.Find(pi.PopoutID, x) == null)
                            PopOut(pi.PopoutID, x);
                    }
                }
            }
        }

        // pop out the next free instance number of this pop out
        public UserControlCommonBase PopOut(PanelInformation.PanelIDs selected)
        {
            for( int i =0; i < UserControlCommonBase.DisplayNumberStartExtraTabs-1; i++ )       //0..98 (= DisplayNumbers 1 to 99, this is the limit defined)
            {
                if (usercontrolsforms.Find(selected,i)==null)      // if popout i is not there
                    return PopOut(selected, i);
            }

            return PopOut(selected, 0); // unlikely but a backup
        }

        // open panel instance N of selected. N = 0 onwards
        public UserControlCommonBase PopOut(PanelInformation.PanelIDs selected, int number)
        {
            // tcf holds the panel

            UserControlForm ucf = usercontrolsforms.NewForm();
            ucf.Icon = Properties.Resources.edlogo_3mo_icon;

            // uccb creation of selected panel
            UserControlCommonBase uccb = PanelInformation.Create(selected);

            if (uccb != null )
            {
                uccb.RequestPanelOperation += (s, o) =>
                {
                    return RequestPanelOperation.Invoke(s, o);
                };

                PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByPanelID(selected);

                // we make up the title and refname based on how many previously opened of this type
                string windowtitle = pi.WindowTitle + ((number > 0) ? $" {number+1}" : "");
                string refname = pi.WindowRefName + (number+1).ToString();      // +1 is historical

                System.Diagnostics.Trace.WriteLine($"Popout Init UCF {pi.WindowTitle} instance {number} title `{windowtitle}` refnumber {refname}");

                ucf.Init(uccb, windowtitle, ExtendedControls.Theme.Current.WindowsFrame, refname, discoveryform.TopMost);

                uccb.CallInit(discoveryform, UserControlCommonBase.DisplayNumberPopOuts + number);

                System.Diagnostics.Debug.WriteLine($"UCCB Display number {ucf.PopOutNumber}");
                ExtendedControls.Theme.Current.ApplyStd(ucf);   // apply theming/scaling to form before shown, so that it restored back to correct position (done in UCF::onLoad)

                ucf.Show();                                     // this ends up, via Form Shown, calls LoadLayout in the UCCB.

                discoveryform.ActionRun(Actions.ActionEventEDList.onPopUp,  new BaseUtils.Variables(new string[] { "PopOutName", refname , "PopOutTitle", windowtitle, "PopOutIndex", number.ToString()} ));
            }

            return uccb;
        }

        public void OnThemeChanged()        // called when themes have changed
        {
            for( int i = 0; i < usercontrolsforms.Count; i++ )
                usercontrolsforms[i].OnThemeChanged();
        }

        public UserControlCommonBase.PanelActionState PerformPanelOperation(UserControlCommonBase sender, object actionobj)
        {
            return usercontrolsforms.PerformPanelOperation(sender, actionobj);
        }

        public bool AllowClose()
        {
            return usercontrolsforms.AllowClose();
        }

        private UserControlFormList usercontrolsforms;
        private EDDiscoveryForm discoveryform;
    }
}
