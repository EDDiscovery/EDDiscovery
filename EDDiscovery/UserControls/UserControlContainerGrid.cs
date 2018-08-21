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
using EDDiscovery.UserControls;
using EliteDangerousCore.DB;
using EliteDangerousCore;

namespace EDDiscovery.UserControls
{
    public partial class UserControlContainerGrid: UserControlCommonBase        // circular, huh! neat!
    {
        private IHistoryCursor ucursor_history;     // one passed to us, refers to thc.uctg
        private IHistoryCursor ucursor_inuse;  // one in use

        private List<UserControlContainerResizable> uccrlist = new List<UserControlContainerResizable>();

        private string DbWindows { get { return DBName("GridControlWindows" ); } }
        private string DbPositions { get { return DBName("GridControlPositons" ); } }
        private string DbZOrder { get { return DBName("GridControlZOrder" ); } }

        ExtendedControls.DropDownCustom popoutdropdown;

        public UserControlContainerGrid()
        {
            InitializeComponent();
            rollUpPanelMenu.SetToolTip(toolTip);    // use the defaults
        }

        bool checkmulticall = false;    // debug for now

        public override void LoadLayout()       // unlike splitter, we don't need to be so careful about the cursor.  We can load in load layout
        {
            ucursor_history = ucursor_inuse = uctg;

            System.Diagnostics.Debug.Assert(checkmulticall == false);
            checkmulticall = true;      // examples seen of multi call, lets trap it

            //System.Diagnostics.Debug.WriteLine("Grid Restore from " + DbWindows);

            string[] names = SQLiteConnectionUser.GetSettingString(DbWindows, "").Split(',');
            int[] positions;
            int[] zorder;

            string pos = SQLiteConnectionUser.GetSettingString(DbPositions, "");
            string zo = SQLiteConnectionUser.GetSettingString(DbZOrder, "");

            SuspendLayout();

            if ( pos.RestoreArrayFromString(out positions) && zo.RestoreArrayFromString(out zorder,0,names.Length-1) && 
                        names.Length == zorder.Length && positions.Length == 4*names.Length )
            {
                int ppos = 0;
                foreach (string n in names)
                {
                    Type t = Type.GetType("EDDiscovery.UserControls." + n);
                    if (t != null )
                    {
                        UserControlCommonBase uccb = (UserControlCommonBase)Activator.CreateInstance(t);
                        CreatePanel(uccb , new Point(positions[ppos++], positions[ppos++]), new Size(positions[ppos++], positions[ppos++]));
                    }
                }

                ppos = 0;

                foreach (int item in zorder)
                {
                    if (item >= 0 && item < uccrlist.Count)
                    {
                        UserControlContainerResizable uccr = uccrlist[item];
                        panelPlayfield.Controls.SetChildIndex(uccr, ppos++);
                    }
                }
            }

            ResumeLayout();
            Invalidate(true);
            Update();        // need this to FORCE a full refresh in case there are lots of windows
            //System.Diagnostics.Debug.WriteLine("----- Grid Restore END " + DbWindows);

            UpdateButtons();

            AssignTHC();
        }

        void UpdateButtons()
        {
            buttonExtDelete.Enabled = (from x in uccrlist where x.Selected select x).Count() > 0;
            buttonExtTile.Enabled = uccrlist.Count > 0;
        }

        public override void InitialDisplay()
        {
            foreach (UserControlContainerResizable r in uccrlist)
            {
                UserControlCommonBase uc = (UserControlCommonBase)r.control;
                uc.InitialDisplay();
            }
        }

        public override void Closing()
        {
            //System.Diagnostics.Debug.WriteLine("Grid Saving to " + DbWindows);
            string s = "", p = "";
            foreach (UserControlContainerResizable r in uccrlist)   // save in uccr list
            {
                UserControlCommonBase uc = (UserControlCommonBase)r.control;

                s = s.AppendPrePad(uc.GetType().Name,",");
                p = p.AppendPrePad(r.Location.X + "," + r.Location.Y + "," + r.Size.Width + "," + r.Size.Height, ",");

                //System.Diagnostics.Debug.WriteLine("  Save " + uc.GetType().Name);

                uc.Closing();
            }

            SQLiteConnectionUser.PutSettingString(DbWindows, s);
            SQLiteConnectionUser.PutSettingString(DbPositions, p);

            string z = "";

            foreach( Control c in panelPlayfield.Controls )
            {
                if ( c is UserControlContainerResizable )
                {
                    z = z.AppendPrePad(uccrlist.FindIndex(x => x.Equals(c)).ToStringInvariant(), ",");
                }
            }

            SQLiteConnectionUser.PutSettingString(DbZOrder, z);
            //System.Diagnostics.Debug.WriteLine("---- END Grid Saving to " + DbWindows);
        }

