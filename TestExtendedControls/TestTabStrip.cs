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
    public partial class TestTabStrip : Form
    {
        public TestTabStrip()
        {
            InitializeComponent();
            tabStrip1.Images = new Bitmap[] {
                DialogTest.Properties.Resources.galaxy_red, DialogTest.Properties.Resources.galaxy_gray,
                DialogTest.Properties.Resources.galaxy_black, DialogTest.Properties.Resources.galaxy_gray,
                DialogTest.Properties.Resources.galaxy_black, DialogTest.Properties.Resources.galaxy_gray,
                DialogTest.Properties.Resources.galaxy_black, DialogTest.Properties.Resources.galaxy_gray,
                DialogTest.Properties.Resources.galaxy_black, DialogTest.Properties.Resources.galaxy_gray,
                DialogTest.Properties.Resources.galaxy_black, DialogTest.Properties.Resources.galaxy_gray,
                DialogTest.Properties.Resources.galaxy_white, DialogTest.Properties.Resources.galaxy_gray,
                DialogTest.Properties.Resources.galaxy_gray,DialogTest.Properties.Resources.galaxy,
                                            };
            tabStrip1.ToolTips = new string[] { "icon 1", "icon 2",
                "icon 2", "icon 3",
                "icon 4", "icon 5",
                "icon 6", "icon 7",
                "icon 8", "icon 9",
                "icon 10", "icon 11",
                "icon 12", "icon 13",
                "icon 14", "icon 15",
                "icon 16", "icon 17",
                "icon 18", "icon 19",
            };
            tabStrip1.SetControlText("Test tab");
        }
    }
}
