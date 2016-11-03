using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EDDiscovery.DB;

namespace EDDiscovery.UserControls
{
    public class UserControlCommonBase : UserControl
    {
        public virtual void LoadLayout() { }
        public virtual void SaveLayout() { }

        #region DGV Column helpers - used to save/size the DGV of user controls dynamically.

        private bool ignorewidthchange = false;

        // Derived class defines this to tell us which order you want the columns to shrink/expand by.  -1 at end of list.
        public virtual int ColumnWidthPreference(DataGridView dgv, int i) { return (i < 1) ? 0 : -1; }
        public virtual int ColumnExpandPreference() { return 0; }

        public void DGVLoadColumnLayout(DataGridView dgv, string root)
        {
            ignorewidthchange = true;

            if (SQLiteConnectionUser.keyExists(root + "1"))        // if stored values, set back to what they were..
            {
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    int w = SQLiteDBClass.GetSettingInt(root + ((i + 1).ToString()), -1);
                    if (w >= 10)        // in case something is up (min 10 pixels)
                        dgv.Columns[i].Width = w;
                }
            }

            FillDGVOut(dgv);
        }

        public void DGVSaveColumnLayout(DataGridView dgv, string root)
        {
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                SQLiteDBClass.PutSettingInt(root + ((i + 1).ToString()), dgv.Columns[i].Width);
            }
        }

        public void DGVColumnWidthChanged(DataGridView dgv)
        {
            if (!ignorewidthchange)
                FillDGVOut(dgv);       // scale out so its filled..
        }

        public void DGVResize(DataGridView dgv)
        {
            FillDGVOut(dgv);
        }

        private void FillDGVOut(DataGridView dgv)
        {
            ignorewidthchange = true;

            int twidth = dgv.RowHeadersVisible ? dgv.RowHeadersWidth : 0;        // get how many pixels we are using..

            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                twidth += dgv.Columns[i].Width;

                if (dgv.Name.Contains("Ledger")) System.Diagnostics.Debug.WriteLine(dgv.Name + ":" + i + "," + dgv.Columns[i].Width);
            }

            int delta = dgv.Width - twidth;

            if (dgv.Name.Contains("Ledger")) System.Diagnostics.Debug.WriteLine(dgv.Name + ":" + dgv.Width + " " + delta + " "  + twidth);

            if (delta < 0)        // not enough space
            {
                int i=0,c=0;
                while( (c = ColumnWidthPreference(dgv,i++)) >= 0 )
                {
                    Collapse(dgv, ref delta, c);
                }
            }
            else
                dgv.Columns[ColumnExpandPreference()].Width += delta;   // note is used to fill out columns

            ignorewidthchange = false;
        }

        private void Collapse(DataGridView dgv, ref int delta, int col)
        {
            if (delta < 0)
            {
                int colsaving = dgv.Columns[col].Width - dgv.Columns[col].MinimumWidth;

                if (-delta <= colsaving)       // if can save 30 from col3, and delta is -20, 20<=30, do it.
                {
                    dgv.Columns[col].Width += delta;
                    if (dgv.Name.Contains("Ledger")) System.Diagnostics.Debug.WriteLine(dgv.Name + ":Collapse " + col + " reclaim " + delta + " new width saved all " + dgv.Columns[col].Width);
                    delta = 0;
                }
                else
                {
                    delta += colsaving;
                    dgv.Columns[col].Width = dgv.Columns[col].MinimumWidth;
                    if (dgv.Name.Contains("Ledger")) System.Diagnostics.Debug.WriteLine(dgv.Name + ":Collapse " + col + "," + colsaving + " new width part save " + dgv.Columns[col].Width);
                }
            }
        }


        #endregion

    }
}
