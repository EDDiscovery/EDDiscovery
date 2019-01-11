/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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

namespace EDDiscovery.Forms
{
    public partial class SplashForm : Form
    {
        public SplashForm()
        {
            InitializeComponent();
            this.label_version.Text = EDDApplicationContext.FriendlyName + " " + EDDApplicationContext.AppVersion;
        }

        public void SetLoadingText(string value)
        {
            if (IsDisposed || label1 == null || label1.Text == value)
                return;

            if (!InvokeRequired)
            {
                label1.Text = value ?? string.Empty;
                label1.Update();    // Force the immediate paint, since this thread is quite busy.
            }
            else
                Invoke(new Action(() => SetLoadingText(value)));
        }
    }
}
