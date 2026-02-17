/*
 * Copyright 2019-2023 Robbyxp1 @ github.com
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
 */

using EliteDangerousCore;
using GLOFC;
using GLOFC.GL4.Controls;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static GLOFC.GL4.Controls.GLBaseControl;

namespace EDDiscovery.UserControls.Map3D
{
    public partial class Map
    {
        #region Updators
        public void UpdateTravelPath()   // new history entry
        {
            if (travelpath != null)
            {
                travelpath.CreatePath(parent.DiscoveryForm.History, galmapobjects?.PositionsWithEnable);
                travelpath.SetSystem(parent.DiscoveryForm.History.LastSystem);
            }

            CheckRefreshLocalArea();  // also see if local stars need updating
        }

        public void UpdateNavRoute()
        {
            if (navroute != null)
            {
                var route = parent.DiscoveryForm.History.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.NavRoute)?.journalEntry as EliteDangerousCore.JournalEvents.JournalNavRoute;
                if (route?.Route != null) // If a navroute with a valid route..
                {
                    var syslist = route.Route.Select(x => new SystemClass(x.StarSystem, null, x.StarPos.X, x.StarPos.Y, x.StarPos.Z, SystemSource.FromJournal, x.EDStarClass)).Cast<ISystem>().ToList();
                    navroute.CreatePath(syslist, Color.Purple, galmapobjects?.PositionsWithEnable);
                }
            }
        }

        public void UpdateRoutePath()
        {
            if (routepath != null)
            {
                var curroute = routepath.CurrentListSystem != null ? routepath.CurrentListSystem : new List<ISystem>();
                routepath.CreatePath(curroute, Color.Green, galmapobjects?.PositionsWithEnable);
            }
        }

        public void UpdateDueToGMOEnableChanging()       // feed in list of galmapobject positions to other classes so they don't repeat
        {
            if (galmapobjects != null)
            {
                if (galaxystars != null)
                {
                    galaxystars.Clear();
                }
                if (travelpath != null)
                {
                    UpdateTravelPath();
                }
                if (navroute != null)
                {
                    UpdateNavRoute();
                }
                if (routepath != null)
                {
                    UpdateRoutePath();
                }
            }
        }

        public void UpdateEDSMStarsLocalArea()
        {
            if (galaxystars != null)
            {
                galaxystars.ClearBoxAround();
                CheckRefreshLocalArea();
            }
        }

        public void CheckRefreshLocalArea()
        {
            HistoryEntry he = parent.DiscoveryForm.History.GetLast;       // may be null
            // basic check we are operating in this mode
            if (galaxystars != null && he != null && he.System.HasCoordinate && (parts & Parts.PrepopulateGalaxyStarsLocalArea) != 0)
            {
                var hepos = new Vector3((float)he.System.X, (float)he.System.Y, (float)he.System.Z);

                if ((galaxystars.CurrentPos - hepos).Length >= LocalAreaSize / 4)       // if current pos too far away, go for it
                {
                    galaxystars.ClearBoxAround();
                    galaxystars.RequestBoxAround(hepos, LocalAreaSize);      // this sets CurrentPos
                }
            }
        }
        public void AddMoreStarsAtLookat()
        {
            galaxystars.Request9x3Box(gl3dcontroller.PosCamera.LookAt);
        }

        public void GoToTravelSystem(int dir)      //0 = current, 1 = next, -1 = prev
        {
            var isys = dir == 0 ? travelpath.CurrentSystem : (dir < 0 ? travelpath.PrevSystem() : travelpath.NextSystem());
            if (isys != null)
            {
                gl3dcontroller.SlewToPosition(new Vector3((float)isys.X, (float)isys.Y, (float)isys.Z), -1);
                SetEntryText(isys.Name);
            }
        }

        public void GoToCurrentSystem(float lydist = 50f)
        {
            HistoryEntry he = parent.DiscoveryForm.History.GetLast;       // may be null
            if (he != null)
            {
                GoToSystem(he.System, lydist);
                travelpath?.SetSystem(he.System.Name);
            }
        }

        public void GoToSystem(ISystem isys, float lydist = 50f)
        {
            gl3dcontroller.SlewToPositionZoom(new Vector3((float)isys.X, (float)isys.Y, (float)isys.Z), gl3dcontroller.ZoomDistance / lydist, -1);
            SetEntryText(isys.Name);
        }

