using EDDiscovery.Forms;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlBookmarks : UserControlCommonBase
    {
        DataGridViewRow currentedit = null;

        Timer searchtimer;

        #region init
        public UserControlBookmarks()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            searchtimer = new Timer() { Interval = 500 };
            searchtimer.Tick += Searchtimer_Tick;
            GlobalBookMarkList.Instance.OnBookmarkChange += BookmarksChanged;
        }

        public override void Closing()
        {
            SaveBackAnyChanges();

            searchtimer.Dispose();

            GlobalBookMarkList.Instance.OnBookmarkChange -= BookmarksChanged;
        }
        #endregion

        #region display
        public override void InitialDisplay()
        {
            Display();
        }
        
        private void Display()
        {
            this.dataGridViewBookMarks.SelectionChanged -= new System.EventHandler(this.dataGridViewBookMarks_SelectionChanged);

            int lastrow = dataGridViewBookMarks.CurrentCell != null ? dataGridViewBookMarks.CurrentCell.RowIndex : -1;

            DataGridViewColumn sortcol = dataGridViewBookMarks.SortedColumn != null ? dataGridViewBookMarks.SortedColumn : dataGridViewBookMarks.Columns[0];
            SortOrder sortorder = dataGridViewBookMarks.SortOrder;

            dataGridViewBookMarks.SuspendLayout();

            dataGridViewBookMarks.Rows.Clear();
            
            foreach (BookmarkClass bk in GlobalBookMarkList.Instance.Bookmarks)
            {
                //System.Diagnostics.Debug.WriteLine("Bookmark " + bk.Name  +":" + bk.Note);
                var rw = dataGridViewBookMarks.RowTemplate.Clone() as DataGridViewRow;
                rw.CreateCells( dataGridViewBookMarks , bk.isRegion ? "Region" : "System" ,
                    bk.isRegion ? bk.Heading : bk.StarName,
                    bk.Note,
                    bk.x.ToString("0.##"),
                    bk.y.ToString("0.##"),
                    bk.z.ToString("0.##") );
                rw.Tag = bk;

                dataGridViewBookMarks.Rows.Add(rw);
            }

            dataGridViewBookMarks.ResumeLayout();

            dataGridViewBookMarks.Sort(sortcol, (sortorder == SortOrder.Descending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
            dataGridViewBookMarks.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;

            if (lastrow != -1)
                dataGridViewBookMarks.CurrentCell = dataGridViewBookMarks.Rows[Math.Min(lastrow, dataGridViewBookMarks.Rows.Count - 1)].Cells[2];

            RefreshCurrentEdit();

            this.dataGridViewBookMarks.SelectionChanged += new System.EventHandler(this.dataGridViewBookMarks_SelectionChanged);
        }

        private void RefreshCurrentEdit()
        {
            if (dataGridViewBookMarks.CurrentCell != null)
            {
                currentedit = dataGridViewBookMarks.Rows[dataGridViewBookMarks.CurrentCell.RowIndex];
                BookmarkClass bk = (BookmarkClass)(currentedit.Tag);
                //System.Diagnostics.Debug.WriteLine("Move to row " + currentedit.Index + " Notes " + bk.Name);
                userControlSurfaceBookmarks.DisplayPlanetMarks(bk);
            }
            else
            {
                currentedit = null;
                userControlSurfaceBookmarks.DisplayPlanetMarks(null);
            }
        }

        private void SaveBackAnyChanges()
        {
            if (currentedit != null )
            {
                BookmarkClass bk = (BookmarkClass)currentedit.Tag;
                string newNote = currentedit.Cells[2].Value.ToString();
                //System.Diagnostics.Debug.WriteLine("Checking for save " + currentedit.Index);

                if (!newNote.Equals(bk.Note) || userControlSurfaceBookmarks.Edited)     // notes or planet marks changed
                {
                    updating = true;
                    //System.Diagnostics.Debug.WriteLine("Save back " + bk.Name + " " + newNote);
                    PlanetMarks latestMarks = userControlSurfaceBookmarks.PlanetMarks;
                    currentedit.Tag = GlobalBookMarkList.Instance.AddOrUpdateBookmark(bk, !bk.isRegion, bk.isRegion ? bk.Heading : bk.StarName, bk.x, bk.y, bk.z, bk.Time, newNote, latestMarks);
                    updating = false;
                }

                currentedit = null;
            }
        }


        private void dataGridViewBookMarks_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index >= 3)
            {
                double v1;
                double v2;
                bool v1hasval = Double.TryParse(e.CellValue1.ToString(), out v1);
                bool v2hasval = Double.TryParse(e.CellValue2.ToString(), out v2);

                if (v1hasval)
                {
                    if (v2hasval)
                        e.SortResult = v1.CompareTo(v2);
                    else
                        e.SortResult = 1;
                }
                else if (v2hasval)
                    e.SortResult = -1;
                else
                    return;

                e.Handled = true;
            }
        }

        #endregion

        #region UI

        private void buttonNew_Click(object sender, EventArgs e)
        {
            BookmarkForm frm = new BookmarkForm();
            DateTime tme = DateTime.Now;
            frm.NewSystemBookmark(tme.ToString());
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                updating = true;
                GlobalBookMarkList.Instance.AddOrUpdateBookmark(null, true, frm.StarHeading, double.Parse(frm.x), double.Parse(frm.y), double.Parse(frm.z),
                                                                     tme, frm.Notes, frm.SurfaceLocations);
                updating = false;
                Display();
            }
        }

        private void dataGridViewBookMarks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            buttonEdit_Click(sender, e);
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (currentedit != null)      // if we have a current cell.. 
            {
                BookmarkClass bk = (BookmarkClass)currentedit.Tag;

                SaveBackAnyChanges();

                BookmarkForm frm = new BookmarkForm();
                frm.Update(bk);
                DialogResult dr = frm.ShowDialog(this);

                updating = true;

                if (dr == DialogResult.OK)
                {
                    //System.Diagnostics.Debug.WriteLine("Updating bookmark " + bk.Name);
                    GlobalBookMarkList.Instance.AddOrUpdateBookmark(bk, !bk.isRegion, frm.StarHeading, double.Parse(frm.x), double.Parse(frm.y), double.Parse(frm.z),
                                                                     bk.Time, frm.Notes, frm.SurfaceLocations);

                }
                else if (dr == DialogResult.Abort)
                {
                    GlobalBookMarkList.Instance.Delete(bk);
                }

                updating = false;
                Display();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (currentedit != null)      // if we have a current cell.. 
            {
                BookmarkClass bk = (BookmarkClass)currentedit.Tag;

                if (ExtendedControls.MessageBoxTheme.Show(FindForm(), "Do you really want to delete the bookmark for " + bk.Name + Environment.NewLine + "Confirm or Cancel", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    updating = true;
                    GlobalBookMarkList.Instance.Delete(bk);
                    updating = false;
                    Display();
                }
            }
        }

        private void dataGridViewBookMarks_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 2)
            {
                SaveBackAnyChanges();
            }
        }

        private void dataGridViewBookMarks_SelectionChanged(object sender, EventArgs e)
        {
            SaveBackAnyChanges();
            RefreshCurrentEdit();
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            searchtimer.Stop();
            searchtimer.Start();
        }

        private void Searchtimer_Tick(object sender, EventArgs e)
        {
            searchtimer.Stop();
            this.Cursor = Cursors.WaitCursor;

            SaveBackAnyChanges();

            StaticFilters.FilterGridView(dataGridViewBookMarks, textBoxFilter.Text);

            RefreshCurrentEdit();

            this.Cursor = Cursors.Default;
        }

        #endregion

        #region Reaction to bookmarks doing stuff from outside sources

        bool updating;
        private void BookmarksChanged(BookmarkClass bk, bool deleted)
        {
            //System.Diagnostics.Debug.WriteLine("Changed called " + updating);
            if (updating)
                return;

            SaveBackAnyChanges();
            Display();
        }

        #endregion

    }
}
