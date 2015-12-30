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

        private class MyRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
            {
                var btn = e.Item as ToolStripButton;
                if (btn != null && btn.CheckOnClick && btn.Checked)
                {
                    Rectangle bounds = new Rectangle(Point.Empty, e.Item.Size);
                    e.Graphics.FillRectangle(Brushes.Orange, bounds);
                }
                else base.OnRenderButtonBackground(e);
            }
        }



        private const int ZoomMax = 100;
        private const double ZoomMin = 0.005;
        private const double ZoomFact = 1.2589254117941672104239541063958;
        private AutoCompleteStringCollection _systemNames;
        private SystemClass _centerSystem;
        private bool loaded = false;

        private float x = 0;
        private float y = 0;
        private float z = 0;
        private float zoom = 1;
        private float xang = 0;
        private float yang = 90;

        private Point MouseStartRotate;
        private Point MouseStartTranslate;

        public List<SystemClass> StarList;
        public List<SystemClass> ReferenceSystems;
        private Dictionary<string, SystemClass> VisitedStars;

        private List<IData3DSet> datasets;

        public bool ShowTril;

        public List<SystemPosition> VisitedSystems { get; set; }

        public SystemClass CenterSystem {
            get
            {
                return _centerSystem;
            }
            set
            {
                if (value != null && value.HasCoordinate)
                {
                    _centerSystem = value;
                }
                else
                {
                    _centerSystem = null;
                }
            }
        }

        public String CenterSystemName
        {
            get
            {
                if (CenterSystem != null)
                {
                    return CenterSystem.name;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    CenterSystem = SystemData.GetSystem(value.Trim());
                }
            }
        }

        public AutoCompleteStringCollection SystemNames {
            get
            {
                return _systemNames;
            }
            set
            {
                _systemNames = value;
            }
        }

        public FormMap()
        {
            InitializeComponent();
        }

        public void Prepare()
        {
            if (CenterSystem == null)
            {
                var db = new SQLiteDBClass();
                string defaultCenter = db.GetSettingString("DefaultMapCenter", null);
                OrientateMapAroundSystem(defaultCenter);
            }
            else
            {
                OrientateMapAroundSystem(CenterSystem);

            }
            toolStripShowAllStars.Renderer = new MyRenderer();
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

            if (w == 0 || h == 0) return;

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

            if (VisitedSystems != null)
            {
                foreach (SystemPosition sp in VisitedSystems)
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


            if (toolStripButtonGrid.Checked)
            {
                bool addstations = !toolStripButtonStations.Checked;
                var datasetGrid = new Data3DSetClass<LineData>("grid", (Color)System.Drawing.ColorTranslator.FromHtml("#ff7100"), 1.0f);

                for (int xx = -40000; xx <= 40000; xx += 1000)
                    {
                        datasetGrid.Add(new LineData(xx - CenterSystem.x, 0 - CenterSystem.y, CenterSystem.x  - - 20000,
                            xx - CenterSystem.x, 0 - CenterSystem.y, CenterSystem.z - 70000));
                    }
                    for (int zz = -20000; zz <= 70000; zz += 1000)
                                        datasetGrid.Add(new LineData(-40000 - CenterSystem.x, 0 - CenterSystem.y, CenterSystem.z - zz,
                         40000 - CenterSystem.x, 0 - CenterSystem.y, CenterSystem.z - zz));



                    datasets.Add(datasetGrid);
            }


            if (toolStripButtonShowAllStars.Checked)
            {
                bool addstations = !toolStripButtonStations.Checked;
                var datasetS = new Data3DSetClass<PointData>("stars", Color.White, 1.0f);

                foreach (SystemClass si in StarList)
                {
                    if (addstations  || si.population==0)
                        AddSystem(si, datasetS);
                }
                datasets.Add(datasetS);
            }


            if (toolStripButtonStations.Checked)
            {
                var datasetS = new Data3DSetClass<PointData>("stations", Color.RoyalBlue, 1.0f);

                foreach (SystemClass si in StarList)
                {
                   if (si.population>0)
                        AddSystem(si, datasetS);
                }
                datasets.Add(datasetS);
            }

            if (VisitedSystems != null && VisitedSystems.Any())
            {
                SystemClass lastknownps = null;
                foreach (SystemPosition ps in VisitedSystems)
                {
                    if (ps.curSystem!=null && ps.curSystem.HasCoordinate)
                    {
                        ps.lastKnownSystem = lastknownps;
                        lastknownps = ps.curSystem;
                    }
                }


                // For some reason I am unable to fathom this errors during the session after DBUpgrade8
                // colours just resolves to an object reference not set error, but after a restart it works fine
                // Not going to waste any more time, a one time restart is hardly the worst workaround in the world...
                IEnumerable<IGrouping<int, SystemPosition>> colours =
                    from SystemPosition sysPos in VisitedSystems
                    group sysPos by sysPos.vs.MapColour;

                foreach (IGrouping<int, SystemPosition> colour in colours)
                {
                    if (toolStripButtonDrawLines.Checked)
                    {
                        Data3DSetClass<LineData>  datasetl = new Data3DSetClass<LineData>("visitedstars" + colour.Key.ToString(), Color.FromArgb(colour.Key), 2.0f);
                        foreach (SystemPosition sp in colour)
                        {
                            if (sp.curSystem != null && sp.curSystem.HasCoordinate && sp.lastKnownSystem!=null && sp.lastKnownSystem.HasCoordinate)
                            {
                                datasetl.Add(new LineData(sp.curSystem.x - CenterSystem.x, sp.curSystem.y - CenterSystem.y, CenterSystem.z - sp.curSystem.z,
                                    sp.lastKnownSystem.x - CenterSystem.x, sp.lastKnownSystem.y - CenterSystem.y,  CenterSystem.z - sp.lastKnownSystem.z));

                            }
                        }
                        datasets.Add(datasetl);
                    }
                    else
                    {
                        var datasetvs = new Data3DSetClass<PointData>("visitedstars" + colour.Key.ToString(), Color.FromArgb(colour.Key), 2.0f);
                        foreach (SystemPosition sp in colour)
                        {
                            SystemClass star = SystemData.GetSystem(sp.Name);
                            if (star != null && star.HasCoordinate)
                            {
                                
                                AddSystem(star, datasetvs);
                            }
                        }
                        datasets.Add(datasetvs);
                    }

                }
            }


            var dataset = new Data3DSetClass<PointData>("Center", Color.Yellow, 5.0f);

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

        public void Reset()
        {
            CenterSystem = null;
            ReferenceSystems = null;        
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


            if (VisitedSystems != null)
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
            if (e.Delta > 0 && zoom < ZoomMax) zoom *= (float)ZoomFact;
            if (e.Delta < 0 && zoom > ZoomMin) zoom /= (float)ZoomFact;

            //System.Diagnostics.Trace.WriteLine("Zoom:" + zoom + " : W:" + (2000/ zoom).ToString("0"));
            UpdateStatus();
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
            textboxFrom.AutoCompleteCustomSource = _systemNames;
            ShowCenterSystem();
            GenerateDataSets();
            //GenerateDataSetsAllegiance();
            //GenerateDataSetsGovernment();
        }



        private void buttonCenter_Click(object sender, EventArgs e)
        {
            SystemClass sys = SystemData.GetSystem(textboxFrom.Text);
            SetCenterSystem(sys);
        }

        private void SetCenterSystem(SystemClass sys)
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
            UpdateStatus();
            labelSystemCoords.Text = string.Format("{0} x:{1} y:{2} z:{3}", CenterSystem.name, CenterSystem.x.ToString("0.00"), CenterSystem.y.ToString("0.00"), CenterSystem.z.ToString("0.00"));
        }

        private void UpdateStatus()
        {
            toolStripStatusLabelSystem.Text = CenterSystem.name;
            toolStripStatusLabelCoordinates.Text = string.Format("x:{0} y:{1} z:{2}", CenterSystem.x.ToString("0.00"), CenterSystem.y.ToString("0.00"), CenterSystem.z.ToString("0.00"));
            toolStripStatusLabelZoom.Text = "Width:" + (2000 / zoom).ToString("0");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SystemPosition ps2 = (from c in VisitedSystems where c.curSystem!=null && c.curSystem.HasCoordinate==true orderby c.time descending select c).FirstOrDefault<SystemPosition>();

            if (ps2!=null)
                SetCenterSystem(ps2.curSystem);
        }

        private void toolStripButtonDrawLines_Click(object sender, EventArgs e)
        {
            //toolStripButtonDrawLines.Checked = !toolStripButtonDrawLines.Checked;
            SetCenterSystem(CenterSystem);
        }

        private void buttonSetDefault_Click(object sender, EventArgs e)
        {
            SystemClass sys = SystemData.GetSystem(textboxFrom.Text);
            if (sys != null && sys.HasCoordinate)
            {
                var db = new SQLiteDBClass();
                db.PutSettingString("DefaultMapCenter", sys.name);
                if (CenterSystem.name != sys.name) SetCenterSystem(sys);
            }
        }

        private void toolStripButtonShowAllStars_Click(object sender, EventArgs e)
        {
            SetCenterSystem(CenterSystem);
        }

        private void toolStripButtonStations_Click(object sender, EventArgs e)
        {
            SetCenterSystem(CenterSystem);
        }

        private void toolStripButtonGrud_Click(object sender, EventArgs e)
        {
            SetCenterSystem(CenterSystem);
        }

        private void FormMap_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void OrientateMapAroundSystem(String systemName)
        {
            if (!String.IsNullOrWhiteSpace(systemName))
            {
                SystemClass system = SystemData.GetSystem(systemName.Trim());
                OrientateMapAroundSystem(system);
            }
        }

        private void OrientateMapAroundSystem(SystemClass system)
        {
            CenterSystem = system;
            textboxFrom.Text = system.name;
            SetCenterSystem(system);
        }
    }
}

