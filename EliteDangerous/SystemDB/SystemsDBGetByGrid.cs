/*
 * Copyright © 2015 - 2019 EDDiscovery development team
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
using System.Data.Common;
using System.Drawing;

namespace EliteDangerousCore.DB
{
    public partial class SystemsDB
    {
        public enum SystemAskType { AllStars, SplitPopulatedStars, UnpopulatedStars, PopulatedStars };

        // all stars/Unpopulated/Poplulated only
        public static void GetSystemVector<V>(int gridid, ref V[] vertices1, ref uint[] colours1, int percentage, Func<int, int, int, V> tovect, SystemAskType ask = SystemAskType.AllStars)
        {
            V[] v2 = null;
            uint[] c2 = null;
            GetSystemVector<V>(gridid, ref vertices1, ref colours1, ref v2, ref c2, percentage, tovect, ask);
        }

        // full interface. 
        // ask = AllStars/UnpopulatedStars/PopulatedStars = only v1/c1 is returned..
        // ask = SplitPopulatedStars = vertices1 is populated, 2 is unpopulated stars

        public static void GetSystemVector<V>(int gridid, ref V[] vertices1, ref uint[] colours1,
                                                          ref V[] vertices2, ref uint[] colours2,
                                                          int percentage, Func<int, int, int, V> tovect,
                                                          SystemAskType ask = SystemAskType.SplitPopulatedStars)
        {
            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem(mode: SQLLiteExtensions.SQLExtConnection.AccessMode.Reader))
            {
                GetSystemVector<V>(gridid, ref vertices1, ref colours1, ref vertices2, ref colours2, percentage, tovect, cn, ask);
            }
        }

        public static void GetSystemVector<V>(int gridid, ref V[] vertices1, ref uint[] colours1,
                                                          ref V[] vertices2, ref uint[] colours2,
                                                          int percentage, Func<int, int, int, V> tovect,
                                                          SQLiteConnectionSystem cn,
                                                          SystemAskType ask = SystemAskType.SplitPopulatedStars)
        {
            int numvertices1 = 0;
            vertices1 = vertices2 = null;

            int numvertices2 = 0;
            colours1 = colours2 = null;

            Color[] fixedc = new Color[4];
            fixedc[0] = Color.Red;
            fixedc[1] = Color.Orange;
            fixedc[2] = Color.Yellow;
            fixedc[3] = Color.White;

            //System.Diagnostics.Debug.WriteLine("sysLap : " + BaseUtils.AppTicks.TickCountLap());

            using (DbCommand cmd = cn.CreateSelect("Systems s",
                                                    outparas: "s.edsmid,s.x,s.y,s.z" + (ask == SystemAskType.SplitPopulatedStars ? ",e.eddbid" : ""),
                                                    where: "s.sectorid IN (Select id FROM Sectors c WHERE c.gridid = @p1)" +
                                                            (percentage < 100 ? (" AND ((s.edsmid*2333)%100) <" + percentage.ToStringInvariant()) : "") +
                                                            (ask == SystemAskType.PopulatedStars ? " AND e.edsmid NOT NULL " : "") +
                                                            (ask == SystemAskType.UnpopulatedStars ? " AND e.edsmid IS NULL " : ""),
                                                    paras: new Object[] { gridid },
                                                    joinlist: ask != SystemAskType.AllStars ? new string[] { "LEFT OUTER JOIN EDDB e ON e.edsmid = s.edsmid " } : null
                                                    ))
            {
                //System.Diagnostics.Debug.WriteLine( cn.ExplainQueryPlanString(cmd));

                vertices1 = new V[250000];
                colours1 = new uint[250000];

                if (ask == SystemAskType.SplitPopulatedStars)
                {
                    vertices2 = new V[250000];
                    colours2 = new uint[250000];
                }
                
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    //System.Diagnostics.Debug.WriteLine("sysLapStart : " + BaseUtils.AppTicks.TickCountLap());

                    Object[] data = new Object[4];

                    while (reader.Read())
                    {
                        long id = reader.GetInt64(0);       // quicker than cast
                        int x = reader.GetInt32(1);
                        int y = reader.GetInt32(2);
                        int z = reader.GetInt32(3);

                        bool addtosecondary = (ask == SystemAskType.SplitPopulatedStars) ? (reader[4] is System.DBNull) : false;

                        Color basec = fixedc[(id) & 3];
                        int fade = 100 - (((int)id >> 2) & 7) * 8;
                        byte red = (byte)(basec.R * fade / 100);
                        byte green = (byte)(basec.G * fade / 100);
                        byte blue = (byte)(basec.B * fade / 100);

                        if (addtosecondary)
                        {
                            if (numvertices2 == vertices2.Length)
                            {
                                Array.Resize(ref vertices2, vertices2.Length *2);
                                Array.Resize(ref colours2, colours2.Length *2);
                            }

                            colours2[numvertices2] = BitConverter.ToUInt32(new byte[] { red, green, blue, 255 }, 0);
                            vertices2[numvertices2++] = tovect(x, y, z);
                        }
                        else
                        {
                            if (numvertices1 == vertices1.Length)
                            {
                                Array.Resize(ref vertices1, vertices1.Length *2);
                                Array.Resize(ref colours1, colours1.Length *2);
                            }

                            colours1[numvertices1] = BitConverter.ToUInt32(new byte[] { red, green, blue, 255 }, 0);
                            vertices1[numvertices1++] = tovect(x, y, z);
                        }
                    }

              //      System.Diagnostics.Debug.WriteLine("sysLapEnd : " + BaseUtils.AppTicks.TickCountLap());
                }

                Array.Resize(ref vertices1, numvertices1);
                Array.Resize(ref colours1, numvertices1);

                if (ask == SystemAskType.SplitPopulatedStars)
                {
                    Array.Resize(ref vertices2, numvertices2);
                    Array.Resize(ref colours2, numvertices2);
                }

                if (gridid == GridId.SolGrid && vertices1 != null)    // BODGE do here, better once on here than every star for every grid..
                {                       // replace when we have a better naming system
                    int solindex = Array.IndexOf(vertices1, tovect(0, 0, 0));
                    if (solindex >= 0)
                        colours1[solindex] = 0x00ffff;   //yellow
                }
            }
        }
    }
}


