using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousCore
{
    public static class VisitingStarsCacheFolder
    {
        public static string GetVisitedStarsCacheDirectory()    // null if can't find it.
        {
            string[] allFiles;
            try
            {
                string EDimportstarsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Frontier Developments", "Elite Dangerous");
                allFiles = Directory.GetFiles(EDimportstarsDir, "VisitedStarsCache.dat", SearchOption.AllDirectories);
            }
            catch (IOException)
            {
                return null;
            }

            if (allFiles.Count<string>() == 0)
            {
                return null;
            }

            String folder = null;
            if (allFiles.Count<string>() > 1)  // signle account  just export
            {
                DirectoryInfo newesetDi = null;
                for (int ii = 0; ii < allFiles.Count<string>(); ii++)
                {
                    DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(allFiles[ii]));

                    if (newesetDi == null)
                        newesetDi = di;

                    if (di.LastWriteTimeUtc > newesetDi.LastWriteTimeUtc)
                        newesetDi = di;
                }

                folder = newesetDi.FullName;
            }
            else
            {
                folder = Path.GetDirectoryName(allFiles[0]);
            }

            if (string.IsNullOrWhiteSpace(folder))
                return null;

            return folder;
        }
    }
}
