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
using EDDiscovery2.DB;

namespace EDDiscovery
{
    public partial class RouteControl : UserControl
    {
        internal TravelHistoryControl travelhistorycontrol1;
        private EDDiscoveryForm _discoveryForm;
        internal bool changesilence = false;
        private SQLiteDBClass db;
        private List<SystemClass> routeSystems;

        int metric_nearestwaypoint = 0;     // easiest way to synchronise metric text and index's. 
        int metric_mindevfrompath = 1;
        int metric_maximum100ly = 2;
        int metric_maximum250ly = 3;
        int metric_maximum500ly = 4;
        int metric_waypointdev2 = 5;      

        string[] metric_options = { "Nearest to Waypoint", "Minimum Deviation from Path",
                                    "Nearest to Waypoint with dev<=100ly", "Nearest to Waypoint with dev<=250ly",
                                    "Nearest to Waypoint with dev<=500ly", "Nearest to Waypoint + Deviation / 2"
                                   };

        public RouteControl()
        {
            InitializeComponent();
            button_Route.Enabled = false;
            cmd3DMap.Enabled = false;

            for (int i = 0; i < metric_options.Length; i++)
                comboBoxRoutingMetric.Items.Add(metric_options[i]);

        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
        }

        private Thread ThreadRoute;

        private void button_Route_Click_1(object sender, EventArgs e)
        {
            if (db ==null)
                db = new SQLiteDBClass();


            ToggleButtons(false);           // beware the tab order, this moves the focus onto the next control, which in this dialog can be not what we want.
            richTextBox_routeresult.Clear();

            ThreadRoute = new System.Threading.Thread(new System.Threading.ThreadStart(RouteMain));
            ThreadRoute.Name = "Thread Route";
            ThreadRoute.Start();
        }

        private void RouteMain()
        {
            float maxrange = 30;
            string maxrangetext = "";
            bool usingcoordsfrom = false;
            bool usingcoordsto=false;
            Point3D coordsfrom = new Point3D(0, 0, 0);
            Point3D coordsto = new Point3D(0, 0, 0);
            string fromsys = "";
            string tosys = "";
            int routemethod = 0;

            Invoke( (MethodInvoker)delegate {                       // we are in a thread, should pick info up using a delegate
                maxrangetext = textBox_Range.Text;
                usingcoordsfrom = textBox_From.ReadOnly == true;
                usingcoordsto = textBox_To.ReadOnly == true;
                GetCoordsFrom(out coordsfrom);                      // will be valid for a system or a co-ords box
                GetCoordsTo(out coordsto);
                fromsys = textBox_From.Text;
                tosys = textBox_To.Text;
                routemethod = comboBoxRoutingMetric.SelectedIndex;
            });

            if (usingcoordsfrom)
                fromsys = "START POINT";
            if (usingcoordsto)
                tosys = "END POINT";

            if (!float.TryParse(maxrangetext, out maxrange)) maxrange = 30;
            double possiblejumps = Point3D.DistanceBetween(coordsfrom, coordsto) / maxrange;

            if (possiblejumps > 100)
            {
                DialogResult res = MessageBox.Show("This will result in a large number (" + possiblejumps.ToString("0") + ") of jumps" + Environment.NewLine + Environment.NewLine + "Confirm please", "Confirm you want to compute", MessageBoxButtons.YesNo);
                if (res != System.Windows.Forms.DialogResult.Yes)
                {
                    this.Invoke(new Action(() => ToggleButtons(true)));
                    return;
                }
            }

            RouteIterative(fromsys, usingcoordsfrom, coordsfrom,            
                            tosys, usingcoordsto, coordsto,
                              maxrange, routemethod);

            this.Invoke(new Action(() => ToggleButtons(true)));
        }

        private void ToggleButtons(bool state)
        {
            button_Route.Enabled = state;
            cmd3DMap.Enabled = state;
        }

