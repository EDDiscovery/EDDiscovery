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
    public class LabelExt : Label               // draws label using a bitmap - solves problems with aliasing over transparent backgrounds
    {
        #region Public interfaces

        #region ctors

        public LabelExt() { }

        #endregion // ctors

        #region Properties

        public bool CentreX { get; set; } = false;
        public bool CentreY { get; set; } = false;
        public Color TextBackColor { get; set; } = Color.Transparent;

        #endregion // Properties

        #endregion // Public interfaces


        #region Implementation

        protected override void OnPaint(PaintEventArgs pe)
        {
            SizeF sizef = pe.Graphics.MeasureString(Text, Font);
            Size sz = new Size((int)(sizef.Width + 1), (int)(sizef.Height + 1));

            //Console.WriteLine("Label size {0}", sz);

            if (sz.Width > 0 && sz.Height > 0 && Text.Length > 0)
            {
                using (Bitmap mp = new Bitmap(sz.Width, sz.Height))   // bitmaps .. drawing directly does not work due to aliasing
                using (Graphics mpg = Graphics.FromImage(mp))
                {
                    if (!TextBackColor.IsFullyTransparent())
                    {
                        using (Brush b = new SolidBrush(TextBackColor))
                        {
                            mpg.FillRectangle(b, new Rectangle(0, 0, sz.Width, sz.Height));
                        }
                    }

                    mpg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    using (Brush b = new SolidBrush(ForeColor))
                    {
                        mpg.DrawString(Text, Font, b, new Point(0, 0));
                    }

                    mpg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

                    if (CentreY || CentreX)
                    {
                        int x = (CentreX) ? (ClientRectangle.Width / 2 - sz.Width / 2) : 0;
                        int y = (CentreY) ? (ClientRectangle.Height / 2 - sz.Height / 2) : 0;
                        if (x < 0)
                            x = 0;
                        if (y < 0)
                            y = 0;
                        pe.Graphics.DrawImageUnscaled(mp, x, y);
                    }
                    else
                        pe.Graphics.DrawImageUnscaled(mp, 0, 0);
                }
            }
        }

        #endregion // Implementation
    }
}
