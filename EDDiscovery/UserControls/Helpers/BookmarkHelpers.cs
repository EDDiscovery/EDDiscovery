/*
 * Copyright © 2016-2022 EDDiscovery development team
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

using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using System;

using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    static class BookmarkHelpers
    { 
        // cursystem = null, curbookmark = null, new system free entry bookmark
        // cursystem != null, curbookmark = null, system bookmark found, update
        // cursystem != null, curbookmark = null, no system bookmark found, new bookmark on system
        // curbookmark != null, edit current bookmark
        // region = true, with  cursystem=null,curbookmark = null, region marker

        public static bool ShowBookmarkForm(Control parent, EDDiscoveryForm discoveryForm, ISystem cursystem, BookmarkClass curbookmark, bool region = false)
        {
            // try and find the associated bookmark..
            BookmarkClass bkmark = (curbookmark != null) ? curbookmark : (cursystem != null ? GlobalBookMarkList.Instance.FindBookmarkOnSystem(cursystem.Name) : null);

            BookmarkForm frm = new BookmarkForm(discoveryForm.history, true);       // we handle target changes so show checkbox

            bool regionmarker = false;                  // if region marker..
            DateTime timeutc;

            if (bkmark == null)                         // new bookmark
            {
                timeutc = DateTime.UtcNow;
                if (region == true)
                {
                    frm.NewRegionBookmark(timeutc);
                    regionmarker = true;
                }
                else if (cursystem == null)
                    frm.NewFreeEntrySystemBookmark(timeutc);
                else
                    frm.NewSystemBookmark(cursystem, timeutc);
            }
            else                                        // update bookmark
            {
                regionmarker = bkmark.isRegion;
                timeutc = bkmark.TimeUTC;
                frm.Bookmark(bkmark);
            }

            DialogResult res = frm.ShowDialog(parent);

            long curtargetid = TargetClass.GetTargetBookmarkID();      // who is the current bookmark target? (ID). If not a bookmark, -1

            if (res == DialogResult.OK)
            {
                BookmarkClass newcls = GlobalBookMarkList.Instance.AddOrUpdateBookmark(bkmark, !regionmarker, frm.StarHeading, double.Parse(frm.x), double.Parse(frm.y), double.Parse(frm.z),
                                                                    timeutc, frm.Notes, frm.SurfaceLocations);



                // if form sets target, and our target ID is not the same.. or we have removed the target, we update the target system

                if ((frm.IsTarget && curtargetid != newcls.id) || (!frm.IsTarget && curtargetid == newcls.id)) // changed..
                {
                    if (frm.IsTarget)
                        TargetClass.SetTargetOnBookmark(regionmarker ? ("RM:" + newcls.Heading) : newcls.StarName, newcls.id, newcls.x, newcls.y, newcls.z);
                    else
                        TargetClass.ClearTarget();

                    discoveryForm.NewTargetSet(parent);     // inform system
                }

                return true;
            }
            else if (res == DialogResult.Abort && bkmark != null)
            {
                if (curtargetid == bkmark.id)       // if we deleted it
                {
                    TargetClass.ClearTarget();
                    discoveryForm.NewTargetSet(parent);     // inform system
                }

                GlobalBookMarkList.Instance.Delete(bkmark);

                return true;
            }

            return false;
        }
    }
}
