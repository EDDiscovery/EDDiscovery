/*
 * Copyright © 2021 - 2022 EDDiscovery development team
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

using System.ComponentModel;
using System.Windows.Forms;

namespace EDDiscovery.UserControls.Webbrowser
{
    public class BrowserIE11 : BrowserBase
    {
        private WebBrowser wbie11;
        public BrowserIE11()
        {
            webbrowser = wbie11 = new WebBrowser();
            wbie11.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            wbie11.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser_Navigating);
            wbie11.NewWindow += new System.ComponentModel.CancelEventHandler(this.webBrowser_NewWindow);
            webbrowser.Visible = false; // hide ugly white until load
            wbie11.ScriptErrorsSuppressed = true;
        }
        public override void Navigate(string uri)
        {
            wbie11.Navigate(uri);
        }
        public override void GoBack()
        {
            wbie11.GoBack();
        }

        public override void SetDocumentText(string text)
        {
            wbie11.DocumentText = text;
        }
        public override void Stop()
        {
            wbie11.Stop();
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            wbie11.Visible = true;
        }
        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            e.Cancel = IsDisallowed(e.Url.AbsoluteUri);
        }

        private void webBrowser_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }


}
