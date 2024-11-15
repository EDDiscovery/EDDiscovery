/*
 * Copyright © 2017-2022 EDDiscovery development team
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
                int numopened = usercontrolsforms.CountOf(pi.PopoutID);
                //System.Diagnostics.Debug.WriteLine($"Popout {PopOutSaveID(p)} = {numopened}");
                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(PopOutSaveID(pi.PopoutID), numopened);
            }
        }

        public void LoadSavedPopouts()
        {
            PanelInformation.PanelInfo[] userselectablepanels = PanelInformation.GetUserSelectablePanelInfo(false);        // get list of panels in system

            foreach( var pi in userselectablepanels)
            {
                int numtoopen = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(PopOutSaveID(pi.PopoutID), 0);          // get number, from id

                if (numtoopen > 0) 
                {
                    System.Diagnostics.Debug.WriteLine($"Popout load {pi} number {numtoopen}");

                    int numopened = usercontrolsforms.CountOf(pi.PopoutID);                                                       // see how many we already have..
                    if (numopened < numtoopen)
                    {
                        for (int i = numopened + 1; i <= numtoopen; i++)
                            PopOut(pi.PopoutID);
                    }
                }
            }
        }

        public UserControlCommonBase PopOut(PanelInformation.PanelIDs selected)
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

                PanelInformation.PanelInfo poi = PanelInformation.GetPanelInfoByPanelID(selected);

                // we make up the title and refname based on how many previously opened of this type
                int numopened = usercontrolsforms.CountOf(selected) + 1;
                string windowtitle = poi.WindowTitle + ((numopened > 1) ? $" {numopened}" : "");
                string refname = poi.WindowRefName + numopened.ToString();

                System.Diagnostics.Trace.WriteLine($"Popout Init UCF `{windowtitle}` rn {refname}");

                ucf.Init(uccb, windowtitle, ExtendedControls.Theme.Current.WindowsFrame, refname, discoveryform.TopMost,
                             ExtendedControls.Theme.Current.LabelColor, ExtendedControls.Theme.Current.SPanelColor, ExtendedControls.Theme.Current.TransparentColorKey);

                uccb.Init(discoveryform, UserControls.UserControlCommonBase.DisplayNumberPopOuts + numopened - 1);

                ExtendedControls.Theme.Current.ApplyStd(ucf);  // apply theming/scaling to form before shown, so that it restored back to correct position (done in UCF::onLoad)

                ucf.Show();                                                     // this ends up, via Form Shown, calls LoadLayout in the UCCB.

                discoveryform.ActionRun(Actions.ActionEventEDList.onPopUp,  new BaseUtils.Variables(new string[] { "PopOutName", refname , "PopOutTitle", windowtitle, "PopOutIndex", numopened.ToString()} ));
            }

            return uccb;
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
