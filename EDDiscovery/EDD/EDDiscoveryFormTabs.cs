/*
 * Copyright © 2015 - 2022 EDDiscovery development team
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery
{
    public partial class EDDiscoveryForm
    {
        public void PerformOperationOnTabs(UserControls.UserControlCommonBase sender, object actionobj)
        {
            System.Diagnostics.Debug.WriteLine($"Perform Action Script Panel operation request {actionobj}");
            tabControlMain.RequestOperationOther(null, sender, actionobj);
        }

        public void AddTab(PanelInformation.PanelIDs id, int tabindex = 0) // negative means from the end.. -1 is one before end
        {
            tabControlMain.AddTab(id, tabindex);
        }

        public bool TabPageExists(string name)
        {
            PanelInformation.PanelIDs? id = PanelInformation.GetPanelIDByWindowsRefName(name);
            if (id != null)
                return tabControlMain.GetMajorTab(id.Value) != null;
            else
                return tabControlMain.FindTabPageByName(name) != null;
        }

        public bool SelectTabPage(string name, bool openit, bool closeit)
        {
            PanelInformation.PanelIDs? id = PanelInformation.GetPanelIDByWindowsRefName(name);

            if (id == null)     // if a name, see if it exists
            {
                TabPage p = tabControlMain.FindTabPageByName(name);
                if (p != null)      // it does, return true, either remove it or select it
                {
                    if (closeit)
                        tabControlMain.RemoveTab(p);
                    else
                        tabControlMain.SelectTab(p);

                    return true;
                }
            }
            else
                return SelectTabPage(id.Value, openit, closeit);

            return false;
        }

        public bool SelectTabPage(PanelInformation.PanelIDs id, bool openit, bool closeit)
        {
            TabPage p = tabControlMain.GetMajorTab(id);   // does ID exist?
            if (p == null)          // no, either open it, or error
            {
                if (openit)
                {
                    tabControlMain.EnsureMajorTabIsPresent(id, true);
                    return true;
                }
            }
            else
            {                   // yes, either close it or select, no error
                if (closeit)
                    tabControlMain.RemoveTab(p);
                else
                    tabControlMain.SelectTab(p);

                return true;
            }

            return false;
        }


        private bool IsNonRemovableTab(int n)
        {
            bool uch = Object.ReferenceEquals(tabControlMain.TabPages[n].Controls[0], tabControlMain.PrimarySplitterTab);
            bool sel = tabControlMain.TabPages[n].Controls[0] is UserControls.UserControlPanelSelector;
            return uch || sel;
        }

        private void panelTabControlBack_MouseDown(object sender, MouseEventArgs e)     // click on the empty space of the tabs.. backed up by the panel
        {
            if (e.Button == MouseButtons.Right)
            {
                tabControlMain.ClearLastTab();      // this sets LastTab to -1, which thankfully means insert at last but one position to the AddTab function
                contextMenuStripTabs.Show(tabControlMain.PointToScreen(e.Location));
            }
        }

        private void tabControlMain_MouseClick(object sender, MouseEventArgs e)     // click on one of the tab buttons
        {
            if (tabControlMain.LastTabClicked >= 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    Point p = tabControlMain.PointToScreen(e.Location);
                    p.Offset(0, -8);
                    contextMenuStripTabs.Show(p);
                }
                else if (e.Button == MouseButtons.Middle && !IsNonRemovableTab(tabControlMain.LastTabClicked))
                {
                    tabControlMain.RemoveTab(tabControlMain.LastTabClicked);
                }
            }
        }

        private void ContextMenuStripTabs_Opening(object sender, CancelEventArgs e)
        {
            int n = tabControlMain.LastTabClicked;
            bool validtab = n >= 0 && n < tabControlMain.TabPages.Count;   // sanity check

            removeTabToolStripMenuItem.Enabled = validtab && !IsNonRemovableTab(n);
            renameTabToolStripMenuItem.Enabled = validtab && !(tabControlMain.TabPages[n].Controls[0] is UserControls.UserControlPanelSelector);
        }

        private void UpdatePanelListInContextMenuStrip()
        {
            addTabToolStripMenuItem.DropDownItems.Clear();

            foreach (PanelInformation.PanelInfo pi in PanelInformation.GetUserSelectablePanelInfo(EDDConfig.Instance.SortPanelsByName))
            {
                ToolStripMenuItem tsmi = PanelInformation.MakeToolStripMenuItem(pi.PopoutID,
                    (s, e) => tabControlMain.AddTab((PanelInformation.PanelIDs)((s as ToolStripMenuItem).Tag), tabControlMain.LastTabClicked));

                if (tsmi != null)
                    addTabToolStripMenuItem.DropDownItems.Add(tsmi);

                ToolStripMenuItem tsmi2 = PanelInformation.MakeToolStripMenuItem(pi.PopoutID,
                    (s, e) => PopOuts.PopOut((PanelInformation.PanelIDs)((s as ToolStripMenuItem).Tag)));

                if (tsmi2 != null)
                    popOutPanelToolStripMenuItem.DropDownItems.Add(tsmi2);
            }
        }


        private void EDDiscoveryForm_MouseDown(object sender, MouseEventArgs e)     // use the form to detect the click on the empty tab area.. it passes thru
        {
            if (e.Button == MouseButtons.Right && e.Y >= tabControlMain.Top)
            {
                tabControlMain.ClearLastTab();      // this sets LastTab to -1, which thankfully means insert at last but one position to the AddTab function
                Point p = this.PointToScreen(e.Location);
                p.Offset(0, -8);
                contextMenuStripTabs.Show(p);
            }
        }

    }
}
