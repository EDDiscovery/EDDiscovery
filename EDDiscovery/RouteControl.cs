using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using EMK.Cartography;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using EDDiscovery.DB;
using System.Threading;
using EMK.LightGeometry;

namespace EDDiscovery
{
    public partial class RouteControl : UserControl
    {
        Graph G;
        internal TravelHistoryControl travelhistorycontrol1;
        internal bool changesilence = false;
        public RouteControl()
        {
            InitializeComponent();
            button_Route.Enabled = false;
        }

        private Thread ThreadRoute;

        private void button_Route_Click_1(object sender, EventArgs e)
        {
            button_Route.Enabled = false;
            richTextBox1.Clear();

            ThreadRoute = new System.Threading.Thread(new System.Threading.ThreadStart(RouteMain));
            ThreadRoute.Name = "Thread Route";
            ThreadRoute.Start();
        }

        private void RouteMain()
        {
            float maxrange = float.Parse(textBox_Range.Text);
            bool usingcoordsfrom = textBox_From.ReadOnly == true;
            bool usingcoordsto = textBox_To.ReadOnly == true;
            Point3D coordsfrom, coordsto;
            GetCoordsFrom(out coordsfrom);                      // will be valid for a system or a co-ords box
            GetCoordsTo(out coordsto);
            string fromsys = textBox_From.Text;
            string tosys = textBox_To.Text;
            if (usingcoordsfrom)
                fromsys = "START POINT";
            if (usingcoordsto)
                tosys = "END POINT";

            RouteIterative(fromsys, usingcoordsfrom, coordsfrom,            // use Route if you want to try the previous version..
                  tosys, usingcoordsto, coordsto,
                  maxrange);

            this.Invoke(new Action(() => button_Route.Enabled = true));
        }


        private void RouteIterative(string fromsys, bool usingcoordsfrom, Point3D coordsfrom,
                           string tosys, bool usingcoordsto, Point3D coordsto,
                           float maxrange)
        {
            double traveldistance = Point3D.DistanceBetween(coordsfrom, coordsto);      // its based on a percentage of the traveldistance

            double possiblejumps = traveldistance / maxrange;

            if ( possiblejumps > 100)
            {
                DialogResult res = MessageBox.Show("This will result in a large number (" + possiblejumps.ToString("0") + ") of jumps" + Environment.NewLine + "Confirm please", "Confirm you want to compute", MessageBoxButtons.YesNo);
                if (res != System.Windows.Forms.DialogResult.Yes)
                    return;
            }

            AppendText("Searching route from " + fromsys + " to " + tosys + Environment.NewLine);
            AppendText("Total distance: " + traveldistance.ToString("0.00") + " in " + maxrange.ToString("0.00") + "ly jumps" + Environment.NewLine);

            AppendText(Environment.NewLine + string.Format("{0,-30}    Depart          Co-Ords:{1,9:0.00},{2,8:0.00},{3,9:0.00}" + Environment.NewLine, fromsys, coordsfrom.X, coordsfrom.Y, coordsfrom.Z));

            Point3D curpos = coordsfrom;
            int jump = 1;
            double actualdistance = 0;

            do
            {
                double distancetogo = Point3D.DistanceBetween(coordsto, curpos);      // to go

                if (distancetogo <= maxrange)                                         // within distance, we can go directly
                    break;

                Point3D travelvector = new Point3D(coordsto.X - curpos.X, coordsto.Y - curpos.Y, coordsto.Z - curpos.Z); // vector to destination
                Point3D travelvectorperly = new Point3D(travelvector.X / distancetogo, travelvector.Y / distancetogo, travelvector.Z / distancetogo); // per ly travel vector

                Point3D nextpos = new Point3D(curpos.X + maxrange * travelvectorperly.X,
                                              curpos.Y + maxrange * travelvectorperly.Y,
                                              curpos.Z + maxrange * travelvectorperly.Z);   // where we would like to be..

#if DEBUG
                Console.WriteLine("Curpos " + curpos.X + "," + curpos.Y + "," + curpos.Z);
                Console.WriteLine(" next" + nextpos.X + "," + nextpos.Y + "," + nextpos.Z);
#endif
                SystemClass bestsystem;
                Point3D bestposition;

                FindBestSystem(curpos, nextpos, maxrange, maxrange-0.5, out bestsystem, out bestposition);
                string sysname = "WAYPOINT";
                double deltafromwaypoint = 0;

                if (bestsystem != null)
                {
                    deltafromwaypoint = Point3D.DistanceBetween(bestposition, nextpos);     // how much in error
                    nextpos = bestposition;
                    sysname = bestsystem.name;
                }

                AppendText(string.Format("{0,-30}{1,3} Dist:{2,8:0.00}ly Co-Ords:{3,9:0.00},{4,8:0.00},{5,9:0.00} Deviation:{6,8:0.00}ly" + Environment.NewLine,
                            sysname, jump , Point3D.DistanceBetween(curpos, nextpos), nextpos.X, nextpos.Y, nextpos.Z,deltafromwaypoint));

                actualdistance += Point3D.DistanceBetween(curpos, nextpos);
                curpos = nextpos;
                jump++;

            } while (true);

            actualdistance += Point3D.DistanceBetween(curpos, coordsto);
            AppendText(string.Format("{0,-30}{1,3} Dist:{2,8:0.00}ly Co-Ords:{3,9:0.00},{4,8:0.00},{5,9:0.00}" + Environment.NewLine, tosys, jump, Point3D.DistanceBetween(curpos, coordsto), coordsto.X, coordsto.Y, coordsto.Z));
            AppendText(string.Format(Environment.NewLine + "Straight Line Distance {0,8:0.00}ly vs Travelled Distance {1,8:0.00}ly" + Environment.NewLine, traveldistance , actualdistance));
        }

