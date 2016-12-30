using System;
using System.Windows.Forms;
using EDDiscovery.DB;
using EDDiscovery2.DB;
using EDDiscovery.EDSM;

namespace EDDiscovery
{
    public class RoutingUtils
    {
        public static void setTargetSystem(EDDiscoveryForm _discoveryForm, String sn)
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
                        if (MessageBox.Show("Make a bookmark on " + sc.name + " and set as target?", "Make Bookmark", MessageBoxButtons.OKCancel) == DialogResult.OK)
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

            if (msgboxtext != null)
                MessageBox.Show(msgboxtext, "Create a target", MessageBoxButtons.OK);

        }
    }
}
