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
        public static void setTargetSystem(Object sender, EDDiscoveryForm _discoveryForm, String sn)
        {
            setTargetSystem(sender, _discoveryForm, sn, true);

        }
        public static void setTargetSystem(Object sender, EDDiscoveryForm _discoveryForm, String sn, Boolean prompt)
        {
            Form senderForm = ((Control)sender)?.FindForm() ?? _discoveryForm;

            if (string.IsNullOrWhiteSpace(sn))
            {
                if (prompt && TargetClass.IsTargetSet())      // if prompting, and target is set, ask for delete
                {
                    if (ExtendedControls.MessageBoxTheme.Show(senderForm, "Confirm deletion of target", "Delete a target", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        TargetClass.ClearTarget();
                        _discoveryForm.NewTargetSet(sender);          // tells everyone who cares a new target was set
                    }
                }

                return;
            }

            ISystem sc = _discoveryForm.history.FindSystem(sn);
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
                    BookmarkClass bk = BookmarkClass.FindBookmarkOnSystem(sn);    // has it been bookmarked?

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
                            BookmarkClass newbk = new BookmarkClass();
                            newbk.StarName = sn;
                            newbk.x = sc.X;
                            newbk.y = sc.Y;
                            newbk.z = sc.Z;
                            newbk.Time = DateTime.Now;
                            newbk.Note = "";
                            newbk.Add();
                            TargetClass.SetTargetBookmark(sc.Name, newbk.id, newbk.x, newbk.y, newbk.z);
                        }
                    }
                }

            }
            else
            {
                if (sn.Length > 2 && sn.Substring(0, 2).Equals("G:"))
                    sn = sn.Substring(2, sn.Length - 2);

                GalacticMapObject gmo = _discoveryForm.galacticMapping.Find(sn, true, true);    // ignore if its off, find any part of string, find if disabled

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

            _discoveryForm.NewTargetSet(sender);          // tells everyone who cares a new target was set

            if (msgboxtext != null && prompt)
                ExtendedControls.MessageBoxTheme.Show(senderForm, msgboxtext, "Create a target", MessageBoxButtons.OK);

        }

        public static void showBookmarkForm(Object sender,
            EDDiscoveryForm discoveryForm, ISystem cursystem, BookmarkClass curbookmark, bool notedsystem)
        {
            Form senderForm = ((Control)sender)?.FindForm() ?? discoveryForm;

            // try and find the associated bookmark..
            BookmarkClass bkmark = (curbookmark != null) ? curbookmark : BookmarkClass.bookmarks.Find(x => x.StarName != null && x.StarName.Equals(cursystem.Name));

            SystemNoteClass sn = (cursystem != null) ? SystemNoteClass.GetNoteOnSystem(cursystem.Name, cursystem.EDSMID) : null;
            string note = (sn != null) ? sn.Note : "";

            BookmarkForm frm = new BookmarkForm();

            if (notedsystem && bkmark == null)              // note on a system
            {
                long targetid = TargetClass.GetTargetNotedSystem();      // who is the target of a noted system (0=none)
                long noteid = sn.id;

                frm.InitialisePos(cursystem.X, cursystem.Y, cursystem.Z);
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

                long targetid = TargetClass.GetTargetBookmark();      // who is the target of a bookmark (0=none)

                if (bkmark == null)                         // new bookmark
                {
                    frm.InitialisePos(cursystem.X, cursystem.Y, cursystem.Z);
                    tme = DateTime.Now;
                    frm.NewSystemBookmark(cursystem.Name, note, tme.ToString());
                }
                else                                        // update bookmark
                {
                    frm.InitialisePos(bkmark.x, bkmark.y, bkmark.z);
                    regionmarker = bkmark.isRegion;
                    tme = bkmark.Time;
                    frm.Update(regionmarker ? bkmark.Heading : bkmark.StarName, note, bkmark.Note, tme.ToString(), regionmarker, targetid == bkmark.id);
                }

                DialogResult res = frm.ShowDialog(senderForm);

                if (res == DialogResult.OK)
                {
                    BookmarkClass newcls = new BookmarkClass();

                    if (regionmarker)
                        newcls.Heading = frm.StarHeading;
                    else
                        newcls.StarName = frm.StarHeading;

                    newcls.x = double.Parse(frm.x);
                    newcls.y = double.Parse(frm.y);
                    newcls.z = double.Parse(frm.z);
                    newcls.Time = tme;
                    newcls.Note = frm.Notes;

                    if (bkmark != null)
                    {
                        newcls.id = bkmark.id;
                        newcls.Update();
                    }
                    else
                        newcls.Add();

                    if ((frm.IsTarget && targetid != newcls.id) || (!frm.IsTarget && targetid == newcls.id)) // changed..
                    {
                        if (frm.IsTarget)
                            TargetClass.SetTargetBookmark(regionmarker ? ("RM:" + newcls.Heading) : newcls.StarName, newcls.id, newcls.x, newcls.y, newcls.z);
                        else
                            TargetClass.ClearTarget();
                    }
                }
                else if (res == DialogResult.Abort && bkmark != null)
                {
                    if (targetid == bkmark.id)
                    {
                        TargetClass.ClearTarget();
                    }

                    bkmark.Delete();
                }
            }

            discoveryForm.NewTargetSet(sender);
        }
    }
}
