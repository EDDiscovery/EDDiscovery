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
            compassControl1.Bug = 358;
            compassControl1.ShowNegativeDegrees = false;
            compassControl1.CompassHeightPercentage = 60;
            compassControl1.TickHeightPercentage = 60;
            compassControl1.CentreTickHeightPercentage = 100;
            compassControl1.Distance = 100.2;
            compassControl1.DistanceFormat = "{0:0.##} km";
           
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
    }
}