        private void FindBestSystem(Point3D curpos, Point3D wantedpos, double maxfromcurpos , double maxfromwanted, 
                                    out SystemClass system, out Point3D position )
        {
            List<SystemClass> systems = SystemData.SystemList;

            double maxfromcurposx2 = maxfromcurpos * maxfromcurpos;
            double maxfromwantedx2 = maxfromwanted * maxfromwanted;

            SystemClass nearestsystem = null;
            double bestmindistancex2 = 1E39;

            for (int ii = 0; ii < systems.Count; ii++)
            {
                SystemClass syscheck = systems[ii];
                Point3D syspos = new Point3D(syscheck.x, syscheck.y, syscheck.z);
                double distancefromwantedx2 = Point3D.DistanceBetweenX2(wantedpos, syspos); // range between the wanted point and this, ^2
                double distancefromcurposx2 = Point3D.DistanceBetweenX2(curpos, syspos);    // range between the wanted point and this, ^2

                if ( distancefromwantedx2 <= maxfromwantedx2 &&         // if within the radius of wanted
                     distancefromcurposx2 <= maxfromcurposx2 )          // and within the jump range of current
                {
                    if ( distancefromwantedx2 < bestmindistancex2 )
                    {
                        nearestsystem = syscheck;
                        bestmindistancex2 = distancefromwantedx2;
                    }
                }
            }

            system = nearestsystem;
            position = null;
            if ( system != null )
                position = new Point3D(system.x, system.y, system.z);
        }







