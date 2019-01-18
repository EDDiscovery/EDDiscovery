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

        public UserControlTravelGrid GetTravelGrid { get { return GetUserControl<UserControlTravelGrid>(); } }

        public T GetUserControl<T>() where T:class
        {
            T v = default(T);
            panelPlayfield?.Controls[0]?.RunActionOnTree((c) => c.GetType() == typeof(T), (c) => { v = c as T; }); // see if we can find a TG
            return v;
        }

        public ExtendedControls.TabStrip GetTabStrip(string name)       // name is a logical name, map to approx locations
        {
            int tagid = -1; // -1 will make it not find
            if (name.Equals("Bottom", StringComparison.InvariantCultureIgnoreCase))                 // tagids based on default grid IDs on the history window. 
                tagid = 1;                                                                          // if user changes the deployment -tough
            else if (name.Equals("Bottom-Right", StringComparison.InvariantCultureIgnoreCase))
                tagid = 4;
            else if (name.Equals("Middle-Right", StringComparison.InvariantCultureIgnoreCase))
                tagid = 3;
            else if (name.Equals("Top-Right", StringComparison.InvariantCultureIgnoreCase))
                tagid = 2;

            ExtendedControls.TabStrip found = null;
            (panelPlayfield.Controls[0] as SplitContainer).RunActionOnSplitterTree((p, c) =>        // runs on each split panel node exactly..
            {
                if ((int)c.Tag == tagid && c is ExtendedControls.TabStrip)      // if its a tab strip, and tag is right
                    found = c as ExtendedControls.TabStrip;
            });

            return found;
        }

        public string DbWindows { get { return DBName("SplitterControlWindows" ); } }

        const int FixedPanelOffset = 1000;        // panel IDs, 1000+ are fixed windows, 0-999 are embedded in tab strips
        const int NoTabPanelSelected = -1;        // -1 is no tab selected

        private bool TabListSortAlpha;            // constant across run of splitter, even if user changes the alpha sort later

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
            TabListSortAlpha = EDDConfig.Instance.SortPanelsByName;     // held constant, because we store in each tab splitter the implicit order, can't change it half way thru

            string defaultview = "H(0.50, U'0,-1', U'1,-1')";       // default is a splitter without any selected panels

            string splitctrl = SQLiteConnectionUser.GetSettingString(DbWindows, defaultview);

            SuspendLayout();

            // try and make the configured splitter tree
            SplitContainer sp = ControlHelpersStaticFunc.SplitterTreeMakeFromCtrlString(new BaseUtils.StringParser(splitctrl), MakeSplitContainer, MakeNode, 0);

            if (sp == null)       // string is screwed, nothing was returned.  Lets set the default up
                sp = ControlHelpersStaticFunc.SplitterTreeMakeFromCtrlString(new BaseUtils.StringParser(defaultview), MakeSplitContainer, MakeNode, 0);

            panelPlayfield.Controls.Add(sp);

            //panelPlayfield.Controls[0].DumpTree(0);

            ResumeLayout();

            BaseUtils.Translator.Instance.Translate(contextMenuStripSplitter, this);

            RunActionOnSplitterTree((p, c, uccb) =>
            {
                int tagid = (int)c.Tag;
                int displaynumber = DisplayNumberOfSplitter(tagid);                         // tab strip - use tag to remember display id which helps us save context.
                System.Diagnostics.Trace.WriteLine("SP:Make UCCB " + uccb.GetType().Name + " tag " + tagid + " dno " + displaynumber);

                uccb.Init(discoveryform, displaynumber);
            });
        }

        public override void LoadLayout()           // cursor now set up, initial setup complete..
        {
            ucursor_history = uctg;                 // record base one
            ucursor_inuse = FindTHC() ?? ucursor_history; // if we have a THC, use it, else use the history one
            string splitctrl = SQLiteConnectionUser.GetSettingString(DbWindows, "");

            //System.Diagnostics.Debug.WriteLine("--------------------" + splitctrl);
            //panelPlayfield.Controls[0].DumpTree(0);

            RunActionOnSplitterTree((p, c, uccb) =>
            {
                uccb.SetCursor(ucursor_inuse);
                uccb.LoadLayout();
                discoveryform.theme.ApplyToControls(uccb);
                uccb.InitialDisplay();
            });

            Invalidate(true);
            Update();        // need this to FORCE a full refresh in case there are lots of windows

            UserControlTravelGrid tg = GetTravelGrid;                   // if travel grid, link up
            if (tg != null)
                tg.OnKeyDownInCell += Tg_OnKeyDownInCell;
        }

        public override void Closing()
        {
            PanelInformation.PanelIDs[] pids = PanelInformation.GetUserSelectablePanelIDs(TabListSortAlpha);

            string state = ControlHelpersStaticFunc.SplitterTreeState(panelPlayfield.Controls[0] as SplitContainer, "",
                (c) =>          // S is either a TabStrip, or a direct UCCB. See the 
                {
                    ExtendedControls.TabStrip ts = c as ExtendedControls.TabStrip;
                    int tagid = (int)c.Tag;
                    if (ts != null)       // if tab strip..
                    {
                        return tagid.ToStringInvariant() + "," + (ts.SelectedIndex >= 0 ? (int)pids[ts.SelectedIndex] : NoTabPanelSelected).ToStringInvariant();
                    }
                    else
                    {
                        PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByType(c.GetType());       // must return, as it must be one of the UCCB types
                        return tagid.ToStringInvariant() + "," + (FixedPanelOffset + ((int)pi.PopoutID)).ToStringInvariant();
                    }
                });

            //System.Diagnostics.Debug.WriteLine("Split save " + state);
            SQLiteConnectionUser.PutSettingString(DbWindows, state);

            (panelPlayfield.Controls[0] as SplitContainer).RunActionOnSplitterTree((p, c) =>        // runs on each split panel node exactly..
            {
                UserControlCommonBase uccb = ((c is ExtendedControls.TabStrip) ? ((c as ExtendedControls.TabStrip).CurrentControl) : c) as UserControlCommonBase;
                if (uccb != null)     // tab strip may not have a control set..
                {
                    uccb.Closing();
                    //System.Diagnostics.Debug.WriteLine("Closing " + c.Name + " " + c.GetType().Name + " " + uccb.Name);
                }
            });
        }

        private void RunActionOnSplitterTree(Action<SplitterPanel, Control, UserControlCommonBase> action)
        {
            (panelPlayfield.Controls[0] as SplitContainer).RunActionOnSplitterTree((p, c) =>        // runs on each split panel node exactly..
            {
                UserControlCommonBase uccb = ((c is ExtendedControls.TabStrip) ? ((c as ExtendedControls.TabStrip).CurrentControl) : c) as UserControlCommonBase;
                if (uccb != null)     // tab strip may not have a control set..
                {
                    action(p, c, uccb);
                }
            });
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
                tabstrip.ImageList = PanelInformation.GetUserSelectablePanelImages(TabListSortAlpha);
                tabstrip.TextList = PanelInformation.GetUserSelectablePanelDescriptions(TabListSortAlpha);
                tabstrip.TagList = PanelInformation.GetUserSelectablePanelIDs(TabListSortAlpha).Cast<Object>().ToArray();
                tabstrip.ListSelectionItemSeparators = PanelInformation.GetUserSelectableSeperatorIndex(TabListSortAlpha);

                tabstrip.Dock = DockStyle.Fill;
                tabstrip.DropDownWidth = 500;
                tabstrip.DropDownHeight = 500;
                tabstrip.StripMode = ExtendedControls.TabStrip.StripModeType.ListSelection;

                tabstrip.Tag = tagid;                         // Tag stores the ID index of this view
                tabstrip.Name = Name + "." + tagid.ToStringInvariant();

                //System.Diagnostics.Debug.WriteLine("Make new tab control " + tabstrip.Name + " id "  + tagid + " of " + panelid );

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

                    return c;
                };

                tabstrip.OnPostCreateTab += (tab, ctrl, i) =>       // only called during dynamic creation..
                {
                    int tabstripid = (int)tab.Tag;       // tag from tab strip
                    int displaynumber = DisplayNumberOfSplitter(tabstripid);                         // tab strip - use tag to remember display id which helps us save context.
                    UserControlCommonBase uc = ctrl as UserControlCommonBase;

                    if (uc != null)
                    {
                        System.Diagnostics.Trace.WriteLine("SP:Make Tab " + tabstripid + " with dno " + displaynumber + " Use THC " + ucursor_inuse.GetHashCode());
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

                PanelInformation.PanelIDs[] pids = PanelInformation.GetUserSelectablePanelIDs(TabListSortAlpha); // sort order v.important.. we need the right index, dep

                int indexofentry = Array.FindIndex(pids, x => x == (PanelInformation.PanelIDs)panelid);      // find ID in array..  -1 if not valid ID, it copes with -1

                if (indexofentry >= 0)       // if we have a panel, open it
                {
                    tabstrip.Create(indexofentry);       // create but not post create yet...
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
            toolTip.SetToolTip(sc, "Right click on splitter bar to change orientation\nor split or merge panels");
            return sc;
        }
        
        public override void ChangeCursorType(IHistoryCursor thc)     // a grid below changed its travel grid, update our history one
        {
            bool changedinuse = Object.ReferenceEquals(ucursor_inuse, ucursor_history);   // if we are using the history as the current tg
            //System.Diagnostics.Debug.WriteLine("Splitter CTG " + ucursor_history.GetHashCode() + " IU " + ucursor_inuse.GetHashCode() + " New " + thc.GetHashCode());
            ucursor_history = thc;         // underlying one has changed. 

            if (changedinuse)   // inform the boys
            {
                ucursor_inuse = ucursor_history;

                RunActionOnSplitterTree((p, c, uccb) =>
                {
                    uccb.ChangeCursorType(ucursor_inuse);
                    //System.Diagnostics.Debug.WriteLine("Change cursor call to " + c.Name + " " + uccb.Name);
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
                //System.Diagnostics.Debug.WriteLine("Children of " + this.GetHashCode() + " Change to " + ucursor_inuse.GetHashCode());

                RunActionOnSplitterTree((p, c, uccb) =>
                {
                    uccb.ChangeCursorType(ucursor_inuse);
                    //System.Diagnostics.Debug.WriteLine("Change cursor call to " + c.Name + " " + uccb.Name);
                });

                ucursor_inuse.FireChangeSelection();       // let the uctg tell the children a change event, so they can refresh
            }
            else
            {
                //System.Diagnostics.Debug.WriteLine("Children of " + this.GetHashCode() + " Stay on " + ucursor_inuse.GetHashCode());
            }

            UserControlTravelGrid tg = GetTravelGrid;       // this is called whenever deployment changes.. so see if TG is there..
            if (tg != null)
            {
                tg.OnKeyDownInCell -= Tg_OnKeyDownInCell;
                tg.OnKeyDownInCell += Tg_OnKeyDownInCell;
            }

        }

        private void Tg_OnKeyDownInCell(int asciikeycode, int rowno, int colno, bool note)
        {
            if (note)           // links a TG with a system info to allow note taking
            {
                UserControlSysInfo s = GetUserControl<UserControlSysInfo>();
                if (s != null && s.IsNotesShowing)
                {
                    s.FocusOnNote(asciikeycode);
                }
            }
        }

        #region Clicks

        SplitContainer currentsplitter = null;
        
        private void Sc_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                currentsplitter = sender as SplitContainer;
                bool v = currentsplitter.Orientation == Orientation.Vertical;
                toolStripOrientation.Text = v ? "Change to Horizontal Split".Tx(this) : "Change to Vertical Split".Tx(this);
                toolStripSplitPanel1.Text = v ? "Split Left Panel".Tx(this) : "Split Top Panel".Tx(this);
                toolStripSplitPanel2.Text = v ? "Split Right Panel".Tx(this) : "Split Bottom Panel".Tx(this);
                toolStripMergePanel1.Text = v ? "Merge Left Panel".Tx(this) : "Merge Top Panel".Tx(this);
                toolStripMergePanel2.Text = v ? "Merge Right Panel".Tx(this) : "Merge Bottom Panel".Tx(this);
                
                toolStripSplitPanel1.Enabled = !(currentsplitter.Panel1.Controls[0] is SplitContainer);
                toolStripSplitPanel2.Enabled = !(currentsplitter.Panel2.Controls[0] is SplitContainer);
                toolStripMergePanel1.Enabled = currentsplitter.Panel1.Controls[0] is SplitContainer;
                toolStripMergePanel2.Enabled = currentsplitter.Panel2.Controls[0] is SplitContainer;
                contextMenuStripSplitter.Show(currentsplitter.PointToScreen(e.Location));
            }
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
        }

        private void toolStripSplitPanel2_Click(object sender, EventArgs e)
        {
            SplitContainer sc = MakeSplitContainer(currentsplitter.Orientation == Orientation.Vertical ? Orientation.Horizontal : Orientation.Vertical, (currentsplitter.Name.Substring(3).InvariantParseIntNull() ?? 1000) + 1);
            currentsplitter.Split(1, sc, MakeNode(GetFreeTag().ToStringInvariant()));
        }

        private void toolStripMergePanel1_Click(object sender, EventArgs e)
        {
            currentsplitter.Merge(0);
            AssignTHC();        // because we may have removed the cursor
        }

        private void toolStripMergePanel2_Click(object sender, EventArgs e)
        {
            currentsplitter.Merge(1);
            AssignTHC();        // because we may have removed the cursor
        }
        
        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            double psize = currentsplitter.GetPanelsSizeSum() / 4;
            currentsplitter.SplitterDistance = (int)psize;
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            double psize = currentsplitter.GetPanelsSizeSum() / 3;
            currentsplitter.SplitterDistance = (int)psize;
        }

        private void toolStripMenuItem23_Click(object sender, EventArgs e)
        {
            double psize = (currentsplitter.GetPanelsSizeSum() / 5) * 2;
            currentsplitter.SplitterDistance = (int)psize;
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            double psize = currentsplitter.GetPanelsSizeSum() / 2;
            currentsplitter.SplitterDistance = (int)psize;
        }

        private void toolStripMenuItem32_Click(object sender, EventArgs e)
        {
            double psize = (currentsplitter.GetPanelsSizeSum() / 5) * 3;
            currentsplitter.SplitterDistance = (int)psize;
        }

        private void toolStripMenuItem21_Click(object sender, EventArgs e)
        {
            double psize = (currentsplitter.GetPanelsSizeSum() / 3) * 2;
            currentsplitter.SplitterDistance = (int)psize;
        }

        private void toolStripMenuItem31_Click(object sender, EventArgs e)
        {
            double psize = (currentsplitter.GetPanelsSizeSum() / 4) * 3;
            currentsplitter.SplitterDistance = (int)psize;
        }

        int GetFreeTag()            // find a free tag number in all of the TagStrips around.. Tag holds the number allocated.
        {
            byte[] tagsinuse = new byte[100];      // will be zero

            (panelPlayfield.Controls[0] as SplitContainer).RunActionOnSplitterTree((p, c) =>        // runs on each split panel node exactly..
            {
                tagsinuse[(int)c.Tag] = 1;        // tags are stored on the object pointed to by the splitter panel
            });

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

            RunActionOnSplitterTree((p, c, uccb) =>
            {
                uccb.SetTransparency(on, curcol);
            });
        }

        #endregion

        #region Default

        public static void CheckPrimarySplitterControlSettings(string defaultname )
        {
            string primarycontrolname = EDDProfiles.Instance.UserControlsPrefix + "SplitterControlWindows";                   // primary name for first splitter

            string splitctrl = SQLiteConnectionUser.GetSettingString(primarycontrolname, "");

            if (splitctrl == "" || !splitctrl.Contains("'0,1006'"))   // never set, or wiped, or does not have TG in it, reset.. if previous system had the IDs, use them, else use defaults
            {
                int enum_bottom = SQLiteDBClass.GetSettingInt(defaultname + "BottomTab", (int)(PanelInformation.PanelIDs.Scan));
                int enum_bottomright = SQLiteDBClass.GetSettingInt(defaultname + "BottomRightTab", (int)(PanelInformation.PanelIDs.Log));
                int enum_middleright = SQLiteDBClass.GetSettingInt(defaultname + "MiddleRightTab", (int)(PanelInformation.PanelIDs.StarDistance));
                int enum_topright = SQLiteDBClass.GetSettingInt(defaultname + "TopRightTab", (int)(PanelInformation.PanelIDs.SystemInformation));

                string ctrl = "V(0.75, H(0.6, U'0,1006',U'1," + enum_bottom.ToStringInvariant() + "')," +
                                "H(0.5, U'2," + enum_topright.ToStringInvariant() + "', " +
                                "H(0.25,U'3," + enum_middleright.ToStringInvariant() + "',U'4," + enum_bottomright + "')) )";

                SQLiteConnectionUser.PutSettingString(primarycontrolname, ctrl);
            }
        }

        #endregion 
    }
}
