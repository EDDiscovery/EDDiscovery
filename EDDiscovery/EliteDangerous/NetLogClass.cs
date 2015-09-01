using EDDiscovery.DB;
using EDDiscovery2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace EDDiscovery
{
    public delegate void NetLogEventHandler(object source);

    public class NetLogFileInfo
    {
        public string FileName;
        public DateTime lastchanged;
        public long filePos, fileSize;

    }

    public class NetLogClass
    {
        public List<SystemPosition> visitedSystems = new List<SystemPosition>();
        Dictionary<string, NetLogFileInfo> netlogfiles = new Dictionary<string, NetLogFileInfo>();
        FileSystemWatcher m_Watcher;
        Thread ThreadNetLog;
        bool Exit = false;
        bool NoEvents = false;
        public event NetLogEventHandler OnNewPosition;

        SQLiteDBClass db=null;


        public string GetNetLogPath()
        {
            try
            {
                if (db == null)
                    db = new SQLiteDBClass();

                string netlogdirstored = db.GetSettingString("Netlogdir", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Frontier_Developments\\Products");
                string datapath = null;
                if (db.GetSettingBool("NetlogDirAutoMode", true))
                {
                    if (EliteDangerous.EDDirectory != null && EliteDangerous.EDDirectory.Length > 0)
                    {
                        datapath = Path.Combine(EliteDangerous.EDDirectory, "Logs");
                        if (!netlogdirstored.Equals(datapath))
                            db.PutSettingString("Netlogdir", datapath);
                        return datapath;
                    }

                    datapath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Frontier_Developments\\Products"; // \\FORC-FDEV-D-1001\\Logs\\";

                    // Find the right subdirectory....

                    DirectoryInfo dirInfo = new DirectoryInfo(datapath);
                    FileInfo[] allFiles = null;

                    try
                    {
                        allFiles = dirInfo.GetFiles("netLog*.log", SearchOption.AllDirectories);
                    }
                    catch
                    {
                    }

                    if (allFiles == null)
                    {
                        return null;
                    }

                    DateTime newtime = new DateTime(2000, 10, 10);
                    FileInfo newfi = null;


                    foreach (FileInfo fi in allFiles)
                    {
                        if (fi.CreationTimeUtc > newtime)
                        {
                            newtime = fi.CreationTimeUtc;
                            newfi = fi;
                        }
                    }

                    if (newfi != null)
                    {
                        db.PutSettingString("Netlogdir" , newfi.DirectoryName);
                        db.PutSettingBool("NetlogDirAutoMode" , false);
                        datapath = newfi.DirectoryName;
                    }



                }
                else
                {
                    datapath = db.GetSettingString("Netlogdir", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Frontier_Developments\\Products");
                }

                return datapath;
            }
            catch (Exception ex)
            {
                MessageBox.Show("GetNetLogPath exception: " + ex.Message);
                return null;
            }
        }



        public List<SystemPosition> ParseFiles(RichTextBox richTextBox_History)
        {
            string datapath;
            datapath = GetNetLogPath();

            if (datapath == null)
            {
                AppendText(richTextBox_History, "Netlog directory not found!" + Environment.NewLine + "Specify location in settings tab" + Environment.NewLine, Color.Red);
                return null;
            }
            DirectoryInfo dirInfo = new DirectoryInfo(datapath);

            if (!Directory.Exists(datapath))   // if logfiles directory is not found
            {
                if (richTextBox_History != null)
                {
                    richTextBox_History.Clear();
                    AppendText(richTextBox_History, "Netlog directory not found!" + Environment.NewLine + "Specify location in settings tab" + Environment.NewLine, Color.Red);
                    //MessageBox.Show("Netlog directory not found!" + Environment.NewLine + "Specify location in settings tab", "EDDiscovery Error", MessageBoxButtons.OK);
                }
                return null;
            }

            FileInfo[] allFiles = dirInfo.GetFiles("netLog.*.log", SearchOption.AllDirectories).OrderBy(p => p.Name).ToArray();

            NoEvents = true;

            foreach (FileInfo fi in allFiles)
            {
                ParseFile(fi, visitedSystems);
            }
            NoEvents = false;

            //var result = visitedSystems.OrderByDescending(a => a.time).ToList<SystemPosition>();

            return visitedSystems;
        }


        private int ParseFile(FileInfo fi, List<SystemPosition> visitedSystems)
        {

            int count = 0, nrsystems=visitedSystems.Count;
            try
            {
                using (Stream fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        count = ReadData(fi, visitedSystems, count, sr);
                    }
                }
            }
            catch
            {
                return 0;
            }

            if (nrsystems < visitedSystems.Count)
            {
                if (!NoEvents)
                    OnNewPosition(this);
            }
            return count;
        }

        private NetLogFileInfo lastnfi = null;
        private int ReadData(FileInfo fi, List<SystemPosition> visitedSystems, int count, StreamReader sr)
        {
            DateTime gammastart = new DateTime(2014, 11, 22, 13, 00, 00);

            DateTime filetime = DateTime.Now.AddDays(-500);
            string FirstLine = sr.ReadLine();
            string line, str;
            NetLogFileInfo nfi = null;

            str = "20" + FirstLine.Substring(0, 8) + " " + FirstLine.Substring(9, 5);

            filetime = DateTime.Parse(str);

            if (netlogfiles.ContainsKey(fi.FullName))
            {
                nfi = netlogfiles[fi.FullName];
                sr.BaseStream.Position = nfi.filePos;
                sr.DiscardBufferedData();
            }

            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains(" System:"))
                {
                    SystemPosition ps = SystemPosition.Parse(filetime, line);
                    if (ps != null)
                    {
                        if (ps.Name.Equals("Training"))
                            continue;
                        if (ps.Name.Equals("Destination"))
                            continue;

                        filetime = ps.time;

                        if (visitedSystems.Count > 0)
                            if (visitedSystems[visitedSystems.Count - 1].Name.Equals(ps.Name))
                                continue;

                        if (ps.time.Subtract(gammastart).TotalMinutes > 0)  // Ta bara med efter gamma. 
                        {


                            visitedSystems.Add(ps);
                            count++;

                            //System.Diagnostics.Trace.WriteLine("Added system: " + ps.Name);
                        }

                        //Console.WriteLine(line);
                    }
                }
            }


            if (nfi ==null)
                nfi = new NetLogFileInfo();

            nfi.FileName = fi.FullName;
            nfi.lastchanged = File.GetLastWriteTimeUtc(nfi.FileName);
            nfi.filePos = sr.BaseStream.Position;
            nfi.fileSize = fi.Length;

            netlogfiles[nfi.FileName] = nfi;
            lastnfi = nfi;

            return count;
        }


        private void AppendText(RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }

        public bool StartMonitor()
        {
            ThreadNetLog = new System.Threading.Thread(new System.Threading.ThreadStart(NetLogMain));
            ThreadNetLog.Name = "Net log";
            ThreadNetLog.Start();

            return true;
        }


        public void StopMonitor()
        {
            Exit = true;
        }


        private void NetLogMain()
        {
            try
            {
                m_Watcher = new System.IO.FileSystemWatcher();

                if (Directory.Exists(GetNetLogPath()))
                {
                    m_Watcher.Path = GetNetLogPath() + "\\";
                    m_Watcher.Filter = "netLog*.log";
                    m_Watcher.IncludeSubdirectories = true;
                    m_Watcher.NotifyFilter = NotifyFilters.FileName; // | NotifyFilters.Size; 

                    m_Watcher.Changed += new FileSystemEventHandler(OnChanged);
                    m_Watcher.Created += new FileSystemEventHandler(OnChanged);
                    m_Watcher.Deleted += new FileSystemEventHandler(OnChanged);
                    m_Watcher.EnableRaisingEvents = true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Net log watcher exception : " + ex.Message, "EDDiscovery Error");
                System.Diagnostics.Trace.WriteLine("NetlogMAin exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);

            }
            while (!Exit)
            {
                try
                {
                    Thread.Sleep(2000);

                    EliteDangerous.CheckED();

                    if (NoEvents == false)
                    {
                        if (lastnfi != null)
                        {
                            FileInfo fi = new FileInfo(lastnfi.FileName);

                            if (fi.Length != lastnfi.fileSize)
                                ParseFile(fi, visitedSystems);
                            else
                            {
                                //System.Diagnostics.Trace.WriteLine("No change");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("NetlogMAin exception : " + ex.Message);
                    System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                }

        }
        }


        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            string filename = e.FullPath;

            try
            {
                m_Watcher.EnableRaisingEvents = false;


                if (!netlogfiles.ContainsKey(filename))
                {
                    System.Diagnostics.Trace.WriteLine("NEW FILE !!!" + filename);
                }
                else
                {
                    System.Diagnostics.Trace.WriteLine("A CHANGE has occured with " + filename);
                }

                ParseFile(new FileInfo(filename), visitedSystems);

            }
            finally
            {
                m_Watcher.EnableRaisingEvents = true;

            }
        }


    }
}
