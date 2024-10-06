/*
 * Copyright © 2022-2022 EDDiscovery development team
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

using QuickJSON;
using System;
using System.Drawing;
using System.Windows.Forms;
using static EDDDLLInterfaces.EDDDLLIF;

namespace EDDiscovery.UserControls
{
    public partial class UserControlPythonPanel : UserControlCommonBase
    {
        private Actions.ActionController actioncontroller;

        public UserControlPythonPanel()
        {
            InitializeComponent();
        }

        #region UCCB IF

        public override void Creation(PanelInformation.PanelInfo p)
        {
            base.Creation(p);
            System.Diagnostics.Debug.WriteLine($"Python panel create class {p.WindowTitle} db {DBBaseName}");
            DBBaseName = "ExtPanel" + p.PopoutID;
        }

        public override void Init()
        {
            System.Diagnostics.Debug.WriteLine($"Python panel Init {DBBaseName}");

            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewUIEvent += Discoveryform_OnNewUIEvent;
            DiscoveryForm.OnNewEntry += Discoveryform_OnNewEntry;
            DiscoveryForm.OnNewHistoryEntryUnfiltered += Discoveryform_OnNewHistoryEntryUnfiltered;
            DiscoveryForm.OnThemeChanged += Discoveryform_OnThemeChanged;
            DiscoveryForm.ScreenShotCaptured += Discoveryform_ScreenShotCaptured;
            DiscoveryForm.OnNewTarget += Discoveryform_OnNewTarget;

          //  actioncontroller = new Actions.ActionController(DiscoveryForm, DiscoveryForm.Icon, new Type[] { }, true);
        }

        public override void SetTransparency(bool ison, Color curcol)
        {
        }

        public override void LoadLayout()
        {
        }
        public override void InitialDisplay()
        {
        }

        public override void Closing()
        {
            System.Diagnostics.Debug.WriteLine($"Python panel Closing {DBBaseName}");

            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewUIEvent -= Discoveryform_OnNewUIEvent;
            DiscoveryForm.OnNewEntry -= Discoveryform_OnNewEntry;
            DiscoveryForm.OnNewHistoryEntryUnfiltered -= Discoveryform_OnNewHistoryEntryUnfiltered;
            DiscoveryForm.OnThemeChanged -= Discoveryform_OnThemeChanged;
            DiscoveryForm.ScreenShotCaptured -= Discoveryform_ScreenShotCaptured;
            DiscoveryForm.OnNewTarget -= Discoveryform_OnNewTarget;
        }

        //public override bool SupportTransparency { get { return panel.SupportTransparency; } }  // override to say support transparency
        //public override bool DefaultTransparent { get { return panel.DefaultTransparent; } }  // override to say default to be transparent
        
        public override void TransparencyModeChanged( bool on) {  }  // override to say default to be transparent
        //public override bool AllowClose() { return panel.AllowClose(); }

        //public override string HelpKeyOrAddress()
        //{
        //    return panel.HelpKeyOrAddress();
        //}

        public override void onControlTextVisibilityChanged(bool newvalue)
        {
        //    panel.ControlTextVisibleChange(newvalue);
        }

        #endregion

        #region Panel reactions

        public override void ReceiveHistoryEntry(EliteDangerousCore.HistoryEntry he)
        {
        }

        #endregion

        #region Events

        private void Discoveryform_OnHistoryChange()
        {
        }

        private void Discoveryform_OnNewHistoryEntryUnfiltered(EliteDangerousCore.HistoryEntry he)
        {
        }

        private void Discoveryform_OnNewEntry(EliteDangerousCore.HistoryEntry he)
        {
        }

        private void Discoveryform_OnNewUIEvent(EliteDangerousCore.UIEvent uievent)
        {
            QuickJSON.JToken t = QuickJSON.JToken.FromObject(uievent, ignoreunserialisable: true,
                                                            ignored: new Type[] { typeof(Bitmap), typeof(Image) },
                                                            maxrecursiondepth: 3);
        }

        private void Discoveryform_OnThemeChanged()
        {
            var th = ExtendedControls.Theme.Current;
            var jo = JObject.FromObject(th, true, maxrecursiondepth: 5, membersearchflags: System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        }

        private void Discoveryform_ScreenShotCaptured(string file, Size size)
        {
        }

        private void Discoveryform_OnNewTarget(object obj)
        {
            var hastarget = EliteDangerousCore.DB.TargetClass.GetTargetPosition(out string name, out double x, out double y, out double z);
        }


        #endregion
    }
}
