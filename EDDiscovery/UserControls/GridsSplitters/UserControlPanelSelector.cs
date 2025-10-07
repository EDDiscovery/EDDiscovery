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

using ActionLanguage.Manager;
using EDDiscovery.Forms;
using ExtendedControls;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlPanelSelector : UserControlCommonBase
    {
        public UserControlPanelSelector()
        {
            InitializeComponent();
            DBBaseName = "SelectorPanel";
        }

        protected override void Init()
        {
            Draw();
            DiscoveryForm.OnAddOnsChanged += Draw;

            // We need to remove and redraw based on theme, before the theme is applyed at ApplyTheme
            // This is the thing which paints the initial set
            DiscoveryForm.OnThemeChanging += Draw;     
            DiscoveryForm.OnPanelAdded += Draw;
            DiscoveryForm.OnPanelRemoved += Draw;
        }

        protected override void LoadLayout()
        {
            base.LoadLayout();
            Position();
        }

        protected override void Closing()
        {
            DiscoveryForm.OnAddOnsChanged -= Draw;
            DiscoveryForm.OnThemeChanging -= Draw;
            DiscoveryForm.OnPanelAdded -= Draw;
            DiscoveryForm.OnPanelRemoved -= Draw;
        }

        private void Draw(PanelInformation.PanelIDs p)
        {
            Draw();
        }

        public void Draw()
        {
            panelVScroll.RemoveAllControls();

            Bitmap backimage = new Bitmap(EDDiscovery.Icons.Controls.Selector);
            Color centre = backimage.GetPixel(48, 48);

            float brigthness = ExtendedControls.Theme.Current.Form.GetBrightness();
            Bitmap selback = (Bitmap)(brigthness < 0.3 ? EDDiscovery.Icons.Controls.Selector : EDDiscovery.Icons.Controls.Selector2);

            {
                GitActionFiles gh = new GitActionFiles(EDDOptions.Instance.AppDataDirectory);
                gh.ReadLocalFolder(EDDOptions.Instance.ActionsAppDirectory(), gh.ActionFileWildCard,"ActionFiles");
                int i = gh.VersioningManager.DownloadItems.Count;

                CompositeAutoScaleButton cb = CompositeAutoScaleButton.QuickInit(
                            selback,
                            (i == 0) ? "NO ADD ONS!".Tx(): i.ToString() + " Add Ons".Tx(),
                            new Image[] { EDDiscovery.Icons.Controls.ManageAddOns48 },
                            new Image[] { EDDiscovery.Icons.Controls.Popout },
                            ButtonPress,
                            2);
                cb.Name = "Add on";
                cb.Tag = null;
                toolTip.SetToolTip(cb.Buttons[0], "Click to add or remove Add Ons".Tx());
                toolTip.SetToolTip(cb.Decals[0], "Add ons are essential additions to your EDD experience!".Tx());

                panelVScroll.Controls.Add(cb);
            }

            foreach( var pi in PanelInformation.GetUserSelectablePanelInfo(EDDConfig.Instance.SortPanelsByName))
            {
                //System.Diagnostics.Debug.WriteLine($"Panel {pi.WindowTitle} {pi.TabIcon.Width} x {pi.TabIcon.Height}");

                CompositeAutoScaleButton cb = CompositeAutoScaleButton.QuickInit(
                            selback,
                            pi.WindowTitle,
                            new Image[] { pi.TabIcon },
                            pi.PopOutOnly ? new Image[] { EDDiscovery.Icons.Controls.Popout } : new Image[] { EDDiscovery.Icons.Controls.Popout, EDDiscovery.Icons.Controls.Addtab48 },
                            ButtonPress,
                            2);

                cb.Tag = pi.PopoutID;
                toolTip.SetToolTip(cb.Buttons[0], "Pop out in a new window".Tx());
                if ( cb.Buttons.Length>1)
                    toolTip.SetToolTip(cb.Buttons[1], "Open as a new menu tab".Tx());
                toolTip.SetToolTip(cb.Decals[0], pi.Description);

                panelVScroll.Controls.Add(cb);
            }

            Position();
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
            if (!IsClosed && IsInitialDisplayCalled)
                Position();
        }

        private void ButtonPress(Object o, int i)
        {
            Object cbtag = ((Control)o).Tag;

            if ( cbtag is null )        // tag being null means
                DiscoveryForm.manageAddOnsToolStripMenuItem_Click(null, null);
            else
            {
                PanelInformation.PanelIDs pid = (PanelInformation.PanelIDs)cbtag;
                System.Diagnostics.Debug.WriteLine("Selected " + pid + " " + i);

                if (i == 0)
                    DiscoveryForm.PopOuts.PopOut(pid);
                else
                    DiscoveryForm.AddTab(pid, -1);   // add as last tab
            }

        }


    }
}
