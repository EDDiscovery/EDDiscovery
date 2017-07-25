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
using BaseUtils.Win32Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ExtendedControls
{
    public class TextBoxBorder : TextBox
    {
        public Color BorderColor { get; set; } = Color.Transparent;
        public float BorderColorScaling { get; set; } = 0.5F;           
        public int BorderOffset = 3;
        public new virtual bool Visible { get { return base.Visible; } set { base.Visible = value; if ( Parent != null) Parent.Invalidate(); } }

        public TextBoxBorder() : base()
        {
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM.PAINT)          // Stupid control does not have a OnPaint
            {
                if (!BorderColor.IsFullyTransparent())
                {
                    Graphics g = Parent.CreateGraphics();

                    Rectangle clientborder = new Rectangle(Location.X - BorderOffset, Location.Y - BorderOffset, ClientRectangle.Width + BorderOffset*2, ClientRectangle.Height + BorderOffset*2);

                    Color color1 = BorderColor;
                    Color color2 = BorderColor.Multiply(BorderColorScaling);
                    System.Diagnostics.Debug.WriteLine("Repaint border for " + this.Name);

                    GraphicsPath g1 = ControlHelpersStaticFunc.RectCutCorners(clientborder.X + 1, clientborder.Y+1, clientborder.Width - 2, clientborder.Height - 1, 1, 1);
                    using (Pen pc1 = new Pen(color1, 1.0F))
                        g.DrawPath(pc1, g1);

                    GraphicsPath g2 = ControlHelpersStaticFunc.RectCutCorners(clientborder.X, clientborder.Y, clientborder.Width, clientborder.Height - 1, 2, 2);
                    using (Pen pc2 = new Pen(color2, 1.0F))
                        g.DrawPath(pc2, g2);

                    g.Dispose();
                }
            }
        }
    }

}
