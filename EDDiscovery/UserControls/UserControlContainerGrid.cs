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
        EDDiscoveryForm discoveryForm;
        UserControlTravelGrid uctg;     // one passed to us, refers to thc.uctg
        int displaynumber;

        List<UserControlContainerResizable> list = new List<UserControlContainerResizable>();

        private string DbWindows { get { return "GridControlWindows" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbPositions { get { return "GridControlPositons" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        public UserControlContainerGrid()
        {
            InitializeComponent();
            comboBoxGridSelector.Items.AddRange(PopOutControl.GetPopOutNames());
        }

        public override void Init( EDDiscoveryForm f , UserControlTravelGrid thc, int dn )       //dn = 0 primary grid, or 1 first pop out, etc
        {
            discoveryForm = f;
            uctg = thc;
            displaynumber = dn;
        }

        bool checkmulticall = false;

        public override void LoadLayout()
        {
            System.Diagnostics.Debug.Assert(checkmulticall == false);
            checkmulticall = true;      // examples seen of multi call, lets trap it

            string ret = SQLiteConnectionUser.GetSettingString(DbWindows, "");
            string pos = SQLiteConnectionUser.GetSettingString(DbPositions, "");
            System.Diagnostics.Debug.WriteLine("Grid Restore from " + DbWindows);
            if (ret.Length > 0 && pos.Length > 0)
            {
                string[] names = ret.Split(',');
                string[] positions = pos.Split(',');
                int ppos = 0;

                foreach (string n in names)
                {
                    Type t = Type.GetType("EDDiscovery.UserControls." + n);
                    int x, y, w, h;
                    if (t != null && positions[ppos++].InvariantParse(out x) &&
                                        positions[ppos++].InvariantParse(out y) &&
                                        positions[ppos++].InvariantParse(out w) &&
                                        positions[ppos++].InvariantParse(out h))
                    {
                        UserControlCommonBase uccb = (UserControlCommonBase)Activator.CreateInstance(t);
                        OpenPanel(uccb, new Point(x, y), new Size(w, h));
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine("----- Grid Restore END " + DbWindows);
        }

        public override void Display(HistoryEntry current, HistoryList history)
        {
            foreach (UserControlContainerResizable r in list)
            {
                UserControlCommonBase uc = (UserControlCommonBase)r.control;
                uc.Display(current, history);
            }
        }

        public override void Closing()
        {
            System.Diagnostics.Debug.WriteLine("Grid Saving to " + DbWindows);
            string s = "", p = "";
            foreach (UserControlContainerResizable r in list)
            {
                UserControlCommonBase uc = (UserControlCommonBase)r.control;

                s += uc.GetType().Name + ",";
                p += r.Location.X + "," + r.Location.Y + "," + r.Size.Width + "," + r.Size.Height + ",";

                System.Diagnostics.Debug.WriteLine("  Save " + uc.GetType().Name);

                uc.Closing();
            }

            SQLiteConnectionUser.PutSettingString(DbWindows, s);
            SQLiteConnectionUser.PutSettingString(DbPositions, p);
            System.Diagnostics.Debug.WriteLine("---- END Grid Saving to " + DbWindows);
        }

        #region Open/Close

        public void OpenPanel(PopOutControl.PopOuts sel)
        {
            int index = list.Count % 10;
            OpenPanel(PopOutControl.Create(sel) , new Point(index*50,index*50), new Size(300,300));
        }

        public void OpenPanel(UserControlCommonBase uccb, Point pos , Size size)
        {
            SuspendLayout();

            if (uccb != null)
            {
                Select(null);

                UserControlContainerResizable uccr = new UserControlContainerResizable();
                uccr.Init(uccb);
                uccr.ResizeStart += ResizeStart;

                int numopenedinside = list.Count(x => x.GetType().Equals(uccb.GetType()));    // how many others are there?

                list.Add(uccr);
                panelPlayfield.Controls.Add(uccr);

                uccr.Location = pos;
                uccr.Size = size;

                uccr.Selected = true;

                //displayno = 0 main grid control, or 1,2,3,4 dependent if its a pop out grid.  Move that to 10000+n*1000 and add on number of ones opened by us
                int dnum = 10000 + displaynumber * 1000 + numopenedinside;
                System.Diagnostics.Debug.WriteLine("  Grid Open " + uccb.GetType().Name + " " + dnum);

                uccb.Init(discoveryForm, uctg, dnum);
                uccb.LoadLayout();

                uccr.Font = discoveryForm.theme.GetFont;        // Important. Apply font autoscaling to the user control
                                                                // ApplyToForm does not apply the font to the actual UC, only
                                                                // specific children controls.  The TabControl in the discoveryform ends up autoscaling most stuff
                                                                // the children directly attached to the discoveryform are not autoscaled

                discoveryForm.theme.ApplyToControls(uccr, discoveryForm.theme.GetFont);

                uccb.Display(discoveryForm.TravelControl.GetTravelHistoryCurrent, discoveryForm.history);
            }

            ResumeLayout();
        }

        public void ClosePanel( UserControlContainerResizable uccr )
        {
            UserControlCommonBase uc = (UserControlCommonBase)uccr.control;
            uc.Closing();
            panelPlayfield.Controls.Remove(uccr);
            list.Remove(uccr);
            Invalidate();
        }

        private void Select( UserControlContainerResizable uccr )
        {
            foreach( UserControlContainerResizable r in list)
            {
                if (Object.ReferenceEquals(r, uccr))
                {
                    if (!r.Selected)
                        r.Selected = true;
                }
                else if (r.Selected)
                    r.Selected = false;
            }
        }

        #endregion

        #region Panel reactions

        private void ResizeStart( UserControlContainerResizable uccr)
        {
            uccr.BringToFront();
            Select(uccr);
        }

        #endregion

        #region Clicks
        private void comboBoxGridSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            OpenPanel((PopOutControl.PopOuts)comboBoxGridSelector.SelectedIndex);
        }

        private void buttonExtDelete_Click(object sender, EventArgs e)
        {
            UserControlContainerResizable sel = list.Find(x => x.Selected);

            if ( sel != null )
            {
                ClosePanel(sel);
            }
        }


        #endregion

        public override Color ColorTransparency { get { return Color.Green; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            this.BackColor = curcol;
            panelPlayfield.BackColor = curcol;
            rollUpPanel1.BackColor = curcol;
            rollUpPanel1.ShowHiddenMarker = !on;
        }

    }
}
