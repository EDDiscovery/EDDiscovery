using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class DataViewScrollerPanel : Panel      // Must have a DGV and a VScroll added as children by the framework
    {
        public int ScrollBarWidth { get; set; } = 20;
        public bool VerticalScrollBarDockRight { get; set; } = true;        // true for dock right
        public Padding InternalMargin { get; set; }            // allows spacing around controls

        #region Implementation
        public DataViewScrollerPanel() : base()
        {
        }

        protected override void OnControlAdded(ControlEventArgs e ) 
        {  // as controls are added, remember them in local variables.
            if (e.Control is DataGridView)
            {
                dgv = e.Control as DataGridView;
                dgv.Scroll += DGVScroll;
                dgv.RowsAdded += DGVRowsAdded;
                dgv.RowsRemoved += DGVRowsRemoved;
                dgv.MouseWheel += DGVMWheel;
            }
            else if (e.Control is VScrollBarCustom)
            {
                vsc = e.Control as VScrollBarCustom;
                vsc.Scroll += new System.Windows.Forms.ScrollEventHandler(OnScrollBarChanged);
            }
            else
                Debug.Assert(true, "Data view Scroller Panel requires DataGridView and VScrollBarCustom to be added");
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            int dgvcolumnheaderheight = 0;

            Rectangle area = ClientRectangle;
            area.X += InternalMargin.Left;
            area.Y += InternalMargin.Top;
            area.Width -= InternalMargin.Left + InternalMargin.Right;
            area.Height -= InternalMargin.Top + InternalMargin.Bottom;

            if (dgv != null)                       // during designing, you may get in a situation where this code runs but these are not attached.  Fail gracefully. otherwise VS crashes
            {
                dgv.Location = new Point(area.X + ((VerticalScrollBarDockRight) ? 0 : ScrollBarWidth), area.Y);
                dgv.Size = new Size(area.Width - ScrollBarWidth, area.Height);
                dgvcolumnheaderheight = dgv.ColumnHeadersHeight;
            }

            if (vsc != null)
            {
                vsc.Location = new Point(area.X + ((VerticalScrollBarDockRight) ? (area.Width - ScrollBarWidth) : 0), area.Y+dgvcolumnheaderheight);
                vsc.Size = new Size(ScrollBarWidth, area.Height - dgvcolumnheaderheight) ;
            }

            if ( dgv != null && vsc != null )
            {
                vsc.SetValueMaximumLargeChange(dgv.FirstDisplayedScrollingRowIndex, dgv.RowCount - 1, dgv.DisplayedRowCount(false));
            }
        }

        protected void DGVRowsAdded(Object sender, DataGridViewRowsAddedEventArgs e)
        {
            Debug.Assert(vsc != null, "No Scroll bar attached");
           // Console.WriteLine("Rows Added: first:" + dgv.FirstDisplayedScrollingRowIndex + " disp:" + dgv.DisplayedRowCount(false) + " rows" + dgv.RowCount + " Added " + e.RowCount);
            vsc.SetValueMaximumLargeChange(dgv.FirstDisplayedScrollingRowIndex, dgv.RowCount - 1, dgv.DisplayedRowCount(false));
        }

        protected void DGVRowsRemoved(Object sender, DataGridViewRowsRemovedEventArgs e)
        {
            Debug.Assert(vsc != null, "No Scroll bar attached");
          //  Console.WriteLine("Rows Removed: first:" + dgv.FirstDisplayedScrollingRowIndex + " disp:" + dgv.DisplayedRowCount(false) + " rows" + dgv.RowCount +  " Removed " + e.RowCount);
            vsc.SetValueMaximumLargeChange(dgv.FirstDisplayedScrollingRowIndex, dgv.RowCount - 1, dgv.DisplayedRowCount(false));
        }

        protected virtual void DGVMWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                vsc.ValueLimited--;                 // control takes care of end limits..
            else
                vsc.ValueLimited++;                 // end is UserLimit, not maximum

            if ( vsc.Value >= 0 )                   // we may have a situation where the scroll bar is off and value is -1.. only set if to a good value.
                dgv.FirstDisplayedScrollingRowIndex = vsc.Value;
        }

        protected void DGVScroll(Object sender, ScrollEventArgs e)
        {
            Debug.Assert(vsc != null, "No Scroll bar attached");
//            Console.WriteLine("DGV Scroll: first:" + dgv.FirstDisplayedScrollingRowIndex + " disp:" + dgv.DisplayedRowCount(false) + " rows" + dgv.RowCount);
            vsc.SetValueMaximumLargeChange(dgv.FirstDisplayedScrollingRowIndex, dgv.RowCount - 1, dgv.DisplayedRowCount(false));
        }

        protected virtual void OnScrollBarChanged(object sender, ScrollEventArgs e)
        {
            Debug.Assert(dgv != null, "No Data view attached");
            //            Console.WriteLine("VSC Scroll: first:" + dgv.FirstDisplayedScrollingRowIndex + " disp:" + dgv.DisplayedRowCount(false) + " rows" + dgv.RowCount);
            dgv.FirstDisplayedScrollingRowIndex = vsc.Value;
        }


        #endregion

        #region Variables

        DataGridView dgv;
        VScrollBarCustom vsc;

        #endregion
    }
}
