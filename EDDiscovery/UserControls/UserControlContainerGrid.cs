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

namespace EDDiscovery.UserControls
{
    public partial class UserControlContainerGrid: UserControl
    {
        EDDiscoveryForm discoveryForm;
        List<UserControlContainerResizable> list = new List<UserControlContainerResizable>();

        public UserControlContainerGrid()
        {
            InitializeComponent();
            comboBoxGridSelector.Items.AddRange(PopOutControl.spanelbuttonlist);
        }

        public void InitControl( EDDiscoveryForm f)
        {
            discoveryForm = f;
        }


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

                int numopened = list.Count(x => x.GetType().Equals(uccb.GetType()));    // how many others are there?

                list.Add(uccr);
                panelPlayfield.Controls.Add(uccr);

                uccr.Location = pos;
                uccr.Size = size;

                uccr.Selected = true;

                uccb.Init(discoveryForm, discoveryForm.TravelControl.GetTravelGrid, numopened + 2000);
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


        public void SaveSettings()
        {
            string s = "",p = "";
            foreach (UserControlContainerResizable r in list)
            {
                UserControlCommonBase uc = (UserControlCommonBase)r.control;
                s += uc.GetType().Name + ",";
                p += r.Location.X + "," + r.Location.Y + "," + r.Size.Width + "," + r.Size.Height + ",";
            }

            SQLiteConnectionUser.PutSettingString("GridControlWindows", s);
            SQLiteConnectionUser.PutSettingString("GridControlWindowsPos", p);
        }

        public void RestoreState()
        {
            string ret = SQLiteConnectionUser.GetSettingString("GridControlWindows", "");
            string pos = SQLiteConnectionUser.GetSettingString("GridControlWindowsPos", "");
            if ( ret.Length>0 && pos.Length>0)
            {
                string[] names = ret.Split(',');
                string[] positions = pos.Split(',');
                int ppos = 0;

                foreach ( string n in names)
                {
                    Type t = Type.GetType("EDDiscovery.UserControls." + n);
                    int x, y, w, h;
                    if ( t != null && positions[ppos++].InvariantParse(out x) &&
                                        positions[ppos++].InvariantParse(out y) &&
                                        positions[ppos++].InvariantParse(out w) &&
                                        positions[ppos++].InvariantParse(out h) )
                    {
                        UserControlCommonBase uccb = (UserControlCommonBase)Activator.CreateInstance(t);
                        OpenPanel(uccb , new Point(x,y) , new Size(w,h));
                    }
                }
            }
        }

        #endregion
    }
}
