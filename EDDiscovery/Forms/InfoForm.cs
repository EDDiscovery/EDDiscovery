using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery2
{
    public partial class InfoForm : Form
    {
        public InfoForm()
        {
            InitializeComponent();
        }

        public void Info(string title, string info , Font fnt, int[] array )
        {
            Text = title;
            textBoxInfo.SelectionTabs = array;
            textBoxInfo.Text = info;
            textBoxInfo.Select(0, 0);
            textBoxInfo.Font = fnt;
        }

        private void InfoForm_Resize(object sender, EventArgs e)
        {
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void InfoForm_Layout(object sender, LayoutEventArgs e)
        {
            textBoxInfo.Location = new Point(2, 2);
            textBoxInfo.Size = new Size(ClientRectangle.Width - 4, ClientRectangle.Height - buttonOK.Size.Height*2);
            buttonOK.Location = new Point(ClientRectangle.Width - 100, ClientRectangle.Height - buttonOK.Size.Height*3/2);

        }
    }
}
