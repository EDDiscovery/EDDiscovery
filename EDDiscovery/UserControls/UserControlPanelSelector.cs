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
        const int labelvpos = 10;
        const int iconvpos = 50;
        Point popoutpos = new Point(10, 100);
        Point menupos = new Point(hspacing-20 - 24*2 - 10, 100);

        public UserControlPanelSelector()
        {
            InitializeComponent();
        }

        public override void Init()
        {
        }

        public void ButtonPress(Object o, int i)
        {
            int button = (int)o;
            System.Diagnostics.Debug.WriteLine("Selected " + button + " " + i);
            PanelInformation.PanelInfo pi = PanelInformation.PanelList[button];

            if (i == 0)
                discoveryform.PopOuts.PopOut(pi.PopoutID);
            else
                discoveryform.AddTab(pi.PopoutID,-1);   // add as last tab

        }

        public void Redraw()
        {
            Controls.Clear();

            SuspendLayout();

            int havailable = ClientRectangle.Width / hspacing;
            for (int i = 0; i < PanelInformation.GetNumberPanels; i++)
            {
                PanelInformation.PanelInfo pi = PanelInformation.PanelList[i];

                if (pi.IsUserSelectable)
                {
                    CompositeButton cb = new CompositeButton();
                    cb.Location = new Point(hstart + hspacing * (i % havailable), vstart + vspacing * (i / havailable));
                    cb.Size = panelsize;
                    cb.Tag = i;
                    cb.Padding = new Padding(10);
                    cb.QuickInit(EDDiscovery.Icons.Controls.Selector_Background,
                                pi.WindowTitle,
                                EDDTheme.Instance.GetFontAtSize(11),
                                Color.Transparent,
                                pi.TabIcon,
                                new Size(48, 48),
                                new Image[] { EDDiscovery.Icons.Controls.TabStrip_Popout, EDDiscovery.Icons.Controls.Selector_AddTab },
                                new Size(48,48),
                                ButtonPress);
                    toolTip1.SetToolTip(cb.Buttons[0], "Pop out in a new window");
                    toolTip1.SetToolTip(cb.Buttons[1], "Open as a new menu tab");
                    toolTip1.SetToolTip(cb.Decals[0], pi.Description);

                    Controls.Add(cb);
                    EDDTheme.Instance.ApplyToControls(cb);
                }
            }

            ResumeLayout();
        }

        public override void InitialDisplay()
        {
            Redraw();
        }

        public override void Closing()
        {
        }

    }
}
