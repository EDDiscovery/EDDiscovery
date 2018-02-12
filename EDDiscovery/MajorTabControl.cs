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

using EDDiscovery.Forms;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery
{
    public class MajorTabControl : ExtendedControls.TabControlCustom
    {
        UserControls.UserControlHistory userhistory;
        EDDiscoveryForm eddiscovery;

        //EDDiscovery Init calls this
        public void CreateTabs(EDDiscoveryForm edf, UserControls.UserControlHistory uch)
        { 
            eddiscovery = edf;
            userhistory = uch;

            string majortabs = SQLiteConnectionUser.GetSettingString("MajorTabControlList", "");
            string[] majortabnames = null;
            int[] tabctrl;

            if (!majortabs.RestoreArrayFromString(out tabctrl) || tabctrl.Length == 0 || (tabctrl.Length % 2) != 1) // need it odd as we have an index tab as first
            {
                tabctrl = new int[] { 0,        -1, 0 ,         // reset..
                                                (int)PanelInformation.PanelIDs.Route, 0,
                                                (int)PanelInformation.PanelIDs.Expedition, 0,
                                                (int)PanelInformation.PanelIDs.Settings,0,
                                                (int)PanelInformation.PanelIDs.PanelSelector,0
                };
            }
            else
                majortabnames = SQLiteConnectionUser.GetSettingString("MajorTabControlName", "").Split(';');       // if its okay, load the name list

            TabPage history = TabPages[0];       // remember history page, remove
            TabPages.Clear();

            bool donehistory = false;
            for (int i = 1; i < tabctrl.Length; i += 2)
            {
                int nameindex = (i - 1) / 2;
                string name = majortabnames != null && nameindex < majortabnames.Length && majortabnames[nameindex].Length>0 ? majortabnames[nameindex] : null;

                if (tabctrl[i] != -1)
                {
                    try
                    {
                        PanelInformation.PanelIDs p = (PanelInformation.PanelIDs)tabctrl[i];
                        CreateTab(p, name, tabctrl[i + 1], TabPages.Count, false);      // no need the theme, will be themed as part of overall load
                                                                                                 // may fail if p is crap, then just ignore
                    }
                    catch { }   // paranoia in case tabctrl number is crappy.
                }
                else if (!donehistory)
                {
                    if (name != null)
                        history.Text = name;
                    TabPages.Add(history); // add back in right place
                    userhistory.Init(eddiscovery, null, UserControls.UserControlCommonBase.DisplayNumberHistoryGrid); // and init at this point with 0 as dn
                    donehistory = true;
                }
            }

            if (!donehistory)      // just in case its missing
            {
                TabPages.Add(history); // add back in right place
                userhistory.Init(eddiscovery, null, UserControls.UserControlCommonBase.DisplayNumberHistoryGrid); // and init at this point with 0 as dn
            }

            if (tabctrl.Length > 0 && tabctrl[0] >= 0 && tabctrl[0] < TabPages.Count)
                SelectedIndex = tabctrl[0];  // make sure external data does not crash us
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

            foreach (TabPage p in TabPages)      // all main tabs, load/display
            {
                UserControls.UserControlCommonBase uccb = p.Controls[0] as UserControls.UserControlCommonBase;
                uccb.Closing();
                PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByType(uccb.GetType());
                idlist.Add(pi != null ? (int)pi.PopoutID : -1);
                idlist.Add(uccb.displaynumber);
                tabnames += p.Text + ";";
            }

            SQLiteConnectionUser.PutSettingString("MajorTabControlList", string.Join(",", idlist));
            SQLiteConnectionUser.PutSettingString("MajorTabControlName", tabnames);
        }

        public void AddTab(PanelInformation.PanelIDs id , int tabindex = 0)     // -n is from the end, else before 0,1,2
        {
            if (tabindex < 0)
                tabindex = Math.Max(0,TabCount + tabindex);

            TabPage page = CreateTab(id, null, -1, tabindex, true);
            if (page != null)
            {
                UserControls.UserControlCommonBase uccb = page.Controls[0] as UserControls.UserControlCommonBase;
                uccb.LoadLayout();
                uccb.InitialDisplay();
                SelectedIndex = tabindex;   // and select the inserted one
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
                uccb.Closing();
                page.Dispose();
            }
        }

        public TabPage GetMajorTab(PanelInformation.PanelIDs ptype)
        {
            Type t = PanelInformation.GetPanelInfoByEnum(ptype).PopoutType;
            return (from TabPage x in TabPages where x.Controls[0].GetType() == t select x).FirstOrDefault();
        }

        public TabPage EnsureMajorTabIsPresent(PanelInformation.PanelIDs ptype, bool selectit)
        {
            TabPage page = GetMajorTab(ptype);
            if (page == null)
            {
                page = CreateTab(ptype, null, -1, TabCount, true);
                UserControls.UserControlCommonBase uccb = page.Controls[0] as UserControls.UserControlCommonBase;
                uccb.LoadLayout();
                uccb.InitialDisplay();
            }

            if (selectit)
                SelectTab(page);

            return page;
        }

        #region Implementation

        // MAY return null!

        private TabPage CreateTab(PanelInformation.PanelIDs ptype, string name, int dn, int posindex, bool dotheme)
        {
            UserControls.UserControlCommonBase uccb = PanelInformation.Create(ptype);   // must create, since its a ptype.
            if (uccb == null)       // if ptype is crap, it returns null.. catch
                return null;

            uccb.Dock = System.Windows.Forms.DockStyle.Fill;    // uccb has to be fill, even though the VS designer does not indicate you need to set it.. copied from designer code
            uccb.Location = new System.Drawing.Point(3, 3);

            List<int> idlist = (from TabPage p in TabPages where p.Controls[0].GetType() == uccb.GetType() select (p.Controls[0] as UserControls.UserControlCommonBase).displaynumber).ToList();

            if (dn == -1) // if work out display number
            {
                // not travel grid (due to clash with History control) and not in list, use primary number (0)
                if (ptype != PanelInformation.PanelIDs.TravelGrid && !idlist.Contains(UserControls.UserControlCommonBase.DisplayNumberPrimaryTab))
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

            System.Diagnostics.Debug.WriteLine("Create tab {0} dn {1} at {2}", ptype, dn, posindex);

            uccb.Init(eddiscovery, userhistory.GetTravelGrid, dn);    // start the uccb up

            string postfix;
            if (ptype != PanelInformation.PanelIDs.TravelGrid)      // given the dn, work out the name
                postfix = (dn == UserControls.UserControlCommonBase.DisplayNumberPrimaryTab) ? "" : "(" + (dn - UserControls.UserControlCommonBase.DisplayNumberStartExtraTabs + 1).ToStringInvariant() + ")";
            else
                postfix = (dn == UserControls.UserControlCommonBase.DisplayNumberStartExtraTabs) ? "" : "(" + (dn - UserControls.UserControlCommonBase.DisplayNumberStartExtraTabs).ToStringInvariant() + ")";

            string title = name != null ? name : (PanelInformation.GetPanelInfoByEnum(ptype).WindowTitle + postfix);
            TabPage page = new TabPage(title);
            page.Location = new System.Drawing.Point(4, 22);    // copied from normal tab creation code
            page.Padding = new System.Windows.Forms.Padding(3);

            page.Controls.Add(uccb);

            TabPages.Insert(posindex, page);

            if (dotheme)                // only user created ones need themeing
                EDDTheme.Instance.ApplyToControls(page, applytothis: true);

            return page;
        }


        #endregion

    }
}
