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

namespace EDDiscovery.Forms
{
    public partial class SplashForm : Form
    {
        private Thread _thread;

        public SplashForm()
        {
            _thread = Thread.CurrentThread;
            InitializeComponent();
            this.label_version.Text = "EDDiscovery " + System.Reflection.Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1];
        }

        [STAThread]
        private static void _ShowForm(object param)
        {
            BlockingCollection<SplashForm> queue = (BlockingCollection<SplashForm>)param;
            using (SplashForm splash = new SplashForm())
            {
                queue.Add(splash);
                Application.Run(splash);
            }
        }

        public void CloseForm()
        {
            if (!this.IsDisposed)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => this.CloseForm()));
                }
                else
                {
                    this.Close();
                    Application.ExitThread();
                }
            }
        }

        public static SplashForm ShowAsync()
        {
            // Mono doesn't support having forms running on multiple threads.
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                return null;
            }

            BlockingCollection<SplashForm> queue = new BlockingCollection<SplashForm>();
            Thread thread = new Thread(_ShowForm);
            thread.Start(queue);
            return queue.Take();
        }
    }
}
