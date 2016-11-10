using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlStats : UserControlCommonBase
    {
        private int displaynumber = 0;
        private TravelHistoryControl travelhistorycontrol;

        public UserControlStats()
        {
            InitializeComponent();
        }

        public override void Init(TravelHistoryControl thc, int vn) //0=primary, 1 = first windowed version, etc
        {
            travelhistorycontrol = thc;
            displaynumber = vn;
            thc.OnHistoryChange += Display;
            thc.OnNewEntry += AddNewEntry;

            vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(OnScrollBarChanged);
        }

        public override void Closing()
        {
            travelhistorycontrol.OnHistoryChange -= Display;
            travelhistorycontrol.OnNewEntry -= AddNewEntry;
        }

        protected virtual void OnScrollBarChanged(object sender, ScrollEventArgs e)
        {
            int percent = e.NewValue;
            panelData.AutoScrollPosition = new Point(0, percent);       // this is the one to change with AutoScroll off
            System.Diagnostics.Debug.WriteLine("Scroll {0} {1} {2}", percent , panelData.AutoScrollPosition.X, panelData.AutoScrollPosition.Y);
        }

        public void Display(HistoryList hl)
        {
            Stats(hl);
        }

        private void AddNewEntry(HistoryEntry he, HistoryList hl)
        {
            Stats(hl);
        }

        private void Stats(HistoryList hl)
        {
            dataGridViewStats.Rows.Clear();

            StatToDGV("Total No of jumps: ", hl.GetFSDJumps(new TimeSpan(10000, 0, 0, 0)));
            StatToDGV("Last 24 hours: ", hl.GetFSDJumps(new TimeSpan(1, 0, 0, 0)));
            StatToDGV("Last Week: ", hl.GetFSDJumps(new TimeSpan(7, 0, 0, 0)));
            StatToDGV("Last 30 days: ", hl.GetFSDJumps(new TimeSpan(30, 0, 0, 0)));
            StatToDGV("Last year: ", hl.GetFSDJumps(new TimeSpan(365, 0, 0, 0)));

            SizeDGV();
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

        void SizeDGV()
        {
            int height = 0;
            foreach (DataGridViewRow row in dataGridViewStats.Rows)
            {
                height += row.Height + 1;
            }
            height += dataGridViewStats.ColumnHeadersHeight+2;
            dataGridViewStats.ClientSize = new Size(panelData.Width, height);

            chart1.Location = new Point(0, height );
            

        }

        private void UserControlStats_Resize(object sender, EventArgs e)
        {
            panelData.Size = new Size(this.ClientRectangle.Width - vScrollBar.Width,this.ClientRectangle.Height);

            int maxh = 0;
            foreach( Control c in panelData.Controls)
            {
                int bot = c.Location.Y + c.Height;
                if (bot > maxh)
                    maxh = bot;
            }

            vScrollBar.Maximum = maxh - panelData.Height + vScrollBar.LargeChange + 16;
            vScrollBar.Minimum = 0;
            System.Diagnostics.Debug.WriteLine("UC H {0} Max H {1} vscroll {2}", ClientRectangle.Height, maxh , vScrollBar.Maximum);

            SizeDGV();
        }
    }
}
