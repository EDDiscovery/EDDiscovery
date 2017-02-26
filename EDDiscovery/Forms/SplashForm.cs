/*
 * Copyright © 2016 EDDiscovery development team
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Concurrent;
using EDDiscovery2;
using System.Threading.Tasks;

namespace EDDiscovery.Forms
{
    public partial class SplashForm : Form
    {
        private Task inittask;
        private EDDiscoveryForm mainform;

        public SplashForm()
        {
            InitializeComponent();
            this.label_version.Text = "EDDiscovery " + System.Reflection.Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1];
            inittask = EDDiscoveryController.Initialize(Control.ModifierKeys.HasFlag(Keys.Shift)).ContinueWith(t => InitComplete(t));
        }

        private void InitComplete(Task t)
        {
            this.BeginInvoke(new Action(() =>
            {
                if (t.IsCompleted)
                {
                    mainform = new EDDiscoveryForm(this);
                    mainform.Show();
                }
                else if (t.IsFaulted)
                {
                    MessageBox.Show($"Error initializing database:\n{t.Exception.InnerExceptions.FirstOrDefault()?.ToString()}", "Error initializing database");
                    Application.Exit();
                }
            }));
        }
    }
}
