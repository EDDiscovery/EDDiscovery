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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class RichTextBoxBorder : RichTextBox
    {
        public Color BorderColor { get; set; } = Color.Transparent;
        public int BorderPadding { get; set; } = 1;
        public int BorderSize { get; set; } = 1;

        public RichTextBoxBorder() : base()
        {
            Debug.Assert(false, "Depreciated - use RichTextBoxScroll");
        }

        private const int WM_PAINT = 15;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_PAINT)          // Stupid control does not have a OnPaint
            {
                if (BorderColor != Color.Transparent)
                {
                    Graphics g = Parent.CreateGraphics();

                    Pen p = new Pen(BorderColor);
                    Pen b = new Pen(BackColor);
                    int offl = (this.BorderStyle != BorderStyle.None) ? 1 : 1;
                    int offr = (this.BorderStyle != BorderStyle.None) ? 5 : 1;

                    Rectangle rect = new Rectangle(Location.X - offl, Location.Y - offl, ClientRectangle.Width + offr, ClientRectangle.Height + offr);

                    for (int i = 0; i < BorderPadding; i++)
                    {
                        g.DrawRectangle(b, rect);
                        rect.Inflate(1, 1);
                    }
                    for (int i = 0; i < BorderSize; i++)
                    {
                        g.DrawRectangle(p, rect);
                        rect.Inflate(1, 1);
                    }

                    p.Dispose();
                    b.Dispose();
                    g.Dispose();
                }
            }
        }
    }
}
