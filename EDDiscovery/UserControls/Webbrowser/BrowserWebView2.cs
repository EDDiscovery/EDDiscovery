/*
 * Copyright © 2021 - 2021 EDDiscovery development team
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

using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.UserControls.Webbrowser
{
    public class BrowserWebView2 : BrowserBase
    {
        public Action<bool> LoadResult;

        private WebView2 wv2;

        public BrowserWebView2()
        {
        }

        // we need to register the LoadResult before we start. If webview2 is not there, we get an immeidate initialisation complete with failed
        // if its there, it takes a while, and needs winforms to run, and then it sends a complete with success

        public void Start()
        {
            webbrowser = wv2 = new WebView2();
            wv2.CoreWebView2InitializationCompleted += Wv2_CoreWebView2InitializationCompleted;
            wv2.NavigationCompleted += Wv2_NavigationCompleted;
            wv2.NavigationStarting += Wv2_NavigationStarting;
            CreateEnvironment(wv2).ConfigureAwait(false);
            wv2.Visible = false; // hide ugly white until load
           
        }

        private async Task CreateEnvironment(WebView2 wv2)
        {
            var env = await CoreWebView2Environment.CreateAsync(userDataFolder: EDDOptions.Instance.WebView2ProfileDirectory());
            await wv2.EnsureCoreWebView2Async(env);
        }

        private void Wv2_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess)
            {
                System.Diagnostics.Trace.WriteLine($"Webview 2 exception {e.InitializationException} {e.InitializationException.StackTrace}");
            }

            LoadResult(e.IsSuccess);

            if ( e.IsSuccess )
            {
                wv2.CoreWebView2.Settings.IsPasswordAutosaveEnabled = true;     // enable password saving
                wv2.CoreWebView2.FrameNavigationStarting += CoreWebView2_FrameNavigationStarting;
            }
        }

        private void CoreWebView2_FrameNavigationStarting(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine($"FVS {e.Uri}");
            e.Cancel = IsDisallowed(e.Uri);
        }

        private void Wv2_NavigationStarting(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            e.Cancel = IsDisallowed(e.Uri);
        }

        private void Wv2_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            wv2.Visible = true;
        }

        public override void Navigate(string uri)
        {
            if ( wv2.CoreWebView2 != null )
                wv2.CoreWebView2.Navigate(uri);
        }
        public override void GoBack()
        {
            wv2.GoBack();
        }
    }


}
