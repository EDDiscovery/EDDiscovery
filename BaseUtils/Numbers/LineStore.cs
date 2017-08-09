using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUtils
{
    public class LineStore
    {
        public int[] items;     // 0 is blank
        public int ystart;
        public int yend;

        public bool Blank()
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] > 0)
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

                ls.Add(new LineStore() { items = line });
            }
            //DumpOrder(ls, "Restore");
            return ls;
        }

        public static string ToString(List<LineStore> lines)
        {
            string s = "";
            foreach (LineStore l in lines)
            {
                string v = string.Join(",", l.items);
                s = s.AppendPrePad(v,",");
            }
            return s;
        }

        public static int FindRow(List<LineStore> lines, int y)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (i == 0 && y < lines[i].ystart)
                    return i;

                if (y >= lines[i].ystart && y <= lines[i].yend)
                    return i;
            }

            return -1;
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
                System.Diagnostics.Debug.WriteLine(string.Join(",", lines[i].items));
            }
        }
    }
}
