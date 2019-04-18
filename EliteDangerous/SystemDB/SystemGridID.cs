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
using System.Collections.Generic;

namespace EliteDangerousCore.DB
{
    public class GridId
    {
        private const int GridSize = 1000;              // based on 1000x1000 min size

        public const int GridXRange = 20;
        static private int[] compresstablex = {
                                                0,1,1,1,1, 2,2,2,2,2,                   // 0   -20
                                                3,3,4,4,5, 5,6,7,8,9,                   // 10   -10,-8,-6,..
                                                10,11,12,13,14, 14,15,15,16,16,         // 20 centre
                                                17,17,17,17,17, 18,18,18,18,18,         // 30   +10
                                                19,19                                   // 40   +20
                                            };
        public const int GridZRange = 26;
        static private int[] compresstablez = {
                                                0,1,1,2,2,      3,4,5,6,7,              // 0  -10
                                                8,9,10,11,12,   12,13,13,14,14,         // 10 Sol 0
                                                15,15,15,15,15, 16,16,16,16,16,         // 20   +10
                                                17,17,17,17,17, 18,18,18,18,18,         // 30 centre +20
                                                19,19,19,19,19, 20,20,20,20,20,         // 40 +30
                                                21,21,21,21,21, 22,22,22,22,22,         // 50 +40    
                                                23,23,23,23,23, 24,24,24,24,24,         // 60 +50
                                                25,25                                   // 70 +60
                                            };
        public const int xleft = -20500;
        public const int xright = 20000;
        public const int zbot = -10500;
        public const int ztop = 60000;

        public const int MinGridID = 0;
        public const int MaxGridID = GridZRange * ZMult + GridXRange;

        public const int SolGrid = 810;

        private const int ZMult = 100;

        public static int Id(double x, double z)
        {
            x = Math.Min(Math.Max(x - xleft, 0), xright - xleft);       // 40500
            z = Math.Min(Math.Max(z - zbot, 0), ztop - zbot);           // 70500
            x /= (float)GridSize;                                       // 0-40.5 inc
            z /= (float)GridSize;                                       // 0-70.5 inc
            return compresstablex[(int)x] + ZMult * compresstablez[(int)z];
        }

        public static int Id(int x128, int z128)
        {
            return Id((double)x128 / 128.0, (double)z128 / 128.0);
        }

        public static List<int> Ids(double minx, double maxx, double minz, double maxz)
        {
            int botleft = Id(minx, minz);
            int topright = Id(maxx, maxz);
            List<int> ids = new List<int>();
            for (int x = botleft % ZMult; x <= topright % ZMult; x++)
            {
                for (int z = botleft / ZMult; z <= topright / ZMult; z++)
                    ids.Add(IdFromComponents(x, z));
            }
            return ids;
        }

        public static List<int> AllId()
        {
            List<int> list = new List<int>();

            for (int z = 0; z < GridZRange; z++)
            {
                for (int x = 0; x < GridXRange; x++)
                    list.Add(IdFromComponents(x, z));
            }
            return list;
        }

        public static int IdFromComponents(int x, int z)                // given x grid/ y grid give ID
        {
            return x + ZMult * z;
        }

        public static bool XZ(int id, out float x, out float z, bool mid = true)         // given id, return x/z pos of left bottom or mid
        {
            x = 0; z = 0;
            if (id >= 0)
            {
                int xid = (id % ZMult);
                int zid = (id / ZMult);

                if (xid < GridXRange && zid < GridZRange)
                {
                    for (int i = 0; i < compresstablex.Length; i++)
                    {
                        if (compresstablex[i] == xid)
                        {
                            double startx = i * GridSize + xleft;

                            while (i < compresstablex.Length && compresstablex[i] == xid)
                                i++;

                            x = (mid) ? (float)((((i * GridSize) + xleft) + startx) / 2.0) : (float)startx;
                            break;
                        }
                    }

                    for (int i = 0; i < compresstablez.Length; i++)
                    {
                        if (compresstablez[i] == zid)
                        {
                            double startz = i * GridSize + zbot;

                            while (i < compresstablez.Length && compresstablez[i] == zid)
                                i++;

                            z = (mid) ? (float)((((i * GridSize) + zbot) + startz) / 2.0) : (float)startz;
                            break;
                        }
                    }

                    return true;
                }
            }

            return false;
        }


        public static bool Boundaries(int id, out float left, out float bottom, out float right, out float top)         // given id, return boundaries
        {
            left = bottom = right = top = 0;

            if (id >= 0)
            {
                int xid = (id % ZMult);
                int zid = (id / ZMult);

                if (xid < GridXRange && zid < GridZRange)
                {
                    for (int i = 0; i < compresstablex.Length; i++)
                    {
                        if (compresstablex[i] == xid)
                        {
                            left = i * GridSize + xleft;

                            while (i < compresstablex.Length && compresstablex[i] == xid)
                                i++;

                            right = i * GridSize + xleft;
                            break;
                        }
                    }

                    for (int i = 0; i < compresstablez.Length; i++)
                    {
                        if (compresstablez[i] == zid)
                        {
                            bottom = i * GridSize + zbot;

                            while (i < compresstablez.Length && compresstablez[i] == zid)
                                i++;

                            top = i * GridSize + zbot;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        public static int[] XLines(int endentry)            // fill in the LY values, plus an end stop one
        {
            int[] xlines = new int[GridXRange + 1];

            for (int x = 0; x < GridXRange; x++)
            {
                float xp, zp;
                int id = GridId.IdFromComponents(x, 0);
                GridId.XZ(id, out xp, out zp, false);
                xlines[x] = (int)xp;
            }

            xlines[GridXRange] = endentry;

            return xlines;
        }

        public static int[] ZLines(int endentry)
        {
            int[] zlines = new int[GridZRange + 1];

            for (int z = 0; z < GridZRange; z++)
            {
                float xp, zp;
                int id = GridId.IdFromComponents(0, z);
                GridId.XZ(id, out xp, out zp, false);
                zlines[z] = (int)zp;
            }

            zlines[GridZRange] = endentry;

            return zlines;
        }

        public static string Grids(float minx, float maxx, float minz, float maxz)
        {
            int botleft = Id(minx, minz);
            int topright = Id(maxx, maxz);
            string grids = "";
            for (int x = botleft % ZMult; x <= topright % ZMult; x++)
            {
                for (int z = botleft / ZMult; z <= topright / ZMult; z++)
                    grids = grids + IdFromComponents(x, z) + ";";
            }
            return grids;
        }

        public static List<int> FromString(string s)
        {
            if (s == "All")
                return GridId.AllId();
            else
                return s.RestoreIntListFromString();
        }

        public static string ToString(List<int> ids)
        {
            List<int> allid = GridId.AllId();
            if (ids.Count == allid.Count)
                return "All";
            else
                return string.Join(",", ids);
        }
    }
}
