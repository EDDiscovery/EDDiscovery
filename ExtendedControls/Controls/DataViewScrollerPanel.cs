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
                dgv.RowStateChanged += DGVRowStateChanged;
                dgv.RowHeightChanged += Dgv_RowHeightChanged;
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
                vsc.SetValueMaximumLargeChange(dgv.FirstDisplayedScrollingRowIndex, dgv.Rows.GetRowCount(DataGridViewElementStates.Visible) - 1, dgv.DisplayedRowCount(false));
            }
        }

        protected void DGVRowsAdded(Object sender, DataGridViewRowsAddedEventArgs e)
        {
            int rows = dgv.Rows.GetRowCount(DataGridViewElementStates.Visible);
            Debug.Assert(vsc != null, "No Scroll bar attached");
            //Console.WriteLine("Rows Added: first:" + dgv.FirstDisplayedScrollingRowIndex + " disp:" + dgv.DisplayedRowCount(false) + " rows" + rows + " Added " + e.RowCount);
            vsc.SetValueMaximumLargeChange(dgv.FirstDisplayedScrollingRowIndex, rows - 1, dgv.DisplayedRowCount(false));
        }

        protected void DGVRowsRemoved(Object sender, DataGridViewRowsRemovedEventArgs e)
        {
            int rows = dgv.Rows.GetRowCount(DataGridViewElementStates.Visible);
            Debug.Assert(vsc != null, "No Scroll bar attached");
            //Console.WriteLine("Rows Removed: first:" + dgv.FirstDisplayedScrollingRowIndex + " disp:" + dgv.DisplayedRowCount(false) + " rows" + rows +  " Removed " + e.RowCount);
            vsc.SetValueMaximumLargeChange(dgv.FirstDisplayedScrollingRowIndex, rows - 1, dgv.DisplayedRowCount(false));
        }

        protected virtual void DGVRowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged.Equals(DataGridViewElementStates.Visible))
            {
                int rows = dgv.Rows.GetRowCount(DataGridViewElementStates.Visible);
                Debug.Assert(vsc != null, "No Scroll bar attached");
                vsc.SetValueMaximumLargeChange(dgv.FirstDisplayedScrollingColumnIndex, rows - 1, dgv.DisplayedRowCount(false));
            }
        }

        private void Dgv_RowHeightChanged(object sender, DataGridViewRowEventArgs e)
        {
            // TBD Whats up? Try journal..
        //    int rows = dgv.Rows.GetRowCount(DataGridViewElementStates.Visible);
        //    Debug.Assert(vsc != null, "No Scroll bar attached");
        //    vsc.SetValueMaximumLargeChange(dgv.FirstDisplayedScrollingColumnIndex, rows - 1, dgv.DisplayedRowCount(false));
        }


        protected virtual void DGVMWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                vsc.ValueLimited--;                 // control takes care of end limits..
            else
                vsc.ValueLimited++;                 // end is UserLimit, not maximum

            SetFirstDisplayed(vsc.Value);
        }

        bool ignoredgvscroll = false;
        protected void DGVScroll(Object sender, ScrollEventArgs e)
        {
            if (ignoredgvscroll == false)
            {
                int rows = dgv.Rows.GetRowCount(DataGridViewElementStates.Visible);
                Debug.Assert(vsc != null, "No Scroll bar attached");
                //Console.WriteLine("DGV Scroll: first:" + dgv.FirstDisplayedScrollingRowIndex + " disp:" + dgv.DisplayedRowCount(false) + " rows" + rows);
                vsc.SetValueMaximumLargeChange(dgv.FirstDisplayedScrollingRowIndex, rows - 1, dgv.DisplayedRowCount(false));
            }
        }

        protected virtual void OnScrollBarChanged(object sender, ScrollEventArgs e)
        {
            Debug.Assert(dgv != null, "No Data view attached");
            int rows = dgv.Rows.GetRowCount(DataGridViewElementStates.Visible);
            //Console.WriteLine("VSC Scroll: first:" + dgv.FirstDisplayedScrollingRowIndex + " disp:" + dgv.DisplayedRowCount(false) + " rows" + rows);
            SetFirstDisplayed(vsc.Value);
        }

        private void SetFirstDisplayed(int count)                   // find first entry which is visible at this count
        {
            //Console.WriteLine("Want " + count );
            for (int rowi = 0; rowi < dgv.RowCount; rowi++)
            {
                if (dgv.Rows[rowi].Visible == true && count-- == 0)
                {
                    //Console.WriteLine("Picked " + rowi + " Scroll bar " + vsc.Value);
                    ignoredgvscroll = true;
                    dgv.FirstDisplayedScrollingRowIndex = rowi;     // don't fire the DGVScroll.. as we can get into a cycle if rows are hidden
                    ignoredgvscroll = false;
                    return;
                }
            }
        }

        public void UpdateScroll()      // call if hide/unhide cells - no call back for this
        {
            int rows = dgv.Rows.GetRowCount(DataGridViewElementStates.Visible);
            vsc.SetValueMaximumLargeChange(dgv.FirstDisplayedScrollingRowIndex, rows - 1, dgv.DisplayedRowCount(false));
        }


        #endregion

        #region Variables

        DataGridView dgv;
        VScrollBarCustom vsc;

        #endregion
    }
}
