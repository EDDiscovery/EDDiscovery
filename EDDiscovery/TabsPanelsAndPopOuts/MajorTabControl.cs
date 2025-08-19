/*
 * Copyright 2015 - 2025 EDDiscovery development team
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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery
{
    public partial class MajorTabControl : ExtendedControls.ExtTabControl
    {
        #region Public IF

        public UserControls.UserControlContainerSplitter PrimarySplitterTab
        {
            get
            {
                foreach (TabPage p in TabPages)      // all main tabs, load/display
                {
                    if (p.Tag != null)
                        return p.Controls[0] as UserControls.UserControlContainerSplitter;
                }
                return null;
            }
        }

        //EDDiscovery Init and Profiles calls this
        public void CreateTabs(EDDiscoveryForm edf, bool resettabs, string resetsettings)
        {
            eddiscovery = edf;

            int[] panelids;
            int[] displaynumbers;
            int currentlyselectedtab = 0;

            string majortabs = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting(EDDProfiles.Instance.UserControlsPrefix + "MajorTabControlList", "");
            string[] majortabnames = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting(EDDProfiles.Instance.UserControlsPrefix + "MajorTabControlName", "").Replace("!error!", "+").Split(';');       // if its okay, load the name list

            while (true)
            {
                int[] rawtabctrl;
                majortabs.RestoreArrayFromString(out rawtabctrl);       // string is : selectedtab, [ <PanelID>, <displayno> ]..

                panelids = rawtabctrl.Where((value, index) => index % 2 != 0).ToArray();
                displaynumbers = rawtabctrl.Where((value, index) => index > 0 && index % 2 == 0).ToArray();

                if (resettabs || panelids.Length == 0 || panelids.Length != displaynumbers.Length || !panelids.Contains(-1) || !panelids.Contains((int)PanelInformation.PanelIDs.PanelSelector))
                {
                    majortabs = resetsettings;
                    majortabnames = null;
                    resettabs = false;
                }
                else
                {
                    if (rawtabctrl[0] > 0 && rawtabctrl[0] < panelids.Length)
                        currentlyselectedtab = rawtabctrl[0];
                    break;
                }
            }

            for (int i = 0; i < panelids.Length; i++)
            {
                string name = majortabnames != null && i < majortabnames.Length && majortabnames[i].Length > 0 ? majortabnames[i] : null;

                try
                {
                    if (panelids[i] == -1)      // marker indicating the special history tab
                    {
                        CreateTab(PanelInformation.PanelIDs.SplitterControl, name ?? "History", displaynumbers[i], TabPages.Count, true);
                    }
                    else
                    {
                        PanelInformation.PanelIDs p = (PanelInformation.PanelIDs)panelids[i];
                        CreateTab(p, name, displaynumbers[i], TabPages.Count,false);      // no need the theme, will be themed as part of overall load
                    }
                }
                catch (Exception ex)   // paranoia in case something crashes it, unlikely, but we want maximum chance the history tab will show
                {
                    System.Diagnostics.Trace.WriteLine($"Exception caught creating tab {i} ({name}): {ex.ToString()}");
                    MessageBox.Show($"Report to EDD team - Exception caught creating tab {i} ({name}): {ex.ToString()}");
                }
            }

            SelectedIndex = currentlyselectedtab;
        }

        public void LoadTabs()     // called on Loading..
        {
            //foreach (TabPage tp in tabControlMain.TabPages) System.Diagnostics.Debug.WriteLine("TP Size " + tp.Controls[0].DisplayRectangle);

            foreach (TabPage p in TabPages)      // all main tabs, load/display
            {
                // now a strange thing. tab Selected, cause its shown, gets resized (due to repoisition form). Other tabs dont.
                // LoadLayout could fail due to an incorrect size that would break something (such as spitters)..
                // so force size. tried perform layout to no avail
                p.Size = TabPages[SelectedIndex].Size;
                UserControls.UserControlCommonBase uccb = (UserControls.UserControlCommonBase)p.Controls[0];
                uccb.CallLoadLayout();
                uccb.CallInitialDisplay();
            }

            //foreach (TabPage tp in tabControlMain.TabPages) System.Diagnostics.Debug.WriteLine("TP Size " + tp.Controls[0].DisplayRectangle);
        }

        public bool AllowClose()                 // tabs are closing, does all tabs allow close
        {
            foreach (TabPage p in TabPages)
            {
                if (p.Controls.Count > 0)       // should not happen, but just seen it closing with no controls when using the python plugin - double check
                {
                    UserControls.UserControlCommonBase uccb = p.Controls[0] as UserControls.UserControlCommonBase;
                    if (uccb?.AllowClose() == false)
                        return false;
                }
            }
            return true;
        }

        // call closedown on all tabs and save the tab control list
        // this does not dispose of the TAB
        public void CloseSaveTabs()
        {
            List<int> idlist = new List<int>();

            idlist.Add(SelectedIndex);   // first is current index

            string tabnames = "";

            foreach (TabPage p in TabPages)      // all main tabs, close down
            {
                UserControls.UserControlCommonBase uccb = p.Controls[0] as UserControls.UserControlCommonBase;
                uccb.CallCloseDown();
                idlist.Add(Object.ReferenceEquals(uccb, PrimarySplitterTab) ? -1 : (int)uccb.PanelID);      // primary is marked -1
                idlist.Add(uccb.DisplayNumber);
                tabnames += p.Text + ";";
            }

            EliteDangerousCore.DB.UserDatabase.Instance.PutSetting(EDDProfiles.Instance.UserControlsPrefix + "MajorTabControlList", string.Join(",", idlist));
            EliteDangerousCore.DB.UserDatabase.Instance.PutSetting(EDDProfiles.Instance.UserControlsPrefix + "MajorTabControlName", tabnames);
        }

        // Dispose and remove all tabs
        public void DisposeRemoveAllTabs()         
        {
            foreach (TabPage page in TabPages)
            {
                page.Dispose();
            }
        }


        public void AddTab(PanelInformation.PanelIDs id , int tabindex = 0)     // -n is from the end, else before 0,1,2
        {
            if (tabindex < 0)
                tabindex = Math.Max(0,TabCount + tabindex);

            TabPage page = CreateTab(id, null, -1, tabindex,false);

            if (page != null)
            {
                UserControls.UserControlCommonBase uccb = page.Controls[0] as UserControls.UserControlCommonBase;
                uccb.CallLoadLayout();
                uccb.CallInitialDisplay();
                SelectedIndex = tabindex;   // and select the inserted one
            }
        }

        public void HelpOn(Form parent, System.Drawing.Point pt, int tabIndex)
        {
            if (tabIndex >= 0 && tabIndex < TabPages.Count)
            {
                TabPage page = TabPages[tabIndex];
                if (page.Tag != null)
                    EDDHelp.HistoryTab(parent, pt);
                else
                {
                    UserControls.UserControlCommonBase uccb = page.Controls[0] as UserControls.UserControlCommonBase;
                    EDDHelp.Help(parent, pt, uccb.HelpKeyOrAddress());
                }
            }

        }

        public void RenameTab(int tabIndex, string newname)
        {
            if (tabIndex >= 0 && tabIndex < TabPages.Count)
            {
                TabPage page = TabPages[tabIndex];
                page.Text = newname;
            }
        }

        public void RemoveTab(int tabIndex)         // from right click menu
        {
            if (tabIndex >= 0 && tabIndex < TabPages.Count)
            {
                RemoveTab(TabPages[tabIndex]);
            }
        }

        // Remove tab, if allowed, release resources
        public void RemoveTab(TabPage page)         
        {
            UserControls.UserControlCommonBase uccb = page.Controls[0] as UserControls.UserControlCommonBase;

            if (uccb.AllowClose())              // it must allow a close to remove it
            {
                uccb.CallCloseDown();
                page.Dispose();
            }
        }

        // of type P, close
        public void CloseAllTabs(PanelInformation.PanelIDs p)
        {
            List<TabPage> toclose = new List<TabPage>();

            foreach (TabPage tp in TabPages)        // make a close list
            {
                var f = ((UserControls.UserControlCommonBase)tp.Controls[0]).Find(p);
                if (f != null)
                    toclose.Add(tp);
            }

            foreach (var x in toclose)
                RemoveTab(x);         // from right click menu
        }

        public bool SelectPrimarySplitterTab()      // select the primary splitter
        {
            foreach (TabPage p in TabPages)      
            {
                if (p.Tag != null)
                {
                    SelectTab(p);
                    return true;
                }
            }
            return false;
        }

        public TabPage GetMajorTab(PanelInformation.PanelIDs ptype)
        {
            return (from TabPage x in TabPages where ((UserControls.UserControlCommonBase)x.Controls[0]).PanelID == ptype select x).FirstOrDefault();
        }

        // placed it at the end of the tab line, but before the +
        public TabPage EnsureMajorTabIsPresent(PanelInformation.PanelIDs ptype, bool selectit)  
        {
            TabPage page = GetMajorTab(ptype);
            if (page == null)
            {
                page = CreateTab(ptype, null, -1, TabCount>0 ? TabCount-1 : 0,false);

                if (page != null)       // if created..
                {
                    UserControls.UserControlCommonBase uccb = page.Controls[0] as UserControls.UserControlCommonBase;
                    uccb.CallLoadLayout();
                    uccb.CallInitialDisplay();
                }
            }

            if (selectit)
                SelectTab(page);

            return page;
        }

        // find first tab containing UCCB type t.
        public Tuple<TabPage, UserControls.UserControlCommonBase> Find(PanelInformation.PanelIDs p)
        {
            foreach (TabPage tp in TabPages)
            {
                var f = ((UserControls.UserControlCommonBase)tp.Controls[0]).Find(p);
                if (f != null)
                    return new Tuple<TabPage, UserControls.UserControlCommonBase>(tp, f);
            }
            return null;
        }

        #endregion

        #region Implementation

        // Create a tab, this may return null if it fails to create the panel ID (unlikely)

        private TabPage CreateTab(PanelInformation.PanelIDs ptype, string name, int dn, int posindex, bool primary)
        {
            System.Diagnostics.Trace.WriteLine($"\nMajorTabControl create tab {ptype} {name} {posindex}");

            UserControls.UserControlCommonBase uccb = PanelInformation.Create(ptype);   // must create, since its a ptype.
            if (uccb == null)       // if ptype is crap, it returns null.. catch
                return null;

            uccb.AutoScaleMode = AutoScaleMode.Inherit;     // inherit will mean Font autoscale won't happen at attach - very important. we keep control of scaling
            uccb.Dock = System.Windows.Forms.DockStyle.Fill;    // uccb has to be fill, even though the VS designer does not indicate you need to set it.. copied from designer code
            uccb.Location = new System.Drawing.Point(3, 3);

            if (dn == -1) // if work out display number
            {
                // go thru the tabs trying to find another page with the same panelid as ptype

                List<int> idlist = (from TabPage p in TabPages where ((UserControls.UserControlCommonBase)p.Controls[0]).PanelID == ptype select (p.Controls[0] as UserControls.UserControlCommonBase).DisplayNumber).ToList();

                if (!idlist.Contains(UserControls.UserControlCommonBase.DisplayNumberPrimaryTab))
                    dn = UserControls.UserControlCommonBase.DisplayNumberPrimaryTab;
                else
                {   // search for empty id.
                    for (int i = UserControls.UserControlCommonBase.DisplayNumberStartExtraTabs; i <= UserControls.UserControlCommonBase.DisplayNumberStartExtraTabsMax; i++)
                    {
                        if (!idlist.Contains(i))
                        {
                            dn = i;
                            break;
                        }
                    }
                }
            }

            int numoftab = (dn == UserControls.UserControlCommonBase.DisplayNumberPrimaryTab) ? 0 : (dn - UserControls.UserControlCommonBase.DisplayNumberStartExtraTabs + 1);
            if (uccb is UserControls.UserControlContainerSplitter && numoftab > 0)          // so history is a splitter, so first real splitter will be dn=100, adjust for it
                numoftab--;

            string postfix = numoftab == 0 ? "" : "(" + numoftab.ToStringInvariant() + ")";
            string title = name != null ? name : (PanelInformation.GetPanelInfoByPanelID(ptype).WindowTitle + postfix);

            uccb.Name = title;              // for debugging use


            TabPage page = new TabPage(title);
            page.Name = "MajorTabPage " + title;
            page.Location = new System.Drawing.Point(4, 22);    // copied from normal tab creation code
            page.Padding = new System.Windows.Forms.Padding(3); // this is to allow a pad around the sides

            if ( primary )
                page.Tag = true;       // this marks it as the primary tab..

            page.SuspendLayout();

            page.Controls.Add(uccb);

            // because we get called before shown, we may not be created with a windows handle. And Insert fails if so
            // previously the tab control OnFontControl would trigger a creation by accessing a property.
            // lets do it explicitly so we know its needed

            if (!this.IsHandleCreated)      
                CreateHandle();

            TabPages.Insert(posindex, page);        // with inherit above, no font autoscale. It will fail SILENTLY (bastard) if the tabcontrol handle is not there

            if (primary)                            // hook up the request system
                uccb.RequestPanelOperation = RequestPanelOperationPrimaryTab;
            else
                uccb.RequestPanelOperation = (sender, op) => { return RequestPanelOperationSecondaryTab(page,sender, op); };

            if (Environment.OSVersion.Platform != PlatformID.Win32NT && SelectedIndex >= posindex)
            {
                // Mono does not automatically change SelectedIndex to +1 if you at or before it.  So it gets on the wrong tab. Fix it back
                SelectedIndex = SelectedIndex + 1;
            }

            //Init control after it is added to the form
            uccb.CallInit(eddiscovery, dn);    // start the uccb up

            // the standard method is to scale then theme. Order as per the contract in UCCB. Uccb should be in AutoScaleInherit mode
            // done in splitter, grid, popout the same
            // and the uccb must be in AutoScaleMode.Inherit mode so it does end up double scaling

            var scale = this.FindForm().CurrentAutoScaleFactor();
            System.Diagnostics.Debug.WriteLine($"MajorTabControl apply scaling {scale}");
            uccb.Scale(scale);

            System.Diagnostics.Debug.WriteLine($"MajorTabControl apply theming");
            ExtendedControls.Theme.Current.ApplyStd(page);

            System.Diagnostics.Debug.WriteLine($"MajorTabControl finished");

            page.ResumeLayout();

            return page;
        }

        private EDDiscoveryForm eddiscovery;

        #endregion

    }
}
