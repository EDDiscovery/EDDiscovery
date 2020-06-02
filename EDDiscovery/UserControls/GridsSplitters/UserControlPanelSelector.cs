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
            discoveryform.OnThemeChanged += Discoveryform_OnThemeChanged;
        }

        public override void Closing()
        {
            discoveryform.OnAddOnsChanged -= Discoveryform_OnAddOnsChanged;
            discoveryform.OnThemeChanged -= Discoveryform_OnThemeChanged;
        }

        private void Discoveryform_OnThemeChanged()
        {
            Redraw();
        }

        private void Redraw()
        {
            panelVScroll.SuspendLayout();

            panelVScroll.RemoveAllControls();

            // design for std 8.25 font sizes

            Bitmap backimage = new Bitmap(EDDiscovery.Icons.Controls.Selector_Background);
            Color centre = backimage.GetPixel(48, 48);
            Size iconsize = new Size(24,24);
            int width = 96;
            int padbot = 6, padbetween = 5;

            float brigthness = EDDTheme.Instance.Form.GetBrightness();
            Image selback = brigthness < 0.3 ? EDDiscovery.Icons.Controls.Selector_Background : EDDiscovery.Icons.Controls.Selector_Background2;

            {
                Versions.VersioningManager mgr = new Versions.VersioningManager();
                AddOnManagerForm.ReadLocalFiles(mgr, true);

                int i = mgr.DownloadItems.Count;

                CompositeButton cb = CompositeButton.QuickInit(
                            selback,
                            (i == 0) ? "NO ADD ONS!".T(EDTx.UserControlPanelSelector_NOADDONS) : i.ToString() + " Add Ons".T(EDTx.UserControlPanelSelector_AddOns),
                            EDDTheme.Instance.GetFont,
                            (i == 0) ? Color.Red : (EDDTheme.Instance.TextBlockColor.GetBrightness() < 0.1 ? Color.AntiqueWhite : EDDTheme.Instance.TextBlockColor),
                            Color.Transparent,
                            EDDiscovery.Icons.Controls.Main_Addons_ManageAddOns, iconsize,
                            new Image[] { EDDiscovery.Icons.Controls.Main_Addons_ManageAddOns }, iconsize,
                            padbetween,
                            ButtonPress);

                panelVScroll.Controls.Add(cb);
                cb.Name = "Add on";
                cb.Tag = null;
                toolTip.SetToolTip(cb.Buttons[0], "Click to add or remove Add Ons".T(EDTx.UserControlPanelSelector_TTA));
                toolTip.SetToolTip(cb.Decals[0], "Add ons are essential additions to your EDD experience!".T(EDTx.UserControlPanelSelector_TTB));

                EDDTheme.Instance.ApplyStd(cb);       // need to theme up the button
                cb.Size = new Size(width, cb.FindMaxSubControlArea(0, padbot).Height);
                cb.Label.BackColor = cb.Decals[0].BackColor = Color.Transparent;
                cb.Buttons[0].BackColor = centre;   // but then fix the back colour again
            }

            PanelInformation.PanelIDs[] pids = PanelInformation.GetUserSelectablePanelIDs(EDDConfig.Instance.SortPanelsByName);

            for (int i = 0; i < pids.Length; i++)
            {
                PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByPanelID(pids[i]);

                CompositeButton cb = CompositeButton.QuickInit(
                            selback,
                            pi.WindowTitle,
                            EDDTheme.Instance.GetFont,
                            EDDTheme.Instance.TextBlockColor.GetBrightness() < 0.1 ? Color.AntiqueWhite : EDDTheme.Instance.TextBlockColor,
                            Color.Transparent,
                            pi.TabIcon, iconsize,
                            new Image[] { EDDiscovery.Icons.Controls.TabStrip_Popout, EDDiscovery.Icons.Controls.Selector_AddTab }, iconsize,
                            padbetween,
                            ButtonPress);
                cb.SuspendLayout();
                panelVScroll.Controls.Add(cb);
                cb.Tag = pi.PopoutID;
                toolTip.SetToolTip(cb.Buttons[0], "Pop out in a new window".T(EDTx.UserControlPanelSelector_PP1));
                toolTip.SetToolTip(cb.Buttons[1], "Open as a new menu tab".T(EDTx.UserControlPanelSelector_MT1));
                toolTip.SetToolTip(cb.Decals[0], pi.Description);

                EDDTheme.Instance.ApplyStd(cb);
                cb.ResumeLayout();

                cb.Size = new Size(width, cb.FindMaxSubControlArea(0, padbot).Height);
                cb.Label.BackColor =  cb.Decals[0].BackColor = Color.Transparent;
                cb.Buttons[0].BackColor = centre; // need to reset the colour back!
                cb.Buttons[1].BackColor = centre; // need to reset the colour back!
            }

            panelVScroll.Scale(this.FindForm().CurrentAutoScaleFactor());
            Reposition();

            panelVScroll.ResumeLayout();
        }


        private int MarginAround()
        {
            return Font.ScalePixels(10);
        }

        private Tuple<int,int> Spacing()
        {
            CompositeButton cb = panelVScroll.Controls.OfType<CompositeButton>().First();
            return new Tuple<int,int> (cb.Width + MarginAround(), cb.Height + MarginAround());
        }


        private int NumberAcross()
        {
            return Math.Max((panelVScroll.Width - panelVScroll.ScrollBarWidth) / Spacing().Item1, 1);
        }

        private void Reposition()
        {
            panelVScroll.SuspendLayout();

            curhorz = NumberAcross();
            int margin = MarginAround();
            var spacing = Spacing();

            int i = 0;
            foreach ( Control c in panelVScroll.Controls)
            {
                if (c is CompositeButton)
                {
                    c.Location = new Point(margin + spacing.Item1 * (i % curhorz), margin + spacing.Item2 * (i / curhorz));
                    i++;
                }

            }

            panelVScroll.ResumeLayout();
        }

        private void panelVScroll_Resize(object sender, EventArgs e)
        {
            if (panelVScroll.Controls.Count > 1 && NumberAcross() != curhorz)
            {
                Reposition();
            }
        }

        private void ButtonPress(Object o, int i)
        {
            Object cbtag = ((CompositeButton)o).Tag;

            if ( cbtag is null )        // tag being null means
                discoveryform.manageAddOnsToolStripMenuItem_Click(null, null);
            else
            {
                PanelInformation.PanelIDs pid = (PanelInformation.PanelIDs)cbtag;
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

    }
}