        private void Route(string fromsys, bool usingcoordsfrom, Point3D coordsfrom,
                           string tosys, bool usingcoordsto, Point3D coordsto,
                           float maxrange)
        {
            double xwindow = Math.Abs(coordsfrom.X - coordsto.X);                       // we need a window of co-ords
            double ywindow = Math.Abs(coordsfrom.Y - coordsto.Y);                       // to pick up systems. consider from 0,0,0 to 0,1000,1000
            double zwindow = Math.Abs(coordsfrom.Z - coordsto.Z);                       // x has no window.
            double traveldistance = Point3D.DistanceBetween(coordsfrom, coordsto);      // its based on a percentage of the traveldistance
            double wanderpercentage = 0.1;
            double wanderwindow = traveldistance * wanderpercentage;                    // this is the minimum window size

            if (wanderwindow > 100)                                                     // limit, otherwise we just get too many
                wanderwindow = 100;

            xwindow = (xwindow < wanderwindow) ? (wanderwindow / 2) : 10;               // if less than the wander window, open it up, else open it up a little so it can
            ywindow = (ywindow < wanderwindow) ? (wanderwindow / 2) : 10;               // find start/end points without any rounding errors..
            zwindow = (zwindow < wanderwindow) ? (wanderwindow / 2) : 10;

            Point3D minpos = new EMK.LightGeometry.Point3D(
                    Math.Min(coordsfrom.X, coordsto.X) - xwindow,
                    Math.Min(coordsfrom.Y, coordsto.Y) - ywindow,
                    Math.Min(coordsfrom.Z, coordsto.Z) - zwindow );
            Point3D maxpos = new EMK.LightGeometry.Point3D(
                    Math.Max(coordsfrom.X, coordsto.X) + xwindow,
                    Math.Max(coordsfrom.Y, coordsto.Y)+ ywindow,
                    Math.Max(coordsfrom.Z, coordsto.Z) + zwindow );

            AppendText("Bounding Box " + minpos.X.ToString("0.0") + "," + minpos.Y.ToString("0.0") + "," + minpos.Z.ToString("0.0") + " to " + maxpos.X.ToString("0.0") + "," + maxpos.Y.ToString("0.0") + "," + maxpos.Z.ToString("0.0") + " window " + wanderwindow.ToString("0.0") + Environment.NewLine);

            Stopwatch sw = new Stopwatch();

            G = new Graph();                    // need to compute each time as systems in range changes each time
                                                
            AddStarNodes(G, minpos, maxpos);

            if (usingcoordsfrom)                    // if using an arbitary point, add it as a system..
            {
                SystemClass system = new SystemClass(fromsys);
                system.x = coordsfrom.X;
                system.y = coordsfrom.Y;
                system.z = coordsfrom.Z;
                G.AddNodeWithNoChk(new Node(system.x, system.y, system.z, system));
            }

            if (usingcoordsto)                      // if using an arbitary point, add it as a system..
            {
                SystemClass system = new SystemClass(tosys);
                system.x = coordsto.X;
                system.y = coordsto.Y;
                system.z = coordsto.Z;
                G.AddNodeWithNoChk(new Node(system.x, system.y, system.z, system));
            }

            AppendText("Number of stars within bounds " + G.Count + Environment.NewLine);

            CalculateArcs(G, maxrange);
        
            AStar AS = new AStar(G);

            Node start, stop;

            start = G.GetNodes.FirstOrDefault(x => x.System.SearchName == fromsys.ToLower());
            stop = G.GetNodes.FirstOrDefault(x => x.System.SearchName == tosys.ToLower());

            if ( start == null || stop == null )
            {
                AppendText("Code failed - blame Rob");
                return;
            }

            bool res;

            sw = new Stopwatch();

            sw.Start();
            res = AS.SearchPath(start, stop);
            sw.Stop();

            AppendText("Searching route from " + fromsys + " to " + tosys + Environment.NewLine);
            AppendText("Find route Time: " + sw.Elapsed.TotalSeconds.ToString("0.000s") + Environment.NewLine);
            AppendText("Total distance: " + traveldistance.ToString("0.00") + Environment.NewLine);
            AppendText("Max jumprange:" + maxrange + Environment.NewLine);

            if (res)
            {
                AppendText(Environment.NewLine + string.Format("{0,-30}Depart Co-Ords:{1:0.00},{2:0.00},{3:0.00}" + Environment.NewLine, fromsys , start.X, start.Y, start.Z));

                double totdist = 0;
                int jumps = 0;

                foreach (Arc A in AS.PathByArcs)
                {
                                                        // have to do it manually in case using the START, WAYPOINT or END points
                    double dist = Math.Sqrt((A.StartNode.X - A.EndNode.X) * (A.StartNode.X - A.EndNode.X) +
                                            (A.StartNode.Y - A.EndNode.Y) * (A.StartNode.Y - A.EndNode.Y) +
                                            (A.StartNode.Z - A.EndNode.Z) * (A.StartNode.Z - A.EndNode.Z));
                    AppendText(string.Format("{0,-30}Dist:{1:0.00}ly Co-Ords:{2:0.00},{3:0.00},{4:0.00}" + Environment.NewLine, A.EndNode.System.name, dist, A.EndNode.System.x, A.EndNode.System.y, A.EndNode.System.z));

                    totdist += dist;
                    jumps++;

                    Console.WriteLine(A.ToString());
                }

                AppendText(Environment.NewLine + "Total distance: " + totdist.ToString("0.00") + Environment.NewLine);
                AppendText("Jumps: " + jumps + Environment.NewLine);
            }
            else
                AppendText(Environment.NewLine + "NO Solution found - jump distance is too small or not enough star data between systems" + Environment.NewLine);
        }

