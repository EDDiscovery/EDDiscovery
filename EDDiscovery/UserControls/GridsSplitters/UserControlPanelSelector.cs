﻿/*
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
        private bool doresize = false;

        public UserControlPanelSelector()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            Draw();
            discoveryform.OnAddOnsChanged += Redraw;
            discoveryform.OnThemeChanging += Redraw;     // because we pick the image for the composite button based on theme
        }

        public override void LoadLayout()
        {
            base.LoadLayout();
            Position();
        }

        public override void InitialDisplay()
        {
            doresize = true;                            // now allow resizing actions, before, resizes were due to setups, now due to user interactions
        }

        public override void Closing()
        {
            discoveryform.OnAddOnsChanged -= Redraw;
            discoveryform.OnThemeChanging -= Redraw;
        }
        public void Draw()
        {
            panelVScroll.RemoveAllControls();

            Bitmap backimage = new Bitmap(EDDiscovery.Icons.Controls.Selector);
            Color centre = backimage.GetPixel(48, 48);

            float brigthness = ExtendedControls.Theme.Current.Form.GetBrightness();
            Bitmap selback = (Bitmap)(brigthness < 0.3 ? EDDiscovery.Icons.Controls.Selector : EDDiscovery.Icons.Controls.Selector2);

            PanelInformation.PanelIDs[] pids = PanelInformation.GetUserSelectablePanelIDs(EDDConfig.Instance.SortPanelsByName);

            for (int i = 0; i < pids.Length; i++)
            {
                PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByPanelID(pids[i]);

                System.Diagnostics.Debug.WriteLine($"Panel {pi.WindowTitle} {pi.TabIcon.Width} x {pi.TabIcon.Height}");
                CompositeAutoScaleButton cb = CompositeAutoScaleButton.QuickInit(
                            selback,
                            pi.WindowTitle,
                            new Image[] { pi.TabIcon },
                            new Image[] { EDDiscovery.Icons.Controls.Popout, EDDiscovery.Icons.Controls.AddTab },
                            ButtonPress);

                cb.Tag = pi.PopoutID;
                toolTip.SetToolTip(cb.Buttons[0], "Pop out in a new window".T(EDTx.UserControlPanelSelector_PP1));
                toolTip.SetToolTip(cb.Buttons[1], "Open as a new menu tab".T(EDTx.UserControlPanelSelector_MT1));
                toolTip.SetToolTip(cb.Decals[0], pi.Description);

                panelVScroll.Controls.Add(cb);
            }
        }

        public void Position()
        {
            var cblist = panelVScroll.Controls.OfType<CompositeAutoScaleButton>().ToList();

            int widthavailable = ClientRectangle.Width - panelVScroll.ScrollBarWidth;
            int heightavailable = ClientRectangle.Height;

            int width = 300;
            while( width > 72)      // tried a programatic way, but because the area is not square, sqrt does not work well. best to search for the first fit
            {
                int no = (widthavailable / width) * (heightavailable / width);
                int delta = no - cblist.Count();
                if (delta >= 0)
                    break;
                width -= 2;
            }

            int spacing = ClientRectangle.Width / 150;
            width = width - spacing;

            //System.Diagnostics.Debug.WriteLine($"Panel Selector {widthavailable} {ClientRectangle.Height} -> {width}");

            int hpos = 0;
            int vpos = 0;

            panelVScroll.SuspendLayout();

            foreach ( Control c  in cblist )
            {
                c.Bounds = new Rectangle(hpos, vpos, width, width);
                hpos += width + spacing;
                if ( hpos + width > widthavailable)
                {
                    hpos = 0;
                    vpos += width + spacing;
                }
            }

            panelVScroll.ResumeLayout();
        }

        private void panelVScroll_Resize(object sender, EventArgs e)
        {
            if (!IsClosed && doresize)
                Position();
        }

        private void ButtonPress(Object o, int i)
        {
            Object cbtag = ((Control)o).Tag;

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

        private void Redraw()
        {
            Draw();
            Position();
        }
    }
}
