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
    public class TextBoxBorder : Control
    {
        // BorderColour != transparent to use ours
        // BorderStyle to set textbox style..  None for off.  Can use both if you wish 

        public Color BorderColor { get { return bordercolor; } set { bordercolor = value; Reposition(); Invalidate(true); } }
        public float BorderColorScaling { get; set; } = 0.5F;
        public System.Windows.Forms.BorderStyle BorderStyle { get { return textbox.BorderStyle; } set { textbox.BorderStyle = value; Reposition(); Invalidate(true); } }

        public override Color ForeColor { get { return textbox.ForeColor; } set { textbox.ForeColor = value; Invalidate(true); } }
        public override Color BackColor { get { return textbox.BackColor; } set { textbox.BackColor = value; Invalidate(true); } }
        public Color ControlBackground { get { return controlbackcolor; } set { controlbackcolor = value; Invalidate(true); } } // colour of unfilled control area if border is on or button

        public bool WordWrap { get { return textbox.WordWrap; } set { textbox.WordWrap = value; } }
        public bool Multiline { get { return textbox.Multiline; } set { textbox.Multiline = value; Reposition(); } }
        public bool ReadOnly { get { return textbox.ReadOnly; } set { textbox.ReadOnly = value; } }
        public System.Windows.Forms.ScrollBars ScrollBars { get { return textbox.ScrollBars; } set { textbox.ScrollBars = value; } }

        public override string Text { get { return textbox.Text; } set { textbox.Text = value; } }

        public void SelectAll() { textbox.SelectAll(); }
        public int SelectionStart { get { return textbox.SelectionStart; } set { textbox.SelectionStart = value; } }
        public int SelectionLength { get { return textbox.SelectionLength; } set { textbox.SelectionLength = value; } }
        public void Select(int s, int e) { textbox.Select(s, e); }

        public HorizontalAlignment TextAlign { get { return textbox.TextAlign; } set { textbox.TextAlign = value; } }
        //        public new virtual bool Visible { get { return base.Visible; } set { base.Visible = value; if ( Parent != null) Parent.Invalidate(); } }

        public System.Windows.Forms.AutoCompleteMode AutoCompleteMode { get { return textbox.AutoCompleteMode; } set { textbox.AutoCompleteMode = value; } }
        public System.Windows.Forms.AutoCompleteSource AutoCompleteSource { get { return textbox.AutoCompleteSource; } set { textbox.AutoCompleteSource = value; } }

        private TextBox textbox;
        private Color bordercolor = Color.Transparent;
        private Color controlbackcolor = SystemColors.Control;

        public TextBoxBorder() : base()
        {
            textbox = new TextBox();
            textbox.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(textbox);
        }

        const int borderoffset = 3;

        private bool OurBorder { get { return !BorderColor.IsFullyTransparent(); } }

        private void Reposition()
        {
            if (ClientRectangle.Width > 0)
            {
                int offset = OurBorder ? borderoffset : 0;
                textbox.Location = new Point(offset, offset);
                textbox.Size = new Size(ClientRectangle.Width - offset * 2, ClientRectangle.Height - offset * 2);
                //System.Diagnostics.Debug.WriteLine("Repos " + Name + ":" + ClientRectangle.Size + " " + textbox.Location + " " + textbox.Size + " " + BorderColor + " " + textbox.BorderStyle);
            }
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            Reposition();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Brush highlight = new SolidBrush(controlbackcolor))
            {
                e.Graphics.FillRectangle(highlight, ClientRectangle);
            }

            base.OnPaint(e);

            //System.Diagnostics.Debug.WriteLine("Repaint" + Name + ":" + ClientRectangle.Size + " " + textbox.Location + " " + textbox.Size + " " + BorderColor + " " + textbox.BorderStyle);

            if (OurBorder)
            {
                Rectangle clientborder = new Rectangle(0, 0, ClientRectangle.Width, textbox.Height + borderoffset*2);       // paint it around the actual area of the textbox, not just bit

                Color color1 = BorderColor;
                Color color2 = BorderColor.Multiply(BorderColorScaling);

                GraphicsPath g1 = ControlHelpersStaticFunc.RectCutCorners(clientborder.X + 1, clientborder.Y + 1, clientborder.Width - 2, clientborder.Height - 1, 1, 1);
                using (Pen pc1 = new Pen(color1, 1.0F))
                    e.Graphics.DrawPath(pc1, g1);

                GraphicsPath g2 = ControlHelpersStaticFunc.RectCutCorners(clientborder.X, clientborder.Y, clientborder.Width, clientborder.Height - 1, 2, 2);
                using (Pen pc2 = new Pen(color2, 1.0F))
                    e.Graphics.DrawPath(pc2, g2);
            }
        }
    }
}
