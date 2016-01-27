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
            Route(textBox_From.Text, textBox_To.Text, maxrange, 0);
            this.Invoke(new Action(() => button_Route.Enabled = true));
        }

        private void Route(string s1, string s2, float maxrange, int mode)
        {
            EDDiscovery2.DB.ISystem ds1 = SystemData.GetSystem(s1);
            EDDiscovery2.DB.ISystem ds2 = SystemData.GetSystem(s2);

            if (ds1 == null)          // warn here, quicker to complain than waiting for nodes to populate.
            {
                AppendText("Start system:  " + s1 + " unknown");
                return;
            }
            if (ds2 == null)
            {
                AppendText("Destination system:  " + s2 + " unknown");
                return;
            }

            double earea = 10.0;            // add a little to the box for first/end points allowing out of box systems
            Point3D minpos = new EMK.LightGeometry.Point3D(
                    Math.Min(ds1.x, ds2.x) - earea,
                    Math.Min(ds1.y, ds2.y) - earea,
                    Math.Min(ds1.z, ds2.z) - earea );
            Point3D maxpos = new EMK.LightGeometry.Point3D(
                    Math.Max(ds1.x, ds2.x) + earea,
                    Math.Max(ds1.y, ds2.y)+ earea,
                    Math.Max(ds1.z, ds2.z) + earea );

            AppendText("Bounding Box " + minpos.X + "," + minpos.Y + "," + minpos.Z + " to " + maxpos.X + "," + maxpos.Y + "," + maxpos.Z + Environment.NewLine);

            Stopwatch sw = new Stopwatch();

            G = new Graph();                    // need to compute each time as systems in range changes each time
            PrepareNodes(G, maxrange, mode,minpos,maxpos);
        
            AStar AS = new AStar(G);

            Node start, stop;

            start = G.GetNodes.FirstOrDefault(x => x.System.SearchName == s1.ToLower());
            stop = G.GetNodes.FirstOrDefault(x => x.System.SearchName == s2.ToLower());

            bool res;

            sw = new Stopwatch();

            sw.Start();
            res = AS.SearchPath(start, stop);
            sw.Stop();


            AppendText("Searching route from " + ds1.name + " to " + ds2.name + Environment.NewLine);
            AppendText("Find route Time: " + sw.Elapsed.TotalSeconds.ToString("0.000s") + Environment.NewLine);
            AppendText("Total distance: " + SystemData.Distance(s1, s2).ToString("0.00") + Environment.NewLine);
            AppendText("Max jumprange:" + maxrange + Environment.NewLine);

            if (res)
            {
                AppendText(Environment.NewLine + "Depart " + ds1.name + Environment.NewLine);

                double totdist = 0;
                int jumps = 0;

                foreach (Arc A in AS.PathByArcs)
                {
                    double dist = SystemData.Distance(A.StartNode.System.name, A.EndNode.System.name);
                    AppendText(string.Format("{0,-30}Dist: {1:0.00} ly" + Environment.NewLine, A.EndNode.System.name, dist));

                    totdist += dist;
                    jumps++;

                    Console.WriteLine(A.ToString());
                }

                AppendText(Environment.NewLine + "Total distance: " + totdist.ToString("0.00") + Environment.NewLine);
                AppendText("Jumps: " + jumps + Environment.NewLine);

                // JArray ja = Path2JSON(AS);
            }
            else
                AppendText(Environment.NewLine + "NO Solution found - jump distance is too small or not enough star data between systems" + Environment.NewLine);
        }


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

        private  void PrepareNodes(Graph G, float maxrange, int mode, Point3D minpos , Point3D maxpos)
        {
            List<SystemClass> systems = SystemData.SystemList;
            float distance, maxrangex2 = maxrange * maxrange;
            Node N1,N2;
            float weight = 1;
            float dx, dy, dz;

            Stopwatch sw = new Stopwatch();

            sw.Start();

            SystemClass system;

            for (int ii = 0; ii < systems.Count; ii++)
            {
                system = systems[ii];
                                                    // screening out stars reduces number and makes the arc calculator much quicker.
                if (system.x >= minpos.X && system.x <= maxpos.X &&
                    system.y >= minpos.Y && system.y <= maxpos.Y &&
                    system.z >= minpos.Z && system.z <= maxpos.Z )
                {
                    N1 = new Node(system.x, system.y, system.z, system);
                    G.AddNodeWithNoChk(N1);
                }
            }

            sw.Stop();

            AppendText("Add stars: " + sw.Elapsed.TotalSeconds.ToString("0.000s") + Environment.NewLine);
            AppendText("No stars within bounds " + G.Count + Environment.NewLine);

            sw.Restart();
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

                    if (distance > 0 && distance <= maxrangex2)
                    {
                        if (mode == 1)
                            weight = (float)(1 / distance);

                        G.AddArcWithNoChk(N1, N2, weight);
                        G.AddArcWithNoChk(N2, N1, weight);
                    }
                }
            }

            sw.Stop();
            //Console.WriteLine("Create arcs: {0}", sw.Elapsed);
            AppendText("Create arcs: " + sw.Elapsed.TotalSeconds.ToString("0.000s") + Environment.NewLine);

        }

        private void textBox_Range_KeyPress(object sender, KeyPressEventArgs e)
        {
            Tools.TextBox_Numeric_KeyPress(sender, e);
        }

        private void RouteControl_Load(object sender, EventArgs e)
        {
            //travelhistorycontrol1.netlog.OnNewPosition += new NetLogEventHandler(NewPosition);
       
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

        private void textBox_From_TextChanged(object sender, EventArgs e)
        {
            button_Route.Enabled = IsRouteValid();
        }

        private void textBox_To_TextChanged(object sender, EventArgs e)
        {
            button_Route.Enabled = IsRouteValid();
        }

        private bool IsRouteValid() 
        {
            EDDiscovery2.DB.ISystem ds1 = SystemData.GetSystem(textBox_From.Text);
            EDDiscovery2.DB.ISystem ds2 = SystemData.GetSystem(textBox_To.Text);
            return ds1 != null && ds2 != null;
        }

        public void SetFromTo(string from, string to , string range )
        {
            textBox_From.Text = from;
            textBox_To.Text = to;
            textBox_Range.Text = range;
        }

        public void UpdateRouteButtonState()
        {
            button_Route.Enabled = IsRouteValid();
        }
    }
}
