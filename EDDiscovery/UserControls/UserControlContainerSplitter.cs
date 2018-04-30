/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EDDiscovery.Forms;
using EDDiscovery.UserControls;
using EliteDangerousCore.DB;
using EliteDangerousCore;

namespace EDDiscovery.UserControls
{
    public partial class UserControlContainerSplitter: UserControlCommonBase        // circular, huh! neat!
    {
        private IHistoryCursor ucursor_history;     // one passed to us, refers to thc.uctg
        private IHistoryCursor ucursor_inuse;  // one in use

        private string DbWindows { get { return "SplitterControlWindows" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbPositions { get { return "SplitterControlSplitters" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        public UserControlContainerSplitter()
        {
            InitializeComponent();
        }

        public override void Init()
        { 
            ucursor_history = ucursor_inuse = uctg;
            //System.Diagnostics.Debug.WriteLine("Init Grid Use THC " + ucursor_inuse.GetHashCode());
        }

        UserControl MakeUC(string t)
        {
            UserControl u = new UserControl();
            u.Name = t;
            Button b = new Button();
            b.Size = new Size(200, 40);
            b.Text = t;
            b.Location = new Point(10, 10);
            u.Controls.Add(b);
            u.Dock = DockStyle.Fill;
            u.BackColor = Color.Yellow;
            return u;
        }

        private Control MakeTabControl(string s)
        {
            ExtendedControls.TabStrip tabstrip = new ExtendedControls.TabStrip();
            tabstrip.ImageList = PanelInformation.GetPanelImages();
            tabstrip.TextList = PanelInformation.GetPanelDescriptions();
            tabstrip.TagList = PanelInformation.GetPanelIDs().Cast<Object>().ToArray();
            tabstrip.BackColor = Color.FromArgb(40, 40, 40);
            tabstrip.Dock = DockStyle.Fill;
            tabstrip.DropDownWidth = 500;
            tabstrip.DropDownHeight = 500;
            tabstrip.StripMode = ExtendedControls.TabStrip.StripModeType.ListSelection;

            BaseUtils.StringParser sp = new BaseUtils.StringParser(s);      // ctrl string is tag,panel enum number
            int id = sp.NextInt(",") ?? 0;
            sp.IsCharMoveOn(',');
            int panelid = sp.NextInt(",") ?? -1;

            tabstrip.Tag = id;                                                 // Tag stores the ID index of this view
            tabstrip.Name = "TC-" + id.ToStringInvariant();
            System.Diagnostics.Debug.WriteLine("Make new tab control " + tabstrip.Name);

            tabstrip.OnRemoving += (tab, ctrl) =>
            {
                UserControlCommonBase uccb = ctrl as UserControlCommonBase;
                uccb.Closing();
            };

            tabstrip.OnCreateTab += (tab, si) =>
            {
                PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByEnum((PanelInformation.PanelIDs)tab.TagList[si]);
                Control c = PanelInformation.Create(pi.PopoutID);
                c.Name = pi.WindowTitle;        // tabs uses Name field for display, must set it

                //discoveryform.ActionRun(Actions.ActionEventEDList.onPanelChange, null,
                //    new Conditions.ConditionVariables(new string[] { "PanelTabName", pi.WindowRefName, "PanelTabTitle", pi.WindowTitle, "PanelName", t.Name }));

                return c;
            };

            tabstrip.OnPostCreateTab += (tab, ctrl, i) =>
            {
                int displaynumber = DisplayNumberOfGridInstance((int)tab.Tag);                         // tab strip - use tag to remember display id which helps us save context.
                UserControlCommonBase uc = ctrl as UserControlCommonBase;

                if (uc != null)
                {
                    System.Diagnostics.Debug.WriteLine("Make Tab " + tab.Tag + " Use THC " + ucursor_inuse.GetHashCode());
                    uc.Init(discoveryform, ucursor_inuse, displaynumber);
                    uc.LoadLayout();
                    uc.InitialDisplay();
                }

                //System.Diagnostics.Debug.WriteLine("And theme {0}", i);
                discoveryform.theme.ApplyToControls(tab);

                if ( panelPlayfield.Controls.Count>0)       // so, on first create, during Init, this won't be set yet.. only if dynamically change we want to do this.
                    AssignTHC();        
            };

            //            discoveryform.theme.ApplyToControls(tab);

            discoveryform.theme.ApplyToControls(tabstrip, applytothis:true);

            tabstrip.OnPopOut += (tab, i) => { discoveryform.PopOuts.PopOut((PanelInformation.PanelIDs)tabstrip.TagList[i]); };

            PanelInformation.PanelIDs[] pids = PanelInformation.GetPanelIDs();

            panelid = Array.FindIndex(pids, x=> x == (PanelInformation.PanelIDs)panelid);
            if (panelid >= 0)
                tabstrip.SelectedIndex = panelid;
            return tabstrip;
        }

        private SplitContainer MakeSplitContainer(Orientation ori, int lv)
        {
            SplitContainer sc = new SplitContainer() { Orientation = ori, Width = 1000, Height = 1000 };    // set width big to provide some res to splitter dist
            sc.Dock = DockStyle.Fill;
            sc.FixedPanel = FixedPanel.None;    // indicate to scale splitter distance on resize of control
            sc.MouseClick += Sc_MouseClick;
            sc.Name = "SC-" + lv;
            sc.Controls[0].Name = lv + "-P1";
            sc.Controls[1].Name = lv + "-P2";
            return sc;
        }

        public override void LoadLayout()
        {
            string splitctrl = SQLiteConnectionUser.GetSettingString(DbWindows, "H(0.50, U'0', U'1')");
            System.Diagnostics.Debug.WriteLine("Split ctrl" + splitctrl);
            SuspendLayout();

            SplitContainer sp = ControlHelpersStaticFunc.SplitterTreeMakeFromCtrlString(new BaseUtils.StringParser(splitctrl),
                    MakeSplitContainer, 
                    MakeTabControl , 0);

            panelPlayfield.Controls.Add(sp);
            //panelPlayfield.Controls[0].DumpTree(0);

            ResumeLayout();
            Invalidate(true);
            Update();        // need this to FORCE a full refresh in case there are lots of windows

            AssignTHC();
        }

        public override void Closing()
        {
            PanelInformation.PanelIDs[] pids = PanelInformation.GetPanelIDs();

            string state = ControlHelpersStaticFunc.SplitterTreeState(panelPlayfield.Controls[0] as SplitContainer, "",
                (s) => {
                    ExtendedControls.TabStrip ts = s as ExtendedControls.TabStrip;
                    int v = (int)ts.Tag;
                    return v.ToStringInvariant() + "," + (ts.SelectedIndex>=0 ? (int)pids[ts.SelectedIndex] : -1).ToStringInvariant();
                });

            System.Diagnostics.Debug.WriteLine("Split save " + state);
            SQLiteConnectionUser.PutSettingString(DbWindows, state);

            panelPlayfield.Controls[0].RunActionOn(typeof(ExtendedControls.TabStrip), (c) => 
            {
                var ts = c as ExtendedControls.TabStrip;
                var uccb = ts.CurrentControl as UserControlCommonBase;
                uccb.Closing();
            });
        }

        public override void ChangeCursorType(IHistoryCursor thc)     // a grid below changed its travel grid, update our history one
        {
            bool changedinuse = Object.ReferenceEquals(ucursor_inuse, ucursor_history);   // if we are using the history as the current tg
            System.Diagnostics.Debug.WriteLine("Splitter CTG " + ucursor_history.GetHashCode() + " IU " + ucursor_inuse.GetHashCode() + " New " + thc.GetHashCode());
            ucursor_history = thc;         // underlying one has changed. 

            if (changedinuse)   // inform the boys
            {
                ucursor_inuse = ucursor_history;

                panelPlayfield.Controls[0].RunActionOn(typeof(ExtendedControls.TabStrip), (c) =>        // tell the children!
                {
                    var ts = c as ExtendedControls.TabStrip;
                    var uccb = ts.CurrentControl as UserControlCommonBase;
                    uccb.ChangeCursorType(ucursor_inuse);
                    System.Diagnostics.Debug.WriteLine("Change cursor call to " + ts.Tag + ts.Name);
                });
            }
        }

        private void AssignTHC()
        {
            UserControlCommonBase v = null;
            panelPlayfield.Controls[0].RunActionOn(typeof(UserControlTravelGrid), (c) => { v = c as UserControlTravelGrid; }); // see if we can find a TG

            if (v == null)
                panelPlayfield.Controls[0].RunActionOn(typeof(UserControlJournalGrid), (c) => { v = c as UserControlJournalGrid; }); // see if we can find a TG

            if (v == null)
                panelPlayfield.Controls[0].RunActionOn(typeof(UserControlStarList), (c) => { v = c as UserControlStarList; }); // see if we can find a TG

            IHistoryCursor uctgfound = (v != null) ? (v as IHistoryCursor) : null;    // if found, set to it

            if ((uctgfound != null && !Object.ReferenceEquals(uctgfound, ucursor_inuse)) ||    // if got one but its not the one currently in use
                 (uctgfound == null && !Object.ReferenceEquals(ucursor_history, ucursor_inuse))    // or not found, but we are not on the history one
                )
            {
                ucursor_inuse = (uctgfound != null) ? uctgfound : ucursor_history;    // select
                System.Diagnostics.Debug.WriteLine("Children of " + this.GetHashCode() + " Change to " + ucursor_inuse.GetHashCode());

                panelPlayfield.Controls[0].RunActionOn(typeof(ExtendedControls.TabStrip), (c) =>        // tell the children!
                {
                    var ts = c as ExtendedControls.TabStrip;
                    var uccb = ts.CurrentControl as UserControlCommonBase;
                    uccb.ChangeCursorType(ucursor_inuse);
                    System.Diagnostics.Debug.WriteLine("Change cursor call to " + ts.Tag + ts.Name);
                });

                ucursor_inuse.FireChangeSelection();       // let the uctg tell the children a change event, so they can refresh
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Children of " + this.GetHashCode() + " Stay on " + ucursor_inuse.GetHashCode());
            }
        }

        #region Clicks

        SplitContainer currentsplitter = null;

        private void Sc_MouseClick(object sender, MouseEventArgs e)
        {
            string state = ControlHelpersStaticFunc.SplitterTreeState(panelPlayfield.Controls[0] as SplitContainer, "",
                (s) => { return "" + (int)s.Tag; });

            System.Diagnostics.Debug.WriteLine(state);

            currentsplitter = sender as SplitContainer;
            bool v = currentsplitter.Orientation == Orientation.Vertical;
            toolStripOrientation.Text = v ? "Change to Horizontal Split" : "Change to Vertical Split";
            toolStripSplitPanel1.Text = v ? "Split Left Panel" : "Split Top Panel";
            toolStripSplitPanel2.Text = v ? "Split Right Panel" : "Split Bottom Panel";
            toolStripMergePanel1.Text = v ? "Merge Left Panel" : "Merge Top Panel";
            toolStripMergePanel2.Text = v ? "Merge Right Panel" : "Merge Bottom Panel";

            toolStripSplitPanel1.Enabled = !(currentsplitter.Panel1.Controls[0] is SplitContainer);
            toolStripSplitPanel2.Enabled = !(currentsplitter.Panel2.Controls[0] is SplitContainer);
            toolStripMergePanel1.Enabled = currentsplitter.Panel1.Controls[0] is SplitContainer;
            toolStripMergePanel2.Enabled = currentsplitter.Panel2.Controls[0] is SplitContainer;
            contextMenuStripSplitter.Show(currentsplitter.PointToScreen(e.Location));
        }

        private void toolStripOrientation_Click(object sender, EventArgs e)
        {
            double sdist = currentsplitter.GetSplitterDistance();
            currentsplitter.Orientation = currentsplitter.Orientation == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;
            currentsplitter.SplitterDistance(sdist);
        }

        private void toolStripSplitPanel1_Click(object sender, EventArgs e)
        {
            SplitContainer sc = MakeSplitContainer(currentsplitter.Orientation == Orientation.Vertical ? Orientation.Horizontal : Orientation.Vertical, (currentsplitter.Name.Substring(3).InvariantParseIntNull() ?? 1000) + 1);
            currentsplitter.Split(0, sc, MakeTabControl(GetFreeTag().ToStringInvariant()));
            panelPlayfield.Controls[0].DumpTree(0);
        }

        private void toolStripSplitPanel2_Click(object sender, EventArgs e)
        {
            SplitContainer sc = MakeSplitContainer(currentsplitter.Orientation == Orientation.Vertical ? Orientation.Horizontal : Orientation.Vertical, (currentsplitter.Name.Substring(3).InvariantParseIntNull() ?? 1000) + 1);
            currentsplitter.Split(1, sc, MakeTabControl(GetFreeTag().ToStringInvariant()));
            panelPlayfield.Controls[0].DumpTree(0);
        }

        private void toolStripMergePanel1_Click(object sender, EventArgs e)
        {
            currentsplitter.Merge(0);
            panelPlayfield.Controls[0].DumpTree(0);
            AssignTHC();
        }

        private void toolStripMergePanel2_Click(object sender, EventArgs e)
        {
            currentsplitter.Merge(1);
            panelPlayfield.Controls[0].DumpTree(0);
            AssignTHC();
        }

        int GetFreeTag()            // find a free tag number in all of the TagStrips around.. Tag holds the number allocated.
        {
            byte[] tagsinuse = new byte[100];      // will be zero
            panelPlayfield.Controls[0].RunActionOn(typeof(ExtendedControls.TabStrip), (c) => { tagsinuse[(int)c.Tag] = 1; });
            int t = Array.FindIndex(tagsinuse, x => x==0);
            return t;
        }

        #endregion

        #region Transparency

        public override Color ColorTransparency { get { return Color.Green; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            this.BackColor = curcol;
            panelPlayfield.BackColor = curcol;

            panelPlayfield.Controls[0].RunActionOn(typeof(ExtendedControls.TabStrip), (c) =>        // tell the children!
            {
                var ts = c as ExtendedControls.TabStrip;
                var uccb = ts.CurrentControl as UserControlCommonBase;
                uccb.SetTransparency(on, curcol);
            });
        }

        #endregion

    }
}
