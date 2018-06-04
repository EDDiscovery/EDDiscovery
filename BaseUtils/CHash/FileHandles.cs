using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUtils
{
    public class FileHandle
    {
        StreamReader reader;
        StreamWriter writer;

        public bool Open(string f, FileMode fm, FileAccess ac, out string errmsg)
        {
            try
            {
                FileStream file = File.Open(f, fm, ac);

                if (ac == FileAccess.Read)
                    reader = new StreamReader(file);
                else
                    writer = new StreamWriter(file);

                errmsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
        }

        public void Close()
        {
            try
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
            }
            catch { }
        }

        public bool ReadLine(out string output)
        {
            try
            {
                if (reader != null)
                {
                    output = reader.ReadLine();
                    return true;
                }
                else
                {
                    output = "Not open for read";
                    return false;
                }
            }
            catch
            {
                output = "Failed to read file";
                return false;
            }
        }

        public bool WriteLine(string value, bool lf, out string errmsg)
        {
            try
            {
                if (writer != null)
                {
                    if (lf)
                        writer.WriteLine(value);
                    else
                        writer.Write(value);

                    errmsg = "";
                    return true;
                }
                else
                {
                    errmsg = "Not open for write";
                    return false;
                }
            }
            catch
            {
                errmsg = "Failed to read file";
                return false;
            }
        }

        public bool Seek(long value, out string errmsg)
        {
            try
            {
                if (writer != null)
                    writer.BaseStream.Seek(value, SeekOrigin.Begin);
                else
                {
                    reader.DiscardBufferedData();
                    reader.BaseStream.Seek(value, SeekOrigin.Begin);
                }
                errmsg = "";
                return true;
            }
            catch
            {
                errmsg = "Failed to read file";
                return false;
            }
        }

        public bool Tell(long value, out string output)
        {
            try
            {
                if (writer != null)
                    output = writer.BaseStream.Position.ToString(System.Globalization.CultureInfo.InvariantCulture);
                else
                    output = reader.BaseStream.Position.ToString(System.Globalization.CultureInfo.InvariantCulture);

                return true;
            }
            catch
            {
                output = "Failed to tell file position";
                return false;
            }
        }
    }

    public class FileHandles
    {
        private Dictionary<int, FileHandle> handles;
        private int nexthid;

        public FileHandles()
        {
            nexthid = 1;
            handles = new Dictionary<int, FileHandle>();
        }

        public void CloseAll()
        {
            foreach (int i in handles.Keys)
            {
                handles[i].Close();
            }

            handles.Clear();
        }

        public int Open(string f, FileMode fm, FileAccess ac, out string errmsg)        // 0 is bad!
        {
            FileHandle cfh = new FileHandle();
            if (cfh.Open(f, fm, ac, out errmsg))
            {
                handles[nexthid] = cfh;
                return nexthid++;
            }
            else
                return 0;
        }

        public void Close(int h)
        {
            if (handles.ContainsKey(h))
            {
                handles[h].Close();
                handles.Remove(h);
            }
        }

        public bool ReadLine(int h, out string output)
        {
            if (handles.ContainsKey(h))
                return handles[h].ReadLine(out output);
            else
            {
                output = "Handle not found";
                return false;
            }
        }

        public bool WriteLine(int h, string value, bool lf, out string output)
        {
            if (handles.ContainsKey(h))
                return handles[h].WriteLine(value, lf, out output);
            else
            {
                output = "Handle not found";
                return false;
            }
        }

        public bool Seek(int h, long value, out string output)
        {
            if (handles.ContainsKey(h))
                return handles[h].Seek(value, out output);
            else
            {
                output = "Handle not found";
                return false;
            }
        }

        public bool Tell(int h, out string output)
        {
            if (handles.ContainsKey(h))
                return handles[h].Tell(h, out output);
            else
            {
                output = "Handle not found";
                return false;
            }
        }
    }
}
