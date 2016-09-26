using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery
{
    public class DataGridViewSorter
    {
        public static void DataGridSort(DataGridView dgv, int column)
        {
            DataGridViewCustomSorter dgvSorter = null;
            if (dgv.Tag == null || !(dgv.Tag is IComparer))
            {
                dgvSorter = new DataGridViewCustomSorter(dgv);
                dgv.Tag = dgvSorter;
            }
            else
            {
                dgvSorter = (DataGridViewCustomSorter)dgv.Tag;
            }
            dgvSorter.SortColumn = column;
            dgv.Sort(dgvSorter);
        }

        private class DataGridViewCustomSorter : IComparer
        {
            private int ColumnIndex;
            private SortOrder OrderOfSort;
            private DataGridView myDataGridView;
            private TypeCode mySortTypeCode;

            public DataGridViewCustomSorter(DataGridView dgv)
            {
                myDataGridView = dgv;
                mySortTypeCode = Type.GetTypeCode(Type.GetType("System.String"));
                ColumnIndex = 0;
                OrderOfSort = SortOrder.None;
            }

            public int Compare(object x, object y)
            {
                int result;
                DataGridViewRow dgvX, dgvY;
                string sx, sy;

                dgvX = (DataGridViewRow)x;
                dgvY = (DataGridViewRow)y;

                if (dgvX.Cells[ColumnIndex].Value == null)
                    sx = "";
                else
                    sx = dgvX.Cells[ColumnIndex].Value.ToString();

                if (dgvY.Cells[ColumnIndex].Value == null)
                    sy = "";
                else
                    sy = dgvY.Cells[ColumnIndex].Value.ToString();

                //null handling
                if (sx == String.Empty && sy == String.Empty)
                    result = 0;
                else if (sx == String.Empty && sy != String.Empty)
                    result = -1;
                else if (sx != String.Empty && sy == String.Empty)
                    result = 1;
                else if (dgvY.Cells[ColumnIndex].Value.GetType()== typeof(DateTime))
                { 
                    DateTime dx = (DateTime)dgvX.Cells[ColumnIndex].Value;
                    DateTime dy = (DateTime)dgvY.Cells[ColumnIndex].Value;
                    result = dx.CompareTo(dy);

                }
                else
                {
                    switch (mySortTypeCode)
                    {
                        case TypeCode.Decimal:
                            Decimal nx = Convert.ToDecimal(sx);
                            Decimal ny = Convert.ToDecimal(sy);
                            result = nx.CompareTo(ny);
                            break;
                        case TypeCode.Double:
                            Double dbx = Convert.ToDouble(sx);
                            Double dby = Convert.ToDouble(sy);
                            result = dbx.CompareTo(dby);
                            break;
                        case TypeCode.DateTime:
                            DateTime dx = Convert.ToDateTime(sx);
                            DateTime dy = Convert.ToDateTime(sy);
                            result = dx.CompareTo(dy);
                            break;
                        case TypeCode.String:
                            result = (new CaseInsensitiveComparer()).Compare(sx, sy);
                            break;
                        default:
                            result = (new CaseInsensitiveComparer()).Compare(sx, sy);
                            break;
                    }
                }
                if (OrderOfSort == SortOrder.Descending)
                    result = (-result);

                return result;
            }

            public int SortColumn
            {
                set
                {
                    if (ColumnIndex == value)
                    {
                        OrderOfSort = (OrderOfSort == SortOrder.Descending ? SortOrder.Ascending : SortOrder.Descending);
                    }
                    ColumnIndex = value;
                    try
                    {
                        mySortTypeCode = TypeCode.String;

                        if (ColumnIndex == 0)           // 0 is date time on all views which use this.. bodge
                            mySortTypeCode = TypeCode.DateTime;
                    }
                    catch
                    {
                        mySortTypeCode = TypeCode.String;
                    }
                }
                get { return ColumnIndex; }
            }

            public SortOrder Order
            {
                set { OrderOfSort = value; }
                get { return OrderOfSort; }
            }
        }
    }



}
