/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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

namespace BaseUtils
{
    public class LineStore
    {
        public int[] Items;     // 0 is blank
        public int YStart;
        public int YEnd;

        public bool Blank()
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] > 0)
                    return false;
            }
            return true;
        }

        public static List<LineStore> Restore(string s, int width)
        {
            List<LineStore> ls = new List<LineStore>();
            string[] parray = s.Split(',');
            for (int i = 0; i < parray.Length;)
            {
                int[] line = new int[width];
                for (int w = 0; w < width; w++)
                {
                    int v = 0;
                    if (i < parray.Length)
                        parray[i++].InvariantParse(out v);
                    line[w] = v;
                }

                ls.Add(new LineStore() { Items = line });
            }
            //DumpOrder(ls, "Restore");
            return ls;
        }

        public static string ToString(List<LineStore> lines)
        {
            string s = "";
            foreach (LineStore l in lines)
            {
                string v = string.Join(",", l.Items);
                s = s.AppendPrePad(v,",");
            }
            return s;
        }

        public static int FindRow(List<LineStore> lines, int y)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (i == 0 && y < lines[i].YStart)
                    return i;

                if (y >= lines[i].YStart && y <= lines[i].YEnd)
                    return i;
            }

            return -1;
        }

        public static LineStore FindValue(List<LineStore> lines, int v)
        {
            foreach( LineStore ls in lines)
            {
                if (Array.IndexOf(ls.Items, v) >= 0)
                    return ls;
            }
            return null;
        }

        public static void CompressOrder(List<LineStore> lines)
        {
            for (int i = 0; i < lines.Count; )
            {
                if (lines[i].Blank())
                    lines.RemoveAt(i);
                else
                    i++;
            }
        }

        public static void DumpOrder(List<LineStore> lines, string s)
        {
            System.Diagnostics.Debug.WriteLine("--- " + s);
            for (int i = 0; i < lines.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine(string.Join(",", lines[i].Items));
            }
        }
    }
}
