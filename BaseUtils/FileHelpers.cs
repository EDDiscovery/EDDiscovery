using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUtils
{
    public static class FileHelpers
    {
        public static string TryReadAllTextFromFile(string filename)
        {
            try
            {
                return File.ReadAllText(filename, Encoding.UTF8);
            }
            catch
            {
                return null;
            }
        }
    }
}