        private void AddStarNodes(Graph G, Point3D minpos, Point3D maxpos)
        {
            List<SystemClass> systems = SystemData.SystemList;
            SystemClass system;

            for (int ii = 0; ii < systems.Count; ii++)
            {
                system = systems[ii];
                
                if (system.x >= minpos.X && system.x <= maxpos.X &&     // screening out stars reduces number and makes the arc calculator much quicker.
                    system.y >= minpos.Y && system.y <= maxpos.Y &&
                    system.z >= minpos.Z && system.z <= maxpos.Z)
                {
                    G.AddNodeWithNoChk(new Node(system.x, system.y, system.z, system));
                    //Console.WriteLine("Star " + system.SearchName );
                }
            }
        }

        private void CalculateArcs(Graph G, float maxrange )
        {
            float distance;
            float maxrangex2 = maxrange * maxrange;
            float minrangex2 = 1;                               // 1 ly.
            Node N1, N2;
            float weight = 1;
            float dx, dy, dz;

            int noofarcs = 0;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int ii = 0; ii < G.Count; ii++)
            {
                N1 = G.GetN(ii);

                for (int jj = ii; jj < G.Count; jj++)
                {
                    N2 = G.GetN(jj);

                    dx = (float)(N1.X - N2.X);
                    dy = (float)(N1.Y - N2.Y);
                    dz = (float)(N1.Z - N2.Z);
                    distance = dx * dx + dy * dy + dz * dz;

                    if (distance > minrangex2 && distance <= maxrangex2)
                    {
                        G.AddArcWithNoChk(N1, N2, weight);  // add N1->N2, its in the right direction
                        G.AddArcWithNoChk(N2, N1, weight);

                        noofarcs++;
                    }
                }
            }

            sw.Stop();
            AppendText("Create " + noofarcs + " arcs: " + sw.Elapsed.TotalSeconds.ToString("0.000s") + Environment.NewLine);
        }

        private void textBox_Range_KeyPress(object sender, KeyPressEventArgs e)
        {
            Tools.TextBox_Numeric_KeyPress(sender, e);
        }

        internal void NewPosition(object source)
        {
            string name = travelhistorycontrol1.netlog.visitedSystems.Last().Name;
            Invoke((MethodInvoker)delegate
            {
                textBoxCurrent.Text = name;
            });
        }

         private void textBoxCurrent_DoubleClick(object sender, EventArgs e)
        {
            textBox_From.Text = textBoxCurrent.Text;
        }


