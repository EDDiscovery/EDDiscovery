/*
 * Copyright © 2021 EDDiscovery development team
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

using EliteDangerousCore;
using EliteDangerousCore.DB;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public static class PaintHelpers
    {
        public static void PaintEventColumn(DataGridView grid, DataGridViewRowPostPaintEventArgs e,
                                             int rown,
                                             HistoryEntry he,
                                             int iconcolumn,        //-1 for off
                                             bool showfsdmapcolour)
        {
            System.Diagnostics.Debug.Assert(he != null);    // Trip for debug builds if something is wrong,
            if (he == null)                                 // otherwise, ignore it and return.
                return;

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);

            using (var centerFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Brush br = new SolidBrush(grid.RowHeadersDefaultCellStyle.ForeColor))
                    e.Graphics.DrawString(rown.ToString(), grid.RowHeadersDefaultCellStyle.Font, br, headerBounds, centerFormat);
            }

            if (iconcolumn >= 0 && grid.Columns[iconcolumn].Visible)
            {
                int noicons = (he.IsFSDCarrierJump && showfsdmapcolour) ? 2 : 1;
                if (he.StartMarker || he.StopMarker)
                    noicons++;

                BookmarkClass bk = null;
                if (he.IsLocOrJump)
                {
                    bk = GlobalBookMarkList.Instance.FindBookmarkOnSystem(he.System.Name);
                    if (bk != null)
                        noicons++;
                }

                int padding = 4;
                int size = 24;
                int iconcolumnwidth = grid.Columns[iconcolumn].Width;

                if (size * noicons > (iconcolumnwidth - 2))
                    size = (iconcolumnwidth - 2) / noicons;

                int iconhpos = grid.GetColumnDisplayRectangle(iconcolumn,false).Left;

                int hstart = (iconhpos + iconcolumnwidth / 2) - size / 2 * noicons - padding / 2 * (noicons - 1);

                int top = (e.RowBounds.Top + e.RowBounds.Bottom) / 2 - size / 2;

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                e.Graphics.DrawImage(he.journalEntry.Icon, new Rectangle(hstart, top, size, size));
                hstart += size + padding;

                if (he.journalEntry is IJournalJumpColor && showfsdmapcolour)
                {
                    Color c = Color.FromArgb(((IJournalJumpColor)he.journalEntry).MapColor);

                    using (Brush b = new SolidBrush(c))
                    {
                        e.Graphics.FillEllipse(b, new Rectangle(hstart + 2, top + 2, size - 6, size - 6));
                    }

                    hstart += size + padding;
                }

                if (he.StartMarker)
                {
                    e.Graphics.DrawImage(Icons.Controls.FlagStart, new Rectangle(hstart, top, size, size));
                    hstart += size + padding;
                }
                else if (he.StopMarker)
                {
                    e.Graphics.DrawImage(Icons.Controls.FlagStop, new Rectangle(hstart, top, size, size));
                    hstart += size + padding;
                }
                if (bk != null)
                {
                    e.Graphics.DrawImage(Icons.Controls.Bookmarks, new Rectangle(hstart, top, size, size));
                }
            }
        }

        public class StarColumnOverlays
        {
            public bool landable, materials, volcanism, mapped;     // all false on creation
        }

        public static void PaintStarColumn(DataGridView grid, DataGridViewRowPostPaintEventArgs e, StarColumnOverlays overlays, int iconcolumn, int iconsize, int bodysize)
        {
            var cur = grid.Rows[e.RowIndex];

            if (cur.Tag != null && grid.Columns[iconcolumn].Visible)
            {
                // we programatically draw the image because we have control over its pos/ size this way, which you can't do
                // with a image column - there you can only draw a fixed image or stretch it to cell contents.. which we don't want to do

                int iconhpos = grid.GetColumnDisplayRectangle(iconcolumn, false).Left;
                int vmid = (e.RowBounds.Top + e.RowBounds.Bottom) / 2;

                int hmid = iconhpos + cur.Cells[iconcolumn].Size.Width / 2;

                int icons = 0;

                if (overlays != null)
                    icons = (overlays.mapped ? 1 : 0) + (overlays.volcanism ? 1 : 0) + (overlays.materials ? 1 : 0) + (overlays.landable ? 1 : 0);

                int iconspacing = 2;

                Image img = cur.Tag as Image;
                Rectangle body = new Rectangle(hmid - (iconsize + bodysize + iconspacing) / 2, vmid - bodysize / 2, bodysize, bodysize);
                e.Graphics.DrawImage(img, body);        // main icon

                int right = body.Left + bodysize + iconspacing;
                int vposoverlay = vmid - iconsize * icons / 2;                       // position it centrally vertically

                if (overlays?.landable ?? false)
                {
                    e.Graphics.DrawImage((Image)EDDiscovery.Icons.Controls.Scan_Bodies_Landable, new Rectangle(right, vposoverlay, iconsize, iconsize));
                    vposoverlay += iconsize + iconspacing;
                }

                if (overlays?.materials ?? false)
                {
                    e.Graphics.DrawImage((Image)EDDiscovery.Icons.Controls.Scan_ShowAllMaterials, new Rectangle(right, vposoverlay, iconsize, iconsize));
                    vposoverlay += iconsize + iconspacing;
                }

                if (overlays?.volcanism ?? false)
                {
                    e.Graphics.DrawImage((Image)EDDiscovery.Icons.Controls.Scan_Bodies_Volcanism, new Rectangle(right, vposoverlay, iconsize, iconsize));
                    vposoverlay += iconsize + iconspacing;
                }

                if (overlays?.mapped ?? false)
                {
                    e.Graphics.DrawImage((Image)EDDiscovery.Icons.Controls.Scan_Bodies_Mapped, new Rectangle(right, vposoverlay, iconsize, iconsize));
                }
            }
        }

    }
}