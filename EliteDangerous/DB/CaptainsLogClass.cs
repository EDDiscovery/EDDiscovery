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
    public class CaptainsLogClass
    {
        public long id { get; private set; }
        public DateTime TimeUTC { get; private set; }
        public string SystemName { get; private set; }
        public string BodyName { get; private set; }
        public string Note { get; private set; }
        public string Tags { get; private set; }     // may be null
        public string Parameters { get; private set; }     // may be null

        public DateTime TimeLocal { get { return TimeUTC.ToLocalTime(); } }

        public CaptainsLogClass()
        {
        }

        public CaptainsLogClass(DataRow dr)
        {
            id = (long)dr["Id"];
            TimeUTC = (DateTime)dr["Time"];
            SystemName = (string)dr["SystemName"];
            BodyName = (string)dr["BodyName"];
            Note = (string)dr["Note"];

            if (System.DBNull.Value != dr["Tags"])
                Tags = (string)dr["Tags"];
            if (System.DBNull.Value != dr["Parameters"])
                Parameters = (string)dr["Parameters"];
        }

        public void Set( string s , string b , DateTime t, string n, string tags = null, string p = null)
        {
            SystemName = s;
            BodyName = b;
            TimeUTC = t;
            Note = n;
            Tags = tags;
            Parameters = p;
        }

        internal bool Add()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())      // open connection..
            {
                return Add(cn);
            }
        }

        private bool Add(SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("Insert into CaptainsLog (Time, SystemName, BodyName, Note, Tags, Parameters) values (@t,@s,@b,@n,@g,@p)"))
            {
                cmd.AddParameterWithValue("@t", TimeUTC);
                cmd.AddParameterWithValue("@s", SystemName);
                cmd.AddParameterWithValue("@b", BodyName);
                cmd.AddParameterWithValue("@n", Note);
                cmd.AddParameterWithValue("@g", Tags);
                cmd.AddParameterWithValue("@p", Parameters);
                cn.SQLNonQueryText( cmd);

                using (DbCommand cmd2 = cn.CreateCommand("Select Max(id) as id from Bookmarks"))
                {
                    id = (long)cn.SQLScalar( cmd2);
                }

                return true;
            }
        }

        internal bool Update()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                return Update(cn);
            }
        }

        private bool Update(SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("Update CaptainsLog set Time=@t, SystemName=@s, BodyName=@b, Note=@n, Tags=@g, Parameters=@p where ID=@id"))
            {
                cmd.AddParameterWithValue("@id", id);
                cmd.AddParameterWithValue("@t", TimeUTC);
                cmd.AddParameterWithValue("@s", SystemName);
                cmd.AddParameterWithValue("@b", BodyName);
                cmd.AddParameterWithValue("@n", Note);
                cmd.AddParameterWithValue("@g", Tags);
                cmd.AddParameterWithValue("@p", Parameters);
                cn.SQLNonQueryText( cmd);

                return true;
            }
        }

        internal bool Delete()
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                return Delete(cn);
            }
        }

        private bool Delete(SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("DELETE FROM CaptainsLog WHERE id = @id"))
            {
                cmd.AddParameterWithValue("@id", id);
                cn.SQLNonQueryText( cmd);
                return true;
            }
        }

        // Update notes
        public void UpdateNotes(string notes)
        {
            Note = notes;
            Update();
        }
       
    }

    // EVERYTHING goes thru list class for adding/deleting log entries

    public class GlobalCaptainsLogList
    {
        public static bool Instanced { get { return gbl != null; } }
        public static GlobalCaptainsLogList Instance { get { return gbl; } }

        public List<CaptainsLogClass> LogEntries { get { return globallog; } }

        public Action<CaptainsLogClass, bool> OnLogEntryChanged;        // bool = true if deleted

        private static GlobalCaptainsLogList gbl = null;

        private List<CaptainsLogClass> globallog = new List<CaptainsLogClass>();

        public static bool LoadLog()
        {
            System.Diagnostics.Debug.Assert(gbl == null);       // no double instancing!
            gbl = new GlobalCaptainsLogList();

            try
            {
                using (SQLiteConnectionUser cn = new SQLiteConnectionUser(mode: SQLLiteExtensions.SQLExtConnection.AccessMode.Reader))
                {
                    using (DbCommand cmd = cn.CreateCommand("select * from CaptainsLog"))
                    {
                        DataSet ds = null;

                        ds = cn.SQLQueryText(cmd);

                        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        {
                            return false;
                        }

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            CaptainsLogClass bc = new CaptainsLogClass(dr);
                            gbl.globallog.Add(bc);
                        }

                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        // return any mark
        //public CaptainsLogClass FindNote(string systemname, string bodyname)
        //{
        //    CaptainsLogClass bc = globallog.Find(x => x.BodyName.Equals(bodyname, StringComparison.InvariantCultureIgnoreCase) && x.SystemName.Equals(systemname, StringComparison.InvariantCultureIgnoreCase));
        //    return bc;
        //}

        //public List<CaptainsLogClass> FindNoteOnSystem(string systemname)
        //{
        //    CaptainsLogClass bc = globallog.Find(x => x.SystemName.Equals(systemname, StringComparison.InvariantCultureIgnoreCase));
        //    return bc;
        //}

        //public CaptainsLogClass FindNoteOnBodyName(string bodyname)
        //{
        //    CaptainsLogClass bc = globallog.Find(x => x.BodyName.Equals(bodyname, StringComparison.InvariantCultureIgnoreCase));
        //    return bc;
        //}

        //public CaptainsLogClass EnsureNoteOnSystem(string systemname, string bodyname,   DateTime tme, string note, string tags = null, string parameters = null )
        //{
        //    CaptainsLogClass bk = FindNote(systemname, bodyname);
        //    return bk != null ? bk : AddOrUpdate(null, systemname, bodyname, tme, note, tags, parameters);
        //}

        // bk = null, new note, else update. Note systemname/bodyname are not unique.  Id it the only unique property

        public CaptainsLogClass AddOrUpdate(CaptainsLogClass bk, string systemname, string bodyname, DateTime tme, string notes, string tags = null, string parameters = null)
        {
            System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);
            bool addit = bk == null;

            if (addit)
            {
                bk = new CaptainsLogClass();
                globallog.Add(bk);
                System.Diagnostics.Debug.WriteLine("New log created");
            }

            bk.Set(systemname, bodyname, tme, notes, tags, parameters);

            if (addit)
                bk.Add();
            else
            {
                System.Diagnostics.Debug.WriteLine(GlobalCaptainsLogList.Instance.LogEntries.Find((xx) => Object.ReferenceEquals(bk, xx)) != null);
                bk.Update();
            }

            System.Diagnostics.Debug.WriteLine("Write captains log " + bk.SystemName + ":" + bk.BodyName + " Notes " + notes);

            OnLogEntryChanged?.Invoke(bk, false);

            return bk;
        }

        public void Delete(CaptainsLogClass bk)
        {
            System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);
            long id = bk.id;
            bk.Delete();
            globallog.RemoveAll(x => x.id == id);
            OnLogEntryChanged?.Invoke(bk, true);
        }

        public void TriggerChange(CaptainsLogClass bk)
        {
            OnLogEntryChanged?.Invoke(bk, true);
        }
    }
}
