using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace EDDiscovery.UserControls
{
    public partial class UserControlStats : UserControlCommonBase
    {
        private int displaynumber = 0;
        private EDDiscoveryForm discoveryform;
        private TravelHistoryControl travelhistorycontrol;

        public UserControlStats()
        {
            InitializeComponent();
        }

        public override void Init(TravelHistoryControl thc, int vn) //0=primary, 1 = first windowed version, etc
        {
            travelhistorycontrol = thc;
            discoveryform = thc._discoveryForm;
            displaynumber = vn;
            discoveryform.OnNewEntry += AddNewEntry;
            thc.OnTravelSelectionChanged += SelectionChanged;

            vScrollBarCustom.Scroll += new System.Windows.Forms.ScrollEventHandler(OnScrollBarChanged);
        }

        public override void Closing()
        {
            discoveryform.OnNewEntry -= AddNewEntry;
            travelhistorycontrol.OnTravelSelectionChanged -= SelectionChanged;
        }

        protected virtual void OnScrollBarChanged(object sender, ScrollEventArgs e)
        {
            int percent = e.NewValue;
            panelData.AutoScrollPosition = new Point(0, percent);       // this is the one to change with AutoScroll off
           // System.Diagnostics.Debug.WriteLine("Scroll {0} {1} {2}", percent , panelData.AutoScrollPosition.X, panelData.AutoScrollPosition.Y);
        }

        private void AddNewEntry(HistoryEntry he, HistoryList hl)
        {
            Stats(he,hl);
        }

        public void SelectionChanged(HistoryEntry he, HistoryList hl)
        {
            Stats(he, hl);
        }

        private void Stats(HistoryEntry he, HistoryList hl)
        {
            dataGridViewStats.Rows.Clear();

            if (he != null)
            {
                StatToDGV("Visits", hl.GetVisitsCount(he.System.name, he.System.id_edsm) + " to system " + he.System.name);
                StatToDGV("Jumps Before System", hl.GetFSDJumpsBeforeUTC(he.EventTimeUTC));
            }

            int totaljumps = hl.GetFSDJumps(new TimeSpan(10000, 0, 0, 0));
            StatToDGV("Total No of jumps: ", totaljumps);
            if (totaljumps > 0)
            {
                StatToDGV("Jump History", "24 Hours: " + hl.GetFSDJumps(new TimeSpan(1, 0, 0, 0)) +
                                      ", One Week: " + hl.GetFSDJumps(new TimeSpan(7, 0, 0, 0)) +
                                      ", 30 Days: " + hl.GetFSDJumps(new TimeSpan(30, 0, 0, 0)) +
                                      ", One Year: " + hl.GetFSDJumps(new TimeSpan(365, 0, 0, 0))
                                      );

                HistoryEntry north = hl.GetConditionally(Double.MinValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDJump && s.System.HasCoordinate && s.System.z > l; if (v) l = s.System.z; return v; } );

                HistoryEntry south = hl.GetConditionally(Double.MaxValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDJump && s.System.HasCoordinate && s.System.z < l; if (v) l = s.System.z; return v;} );

                HistoryEntry east = hl.GetConditionally(Double.MinValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDJump && s.System.HasCoordinate && s.System.x > l; if (v) l = s.System.x; return v; } );

                HistoryEntry west = hl.GetConditionally(Double.MaxValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDJump && s.System.HasCoordinate && s.System.x < l; if (v) l = s.System.x; return v; } );

                HistoryEntry up = hl.GetConditionally(Double.MinValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDJump && s.System.HasCoordinate && s.System.y > l; if (v) l = s.System.y; return v; } );

                HistoryEntry down = hl.GetConditionally(Double.MaxValue, (HistoryEntry s, ref double l) =>
                { bool v = s.IsFSDJump && s.System.HasCoordinate && s.System.y < l; if (v) l = s.System.y; return v; } );

                StatToDGV("Most North", north.System.name + " @ " + north.System.x.ToString("0.0") + "," + north.System.y.ToString("0.0") + "," + north.System.z.ToString("0.0"));
                StatToDGV("Most South", south.System.name + " @ " + south.System.x.ToString("0.0") + "," + south.System.y.ToString("0.0") + "," + south.System.z.ToString("0.0"));
                StatToDGV("Most East", east.System.name + " @ " + east.System.x.ToString("0.0") + "," + east.System.y.ToString("0.0") + "," + east.System.z.ToString("0.0"));
                StatToDGV("Most West", west.System.name + " @ " + west.System.x.ToString("0.0") + "," + west.System.y.ToString("0.0") + "," + west.System.z.ToString("0.0"));
                StatToDGV("Most Highest", up.System.name + " @ " + up.System.x.ToString("0.0") + "," + up.System.y.ToString("0.0") + "," + up.System.z.ToString("0.0"));
                StatToDGV("Most Lowest", down.System.name + " @ " + down.System.x.ToString("0.0") + "," + down.System.y.ToString("0.0") + "," + down.System.z.ToString("0.0"));


                var groupeddata = from data in hl.OrderByDate
                                  where data.IsFSDJump
                                  group data by data.System.name
                                      into grouped
                                  select new
                                  {
                                      Title = grouped.Key,
                                      Count = grouped.Count()
                                  };

                mostVisited.Visible = true;

                Color GridC = discoveryform.theme.GridCellText;
                Color TextC = discoveryform.theme.VisitedSystemColor;
                mostVisited.Titles.Clear();
                mostVisited.Titles.Add(new Title("Most Visited", Docking.Top, discoveryform.theme.GetFont, TextC));
                mostVisited.Series[0].Points.Clear();

                mostVisited.ChartAreas[0].AxisX.LabelStyle.ForeColor = TextC;
                mostVisited.ChartAreas[0].AxisY.LabelStyle.ForeColor = TextC;
                mostVisited.ChartAreas[0].AxisX.MajorGrid.LineColor = GridC;
                mostVisited.ChartAreas[0].AxisX.MinorGrid.LineColor = GridC;
                mostVisited.ChartAreas[0].AxisY.MajorGrid.LineColor = GridC;
                mostVisited.ChartAreas[0].AxisY.MinorGrid.LineColor = GridC;
                mostVisited.ChartAreas[0].BorderColor = GridC;
                mostVisited.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
                mostVisited.ChartAreas[0].BorderWidth = 2;

                mostVisited.ChartAreas[0].BackColor = Color.Transparent;
                mostVisited.Series[0].Color = GridC;
                mostVisited.BorderlineColor = Color.Transparent;

                int i = 0;
                foreach (var data in from a in groupeddata orderby a.Count descending select a)
                {
                    if (data.Count <= 1 || i == 10)
                        break;

                    mostVisited.Series[0].Points.Add(new DataPoint(i, data.Count));
                    mostVisited.Series[0].Points[i].AxisLabel = data.Title;
                    mostVisited.Series[0].Points[i].LabelForeColor = TextC;
                    i++;
                }
            }
            else
                mostVisited.Visible = false;

            SizeControls();
        }

        void StatToDGV(string title, string data)
        {
            object[] rowobj = { title, data };
            dataGridViewStats.Rows.Add(rowobj);
        }

        void StatToDGV(string title, int data)
        {
            object[] rowobj = { title, data.ToString() };
            dataGridViewStats.Rows.Add(rowobj);
        }

        void SizeControls()
        {
            int height = 0;
            foreach (DataGridViewRow row in dataGridViewStats.Rows)
            {
                height += row.Height + 1;
            }
            height += dataGridViewStats.ColumnHeadersHeight+2;
            dataGridViewStats.ClientSize = new Size(panelData.Width, height);

            mostVisited.Location = new Point(0, height );
            mostVisited.Size = new Size(panelData.Width, mostVisited.Height);

            int maxh = 0;
            foreach (Control c in panelData.Controls)
            {
                int bot = c.Location.Y + c.Height;
                if (bot > maxh)
                    maxh = bot;
            }

            vScrollBarCustom.Maximum = maxh - panelData.Height + vScrollBarCustom.LargeChange + 16;
            vScrollBarCustom.Minimum = 0;
            //System.Diagnostics.Debug.WriteLine("UC H {0} Max H {1} vscroll {2}", ClientRectangle.Height, maxh, vScrollBarCustom.Maximum);
        }

        private void UserControlStats_Resize(object sender, EventArgs e)
        {
            panelData.Size = new Size(this.ClientRectangle.Width - vScrollBarCustom.Width,this.ClientRectangle.Height);
            SizeControls();
        }
    }
}
