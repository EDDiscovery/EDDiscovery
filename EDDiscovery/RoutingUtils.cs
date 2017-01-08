using System;
using System.Windows.Forms;
using EDDiscovery.DB;
using EDDiscovery2.DB;
using EDDiscovery.EDSM;
using EDDiscovery2;

namespace EDDiscovery
{
    public class RoutingUtils
    {
        public static void setTargetSystem(EDDiscoveryForm _discoveryForm, String sn)
        {
            setTargetSystem(_discoveryForm, sn, true);

        }
    public static void setTargetSystem(EDDiscoveryForm _discoveryForm, String sn, Boolean prompt)
        {
            if (string.IsNullOrWhiteSpace(sn))
                return;

            ISystem sc = _discoveryForm.history.FindSystem(sn);
            string msgboxtext = null;

            if (sc != null && sc.HasCoordinate)
            {
                SystemNoteClass nc = SystemNoteClass.GetNoteOnSystem(sc.name, sc.id_edsm);        // has it got a note?

                if (nc != null)
                {
                    TargetClass.SetTargetNotedSystem(sc.name, nc.id, sc.x, sc.y, sc.z);
                    msgboxtext = "Target set on system with note " + sc.name;
                }
                else
                {
                    BookmarkClass bk = BookmarkClass.FindBookmarkOnSystem(sn);    // has it been bookmarked?

                    if (bk != null)
                    {
                        TargetClass.SetTargetBookmark(sc.name, bk.id, bk.x, bk.y, bk.z);
                        msgboxtext = "Target set on booked marked system " + sc.name;
                    }
                    else
                    {
                        bool createbookmark = false;
                        if ((prompt && MessageBox.Show("Make a bookmark on " + sc.name + " and set as target?", "Make Bookmark", MessageBoxButtons.OKCancel) == DialogResult.OK) || !prompt)
                        {
                            createbookmark = true;
                        }
                        if (createbookmark)
                        {
                            BookmarkClass newbk = new BookmarkClass();
                            newbk.StarName = sn;
                            newbk.x = sc.x;
                            newbk.y = sc.y;
                            newbk.z = sc.z;
                            newbk.Time = DateTime.Now;
                            newbk.Note = "";
                            newbk.Add();
                            TargetClass.SetTargetBookmark(sc.name, newbk.id, newbk.x, newbk.y, newbk.z);
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

            _discoveryForm.NewTargetSet();          // tells everyone who cares a new target was set

            if (msgboxtext != null && prompt)
                MessageBox.Show(msgboxtext, "Create a target", MessageBoxButtons.OK);

        }

        public static void showBookmarkForm(
            EDDiscoveryForm discoveryForm, ISystem cursystem, BookmarkClass curbookmark, bool notedsystem)
        {
            // try and find the associated bookmark..
            BookmarkClass bkmark = (curbookmark != null) ? curbookmark : BookmarkClass.bookmarks.Find(x => x.StarName != null && x.StarName.Equals(cursystem.name));

            SystemNoteClass sn = (cursystem != null) ? SystemNoteClass.GetNoteOnSystem(cursystem.name, cursystem.id_edsm) : null;
            string note = (sn != null) ? sn.Note : "";

            BookmarkForm frm = new BookmarkForm();

            if (notedsystem && bkmark == null)              // note on a system
            {
                long targetid = TargetClass.GetTargetNotedSystem();      // who is the target of a noted system (0=none)
                long noteid = sn.id;

                frm.InitialisePos(cursystem.x, cursystem.y, cursystem.z);
                frm.NotedSystem(cursystem.name, note, noteid == targetid);       // note may be passed in null
                frm.ShowDialog();

                if ((frm.IsTarget && targetid != noteid) || (!frm.IsTarget && targetid == noteid)) // changed..
                {
                    if (frm.IsTarget)
                        TargetClass.SetTargetNotedSystem(cursystem.name, noteid, cursystem.x, cursystem.y, cursystem.z);
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
                    frm.InitialisePos(cursystem.x, cursystem.y, cursystem.z);
                    tme = DateTime.Now;
                    frm.NewSystemBookmark(cursystem.name, note, tme.ToString());
                }
                else                                        // update bookmark
                {
                    frm.InitialisePos(bkmark.x, bkmark.y, bkmark.z);
                    regionmarker = bkmark.isRegion;
                    tme = bkmark.Time;
                    frm.Update(regionmarker ? bkmark.Heading : bkmark.StarName, note, bkmark.Note, tme.ToString(), regionmarker, targetid == bkmark.id);
                }

                DialogResult res = frm.ShowDialog();

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

            discoveryForm.NewTargetSet();
        }
    }
}
