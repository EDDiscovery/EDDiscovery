/*
 * Copyright © 2016 - 2022 EDDiscovery development team
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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlContainerSplitter: UserControlCommonBase        // circular, huh! neat!
    {
        public UserControlTravelGrid GetTravelGrid { get { return GetUserControl<UserControlTravelGrid>(PanelInformation.PanelIDs.TravelGrid); } }

        public T GetUserControl<T>(PanelInformation.PanelIDs p) where T : class
        {
            T v = default(T);
            panelPlayfield?.Controls[0]?.RunActionOnTree((c) => c is UserControlCommonBase && ((UserControlCommonBase)c).PanelID == p, (c) => { v = c as T; }); // see if we can find
            return v;
        }
        public UserControlCommonBase GetUserControl(PanelInformation.PanelIDs p)
        {
            UserControlCommonBase v = null;
            panelPlayfield?.Controls[0]?.RunActionOnTree((c) => c is UserControlCommonBase && ((UserControlCommonBase)c).PanelID == p, (c) => { v = c as UserControlCommonBase; }); // see if we can find
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

        public string dbWindows = "Windows";

        const int FixedPanelOffsetLow = 1000;        // panel IDs, 1000-1999 are fixed windows, 0-999 are embedded in tab strips
        const int FixedPanelOffsetHigh = 1999;        
        const int NoTabPanelSelected = -1;          // -1 is no tab selected

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
            DBBaseName = "SplitterControl";
        
            TabListSortAlpha = EDDConfig.Instance.SortPanelsByName;     // held constant, because we store in each tab splitter the implicit order, can't change it half way thru

            string defaultview = "H(0.50, U'0,-1', U'1,-1')";       // default is a splitter without any selected panels

            string splitctrl = GetSetting(dbWindows, defaultview);

            //System.Diagnostics.Debug.WriteLine("Init " + displaynumber + " " + splitctrl);

            SuspendLayout();

            //try and make the configured splitter tree
            SplitContainer sp = ControlHelpersStaticFunc.SplitterTreeMakeFromCtrlString(new BaseUtils.StringParser(splitctrl), MakeSplitContainer, MakeNode, 0);

            if (sp == null)       // string is screwed, nothing was returned.  Lets set the default up
                sp = ControlHelpersStaticFunc.SplitterTreeMakeFromCtrlString(new BaseUtils.StringParser(defaultview), MakeSplitContainer, MakeNode, 0);

            panelPlayfield.Controls.Add(sp);

            ResumeLayout();

            var enumlistcms = new Enum[] { EDTx.UserControlContainerSplitter_sizeRatioToolStripMenuItem };
            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStripSplitter, enumlistcms, this);

            RunActionOnSplitterTree((p, c, uccb) =>
            {
                int tagid = (int)c.Tag;
                int displaynumber = DisplayNumberOfSplitter(tagid);                         // tab strip - use tag to remember display id which helps us save context.
                System.Diagnostics.Trace.WriteLine($"Splitter Make {uccb.PanelID} tag {tagid} dno {displaynumber}");

                uccb.Init(DiscoveryForm, displaynumber);
            });

            DiscoveryForm.OnPanelAdded += PanelAdded;
            DiscoveryForm.OnPanelRemoved += PanelAdded;

            // contract states the PanelAndPopOuts OR the MajorTabControl will now theme and size it.
        }

        public override void LoadLayout()           // cursor now set up, initial setup complete..
        {
            string splitctrl = GetSetting(dbWindows, "");

            //System.Diagnostics.Debug.WriteLine("Layout loading " + displaynumber + " " + splitctrl);
            //panelPlayfield.Controls[0].DumpTree(0);

            RunActionOnSplitterTree((p, c, uccb) =>     // now, at load layout, do the rest of the UCCB contract.
            {
                uccb.LoadLayout();
                uccb.InitialDisplay();
            });

            Invalidate(true);
            Update();        // need this to FORCE a full refresh in case there are lots of windows
        }

        public override bool AllowClose()                  // splitter is closing, does the consistuent panels allow close?
        {
            bool closeok = true;
            RunActionOnSplitterTree((p, c, uccb) =>        // check for close, uccb is set otherwise not called
            {
                if (uccb.AllowClose() == false)
                    closeok = false;
            });

            return closeok;
        }

        public override void Closing()
        {
            //System.Diagnostics.Debug.WriteLine("Closing splitter " + displaynumber);

            SplitContainer sc = (SplitContainer)panelPlayfield.Controls[0];

            string state = ControlHelpersStaticFunc.SplitterTreeState(sc, "",
                (c) =>          // S is either a TabStrip, or a direct UCCB. See the 
                {
                    ExtendedControls.TabStrip ts = c as ExtendedControls.TabStrip;
                    int tagid = (int)c.Tag;
                    if (ts != null)       // if tab strip..
                    {
                        // pick out the uccb, if currentcontrol is null it will be null. Then using that pick out the panelid from it, which is the definitive one used to create it
                        UserControlCommonBase uccb = ((c as ExtendedControls.TabStrip).CurrentControl) as UserControlCommonBase;
                        return tagid.ToStringInvariant() + "," + (uccb != null ? (int)uccb.PanelID : NoTabPanelSelected).ToStringInvariant();
                    }
                    else
                    {
                        UserControlCommonBase uccb = c as UserControlCommonBase;
                        return tagid.ToStringInvariant() + "," + (FixedPanelOffsetLow + ((int)uccb.PanelID)).ToStringInvariant();
                    }
                });

            //System.Diagnostics.Debug.WriteLine("Split save " + state);
            PutSetting(dbWindows, state);

            (panelPlayfield.Controls[0] as SplitContainer).RunActionOnSplitterTree((p, c) =>        // runs on each split panel node exactly..
            {
                UserControlCommonBase uccb = ((c is ExtendedControls.TabStrip) ? ((c as ExtendedControls.TabStrip).CurrentControl) : c) as UserControlCommonBase;
                if (uccb != null)     // tab strip may not have a control set..
                {
                    uccb.CloseDown();
                    System.Diagnostics.Trace.WriteLine($"Splitter Close {uccb.PanelID} dno {DisplayNumber}");
                }
            });

            DiscoveryForm.OnPanelAdded -= PanelAdded;
            DiscoveryForm.OnPanelRemoved -= PanelAdded;

        }

        public void PanelAdded(PanelInformation.PanelIDs p)
        {
            RunActionOnSplitterTree((sp, c, uccb) =>
            {
                ExtendedControls.TabStrip tabstrip = c as ExtendedControls.TabStrip;
                if (tabstrip != null)       // reset the tab strip list. This will affect the order of SelectedIndex, but won't cause a change in panel type.
                {
                    var list = PanelInformation.GetUserSelectablePanelInfo(TabListSortAlpha, true);

                    tabstrip.ImageList = list.Select(x => x.TabIcon).ToArray();
                    tabstrip.TextList = list.Select(x => x.Description).ToArray();
                    tabstrip.TagList = list.Select(x => (object)x.PopoutID).ToArray();
                    tabstrip.ListSelectionItemSeparators = PanelInformation.GetUserSelectableSeperatorIndex(TabListSortAlpha, true);
                }
            });
        }

        public override UserControlCommonBase Find(PanelInformation.PanelIDs p)              // find UCCB of this type in
        {
            UserControlCommonBase found = null;

            RunActionOnSplitterTree( (sp,c,uccb)=> { var f = uccb.Find(p); if (found == null && f != null) found = f; });       // run this action on all UCCB in splitter
            return found;
        }

        private void RunActionOnSplitterTree(Action<SplitterPanel, Control, UserControlCommonBase> action)
        {
            (panelPlayfield.Controls[0] as SplitContainer).RunActionOnSplitterTree((p, c) =>        // runs on each split panel node exactly, does not run if splitter is empty.
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

            if (panelid >= FixedPanelOffsetLow && panelid <= FixedPanelOffsetHigh)           // this range of ids are UCCB directly in the splitter, so are not changeable
            {
                PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByPanelID((PanelInformation.PanelIDs)(panelid - FixedPanelOffsetLow));
                if (pi == null)
                    pi = PanelInformation.GetPanelInfoByPanelID(PanelInformation.PanelIDs.Log);      // make sure we have a valid one - can't return nothing

                UserControlCommonBase uccb = PanelInformation.Create(pi.PopoutID);      // must return as we made sure pi is valid
                uccb.AutoScaleMode = AutoScaleMode.Inherit;     // very very important and took 2 days to work out! UCCB, as per major tab control, should be in inherit mode
                uccb.Dock = DockStyle.Fill;
                uccb.Tag = tagid;
                uccb.Name = "UC-" + tagid.ToStringInvariant();
                uccb.RequestPanelOperation += SplitterRequestAction;

                return uccb;
            }
            else                        // positive ones are tab strip with the panel id selected, if valid..
            {
                ExtendedControls.TabStrip tabstrip = new ExtendedControls.TabStrip();

                var list = PanelInformation.GetUserSelectablePanelInfo(TabListSortAlpha, true);
                tabstrip.ImageList = list.Select(x => x.TabIcon).ToArray();
                tabstrip.TextList = list.Select(x => x.Description).ToArray(); 
                tabstrip.TagList = list.Select(x => (object)x.PopoutID).ToArray();
                tabstrip.ListSelectionItemSeparators = PanelInformation.GetUserSelectableSeperatorIndex(TabListSortAlpha,true);

                tabstrip.Dock = DockStyle.Fill;
                tabstrip.StripMode = ExtendedControls.TabStrip.StripModeType.ListSelection;
                tabstrip.DropDownFitImagesToItemHeight = true;
                tabstrip.ThemeColorSet = 0;     // use theme from tab strip

                tabstrip.Tag = tagid;                         // Tag stores the ID index of this view
                tabstrip.Name = Name + "." + tagid.ToStringInvariant();

                // we need to theme the tabstrip since we added it..

                tabstrip.Theme(ExtendedControls.Theme.Current, ExtendedControls.Theme.Current.GetFont);

                //System.Diagnostics.Debug.WriteLine("Make new tab control " + tabstrip.Name + " id "  + tagid + " of " + panelid );

                tabstrip.AllowClose += (tab, index, ctrl) =>
                {
                    UserControlCommonBase uccb = ctrl as UserControlCommonBase;
                    return uccb.AllowClose();
                };

                tabstrip.OnRemoving += (tab, ctrl) =>
                {
                    UserControlCommonBase uccb = ctrl as UserControlCommonBase;
                    uccb.CloseDown();
                };

                tabstrip.OnCreateTab += (tab, si) =>        // called when the tab strip wants a new control for a tab. 
                {
                    PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByPanelID((PanelInformation.PanelIDs)tabstrip.TagList[si]);  // must be valid, as it came from the taglist
                    Control c = PanelInformation.Create(pi.PopoutID);
                    var uccb = (c as UserControlCommonBase);
                    uccb.AutoScaleMode = AutoScaleMode.Inherit;     // must be in inherit mode to prevent multi scaling
                    c.Name = pi.WindowTitle;        // tabs uses Name field for display, must set it
                    tab.HelpAction = (pt) => { EDDHelp.Help(this.FindForm(), pt,uccb.HelpKeyOrAddress()); };

                    System.Diagnostics.Trace.WriteLine("SplitterCreate Tab " + c.Name );

                    uccb.RequestPanelOperation += SplitterRequestAction;

                    return c;
                };

                tabstrip.OnPostCreateTab += (tab, ctrl, i) =>       // only called during dynamic creation..
                {
                    int tabstripid = (int)tab.Tag;       // tag from tab strip
                    int displaynumber = DisplayNumberOfSplitter(tabstripid);                         // tab strip - use tag to remember display id which helps us save context.
                    UserControlCommonBase uccb = ctrl as UserControlCommonBase;

                    if (uccb != null)
                    {
                        System.Diagnostics.Trace.WriteLine("Splitter Make Tab " + tabstripid + " with dno " + displaynumber );
                        uccb.Init(DiscoveryForm, displaynumber);      // init..

                        var scale = this.FindForm().CurrentAutoScaleFactor();
                        System.Diagnostics.Trace.WriteLine($"Splitter apply scaling to {uccb.Name} {scale}");
                        uccb.Scale(scale);       // keeping to the contract, scale and theme the uc. between init and set cursor

                        ExtendedControls.Theme.Current.ApplyStd(uccb);

                        uccb.LoadLayout();
                        uccb.InitialDisplay();
                    }
                };

                tabstrip.OnPopOut += (tab, i) => { DiscoveryForm.PopOuts.PopOut((PanelInformation.PanelIDs)tabstrip.TagList[i]); };

                var pids = PanelInformation.GetUserSelectablePanelInfo(TabListSortAlpha, true).Select(x=>x.PopoutID).ToArray();

                int indexofentry = Array.FindIndex(pids, x => x == (PanelInformation.PanelIDs)panelid);      // find ID in array..  -1 if not valid ID, it copes with -1

                if (indexofentry >= 0)       // if we have a panel, open it
                {
                    tabstrip.Create(indexofentry);       // create but not post create during the init phase. Post create is only used during dynamics
                }

                return tabstrip;
            }
        }

        private SplitContainer MakeSplitContainer(Orientation ori, int lv)
        {
            SplitContainer sc = new SplitContainer() { Orientation = ori, Width = 1000, Height = 1000 };    // set width big to provide some res to splitter dist
            sc.Name = "SplitContainer" + lv.ToString() + ori.ToString();

            sc.Dock = DockStyle.Fill;
            sc.FixedPanel = FixedPanel.None;    // indicate to scale splitter distance on resize of control
            sc.MouseClick += Sc_MouseClick;
            sc.Name = "SC-" + lv;
            sc.Controls[0].Name = lv + "-P1";       // names used for debugging this complicated beast!
            sc.Controls[1].Name = lv + "-P2";
            toolTip.SetToolTip(sc, "Right click on splitter bar to change orientation\nor split or merge panels".T(EDTx.UserControlContainerSplitter_RC));
            return sc;
        }

        // called by the panels to do something - pass to siblings, and then work out if we should pass upwards
        // a panel may claim the event, in which case its not sent up
        private PanelActionState SplitterRequestAction(UserControlCommonBase sender, object actionobj)
        {
            //System.Diagnostics.Debug.WriteLine($"Splitter {DisplayNumber} request action {actionobj}");

            PanelActionState retstate = PanelActionState.NotHandled;

            RunActionOnSplitterTree((sp, c, uccb) =>    // reflect to us first
            {
                if (uccb != null && uccb != sender && !IsPASResult(retstate) )   // make sure we have one, and don't send to sender, and we are not done (claimed)
                {
                    //System.Diagnostics.Debug.WriteLine($"...splitter {uccb.PanelID} perform operation {actionobj}");

                    var state = uccb.PerformPanelOperation(sender, actionobj);

                    //System.Diagnostics.Debug.WriteLine($"...splitter {uccb.PanelID} perform operation {actionobj} result {state}");

                    if (state != PanelActionState.NotHandled)       // if not handled, retstate is set, either to HandledContinue (and we cont processing) or to a result
                    {
                        retstate = state;
                        if (IsPASResult(state))
                        {
                            //System.Diagnostics.Debug.WriteLine($"...splitter {uccb.PanelID} claimed call {actionobj} result {state}");
                        }
                    }
                }
            });

            if (!IsPASResult(retstate))     // if not claimed, pass up
            {
                //System.Diagnostics.Debug.WriteLine($".. splitter no claim on {actionobj}, pass on up the chain");
                return RequestPanelOperation.Invoke(sender, actionobj);     // No one claimed it, so pass it up the chain
            }
            else
                return retstate;
        }

        // called from above for us to do something, work out if we should pass it down
        // we don't pass up some travel grid stuff if we have a travel grid ourselves
        // sender can't be us since we are being called from above.
        public override PanelActionState PerformPanelOperation(UserControlCommonBase sender, object actionobj)
        {
            //System.Diagnostics.Debug.WriteLine($"Splitter {DisplayNumber} perform action {actionobj}");

            if (IsOperationHistoryPush(actionobj) && GetUserControl(PanelInformation.PanelIDs.TravelGrid)!=null)
            {
                //System.Diagnostics.Debug.WriteLine($".. blocked because we have a TH in the splitter for {actionobj}");
                return PanelActionState.NotHandled;
            }

            PanelActionState retstate = PanelActionState.NotHandled;

            RunActionOnSplitterTree((sp, c, uccb) =>    
            {
                if (uccb != null && !IsPASResult(retstate)) // if valid, and we have not stopped distributing because someone has claimed it
                {
                    //System.Diagnostics.Debug.WriteLine($"...splitter perform action from above {uccb.PanelID} perform operation {actionobj}");
                    var state = uccb.PerformPanelOperation(sender, actionobj);
                    //System.Diagnostics.Debug.WriteLine($"...splitter perform action from above {uccb.PanelID} perform operation {actionobj} result {state}");

                    if (IsPASResult(state))
                    {
                        retstate = state;
                        //System.Diagnostics.Debug.WriteLine($".. uccb {uccb.PanelID} claimed this operation {actionobj} {state}");
                    }
                }
            });

            return retstate;
        }


        #region Clicks

        SplitContainer currentsplitter = null;
        
        private void Sc_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                currentsplitter = sender as SplitContainer;
                bool v = currentsplitter.Orientation == Orientation.Vertical;
                toolStripOrientation.Text = v ? "Change to Horizontal Split".T(EDTx.UserControlContainerSplitter_ChangetoHorizontalSplit) : "Change to Vertical Split".T(EDTx.UserControlContainerSplitter_ChangetoVerticalSplit);
                toolStripSplitPanel1.Text = v ? "Split Left Panel".T(EDTx.UserControlContainerSplitter_SplitLeftPanel) : "Split Top Panel".T(EDTx.UserControlContainerSplitter_SplitTopPanel);
                toolStripSplitPanel2.Text = v ? "Split Right Panel".T(EDTx.UserControlContainerSplitter_SplitRightPanel) : "Split Bottom Panel".T(EDTx.UserControlContainerSplitter_SplitBottomPanel);
                toolStripMergePanel1.Text = v ? "Merge Left Panel".T(EDTx.UserControlContainerSplitter_MergeLeftPanel) : "Merge Top Panel".T(EDTx.UserControlContainerSplitter_MergeTopPanel);
                toolStripMergePanel2.Text = v ? "Merge Right Panel".T(EDTx.UserControlContainerSplitter_MergeRightPanel) : "Merge Bottom Panel".T(EDTx.UserControlContainerSplitter_MergeBottomPanel);
                
                toolStripSplitPanel1.Enabled = !(currentsplitter.Panel1.Controls[0] is SplitContainer);
                toolStripSplitPanel2.Enabled = !(currentsplitter.Panel2.Controls[0] is SplitContainer);
                toolStripMergePanel1.Enabled = currentsplitter.Panel1.Controls[0] is SplitContainer;
                toolStripMergePanel2.Enabled = currentsplitter.Panel2.Controls[0] is SplitContainer;
                contextMenuStripSplitter.Font = this.Font;
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

        private void Merge(int panel)       // Merge a panel, which involves closing one, so we need to check
        {
            SplitterPanel sp = panel == 0 ? currentsplitter.Panel1 : currentsplitter.Panel2;    // fix 3443 by looking it up

            SplitContainer insidesplitter = (SplitContainer)sp.Controls[0];  // get that split container in the panel

            UserControlCommonBase uccb = insidesplitter.Panel2.Controls[0] as UserControlCommonBase;        // the panel may an embedded direct uccb.. 
            if ( uccb == null )
            {
                ExtendedControls.TabStrip tabstrip = insidesplitter.Panel2.Controls[0] as ExtendedControls.TabStrip;    // it may contain a tabstrip...
                if (tabstrip != null)
                    uccb = tabstrip.CurrentControl as UserControlCommonBase;    // if its a tabstrip, it may contain a UCCB, or it may be empty..
                                                                                // if its not a uccb, its a split container, so null is fine
            }

            if (uccb?.AllowClose() ?? true)      // check if can close, if null, we can
            {
                currentsplitter.Merge(panel);
            }
        }

        private void toolStripMergePanel1_Click(object sender, EventArgs e)
        {
            Merge(0);
        }

        private void toolStripMergePanel2_Click(object sender, EventArgs e)
        {
            Merge(1);
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

        public override bool SupportTransparency { get { return true; } }

        public override void SetTransparency(bool on, Color curcol)
        {
            //System.Diagnostics.Debug.WriteLine("Splitter panel to tx " + on + " " + curcol);

            this.BackColor = curcol;
            panelPlayfield.BackColor = curcol;

            (panelPlayfield.Controls[0] as SplitContainer).RunActionOnSplitterTree((p, c) =>        // runs on each split panel node exactly..
            {
                ExtendedControls.TabStrip ts = c as ExtendedControls.TabStrip;
                ts.PaintTransparentColor = on ? curcol : Color.Transparent;     // enable/disable transparency for tab strip top border

                UserControlCommonBase uccb = (ts != null ? (ts.CurrentControl) : c) as UserControlCommonBase;
                if (uccb != null)     // tab strip may not have a control set..
                {
                    uccb.SetTransparency(on, curcol); 
                }
            });

        }

        #endregion

        #region Default

        public static void CheckPrimarySplitterControlSettings(bool reset )
        {
            string primarycontrolname = EDDProfiles.Instance.UserControlsPrefix + "SplitterControlWindows";                   // primary name for first splitter

            string splitctrl = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting(primarycontrolname, "");

            if (splitctrl == "" || !splitctrl.Contains("'0,1006'") || reset)   // never set, or wiped, or does not have TG in it or reset
            {
                string ctrl = "V(0.75, H(0.6, U'0,1006',U'1,9')," +
                                "H(0.5, U'2,16', " +
                                "H(0.25,U'3,1',U'4,0')) )";

                EliteDangerousCore.DB.UserDatabase.Instance.PutSetting(primarycontrolname, ctrl);
            }
        }

        #endregion 

    }
}
