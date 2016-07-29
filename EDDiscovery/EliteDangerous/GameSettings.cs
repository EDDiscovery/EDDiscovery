using EDDiscovery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace EDDiscovery2
{
    //GameSettings class interfaces with the actual Game configuration files.
    //Note only needed functions and properties are loaded.

    public class GameSettings
    {
        public AppConfig AppConfig;

        private DateTime lastTry_Displaydata = DateTime.Now - new TimeSpan(1, 0, 0);

        public EDDiscoveryForm _parent;

        public GameSettings(EDDiscoveryForm parent)
        {
            _parent = parent;

            //Load AppConfig
            LoadAppConfig();

            //Set up some filewatchers, If user changes config its reflected here
            WatcherAppDataSettings(); //Currently disabled as we only check Verbose logging and that cant be changed from the game

            //Check and Request for Verbose Logging
            CheckAndRequestVerboseLogging();
        }

        void CheckAndRequestVerboseLogging()
        {
            if (AppConfig.Network.VerboseLogging != 1)
            {
                var setLog =
                    MessageBox.Show(
                        "Verbose logging isn't set in your Elite Dangerous AppConfig.xml, so I can't read system names. Would you like me to set it for you?",
                        "Set verbose logging?", MessageBoxButtons.YesNo);

                if (setLog == DialogResult.Yes)
                {
                    var appconfig = Path.Combine(EliteDangerousClass.EDDirectory, "AppConfig.xml");

                    //Make backup
                    File.Copy(appconfig, appconfig + ".bak", true);

                    //Set werbose to one
                    var doc = new XmlDocument();
                    doc.Load(appconfig);
                    var ie = doc.SelectNodes("/AppConfig/Network").GetEnumerator();

                    while (ie.MoveNext())
                    {
                        if ((ie.Current as XmlNode).Attributes["VerboseLogging"] != null)
                        {
                            (ie.Current as XmlNode).Attributes["VerboseLogging"].Value = "1";
                        }
                        else
                        {
                            var verb = doc.CreateAttribute("VerboseLogging");
                            verb.Value = "1";

                            (ie.Current as XmlNode).Attributes.Append(verb);
                        }
                    }

                    doc.Save(appconfig);

                    MessageBox.Show(
                        "AppConfig.xml updated.  You'll need to restart Elite Dangerous if it's already running.");
                }

                //Update config
                LoadAppConfig();
            }
        }

        void LoadAppConfig()
        {
            AppConfig locAppConfig;

            DialogResult MBResult = DialogResult.Ignore;
            string configFile = Path.Combine(EliteDangerousClass.EDDirectory, "AppConfig.xml");
            XmlSerializer serializer;

            do
            {

                try
                {
                    serializer = new XmlSerializer(typeof(AppConfig));
                    using (var myFileStream = new FileStream(configFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        locAppConfig = (AppConfig)serializer.Deserialize(myFileStream);
                        AppConfig = locAppConfig;
                    }
                }
                catch 
                {

                    if (AppConfig == null)
                    {
                        // ignore if it was loaded before
                        //cErr.processError(ex, String.Format("Error while loading ED-Appconfig from file <{0}>", configFile));
                    }

                }
            } while (MBResult == DialogResult.Retry);

        }

        private void AppData_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                LoadAppConfig();
            }
            catch 
            {
                //cErr.processError(ex, "Error in AppData_Changed()");
            }
        }

  

        private readonly FileSystemWatcher _displayWatcher = new FileSystemWatcher();
  

        private readonly FileSystemWatcher _appdataWatcher = new FileSystemWatcher();
        void WatcherAppDataSettings()
        {
            _appdataWatcher.Path = EliteDangerousClass.EDDirectory;
            _appdataWatcher.Filter = "AppConfig.xml";
            _appdataWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _appdataWatcher.Changed += AppData_Changed;
            _appdataWatcher.EnableRaisingEvents = false; //Set to TRUE to enable watching!
        }

    }
}
