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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ExtendedControls
{
    public class CompositeButton : Control, IDisposable
    {
        public ButtonExt[] Buttons { get; set; }
        public int ButtonSpacing {get;set;} = 8;
        public Panel[] Decals { get; set; }
        public int DecalSpacing { get; set; } = 8;
        public int MinimumDecalButtonVerticalSpacing { get; set; } = 8;
        public override Image BackgroundImage { get { return backgroundpanel.BackgroundImage; } set { backgroundpanel.BackgroundImage = value; } }
        public override ImageLayout BackgroundImageLayout { get { return backgroundpanel.BackgroundImageLayout; } set { backgroundpanel.BackgroundImageLayout = value; } }

        public override string Text { get { return textlab.Text; } set { textlab.Text = value; } }
        public Color TextBackground { get { return textlab.BackColor; } set { textlab.BackColor = value; Invalidate(); } }
        public Color TextBackColor { get { return textlab.TextBackColor; } set { textlab.TextBackColor = value; Invalidate(); } }
        public Font TextFont { get { return textlab.Font; } set { textlab.Font = value; Invalidate(); } }

        private LabelExt textlab;
        private Panel backgroundpanel { get; set; }

        public CompositeButton()
        {
            backgroundpanel = new Panel();
            textlab = new LabelExt();
            textlab.AutoSize = false;
            textlab.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            textlab.BackColor = Color.Transparent;
            backgroundpanel.Controls.Add(textlab);
            Controls.Add(backgroundpanel);
        }

        public new void Dispose()
        {
            backgroundpanel.Controls.Clear();
            backgroundpanel.Dispose();
            textlab.Dispose();
        }

        public void QuickInit(Image backimage, string text, Font textfont, Color textfore, Color backgroundcol, Image decal, Size decalsize, Image[] buttons, Size buttonsize, Action<object, int> ButtonPressed)
        {
            BackgroundImage = backimage;
            BackgroundImageLayout = ImageLayout.Stretch;
            Text = text;
            TextBackColor = backgroundcol;
            TextFont = textfont;
            textlab.ForeColor = textfore;

            Decals = new Panel[1];
            Decals[0] = new Panel();
            Decals[0].Size = decalsize;
            Decals[0].BackgroundImageLayout = ImageLayout.Stretch;
            Decals[0].BackgroundImage = decal;
            Decals[0].BackColor = backgroundcol;

            Buttons = new ButtonExt[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                Buttons[i] = new ButtonExt();
                Buttons[i].Size = buttonsize;
                Buttons[i].Image = buttons[i];
                Buttons[i].ImageLayout = ImageLayout.Stretch;
                Buttons[i].Tag = i;
                Buttons[i].BackColor = backgroundcol;
                Buttons[i].Click += (o, e) => { ButtonExt b = o as ButtonExt; ButtonPressed?.Invoke(this.Tag, (int)b.Tag); };
            }

            EnsureControlsAdded();
        }

        protected override void OnResize(EventArgs e)
        {
            LayoutItems();
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
        }

        bool performedlayout = false;
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!performedlayout)
            {
                LayoutItems();
                performedlayout = true;
            }

            base.OnPaint(e);
        }

        private void EnsureControlsAdded()
        {
            if (Buttons != null)
            {
                foreach (ButtonExt b in Buttons)
                {
                    if (!backgroundpanel.Controls.Contains(b))
                        backgroundpanel.Controls.Add(b);
                }
            }

            if (Decals != null)
            {
                foreach (Panel bm in Decals)
                {
                    if (!backgroundpanel.Controls.Contains(bm))
                        backgroundpanel.Controls.Add(bm);
                }
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
        }

        void LayoutItems()
        {
            EnsureControlsAdded();

            int vcentre = ClientRectangle.Height/2;
            int hcentre = ClientRectangle.Width / 2;
            int husabled = ClientRectangle.Width - Padding.Horizontal;

            backgroundpanel.Size = ClientRectangle.Size;
            textlab.Location = new Point(Padding.Left,Padding.Top);
            textlab.Size = new Size(husabled,20);

            int buttonvtop = ClientRectangle.Height;

            if (Buttons != null)
            {
                int totalwidth = (from x in Buttons select x.Width).Sum() + (Buttons.Count() - 1) * ButtonSpacing;
                int maxvertheight = (from x in Buttons select x.Height).Max();

                int buttonvcentre = ClientRectangle.Height - Padding.Bottom - maxvertheight / 2;
                buttonvtop = buttonvcentre - maxvertheight / 2;

                //System.Diagnostics.Debug.WriteLine("Button vline at " + buttonvcentre);
                int sp = hcentre - totalwidth / 2;
                foreach (ButtonExt b in Buttons)
                {
                    b.Location = new Point(sp, buttonvcentre - b.Height/2);
                    //System.Diagnostics.Debug.WriteLine("Button {0} {1}", b.Name, b.Location);
                    sp += b.Width + ButtonSpacing;
                }
            }

            if (Decals != null)
            {
                int totalwidth = (from x in Decals select x.Width).Sum() + (Decals.Count() - 1) * DecalSpacing;
                int maxvertheight = (from x in Decals select x.Height).Max();

                int decalvcentre = vcentre;
                if (decalvcentre + maxvertheight / 2 >= buttonvtop)
                    decalvcentre = buttonvtop - maxvertheight / 2 - MinimumDecalButtonVerticalSpacing;

                //System.Diagnostics.Debug.WriteLine("Decal vline at " + decalvcentre);
                int sp = hcentre - totalwidth / 2;

                foreach (Panel bm in Decals)
                {
                    bm.Location = new Point(sp, decalvcentre - bm.Height/2);
                    sp += bm.Width + DecalSpacing;
                }
            }

            Invalidate();
        }
    }
}
