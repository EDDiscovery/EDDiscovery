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
 *
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */

using EDDiscovery.UserControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace EDDiscovery
{
    public class PopOutControl
    {
        public UserControlFormList usercontrolsforms;
        EDDiscoveryForm discoveryform;

        public PopOutControl( EDDiscoveryForm ed )
        {
            discoveryform = ed;
            usercontrolsforms = new UserControlFormList(discoveryform);
        }

        public int Count { get { return usercontrolsforms.Count;  } }
        public UserControlForm GetByWindowsRefName(string name) { return usercontrolsforms.GetByWindowsRefName(name); }

        public UserControlForm FindUCCB(Type t) { return usercontrolsforms.FindUCCB(t); }
        public UserControlForm this[int i] { get { return usercontrolsforms[i]; } }

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

        public void SaveCurrentPopouts()
        {
            foreach (PanelInformation.PanelIDs p in Enum.GetValues(typeof(PanelInformation.PanelIDs)))        // in terms of PanelInformation.PopOuts Enum
            {
                PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByPanelID(p);
                if (pi != null) // paranoia
                {
                    int numopened = usercontrolsforms.CountOf(pi.PopoutType);
                    if ( numopened>0) System.Diagnostics.Debug.WriteLine($"Save Popout {p} {numopened}");
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(PopOutSaveID(p), numopened);
                }
            }
        }

        public void LoadSavedPopouts()
        {
            foreach (PanelInformation.PanelIDs p in Enum.GetValues(typeof(PanelInformation.PanelIDs)))        // in terms of PanelInformation.PopOuts Enum
            {
                int numtoopen = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(PopOutSaveID(p), 0);

                PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByPanelID(p);

                //System.Diagnostics.Debug.WriteLine("Load panel type " + paneltype.Name + " " + p.ToString() + " " + numtoopen);

                if (pi != null && numtoopen > 0) // paranoia on first..
                {
                    System.Diagnostics.Debug.WriteLine($"Load Popout {p} {numtoopen}");

                    int numopened = usercontrolsforms.CountOf(pi.PopoutType);
                    if (numopened < numtoopen)
                    {
                        for (int i = numopened + 1; i <= numtoopen; i++)
                            PopOut(p);
                    }
                }
            }
        }


        public UserControlCommonBase PopOut(PanelInformation.PanelIDs selected)
        {
            UserControlForm tcf = usercontrolsforms.NewForm();
            tcf.Icon = Properties.Resources.edlogo_3mo_icon;

            UserControlCommonBase ctrl = PanelInformation.Create(selected);

            PanelInformation.PanelInfo poi = PanelInformation.GetPanelInfoByPanelID(selected);

            if (ctrl != null && poi != null )
            {
                int numopened = usercontrolsforms.CountOf(ctrl.GetType()) + 1;
                string windowtitle = poi.WindowTitle + " " + ((numopened > 1) ? numopened.ToString() : "");
                string refname = poi.WindowRefName + numopened.ToString();

                System.Diagnostics.Trace.WriteLine("PO:Make " + windowtitle + " ucf " + ctrl.GetType().Name);

                //System.Diagnostics.Debug.WriteLine("TCF init");
                tcf.Init(ctrl, windowtitle, ExtendedControls.Theme.Current.WindowsFrame, refname, discoveryform.TopMost,
                             ExtendedControls.Theme.Current.LabelColor, ExtendedControls.Theme.Current.SPanelColor, ExtendedControls.Theme.Current.TransparentColorKey);

                //System.Diagnostics.Debug.WriteLine("UCCB init of " + ctrl.GetType().Name);
                ctrl.Init(discoveryform, UserControls.UserControlCommonBase.DisplayNumberPopOuts + numopened - 1);

                ExtendedControls.Theme.Current.ApplyStd(tcf);  // apply theming/scaling to form before shown, so that it restored back to correct position (done in UCF::onLoad)

                //System.Diagnostics.Debug.WriteLine("Show");
                tcf.Show();                                                     // this ends up, via Form Shown, calls LoadLayout in the UCCB.

                discoveryform.ActionRun(Actions.ActionEventEDList.onPopUp,  new BaseUtils.Variables(new string[] { "PopOutName", refname , "PopOutTitle", windowtitle, "PopOutIndex", numopened.ToString()} ));
            }

            return ctrl;
        }

        public bool AllowClose()
        {
            return usercontrolsforms.AllowClose();
        }
    }
}
