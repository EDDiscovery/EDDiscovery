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

using EliteDangerousCore.EDSM;
using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    static class TargetHelpers
    {
        public static void setTargetSystem(Object sender, EDDiscoveryForm discoveryform, String sn)
        {
            setTargetSystem(sender, discoveryform, sn, true);

        }
        public static void setTargetSystem(Object sender, EDDiscoveryForm discoveryform, String sn, Boolean prompt)
        {
            Form senderForm = ((Control)sender)?.FindForm() ?? discoveryform;

            if (string.IsNullOrWhiteSpace(sn))
            {
                if (prompt && TargetClass.IsTargetSet())      // if prompting, and target is set, ask for delete
                {
                    if (ExtendedControls.MessageBoxTheme.Show(senderForm, "Confirm deletion of target", "Delete a target", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        TargetClass.ClearTarget();
                        discoveryform.NewTargetSet(sender);          // tells everyone who cares a new target was set
                    }
                }

                return;
            }

            ISystem sc = discoveryform.history.FindSystem(sn);
            string msgboxtext = null;

            if (sc != null && sc.HasCoordinate)
            {
                SystemNoteClass nc = SystemNoteClass.GetNoteOnSystem(sc.Name, sc.EDSMID);        // has it got a note?

                if (nc != null)
                {
                    TargetClass.SetTargetNotedSystem(sc.Name, nc.id, sc.X, sc.Y, sc.Z);
                    msgboxtext = "Target set on system with note " + sc.Name;
                }
                else
                {
                    BookmarkClass bk = GlobalBookMarkList.Instance.FindBookmarkOnSystem(sn);    // has it been bookmarked?

                    if (bk != null)
                    {
                        TargetClass.SetTargetBookmark(sc.Name, bk.id, bk.x, bk.y, bk.z);
                        msgboxtext = "Target set on bookmarked system " + sc.Name;
                    }
                    else
                    {
                        bool createbookmark = false;
                        if ((prompt && ExtendedControls.MessageBoxTheme.Show(senderForm, "Make a bookmark on " + sc.Name + " and set as target?", "Make Bookmark", MessageBoxButtons.OKCancel) == DialogResult.OK) || !prompt)
                        {
                            createbookmark = true;
                        }

                        if (createbookmark)
                        {
                            BookmarkClass newbk = GlobalBookMarkList.Instance.AddOrUpdateBookmark(null, true, sn, sc.X, sc.Y, sc.Z, DateTime.Now, "");
                            TargetClass.SetTargetBookmark(sc.Name, newbk.id, newbk.x, newbk.y, newbk.z);
                        }
                    }
                }

            }
            else
            {
                if (sn.Length > 2 && sn.Substring(0, 2).Equals("G:"))
                    sn = sn.Substring(2, sn.Length - 2);

                GalacticMapObject gmo = discoveryform.galacticMapping.Find(sn, true, true);    // ignore if its off, find any part of string, find if disabled

                if (gmo != null)
                {
                    TargetClass.SetTargetGMO("G:" + gmo.name, gmo.id, gmo.points[0].X, gmo.points[0].Y, gmo.points[0].Z);
                    msgboxtext = "Target set on galaxy object " + gmo.name;
                }
                else
                {
                    msgboxtext = "Unknown system, system is without co-ordinates or galaxy object not found";
                }
            }

            discoveryform.NewTargetSet(sender);          // tells everyone who cares a new target was set

            if (msgboxtext != null && prompt)
                ExtendedControls.MessageBoxTheme.Show(senderForm, msgboxtext, "Create a target", MessageBoxButtons.OK);

        }

        // cursystem = null, curbookmark = null, new system free entry bookmark
        // cursystem != null, curbookmark = null, system bookmark found, update
        // cursystem != null, curbookmark = null, no system bookmark found, new bookmark on system
        // curbookmark != null, edit current bookmark

        public static void showBookmarkForm(Object sender,
            EDDiscoveryForm discoveryForm, ISystem cursystem, BookmarkClass curbookmark, bool notedsystem)
        {
            Form senderForm = ((Control)sender)?.FindForm() ?? discoveryForm;

            // try and find the associated bookmark..
            BookmarkClass bkmark = (curbookmark != null) ? curbookmark : (cursystem != null ? GlobalBookMarkList.Instance.FindBookmarkOnSystem(cursystem.Name) : null);

            SystemNoteClass sn = (cursystem != null) ? SystemNoteClass.GetNoteOnSystem(cursystem.Name, cursystem.EDSMID) : null;
            string note = (sn != null) ? sn.Note : "";

            BookmarkForm frm = new BookmarkForm();

            if (notedsystem && bkmark == null)              // note on a system
            {
                long targetid = TargetClass.GetTargetNotedSystem();      // who is the target of a noted system (0=none)
                long noteid = sn.id;

                frm.InitialisePos(cursystem);
                frm.NotedSystem(cursystem.Name, note, noteid == targetid);       // note may be passed in null
                frm.ShowDialog(senderForm);

                if ((frm.IsTarget && targetid != noteid) || (!frm.IsTarget && targetid == noteid)) // changed..
                {
                    if (frm.IsTarget)
                        TargetClass.SetTargetNotedSystem(cursystem.Name, noteid, cursystem.X, cursystem.Y, cursystem.Z);
                    else
                        TargetClass.ClearTarget();
                }
            }
            else
            {
                bool regionmarker = false;
                DateTime tme;

                if (bkmark == null)                         // new bookmark
                {
                    tme = DateTime.Now;
                    if (cursystem == null)
                        frm.NewFreeEntrySystemBookmark(tme);
                    else
                        frm.NewSystemBookmark(cursystem, note, tme);
                }
                else                                        // update bookmark
                {
                    regionmarker = bkmark.isRegion;
                    tme = bkmark.Time;
                    frm.Update(bkmark);
                }

                DialogResult res = frm.ShowDialog(senderForm);

                long curtargetid = TargetClass.GetTargetBookmark();      // who is the target of a bookmark (0=none)

                if (res == DialogResult.OK)
                {
                    BookmarkClass newcls = GlobalBookMarkList.Instance.AddOrUpdateBookmark(bkmark, !regionmarker, frm.StarHeading, double.Parse(frm.x), double.Parse(frm.y), double.Parse(frm.z),
                                                                     tme, frm.Notes, frm.SurfaceLocations);


                    if ((frm.IsTarget && curtargetid != newcls.id) || (!frm.IsTarget && curtargetid == newcls.id)) // changed..
                    {
                        if (frm.IsTarget)
                            TargetClass.SetTargetBookmark(regionmarker ? ("RM:" + newcls.Heading) : newcls.StarName, newcls.id, newcls.x, newcls.y, newcls.z);
                        else
                            TargetClass.ClearTarget();
                    }
                }
                else if (res == DialogResult.Abort && bkmark != null)
                {
                    if (curtargetid == bkmark.id)
                    {
                        TargetClass.ClearTarget();
                    }

                    GlobalBookMarkList.Instance.Delete(bkmark);
                }
            }

            discoveryForm.NewTargetSet(sender);
        }
    }
}
