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
    public partial class TestCompositeButton : Form
    {
        public TestCompositeButton()
        {
            InitializeComponent();
            compositeButton1.TextBackColor = Color.White;
            //compositeButton1.TextBackground = Color.Green;
            compositeButton1.Font = new Font("Euro caps", 12);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            compositeButton1.Size = new Size(128, 128);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            compositeButton1.Size = new Size(400, 400);

        }
    }
}