        public override void ChangeCursorType(IHistoryCursor thc)     // a grid below changed its travel grid, update our history one
        {
            bool changedinuse = Object.ReferenceEquals(ucursor_inuse, ucursor_history);   // if we are using the history as the current tg
            //System.Diagnostics.Debug.WriteLine("Grid CTG " + ucursor_history.GetHashCode() + " IU " + ucursor_inuse.GetHashCode() + " New " + thc.GetHashCode());
            ucursor_history = thc;         // underlying one has changed. 

            if (changedinuse)   // inform the boys
            {
                ucursor_inuse = ucursor_history;
                //System.Diagnostics.Debug.WriteLine(".. changed in use, inform children");

                foreach (UserControlContainerResizable u in uccrlist)
                    ((UserControlCommonBase)u.control).ChangeCursorType(ucursor_inuse);
            }
        }

        #region Open/Close

        private UserControlContainerResizable CreatePanel(UserControlCommonBase uccb , Point pos, Size size)
        {
            UserControlContainerResizable uccr = new UserControlContainerResizable();

            PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByType(uccb.GetType());
            uccr.Init(uccb,pi.WindowTitle);
            uccr.ResizeStart += ResizeStart;
            uccr.ResizeEnd += ResizeEnd;
            uccr.BorderColor = discoveryform.theme.GridBorderLines;
            uccr.SelectedBorderColor = discoveryform.theme.TextBlockHighlightColor;

            uccrlist.Add(uccr);

            int numopenedinside = uccrlist.Count(x => x.GetType().Equals(uccb.GetType()));    // how many others are there?

            int dnum = DisplayNumberOfGrid(numopenedinside);
            System.Diagnostics.Trace.WriteLine("GD:Create " + uccb.GetType().Name + " " + dnum + " Assign THC " + ucursor_inuse.GetHashCode() );

            panelPlayfield.Controls.Add(uccr);

            uccb.Init(discoveryform, dnum);
            uccb.SetCursor(ucursor_inuse);
            uccb.LoadLayout();

            uccr.Font = discoveryform.theme.GetFont;        // Important. Apply font autoscaling to the user control
                                                            // ApplyToForm does not apply the font to the actual UC, only
                                                            // specific children controls.  The TabControl in the discoveryform ends up autoscaling most stuff
                                                            // the children directly attached to the discoveryform are not autoscaled

            discoveryform.theme.ApplyToControls(uccr, discoveryform.theme.GetFont);

            uccr.Location = pos;
            uccr.Size = size;

            uccb.InitialDisplay();

            return uccr;
        }

        public void ClosePanel( UserControlContainerResizable uccr )
        {
            UserControlCommonBase uc = (UserControlCommonBase)uccr.control;
            uc.Closing();
            panelPlayfield.Controls.Remove(uccr);
            uccrlist.Remove(uccr);
            Invalidate();
            uc.Dispose();
            uccr.Dispose();
            UpdateButtons();
            AssignTHC();
        }

        private void Select(UserControlContainerResizable uccr)
        {
            foreach (UserControlContainerResizable r in uccrlist)
            {
                if (Object.ReferenceEquals(r, uccr))
                {
                    if (!r.Selected)
                    {
                        r.Selected = true;
                        r.BringToFront();
                    }
                }
                else if (r.Selected)
                {
                    r.Selected = false;
                }
            }

            UpdateButtons();
        }

        private void AssignTHC()
        {
            var v = uccrlist.Find(x => x.control.GetType() == typeof(UserControlTravelGrid));   // find one with TG

            if (v == null)
                v = uccrlist.Find(x => x.control.GetType() == typeof(UserControlJournalGrid));   // find one with Journal grid if no TG

            if (v == null)
                v = uccrlist.Find(x => x.control.GetType() == typeof(UserControlStarList));   // find one with Journal grid if no TG

            IHistoryCursor uctgfound = (v != null) ? (v.control as IHistoryCursor) : null;    // if found, set to it

            if ( (uctgfound != null && !Object.ReferenceEquals(uctgfound,ucursor_inuse) ) ||    // if got one but its not the one currently in use
                 (uctgfound == null && !Object.ReferenceEquals(ucursor_history,ucursor_inuse))    // or not found, but we are not on the history one
                )
            { 
                ucursor_inuse = (uctgfound != null) ? uctgfound : ucursor_history;    // select
                //System.Diagnostics.Debug.WriteLine("Children of " + this.GetHashCode() + " Use THC " + uctg_inuse.GetHashCode());

                foreach (UserControlContainerResizable u in uccrlist)
                    ((UserControlCommonBase)u.control).ChangeCursorType(ucursor_inuse);

                ucursor_inuse.FireChangeSelection();       // let the uctg tell the children a change event, so they can refresh
            }
        }

