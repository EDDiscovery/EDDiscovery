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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExtendedControls
{
    // WARNING ONLY USE IN SPECIAL CIRCUMSTANCES.. NORMALLY YOU WILL WANT THE NORMAL LABEL
    // draws label using a bitmap - solves problems with aliasing over transparent backgrounds
    // but it does not antialias properly if the background is not drawn..

    public class LabelExt : Label               
    {
        public Color TextBackColor { get; set; } = Color.Transparent;       // area of text box only

        public LabelExt()
        {
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            SizeF sizef = pe.Graphics.MeasureString(this.Text, this.Font);
            Size sz = new Size((int)(sizef.Width + 1), (int)(sizef.Height + 1));

            //Console.WriteLine("Label size {0}", sz);

            if (sz.Width > 0 && sz.Height > 0 && this.Text.Length > 0)
            {
                using (Bitmap mp = new Bitmap(sz.Width, sz.Height))   // bitmaps .. drawing directly does not work due to aliasing
                {
                    using (Graphics mpg = Graphics.FromImage(mp))
                    {
                        if (!this.TextBackColor.IsFullyTransparent())
                        {
                            using (Brush b = new SolidBrush(this.TextBackColor))
                            {
                                mpg.FillRectangle(b, new Rectangle(0, 0, sz.Width, sz.Height));
                            }
                        }

                        mpg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                        using (Brush b = new SolidBrush(this.ForeColor))
                        {
                            mpg.DrawString(this.Text, this.Font, b, new Point(0, 0));
                        }

                        mpg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

                        Rectangle pos = TextAlign.ImagePositionFromContentAlignment(ClientRectangle, mp.Size, true);
                        pe.Graphics.DrawImageUnscaled(mp, pos.Left, pos.Top);
                    }
                }
            }
        }
    }
}
