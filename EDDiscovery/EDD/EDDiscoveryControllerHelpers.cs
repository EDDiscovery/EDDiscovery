/*
 * Copyright © 2015 - 2024 EDDiscovery development team
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
 */

using EliteDangerousCore.EDSM;
using System;
using EliteDangerousCore.DB;
using System.IO;
using BaseUtils;
using System.Threading.Tasks;
using EliteDangerousCore.GEC;

namespace EDDiscovery
{
    public partial class EDDiscoveryController
    {
        #region Aux file downloads

        // in its own thread..
        public void DownloadExpeditions(System.Threading.CancellationToken cancel)
        {
            LogLine("Checking for new Expedition data".Tx());

            Task.Factory.StartNew(() =>
            {
                GitHubClass github = new GitHubClass(EDDiscovery.Properties.Resources.URLGithubDataDownload);

                var files = github.ReadFolder(cancel, "Expeditions");

                if (files != null)        // may be empty, unlikely, but
                {
                    string expeditiondir = EDDOptions.Instance.ExpeditionsAppDirectory();

                    if (github.DownloadFiles(cancel,expeditiondir, files, true, true))
                    {
                        if (!cancel.IsCancellationRequested)
                        {
                            bool changed = SavedRouteClass.UpdateDBFromExpeditionFiles(expeditiondir);
                            InvokeAsyncOnUiThread(() => { OnExpeditionsDownloaded?.Invoke(changed); });
                        }
                    }
                }
            });
        }

        public void DownloadHelp(System.Threading.CancellationToken cancel)
        {
            Task.Factory.StartNew(() =>
            {
                string helpdir = EDDOptions.Instance.HelpDirectory();

                GitHubClass github = new GitHubClass(EDDiscovery.Properties.Resources.URLGithubDataDownload);

                var files = github.ReadFolder(cancel, "Help");
                if (files != null)        // may be empty, unlikely, but
                {
                    if (github.DownloadFiles(cancel, helpdir, files, true, true))
                    {
                        if (!cancel.IsCancellationRequested)
                        {
                            InvokeAsyncOnUiThread(() => { OnHelpDownloaded?.Invoke(); });
                        }
                    }
                }
            });
        }

        public void DownloadEDSMGEC(System.Threading.CancellationToken cancel)
        {
            Task.Factory.StartNew(() =>
            {
                // if we have a edsm gmo file, but its out of date, refresh it for next time, do it in background thread since not critical.
                string gmofile = Path.Combine(EDDOptions.Instance.AppDataDirectory, "galacticmapping.json");

                if (File.Exists(gmofile) && DateTime.UtcNow.Subtract(SystemsDatabase.Instance.GetEDSMGalMapLast()).TotalDays >= 7)
                {
                    LogLine("Get galactic mapping from EDSM.".Tx());
                    System.Diagnostics.Trace.WriteLine($"Download EDSM file background");
                    if (EDSMClass.DownloadGMOFileFromEDSM(gmofile,cancel))
                        SystemsDatabase.Instance.SetEDSMGalMapLast(DateTime.UtcNow);
                    System.Diagnostics.Trace.WriteLine($"Download GEC file background");

                }

                // if we have a gec file, but its out of date, refresh it for next time, do it in background thread since not critical.
                string gecfile = Path.Combine(EDDOptions.Instance.AppDataDirectory, "gecmapping.json");

                if ( !cancel.IsCancellationRequested && File.Exists(gecfile) && DateTime.UtcNow.Subtract(SystemsDatabase.Instance.GetGECGalMapLast()).TotalDays >= 7)
                {
                    LogLine("Get galactic mapping from GEC.".Tx());
                    System.Diagnostics.Trace.WriteLine($"Download GEC file background");
                    if (GECClass.DownloadGECFile(gecfile, cancel))
                        SystemsDatabase.Instance.SetGECGalMapLast(DateTime.UtcNow);
                    System.Diagnostics.Trace.WriteLine($"Download GEC file end");
                }
            });
        }


        #endregion

    }
}

