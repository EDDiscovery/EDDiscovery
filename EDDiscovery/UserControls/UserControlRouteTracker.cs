using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EDDiscovery.DB;

namespace EDDiscovery.UserControls
{
    public partial class UserControlRouteTracker :   UserControlCommonBase
    {

        private EDDiscoveryForm discoveryform;
        private int displaynumber = 0;
        private Font displayfont;
        private string DbSave { get { return "RouteTracker" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private SavedRouteClass _currentRoute;
        private List<SavedRouteClass> _savedRoutes = new List<SavedRouteClass>();
        private  HistoryEntry lastHE;
        private string lastsystem;

        public UserControlRouteTracker()
        {
            InitializeComponent();
        }

        public override void Closing()
        {
            SQLiteDBClass.PutSettingBool(DbSave + "autoCopyWP", autoCopyWPToolStripMenuItem.Checked);
            SQLiteDBClass.PutSettingBool(DbSave + "autoSetTarget", autoSetTargetToolStripMenuItem.Checked);
            discoveryform.OnHistoryChange -= Display;
            discoveryform.OnNewEntry -= NewEntry;
            discoveryform.OnNewTarget -= NewTarget;
        }

        public override Color ColorTransparency { get { return Color.Green; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            pictureBox.BackColor = this.BackColor = curcol;
            updateScreen();
        }
        
        public override void Init(EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            displaynumber = vn;

            discoveryform.OnHistoryChange += Display;
            discoveryform.OnNewEntry += NewEntry;
            discoveryform.OnNewTarget += NewTarget;

            displayfont = discoveryform.theme.GetFont;

            autoCopyWPToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "autoCopyWP", false);
            autoSetTargetToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "autoSetTarget", false);

            _savedRoutes = SavedRouteClass.GetAllSavedRoutes();
            String selRoute = SQLiteDBClass.GetSettingString(DbSave + "SelectedRoute" ,"-1");
            long id = long.Parse(selRoute);
            _currentRoute = _savedRoutes.Find(r => r.Id.Equals(id));
            updateScreen();
        }

        public override void Display(HistoryEntry current, HistoryList history)
        {
            Display(history);
        }


        public void Display(HistoryList hl)            // when user clicks around..  HE may be null here
        {
            lastHE = hl.GetLastFSD;
            updateScreen();
        }

        private void NewEntry(HistoryEntry l, HistoryList hl)
        {
            lastHE = hl.GetLastFSD;
            updateScreen();
        }

        private void NewTarget()
        {
           //updateScreen();
        }


        static class Prompt
        {
            public static Boolean ShowDialog(Form p, 
                List<SavedRouteClass> savedRoutes, 
                String defaultValue,
                string caption,
                out SavedRouteClass selectedRoute)
            {
                Form prompt = new Form()
                {
                    Width = 440,
                    Height = 160,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen,
                };

                Label textLabel = new Label() { Left = 10, Top = 20, Width = 400, Text = "Route" };
                Button confirmation = new Button() { Text = "Ok", Left = 245, Width = 80, Top = 90, DialogResult = DialogResult.OK };
                Button cancel = new Button() { Text = "Cancel", Left = 330, Width = 80, Top = 90, DialogResult = DialogResult.Cancel };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                cancel.Click += (sender, e) => { prompt.Close(); };
                ComboBox cb = new ComboBox() { Left = 10, Top = 50, Width = 400 };
                foreach(SavedRouteClass src in savedRoutes)
                {
                    cb.Items.Add(src.Name);
                    if (src.Name.Equals(defaultValue))
                    {
                        cb.SelectedItem = defaultValue;
                    }
                }
                
                prompt.Controls.Add(cb);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(cancel);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;

               var  res = prompt.ShowDialog(p); 
                selectedRoute = (cb.SelectedIndex != -1) ? savedRoutes[cb.SelectedIndex] : null;
                return (res  == DialogResult.OK && selectedRoute != null);
            }
        }

        private void setRouteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SavedRouteClass selectedRoute;
            if (Prompt.ShowDialog(discoveryform, _savedRoutes, _currentRoute!=null? _currentRoute.Name:"", "Select route", out selectedRoute))
            {
                if (selectedRoute == null)
                    return;
                _currentRoute = selectedRoute;
                SQLiteDBClass.PutSettingString(DbSave + "SelectedRoute", selectedRoute.Id.ToString());
                updateScreen();
            }
        }

