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

        protected override void Creation(PanelInformation.PanelInfo p)
        {
            base.Creation(p);
            System.Diagnostics.Debug.WriteLine($"Ext panel create class {p.WindowTitle}");
            panel = (IEDDPanelExtension)Activator.CreateInstance((Type)p.Tag, null);
            Control pp = (Control)panel;
            pp.Dock = DockStyle.Fill;
            Controls.Add(pp);
            DBBaseName = "ExtPanel" + p.PopoutID;
        }

        public const int PanelCallBackVersion = 1;      // explicitly,this is what we do

        protected override void Init()
        {
            System.Diagnostics.Debug.Assert(PanelCallBackVersion == EDDDLLInterfaces.EDDDLLIF.PanelCallBackVersion, "***** Updated EDD DLL IF but not updated panel callbacks");

            var callbacks = new EDDPanelCallbacks();
            callbacks.ver = PanelCallBackVersion;
            callbacks.SaveString = (s, d) => { PutSetting(s, d); };
            callbacks.GetString = (s, d) => {  return GetSetting(s, d); };
            callbacks.SaveDouble = (s, d) => { PutSetting(s, d); };
            callbacks.GetDouble = (s, d) => { return GetSetting(s, d); };
            callbacks.SaveLong = (s, d) => { PutSetting(s, d); };
            callbacks.GetLong = (s, d) => { return GetSetting(s, d); };
            callbacks.LoadGridLayout = (s, d) => { System.Diagnostics.Trace.Assert(Application.MessageLoop); DGVLoadColumnLayout((DataGridView)s, d); };
            callbacks.SaveGridLayout = (s, d) => { System.Diagnostics.Trace.Assert(Application.MessageLoop); DGVSaveColumnLayout((DataGridView)s, d); };
            callbacks.SetControlText = (s) => { System.Diagnostics.Trace.Assert(Application.MessageLoop); SetControlText(s); };
            callbacks.HasControlTextArea = () => { return HasControlTextArea(); };
            callbacks.IsControlTextVisible = () => { return IsControlTextVisible(); };
            callbacks.IsTransparentModeOn = () => { return IsTransparentModeOn; };
            callbacks.IsFloatingWindow = () => {  return IsFloatingWindow; };
            callbacks.IsClosed = () => IsClosed;
            callbacks.DGVTransparent = (g, t, c) => { System.Diagnostics.Trace.Assert(Application.MessageLoop); DGVTransparent((DataGridView)g, t, c); };
            callbacks.RequestTravelGridPosition = () => 
            {
                System.Diagnostics.Trace.Assert(Application.MessageLoop);       // must be
                return (RequestPanelOperation?.Invoke(this, new RequestHistoryGridPos()) ?? PanelActionState.NotHandled) == PanelActionState.Success; 
            };
            callbacks.PushStars = (name,list) => 
            {
                System.Diagnostics.Trace.Assert(Application.MessageLoop);       // must be
                PushStars.PushType pt = name.EqualsIIC("triwanted") ? PushStars.PushType.TriWanted :
                                        name.EqualsIIC("trisystems") ? PushStars.PushType.TriSystems :
                                        PushStars.PushType.Expedition;

                return (RequestPanelOperation?.Invoke(this, new PushStars { PushTo = pt, SystemNames = list }) ?? PanelActionState.NotHandled )== PanelActionState.Success; 
            };
            callbacks.PushCSVToExpedition = (file) =>
            {
                System.Diagnostics.Trace.Assert(Application.MessageLoop);       // must be
                return (RequestPanelOperation?.Invoke(this, new UserControlCommonBase.PanelAction() { Action = PanelAction.ImportCSV, Data = file }) ?? PanelActionState.NotHandled) == PanelActionState.Success;
            };

            // we pump out same names as the theme save
            var jo = ExtendedControls.Theme.Current.ToJSON(true);
            string jostring = jo.ToString(true);
            panel.Initialise(callbacks, DisplayNumber, jostring,"");     // initialise, pass in callbacks and unused config string

            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewUIEvent += Discoveryform_OnNewUIEvent;
            DiscoveryForm.OnNewEntry += Discoveryform_OnNewEntry;
            DiscoveryForm.OnNewHistoryEntryUnfiltered += Discoveryform_OnNewHistoryEntryUnfiltered;
            DiscoveryForm.OnThemeChanged += Discoveryform_OnThemeChanged;
            DiscoveryForm.ScreenShotCaptured += Discoveryform_ScreenShotCaptured;
            DiscoveryForm.OnNewTarget += Discoveryform_OnNewTarget;
        }

        protected override void SetTransparency(bool ison, Color curcol)
        {
            panel.SetTransparency(ison, curcol);
        }

        protected override void LoadLayout()
        {
            panel.LoadLayout();
        }
        protected override void InitialDisplay()
        {
            panel.InitialDisplay();
        }

        protected override void Closing()
        {
            panel.Closing();

            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewUIEvent -= Discoveryform_OnNewUIEvent;
            DiscoveryForm.OnNewEntry -= Discoveryform_OnNewEntry;
            DiscoveryForm.OnNewHistoryEntryUnfiltered -= Discoveryform_OnNewHistoryEntryUnfiltered;
            DiscoveryForm.OnThemeChanged -= Discoveryform_OnThemeChanged;
            DiscoveryForm.ScreenShotCaptured -= Discoveryform_ScreenShotCaptured;
            DiscoveryForm.OnNewTarget -= Discoveryform_OnNewTarget;
        }

        public override bool SupportTransparency { get { return panel.SupportTransparency; } }  // override to say support transparency
        public override bool DefaultTransparent { get { return panel.DefaultTransparent; } }  // override to say default to be transparent
        protected override void TransparencyModeChanged( bool on) { panel.TransparencyModeChanged(on); }  // override to say default to be transparent
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

        public override void ReceiveHistoryEntry(EliteDangerousCore.HistoryEntry he)
        {
            panel.CursorChanged(EliteDangerousCore.DLL.EDDDLLCallerHE.CreateFromHistoryEntry(DiscoveryForm.History, he, false));
        }

        #endregion

        #region Events

        private void Discoveryform_OnHistoryChange()
        {
            var cmdr = EliteDangerousCore.EDCommander.GetCommander(DiscoveryForm.History.CommanderId);
            if ( cmdr!=null)    // if may be null if in -noload
                panel.HistoryChange(DiscoveryForm.History.Count, cmdr.Name, cmdr.NameIsBeta, cmdr.LegacyCommander);
        }

        private void Discoveryform_OnNewHistoryEntryUnfiltered(EliteDangerousCore.HistoryEntry he)
        {
            panel.NewUnfilteredJournal(EliteDangerousCore.DLL.EDDDLLCallerHE.CreateFromHistoryEntry(DiscoveryForm.History, he, false));
        }

        private void Discoveryform_OnNewEntry(EliteDangerousCore.HistoryEntry he)
        {
            panel.NewFilteredJournal(EliteDangerousCore.DLL.EDDDLLCallerHE.CreateFromHistoryEntry(DiscoveryForm.History, he, false));
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
            var jo = ExtendedControls.Theme.Current.ToJSON(true);
            string jostring = jo.ToString(true);
            panel.ThemeChanged(jostring);
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