        #endregion

        #region Panel reactions

        bool wasselected = false;
        private void ResizeStart(UserControlContainerResizable uccr)
        {
            wasselected = uccr.Selected;
            Select(uccr);
        }

        private void ResizeEnd(UserControlContainerResizable uccr, bool dragged)
        {
            if (!dragged && wasselected)
                Select(null);
        }

        #endregion

        #region Clicks

        private void buttonExtPopOut_Click(object sender, EventArgs e)
        {
            popoutdropdown = new ExtendedControls.DropDownCustom("", true);

            popoutdropdown.ItemHeight = 26;
            popoutdropdown.Items = PanelInformation.GetUserSelectablePanelDescriptions(EDDConfig.Instance.SortPanelsByName).ToList();
            popoutdropdown.ImageItems = PanelInformation.GetUserSelectablePanelImages(EDDConfig.Instance.SortPanelsByName).ToList();
            popoutdropdown.ItemSeperators = PanelInformation.GetUserSelectableSeperatorIndex(EDDConfig.Instance.SortPanelsByName);
            PanelInformation.PanelIDs[] pids = PanelInformation.GetUserSelectablePanelIDs(EDDConfig.Instance.SortPanelsByName);
            popoutdropdown.FlatStyle = FlatStyle.Popup;
            popoutdropdown.Activated += (s, ea) =>
            {
                Point location = buttonExtPopOut.PointToScreen(new Point(0, 0));
                popoutdropdown.Location = popoutdropdown.PositionWithinScreen(location.X + buttonExtPopOut.Width, location.Y);
                this.Invalidate(true);
            };
            popoutdropdown.SelectedIndexChanged += (s, ea) =>
            {
                UserControlContainerResizable uccr = CreatePanel(PanelInformation.Create(pids[popoutdropdown.SelectedIndex]),
                                            new Point((uccrlist.Count % 5) * 50, (uccrlist.Count % 5) * 50),
                                            new Size(Math.Min(300, panelPlayfield.Width - 10), Math.Min(300, panelPlayfield.Height - 10)));
                Select(null);
                uccr.Selected = true;
                uccr.BringToFront();
                UpdateButtons();
                AssignTHC();
            };

            popoutdropdown.Size = new Size(500, 26 * 20);
            discoveryform.theme.ApplyToControls(popoutdropdown);
            popoutdropdown.SelectionBackColor = discoveryform.theme.ButtonBackColor;
            popoutdropdown.Show(this);
        }

        private void buttonExtDelete_Click(object sender, EventArgs e)
        {
            UserControlContainerResizable sel = uccrlist.Find(x => x.Selected);

            if ( sel != null )
                ClosePanel(sel);
        }

        private void buttonExtTile_Click(object sender, EventArgs e)
        {
            int w = panelPlayfield.Width;
            int h = panelPlayfield.Height;
            int bands = (h + 299) / 300;

            int cols;
            int rows = bands;
            if (bands >= uccrlist.Count)
            {
                cols = 1;
                rows = uccrlist.Count;
            }
            else
            {
                cols = (uccrlist.Count + bands - 1) / bands;
                rows = (uccrlist.Count + cols - 1) / cols;
            }

            System.Diagnostics.Debug.WriteLine("Bands " + bands + " C " + cols + " R " + rows);
            
            h = h / rows;
            w = w / cols;

            for (int r = 0; r < bands; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    int i = r * cols + c;
                    if (i < uccrlist.Count)
                    {
                        UserControlContainerResizable uccr = uccrlist[i];
                        uccr.Location = new Point(c * w, r * h);
                        uccr.Size = new Size(w, h);
                    }
                }
            }
        }

        private void panelPlayfield_MouseClick(object sender, MouseEventArgs e)
        {
            Select(null);
        }

        #endregion

        #region Transparency

        public override Color ColorTransparency { get { return Color.Green; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            this.BackColor = curcol;
            panelPlayfield.BackColor = curcol;
            rollUpPanelMenu.BackColor = curcol;
            rollUpPanelMenu.ShowHiddenMarker = !on;

            foreach (UserControlContainerResizable r in uccrlist)
            {
                r.BorderColor = on ? Color.Transparent : discoveryform.theme.GridBorderLines;
                UserControlCommonBase uc = (UserControlCommonBase)r.control;
                uc.SetTransparency(on, curcol);
            }
        }

        #endregion
    }
}
