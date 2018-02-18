using EliteDangerousCore.DB;
using System;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlBookmarks : UserControlCommonBase
    {
        int previousSelectedRow = -1;
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
            BookmarkClass.OnBookmarkLoad += Display;
            BookmarkClass.OnBookmarkAdd += Display;
        }

        public override void Closing()
        {
            if(userControlSurfaceBookmarks1.Edited)
            {
                UpdatedBookmark(previousSelectedRow);
            }
            searchtimer.Dispose();
            BookmarkClass.OnBookmarkLoad -= Display;
            BookmarkClass.OnBookmarkAdd -= Display;
        }
        #endregion

        #region display
        public override void InitialDisplay()
        {
            Display();
        }

        private void Display(BookmarkClass newBookMark)
        {
            Display();
        }

        private void Display()
        {
            dataGridViewBookMarks.SuspendLayout();

            dataGridViewBookMarks.Rows.Clear();
            foreach(BookmarkClass bk in BookmarkClass.Bookmarks)
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
            if (BookmarkClass.Bookmarks.Count > 0)
                RefreshSurfaceMarks(0);
        }

        #endregion

        private void dataGridViewBookMarks_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 2)
            {
                BookmarkClass mark = UpdatedBookmark(e.RowIndex);
                dataGridViewBookMarks.Rows[e.RowIndex].Tag = mark;
            }
        }

        private BookmarkClass UpdatedBookmark(int fromRowID)
        {
            BookmarkClass bk = (BookmarkClass)dataGridViewBookMarks.Rows[fromRowID].Tag;
            string newNote = dataGridViewBookMarks.Rows[fromRowID].Cells[2].Value.ToString();
            PlanetMarks latestMarks = userControlSurfaceBookmarks1.PlanetMarks;
            return BookmarkClass.AddOrUpdateBookmark(bk, !bk.isRegion, bk.isRegion ? bk.Heading : bk.StarName, bk.x, bk.y, bk.z, bk.Time, newNote, latestMarks);
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
            if (userControlSurfaceBookmarks1.Edited && previousSelectedRow >= 0)
                UpdatedBookmark(previousSelectedRow);

            if (forNewRow >= 0)
            {
                DataGridViewRow newSelection = dataGridViewBookMarks.Rows[forNewRow];
                previousSelectedRow = newSelection.Index;
                BookmarkClass bk = (BookmarkClass)newSelection.Tag;
                if(bk != null)
                    userControlSurfaceBookmarks1.ApplyBookmark(bk);
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
    }
}