        private void RouteIterative(string fromsys, bool usingcoordsfrom, Point3D coordsfrom,
                           string tosys, bool usingcoordsto, Point3D coordsto,
                           float maxrange,int routemethod)
        {
            double traveldistance = Point3D.DistanceBetween(coordsfrom, coordsto);      // its based on a percentage of the traveldistance
            routeSystems = new List<SystemClass>();
            routeSystems.Add(SystemData.GetSystem(textBox_From.Text));

            AppendText("Searching route from " + fromsys + " to " + tosys + " using " + metric_options[routemethod] + " metric" + Environment.NewLine);
            AppendText("Total distance: " + traveldistance.ToString("0.00") + " in " + maxrange.ToString("0.00") + "ly jumps" + Environment.NewLine);

            AppendText(Environment.NewLine);
            AppendText(string.Format("{0,-40}    Depart          @ {1,9:0.00},{2,8:0.00},{3,9:0.00}" + Environment.NewLine, fromsys, coordsfrom.X, coordsfrom.Y, coordsfrom.Z));

            Point3D curpos = coordsfrom;
            int jump = 1;
            double actualdistance = 0;
#if DEBUG
            //Console.WriteLine("-------------------------- BEGIN");
#endif
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
                //Console.WriteLine("Curpos " + curpos.X + "," + curpos.Y + "," + curpos.Z);
                //Console.WriteLine(" next" + nextpos.X + "," + nextpos.Y + "," + nextpos.Z);
#endif
                SystemClass bestsystem;
                Point3D bestposition;

                FindBestSystem(curpos, nextpos, maxrange, maxrange-0.5, routemethod, out bestsystem, out bestposition);
                string sysname = "WAYPOINT";
                double deltafromwaypoint = 0;
                double deviation = 0;

                if (bestsystem != null)
                {
                    deltafromwaypoint = Point3D.DistanceBetween(bestposition, nextpos);     // how much in error
                    deviation = Point3D.DistanceBetween(curpos.InterceptPoint(nextpos, bestposition), bestposition);
                    nextpos = bestposition;
                    sysname = bestsystem.name;
                    routeSystems.Add(bestsystem);
                }

                AppendText(string.Format("{0,-40}{1,3} Dist:{2,8:0.00}ly @ {3,9:0.00},{4,8:0.00},{5,9:0.00} WPd:{6,8:0.00}ly Dev:{7,8:0.00}ly" + Environment.NewLine,
                            sysname, jump, Point3D.DistanceBetween(curpos, nextpos), nextpos.X, nextpos.Y, nextpos.Z, deltafromwaypoint, deviation));

                actualdistance += Point3D.DistanceBetween(curpos, nextpos);
                curpos = nextpos;
                jump++;

            } while (true);
            routeSystems.Add(SystemData.GetSystem(textBox_To.Text));
            actualdistance += Point3D.DistanceBetween(curpos, coordsto);
            AppendText(string.Format("{0,-40}{1,3} Dist:{2,8:0.00}ly @ {3,9:0.00},{4,8:0.00},{5,9:0.00}" + Environment.NewLine, tosys, jump, Point3D.DistanceBetween(curpos, coordsto), coordsto.X, coordsto.Y, coordsto.Z));
            AppendText(Environment.NewLine);
            AppendText(string.Format("Straight Line Distance {0,8:0.00}ly vs Travelled Distance {1,8:0.00}ly" + Environment.NewLine, traveldistance, actualdistance));
        }

