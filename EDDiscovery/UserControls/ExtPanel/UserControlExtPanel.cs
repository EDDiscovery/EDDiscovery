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
    public partial class UserControlExtPanel : UserControlCommonBase
    {
        public UserControlExtPanel()
        {
            InitializeComponent();
        }

        private IEDDPanelExtension panel;

        #region UCCB IF

        public override void Creation(PanelInformation.PanelInfo p)
        {
            base.Creation(p);
            System.Diagnostics.Debug.WriteLine($"Ext panel create class {p.WindowTitle}");
            panel = (IEDDPanelExtension)Activator.CreateInstance((Type)p.Tag, null);
            Control pp = (Control)panel;
            pp.Dock = DockStyle.Fill;
            Controls.Add(pp);
            DBBaseName = "ExtPanel" + p.PopoutID;
        }

        public override void Init()
        {
            var callbacks = new EDDPanelCallbacks();
            callbacks.ver = 1;
            callbacks.SaveString = (s, d) => PutSetting(s, d);
            callbacks.GetString = (s, d) => GetSetting(s, d);
            callbacks.SaveDouble = (s, d) => PutSetting(s, d);
            callbacks.GetDouble = (s, d) => GetSetting(s, d);
            callbacks.SaveLong = (s, d) => PutSetting(s, d);
            callbacks.GetLong = (s, d) => GetSetting(s, d);
            callbacks.LoadGridLayout = (s, d) => DGVLoadColumnLayout((DataGridView)s, d);
            callbacks.SaveGridLayout = (s, d) => DGVSaveColumnLayout((DataGridView)s, d);
            callbacks.SetControlText = (s) => SetControlText(s);
            callbacks.HasControlTextArea = () => HasControlTextArea();
            callbacks.IsControlTextVisible = () => IsControlTextVisible();
            callbacks.IsTransparentModeOn = () => IsTransparentModeOn;
            callbacks.IsFloatingWindow = () => IsFloatingWindow;
            callbacks.IsClosed = () => IsClosed;
            callbacks.DGVTransparent = (g, t, c) => DGVTransparent((DataGridView)g, t, c);
  
            var th = ExtendedControls.Theme.Current;
            var jo = JObject.FromObject(th, true, maxrecursiondepth: 5, membersearchflags: System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            panel.Initialise(callbacks, displaynumber, jo.ToString(),"");     // initialise, pass in callbacks and unused config string

            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;
            discoveryform.OnNewUIEvent += Discoveryform_OnNewUIEvent;
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            discoveryform.OnNewHistoryEntryUnfiltered += Discoveryform_OnNewHistoryEntryUnfiltered;
            discoveryform.OnThemeChanged += Discoveryform_OnThemeChanged;
            discoveryform.ScreenShotCaptured += Discoveryform_ScreenShotCaptured;
            discoveryform.OnNewTarget += Discoveryform_OnNewTarget;
        }

        public override void SetTransparency(bool ison, Color curcol)
        {
            panel.SetTransparency(ison, curcol);
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
            panel.LoadLayout();
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
        }

        public override void InitialDisplay()
        {
            panel.InitialDisplay();
        }

        public override void Closing()
        {
            panel.Closing();

            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;

            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
            discoveryform.OnNewUIEvent -= Discoveryform_OnNewUIEvent;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnNewHistoryEntryUnfiltered -= Discoveryform_OnNewHistoryEntryUnfiltered;
            discoveryform.OnThemeChanged -= Discoveryform_OnThemeChanged;
            discoveryform.ScreenShotCaptured -= Discoveryform_ScreenShotCaptured;
            discoveryform.OnNewTarget -= Discoveryform_OnNewTarget;
        }

        public override bool SupportTransparency { get { return panel.SupportTransparency; } }  // override to say support transparency
        public override bool DefaultTransparent { get { return panel.DefaultTransparent; } }  // override to say default to be transparent
        public override bool AllowClose() { return panel.AllowClose(); }

        public override string HelpKeyOrAddress()
        {
            return panel.HelpKeyOrAddress();
        }

        public override void onControlTextVisibilityChanged(bool newvalue)
        {
            panel.ControlTextVisibleChange(newvalue);
        }

        #endregion

        #region Panel reactions
        private void Uctg_OnTravelSelectionChanged(EliteDangerousCore.HistoryEntry he, EliteDangerousCore.HistoryList hl, bool selectedEntry)
        {
            panel.CursorChanged(EliteDangerousCore.DLL.EDDDLLCallerHE.CreateFromHistoryEntry(hl, he, false));
        }

        #endregion

        #region Events

        private void Discoveryform_OnHistoryChange(EliteDangerousCore.HistoryList hl)
        {
            var cmdr = EliteDangerousCore.EDCommander.GetCommander(hl.CommanderId);
            panel.HistoryChange(hl.Count, cmdr.Name, cmdr.NameIsBeta, cmdr.LegacyCommander);
        }

        private void Discoveryform_OnNewHistoryEntryUnfiltered(EliteDangerousCore.HistoryEntry he)
        {
            panel.NewUnfilteredJournal(EliteDangerousCore.DLL.EDDDLLCallerHE.CreateFromHistoryEntry(discoveryform.history, he, false));
        }

        private void Discoveryform_OnNewEntry(EliteDangerousCore.HistoryEntry he, EliteDangerousCore.HistoryList hl)
        {
            panel.NewFilteredJournal(EliteDangerousCore.DLL.EDDDLLCallerHE.CreateFromHistoryEntry(hl, he, false));
        }

        private void Discoveryform_OnNewUIEvent(EliteDangerousCore.UIEvent uievent)
        {
            QuickJSON.JToken t = QuickJSON.JToken.FromObject(uievent, ignoreunserialisable: true,
                                                            ignored: new Type[] { typeof(Bitmap), typeof(Image) },
                                                            maxrecursiondepth: 3);
            panel.NewUIEvent(t?.ToString() ?? "");
        }

        private void Discoveryform_OnThemeChanged()
        {
            var th = ExtendedControls.Theme.Current;
            var jo = JObject.FromObject(th, true, maxrecursiondepth: 5, membersearchflags: System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            panel.ThemeChanged(jo.ToString());
        }

        private void Discoveryform_ScreenShotCaptured(string file, Size size)
        {
            panel.ScreenShotCaptured(file, size);
        }

        private void Discoveryform_OnNewTarget(object obj)
        {
            var hastarget = EliteDangerousCore.DB.TargetClass.GetTargetPosition(out string name, out double x, out double y, out double z);
            panel.NewTarget(hastarget ? new Tuple<string, double, double, double>(name, x, y, z) : null);
        }


        #endregion
    }
}