        private void updateScreen()
        {
            if (lastHE == null)
                return;

            pictureBox.ClearImageList();
            Color textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
            Color backcolour = IsTransparent ? Color.Black : this.BackColor;
            backcolour = IsTransparent ? Color.Transparent : this.BackColor;

            String topline = "Please set a route, by right clicking";
            string bottomLine = "";
            if (_currentRoute != null)
            {
                topline = _currentRoute.Name;
                if (_currentRoute.Systems.Count == 0)
                    return;
                SystemClass scX = SystemClass.GetSystem(_currentRoute.Systems[_currentRoute.Systems.Count -1]);
                if (scX != null)
                {
                    double distX = SystemClass.Distance(lastHE.System, scX);
                    //Small hack to pull the jump range from TripPanel1
                    var jumpRange = SQLiteDBClass.GetSettingDouble("TripPanel1" + "JumpRange", -1.0);
                    string mesg = "remain";
                    if (jumpRange > 0)
                    {
                        int jumps = (int)Math.Ceiling(distX / jumpRange);
                        if (jumps > 0)
                            mesg = "@ " + jumps.ToString() + ((jumps == 1) ? " jump" : " jumps");
                    }

                    topline += String.Format(" {0} WPs {1:N2}ly {2}", _currentRoute.Systems.Count, distX, mesg);
                }
                //topline += " ," + lastHE.System.name;
                //String nearestSystem = "";
                double minDist = double.MaxValue;
                int nearestidx = -1;
                SystemClass nearest  =null;
                for (int i = 0; i < _currentRoute.Systems.Count; i++)
                {
                    String sys = _currentRoute.Systems[i];
                    SystemClass sc = SystemClass.GetSystem(sys);
                    if (sc == null)
                        continue;
                    double dist = SystemClass.Distance(lastHE.System, sc);
                    if (dist <= minDist )
                    {
                        if (nearest == null || !nearest.name.Equals(sc.name))
                        {
                            minDist = dist;
                            nearestidx = i;
                            nearest = sc;
                        }
                    }
                }
                if (nearest!=null )
                {
                    string name = null;
                    double dist = 0.0;
                    int wp = 0;
                   
                    if (nearestidx < _currentRoute.Systems.Count -1)
                    {
                        SystemClass next = SystemClass.GetSystem(_currentRoute.Systems[nearestidx + 1]);
                        double me2next = SystemClass.Distance(lastHE.System, next);
                        double nearest2next = SystemClass.Distance(nearest, next);
                        if(me2next > nearest2next)
                        {
                            dist = minDist;
                            wp = nearestidx + 1;
                            name = nearest.name;
                        }
                        else
                        {
                            dist = me2next;
                            wp = nearestidx + 2;
                            name = next.name;
                        }
                    }
                    else
                    {
                        dist = minDist;
                        wp = nearestidx + 1;
                        name = nearest.name;
                    }
                    bottomLine = String.Format("{0:N2}ly to WP{1}: {2} {3}", 
                        dist, wp, name, 
                        autoCopyWPToolStripMenuItem.Checked?" (AUTO)":"");
                    if (name.CompareTo(lastsystem) != 0)
                    {
                        if (autoCopyWPToolStripMenuItem.Checked)
                            Clipboard.SetText(name);
                        if (autoSetTargetToolStripMenuItem.Checked)
                        {
                            string targetName;
                            double x, y, z;
                            TargetClass.GetTargetPosition(out targetName, out x, out y, out z);
                            if (name.CompareTo(targetName) != 0)
                            {
                                RoutingUtils.setTargetSystem(discoveryform, name, false);
                            }
                        }
                    }
                    lastsystem = name;
                }
            }
            pictureBox.AddTextAutoSize(new Point(10, 5), new Size(1000, 35), topline, displayfont, textcolour, backcolour, 1.0F);
            pictureBox.AddTextAutoSize(new Point(10, 35), new Size(1000, 35), bottomLine, displayfont, textcolour, backcolour, 1.0F);
            pictureBox.Render();
        }

        private void autoCopyWPToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            updateScreen();
        }
    }
}
