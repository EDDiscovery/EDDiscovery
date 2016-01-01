using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2.DB;
using EDDiscovery2._3DMap;
using EDDiscovery2.Trilateration;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
        private ISystem _centerSystem;
        private bool _loaded = false;

        private float _systemOffsetX = 0.0f;
        private float _systemOffsetY = 0.0f;
        private float _zoom = 1.0f;
        private float _xAng = 0.0f;
        private float _yAng = 90.0f;

        private Vector3 _cursorPosition;

        private Point _mouseStartRotate;
        private Point _mouseStartTranslate;

        private List<SystemClass> _starList;
        private Dictionary<string, SystemClass> _visitedStars;

        private List<IData3DSet> _datasets;

        public List<SystemClass> ReferenceSystems { get; set; }
        public List<SystemPosition> VisitedSystems { get; set; }

        public ISystem CenterSystem {
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
            ResetCamera();
            toolStripShowAllStars.Renderer = new MyRenderer();
        }

        private void ResetCamera()
        {
            _systemOffsetX = 0.0f;
            _systemOffsetY = 0.0f;

            _xAng = 0.0f;
            _yAng = 90.0f;

            _zoom = 1.0f;
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

            float orthoW = w * (_zoom + 1.0f);
            float orthoH = h * (_zoom + 1.0f);

            float orthoheight = 1000.0f * h / w;

            GL.Ortho(-1000.0f, 1000.0f, -orthoheight, orthoheight, -5000.0f, 5000.0f);
            //GL.Ortho(0, orthoW, 0, orthoH, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
        }


        public void InitData()
        {
            _visitedStars = new Dictionary<string, SystemClass>();

            if (VisitedSystems != null)
            {
                foreach (SystemPosition sp in VisitedSystems)
                {
                    SystemClass star = SystemData.GetSystem(sp.Name);
                    if (star != null && star.HasCoordinate)
                    {
                        _visitedStars[star.SearchName] = star;
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
            InitStarLists();

            var builder = new DatasetBuilder()
            {
                // TODO: I'm working on deprecating "Origin" so that everything is build with an origin of (0,0,0) and the camera moves instead.
                // This will allow us a little more flexibility with moving the cursor around and improving translation/rotations.
                Origin = CenterSystem,

                VisitedSystems = VisitedSystems,

                GridLines = toolStripButtonGrid.Checked,
                DrawLines = toolStripButtonDrawLines.Checked,
                AllSystems = toolStripButtonShowAllStars.Checked,
                Stations = toolStripButtonStations.Checked
            };
            if (_starList != null)
            {
                builder.StarList = _starList.ConvertAll(system => (ISystem)system);
            }
            if (ReferenceSystems != null)
            {
                builder.ReferenceSystems = ReferenceSystems.ConvertAll(system => (ISystem)system);
            }

            _datasets = builder.Build();
        }


        //TODO: If we reintroduce this, I recommend extracting this out to DatasetBuilder where we can unit test it and keep
        // it out of FormMap's hair
        private void GenerateDataSetsAllegiance()
        {

            var datadict = new Dictionary<int, Data3DSetClass<PointData>>();

            InitStarLists();

            _datasets = new List<IData3DSet>();

            datadict[(int)EDAllegiance.Alliance] = new Data3DSetClass<PointData>(EDAllegiance.Alliance.ToString(), Color.Green, 1.0f);
            datadict[(int)EDAllegiance.Anarchy] = new Data3DSetClass<PointData>(EDAllegiance.Anarchy.ToString(), Color.Purple, 1.0f);
            datadict[(int)EDAllegiance.Empire] = new Data3DSetClass<PointData>(EDAllegiance.Empire.ToString(), Color.Blue, 1.0f);
            datadict[(int)EDAllegiance.Federation] = new Data3DSetClass<PointData>(EDAllegiance.Federation.ToString(), Color.Red, 1.0f);
            datadict[(int)EDAllegiance.Independent] = new Data3DSetClass<PointData>(EDAllegiance.Independent.ToString(), Color.Yellow, 1.0f);
            datadict[(int)EDAllegiance.None] = new Data3DSetClass<PointData>(EDAllegiance.None.ToString(), Color.LightGray, 1.0f);
            datadict[(int)EDAllegiance.Unknown] = new Data3DSetClass<PointData>(EDAllegiance.Unknown.ToString(), Color.DarkGray, 1.0f);

            foreach (SystemClass si in _starList)
            {
                if (si.HasCoordinate)
                {
                    datadict[(int)si.allegiance].Add(new PointData(si.x - CenterSystem.x, si.y - CenterSystem.y, CenterSystem.z - si.z));
                }
            }

            foreach (var ds in datadict.Values)
                _datasets.Add(ds);

            datadict[(int)EDAllegiance.None].Visible = false;
            datadict[(int)EDAllegiance.Unknown].Visible = false;

        }


        //TODO: If we reintroduce this, I recommend extracting this out to DatasetBuilder where we can unit test it and keep
        // it out of FormMap's hair
        private void GenerateDataSetsGovernment()
        {

            var datadict = new Dictionary<int, Data3DSetClass<PointData>>();

            InitStarLists();

            _datasets = new List<IData3DSet>();

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

            foreach (SystemClass si in _starList)
            {
                if (si.HasCoordinate)
                {
                    datadict[(int)si.primary_economy].Add(new PointData(si.x - CenterSystem.x, si.y - CenterSystem.y, CenterSystem.z - si.z));
                }
            }

            foreach (var ds in datadict.Values)
                _datasets.Add(ds);

            datadict[(int)EDGovernment.None].Visible = false;
            datadict[(int)EDGovernment.Unknown].Visible = false;

        }

        private void InitStarLists()
        {

            if (_starList == null)
                _starList = SQLiteDBClass.globalSystems;


            if (_visitedStars == null)
                InitData();
        }

        private void DrawStars()
        {
            foreach (var dataset in _datasets)
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

        private void SetCenterSystem(ISystem sys)
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
            labelSystemCoords.Text = string.Format("{0} x:{1} y:{2} z:{3}", CenterSystem.name, CenterSystem.x.ToString("0.00"), CenterSystem.y.ToString("0.00"), CenterSystem.z.ToString("0.00"));
        }

        private void UpdateStatus()
        {
            statusLabel.Text = $"Coordinates: x={_cursorPosition.X} y={_cursorPosition.Y} z={_cursorPosition.Z}";
        }    
        
        private void OrientateMapAroundSystem(String systemName)
        {
            if (!String.IsNullOrWhiteSpace(systemName))
            {
                ISystem system = SystemData.GetSystem(systemName.Trim());
                OrientateMapAroundSystem(system);
            }
        }

        private void OrientateMapAroundSystem(ISystem system)
        {
            CenterSystem = system;
            textboxFrom.Text = system.name;
            SetCenterSystem(system);
        }

        private void FormMap_Load(object sender, EventArgs e)
        {
            textboxFrom.AutoCompleteCustomSource = _systemNames;
            ShowCenterSystem();
            GenerateDataSets();
            //GenerateDataSetsAllegiance();
            //GenerateDataSetsGovernment();
        }

        private void FormMap_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void buttonCenter_Click(object sender, EventArgs e)
        {
            SystemClass sys = SystemData.GetSystem(textboxFrom.Text);
            SetCenterSystem(sys);
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


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SystemPosition ps2 = (from c in VisitedSystems where c.curSystem != null && c.curSystem.HasCoordinate == true orderby c.time descending select c).FirstOrDefault<SystemPosition>();

            if (ps2 != null)
                SetCenterSystem(ps2.curSystem);
        }

        private void toolStripButtonDrawLines_Click(object sender, EventArgs e)
        {
            //toolStripButtonDrawLines.Checked = !toolStripButtonDrawLines.Checked;
            SetCenterSystem(CenterSystem);
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

        /// <summary>
        /// Loads Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glControl1_Load(object sender, EventArgs e)
        {
            _loaded = true;

            GL.ClearColor((Color)System.Drawing.ColorTranslator.FromHtml("#0D0D10")); 

            SetupViewport();
        }

        /// <summary>
        /// Paint The control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!_loaded) // Play nice
                return;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Translate(_systemOffsetX, _systemOffsetY, 0); // position triangle according to our x variable
            GL.Rotate(_xAng, 0, 1, 0);
            GL.Rotate(_yAng, 1, 0, 0);
            GL.Scale(_zoom, _zoom, _zoom);
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
            UpdateStatus();
        }

        /// <summary>
        /// We need to setup each time our viewport and Ortho.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glControl1_Resize(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            SetupViewport();
        }

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _mouseStartRotate.X = e.X;
                _mouseStartRotate.Y = e.Y;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                _mouseStartTranslate.X = e.X;
                _mouseStartTranslate.Y = e.Y;
            }

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
                int dx = e.X - _mouseStartRotate.X;
                int dy = e.Y - _mouseStartRotate.Y;

                _mouseStartRotate.X = e.X;
                _mouseStartRotate.Y = e.Y;
                //System.Diagnostics.Trace.WriteLine("dx" + dx.ToString() + " dy " + dy.ToString() + " Button " + e.Button.ToString());


                _xAng += (float)(dx / 5.0f);
                _yAng += (float)(-dy / 5.0f);

                SetupCursorXYZ();

                glControl1.Invalidate();
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                int dx = e.X - _mouseStartTranslate.X;
                int dy = e.Y - _mouseStartTranslate.Y;

                _mouseStartTranslate.X = e.X;
                _mouseStartTranslate.Y = e.Y;
                //System.Diagnostics.Trace.WriteLine("dx" + dx.ToString() + " dy " + dy.ToString() + " Button " + e.Button.ToString());


                _systemOffsetX += dx * _zoom;
                _systemOffsetY += -dy * _zoom;

                SetupCursorXYZ();

                glControl1.Invalidate();
            }
        }

        private void glControl1_OnMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Delta > 0 && _zoom < ZoomMax) _zoom *= (float)ZoomFact;
            if (e.Delta < 0 && _zoom > ZoomMin) _zoom /= (float)ZoomFact;

            //System.Diagnostics.Trace.WriteLine("Zoom:" + zoom + " : W:" + (2000/ zoom).ToString("0"));
            SetupCursorXYZ();

            SetupViewport();
            glControl1.Invalidate();
        }

        private void glControl1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
        }
    }
}