        public void ViewGalaxy()
        {
            gl3dcontroller.PosCamera.CameraRotation = 0;
            gl3dcontroller.PosCamera.GoToZoomPan(new Vector3(0, 0, 0), new Vector2(140.75f, 0), 0.5f, 3);
        }

        public void SetRoute(List<ISystem> syslist)
        {
            if (routepath != null)
                routepath.CreatePath(syslist, Color.Green, galmapobjects?.PositionsWithEnable);
        }

        private void SetEntryText(string text)
        {
            if (galaxymenu.EntryTextBox != null)
            {
                galaxymenu.EntryTextBox.Text = text;
                galaxymenu.EntryTextBox.CancelAutoComplete();
            }
            displaycontrol.SetFocus();
        }



        #endregion

        #region Bookmarks

        public void UpdateBookmarks()
        {
            if (bookmarks != null)
            {
                var bks = EliteDangerousCore.DB.GlobalBookMarkList.Instance.Bookmarks;
                var list = bks.Select(a => new Vector4((float)a.X, (float)a.Y + 1.5f, (float)a.Z, 1)).ToArray();
                bookmarks.Create(list);
                FillBookmarkForm();
            }
        }

        public void EditBookmark(EliteDangerousCore.DB.BookmarkClass bkm)
        {
            var res = BookmarkHelpers.ShowBookmarkForm(parent.DiscoveryForm, parent.DiscoveryForm, null, bkm);
            if (res)
                UpdateBookmarks();
        }
        public void DeleteBookmark(EliteDangerousCore.DB.BookmarkClass bkm)
        {
            GLMessageBox msg = new GLMessageBox("Confirm", displaycontrol, new Point(int.MinValue, 0), "Confirm Deletion of bookmark" + " " + bkm.Name, "Warning",
                            GLMessageBox.MessageBoxButtons.OKCancel,
                            callback: (mbox, dr) =>
                            {
                                if (dr == GLForm.DialogResultEnum.OK)
                                {
                                    EliteDangerousCore.DB.GlobalBookMarkList.Instance.Delete(bkm);
                                    UpdateBookmarks();
                                }
                            }
                        );
        }

        GLForm bmform = null;

