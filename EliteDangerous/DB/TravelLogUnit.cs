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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace EliteDangerousCore.DB
{
    public class TravelLogUnit
    {
        public const int NetLogType = 1;
        public const int EDSMType = 2;
        public const int JournalType = 3;
        public const int TypeMask = 0xff;
        public const int BetaMarker = 0x8000;

        public long id;
        public string Name;
        public int type;            // bit 15 = BETA.  Type = 2 EDSM log, 3 = Journal, 1 = Old pre 2.1 logs.
        public int Size;
        public string Path;
        public int? CommanderId;

        public TravelLogUnit()
        {
        }

        public TravelLogUnit(DataRow dr)
        {
            Object obj;
            id = (long)dr["id"];
            Name = (string)dr["Name"];
            type = (int)(long)dr["type"];
            Size = (int)(long)dr["size"];
            Path = (string)dr["Path"];
             obj = dr["CommanderId"];

            if (obj == DBNull.Value)
                CommanderId = null; 
            else
                CommanderId = (int)(long)dr["CommanderId"];

        }

        public TravelLogUnit(DbDataReader dr)
        {
            Object obj;
            id = (long)dr["id"];
            Name = (string)dr["Name"];
            type = (int)(long)dr["type"];
            Size = (int)(long)dr["size"];
            Path = (string)dr["Path"];
            obj =dr["CommanderId"];

            if (obj == DBNull.Value)
                CommanderId = null;  // TODO  use better default value?
            else
                CommanderId = (int)(long)dr["CommanderId"];

        }

        public bool Beta
        {
            get
            {
                if ((Path != null && Path.Contains("PUBLIC_TEST_SERVER")) || (type & BetaMarker) == BetaMarker)
                    return true;
                else
                    return false;
            }
        }


        public bool Add()
        {
            id = UserDatabase.Instance.Add<long>("TravelLogUnit", "id", new Dictionary<string, object>
            {
                ["Name"] = Name,
                ["Type"] = type,
                ["Size"] = Size,
                ["Path"] = Path,
                ["CommanderID"] = CommanderId
            });

            return true;
        }

        public bool Update()
        {
            UserDatabase.Instance.Update("TravelLogUnit", "id", id, new Dictionary<string, object>
            {
                ["Name"] = Name,
                ["Type"] = type,
                ["Size"] = Size,
                ["Path"] = Path,
                ["CommanderID"] = CommanderId
            });

            return true;
        }
        
        static public List<TravelLogUnit> GetAll()
        {
            return UserDatabase.Instance.Retrieve("TravelLogUnit", rdr => new TravelLogUnit(rdr));
        }

        public static List<string> GetAllNames()
        {
            var names = new List<string>();

            foreach (var row in UserDatabase.Instance.Retrieve("TravelLogUnit", new[] { "Name" }))
            {
                names.Add((string)row[0]);
            }

            return names;
        }

        public static TravelLogUnit Get(string name)
        {
            var logs = UserDatabase.Instance.Retrieve(
                "TravelLogUnit", 
                rdr => new TravelLogUnit(rdr), 
                where: "Name = @Name", 
                whereparams: new Dictionary<string, object> { ["Name"] = name }, 
                orderby: "Id DESC"
            );

            if (logs.Count != 0)
            {
                return logs[0];
            }
            else
            {
                return null;
            }
        }

        public static bool TryGet(string name, out TravelLogUnit tlu)
        {
            tlu = Get(name);
            return tlu != null;
        }

        public static TravelLogUnit Get(long id)
        {
            var logs = UserDatabase.Instance.Retrieve(
                "TravelLogUnit",
                rdr => new TravelLogUnit(rdr),
                where: "Id = @Id",
                whereparams: new Dictionary<string, object> { ["Id"] = id }
            );

            if (logs.Count != 0)
            {
                return logs[0];
            }
            else
            {
                return null;
            }
        }

        public static bool TryGet(long id, out TravelLogUnit tlu)
        {
            tlu = Get(id);
            return tlu != null;
        }
    }
}

