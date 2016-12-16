using ExtendedControls;
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
    public partial class PanelVScroll : Panel               // Written because I could not get the manual autoscroll to work when controls dynamically added
    {
        public int ScrollBarWidth { get; set; } = 20;
        public bool VerticalScrollBarDockRight { get; set; } = true;        // true for dock right
        public Padding InternalMargin { get; set; }            // allows spacing around controls
        public VScrollBarCustom vsc;

        public PanelVScroll()
        {
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {  // as controls are added, remember them in local variables.
            if (e.Control is VScrollBarCustom)
            {
                vsc = e.Control as VScrollBarCustom;
                vsc.Width = ScrollBarWidth;
                vsc.Scroll += new System.Windows.Forms.ScrollEventHandler(OnScrollBarChanged);
                vsc.Name = "VScrollPanel";
            }

            e.Control.MouseWheel += Control_MouseWheel;         // grab the controls mouse wheel and direct to our scroll
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            //System.Diagnostics.Debug.WriteLine("Client rect {0}", base.ClientRectangle);
            Rectangle area = ClientRectangle;
            area.X += InternalMargin.Left;
            area.Y += InternalMargin.Top;
            area.Width -= InternalMargin.Left + InternalMargin.Right;
            area.Height -= InternalMargin.Top + InternalMargin.Bottom;

            if (vsc != null)
            {
                Point p = new Point(area.X + ((VerticalScrollBarDockRight) ? (area.Width - ScrollBarWidth) : 0), area.Y);
                vsc.Location = p;
                //System.Diagnostics.Debug.WriteLine("vsc {0}", vsc.Location);
                vsc.Size = new Size(ScrollBarWidth, area.Height);
            }

            ScrollTo(scrollpos);
        }

        private void Control_MouseWheel(object sender, MouseEventArgs e)
        {
            if (vsc != null)
            {
                if (e.Delta > 0)
                    vsc.ValueLimited -= vsc.LargeChange;
                else
                    vsc.ValueLimited += vsc.LargeChange;

                ScrollTo(vsc.Value);
            }
        }

        protected virtual void OnScrollBarChanged(object sender, ScrollEventArgs e)
        {
            ScrollTo(e.NewValue);
        }

        int scrollpos = 0;

        public int ScrollTo(int newscrollpos )
        {
            int maxy = 0;
            foreach (Control c in Controls)
            {
                if (!(c is VScrollBarCustom))
                {
                    int ynoscroll = c.Location.Y + scrollpos;
                    maxy = Math.Max(maxy, ynoscroll + c.Height + 4);
                }
            }

            if (maxy < ClientRectangle.Height)          // see if need scroll..
                newscrollpos = 0;

            if (newscrollpos != scrollpos)
            {
                SuspendLayout();
                foreach (Control c in Controls)
                {
                    if (!(c is VScrollBarCustom))
                    {
                       // System.Diagnostics.Debug.WriteLine("Move {0}", c.Name);

                        int ynoscroll = c.Location.Y + scrollpos;
                        c.Location = new Point(c.Location.X, ynoscroll - newscrollpos);       // SPENT AGES with the bloody AutoScrollPosition.. could not get it to work..
                    }
                }

                ResumeLayout();
                PerformLayout();
            }

            if (vsc != null)
            {
                vsc.Maximum = maxy - ClientRectangle.Height + vsc.LargeChange;
                vsc.Minimum = 0;
                //System.Diagnostics.Debug.WriteLine("Scroll {0} to {1} maxy {0} sb {1}", scrollpos, newscrollpos, maxy, vsc.Maximum);
            }

            scrollpos = newscrollpos;

            return maxy;
        }

        public void RemoveAllControls( List<Control> excluded = null)
        {
            List<Control> listtoremove = (from Control s in Controls where (!(s is VScrollBarCustom) && (excluded==null || !excluded.Contains(s))) select s).ToList();
            foreach (Control c in listtoremove)
            {
                Debug.Assert(!(c is VScrollBarCustom));

                c.Hide();
                c.Dispose();
                Controls.Remove(c);
            }

            scrollpos = 0;
        }
    }
}
