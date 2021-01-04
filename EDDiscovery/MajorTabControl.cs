/*
 * Copyright © 2015 - 2017 EDDiscovery development team
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
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery
{
    public class MajorTabControl : ExtendedControls.ExtTabControl
    {
        EDDiscoveryForm eddiscovery;

        public UserControls.UserControlContainerSplitter PrimaryTab { get 
            { 
                foreach (TabPage p in TabPages)      // all main tabs, load/display
                {
                    if (p.Tag != null)
                        return p.Controls[0] as UserControls.UserControlContainerSplitter;
                }
                return null;
            }
        }

        //EDDiscovery Init calls this
        public void CreateTabs(EDDiscoveryForm edf, bool resettabs, string resetsettings)
        {
            eddiscovery = edf;

            int[] panelids;
            int[] displaynumbers;
            int currentlyselectedtab = 0;

            string majortabs = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(EDDProfiles.Instance.UserControlsPrefix + "MajorTabControlList", "");
            string[] majortabnames = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(EDDProfiles.Instance.UserControlsPrefix + "MajorTabControlName", "").Replace("!error!", "+").Split(';');       // if its okay, load the name list

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
                        TabPage p = CreateTab(PanelInformation.PanelIDs.SplitterControl, name ?? "History", displaynumbers[i], TabPages.Count);
                        p.Tag = true;       // this marks it as the primary tab..
                    }
                    else
                    {
                        PanelInformation.PanelIDs p = (PanelInformation.PanelIDs)panelids[i];
                        CreateTab(p, name, displaynumbers[i], TabPages.Count);      // no need the theme, will be themed as part of overall load
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

            UserControls.UserControlContainerSplitter primary = PrimaryTab;

            foreach (TabPage p in TabPages)      // all main tabs, load/display
            {
                // now a strange thing. tab Selected, cause its shown, gets resized (due to repoisition form). Other tabs dont.
                // LoadLayout could fail due to an incorrect size that would break something (such as spitters).. 
                // so force size. tried perform layout to no avail
                p.Size = TabPages[SelectedIndex].Size;
                UserControls.UserControlCommonBase uccb = (UserControls.UserControlCommonBase)p.Controls[0];
                uccb.SetCursor(primary.GetTravelGrid);
                uccb.LoadLayout();
                uccb.InitialDisplay();
            }

            //foreach (TabPage tp in tabControlMain.TabPages) System.Diagnostics.Debug.WriteLine("TP Size " + tp.Controls[0].DisplayRectangle);
        }

        public void CloseTabList()
        {
            List<int> idlist = new List<int>();

            idlist.Add(SelectedIndex);   // first is current index

            string tabnames = "";

            UserControls.UserControlContainerSplitter primary = PrimaryTab;

            foreach (TabPage p in TabPages)      // all main tabs, load/display
            {
                UserControls.UserControlCommonBase uccb = p.Controls[0] as UserControls.UserControlCommonBase;
                uccb.CloseDown();
                PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByType(uccb.GetType());
                idlist.Add( Object.ReferenceEquals(uccb,primary) ? -1 : (int)pi.PopoutID);      // primary is marked -1
                idlist.Add(uccb.displaynumber);
                tabnames += p.Text + ";";
            }

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(EDDProfiles.Instance.UserControlsPrefix + "MajorTabControlList", string.Join(",", idlist));
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(EDDProfiles.Instance.UserControlsPrefix + "MajorTabControlName", tabnames);
        }

        public void AddTab(PanelInformation.PanelIDs id , int tabindex = 0)     // -n is from the end, else before 0,1,2
        {
            if (tabindex < 0)
                tabindex = Math.Max(0,TabCount + tabindex);

            TabPage page = CreateTab(id, null, -1, tabindex);

            if (page != null)
            {
                UserControls.UserControlCommonBase uccb = page.Controls[0] as UserControls.UserControlCommonBase;
                uccb.SetCursor(PrimaryTab.GetTravelGrid);
                uccb.LoadLayout();
                uccb.InitialDisplay();
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
                    EDDHelp.Help(parent, pt, uccb);
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
                TabPage page = TabPages[tabIndex];
                UserControls.UserControlCommonBase uccb = page.Controls[0] as UserControls.UserControlCommonBase;
                uccb.CloseDown();
                page.Dispose();
            }
        }

        public TabPage GetMajorTab(PanelInformation.PanelIDs ptype)
        {
            Type t = PanelInformation.GetPanelInfoByPanelID(ptype).PopoutType;
            return (from TabPage x in TabPages where x.Controls[0].GetType() == t select x).FirstOrDefault();
        }

        public TabPage EnsureMajorTabIsPresent(PanelInformation.PanelIDs ptype, bool selectit)
        {
            TabPage page = GetMajorTab(ptype);
            if (page == null)
            {
                page = CreateTab(ptype, null, -1, TabCount);
                UserControls.UserControlCommonBase uccb = page.Controls[0] as UserControls.UserControlCommonBase;
                uccb.SetCursor(PrimaryTab.GetTravelGrid);
                uccb.LoadLayout();
                uccb.InitialDisplay();
            }

            if (selectit)
                SelectTab(page);

            return page;
        }

        #region Implementation

        // MAY return null!

        private TabPage CreateTab(PanelInformation.PanelIDs ptype, string name, int dn, int posindex)
        {
            // debug - create an example tab page
            // keep for now 
            //TabPage page = new TabPage();
            //page.Location = new System.Drawing.Point(4, 22);    // copied from normal tab creation code
            //page.Padding = new System.Windows.Forms.Padding(3);
            //UserControl uc = new UserControl();
            //uc.Dock = DockStyle.Fill;
            //uc.AutoScaleMode = AutoScaleMode.Inherit;
            //page.Controls.Add(uc);
            //ExtendedControls.TabStrip ts = new ExtendedControls.TabStrip();
            //ts.Dock = DockStyle.Fill;
            //uc.Controls.Add(ts);
            //TabPages.Insert(posindex, page);


            UserControls.UserControlCommonBase uccb = PanelInformation.Create(ptype);   // must create, since its a ptype.
            if (uccb == null)       // if ptype is crap, it returns null.. catch
                return null;

            uccb.AutoScaleMode = AutoScaleMode.Inherit;     // inherit will mean Font autoscale won't happen at attach
            uccb.Dock = System.Windows.Forms.DockStyle.Fill;    // uccb has to be fill, even though the VS designer does not indicate you need to set it.. copied from designer code
            uccb.Location = new System.Drawing.Point(3, 3);

            if (dn == -1) // if work out display number
            {
                List<int> idlist = (from TabPage p in TabPages where p.Controls[0].GetType() == uccb.GetType() select (p.Controls[0] as UserControls.UserControlCommonBase).displaynumber).ToList();

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

            //System.Diagnostics.Debug.WriteLine("Create tab {0} dn {1} at {2}", ptype, dn, posindex);

            int numoftab = (dn == UserControls.UserControlCommonBase.DisplayNumberPrimaryTab) ? 0 : (dn - UserControls.UserControlCommonBase.DisplayNumberStartExtraTabs + 1);
            if (uccb is UserControls.UserControlContainerSplitter && numoftab > 0)          // so history is a splitter, so first real splitter will be dn=100, adjust for it
                numoftab--;

            string postfix = numoftab == 0 ? "" : "(" + numoftab.ToStringInvariant() + ")";
            string title = name != null ? name : (PanelInformation.GetPanelInfoByPanelID(ptype).WindowTitle + postfix);

            uccb.Name = title;              // for debugging use

            TabPage page = new TabPage(title);
            page.Location = new System.Drawing.Point(4, 22);    // copied from normal tab creation code
            page.Padding = new System.Windows.Forms.Padding(3); // this is to allow a pad around the sides

            page.Controls.Add(uccb);

            TabPages.Insert(posindex, page);        // with inherit above, no font autoscale

            //Init control after it is added to the form
            uccb.Init(eddiscovery, dn);    // start the uccb up

            uccb.Scale(this.FindForm().CurrentAutoScaleFactor());       // scale and  
            EDDTheme.Instance.ApplyStd(page);  // theme it.  Order as per the contract in UCCB

            return page;
        }


        #endregion

    }
}
