/*
 * Copyright © 2015 - 2018 EDDiscovery development team
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
using System.Threading.Tasks;

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
                check = EDDOptions.Instance.CheckRelease;

#if DEBUG
                // for debugging it
                //callbackinthread?.Invoke(new BaseUtils.GitHubRelease("Test","10.9.1.9","http", "2018-09-25T09:17:02Z", "Test","1.exe","2.msi","portable.zip"));
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
