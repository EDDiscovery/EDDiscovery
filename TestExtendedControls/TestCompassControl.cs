using ExtendedControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogTest
{
    public partial class TestCompassControl : Form
    {
        System.Windows.Forms.Timer t = new Timer();

        int bearing = 0;
        public TestCompassControl()
        {
            InitializeComponent();
            t.Interval = 50;
            t.Tick += T_Tick;

            compassControl1.WidthDegrees = 180;
            compassControl1.Bearing = bearing = 260;
            //compassControl1.Bug = double.NaN;
            compassControl1.Bug = 100;
            compassControl1.ShowNegativeDegrees = false;
            compassControl1.CompassHeightPercentage = 60;
            compassControl1.TickHeightPercentage = 60;
            compassControl1.CentreTickHeightPercentage = 100;
            compassControl1.Distance = 100.2;
            compassControl1.DistanceFormat = "{0:0.##} km";
            compassControl1.SlewRateDegreesSec = 40;

            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker1.ValueChanged += DateTimePicker1_ValueChanged;
           
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime p = dateTimePicker1.Value;
            DateTime pp = new DateTime(p.Year, p.Month, p.Day, p.Hour, p.Minute, p.Second, DateTimeKind.Local);
            DateTime utc = p.ToUniversalTime();
            DateTime local = p.ToLocalTime();
            System.Diagnostics.Debug.WriteLine("Time is " + dateTimePicker1.Value);

        }

        private void T_Tick(object sender, EventArgs e)
        {
            Incr(1);
        }

        void Incr(int v)
        { 
            bearing += v;
            if (compassControl1.ShowNegativeDegrees)
            {
                if (bearing >= 180)
                    bearing -= 360;
                else if (bearing < 180)
                    bearing += 360;
            }
            else
                bearing = (bearing+360) % 360;

            compassControl1.Bearing = bearing;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Incr(5);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Incr(-5);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            t.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            t.Stop();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Incr(1);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Incr(-1);

        }

        private void button0_Click(object sender, EventArgs e)
        {
            compassControl1.SlewToBearing = 0;

        }

        private void button90_Click(object sender, EventArgs e)
        {
            compassControl1.SlewToBearing = 90;

        }

        private void button180_Click(object sender, EventArgs e)
        {
            compassControl1.SlewToBearing = 180;

        }

        private void button270_Click(object sender, EventArgs e)
        {
            compassControl1.SlewToBearing = 270;

        }

        private void button359_Click(object sender, EventArgs e)
        {
            compassControl1.SlewToBearing = 359;
        }

        private void buttonbugoff_Click(object sender, EventArgs e)
        {
            compassControl1.Bug = double.NaN;
        }

        private void buttonbug80_Click(object sender, EventArgs e)
        {
            compassControl1.Bug = 80;

        }

        private void buttonbug340_Click(object sender, EventArgs e)
        {
            compassControl1.Bug = 340;

        }

        private void buttondistoff_Click(object sender, EventArgs e)
        {
            compassControl1.Distance = double.NaN;
        }

        private void buttondist1022_Click(object sender, EventArgs e)
        {
            compassControl1.Distance = 102.2;
        }

        private void buttonresize_Click(object sender, EventArgs e)
        {
            compassControl1.Font = new Font(compassControl1.Font.Name, 16);
            compassControl1.CompassHeightPercentage = 80;
            compassControl1.Size = new Size(800, 200);
        }

        private void buttonresizesmall_Click(object sender, EventArgs e)
        {
            compassControl1.Font = new Font(compassControl1.Font.Name, 8);
            compassControl1.CompassHeightPercentage = 80;
            compassControl1.Size = new Size(200, 100);
            compassControl1.StencilMajorTicksAt = 30;

        }

        private void button90view_Click(object sender, EventArgs e)
        {
            compassControl1.WidthDegrees = 90;
        }

        private void button180view_Click(object sender, EventArgs e)
        {
            compassControl1.WidthDegrees = 180;
        }

        private void button270view_Click(object sender, EventArgs e)
        {
            compassControl1.WidthDegrees = 270;

        }

        private void button360view_Click(object sender, EventArgs e)
        {
            compassControl1.WidthDegrees = 360;

        }

        private void buttondistdisb_Click(object sender, EventArgs e)
        {
            compassControl1.DistanceDisable("No Distance data");
        }

        private void buttonenable_Click(object sender, EventArgs e)
        {
            compassControl1.Enabled = true;
        }

        private void buttondisable_Click(object sender, EventArgs e)
        {
            compassControl1.DisableMessage = "Compass is disabled";
            compassControl1.Enabled = false;
        }
    }
}
