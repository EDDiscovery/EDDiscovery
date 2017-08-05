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
        private EDDiscoveryForm discoveryForm;
        private UserControlTravelGrid uctg_history;     // one passed to us, refers to thc.uctg
        private UserControlTravelGrid uctg_inuse;  // one in use
        private int displaynumber;

        private List<UserControlContainerResizable> uccrlist = new List<UserControlContainerResizable>();

        private string DbWindows { get { return "GridControlWindows" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbPositions { get { return "GridControlPositons" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbZOrder { get { return "GridControlZOrder" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        public UserControlContainerGrid()
        {
            InitializeComponent();
            comboBoxGridSelector.Items.AddRange(PopOutControl.GetPopOutNames());
            rollUpPanelMenu.SetToolTip(toolTip);    // use the defaults
            comboBoxGridSelector.SetToolTip(toolTip); // composite, needs it.
        }

        public override void Init( EDDiscoveryForm f , UserControlTravelGrid thc, int dn )       //dn = 0 primary grid, or 1 first pop out, etc
        {
            discoveryForm = f;
            uctg_history = uctg_inuse = thc;
            displaynumber = dn;
        }

        bool checkmulticall = false;    // debug for now

        public override void LoadLayout()
        {
            System.Diagnostics.Debug.Assert(checkmulticall == false);
            checkmulticall = true;      // examples seen of multi call, lets trap it

            System.Diagnostics.Debug.WriteLine("Grid Restore from " + DbWindows);

            string[] names = SQLiteConnectionUser.GetSettingString(DbWindows, "").Split(',');
            int[] positions;
            int[] zorder;

            string pos = SQLiteConnectionUser.GetSettingString(DbPositions, "");
            string zo = SQLiteConnectionUser.GetSettingString(DbZOrder, "");

            SuspendLayout();

            if ( pos.RestoreArrayFromString(out positions) && zo.RestoreArrayFromString(out zorder,0,names.Length-1) && 
                        names.Length == zorder.Length && positions.Length == 4*names.Length )
            {
                // need work.  We want to preserve the order for tiling.  So list is in tiling order.  But we need to add in a specific order
                // as Z order is controlled by order Controls are added.


                var uccrfirst = uccrlist.Find(x => x.GetType() == typeof(UserControlTravelGrid));
                UserControlTravelGrid uctgfirst = (uccrfirst != null) ? (uccrfirst.control as UserControlTravelGrid) : null;

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
                    UserControlContainerResizable uccr = uccrlist[item];
                    panelPlayfield.Controls.SetChildIndex(uccr, ppos++);
                }
            }

            ResumeLayout();
            Invalidate(true);
            Update();        // need this to FORCE a full refresh in case there are lots of windows
            System.Diagnostics.Debug.WriteLine("----- Grid Restore END " + DbWindows);

            UpdateButtons();

            AssignTHC();
        }

        void UpdateButtons()
        {
            buttonExtDelete.Enabled = (from x in uccrlist where x.Selected select x).Count() > 0;
            buttonExtTile.Enabled = uccrlist.Count > 0;
        }

        public override void Display(HistoryEntry current, HistoryList history)
        {
            foreach (UserControlContainerResizable r in uccrlist)
            {
                UserControlCommonBase uc = (UserControlCommonBase)r.control;
                uc.Display(current, history);
            }
        }

        public override void Closing()
        {
            System.Diagnostics.Debug.WriteLine("Grid Saving to " + DbWindows);
            string s = "", p = "";
            foreach (UserControlContainerResizable r in uccrlist)   // save in uccr list
            {
                UserControlCommonBase uc = (UserControlCommonBase)r.control;

                s = s.AppendPrePad(uc.GetType().Name,",");
                p = p.AppendPrePad(r.Location.X + "," + r.Location.Y + "," + r.Size.Width + "," + r.Size.Height, ",");

                System.Diagnostics.Debug.WriteLine("  Save " + uc.GetType().Name);

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
            System.Diagnostics.Debug.WriteLine("---- END Grid Saving to " + DbWindows);
        }

        #region Open/Close

        private UserControlContainerResizable CreatePanel(UserControlCommonBase uccb , Point pos, Size size)
        {
            UserControlContainerResizable uccr = new UserControlContainerResizable();
            uccr.Init(uccb);
            uccr.ResizeStart += ResizeStart;
            uccr.ResizeEnd += ResizeEnd;
            uccr.BorderColor = discoveryForm.theme.GridBorderLines;
            uccr.SelectedBorderColor = discoveryForm.theme.TextBlockHighlightColor;

            uccrlist.Add(uccr);
            System.Diagnostics.Debug.WriteLine("  Create " + uccb.GetType().Name);

            int numopenedinside = uccrlist.Count(x => x.GetType().Equals(uccb.GetType()));    // how many others are there?

            int dnum = 1050 + displaynumber * 100 + numopenedinside;
            System.Diagnostics.Debug.WriteLine("  Add " + uccb.GetType().Name + " " + dnum);

            panelPlayfield.Controls.Add(uccr);

            uccb.Init(discoveryForm, uctg_inuse, dnum);
            uccb.LoadLayout();

            uccr.Font = discoveryForm.theme.GetFont;        // Important. Apply font autoscaling to the user control
                                                            // ApplyToForm does not apply the font to the actual UC, only
                                                            // specific children controls.  The TabControl in the discoveryform ends up autoscaling most stuff
                                                            // the children directly attached to the discoveryform are not autoscaled

            discoveryForm.theme.ApplyToControls(uccr, discoveryForm.theme.GetFont);

            uccr.Location = pos;
            uccr.Size = size;

            uccb.Display(discoveryForm.TravelControl.GetTravelHistoryCurrent, discoveryForm.history);

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
            UserControlTravelGrid uctgfound = (v != null) ? (v.control as UserControlTravelGrid) : null;    // if found, set to it

            if ( (uctgfound != null && !Object.ReferenceEquals(uctgfound,uctg_inuse) ) ||    // if got one but its not the one currently in use
                 (uctgfound == null && !Object.ReferenceEquals(uctg_history,uctg_inuse))    // or not found, but we are not on the history one
                )
            { 
                uctg_inuse = (uctgfound != null) ? uctgfound : uctg_history;    // select

                foreach (UserControlContainerResizable u in uccrlist)
                    ((UserControlCommonBase)u.control).ChangeTravelGrid(uctg_inuse);
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
        private void comboBoxGridSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserControlContainerResizable uccr = CreatePanel(PopOutControl.Create((PopOutControl.PopOuts)comboBoxGridSelector.SelectedIndex) ,
                                        new Point((uccrlist.Count % 5) * 50, (uccrlist.Count % 5) * 50),
                                        new Size(Math.Min(300, panelPlayfield.Width - 10), Math.Min(300, panelPlayfield.Height - 10)));
            Select(null);
            uccr.Selected = true;
            uccr.BringToFront();
            UpdateButtons();
            AssignTHC();
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
                r.BorderColor = on ? Color.Transparent : discoveryForm.theme.GridBorderLines;
                UserControlCommonBase uc = (UserControlCommonBase)r.control;
                uc.SetTransparency(on, curcol);
            }
        }

        #endregion

    }
}
