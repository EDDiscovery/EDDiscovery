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

            if (m.Msg == WM_NCHITTEST && (int)m.Result == HT_BOTTOMRIGHT)
            {
                // Tell the system to test the parent
                m.Result = (IntPtr)HT_TRANSPARENT;
            }
        }
    }
}