        private void FindBestSystem(Point3D curpos, Point3D wantedpos, double maxfromcurpos , double maxfromwanted, 
                                    int routemethod,
                                    out SystemClass system, out Point3D position )
        {
            List<SystemClass> systems = SystemData.SystemList;

            double maxfromcurposx2 = maxfromcurpos * maxfromcurpos;
            double maxfromwantedx2 = maxfromwanted * maxfromwanted;

            SystemClass nearestsystem = null;

            double bestmindistance = 1E39;

            for (int ii = 0; ii < systems.Count; ii++)
            {
                SystemClass syscheck = systems[ii];
                Point3D syspos = new Point3D(syscheck.x, syscheck.y, syscheck.z);
                double distancefromwantedx2 = Point3D.DistanceBetweenX2(wantedpos, syspos); // range between the wanted point and this, ^2
                double distancefromcurposx2 = Point3D.DistanceBetweenX2(curpos, syspos);    // range between the wanted point and this, ^2

                if (distancefromwantedx2 <= maxfromwantedx2 &&         // if within the radius of wanted
                        distancefromcurposx2 <= maxfromcurposx2)          // and within the jump range of current
                {
                    if (routemethod == metric_nearestwaypoint)
                    {
                        if (distancefromwantedx2 < bestmindistance)
                        {
                            nearestsystem = syscheck;
                            bestmindistance = distancefromwantedx2;
                        }
                    }
                    else
                    { 
                        Point3D interceptpoint = curpos.InterceptPoint(wantedpos, syspos);      // work out where the perp. intercept point is..
                        double deviation = Point3D.DistanceBetween(interceptpoint, syspos);
                        double metric = 1E39;

                        if (routemethod == metric_mindevfrompath)                             
                            metric = deviation;
                        else if (routemethod == metric_maximum100ly)
                            metric = (deviation <= 100) ? distancefromwantedx2 : metric;        // no need to sqrt it..
                        else if (routemethod == metric_maximum250ly)
                            metric = (deviation <= 250) ? distancefromwantedx2 : metric;
                        else if (routemethod == metric_maximum500ly)
                            metric = (deviation <= 500) ? distancefromwantedx2 : metric;
                        else if ( routemethod == metric_waypointdev2 )
                            metric = Math.Sqrt(distancefromwantedx2) + deviation / 2;

                        if (metric < bestmindistance)
                        {
                            nearestsystem = syscheck;
                            bestmindistance = metric;
                            //Console.WriteLine("System " + syscheck.name + " way " + deviation.ToString("0.0") + " metric " + metric.ToString("0.0") + " *");
                        }
                        else
                        {
                            //Console.WriteLine("System " + syscheck.name + " way " + deviation.ToString("0.0") + " metric " + metric.ToString("0.0"));
                        }
                    }
                }
            }

            system = nearestsystem;
            position = null;
            if (system != null)
            {
#if DEBUG
                //Console.WriteLine("Best System " + nearestsystem.name);
#endif
                position = new Point3D(system.x, system.y, system.z);
            }
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
            if ( textBoxCurrent.Text != "" )
                textBox_From.Text = textBoxCurrent.Text;
        }


        private void AppendText(string msg)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    richTextBox_routeresult.AppendText(msg);
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

        public void SaveSettings()
        {
            if (db == null)
                db = new SQLiteDBClass();

            db.PutSettingString("RouteFrom", textBox_From.Text);
            db.PutSettingString("RouteTo", textBox_To.Text);
            db.PutSettingString("RouteRange", textBox_Range.Text);
            db.PutSettingString("RouteFromX", textBox_FromX.Text);
            db.PutSettingString("RouteFromY", textBox_FromY.Text);
            db.PutSettingString("RouteFromZ", textBox_FromZ.Text);
            db.PutSettingString("RouteToX", textBox_ToX.Text);
            db.PutSettingString("RouteToY", textBox_ToY.Text);
            db.PutSettingString("RouteToZ", textBox_ToZ.Text);
            db.PutSettingBool("RouteFromState", textBox_From.ReadOnly);
            db.PutSettingBool("RouteToState", textBox_To.ReadOnly);
            db.PutSettingInt("RouteMetric", comboBoxRoutingMetric.SelectedIndex);
        }