        public void ToggleBookmarkList(bool on)
        {
            System.Diagnostics.Debug.WriteLine($"Bookmark list {on}");

            if (bmform == null)
            {
                bmform = new GLForm("BKForm", "Bookmarks", new Rectangle(5, 40, Math.Min(400, displaycontrol.Width - 30), Math.Min(600, displaycontrol.Height - 60)));
                GLDataGridView dgv = null;
                dgv = new GLDataGridView("BKDGV", new Rectangle(10, 10, 10, 10));
                dgv.Dock = DockingType.Fill;
                dgv.SelectCellSelectsRow = true;
                dgv.AllowUserToSelectMultipleRows = false;
                dgv.ColumnFillMode = GLDataGridView.ColFillMode.FillWidth;
                dgv.HorizontalScrollVisible = false;
                dgv.SelectRowOnRightClick = true;
                dgv.RowHeaderEnable = false;
                var col0 = dgv.CreateColumn(fillwidth: 100, title: "Star");
                var col1 = dgv.CreateColumn(fillwidth: 50, title: "X");
                col1.SortCompare = GLDataGridViewSorts.SortCompareNumeric;
                var col2 = dgv.CreateColumn(fillwidth: 50, title: "Y");
                col2.SortCompare = GLDataGridViewSorts.SortCompareNumeric;
                var col3 = dgv.CreateColumn(fillwidth: 50, title: "Z");
                col3.SortCompare = GLDataGridViewSorts.SortCompareNumeric;
                var col4 = dgv.CreateColumn(fillwidth: 100, title: "Note");
                dgv.AddColumn(col0);
                dgv.AddColumn(col1);
                dgv.AddColumn(col2);
                dgv.AddColumn(col3);
                dgv.AddColumn(col4);

                dgv.MouseClickOnGrid += (row, col, mouseevent) =>       // intercept mouse click on grid rather than row selection since we can see what button clicked it
                {
                    if (mouseevent.Button == GLMouseEventArgs.MouseButtons.Left && row >= 0)
                    {
                        var bk = dgv.Rows[row].Tag as EliteDangerousCore.DB.BookmarkClass;
                        gl3dcontroller.SlewToPosition(new Vector3((float)bk.X, (float)bk.Y, (float)bk.Z), -1);
                    }
                };

                dgv.ContextMenuGrid = new GLContextMenu("BookmarksRightClickMenu", true,
                    new GLMenuItem("BKEdit", "Edit")
                    {
                        MouseClick = (s, e) =>
                        {
                            var pos = dgv.ContextMenuGrid.Tag as GLDataGridView.RowColPos;
                            System.Diagnostics.Debug.WriteLine($"Click on {pos.Row}");
                            if (pos.Row >= 0)
                            {
                                var bk = dgv.Rows[pos.Row].Tag as EliteDangerousCore.DB.BookmarkClass;
                                EditBookmark(bk);
                            }
                        }
                    },
                    new GLMenuItem("BKNew", "New")
                    {
                        MouseClick = (s, e) =>
                        {
                            var res = BookmarkHelpers.ShowBookmarkForm(parent.DiscoveryForm, parent.DiscoveryForm, null, null);
                            if (res)
                                UpdateBookmarks();
                        }
                    },
                    new GLMenuItem("BKDelete", "Delete")
                    {
                        MouseClick = (s, e) =>
                        {
                            var pos = dgv.ContextMenuGrid.Tag as GLDataGridView.RowColPos;
                            System.Diagnostics.Debug.WriteLine($"Click on {pos.Row}");
                            if (pos.Row >= 0)
                            {
                                var bk = dgv.Rows[pos.Row].Tag as EliteDangerousCore.DB.BookmarkClass;
                                DeleteBookmark(bk);
                            }
                        }
                    });

                dgv.ContextMenuGrid.Opening = (cms, tag) => {
                    var rcp = tag as GLDataGridView.RowColPos;
                    cms["BKEdit"].Visible = cms["BKDelete"].Visible = rcp.Row >= 0;
                    cms.Tag = rcp; // transfer the opening position tag to the cms for the menu items, so it can get it
                };


                bmform.Add(dgv);
                bmform.FormClosed += (f) => { bmform = null; displaycontrol.ApplyToControlOfName("MSTPBookmarks", (c) => { ((GLCheckBox)c).CheckedNoChangeEvent = false; }); };


                FillBookmarkForm();

                displaycontrol.Add(bmform);
            }

            bmform.Visible = on;
        }

        #endregion

        #region Helpers

        private void FillBookmarkForm()
        {
            if (bmform != null)
            {
                var dgv = bmform.ControlsZ[0] as GLDataGridView;
                dgv.Clear();

                var gl = EliteDangerousCore.DB.GlobalBookMarkList.Instance.Bookmarks;

                foreach (var bk in gl)
                {
                    var row = dgv.CreateRow();

                    row.AddCell(new GLDataGridViewCellText(bk.Name),
                                new GLDataGridViewCellText(bk.X.ToString("N1")), new GLDataGridViewCellText(bk.Y.ToString("N1")), new GLDataGridViewCellText(bk.Z.ToString("N1")),
                                new GLDataGridViewCellText(bk.Note));
                    row.Tag = bk;
                    row.AutoSize = true;
                    dgv.AddRow(row);
                }
            }
        }

        #endregion


        #region Images

