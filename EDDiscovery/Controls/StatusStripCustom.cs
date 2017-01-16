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
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class StatusStripCustom : StatusStrip
    {
        public const int WM_NCHITTEST = 0x84;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int WM_NCLBUTTONUP = 0xA2;
        public const int HT_CLIENT = 0x1;
        public const int HT_BOTTOMRIGHT = 0x11;
        public const int HT_TRANSPARENT = -1;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST)
            {
                if ((int)m.Result == HT_BOTTOMRIGHT)
                {
                    // Tell the system to test the parent
                    m.Result = (IntPtr)HT_TRANSPARENT;
                }
                else if ((int)m.Result == HT_CLIENT)
                {
                    // Work around the implementation returning HT_CLIENT instead of HT_BOTTOMRIGHT
                    int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                    int y = unchecked((short)((uint)m.LParam >> 16));
                    Point p = PointToClient(new Point(x, y));

                    if (p.X >= this.ClientSize.Width - this.ClientSize.Height || p.Y >= this.ClientSize.Height - 5) // corner, or bottom strip
                    {
                        // Tell the system to test the parent
                        m.Result = (IntPtr)HT_TRANSPARENT;
                    }
                }
            }
        }
    }
}
