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

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace EliteDangerousCore.DB
{
    public class PlanetMarks
    {
        public class Location
        {
            public string Name;
            public string Comment;
            public double Latitude;
            public double Longitude;

            [Newtonsoft.Json.JsonIgnore]
            public bool IsWholePlanetBookmark { get { return Name== "" && Latitude == 0 && Longitude == 0; } }
        }

        public class Planet
        {
            public string Name;
            public List<Location> Locations;            // may be null from reader..
        }

        public List<Planet> Planets;                    // may be null if no planets

        public bool hasMarks { get { return Planets != null && Planets.Count > 0 && Planets.Where(pl => pl.Locations.Count > 0).Any(); } }

        public PlanetMarks(string json)
        {
            try // prevent crashes
            {
                JObject jo = JObject.Parse(json);
                if (jo["Marks"] != null)
                {
                    Planets = jo["Marks"].ToObjectProtected<List<Planet>>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("BK PM " + ex.ToString());
            }
        }

        public PlanetMarks()
        {
        }

        public string ToJsonString()
        {
            if (Planets != null)
            {
                JArray ja = new JArray();
                foreach (Planet p in Planets)
                    ja.Add(JObject.FromObject(p));

                JObject overall = new JObject();
                overall["Marks"] = ja;
                return overall.ToString(Newtonsoft.Json.Formatting.Indented);
            }
            else
                return null;
        }

        public IEnumerator<Tuple<Planet,Location>> GetEnumerator()
        {
            foreach (Planet pl in Planets)
            {
                foreach (Location loc in pl.Locations)
                {
                    yield return new Tuple<Planet,Location>(pl,loc);
                }
            }
        }

        public Planet GetPlanet(string planet)  // null if planet does not exist.. else array
        {
            return Planets?.Find(x => x.Name.Equals(planet, StringComparison.InvariantCultureIgnoreCase));
        }

        public Location GetLocation(Planet p, string placename)  // null if planet or place does not exist..
        {
            return p?.Locations?.Find(x => x.Name.Equals(placename, StringComparison.InvariantCultureIgnoreCase));
        }

        public void AddOrUpdateLocation(string planet, string placename, string comment, double latp, double longp)
        {
            Planet p = GetPlanet(planet);            // p = null if planet does not exist, else list of existing places

            if (p == null)      // no planet, make one up
            {
                if (Planets == null)
                    Planets = new List<Planet>();       // new planet list

                p = new Planet() { Name = planet };
                Planets.Add(p);
            }

            if (p.Locations == null)                    // done here, just in case we read a planet not locations in json.
                p.Locations = new List<Location>();

            Location l = GetLocation(p, placename);     // location on planet by name

            if (l == null)                      // no location.. make one up and add
            {
                l = new Location() { Name = placename, Comment = comment, Latitude = latp, Longitude = longp };
                p.Locations.Add(l);
            }
            else
            {
                l.Comment = comment;        // update fields which may have changed
                l.Latitude = latp;
                l.Longitude = longp;
            }
        }

        public void AddOrUpdateLocation(string planet, Location loc)
        {
            AddOrUpdateLocation(planet, loc.Name, loc.Comment, loc.Latitude, loc.Longitude);
        }

        public void AddOrUpdatePlanetBookmark(string planet, string comment)
        {
            AddOrUpdateLocation(planet, "", comment, 0,0 );
        }

        public bool DeleteLocation(string planet, string placename)
        {
            Planet p = GetPlanet(planet);            // p = null if planet does not exist, else list of existing places
            Location l = GetLocation(p, placename); // if p != null, find placename 
            if (l != null)
            {
                p.Locations.Remove(l);
                if (p.Locations.Count == 0) // nothing left?
                    Planets.Remove(p);  // remove planet.
            }
            return l != null;
        }

        public bool HasLocation(string planet, string placename)
        {
            Planet p = GetPlanet(planet);            // p = null if planet does not exist, else list of existing places
            Location l = GetLocation(p, placename); // if p != null, find placenameYour okay, its 
            return l != null;
        }

        public bool UpdateComment(string planet, string placename, string comment)
        {
            Planet p = GetPlanet(planet);            // p = null if planet does not exist, else list of existing places
            Location l = GetLocation(p, placename); // if p != null, find placenameYour okay, its 
            if (l != null)
            {
                l.Comment = comment;
                return true;
            }
            else
                return false;
        }
    }
        
    public class BookmarkClass
    {
        public long id;
        public string StarName;         // set if associated with a star, else null
        public double x;                // x/y/z always set for render purposes
        public double y;
        public double z;
        public DateTime Time;           
        public string Heading;          // set if region bookmark, else null if its a star
        public string Note;
        public PlanetMarks PlanetaryMarks;   // may be null
        
        public bool isRegion { get { return Heading != null; } }
        public bool isStar { get { return Heading == null; } }
        public string Name { get { return Heading == null ? StarName : Heading; } }

        public bool hasPlanetaryMarks
        { get { return PlanetaryMarks != null && PlanetaryMarks.hasMarks; } }

        public BookmarkClass()
        {
        }

        public BookmarkClass(DbDataReader dr)
        {
            id = (long)dr["id"];
            if (System.DBNull.Value != dr["StarName"])
                StarName = (string)dr["StarName"];
            x = (double)dr["x"];
            y = (double)dr["y"];
            z = (double)dr["z"];
            Time = (DateTime)dr["Time"];
            if (System.DBNull.Value != dr["Heading"])
                Heading = (string)dr["Heading"];
            Note = (string)dr["Note"];
            if (System.DBNull.Value != dr["PlanetMarks"])
            {
                //System.Diagnostics.Debug.WriteLine("Planet mark {0} {1}", StarName, (string)dr["PlanetMarks"]);
                PlanetaryMarks = new PlanetMarks((string)dr["PlanetMarks"]);
            }
        }

        internal bool Add()
        {
            id = UserDatabase.Instance.Add<long>("Bookmarks", "id", new Dictionary<string, object>
            {
                ["StarName"] = StarName,
                ["x"] = x,
                ["y"] = y,
                ["z"] = z,
                ["Time"] = Time,
                ["Heading"] = Heading,
                ["Note"] = Note,
                ["PlanetMarks"] = PlanetaryMarks?.ToJsonString()
            });

            return true;
        }

        internal bool Update()
        {
            UserDatabase.Instance.Update("Bookmarks", "id", id, new Dictionary<string, object>
            {
                ["StarName"] = StarName,
                ["x"] = x,
                ["y"] = y,
                ["z"] = z,
                ["Time"] = Time,
                ["Heading"] = Heading,
                ["Note"] = Note,
                ["PlanetMarks"] = PlanetaryMarks?.ToJsonString()
            });
            return true;
        }

        internal bool Delete()
        {
            UserDatabase.Instance.Delete("Bookmarks", "id", id);
            return true;
        }

        // with a found bookmark.. add locations in the system
        public void AddOrUpdateLocation(string planet, string placename, string comment, double latp, double longp)
        {
            if (PlanetaryMarks == null)
                PlanetaryMarks = new PlanetMarks();
            PlanetaryMarks.AddOrUpdateLocation(planet, placename, comment, latp, longp);
            Update();
        }

        public void AddOrUpdatePlanetBookmark(string planet, string comment)
        {
            if (PlanetaryMarks == null)
                PlanetaryMarks = new PlanetMarks();
            PlanetaryMarks.AddOrUpdatePlanetBookmark(planet, comment);
            Update();
        }

        // Update notes
        public void UpdateNotes(string notes)
        {
            Note = notes;
            Update();
        }
        
        public bool HasLocation(string planet, string placename)
        {
            return PlanetaryMarks != null && PlanetaryMarks.HasLocation(planet, placename);
        }

        public bool DeleteLocation(string planet, string placename)
        {
            if (PlanetaryMarks != null && PlanetaryMarks.DeleteLocation(planet, placename))
            {
                Update();
                return true;
            }
            else
                return false;
        }

        public bool UpdateLocationComment(string planet, string placename, string comment)
        {
            if (PlanetaryMarks != null && PlanetaryMarks.UpdateComment(planet, placename,comment))
            {
                Update();
                return true;
            }
            else
                return false;
        }
    }

    // EVERYTHING goes thru list class for adding/deleting bookmarks

    public class GlobalBookMarkList
    {
        public static bool Instanced { get { return gbl != null; } }
        public static GlobalBookMarkList Instance { get { return gbl; } }

        public List<BookmarkClass> Bookmarks { get { return globalbookmarks; } }

        public Action<BookmarkClass, bool> OnBookmarkChange;        // bool = true if deleted

        private static GlobalBookMarkList gbl = null;

        private List<BookmarkClass> globalbookmarks = new List<BookmarkClass>();

        public static bool LoadBookmarks()
        {
            System.Diagnostics.Debug.Assert(gbl == null);       // no double instancing!
            gbl = new GlobalBookMarkList();

            try
            {
                List<BookmarkClass> bookmarks = UserDatabase.Instance.Retrieve("Bookmarks", rdr => new BookmarkClass(rdr));

                if (bookmarks.Count == 0)
                {
                    return false;
                }
                else
                {
                    foreach (var bc in bookmarks)
                    {
                        gbl.globalbookmarks.Add(bc);
                    }
                    return true;
                }
            }
            catch( Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception " + ex.ToString());
                return false;
            }
        }

        // return any mark
        public BookmarkClass FindBookmarkOnRegion(string name)   
        {
            BookmarkClass bc = globalbookmarks.Find(x => x.Heading != null && x.Heading.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            return bc;
        }

        public BookmarkClass FindBookmarkOnSystem(string name)
        {
            // star name may be null if its a region mark
            return globalbookmarks.Find(x => x.StarName != null && x.StarName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
        public BookmarkClass FindBookmark(string name , bool region)
        {
            // star name may be null if its a region mark
            return (region) ? FindBookmarkOnRegion(name) : FindBookmarkOnSystem(name);
        }

        // on a star system, if an existing bookmark, return it, else create a new one with these properties
        public BookmarkClass EnsureBookmarkOnSystem(string name, double x, double y, double z, DateTime tme, string notes = null)
        {
            BookmarkClass bk = FindBookmarkOnSystem(name);
            return bk != null ? bk : AddOrUpdateBookmark(null, true, name, x, y, z, tme, notes);
        }

        // bk = null, new bookmark, else update.  isstar = true, region = false.
        public BookmarkClass AddOrUpdateBookmark(BookmarkClass bk, bool isstar, string name, double x, double y, double z, DateTime tme, string notes = null, PlanetMarks planetMarks = null)
        {
            System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);
            bool addit = bk == null;

            if (addit)
            {
                bk = new BookmarkClass();
                bk.Note = "";       // set empty, in case notes==null
                globalbookmarks.Add(bk);
                System.Diagnostics.Debug.WriteLine("New bookmark created");
            }

            if (isstar)
                bk.StarName = name;
            else
                bk.Heading = name;

            bk.x = x;
            bk.y = y;
            bk.z = z;
            bk.Time = tme;
            bk.PlanetaryMarks = planetMarks ?? bk.PlanetaryMarks;
            bk.Note = notes ?? bk.Note; // only override notes if its set.

            if (addit)
                bk.Add();
            else
            {
                System.Diagnostics.Debug.WriteLine(GlobalBookMarkList.Instance.Bookmarks.Find((xx) => Object.ReferenceEquals(bk, xx)) != null);
                bk.Update();
            }

            System.Diagnostics.Debug.WriteLine("Write bookmark " + bk.Name + " Notes " + notes);

            OnBookmarkChange?.Invoke(bk,false);

            return bk;
		}	

        public void Delete(BookmarkClass bk)
        {
            System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);
            long id = bk.id;
            bk.Delete();
            globalbookmarks.RemoveAll(x => x.id == id);
            OnBookmarkChange?.Invoke(bk, true);
        }

        public void TriggerChange(BookmarkClass bk)
        {
            OnBookmarkChange?.Invoke(bk, true);
        }
    }
}