        public void LoadImages()
        {
            if (userimages != null)
            {
                usertexturebitmaps.Clear();
                userimages.LoadBitmaps(
                    null,   // resources
                    (ie) =>  // text
                    {
                        BaseUtils.StringParser sp = new BaseUtils.StringParser(ie.ImagePathOrURL);

                        // "Text with escape",font,size,colorfore,colorback,format,bitmapwidth,bitmapheight.
                        // quote words if they have spaces
                        // format is centre,right,left,top,bottom,middle,nowrap

                        string text = sp.NextQuotedWordComma(replaceescape: true);

                        if (text != null)
                        {
                            string fontname = sp.NextQuotedWordComma() ?? "Arial";     // all may be null  
                            double size = sp.NextDoubleComma(",") ?? 10;
                            string forecolour = sp.NextQuotedWordComma() ?? "White";
                            string backcolour = sp.NextQuotedWordComma() ?? "Transparent";
                            string format = sp.NextQuotedWordComma() ?? "centre middle";
                            int bitmapwidth = sp.NextIntComma(",") ?? 1024;
                            int bitmapheight = sp.NextInt(",") ?? 256;

                            Font fnt = new Font(fontname, (float)size);
                            Color fore = Color.FromName(forecolour);
                            Color back = Color.FromName(backcolour);
                            StringFormat fmt = format.StringFormatFromName();

                            usertexturebitmaps.Add(null, null, text, fnt, fore, back, new Size(bitmapwidth, bitmapheight),
                                new Vector3(ie.Centre.X, ie.Centre.Y, ie.Centre.Z),
                                new Vector3(ie.Size.X, 0, ie.Size.Y),
                                new Vector3(ie.RotationDegrees.X.Radians(), ie.RotationDegrees.Y.Radians(), ie.RotationDegrees.Z.Radians()),
                                fmt, 1,
                                ie.RotateToViewer, ie.RotateElevation, ie.AlphaFadeScalar, ie.AlphaFadePosition);
                            fnt.Dispose();
                            fmt.Dispose();
                        }

                    },
                    (ie) => // http load
                    {
                        System.Threading.Tasks.Task.Run(() =>
                        {
                            string name = ie.ImagePathOrURL;
                            string filename = ie.ImagePathOrURL.Replace("http://", "", StringComparison.InvariantCultureIgnoreCase).
                                        Replace("https://", "", StringComparison.InvariantCultureIgnoreCase).SafeFileString();
                            string path = System.IO.Path.Combine(EDDOptions.Instance.DownloadedImages(), filename);
                            System.Diagnostics.Debug.WriteLine($"HTTP load {name} to {path}");

                            System.Threading.CancellationToken cancel = new System.Threading.CancellationToken();

                            bool res = BaseUtils.HttpCom.DownloadFileFromURI(cancel, name, path, false, out bool newfile);

                            if (res)
                            {
                                parent.DiscoveryForm.BeginInvoke((MethodInvoker)delegate
                                {
                                    if (ie.LoadBitmap(path))
                                    {
                                        usertexturebitmaps.Add(null, null, ie.Bitmap, 1,
                                            new Vector3(ie.Centre.X, ie.Centre.Y, ie.Centre.Z),
                                            new Vector3(ie.Size.X, 0, ie.Size.Y),
                                            new Vector3(ie.RotationDegrees.X.Radians(), ie.RotationDegrees.Y.Radians(), ie.RotationDegrees.Z.Radians()),
                                            ie.RotateToViewer, ie.RotateElevation, ie.AlphaFadeScalar, ie.AlphaFadePosition);
                                    }
                                });
                            }
                        });
                    },
                    (ie) => //file
                    {
                        usertexturebitmaps.Add(null, null, ie.Bitmap, 1,
                            new Vector3(ie.Centre.X, ie.Centre.Y, ie.Centre.Z),
                            new Vector3(ie.Size.X, 0, ie.Size.Y),
                            new Vector3(ie.RotationDegrees.X.Radians(), ie.RotationDegrees.Y.Radians(), ie.RotationDegrees.Z.Radians()),
                            ie.RotateToViewer, ie.RotateElevation, ie.AlphaFadeScalar, ie.AlphaFadePosition);
                    },
                    (ie) =>  // image:
                    {
                        string name = ie.ImagePathOrURL.Substring(6).Trim();
                        if (BaseUtils.Icons.IconSet.Contains(name))
                        {
                            ie.Bitmap = (Bitmap)BaseUtils.Icons.IconSet.GetImage(name);
                            ie.OwnBitmap = false;
                            usertexturebitmaps.Add(null, null, ie.Bitmap, 1,
                                new Vector3(ie.Centre.X, ie.Centre.Y, ie.Centre.Z),
                                new Vector3(ie.Size.X, 0, ie.Size.Y),
                                new Vector3(ie.RotationDegrees.X.Radians(), ie.RotationDegrees.Y.Radians(), ie.RotationDegrees.Z.Radians()),
                                ie.RotateToViewer, ie.RotateElevation, ie.AlphaFadeScalar, ie.AlphaFadePosition);
                        }
                    }
                    );

            }
        }

        #endregion

    }
}
