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
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BaseUtils
{
    public static class SHA
    {
        static public string CalcSha1(string[] filenames)
        {
            long length = 0;

            foreach (string filename in filenames)
            {
                FileInfo fi = new FileInfo(filename);
                if (fi == null)
                    return "";

                length += fi.Length;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] header = Encoding.UTF8.GetBytes("blob " + (int)length + "\0");
                ms.Write(header, 0, header.Length);

                foreach (string filename in filenames)
                {
                    using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader br = new BinaryReader(fs))
                            ms.Write(br.ReadBytes((int)fs.Length), 0, (int)fs.Length);
                    }
                }

                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    byte[] hash = sha1.ComputeHash(ms.ToArray());
                    StringBuilder formatted = new StringBuilder(2 * hash.Length);
                    foreach (byte b in hash)
                    {
                        formatted.AppendFormat("{0:X2}", b);
                    }

                    return formatted.ToNullSafeString();
                }
            }
        }

        static public string CalcSha1(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            using (BinaryReader br = new BinaryReader(fs))
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] header = Encoding.UTF8.GetBytes("blob " + fs.Length + "\0");
                ms.Write(header, 0, header.Length);

                ms.Write(br.ReadBytes((int)fs.Length), 0, (int)fs.Length);

                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    byte[] hash = sha1.ComputeHash(ms.ToArray());
                    StringBuilder formatted = new StringBuilder(2 * hash.Length);
                    foreach (byte b in hash)
                    {
                        formatted.AppendFormat("{0:X2}", b);
                    }

                    return formatted.ToNullSafeString();
                }
            }
        }
    }
}
