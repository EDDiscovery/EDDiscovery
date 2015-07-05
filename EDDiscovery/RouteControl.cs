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

namespace EDDiscovery
{
    public partial class RouteControl : UserControl
    {
        Graph G;
        List<Node> nodes;
        float lastJumprange = -1;
        internal TravelHistoryControl travelhistorycontrol1;

        public RouteControl()
        {
            InitializeComponent();
        }

             private Thread ThreadRoute;

        private void button_Route_Click_1(object sender, EventArgs e)
        {

            richTextBox1.Clear();

            ThreadRoute = new System.Threading.Thread(new System.Threading.ThreadStart(RouteMain));
            ThreadRoute.Name = "Thread Route";
            ThreadRoute.Start();

            
            //Route(textBox_From.Text, textBox_To.Text, maxrange, 0); 
            //Route(textBox_From.Text, textBox_To.Text, maxrange, 1);

        }

        private void RouteMain()
        {
            float maxrange = float.Parse(textBox_Range.Text);

            button_Route.Enabled = false;
            Route(textBox_From.Text, textBox_To.Text, maxrange, 0);
            button_Route.Enabled = true;
        }

        private void Route(string s1, string s2, float maxrange, int mode)
        {


            Stopwatch sw = new Stopwatch();

            if (lastJumprange != maxrange)
            {
                G = new Graph();
                PrepareNodes(G, out nodes, maxrange, mode);
                //Console.WriteLine("Prepare nodes time: {0}", sw.Elapsed);
                lastJumprange = maxrange;
            }

            AStar AS = new AStar(G);

            Node start, stop;


            start = nodes.FirstOrDefault(x => x.System.SearchName == s1.ToLower());
            stop = nodes.FirstOrDefault(x => x.System.SearchName == s2.ToLower());
            bool res;

            if (start == null)
            {
                AppendText("Start system:  " + s1 + " unknown");
                return;
            }
            if (stop == null)
            {
                AppendText("Destination system:  " + s2 + " unknown");
                return;
            }

            sw = new Stopwatch();

            sw.Start();
            res = AS.SearchPath(start, stop);
            sw.Stop();


            AppendText("Searching route from " + s1 + " to " + s2 + Environment.NewLine);
            AppendText("Find route Time: " + sw.Elapsed.TotalSeconds.ToString("0.000s") + Environment.NewLine);
            AppendText("Total distance: " + SystemData.Distance(s1, s2).ToString("0.00") + Environment.NewLine);
            AppendText("Max jumprange:" + maxrange + Environment.NewLine);

            double totdist = 0;
            int jumps = 0;

            if (res)
            {
                foreach (Arc A in AS.PathByArcs)
                {
                    double dist = SystemData.Distance(A.StartNode.System.name, A.EndNode.System.name);
                    AppendText(A.EndNode.System.name + " \tDist: " + dist.ToString("0.00") + " ly" + Environment.NewLine);
                    totdist += dist;
                    jumps++;

                    Console.WriteLine(A.ToString());
                }

               // JArray ja = Path2JSON(AS);
            }
            else Console.WriteLine("No result !");


            AppendText("Total distance: " + totdist.ToString("0.00") + Environment.NewLine);
            AppendText("Jumps: " + jumps + Environment.NewLine);
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

        private  void PrepareNodes(Graph G, out List<Node> nodes, float maxrange, int mode)
        {
            List<SystemClass> systems = SystemData.SystemList;
            nodes = new List<Node>();
            SystemClass arcsystem;
            float distance, maxrangex2 = maxrange * maxrange;
            Node N2;
            float weight = 1;
            float dx, dy, dz;

            Stopwatch sw = new Stopwatch();

            sw.Start();

            SystemClass system;
            Node N1;

            for (int ii = 0; ii < systems.Count; ii++)
            {
                system = systems[ii];
                N1 = new Node(system.x, system.y, system.z, system);

                G.AddNodeWithNoChk(N1);
                nodes.Add(N1);
            }

            sw.Stop();
            
            AppendText("Add stars: " + sw.Elapsed.TotalSeconds.ToString("0.000s") + Environment.NewLine);

            sw.Restart();
            for (int ii = 0; ii < systems.Count; ii++)
            {
                system = systems[ii];
                N1 = nodes[ii];


                for (int jj = ii; jj < systems.Count; jj++)
                {
                    arcsystem = systems[jj];

                    dx = (float)(system.x - arcsystem.x);
                    dy = (float)(system.y - arcsystem.y);
                    dz = (float)(system.z - arcsystem.z);
                    distance = dx * dx + dy * dy + dz * dz;

                    //distance = (float)((system.x - arcsystem.x) * (system.x - arcsystem.x) + (system.y - arcsystem.y) * (system.y - arcsystem.y) + (system.z - arcsystem.z) * (system.z - arcsystem.z));

                    if (distance > 0 && distance <= maxrangex2)
                    {
                        N2 = nodes[jj];


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

        private void textBox_Range_TextChanged(object sender, EventArgs e)
        {

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

        private void textBoxCurrent_TextChanged(object sender, EventArgs e)
        {

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


    }
}
