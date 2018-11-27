using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EDDiscovery
{
    static class Installer
    {
        static public BaseUtils.GitHubRelease CheckForNewinstaller()
        {
            try
            {
                BaseUtils.GitHubClass github = new BaseUtils.GitHubClass(EDDiscovery.Properties.Resources.URLGithubDownload);

                BaseUtils.GitHubRelease rel = github.GetLatestRelease();

                if (rel != null)
                {
                    var currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetVersionString();
                    var releaseVersion = rel.ReleaseVersion;

                    Version v1 = new Version(releaseVersion);
                    Version v2 = new Version(currentVersion);

                    if (v1.CompareTo(v2) > 0) // Test if newer installer exists:
                    {
                        return rel;
                    }
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        static public Task CheckForNewInstallerAsync(Action<BaseUtils.GitHubRelease> callbackinthread)
        {
            return Task.Factory.StartNew(() =>
            {
                bool check = true;
#if DEBUG
                check = EDDOptions.Instance.CheckGithubFilesInDebug;
#endif 
                if (check)
                {
                    BaseUtils.GitHubRelease rel = CheckForNewinstaller();

                    if (rel != null)
                        callbackinthread?.Invoke(rel);
                }
            });
        }
    }
}
