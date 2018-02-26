/*
 * Copyright © 2015 - 2018 EDDiscovery development team
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
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using EliteDangerousCore.EDSM;
using System.Threading.Tasks;
using EDDiscovery.Controls;
using System.Threading;
using System.Collections.Concurrent;
using EliteDangerousCore.EDDN;
using Newtonsoft.Json.Linq;
using EDDiscovery.UserControls;
using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;

namespace EDDiscovery.UserControls
{
    public partial class UserControlContainerSplit : UserControlCommonBase
    {
        public ExtendedControls.TabStrip GetTabStrip(string name)
        {
            if (name.Equals(tabStrip1.Name, StringComparison.InvariantCultureIgnoreCase))
                return tabStrip1;
            if (name.Equals(tabStrip2.Name, StringComparison.InvariantCultureIgnoreCase))
                return tabStrip2;
            return null;
        }

        public UserControlContainerSplit()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            // codes are used to save info, these are embedded UCs
            TabConfigure(tabStrip1, "1", DisplayNumberSplit1);
            TabConfigure(tabStrip2, "2", DisplayNumberSplit2);
        }

        #region TAB control

        void TabConfigure(ExtendedControls.TabStrip t, string name, int displayno)
        {
            t.ImageList = PanelInformation.GetPanelImages();
            t.TextList = PanelInformation.GetPanelDescriptions();
            t.Tag = displayno;             // these are IDs for purposes of identifying different instances of a control.. 0 = main ones (main travel grid, main tab journal). 1..N are popups
            t.OnRemoving += TabRemoved;
            t.OnCreateTab += TabCreate;
            t.OnPostCreateTab += TabPostCreate;
            t.OnPopOut += TabPopOut;
            t.Name = name;
        }

        void TabRemoved(ExtendedControls.TabStrip t, Control ctrl)     // called by tab strip when a control is removed
        {
            UserControlCommonBase uccb = ctrl as UserControlCommonBase;
            uccb.Closing();
        }

        Control TabCreate(ExtendedControls.TabStrip t, int si)        // called by tab strip when selected index changes.. create a new one.. only create.
        {
            Control c = PanelInformation.Create(si);
            c.Name = PanelInformation.PanelList[si].WindowTitle;        // tabs uses Name field for display, must set it

            discoveryform.ActionRun(Actions.ActionEventEDList.onPanelChange, null,
                new Conditions.ConditionVariables(new string[] { "PanelTabName", PanelInformation.PanelList[si].WindowRefName, "PanelTabTitle", PanelInformation.PanelList[si].WindowTitle, "PanelName", t.Name }));

            return c;
        }

        void TabPostCreate(ExtendedControls.TabStrip t, Control ctrl, int i)        // called by tab strip after control has been added..
        {                                                           // now we can do the configure of it, with the knowledge the tab has the right size
            int displaynumber = (int)t.Tag;                         // tab strip - use tag to remember display id which helps us save context.

            UserControlCommonBase uc = ctrl as UserControlCommonBase;
                        
            //System.Diagnostics.Debug.WriteLine("And theme {0}", i);
            discoveryform.theme.ApplyToControls(t);
        }

        void TabPopOut(ExtendedControls.TabStrip t, int i)        // pop out clicked
        {
            discoveryform.PopOuts.PopOut(i);
        }

        #endregion

        private void splitContainerCustom_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.Button == MouseButtons.Right)
                {
                    contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
                }
            }
        }

        private void verticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainerCustom.Orientation = Orientation.Vertical;
            verticalToolStripMenuItem.Checked = true;
            horizontalToolStripMenuItem.Checked = false;
        }

        private void horizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainerCustom.Orientation = Orientation.Horizontal;
            verticalToolStripMenuItem.Checked = false;
            horizontalToolStripMenuItem.Checked = true;
        }
    }
}
