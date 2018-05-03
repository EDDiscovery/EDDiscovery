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

        public UserControlTravelGrid GetTravelGrid
        {
            get
            {                  // special - have we got a travel grid.. find first.
                UserControlTravelGrid v = null;
                panelPlayfield?.Controls[0]?.RunActionOnTree((c) => c.GetType() == typeof(UserControlTravelGrid), (c) => { v = c as UserControlTravelGrid; }); // see if we can find a TG
                return v;
            }
        }

        public string DbWindows { get { return "SplitterControlWindows" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        const int FixedPanelOffset = 1000;        // panel IDs, 1000+ are fixed windows, 0-999 are embedded in tab strips
        const int NoTabPanelSelected = -1;        // -1 is no tab selected

        public UserControlContainerSplitter()
        {
            InitializeComponent();
        }

        // no cursor..  
        // Since splitter is used as the primary history window, we need to be more careful due to this being used as the primary cursor
        // so we by design don't have a cursor at Init on any UCCB.  So we must create everything here for the primary, then we have a cursor, and
        // then load layout can finish the job

        public override void Init()     
        {
            string defaultview = "H(0.50, U'0,-1', U'1,-1')";       // default is a splitter without any selected panels

            string splitctrl = SQLiteConnectionUser.GetSettingString(DbWindows, defaultview);
            System.Diagnostics.Debug.WriteLine("Split ctrl" + splitctrl);
            SuspendLayout();

            // try and make the configured splitter tree
            SplitContainer sp = ControlHelpersStaticFunc.SplitterTreeMakeFromCtrlString(new BaseUtils.StringParser(splitctrl), MakeSplitContainer, MakeNode, 0);

            if (sp == null)       // string is screwed, nothing was returned.  Lets set the default up
                sp = ControlHelpersStaticFunc.SplitterTreeMakeFromCtrlString(new BaseUtils.StringParser(defaultview), MakeSplitContainer, MakeNode, 0);

            panelPlayfield.Controls.Add(sp);

            //panelPlayfield.Controls[0].DumpTree(0);

            ResumeLayout();
        }


        public override void LoadLayout()           // cursor now set up, initial setup complete..
        {
            ucursor_history = uctg;                 // record base one
            ucursor_inuse = FindTHC() ?? ucursor_history; // if we have a THC, use it, else use the history one

            panelPlayfield.Controls[0].RunActionOnTree((c) => c is UserControlCommonBase, // all UCCB.
                (c) =>
                {
                    var uccb = c as UserControlCommonBase;
                    int tagid = (int)uccb.Tag;
                    int displaynumber = DisplayNumberOfGridInstance(tagid);                         // tab strip - use tag to remember display id which helps us save context.
                    System.Diagnostics.Debug.WriteLine("Make UCCB " + uccb.GetType().Name + " tag " + tagid + " dno " + displaynumber );

                    uccb.Init(discoveryform, displaynumber);
                    uccb.SetCursor(ucursor_inuse);
                    uccb.LoadLayout();
                    discoveryform.theme.ApplyToControls(uccb);
                    uccb.InitialDisplay();
                });

            Invalidate(true);
            Update();        // need this to FORCE a full refresh in case there are lots of windows
        }

        public override void Closing()
        {
            PanelInformation.PanelIDs[] pids = PanelInformation.GetPanelIDs();

            string state = ControlHelpersStaticFunc.SplitterTreeState(panelPlayfield.Controls[0] as SplitContainer, "",
                (s) =>          // S is either a TabStrip, or a direct UCCB. See the 
                {
                    ExtendedControls.TabStrip ts = s as ExtendedControls.TabStrip;
                    if ( ts != null )       // if tab strip..
                        return ((int)s.Tag).ToStringInvariant() + "," + (ts.SelectedIndex >= 0 ? (int)pids[ts.SelectedIndex] : NoTabPanelSelected).ToStringInvariant();
                    else
                    {
                        PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByType(s.GetType());       // must return, as it must be one of the UCCB types
                        return ((int)s.Tag).ToStringInvariant() + "," + (FixedPanelOffset+((int)pi.PopoutID)).ToStringInvariant();
                    }
                });

            System.Diagnostics.Debug.WriteLine("Split save " + state);
            SQLiteConnectionUser.PutSettingString(DbWindows, state);

            panelPlayfield.Controls[0].RunActionOnTree((c) => c is UserControlCommonBase, // all UCCB, either direct or in tab strips
                (c) =>
                {
                    var uccb = c as UserControlCommonBase;      // offer the chance for each UCCB to close..
                    uccb.Closing();
                    System.Diagnostics.Debug.WriteLine("Closing " + c.Name + " " + c.GetType().Name);
                } );
        }


        private Control MakeNode(string s)
        {
            BaseUtils.StringParser sp = new BaseUtils.StringParser(s);      // ctrl string is tag,panelid enum number
            int tagid = sp.NextInt(",") ?? 0;      // enum id
            sp.IsCharMoveOn(',');
            int panelid = sp.NextInt(",") ?? NoTabPanelSelected;  // if not valid, we get an empty tab control

            if (panelid >= FixedPanelOffset)           // this range of ids are UCCB directly in the splitter, so are not changeable
            {
                PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByPanelID((PanelInformation.PanelIDs)(panelid - FixedPanelOffset));
                if (pi == null)
                    pi = PanelInformation.GetPanelInfoByPanelID(PanelInformation.PanelIDs.Log);      // make sure we have a valid one - can't return nothing

                UserControlCommonBase uccb = PanelInformation.Create(pi.PopoutID);      // must return as we made sure pi is valid
                uccb.Dock = DockStyle.Fill;

                uccb.Tag = tagid;
                uccb.Name = "UC-" + tagid.ToStringInvariant();

                return uccb;
            }
            else                        // positive ones are tab strip with the panel id selected, if valid..
            {
                ExtendedControls.TabStrip tabstrip = new ExtendedControls.TabStrip();
                tabstrip.ImageList = PanelInformation.GetPanelImages();
                tabstrip.TextList = PanelInformation.GetPanelDescriptions();
                tabstrip.TagList = PanelInformation.GetPanelIDs().Cast<Object>().ToArray();
                tabstrip.Dock = DockStyle.Fill;
                tabstrip.DropDownWidth = 500;
                tabstrip.DropDownHeight = 500;
                tabstrip.StripMode = ExtendedControls.TabStrip.StripModeType.ListSelection;

                tabstrip.Tag = tagid;                               // Tag stores the ID index of this view

                System.Diagnostics.Debug.WriteLine("Make new tab control " + tagid + " of " + panelid);

                tabstrip.OnRemoving += (tab, ctrl) =>
                {
                    UserControlCommonBase uccb = ctrl as UserControlCommonBase;
                    uccb.Closing();
                    AssignTHC();        // in case we removed anything
                };

                tabstrip.OnCreateTab += (tab, si) =>
                {
                    PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByPanelID((PanelInformation.PanelIDs)tab.TagList[si]);  // must be valid, as it came from the taglist
                    Control c = PanelInformation.Create(pi.PopoutID);
                    c.Name = pi.WindowTitle;        // tabs uses Name field for display, must set it

                    //TBD discoveryform.ActionRun(Actions.ActionEventEDList.onPanelChange, null,
                    //    new Conditions.ConditionVariables(new string[] { "PanelTabName", pi.WindowRefName, "PanelTabTitle", pi.WindowTitle, "PanelName", t.Name }));

                    return c;
                };

                tabstrip.OnPostCreateTab += (tab, ctrl, i) =>       // only called during dynamic creation..
                {
                    int displaynumber = DisplayNumberOfGridInstance((int)tab.Tag);                         // tab strip - use tag to remember display id which helps us save context.
                    UserControlCommonBase uc = ctrl as UserControlCommonBase;

                    if (uc != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Make Tab " + tab.Tag + " with dno " + displaynumber + " Use THC " + ucursor_inuse.GetHashCode());
                        uc.Init(discoveryform, displaynumber);
                        uc.SetCursor(ucursor_inuse);
                        uc.LoadLayout();
                        uc.InitialDisplay();
                    }

                    //System.Diagnostics.Debug.WriteLine("And theme {0}", i);
                    discoveryform.theme.ApplyToControls(tab);
                    AssignTHC();        // in case we added one
                };

                tabstrip.OnPopOut += (tab, i) => { discoveryform.PopOuts.PopOut((PanelInformation.PanelIDs)tabstrip.TagList[i]); };

                discoveryform.theme.ApplyToControls(tabstrip, applytothis: true);

                PanelInformation.PanelIDs[] pids = PanelInformation.GetPanelIDs();

                panelid = Array.FindIndex(pids, x => x == (PanelInformation.PanelIDs)panelid);      // find ID in array..  -1 if not valid ID, it copes with -1

                if (panelid >= 0)
                {
                    tabstrip.Create(panelid);       // create but not post create yet...
                    tabstrip.CurrentControl.Tag = tagid;        // tags stored in here as well so we can get to it..
                }

                return tabstrip;
            }
        }

        private SplitContainer MakeSplitContainer(Orientation ori, int lv)
        {
            SplitContainer sc = new SplitContainer() { Orientation = ori, Width = 1000, Height = 1000 };    // set width big to provide some res to splitter dist
            sc.Dock = DockStyle.Fill;
            sc.FixedPanel = FixedPanel.None;    // indicate to scale splitter distance on resize of control
            sc.MouseClick += Sc_MouseClick;
            sc.Name = "SC-" + lv;
            sc.Controls[0].Name = lv + "-P1";       // names used for debugging this complicated beast!
            sc.Controls[1].Name = lv + "-P2";
            return sc;
        }


        public override void ChangeCursorType(IHistoryCursor thc)     // a grid below changed its travel grid, update our history one
        {
            bool changedinuse = Object.ReferenceEquals(ucursor_inuse, ucursor_history);   // if we are using the history as the current tg
            System.Diagnostics.Debug.WriteLine("Splitter CTG " + ucursor_history.GetHashCode() + " IU " + ucursor_inuse.GetHashCode() + " New " + thc.GetHashCode());
            ucursor_history = thc;         // underlying one has changed. 

            if (changedinuse)   // inform the boys
            {
                ucursor_inuse = ucursor_history;

                panelPlayfield.Controls[0].RunActionOnTree((c) => c is UserControlCommonBase,
                (c) =>
                {
                    var uccb = c as UserControlCommonBase;
                    uccb.ChangeCursorType(ucursor_inuse);
                    System.Diagnostics.Debug.WriteLine("Change cursor call to " + c.Tag + c.Name);
                });
            }
        }

        private IHistoryCursor FindTHC()
        {
            UserControlCommonBase v = null;
            panelPlayfield.Controls[0].RunActionOnTree((c) => c.GetType() == typeof(UserControlTravelGrid), (c) => { v = c as UserControlCommonBase; }); // see if we can find a TG

            if (v == null)
                panelPlayfield.Controls[0].RunActionOnTree((c) => c.GetType() == typeof(UserControlJournalGrid), (c) => { v = c as UserControlCommonBase; }); // see if we can find a TG

            if (v == null)
                panelPlayfield.Controls[0].RunActionOnTree((c) => c.GetType() == typeof(UserControlStarList), (c) => { v = c as UserControlCommonBase; }); // see if we can find a TG

            IHistoryCursor uctgfound = (v != null) ? (v as IHistoryCursor) : null;    // if found, set to it
            return uctgfound;
        }

        private void AssignTHC()
        {
            IHistoryCursor uctgfound = FindTHC();

            if ((uctgfound != null && !Object.ReferenceEquals(uctgfound, ucursor_inuse)) ||    // if got one but its not the one currently in use
                 (uctgfound == null && !Object.ReferenceEquals(ucursor_history, ucursor_inuse))    // or not found, but we are not on the history one
                )
            {
                ucursor_inuse = (uctgfound != null) ? uctgfound : ucursor_history;    // select
                System.Diagnostics.Debug.WriteLine("Children of " + this.GetHashCode() + " Change to " + ucursor_inuse.GetHashCode());

                panelPlayfield.Controls[0].RunActionOnTree((c) => c is UserControlCommonBase,
                (c) =>
                {
                    var uccb = c as UserControlCommonBase;
                    uccb.ChangeCursorType(ucursor_inuse);
                    System.Diagnostics.Debug.WriteLine("Change cursor call to " + c.Tag + c.Name);
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
            currentsplitter.Split(0, sc, MakeNode(GetFreeTag().ToStringInvariant()));
            //panelPlayfield.Controls[0].DumpTree(0);
        }

        private void toolStripSplitPanel2_Click(object sender, EventArgs e)
        {
            SplitContainer sc = MakeSplitContainer(currentsplitter.Orientation == Orientation.Vertical ? Orientation.Horizontal : Orientation.Vertical, (currentsplitter.Name.Substring(3).InvariantParseIntNull() ?? 1000) + 1);
            currentsplitter.Split(1, sc, MakeNode(GetFreeTag().ToStringInvariant()));
            //panelPlayfield.Controls[0].DumpTree(0);
        }

        private void toolStripMergePanel1_Click(object sender, EventArgs e)
        {
            currentsplitter.Merge(0);
            AssignTHC();        // because we may have removed the cursor
            //panelPlayfield.Controls[0].DumpTree(0);
        }

        private void toolStripMergePanel2_Click(object sender, EventArgs e)
        {
            currentsplitter.Merge(1);
            AssignTHC();        // because we may have removed the cursor
            //panelPlayfield.Controls[0].DumpTree(0);
        }

        int GetFreeTag()            // find a free tag number in all of the TagStrips around.. Tag holds the number allocated.
        {
            byte[] tagsinuse = new byte[100];      // will be zero

            // need to isolate either tab strips, or UCCB type of directly under a splitter panel, for their tag ids
            panelPlayfield.Controls[0].RunActionOnTree((c) => (c.GetType() == typeof(ExtendedControls.TabStrip) ||
                                                              (c is UserControlCommonBase && c.Parent.GetType() == typeof(SplitterPanel) )),
                                                       (c) => 
                                                       {
                                                           System.Diagnostics.Debug.WriteLine("Tag lookup on " + c.GetType().Name  +" = " + ((int)c.Tag));
                                                           tagsinuse[(int)c.Tag] = 1;
                                                       } );
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

            panelPlayfield.Controls[0].RunActionOnTree((c) => c is UserControlCommonBase,
            (c) =>
            {
                var uccb = c as UserControlCommonBase;
                uccb.SetTransparency(on, curcol);
            });
        }

        #endregion

    }
}
