using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUtils
{
    public static class HttpUriEncode
    {
        public static string URIGZipBase64Escape(string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);

            using (MemoryStream indata = new MemoryStream(bytes))
            {
                using (MemoryStream outdata = new MemoryStream())
                {
                    using (System.IO.Compression.GZipStream gzipStream = new System.IO.Compression.GZipStream(outdata, System.IO.Compression.CompressionLevel.Optimal, true))
                        indata.CopyTo(gzipStream);      // important to clean up gzip otherwise all the data is not written.. using

                    return Uri.EscapeDataString(Convert.ToBase64String(outdata.ToArray()));
                }
            }
        }
    }
}