        public void EnableRouteTab()
        {
            if (db == null)
                db = new SQLiteDBClass();

            textBox_From.Text = db.GetSettingString("RouteFrom", "");
            textBox_To.Text = db.GetSettingString("RouteTo", "");
            textBox_Range.Text = db.GetSettingString("RouteRange", "30");
            if (textBox_Range.Text == "")
                textBox_Range.Text = "30";
            textBox_FromX.Text = db.GetSettingString("RouteFromX", "");
            textBox_FromY.Text = db.GetSettingString("RouteFromY", "");
            textBox_FromZ.Text = db.GetSettingString("RouteFromZ", "");
            textBox_ToX.Text = db.GetSettingString("RouteToX", "");
            textBox_ToY.Text = db.GetSettingString("RouteToY", "");
            textBox_ToZ.Text = db.GetSettingString("RouteToZ", "");
            bool fromstate = db.GetSettingBool("RouteFromState", false);
            bool tostate = db.GetSettingBool("RouteToState", false);

            int metricvalue = db.GetSettingInt("RouteMetric", 0);
            comboBoxRoutingMetric.SelectedIndex = (metricvalue >= 0 && metricvalue < comboBoxRoutingMetric.Items.Count) ? metricvalue : metric_waypointdev2;

            SelectToMaster(tostate);
            UpdateTo(true);
            SelectFromMaster(fromstate);
            UpdateFrom(true);
            textBox_Range.ReadOnly = false;
            comboBoxRoutingMetric.Enabled = true;
            richTextBox_routeresult.Text = "In either the From or To box areas, either enter a system name in the upper text Box," + Environment.NewLine +
                                "or enter a set of galactic co-ordinates in the bottom three boxes (xyz)." + Environment.NewLine + Environment.NewLine +
                                "If you enter a system, its co-ordinates will be shown in the lower three boxes." + Environment.NewLine +
                                "If you enter galactic co-ordinates, the nearest system will be shown in the upper box." + Environment.NewLine +
                                "Select the jump distance and hit the route planning button to find a list of waypoints to traverse." + Environment.NewLine + Environment.NewLine +

                                "Select the routing method to try different routes from a combination of:" + Environment.NewLine +
                                "  Waypoint distance - how far the star is from the next target co-ordinate" + Environment.NewLine +
                                "  Deviation from the straight line path - how far is the star taking you away" + Environment.NewLine +
                                "  from a straight line journey to the destination" + Environment.NewLine +
                                "  Options exist to limit the deviation allowed." + Environment.NewLine +
                                "  A mix of Options exist to limit the deviation allowed and allow combinations of" + Environment.NewLine +
                                "  waypoint distance and deviation. Experiment to find the best path" + Environment.NewLine + Environment.NewLine
                ;

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

            if (comboBoxRoutingMetric.SelectedIndex < 0)
                readytocalc = false;

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

            UpdateDistance();

            changesilence = false;
            button_Route.Enabled = IsValid();
        }

        private void UpdateDistance()
        {
            Point3D from, to;
            string dist = "";
            if (GetCoordsFrom(out from) && GetCoordsTo(out to))
            {
                dist = Point3D.DistanceBetween(from, to).ToString("0.00");
            }

            textBox_Distance.Text = dist;
        }

        private void textBox_From_Enter(object sender, EventArgs e)
        {
            SelectFromMaster(false);                              // enable system box
            UpdateFrom(true);
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

            UpdateDistance();

            changesilence = false;
            button_Route.Enabled = IsValid();
        }

        private void textBox_To_Enter(object sender, EventArgs e)   // To has been tabbed/clicked..
        {
            SelectToMaster(false);                              // enable system box
            UpdateTo(true);                                  // update xyz, and if valid, update name
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

        private void comboBoxRoutingMetric_SelectedIndexChanged(object sender, EventArgs e)
        {
            button_Route.Enabled = IsValid();
        }

        private void cmd3DMap_Click(object sender, EventArgs e)
        {
            if (_discoveryForm.SystemNames.Count == 0)
            {
                MessageBox.Show("Systems have not been loaded yet, please wait", "No Systems Available", MessageBoxButtons.OK);
                return;
            }

            var map = _discoveryForm.Map;

            map.Instance.SystemNames = _discoveryForm.SystemNames;
            map.Instance.VisitedSystems = _discoveryForm.VisitedSystems;
            map.Instance.Reset();

            if (routeSystems != null && routeSystems.Any())
            {
                float zoom = 400 / float.Parse(textBox_Distance.Text) ;
                if (zoom < 0.01) zoom = 0.01f;
                if (zoom > 50) zoom = 50f;
                map.Instance.CenterSystem = routeSystems.First();
                map.Instance.PlannedRoute = routeSystems;
                map.Show(false, zoom);
            }
            else
            {
                map.Show();
            }
        }
    }
}
