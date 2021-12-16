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

using System;
using System.Windows.Forms;

namespace EDDiscovery.UserControls.Webbrowser
{
    public abstract class BrowserBase
    {
        public string urlallowed { get; set; }
        public string userurllist { get; set; }

        private string defaultallowed =
                "about:" + Environment.NewLine +
                "https://www.google.com/recaptcha" + Environment.NewLine +
                "https://consentcdn.cookiebot.com" + Environment.NewLine +
                "https://auth.frontierstore.net" + Environment.NewLine +
                "https://googleads.g.doubleclick.net" + Environment.NewLine;


        public Control webbrowser { get; set; }
        public abstract void Navigate(string uri);
        public abstract void GoBack();

        public bool IsDisallowed(string absuri)
        {
            string[] userlistsplit = userurllist.HasChars() ? userurllist.Split(Environment.NewLine) : new string[0];
            bool all = Array.IndexOf(userlistsplit, "*") >= 0;
            string[] defaultlistsplit = defaultallowed.Split(Environment.NewLine);

            // if not in these
            if (!all &&
                  !absuri.StartsWith(urlallowed, StringComparison.InvariantCultureIgnoreCase) &&
                  userlistsplit.StartsWith(absuri, StringComparison.InvariantCultureIgnoreCase) == -1 &&
                  defaultlistsplit.StartsWith(absuri, StringComparison.InvariantCultureIgnoreCase) == -1
                  )
            {
                System.Diagnostics.Debug.WriteLine($"Webbrowser Disallowed {absuri}");
                return true;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Webbrowser Allowed {absuri}");
                return false;
            }
        }

    }
}
