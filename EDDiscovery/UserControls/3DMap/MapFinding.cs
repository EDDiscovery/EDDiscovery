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
using EliteDangerousCore.GMO;
using OpenTK;
using System;
using System.Drawing;

namespace EDDiscovery.UserControls.Map3D
{
    public partial class Map
    {

        #region Finding

        // Returns a GMO, HE, ISYSTEM, Bookmark
        private Object FindObjectOnMap(Point loc)
        {
            System.Diagnostics.Debug.WriteLine($"Click find object on map");

            // fixed debug loc = new Point(896, 344);

            float hez = float.MaxValue, gmoz = float.MaxValue, galstarz = float.MaxValue, routez = float.MaxValue, navz = float.MaxValue, bkmz = float.MaxValue;

            GalacticMapObject gmo = galmapobjects?.FindPOI(loc, glwfc.RenderState, matrixcalc.ViewPort.Size, out gmoz);      //z are maxvalue if not found
            int? bkm = bookmarks?.Find(loc, glwfc.RenderState, matrixcalc.ViewPort.Size, out bkmz);
            HistoryEntry he = travelpath?.FindSystem(loc, glwfc.RenderState, matrixcalc.ViewPort.Size, out hez) as HistoryEntry;
            ISystem galstar = galaxystars?.Find(loc, glwfc.RenderState, matrixcalc.ViewPort.Size, out galstarz);
            SystemClass route = routepath?.FindSystem(loc, glwfc.RenderState, matrixcalc.ViewPort.Size, out routez) as SystemClass;
            SystemClass nav = navroute?.FindSystem(loc, glwfc.RenderState, matrixcalc.ViewPort.Size, out navz) as SystemClass;

            if (gmo != null && gmoz < bkmz && gmoz < hez && gmoz < galstarz && gmoz < routez && gmoz < navz)      // got gmo, and closer than the others
            {
                System.Diagnostics.Debug.WriteLine($"Find GMO {gmo.NameList}");
                return gmo;
            }

            if (bkm != null && bkmz < hez && bkmz < galstarz && bkmz < routez && bkmz < navz)
            {
                var bks = EliteDangerousCore.DB.GlobalBookMarkList.Instance.Bookmarks;
                System.Diagnostics.Debug.WriteLine($"Find Bookmark {bks[bkm.Value].Name}");
                return bks[bkm.Value];
            }

            if (galstar != null)        // if its set, find it in the db to give more info on position etc
            {
                ISystem s = EliteDangerousCore.SystemCache.FindSystem(galstar.Name); // look up by name
                if (s != null)                          // must normally be found, so return
                    galstar = s;
                else
                    galstar = null;
            }

            // if he set, and less than galstar, or galstar is set and its name is the same as he, use he.  this gives preference to he's over galstar
            if (he != null && (hez < galstarz || (galstar != null && he.System.Name == galstar.Name)))
            {
                System.Diagnostics.Debug.WriteLine($"Find HE {he.System.Name}");
                return he;
            }

            if (galstar != null && galstarz < navz && galstarz < routez)
            {
                System.Diagnostics.Debug.WriteLine($"Find Galstar {galstar.Name}");
                return galstar;
            }

            if (route != null && routez < navz)
            {
                System.Diagnostics.Debug.WriteLine($"Find Route {route.Name}");
                return route;
            }

            if (nav != null)
            {
                System.Diagnostics.Debug.WriteLine($"Find NavRoute {nav.Name}");
                return nav;
            }

            return null;
        }

        private class NLD
        {
            public string DescriptiveName { get; set; }
            public ISystem SystemC { get; set; }        // will be null for GMOs without names
            public Vector3 Location { get; set; }
            public string Description { get; set; }
            public bool PermitRequired { get; set; }
        }

        // from obj, return info about it, its name, location, and description, permit locked
        // understands HEs, G
        private NLD NameLocationDescription(Object obj, HistoryEntry curpos)
        {
            var he = obj as HistoryEntry;
            var gmo = obj as GalacticMapObject;
            var bkm = obj as EliteDangerousCore.DB.BookmarkClass;
            var sys = obj as ISystem;

            if (he != null)     // if we have a he, we have a system, so move it in 
                sys = he.System;

            string descriptivename = gmo != null ? gmo.NameList : bkm != null ? bkm.Name : sys.Name;
            ISystem system = gmo != null ? gmo.StarSystem : bkm != null && bkm.IsStar ? new SystemClass(bkm.StarName) : sys;

            if (system != null)
                System.Diagnostics.Debug.WriteLine($"3dmap Lookup ISystem {system.Name} edsmid {system.EDSMID} sa {system.SystemAddress}");

            if (bkm != null)
                descriptivename = "Bookmark " + descriptivename;

            Vector3 pos = gmo != null ? new Vector3((float)gmo.Points[0].X, (float)gmo.Points[0].Y, (float)gmo.Points[0].Z) :
                                bkm != null ? new Vector3((float)bkm.X, (float)bkm.Y, (float)bkm.Z) :
                                    new Vector3((float)sys.X, (float)sys.Y, (float)sys.Z);

            string info = "";

            if (curpos != null)
            {
                double dist = curpos.System.Distance(pos.X, pos.Y, pos.Z);
                if (dist > 0)
                    info += $"Distance {dist:N1} ly";
            }

            bool permit = false;
            if (sys != null)
            {
                if (sys.MainStarType != EDStar.Unknown)
                    info = info.AppendPrePad($"Star Type {Stars.ToLocalisedLanguage(sys.MainStarType)}", Environment.NewLine);
                if (EliteDangerousCore.DB.SystemsDatabase.Instance.IsPermitSystem(sys))
                {
                    info = info.AppendPrePad("Permit System", Environment.NewLine);
                    permit = true;
                }
            }

            info = info.AppendPrePad($"Position {pos.X:0.#}, {pos.Y:0.#}, {pos.Z:0.#}" + Environment.NewLine, Environment.NewLine);

            if (he != null)
            {
                info = info.AppendPrePad("Visited on:", Environment.NewLine);

                foreach (var e in travelpath.CurrentListHE)
                {
                    if (e.System.Name.Equals(he.System.Name))
                    {
                        var t = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(e.EventTimeUTC);
                        info = info.AppendPrePad($"{t.ToString()}", Environment.NewLine);
                    }
                }
            }
            else if (gmo != null)
            {
                info = info.AppendPrePad(gmo.Description, Environment.NewLine);
            }
            else if (bkm != null)
            {
                info = info.AppendPrePad(bkm.Note, Environment.NewLine);
            }

            return new NLD() { DescriptiveName = descriptivename, SystemC = system, Location = pos, Description = info, PermitRequired = permit };
        }

        #endregion

    }
}
