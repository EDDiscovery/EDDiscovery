using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2._3DMap;
using EDDiscovery2.Trilateration;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery2
{
    public partial class FormMap : Form
    {
        private const int ZoomMax = 15;
        private const double ZoomMin = 0.005;
        private readonly AutoCompleteStringCollection _systemNames;
        bool loaded = false;

        float x = 0;
        float y = 0;
        float z = 0;
        float zoom = 1;
        float xang = 0;
        float yang = 90;

        private Point MouseStartRotate;
        private Point MouseStartTranslate;
        private SystemClass CenterSystem;

        public List<SystemClass> StarList;
        public List<SystemPosition> visitedSystems;
        public List<SystemClass> ReferenceSystems;
        private Dictionary<string, SystemClass> VisitedStars;

        private List<IData3DSet> datasets;

        public bool ShowTril;

            public FormMap(AutoCompleteStringCollection SystemNames)
        {
                _systemNames = SystemNames;
            InitializeComponent();
        }

            public FormMap(SystemClass centerSystem, AutoCompleteStringCollection SystemNames) : this(SystemNames)
        {
            if (centerSystem != null && centerSystem.HasCoordinate) CenterSystem = centerSystem;
        }


        /// <summary>
        /// Loads Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glControl1_Load(object sender, EventArgs e)
        {
            loaded = true;
            GL.ClearColor(Color.Black); // Yey! .NET Colors can be used directly!

            SetupViewport();

        }

        /// <summary>
        /// Setup the Viewport
        /// </summary>
        private void SetupViewport()
        {

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            int w = glControl1.Width;
            int h = glControl1.Height;

            float orthoW = w * (zoom + 1);
            float orthoH = h * (zoom + 1);


            int orthoheight = 1000 * h / w;

            GL.Ortho(-1000.0, 1000.0, -orthoheight, orthoheight, -5000.0, 5000.0);
            //GL.Ortho(0, orthoW, 0, orthoH, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
        }


        public void InitData()
        {
            VisitedStars = new Dictionary<string, SystemClass>();

            if (visitedSystems != null)
            {
                foreach (SystemPosition sp in visitedSystems)
                {
                    SystemClass star = SystemData.GetSystem(sp.Name);
                    if (star != null && star.HasCoordinate)
                    {
                        VisitedStars[star.SearchName] = star;
                    }
                }
            }

        }


        private void GenerateDataSets()
        {
            GenerateDataSetStandard();
        }

        private void GenerateDataSetStandard()
        {
            InitGenerateDataSet();

            datasets = new List<IData3DSet>();

            var dataset = new Data3DSetClass<PointData>("stars", Color.White, 1.0f);

            foreach (SystemClass si in StarList)
            {
                AddSystem(si, dataset);
            }
            datasets.Add(dataset);


            if (visitedSystems != null && visitedSystems.Any())
            {
                // For some reason I am unable to fathom this errors during the session after DBUpgrade8
                // colours just resolves to an object reference not set error, but after a restart it works fine
                // Not going to waste any more time, a one time restart is hardly the worst workaround in the world...
                IEnumerable<IGrouping<int, SystemPosition>> colours =
                    from SystemPosition sysPos in visitedSystems
                    group sysPos by sysPos.vs.MapColour;

                foreach (IGrouping<int, SystemPosition> colour in colours)
                {
                    dataset = new Data3DSetClass<PointData>("visitedstars" + colour.Key.ToString(), Color.FromArgb(colour.Key), 2.0f);
                    foreach (SystemPosition sp in colour)
                    {
                        SystemClass star = SystemData.GetSystem(sp.Name);
                        if (star != null && star.HasCoordinate)
                        {
                            AddSystem(star, dataset);
                        }
                    }
                    datasets.Add(dataset);
                }
            }


            dataset = new Data3DSetClass<PointData>("Center", Color.Yellow, 5.0f);

            //GL.Enable(EnableCap.ProgramPointSize);
            dataset.Add(new PointData(0, 0, 0));
            datasets.Add(dataset);

            dataset = new Data3DSetClass<PointData>("Interest", Color.Purple, 10.0f);
            AddSystem("sol", dataset);
            AddSystem("sagittarius a*", dataset);
            //AddSystem("polaris", dataset);
            datasets.Add(dataset);


            if (ReferenceSystems != null && ReferenceSystems.Any())
            {
                var referenceLines = new Data3DSetClass<LineData>("CurrentReference", Color.Green, 5.0f);
                foreach (var refSystem in ReferenceSystems)
                {
                    referenceLines.Add(new LineData(0, 0, 0, refSystem.x - CenterSystem.x, refSystem.y - CenterSystem.y, CenterSystem.z - refSystem.z));
                }

                datasets.Add(referenceLines);

                var lineSet = new Data3DSetClass<LineData>("SuggestedReference", Color.DarkOrange, 5.0f);


                Stopwatch sw = new Stopwatch();
                sw.Start();
                SuggestedReferences references = new SuggestedReferences(CenterSystem.x, CenterSystem.y, CenterSystem.z);

                for (int ii = 0; ii < 16; ii++)
                {
                    var rsys = references.GetCandidate();
                    if (rsys == null) break;
                    var system = rsys.System;
                    references.AddReferenceStar(system);
                    if (ReferenceSystems != null && ReferenceSystems.Any(s => s.name == system.name)) continue;
                    System.Diagnostics.Trace.WriteLine(string.Format("{0} Dist: {1} x:{2} y:{3} z:{4}", system.name, rsys.Distance.ToString("0.00"), system.x, system.y, system.z));
                    lineSet.Add(new LineData(0, 0, 0, system.x - CenterSystem.x, system.y - CenterSystem.y, CenterSystem.z - system.z));
                }
                sw.Stop();
                System.Diagnostics.Trace.WriteLine("Reference stars time " + sw.Elapsed.TotalSeconds.ToString("0.000s"));
                datasets.Add(lineSet);
            }

        }

        private void AddSystem(string systemName, Data3DSetClass<PointData> dataset)
        {
            AddSystem(SystemData.GetSystem(systemName), dataset);
        }

        private void AddSystem(SystemClass system, Data3DSetClass<PointData> dataset)
        {
            if (system != null && system.HasCoordinate)
            {
                dataset.Add(new PointData(system.x - CenterSystem.x, system.y - CenterSystem.y, CenterSystem.z - system.z));
            }
        }


        private void GenerateDataSetsAllegiance()
        {

            var datadict = new Dictionary<int, Data3DSetClass<PointData>>();

            InitGenerateDataSet();

            datasets = new List<IData3DSet>();

            datadict[(int)EDAllegiance.Alliance] = new Data3DSetClass<PointData>(EDAllegiance.Alliance.ToString(), Color.Green, 1.0f);
            datadict[(int)EDAllegiance.Anarchy] = new Data3DSetClass<PointData>(EDAllegiance.Anarchy.ToString(), Color.Purple, 1.0f);
            datadict[(int)EDAllegiance.Empire] = new Data3DSetClass<PointData>(EDAllegiance.Empire.ToString(), Color.Blue, 1.0f);
            datadict[(int)EDAllegiance.Federation] = new Data3DSetClass<PointData>(EDAllegiance.Federation.ToString(), Color.Red, 1.0f);
            datadict[(int)EDAllegiance.Independent] = new Data3DSetClass<PointData>(EDAllegiance.Independent.ToString(), Color.Yellow, 1.0f);
            datadict[(int)EDAllegiance.None] = new Data3DSetClass<PointData>(EDAllegiance.None.ToString(), Color.LightGray, 1.0f);
            datadict[(int)EDAllegiance.Unknown] = new Data3DSetClass<PointData>(EDAllegiance.Unknown.ToString(), Color.DarkGray, 1.0f);

            foreach (SystemClass si in StarList)
            {
                if (si.HasCoordinate)
                {
                    datadict[(int)si.allegiance].Add(new PointData(si.x - CenterSystem.x, si.y - CenterSystem.y, CenterSystem.z - si.z));
                }
            }

            foreach (var ds in datadict.Values)
                datasets.Add(ds);

            datadict[(int)EDAllegiance.None].Visible = false;
            datadict[(int)EDAllegiance.Unknown].Visible = false;

        }


        private void GenerateDataSetsGovernment()
        {

            var datadict = new Dictionary<int, Data3DSetClass<PointData>>();

            InitGenerateDataSet();

            datasets = new List<IData3DSet>();

            datadict[(int)EDGovernment.Anarchy] = new Data3DSetClass<PointData>(EDGovernment.Anarchy.ToString(), Color.Yellow, 1.0f);
            datadict[(int)EDGovernment.Colony] = new Data3DSetClass<PointData>(EDGovernment.Colony.ToString(), Color.YellowGreen, 1.0f);
            datadict[(int)EDGovernment.Democracy] = new Data3DSetClass<PointData>(EDGovernment.Democracy.ToString(), Color.Green, 1.0f);
            datadict[(int)EDGovernment.Imperial] = new Data3DSetClass<PointData>(EDGovernment.Imperial.ToString(), Color.DarkGreen, 1.0f);
            datadict[(int)EDGovernment.Corporate] = new Data3DSetClass<PointData>(EDGovernment.Corporate.ToString(), Color.LawnGreen, 1.0f);
            datadict[(int)EDGovernment.Communism] = new Data3DSetClass<PointData>(EDGovernment.Communism.ToString(), Color.DarkOliveGreen, 1.0f);
            datadict[(int)EDGovernment.Feudal] = new Data3DSetClass<PointData>(EDGovernment.Feudal.ToString(), Color.LightBlue, 1.0f);
            datadict[(int)EDGovernment.Dictatorship] = new Data3DSetClass<PointData>(EDGovernment.Dictatorship.ToString(), Color.Blue, 1.0f);
            datadict[(int)EDGovernment.Theocracy] = new Data3DSetClass<PointData>(EDGovernment.Theocracy.ToString(), Color.DarkBlue, 1.0f);
            datadict[(int)EDGovernment.Cooperative] = new Data3DSetClass<PointData>(EDGovernment.Cooperative.ToString(), Color.Purple, 1.0f);
            datadict[(int)EDGovernment.Patronage] = new Data3DSetClass<PointData>(EDGovernment.Patronage.ToString(), Color.LightCyan, 1.0f);
            datadict[(int)EDGovernment.Confederacy] = new Data3DSetClass<PointData>(EDGovernment.Confederacy.ToString(), Color.Red, 1.0f);
            datadict[(int)EDGovernment.Prison_Colony] = new Data3DSetClass<PointData>(EDGovernment.Prison_Colony.ToString(), Color.Orange, 1.0f);
            datadict[(int)EDGovernment.None] = new Data3DSetClass<PointData>(EDGovernment.None.ToString(), Color.Gray, 1.0f);
            datadict[(int)EDGovernment.Unknown] = new Data3DSetClass<PointData>(EDGovernment.Unknown.ToString(), Color.DarkGray, 1.0f);

            foreach (SystemClass si in StarList)
            {
                if (si.HasCoordinate)
                {
                    datadict[(int)si.primary_economy].Add(new PointData(si.x - CenterSystem.x, si.y - CenterSystem.y, CenterSystem.z - si.z));
                }
            }

            foreach (var ds in datadict.Values)
                datasets.Add(ds);

            datadict[(int)EDGovernment.None].Visible = false;
            datadict[(int)EDGovernment.Unknown].Visible = false;

        }



        private void InitGenerateDataSet()
        {

            if (StarList == null)
                StarList = SQLiteDBClass.globalSystems;


            if (VisitedStars == null)
                InitData();
        }

        private void DrawStars()
        {
            foreach (var dataset in datasets)
            {
                dataset.DrawAll();
            }
        }

        /*
        private void DrawStars()
        {
            if (StarList == null)
                StarList = SQLiteDBClass.globalSystems;


            if (VisitedStars == null)
                InitData();

            GL.PointSize(1.0f);

            GL.Begin(PrimitiveType.Points);
            GL.Color3(Color.White);

            foreach (SystemClass si in StarList)
            {
                if (si.HasCoordinate)
                {
                    GL.Vertex3(si.x-CenterSystem.x, si.y-CenterSystem.y, CenterSystem.z-si.z);
                }
            }
            GL.End();



            GL.PointSize(2.0f);
            GL.Begin(PrimitiveType.Points);
            GL.Color3(Color.Red);


            if (visitedSystems != null)
            {
                SystemClass star;

                foreach (SystemClass sp in VisitedStars.Values)
                {
                    star = sp;
                    if (star != null)
                    {
                        GL.Vertex3(star.x - CenterSystem.x, star.y - CenterSystem.y, CenterSystem.z- star.z);
                    }
                }
            }
            GL.End();

            GL.PointSize(5.0f);
            GL.Begin(PrimitiveType.Points);
            GL.Enable(EnableCap.ProgramPointSize);
            GL.Color3(Color.Yellow);
            GL.Vertex3(0,0,0);
            GL.End();
        }

        */

        /// <summary>
        /// Calculate Translation of  (X, Y, Z) - according to mouse input
        /// </summary>
        private void SetupCursorXYZ()
        {
            //                x =   (PointToClient(Cursor.Position).X - glControl1.Width/2 ) * (zoom + 1);
            //                y = (-PointToClient(Cursor.Position).Y + glControl1.Height/2) * (zoom + 1);

            //                System.Diagnostics.Trace.WriteLine("X:" + x + " Y:" + y + " Z:" + zoom);
        }


        /// <summary>
        /// We need to setup each time our viewport and Ortho.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glControl1_Resize(object sender, EventArgs e)
        {
            if (!loaded)
                return;

            SetupViewport();
        }

        int nr = 0;

        /// <summary>
        /// Paint The control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded) // Play nice
                return;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();


            nr++;
            GL.Translate(x, y, 0); // position triangle according to our x variable
            GL.Rotate(xang, 0, 1, 0);
            GL.Rotate(yang, 1, 0, 0);
            GL.Scale(zoom, zoom, zoom);
            //                GL.Enable(EnableCap.PointSmooth);
            //                GL.Enable(EnableCap.Blend);


            GL.Enable(EnableCap.PointSmooth);
            GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);


            //GL.Color3(Color.Yellow);
            //GL.Begin(BeginMode.Triangles);
            //GL.Vertex2(10, 20);
            //GL.Vertex2(100, 20);
            //GL.Vertex2(100, 50);
            //GL.End();

            DrawStars();

            glControl1.SwapBuffers();

        }



        /// <summary>
        /// The triangle will always move with the cursor. 
        /// But is is not a problem to make it only if mousebutton pressed. 
        /// And do some simple ath with old Translation and new translation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                int dx = e.X - MouseStartRotate.X;
                int dy = e.Y - MouseStartRotate.Y;

                MouseStartRotate.X = e.X;
                MouseStartRotate.Y = e.Y;
                //System.Diagnostics.Trace.WriteLine("dx" + dx.ToString() + " dy " + dy.ToString() + " Button " + e.Button.ToString());


                xang += (float)(dx / 5.0);
                yang += (float)(-dy / 5.0);

                SetupCursorXYZ();

                glControl1.Invalidate();
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                int dx = e.X - MouseStartTranslate.X;
                int dy = e.Y - MouseStartTranslate.Y;

                MouseStartTranslate.X = e.X;
                MouseStartTranslate.Y = e.Y;
                //System.Diagnostics.Trace.WriteLine("dx" + dx.ToString() + " dy " + dy.ToString() + " Button " + e.Button.ToString());


                x += dx * zoom;
                y += -dy * zoom;

                SetupCursorXYZ();

                glControl1.Invalidate();
            }



        }


        private void OnMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Delta > 0 && zoom < ZoomMax) zoom *= 1.1f;
            if (e.Delta < 0 && zoom > ZoomMin) zoom /= 1.1f;

            SetupCursorXYZ();

            SetupViewport();
            glControl1.Invalidate();
        }

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseStartRotate.X = e.X;
                MouseStartRotate.Y = e.Y;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                MouseStartTranslate.X = e.X;
                MouseStartTranslate.Y = e.Y;
            }

        }

        private void FormMap_Load(object sender, EventArgs e)
        {
            textBox_From.AutoCompleteCustomSource = _systemNames;
            ShowCenterSystem();
            GenerateDataSets();
            //GenerateDataSetsAllegiance();
            //GenerateDataSetsGovernment();
        }



        private void buttonCenter_Click(object sender, EventArgs e)
        {
            SystemClass sys = SystemData.GetSystem(textBox_From.Text);
            SetCentersystem(sys);
        }

        private void SetCentersystem(SystemClass sys)
        {
            if (sys == null) return;

            CenterSystem = sys;
            ShowCenterSystem();

            GenerateDataSets();
            glControl1.Invalidate();
        }

        private void ShowCenterSystem()
        {
            if (CenterSystem == null)
            {
                CenterSystem = SystemData.GetSystem("sol") ?? new SystemClass { name = "Sol", SearchName = "sol", x = 0, y = 0, z = 0 };
            }

            label1.Text = string.Format("{0} x:{1} y:{2} z:{3}", CenterSystem.name, CenterSystem.x.ToString("0.00"), CenterSystem.y.ToString("0.00"), CenterSystem.z.ToString("0.00"));
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SystemPosition ps2 = (from c in visitedSystems where c.curSystem!=null && c.curSystem.HasCoordinate==true orderby c.time descending select c).FirstOrDefault<SystemPosition>();

            if (ps2!=null)
                SetCentersystem(ps2.curSystem);
        }
    }
}

