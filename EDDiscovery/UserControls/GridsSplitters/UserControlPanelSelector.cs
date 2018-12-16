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
using ExtendedControls;

namespace EDDiscovery.UserControls
{
    public partial class UserControlPanelSelector : UserControlCommonBase
    {
        const int hstart = 10;
        const int vstart = 10;
        const int hspacing = 160;
        const int vspacing = 170;
        Size panelsize = new Size(hspacing - 20, vspacing - 20);

        private int HorzNumber { get { return Math.Max(1,ClientRectangle.Width / hspacing); } }
        private int curhorz = 0;

        public UserControlPanelSelector()
        {
            InitializeComponent();
        }

        public override void InitialDisplay()
        {
            Redraw();
        }

        public override void Init()
        {
            discoveryform.OnAddOnsChanged += Discoveryform_OnAddOnsChanged;
        }

        public override void Closing()
        {
            discoveryform.OnAddOnsChanged -= Discoveryform_OnAddOnsChanged;
        }

        private void Redraw()
        {
            panelVScroll.SuspendLayout();

            panelVScroll.RemoveAllControls();

            Bitmap backimage = new Bitmap(EDDiscovery.Icons.Controls.Selector_Background);
            Color centre = backimage.GetPixel(48, 48);


            {
                Versions.VersioningManager mgr = new Versions.VersioningManager();
                AddOnManagerForm.ReadLocalFiles(mgr, true);

                int i = mgr.DownloadItems.Count;

                CompositeButton cb = new CompositeButton();
                cb.Size = panelsize;
                cb.Tag = 999;
                cb.Padding = new Padding(10);
                cb.QuickInit(EDDiscovery.Icons.Controls.Selector_Background,
                            (i==0) ? "NO ADD ONS!".Tx(this) : i.ToString() + " Add Ons".Tx(this),
                            EDDTheme.Instance.GetFontAtSize(11),
                            (i==0) ? Color.Red : (EDDTheme.Instance.TextBlockColor.GetBrightness() < 0.1 ? Color.AntiqueWhite : EDDTheme.Instance.TextBlockColor),
                            centre,
                            EDDiscovery.Icons.Controls.Main_Addons_ManageAddOns,
                            new Size(48, 48),
                            new Image[] { EDDiscovery.Icons.Controls.Main_Addons_ManageAddOns },
                            new Size(48, 48),
                            ButtonPress);

                toolTip.SetToolTip(cb.Buttons[0], "Click to add or remove Add Ons".Tx(this,"TTA"));
                toolTip.SetToolTip(cb.Decals[0], "Add ons are essential additions to your EDD experience!".Tx(this,"TTB"));
                panelVScroll.Controls.Add(cb);    
                EDDTheme.Instance.ApplyToControls(cb.Buttons[0], null, true);       // need to theme up the button
                cb.Buttons[0].BackColor = centre;   // but then fix the back colour again
            }

            PanelInformation.PanelIDs[] pids = PanelInformation.GetUserSelectablePanelIDs(EDDConfig.Instance.SortPanelsByName);

            for (int i = 0; i < pids.Length; i++)
            {
                PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByPanelID(pids[i]);

                CompositeButton cb = new CompositeButton();
                cb.Size = panelsize;
                cb.Tag = pi.PopoutID;
                cb.Padding = new Padding(10);
                cb.QuickInit(EDDiscovery.Icons.Controls.Selector_Background,
                            pi.WindowTitle,
                            EDDTheme.Instance.GetFontAtSize(11),
                            EDDTheme.Instance.TextBlockColor.GetBrightness() < 0.1 ? Color.AntiqueWhite : EDDTheme.Instance.TextBlockColor,
                            centre,
                            pi.TabIcon,
                            new Size(48, 48),
                            new Image[] { EDDiscovery.Icons.Controls.TabStrip_Popout, EDDiscovery.Icons.Controls.Selector_AddTab },
                            new Size(48, 48),
                            ButtonPress);
                toolTip.SetToolTip(cb.Buttons[0], "Pop out in a new window".Tx(this,"PP1"));
                toolTip.SetToolTip(cb.Buttons[1], "Open as a new menu tab".Tx(this, "MT1"));
                toolTip.SetToolTip(cb.Decals[0], pi.Description);
                EDDTheme.Instance.ApplyToControls(cb.Buttons[0], null, true);
                cb.Buttons[0].BackColor = centre; // need to reset the colour back!
                EDDTheme.Instance.ApplyToControls(cb.Buttons[1], null, true);
                cb.Buttons[1].BackColor = centre;

                panelVScroll.Controls.Add(cb);       // we don't theme it.. its already fully themed to a fixed theme.
            }

            Reposition();

            panelVScroll.ResumeLayout();
        }


        private void Reposition()
        {
            panelVScroll.SuspendLayout();

            int i = 0;
            curhorz = HorzNumber;

            foreach ( Control c in panelVScroll.Controls)
            {
                if (c is CompositeButton)
                {
                    c.Location = new Point(hstart + hspacing * (i % curhorz), vstart + vspacing * (i / curhorz));
                    i++;
                }

            }
            panelVScroll.ResumeLayout();
        }

        private void ButtonPress(Object o, int i)
        {
            int button = (int)o;

            if (button == 999)
                discoveryform.manageAddOnsToolStripMenuItem_Click(null, null);
            else
            {
                PanelInformation.PanelIDs pid = (PanelInformation.PanelIDs)o;
                System.Diagnostics.Debug.WriteLine("Selected " + pid + " " + i);

                if (i == 0)
                    discoveryform.PopOuts.PopOut(pid);
                else
                    discoveryform.AddTab(pid, -1);   // add as last tab
            }

        }

        private void Discoveryform_OnAddOnsChanged()
        {
            Redraw();
        }

        private void panelVScroll_Resize(object sender, EventArgs e)
        {
            if (HorzNumber != curhorz)
            {
                Reposition();
            }
        }
    }
}
