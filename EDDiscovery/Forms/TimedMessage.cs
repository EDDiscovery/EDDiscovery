/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.Forms
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
            labelMessage.TextAlign = ContentAlignment.MiddleCenter;

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

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            ontimer.Start();
        }

        private void Timedout(object sender, EventArgs e)                 // tick.. tock.. every X ms.  Drives everything now.
        {
            Close();
        }
    }

}
