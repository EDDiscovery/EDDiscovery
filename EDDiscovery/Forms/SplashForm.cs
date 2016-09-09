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
