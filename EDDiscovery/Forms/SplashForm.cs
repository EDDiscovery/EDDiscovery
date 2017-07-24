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

using System.Threading.Tasks;

namespace EDDiscovery.Forms
{
    public partial class SplashForm : Form
    {
        private Task inittask;
        private EDDiscoveryForm mainform;
        private ManualResetEvent readyForInvoke = new ManualResetEvent(false);
        public ApplicationContext Context;

        public SplashForm(EDDiscoveryForm mainform)
        {
            InitializeComponent();
            this.label_version.Text = "EDDiscovery " + System.Reflection.Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1];
            this.mainform = mainform;
            this.Owner = mainform;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            readyForInvoke.Set();
        }

        public void Init()      // called from Program once splash and eddiscoveryform have been made..
        {
            inittask = EDDiscoveryController.Initialize(Control.ModifierKeys.HasFlag(Keys.Shift)).ContinueWith(t => InitComplete(t));
        }

        private void InitComplete(Task t)       // tasks due to eddiscovery cotnroller Initialize complete.. now bring up eddiscoveryform
        {
            readyForInvoke.WaitOne();
            this.BeginInvoke(new Action(() =>
            {
                if (t.IsCompleted)
                {
                    try
                    {
                        mainform.Init();        // call the init function, which will initialize the eddiscovery form
                        mainform.Show();
                        Context.MainForm = mainform;
                    }
                    finally
                    {
                        this.Close();
                    }
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
