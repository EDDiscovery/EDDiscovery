/*
 * Copyright © 2017-2019 EDDiscovery development team
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

using System;
using System.Collections.Generic;
using System.Linq;
using BaseUtils;
using ActionLanguage;
using EliteDangerousCore;
using EliteDangerousCore.DB;

namespace EDDiscovery.Actions
{
    public class ActionBookmarks : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(System.Windows.Forms.Form parent, ActionCoreController cp, List<BaseUtils.TypeHelpers.PropertyNameInfo> eventvars)
        {
            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "Command string", UserData, "Configure Bookmark Command" , cp.Icon);
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string res;
            if (ap.functions.ExpandString(UserData, out res) != BaseUtils.Functions.ExpandResult.Failed)
            {
                StringParser sp = new StringParser(res);

                string prefix = "B_";
                string cmdname = sp.NextWord();

                if (cmdname != null && cmdname.Equals("PREFIX", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefix = sp.NextWord();

                    if (prefix == null)
                    {
                        ap.ReportError("Missing name after Prefix in Bookmarks");
                        return true;
                    }

                    cmdname = sp.NextWord();
                }

                if (cmdname != null)
                {
                    if (cmdname.Equals("LIST", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string wildcard = sp.NextQuotedWord() ?? "*";

                        int bcount = 1;
                        foreach (BookmarkClass b in GlobalBookMarkList.Instance.Bookmarks)
                        {
                            if (b.Name.WildCardMatch(wildcard))
                            {
                                string nprefix = prefix + bcount++.ToStringInvariant() + "_";

                                ap[nprefix + "isstar"] = b.isStar.ToStringIntValue();
                                ap[nprefix + "name"] = b.Name;
                                ap[nprefix + "x"] = b.x.ToStringInvariant();
                                ap[nprefix + "y"] = b.y.ToStringInvariant();
                                ap[nprefix + "z"] = b.z.ToStringInvariant();
                                ap[nprefix + "time"] = b.Time.ToStringUS(); // US Date format
                                ap[nprefix + "note"] = b.Note;

                                if ( b.PlanetaryMarks != null )
                                {
                                    string pprefix = nprefix + "Planet_";
                                    ap[pprefix + "Count"] = b.PlanetaryMarks.Planets.Count().ToStringInvariant();

                                    int pcount = 1;
                                    foreach ( PlanetMarks.Planet p in b.PlanetaryMarks.Planets )
                                    {
                                        string plname = pprefix + pcount++.ToStringInvariant() + "_";
                                        ap[plname + "name"] = p.Name;
                                        ap[plname + "Count"] = p.Locations.Count.ToStringInvariant();

                                        int lcount = 1;
                                        foreach( PlanetMarks.Location l in p.Locations )
                                        {
                                            string locname = plname + lcount++.ToStringInvariant() + "_";
                                            ap[locname + "name"] = l.Name;
                                            ap[locname + "comment"] = l.Comment;
                                            ap[locname + "latitude"] = l.Latitude.ToStringInvariant("0.#");
                                            ap[locname + "longitude"] = l.Longitude.ToStringInvariant("0.#");
                                        }
                                    }
                                }
                            }
                        }

                        ap[prefix + "MatchCount"] = (bcount-1).ToStringInvariant();
                        ap[prefix + "TotalCount"] = GlobalBookMarkList.Instance.Bookmarks.Count.ToStringInvariant();

                    }
                    else
                    {
                        string name = sp.NextQuotedWord();
                        bool region = name.Equals("REGION", StringComparison.InvariantCultureIgnoreCase);
                        if (region)
                            name = sp.NextQuotedWord();

                        if (name == null)
                        {
                            ap.ReportError("Missing name in command");
                        }
                        else if (cmdname.Equals("EXIST", StringComparison.InvariantCultureIgnoreCase))
                        {
                            BookmarkClass bk = GlobalBookMarkList.Instance.FindBookmark(name, region);
                            ap[prefix + "Exists"] = (bk != null).ToStringIntValue();
                        }
                        else if (cmdname.Equals("ADD", StringComparison.InvariantCultureIgnoreCase))
                        {
                            double? x = sp.NextDouble();
                            double? y = sp.NextDouble();
                            double? z = sp.NextDouble();
                            string notes = sp.NextQuotedWord(); // valid for it to be null.  Means don't update notes

                            if (x != null && y != null && z != null)
                            {
                                BookmarkClass bk = GlobalBookMarkList.Instance.FindBookmark(name, region);
                                GlobalBookMarkList.Instance.AddOrUpdateBookmark(bk, !region, name, x.Value, y.Value, z.Value, DateTime.Now, notes);
                            }
                            else
                                ap.ReportError("Missing parameters in Add");
                        }
                        else if (cmdname.Equals("DELETE", StringComparison.InvariantCultureIgnoreCase))
                        {
                            BookmarkClass bk = GlobalBookMarkList.Instance.FindBookmark(name, region);
                            if (bk != null)
                                GlobalBookMarkList.Instance.Delete(bk);
                            else
                                ap.ReportError("Delete cannot find star or region " + name);
                        }
                        else if (cmdname.Equals("UPDATENOTE", StringComparison.InvariantCultureIgnoreCase))
                        {
                            string notes = sp.NextQuotedWord();

                            if (notes != null)
                            {
                                BookmarkClass bk = GlobalBookMarkList.Instance.FindBookmark(name, region);
                                if (bk != null)
                                {
                                    bk.UpdateNotes(notes);
                                    GlobalBookMarkList.Instance.TriggerChange(bk);
                                }
                                else
                                    ap.ReportError("UpdateNote cannot find star or region " + name);
                            }
                            else
                                ap.ReportError("UpdateNote notes not present");
                        }
                        else
                        {
                            bool addstar = cmdname.Equals("ADDSTAR", StringComparison.InvariantCultureIgnoreCase);
                            bool addplanet = cmdname.Equals("ADDPLANET", StringComparison.InvariantCultureIgnoreCase);
                            bool deleteplanet = cmdname.Equals("DELETEPLANET", StringComparison.InvariantCultureIgnoreCase);
                            bool updatenoteonplanet = cmdname.Equals("UPDATEPLANETNOTE", StringComparison.InvariantCultureIgnoreCase);
                            bool planetmarkexists = cmdname.Equals("PLANETMARKEXISTS", StringComparison.InvariantCultureIgnoreCase);

                            if ( !addstar && !addplanet && !deleteplanet && !updatenoteonplanet && !planetmarkexists)
                                ap.ReportError("Unknown command");
                            else if ( region )
                                ap.ReportError("Command and REGION are incompatible");
                            else if ( addstar )
                            {
                                ISystem sys = (ap.actioncontroller as ActionController).DiscoveryForm.history.FindSystem(name);

                                if (sys != null)
                                {
                                    string notes = sp.NextQuotedWord();     // valid for it to be null, means don't override or set to empty
                                    BookmarkClass bk = GlobalBookMarkList.Instance.FindBookmarkOnSystem(name);
                                    GlobalBookMarkList.Instance.AddOrUpdateBookmark(bk, true, name, sys.X, sys.Y, sys.Z, DateTime.Now, notes);
                                }
                                else
                                    ap.ReportError("AddStar cannot find star " + name + " in database");
                            }
                            else
                            {
                                BookmarkClass bk = GlobalBookMarkList.Instance.FindBookmarkOnSystem(name);

                                if (bk != null)
                                {
                                    string planet = sp.NextQuotedWord();
                                    string placename = sp.NextQuotedWord();

                                    if (planet != null && placename != null)
                                    {
                                        if (addplanet)
                                        {
                                            double? latp = sp.NextDouble();
                                            double? longp = sp.NextDouble();
                                            string comment = sp.NextQuotedWord();       // can be null

                                            if (planet != null && latp != null && longp != null)
                                            {
                                                bk.AddOrUpdateLocation(planet, placename, comment ?? "", latp.Value, longp.Value);
                                                GlobalBookMarkList.Instance.TriggerChange(bk);
                                            }
                                            else
                                                ap.ReportError("AddPlanet missing parameters");
                                        }
                                        else if (updatenoteonplanet)
                                        {
                                            string comment = sp.NextQuotedWord();
                                            if (comment != null)
                                            {
                                                if (bk.UpdateLocationComment(planet, placename, comment))
                                                    GlobalBookMarkList.Instance.TriggerChange(bk);
                                                else
                                                    ap.ReportError("UpdatePlanetNote no such placename");
                                            }
                                            else
                                                ap.ReportError("UpdatePlanetNote no comment");
                                        }
                                        else if (deleteplanet)
                                        {
                                            if (bk.DeleteLocation(planet, placename))
                                                GlobalBookMarkList.Instance.TriggerChange(bk);
                                            else
                                                ap.ReportError("DeletePlanet no such placename");
                                        }
                                        else if ( planetmarkexists )
                                        {
                                            ap[prefix + "Exists"] = bk.HasLocation(planet, placename).ToStringIntValue();
                                        }
                                    }
                                    else
                                        ap.ReportError("Missing planet and/or placename");
                                }
                                else
                                    ap.ReportError("Cannot find bookmark for " + name);
                            }
                        }
                    }
                }
                else
                    ap.ReportError("Missing Bookmarks command");
            }
            else
                ap.ReportError(res);

            return true;
        }
    }


}
