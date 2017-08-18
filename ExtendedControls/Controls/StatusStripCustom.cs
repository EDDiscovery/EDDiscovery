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

namespace ExtendedControls
{
    public class StatusStripCustom : StatusStrip
    {
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM.NCHITTEST)
            {
                if (m.Result == (IntPtr)HT.BOTTOMRIGHT)
                {
                    // Tell the system to test the parent
                    m.Result = (IntPtr)HT.TRANSPARENT;
                }
                else if (m.Result == (IntPtr)HT.CLIENT)
                {
                    // Work around the implementation returning HT_CLIENT instead of HT_BOTTOMRIGHT
                    Point p = PointToClient(new Point((int)m.LParam));

                    if (p.X >= this.ClientSize.Width - this.ClientSize.Height || p.Y >= this.ClientSize.Height - 5) // corner, or bottom strip
                    {
                        // Tell the system to test the parent
                        m.Result = (IntPtr)HT.TRANSPARENT;
                    }
                }
            }
        }
    }
}