        private void AppendText(string msg)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    richTextBox1.AppendText(msg);
                });
            }
            catch
            {
            }
        }

        string SystemNameOnly(string s)             // removes @ at end.
        {
            int atpos = s.IndexOf('@');
            if (s != null && atpos != -1)
                s = s.Substring(0, atpos);
            s.Trim();
            return s;
        }

        public void SetFromSettings(string settings )
        {
        }

        public string GetStringSettings()
        {
            return  textBox_From.Text + "@!@" + textBox_To.Text + "@!@" + textBox_Range.Text + "@!@" +
                    textBox_FromX.Text + "@!@" + textBox_FromY.Text + "@!@" + textBox_FromZ.Text + "@!@" +
                    textBox_ToX.Text + "@!@" + textBox_ToY.Text + "@!@" + textBox_ToZ.Text + "@!@" +
                    textBox_From.ReadOnly + "@!@" + textBox_To.ReadOnly;
        }

        public void EnableRouteTab( string settings )                        // this means system is ready to go..
        {
            string[] sep = { "@!@" };
            string[] slist = settings.Split(sep, StringSplitOptions.None);
            bool fromstate = false, tostate = false;

            if (slist.Length == 11)
            {
                textBox_From.Text = slist[0];
                textBox_To.Text = slist[1];
                textBox_Range.Text = slist[2];
                textBox_FromX.Text = slist[3];
                textBox_FromY.Text = slist[4];
                textBox_FromZ.Text = slist[5];
                textBox_ToX.Text = slist[6];
                textBox_ToY.Text = slist[7];
                textBox_ToZ.Text = slist[8];
                fromstate = slist[9] == "True";
                tostate = slist[10] == "True";
            }

            SelectToMaster(tostate);
            UpdateTo(true);
            SelectFromMaster(fromstate);
            UpdateFrom(true);
            textBox_Range.ReadOnly = false;
            richTextBox1.Text = "In either the From or To box areas, either enter a system name in the upper text Box," + Environment.NewLine +
                                "or enter a set of galactic co-ordinates in the bottom three boxes (xyz)." + Environment.NewLine + Environment.NewLine +
                                "If you enter a system, its co-ordinates will be shown in the lower three boxes." + Environment.NewLine +
                                "If you enter galactic co-ordinates, the nearest system will be shown in the upper box." + Environment.NewLine +
                                "Select the jump distance and hit the route planning button to find a list of waypoints to traverse.";
        }

        private void textBox_Clicked(object sender, EventArgs e)
        {
            ((System.Windows.Forms.TextBox)sender).Select(0, 1000); // clicking highlights everything
        }

        private bool GetCoordsFrom(out Point3D pos)
        {
            double x = 0, y = 0, z = 0;

            bool worked = System.Double.TryParse(textBox_FromX.Text, out x) &&
                            System.Double.TryParse(textBox_FromY.Text, out y) &&
                            System.Double.TryParse(textBox_FromZ.Text, out z);
            pos = new Point3D(x, y, z);
            return worked;
        }

        private bool GetCoordsTo(out Point3D pos)
        {
            double x = 0, y = 0, z = 0;

            bool worked = System.Double.TryParse(textBox_ToX.Text, out x) &&
                            System.Double.TryParse(textBox_ToY.Text, out y) &&
                            System.Double.TryParse(textBox_ToZ.Text, out z);
            pos = new Point3D(x, y, z);
            return worked;
        }

        private bool IsValid()                          // have we star names or co-ords ready to go
        {
            bool readytocalc = true;
            Point3D pos;

            if (textBox_From.ReadOnly == false)          // if enabled, we are doing star names
            {
                if (SystemData.GetSystem(SystemNameOnly(textBox_From.Text)) == null)
                    readytocalc = false;
            }
            else // check co-ords
            {
                if (!GetCoordsFrom(out pos))
                    readytocalc = false;
            }
            if (textBox_To.ReadOnly == false)          // if enabled, we are doing star names
            {
                if (SystemData.GetSystem(SystemNameOnly(textBox_To.Text)) == null)
                    readytocalc = false;
            }
            else // check co-ords
            {
                if (!GetCoordsTo(out pos))
                    readytocalc = false;
            }

            return readytocalc;
        }

        // From..

        private void SelectFromMaster( bool coords )
        {
            textBox_From.ReadOnly = coords;
            textBox_FromX.ReadOnly = !coords;
            textBox_FromY.ReadOnly = !coords;
            textBox_FromZ.ReadOnly = !coords;
        }

        private void UpdateFrom(bool updatename)
        {
            changesilence = true;

            if (textBox_From.ReadOnly == false)                // if entering system name..
            {
                EDDiscovery2.DB.ISystem ds1 = SystemData.GetSystem(SystemNameOnly(textBox_From.Text));

                if (ds1 != null)
                {
                    if (updatename)                          // can't fix it as you type.. so leave alone
                        textBox_From.Text = ds1.name;

                    textBox_FromX.Text = ds1.x.ToString("0.00");
                    textBox_FromY.Text = ds1.y.ToString("0.00");
                    textBox_FromZ.Text = ds1.z.ToString("0.00");
                }
                else
                    textBox_FromX.Text = textBox_FromY.Text = textBox_FromZ.Text = "";
            }
            else
            {
                string res = "";
                Point3D curpos;
                if (GetCoordsFrom(out curpos))
                {
                    SystemClass nearest;
                    double distance;
                    FindNearestSystem(curpos, out nearest, out distance);

                    if (distance < 0.1)
                        res = nearest.name;
                    else
                        res = nearest.name + " @ " + distance.ToString("0.00") + "ly";
                }

                textBox_From.Text = res;
            }

            changesilence = false;
            button_Route.Enabled = IsValid();
        }

        private void textBox_From_Enter(object sender, EventArgs e)
        {
            SelectFromMaster(false);                              // enable system box
            UpdateFrom(true);
            ((System.Windows.Forms.TextBox)sender).Select(0, 1000); // clicking highlights everything
        }

        private void textBox_FromXYZ_Enter(object sender, EventArgs e)
        {
            SelectFromMaster(true);                       // coords master
            UpdateFrom(false);
            ((System.Windows.Forms.TextBox)sender).Select(0, 1000); // entering selects everything
        }

        private void textBox_From_TextChanged(object sender, EventArgs e)
        {
            if ( !changesilence )
                UpdateFrom(false);
        }

        private void textBox_XYZ_From_Changed(object sender, EventArgs e)
        {
            if (!changesilence)
                UpdateFrom(false);
        }

        // To..

        private void SelectToMaster(bool coords)
        {
            textBox_To.ReadOnly = coords;
            textBox_ToX.ReadOnly = !coords;
            textBox_ToY.ReadOnly = !coords;
            textBox_ToZ.ReadOnly = !coords;
        }

    
        private void UpdateTo(bool updatename)
        {
            changesilence = true;

            if (textBox_To.ReadOnly == false)                // if entering system name..
            {
                EDDiscovery2.DB.ISystem ds1 = SystemData.GetSystem(SystemNameOnly(textBox_To.Text));

                if (ds1 != null)
                {
                    if (updatename)                          // can't fix it as you type.. so leave alone
                        textBox_To.Text = ds1.name;

                    textBox_ToX.Text = ds1.x.ToString("0.00");
                    textBox_ToY.Text = ds1.y.ToString("0.00");
                    textBox_ToZ.Text = ds1.z.ToString("0.00");
                }
                else
                    textBox_ToX.Text = textBox_ToY.Text = textBox_ToZ.Text = "";
            }
            else // Co-ords..
            {
                string res = "";
                Point3D curpos;
                if (GetCoordsTo(out curpos))
                {
                    SystemClass nearest;
                    double distance;
                    FindNearestSystem(curpos, out nearest, out distance);

                    if (distance < 0.1)
                        res = nearest.name;
                    else
                        res = nearest.name + " @ " + distance.ToString("0.00") + "ly";
                }

                textBox_To.Text = res;
            }

            changesilence = false;
            button_Route.Enabled = IsValid();
        }

        private void textBox_To_Enter(object sender, EventArgs e)   // To has been tabbed/clicked..
        {
            SelectToMaster(false);                              // enable system box
            UpdateTo(true);                                  // update xyz, and if valid, update name
            ((System.Windows.Forms.TextBox)sender).Select(0, 1000); // clicking highlights everything
        }

        private void textBox_ToXYZ_Enter(object sender, EventArgs e)
        {
            SelectToMaster(true);                       // coords master
            UpdateTo(false);                            // and update..
            ((System.Windows.Forms.TextBox)sender).Select(0, 1000); // clicking highlights everything
        }

        private void textBox_To_TextChanged(object sender, EventArgs e)
        {
            if (!changesilence)
                UpdateTo(false);
        }

        private void textBox_XYZ_To_Changed(object sender, EventArgs e)
        {
            if (!changesilence)
                UpdateTo(false);
        }

        private void FindNearestSystem(Point3D curpos, out SystemClass system, out double distance)
        {
            List<SystemClass> systems = SystemData.SystemList;
            SystemClass nearestsystem = null;
            double mindistance = 1E19;

            for (int ii = 0; ii < systems.Count; ii++)
            {
                SystemClass syscheck = systems[ii];
                Point3D syspos = new Point3D(syscheck.x, syscheck.y, syscheck.z);
                double curdistance = Point3D.DistanceBetween(curpos, syspos);

                if (curdistance < mindistance)
                {
                    nearestsystem = syscheck;
                    mindistance = curdistance;
                }
            }

            system = nearestsystem;
            distance = mindistance;
        }

        // not called..

        private JArray Path2JSON(AStar AS)
        {
            JArray ja = new JArray();
            Node oldnode = null;

            foreach (Node n in AS.PathByNodes)
            {
                JObject jo = new JObject();

                double dist = 0;

                if (oldnode != null)
                    dist = SystemData.Distance(n.System, oldnode.System);



                oldnode = n;
                jo["name"] = n.System.name;
                //jo.Add("name", n.Name);
                jo["x"] = n.X;
                jo["y"] = n.Y;
                jo["z"] = n.Z;
                jo["dist"] = Math.Round(dist * 100) / 100;

                ja.Add(jo);
            }

            // serialize JSON directly to a file
            using (StreamWriter file = File.CreateText(@"route.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, ja);
            }

            return ja;
        }



    }
}
