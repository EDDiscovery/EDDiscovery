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
    public partial class TimedMessage : Form
    {
        public TimedMessage()
        {
            InitializeComponent();
        }

        private Timer ontimer = new Timer();     // kicks off star naming

        public void Init(string title, string msg, int timeout, bool noborder, float opacity ,
                         Color msgback, Color msgfore , Font fnt = null)
        {

            this.Text = title;
            if (fnt != null)
                labelMessage.Font = fnt;

            labelMessage.BackColor = msgback;
            labelMessage.TextBackColor = msgback;
            labelMessage.ForeColor = msgfore;
            labelMessage.Text = msg;
            labelMessage.CentreX = labelMessage.CentreY = true;

            ontimer.Interval = timeout;
            ontimer.Tick += new EventHandler(Timedout);

            Opacity = opacity;

            if (noborder)
            {
                this.BackColor = Color.Red;
                this.TransparencyKey = this.BackColor;
                this.FormBorderStyle = FormBorderStyle.None;
            }
            else
                this.BackColor = msgback;

            TopMost = true;
        }

        // xpos = +1 right, -1 left, 0 centre ypos = +1 top -1 bot 0 centre of parent
        // yoffper xoffper =  percentage of parent width to shift from indicated pos, can be negative
        // widthper/heightper = percentage of parent width, or 0 to measure string and fit

        public void Position(Control parent, int xpos, int xoffper, int ypos, int yoffper , int widthper, int heightper )
        {
            Rectangle loc = parent.RectangleToScreen( parent.ClientRectangle);

            if (widthper == 0 || heightper == 0)
            {
                using (Graphics g = CreateGraphics())
                {
                    SizeF sizef = g.MeasureString(labelMessage.Text, labelMessage.Font);

                    Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
                    int titleHeight = screenRectangle.Top - this.Top;
                    
                    Size = new Size((int)(sizef.Width + 16), (int)(sizef.Height + titleHeight + 16));
                }
            }
            else
                Size = new Size(loc.Width * widthper / 100, loc.Height * heightper / 100);

            int xref = (xpos < 0) ? loc.Left : ((xpos > 0) ? loc.Right : (loc.X + loc.Width / 2));
            int yref = (ypos < 0) ? loc.Bottom: ((ypos > 0) ? loc.Top : (loc.Y + loc.Height / 2));

            xref += loc.Width * xoffper / 100;
            yref += loc.Height * yoffper / 100;

            Location = new Point(xref - Width / 2, yref - Height / 2);

            labelMessage.Size = new Size(ClientRectangle.Width - 8, ClientRectangle.Height - 8);
        }

        public new void Show()
        {
            base.Show();
            ontimer.Start();
        }

        private void Timedout(object sender, EventArgs e)                 // tick.. tock.. every X ms.  Drives everything now.
        {
            Close();
        }
    }

}
