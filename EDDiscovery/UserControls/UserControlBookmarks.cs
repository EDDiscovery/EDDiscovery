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
        int previousSelectedRow = -1;
        Timer searchtimer;
        bool updating = false;

        #region init
        public UserControlBookmarks()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            searchtimer = new Timer() { Interval = 500 };
            searchtimer.Tick += Searchtimer_Tick;
            GlobalBookMarkList.Instance.OnBookmarkRefresh += BookmarksRefreshed;
            GlobalBookMarkList.Instance.OnBookmarkChange += BookmarksUpdated;
            GlobalBookMarkList.Instance.OnBookmarkRemoved += BookmarksDeleted;
        }

        public override void Closing()
        {
            if(userControlSurfaceBookmarks.Edited)
            {
                UpdateBookmark(previousSelectedRow);
            }
            searchtimer.Dispose();
            GlobalBookMarkList.Instance.OnBookmarkRefresh -= BookmarksRefreshed;
            GlobalBookMarkList.Instance.OnBookmarkChange -= BookmarksUpdated;
            GlobalBookMarkList.Instance.OnBookmarkRemoved -= BookmarksDeleted;
        }
        #endregion

        #region display
        public override void InitialDisplay()
        {
            Display();
        }
        
        private void BookmarksUpdated(long id)
        {
            if (updating)
                return;
            updating = true;
            if (previousSelectedRow >= 0)
            {
                BookmarkClass bk = (BookmarkClass)dataGridViewBookMarks.Rows[previousSelectedRow].Tag;
                if (bk.id != id)
                    UpdateBookmark(previousSelectedRow);
            }
            Display();
            updating = false;
        }

        private void BookmarksDeleted(long id)
        {
            if (updating)
                return;
            updating = true;
            if (previousSelectedRow >= 0)
            {
                // make sure we don't re-add something we just removed!
                List<BookmarkClass> sel = new List<BookmarkClass> { (BookmarkClass)dataGridViewBookMarks.Rows[previousSelectedRow].Tag };
                sel.RemoveAll(x=>x.id == id);
                if (sel.Any())
                    UpdateBookmark(previousSelectedRow);
            }
            Display();
            updating = false;
        }

        private void BookmarksRefreshed()
        {
            if (updating)
                return;
            try
            {
                updating = true;
                Display();
                updating = false;
            }
            finally
            {
                updating = false;
            }
        }

        private void Display()
        {
            dataGridViewBookMarks.SuspendLayout();

            dataGridViewBookMarks.Rows.Clear();
            foreach(BookmarkClass bk in GlobalBookMarkList.Instance.Bookmarks)
            {
                using (DataGridViewRow dr = dataGridViewBookMarks.Rows[dataGridViewBookMarks.Rows.Add()])
                {
                    dr.Cells[0].Value = bk.isRegion ? "Region" : "System";
                    dr.Cells[1].Value = bk.isRegion ? bk.Heading : bk.StarName;
                    dr.Cells[2].Value = bk.Note;
                    dr.Cells[3].Value = bk.x;
                    dr.Cells[4].Value = bk.y;
                    dr.Cells[5].Value = bk.z;
                    dr.Tag = bk;
                }
            }

            dataGridViewBookMarks.ResumeLayout();
            if (GlobalBookMarkList.Instance.Bookmarks.Count > 0)
                RefreshSurfaceMarks(previousSelectedRow >= 0 && previousSelectedRow < GlobalBookMarkList.Instance.Bookmarks.Count ? previousSelectedRow : 0);
        }

        #endregion

        private void dataGridViewBookMarks_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 2)
            {
                BookmarkClass mark = UpdateBookmark(e.RowIndex);
                dataGridViewBookMarks.Rows[e.RowIndex].Tag = mark;
            }
        }

        private BookmarkClass UpdateBookmark(int fromRowID)
        {
            BookmarkClass bk = (BookmarkClass)dataGridViewBookMarks.Rows[fromRowID].Tag;
            string newNote = dataGridViewBookMarks.Rows[fromRowID].Cells[2].Value.ToString();
            PlanetMarks latestMarks = userControlSurfaceBookmarks.PlanetMarks;
            return GlobalBookMarkList.Instance.AddOrUpdateBookmark(bk, !bk.isRegion, bk.isRegion ? bk.Heading : bk.StarName, bk.x, bk.y, bk.z, bk.Time, newNote, latestMarks);
        }

        private void dataGridViewBookMarks_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewBookMarks.SelectedRows.Count > 0)
                RefreshSurfaceMarks(dataGridViewBookMarks.SelectedRows[0].Index);
        }

        private void dataGridViewBookMarks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RefreshSurfaceMarks(e.RowIndex);
        }

        private void RefreshSurfaceMarks(int forNewRow)
        {
            if (userControlSurfaceBookmarks.Edited && previousSelectedRow >= 0)
                UpdateBookmark(previousSelectedRow);

            if (forNewRow >= 0)
            {
                DataGridViewRow newSelection = dataGridViewBookMarks.Rows[forNewRow];
                previousSelectedRow = newSelection.Index;
                BookmarkClass bk = (BookmarkClass)newSelection.Tag;
                if(bk != null)
                    userControlSurfaceBookmarks.DisplayPlanetMarks(bk);
            }
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

            StaticFilters.FilterGridView(dataGridViewBookMarks, textBoxFilter.Text);
            
            this.Cursor = Cursors.Default;
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (previousSelectedRow >= 0)
            {
                BookmarkForm frm = new BookmarkForm();
                BookmarkClass bk = (BookmarkClass)dataGridViewBookMarks.Rows[previousSelectedRow].Tag;
                frm.Update(bk);
                DialogResult dr = frm.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    GlobalBookMarkList.Instance.AddOrUpdateBookmark(bk, !bk.isRegion, frm.StarHeading, double.Parse(frm.x), double.Parse(frm.y), double.Parse(frm.z),
                                                                     bk.Time, frm.Notes, frm.SurfaceLocations);
                }
                else if(dr == DialogResult.Abort)
                {
                    GlobalBookMarkList.Instance.Delete(bk);
                }
            }

        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            BookmarkForm frm = new BookmarkForm();
            DateTime tme = DateTime.Now;
            frm.NewSystemBookmark(tme.ToString());
            if(frm.ShowDialog(this) == DialogResult.OK)
            {
                GlobalBookMarkList.Instance.AddOrUpdateBookmark(null, true, frm.StarHeading, double.Parse(frm.x), double.Parse(frm.y), double.Parse(frm.z),
                                                                     tme, frm.Notes, frm.SurfaceLocations);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (previousSelectedRow >= 0)
            {
                BookmarkClass bk = (BookmarkClass)dataGridViewBookMarks.Rows[previousSelectedRow].Tag;
                GlobalBookMarkList.Instance.Delete(bk);
                
            }
        }
    }
}
