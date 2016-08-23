using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2.DB;
using EDDiscovery2._3DMap;
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
using OpenTK.Input;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Collections.Concurrent;
using EDDiscovery.EDSM;

namespace EDDiscovery2
{
    public partial class FormMap : Form
    {
        #region Variables

        public bool noWindowReposition { get; set; } = false;                       // set externally
        public TravelHistoryControl travelHistoryControl { get; set; } = null;      // set externally

        const int HELP_VERSION = 5;         // increment this to force help onto the screen of users first time.

        public EDDConfig.MapColoursClass MapColours { get; set; } = EDDConfig.Instance.MapColours;

        private List<IData3DSet> _datasets_finegridlines;
        private List<IData3DSet> _datasets_coarsegridlines;
        private List<IData3DSet> _datasets_gridlinecoords;
        private List<IData3DSet> _datasets_maps;
        private List<IData3DSet> _datasets_selectedsystems;
        private List<IData3DSet> _datasets_visitedsystems;
        private List<IData3DSet> _datasets_routetri;
        private List<IData3DSet> _datasets_bookedmarkedsystems;
        private List<IData3DSet> _datasets_notedsystems;
        private List<IData3DSet> _datasets_galmapobjects;
        private List<IData3DSet> _datasets_galmapregions;

        StarGrids _stargrids;                   // holds stars

        StarNamesList _starnameslist;           // holds named stars

        private AutoCompleteStringCollection _systemNames;

        private ISystem _centerSystem;
        private ISystem _homeSystem;
        private ISystem _historySelection;

        private ISystem _clickedSystem;         // left clicked on a system/bookmark system/noted system
        private GalacticMapObject _clickedGMO;  // left clicked on a GMO
        private Vector3 _clickedposition = new Vector3(float.NaN,float.NaN,float.NaN);       // above two also set this.. if its a RM, only this is set.
        private string _clickedurl;             // what url is associated..

        private PositionDirection posdir = new PositionDirection();
        private ZoomFov zoomfov = new ZoomFov();

        private Timer _systemtimer = new Timer();     // kicks off star naming
        private bool _requestrepaint = false;                           // main system tick. Set to request repaint on next tick
        private Stopwatch _updateinterval = new Stopwatch();    // to accurately measure interval between system ticks
        private long _lastmstick;                           
        private int _msticks;                                   // between updates
        private CameraDirectionMovementTracker _lastcameranorm = new CameraDirectionMovementTracker();        // these track movements and zoom for most systems
        private CameraDirectionMovementTracker _lastcamerastarnames = new CameraDirectionMovementTracker();   // and for star names, which may be delayed due to background busy
        bool _starnamesbusy = false;                            // Are we in a compute cycle..
        bool _starnamescomputed = false;                        // worker thread is over..

        private Point _mouseStartRotate = new Point(int.MinValue, int.MinValue);        // used to indicate not started for these using mousemove
        private Point _mouseStartTranslateXY = new Point(int.MinValue, int.MinValue);
        private Point _mouseStartTranslateXZ = new Point(int.MinValue, int.MinValue);
        private Point _mouseDownPos;

        private Point _mouseHover;
        Timer _mousehovertick = new Timer();
        System.Windows.Forms.ToolTip _mousehovertooltip = null;

        private List<SystemClass> _referenceSystems { get; set; }
        public List<VisitedSystemsClass> _visitedSystems { get; set; }
        private List<SystemClass> _plannedRoute { get; set; }

        public List<FGEImage> fgeimages = new List<FGEImage>();

        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        private DateTimePicker startPicker;
        private DateTimePicker endPicker;
        private ToolStripControlHost startPickerHost;
        private ToolStripControlHost endPickerHost;

        private bool _isActivated = false;
        private float _znear;

        private ToolStripMenuItem _toolstripToggleNamingButton;     // for picking up this option quickly
        private ToolStripMenuItem _toolstripToggleRegionColouringButton;     // for picking up this option quickly

        public bool Is3DMapsRunning { get { return _stargrids != null; } }

        MapRecorder maprecorder = new MapRecorder();
        TimedMessage mapmsg = null;

        private float fps = 0;

        bool allowresizematrixchange = false;

        #endregion

        #region External Interface

        public void Prepare(VisitedSystemsClass historysel, string homesys, ISystem centersys, float zoom,
                                AutoCompleteStringCollection sysname, List<VisitedSystemsClass> visited)
        {
            _visitedSystems = visited;
            Prepare((historysel != null) ? FindSystem(historysel.Name) : null, homesys, centersys, zoom, sysname, visited);
        }

        public void Prepare(string historysel, string homesys, string centersys, float zoom,
                                AutoCompleteStringCollection sysname, List<VisitedSystemsClass> visited)
        {
            _visitedSystems = visited;
            Prepare(FindSystem(historysel), homesys, FindSystem(centersys), zoom, sysname, visited);
        }

        public void Prepare(ISystem historysel, string homesys, ISystem centersys, float zoom,
                            AutoCompleteStringCollection sysname, List<VisitedSystemsClass> visited)
        {
            _visitedSystems = visited;

            _historySelection = SafeSystem(historysel);
            _homeSystem = SafeSystem((homesys != null) ? FindSystem(homesys) : null);
            _centerSystem = SafeSystem(centersys);

            if (_stargrids == null)
            {
                _stargrids = new StarGrids();
                _stargrids.Initialise();                        // bring up the class..
                _starnameslist = new StarNamesList(_stargrids, this, glControl);
            }

            _systemNames = sysname;

            zoomfov.SetDefaultZoom(zoom);

            _referenceSystems = null;
            _plannedRoute = null;

            toolStripShowAllStars.Renderer = new MyRenderer();
            toolStripButtonDrawLines.Checked = SQLiteDBClass.GetSettingBool("Map3DDrawLines", true);
            showStarstoolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool("Map3DAllStars", true);
            showStationsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool("Map3DButtonStations", true);
            toolStripButtonPerspective.Checked = SQLiteDBClass.GetSettingBool("Map3DPerspective", false);
            toolStripButtonGrid.Checked = SQLiteDBClass.GetSettingBool("Map3DCoarseGrid", true);
            toolStripButtonFineGrid.Checked = SQLiteDBClass.GetSettingBool("Map3DFineGrid", true);
            toolStripButtonCoords.Checked = SQLiteDBClass.GetSettingBool("Map3DCoords", true);
            toolStripButtonEliteMovement.Checked = SQLiteDBClass.GetSettingBool("Map3DEliteMove", false);
            showNamesToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool("Map3DStarNaming", true);
            showDiscsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool("Map3DStarDiscs", true);
            showNoteMarksToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool("Map3DShowNoteMarks", true);
            showBookmarksToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool("Map3DShowBookmarks", true);
            toolStripButtonAutoForward.Checked = SQLiteDBClass.GetSettingBool("Map3DAutoForward", false);
            enableColoursToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool("Map3DButtonColours", true);
            _stargrids.ForceWhite = !enableColoursToolStripMenuItem.Checked;

            textboxFrom.AutoCompleteCustomSource = _systemNames;

            _stargrids.FillVisitedSystems(_visitedSystems);     // to ensure its updated
            _stargrids.Start();

            if (toolStripDropDownButtonGalObjects.DropDownItems.Count == 0)
            {
                Debug.Assert(EDDiscoveryForm.galacticMapping.galacticMapTypes != null);

                foreach (GalMapType tp in EDDiscoveryForm.galacticMapping.galacticMapTypes)
                {
                    if (tp.Group == GalMapType.GalMapGroup.Markers || tp.Group == GalMapType.GalMapGroup.Regions)       // only markers for now..
                    {
                        toolStripDropDownButtonGalObjects.DropDownItems.Add(AddGalMapButton(tp.Description, tp, tp.Enabled));
                        if (tp.Group == GalMapType.GalMapGroup.Regions)
                        {
                            _toolstripToggleRegionColouringButton = AddGalMapButton("Toggle Region Colouring", 2, SQLiteDBClass.GetSettingBool("Map3DGMORegionColouring", true));
                            toolStripDropDownButtonGalObjects.DropDownItems.Add(_toolstripToggleRegionColouringButton);
                        }
                    }
                }

                toolStripDropDownButtonGalObjects.DropDownItems.Add(AddGalMapButton("Toggle All", 0, null));

                _toolstripToggleNamingButton = AddGalMapButton("Toggle Star Naming", 1, SQLiteDBClass.GetSettingBool("Map3DGMONaming", true));
                toolStripDropDownButtonGalObjects.DropDownItems.Add(_toolstripToggleNamingButton);
            }
        }


        public ToolStripMenuItem AddGalMapButton( string name, Object tt, bool? checkedbut)
        {
            ToolStripMenuItem tsmi = new ToolStripMenuItem();
            tsmi.Text = name; 
            tsmi.Size = new Size(195, 22);
            tsmi.Tag = tt;
            if (checkedbut.HasValue)
            {
                tsmi.CheckState = CheckState.Checked;
                tsmi.CheckOnClick = true;
                tsmi.Checked = checkedbut.Value;
            }
            tsmi.Click += new System.EventHandler(this.showGalacticMapTypeMenuItem_Click);
            return tsmi;
        }


        public void SetPlannedRoute(List<SystemClass> plannedr)
        {
            _plannedRoute = plannedr;
            GenerateDataSetsRouteTri();
            RequestRepaint();
        }

        public void SetReferenceSystems(List<SystemClass> refsys)
        {
            _referenceSystems = refsys;
            GenerateDataSetsRouteTri();
            RequestRepaint();
        }

        public void UpdateVisitedSystems(List<VisitedSystemsClass> visited)
        {
            if (Is3DMapsRunning && visited != null )         // if null, we are not up and running.  visited should never be null, but being defensive
            {
                _visitedSystems = visited;
                _stargrids.FillVisitedSystems(_visitedSystems);          // update visited systems, will be displayed on next update of star grids
                GenerateDataSetsVisitedSystems();
                RequestRepaint();

                if (toolStripButtonAutoForward.Checked)             // auto forward?
                {
                    VisitedSystemsClass vs = _visitedSystems.FindLast(x => x.HasTravelCoordinates || (x.curSystem != null && x.curSystem.HasCoordinate));

                    if ( vs != null )
                        SetCenterSystemTo(vs.Name);
                }
            }
        }

        public void UpdateHistorySystem(string historysel)
        {
            if (Is3DMapsRunning)         // if null, we are not up and running
            {
                ISystem newhist = FindSystem(historysel);

                if (newhist != null)
                {
                    _historySelection = newhist;        // only override if found in starmap (meaning it has co-ords)
                }
            }
        }

        public void UpdateHistorySystem(VisitedSystemsClass historysel)
        {
            if (Is3DMapsRunning)         // if null, we are not up and running
            {
                ISystem newhist = historysel.curSystem;

                if (newhist != null)
                {
                    _historySelection = newhist;        // only override if found in starmap (meaning it has co-ords)
                }
            }
        }

        public void UpdateNote()
        {
            if (Is3DMapsRunning)         // if null, we are not up and running
            {
                GenerateDataSetsBNG();
                RequestRepaint();
            }
        }

        public void UpdateBookmarksGMO(bool gototarget )
        {
            if (Is3DMapsRunning)         // if null, we are not up and running
            {
                GenerateDataSetsBNG();

                if (gototarget)
                {
                    string name;
                    double x, y, z;

                    if (TargetClass.GetTargetPosition(out name, out x, out y, out z))
                        posdir.StartCameraSlew(new Vector3((float)x, (float)y, (float)z),-1F);
                }

                RequestRepaint();
            }
        }

        #endregion

        #region Initialisation

        public FormMap()
        {
            InitializeComponent();
        }

        private void FormMap_Load(object sender, EventArgs e)
        {
            var top = SQLiteDBClass.GetSettingInt("Map3DFormTop", -1);

            if (top >= 0 && noWindowReposition == false)
            {
                var left = SQLiteDBClass.GetSettingInt("Map3DFormLeft", 0);
                var height = SQLiteDBClass.GetSettingInt("Map3DFormHeight", 800);
                var width = SQLiteDBClass.GetSettingInt("Map3DFormWidth", 800);
                this.Location = new Point(left, top);
                this.Size = new Size(width, height);
                //Console.WriteLine("Restore map " + this.Top + "," + this.Left + "," + this.Width + "," + this.Height);
            }

            LoadMapImages();
            FillExpeditions();
            SetCenterSystemLabel();
            labelClickedSystemCoords.Text = "Click a star to select/copy, double-click to center";

            posdir.SetCameraPos(new Vector3((float)_centerSystem.x, -(float)_centerSystem.y, (float)_centerSystem.z));

            zoomfov.SetToDefault();
            _lastcameranorm.Update(posdir.CameraDirection, posdir.Position, zoomfov.Zoom); // set up here so ready for action.. below uses it.
            _lastcameranorm.ForceZoomChanged();                      
            _lastcamerastarnames.Update(posdir.CameraDirection, posdir.Position, zoomfov.Zoom); // set up here so ready for action.. below uses it.
            _lastcamerastarnames.ForceZoomChanged();                    

            GenerateDataSets();
            GenerateDataSetsMaps();
            GenerateDataSetsSelectedSystems();
            GenerateDataSetsVisitedSystems();
            GenerateDataSetsRouteTri();

            _mousehovertick.Tick += new EventHandler(MouseHoverTick);
            _mousehovertick.Interval = 250;

            SetCenterSystemTo(_centerSystem);                   // move to this..
        }

        private void FormMap_Shown(object sender, EventArgs e)
        {
            Console.WriteLine("SHown");
            int helpno = SQLiteDBClass.GetSettingInt("Map3DShownHelp", 0);                 // force up help, to make sure they know it exists

            if (helpno != HELP_VERSION)
            {
                toolStripButtonHelp_Click(null, null);
                SQLiteDBClass.PutSettingInt("Map3DShownHelp", HELP_VERSION);
            }

            SetModelProjectionMatrix();
            allowresizematrixchange = true;
            StartSystemTimer();
        }

        void StartSystemTimer()
        {
            if ( !_systemtimer.Enabled )
            {
                _updateinterval.Stop();
                _updateinterval.Start();
                _lastmstick = _updateinterval.ElapsedMilliseconds;
                _systemtimer.Interval = 25;
                _systemtimer.Tick += new EventHandler(Update);
                _systemtimer.Start();
            }
        }

        private void FormMap_Resize(object sender, EventArgs e)         // resizes changes glcontrol width/height, so needs a new viewport
        {
            if (!allowresizematrixchange)
            {
                SetModelProjectionMatrix();
                RequestRepaint();
            }
        }

        private void FormMap_Activated(object sender, EventArgs e)
        {
            Console.WriteLine("Activated");
            _isActivated = true;
            StartSystemTimer();                     // in case Close, then open, only get activated
            RequestRepaint();
        }

        private void FormMap_Deactivate(object sender, EventArgs e)
        {
            Console.WriteLine("DeActivated");
            _isActivated = false;
        }

        private void FormMap_FormClosing(object sender, FormClosingEventArgs e)
        {
            _systemtimer.Stop();
            _updateinterval.Stop();

            if (Visible)
            {
                SQLiteDBClass.PutSettingInt("Map3DFormWidth", this.Width);
                SQLiteDBClass.PutSettingInt("Map3DFormHeight", this.Height);
                SQLiteDBClass.PutSettingInt("Map3DFormTop", this.Top);
                SQLiteDBClass.PutSettingInt("Map3DFormLeft", this.Left);
                //Console.WriteLine("Save map " + this.Top + "," + this.Left + "," + this.Width + "," + this.Height);
            }

            SQLiteDBClass.PutSettingBool("Map3DAutoForward", toolStripButtonAutoForward.Checked);
            SQLiteDBClass.PutSettingBool("Map3DDrawLines", toolStripButtonDrawLines.Checked);
            SQLiteDBClass.PutSettingBool("Map3DAllStars", showStarstoolStripMenuItem.Checked);
            SQLiteDBClass.PutSettingBool("Map3DButtonColours", enableColoursToolStripMenuItem.Checked);
            SQLiteDBClass.PutSettingBool("Map3DButtonStations", showStationsToolStripMenuItem.Checked);
            SQLiteDBClass.PutSettingBool("Map3DCoarseGrid", toolStripButtonGrid.Checked);
            SQLiteDBClass.PutSettingBool("Map3DFineGrid", toolStripButtonFineGrid.Checked);
            SQLiteDBClass.PutSettingBool("Map3DCoords", toolStripButtonCoords.Checked);
            SQLiteDBClass.PutSettingBool("Map3DEliteMove", toolStripButtonEliteMovement.Checked);
            SQLiteDBClass.PutSettingBool("Map3DStarDiscs", showDiscsToolStripMenuItem.Checked);
            SQLiteDBClass.PutSettingBool("Map3DStarNaming", showNamesToolStripMenuItem.Checked);
            SQLiteDBClass.PutSettingBool("Map3DShowNoteMarks", showNoteMarksToolStripMenuItem.Checked);
            SQLiteDBClass.PutSettingBool("Map3DShowBookmarks", showBookmarksToolStripMenuItem.Checked);
            SQLiteDBClass.PutSettingBool("Map3DPerspective", toolStripButtonPerspective.Checked);
            SQLiteDBClass.PutSettingBool("Map3DGMONaming", _toolstripToggleNamingButton.Checked);
            SQLiteDBClass.PutSettingBool("Map3DGMORegionColouring", _toolstripToggleRegionColouringButton.Checked);
            EDDiscoveryForm.galacticMapping.SaveSettings();

            _stargrids.Stop();

            e.Cancel = true;
            this.Hide();
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            GL.ClearColor((Color)System.Drawing.ColorTranslator.FromHtml("#0D0D10"));
            Console.WriteLine("GL load");
        }

        private void SetModelProjectionMatrix()
        {
            posdir.SetProjectionMatrix(toolStripButtonPerspective.Checked, zoomfov.Fov, glControl.Width, glControl.Height, out _znear);
            posdir.CalculateModelMatrix(zoomfov.Zoom);
        }

        #endregion

        #region Main Timer

        private void Update(object sender, EventArgs e)                 // tick.. tock.. every X ms.  Drives everything now.
        {
            //Console.WriteLine("Tick");
            if (!Visible)
                return;

            long elapsed = _updateinterval.ElapsedMilliseconds;         // stopwatch provides precision timing on last paint time.
            _msticks = (int)(elapsed - _lastmstick);
            _lastmstick = elapsed;

            if (_isActivated && glControl.Focused)                      // if we can accept keys
            {
                KeyboardActions _kbdActions = new KeyboardActions();

                _kbdActions.ReceiveKeyboardActions(posdir.InPerspectiveMode);       // ALL keyboard actions performed in this section.

                if (_kbdActions.Any())                                  // if any actions..
                {
                    posdir.KillSlews();                                 // no slewing now please
                    zoomfov.KillSlew();

                    posdir.HandleTurningAdjustments(_kbdActions, _msticks);     // any turning?
                                                                                // any move?
                    posdir.HandleMovementAdjustments(_kbdActions, _msticks, zoomfov.Zoom, toolStripButtonEliteMovement.Checked);

                    HandleSpecialKeys(_kbdActions);                     // special keys for us

                    zoomfov.HandleZoomAdjustmentKeys(_kbdActions, _msticks);
                }
            }

            posdir.DoCameraSlew(_msticks);
            zoomfov.DoZoomSlew(_msticks);

            if (maprecorder.InPlayBack)
            {
                Vector3 newpos, newdir;
                float newzoom;
                long timetopan, timetofly, timetozoom, timetomsg;
                string message;
                if (maprecorder.PlayBack(out newpos, out timetofly, out newdir, out timetopan, out newzoom, out timetozoom, out message, out timetomsg))
                {
                    Console.WriteLine("{0} Playback {1} {2} {3} fly {4} pan {5} msg {6}", _updateinterval.ElapsedMilliseconds % 10000,
                        newpos, newdir, newzoom, timetofly, timetopan, message);

                    posdir.StartCameraSlew(newpos, (float)timetofly / 1000.0F);

                    posdir.StartCameraPan(newdir, (float)timetopan / 1000.0F);

                    zoomfov.StartZoom(newzoom, (float)timetozoom / 1000.0F);

                    if (message != null && message.Length > 0)
                    {
                        if (mapmsg != null)
                        {
                            mapmsg.Close();
                            mapmsg = null;
                        }

                        if (timetomsg == 0)     // 0 default, use a sensible time
                            timetomsg = 3000;

                        mapmsg = new TimedMessage();
                        mapmsg.Init("", message, (int)timetomsg, true, 0.9F, Color.Transparent, Color.White, new Font("MS Sans Serif", 20.0F));
                        mapmsg.Position(this, 0, 0, -1, -20, 0, 0);
                        mapmsg.Show();
                        glControl.Focus();
                    }
                }

                if (!maprecorder.InPlayBack)  // dropped out of playback?
                    SetDropDownRecordImage();
            }
            else if (maprecorder.Recording)
            {
                maprecorder.Record(posdir.Position, posdir.CameraDirection, zoomfov.Zoom);
            }

            _lastcameranorm.Update(posdir.CameraDirection, posdir.Position, zoomfov.Zoom);

            Tools.LogToFile(String.Format("Tick Dir {0} zoom {1} move {2}", _lastcameranorm.CameraDirChanged, _lastcameranorm.CameraZoomed, _lastcameranorm.CameraMoved));

            if (_lastcameranorm.CameraDirChanged || _lastcameranorm.CameraZoomed || _lastcameranorm.CameraMoved)
            {
                posdir.CalculateModelMatrix(zoomfov.Zoom);
                _requestrepaint = true;

                if (_lastcameranorm.CameraZoomed || _lastcameranorm.CameraDirChanged)
                {
                    UpdateDataSetsDueToZoomOrFlip(_lastcameranorm.CameraZoomed);
                }

                KillHover();
            }

            if (_stargrids.Update(posdir.Position.X, posdir.Position.Z, zoomfov.Zoom, glControl))       // at intervals, inform star grids of position, and if it has
            {
                _requestrepaint = true;
            }

            if (!_starnamesbusy)                            // flag indicates we have not gone thru a complete estimate-draw cycle
            {
                bool names = showNamesToolStripMenuItem.Checked;
                bool discs = showDiscsToolStripMenuItem.Checked;

                _lastcamerastarnames.Update(posdir.CameraDirection, posdir.Position, zoomfov.Zoom);

                if ((names | discs) && zoomfov.Zoom >= 0.99)  // only when shown, and enabled, and with a good zoom
                {
                    bool dirorzoom = _lastcamerastarnames.CameraDirChanged || _lastcamerastarnames.CameraZoomed;

                    if (_stargrids.IsDisplayed(posdir.Position.X, posdir.Position.Z) && (dirorzoom || _lastcamerastarnames.CameraMoved))                              // if changed something
                    {
                        _starnamesbusy = true;
                        _starnameslist.Update(_lastcamerastarnames, dirorzoom, posdir.GetResMatd, _znear, names, discs);
                    }
                }
                else
                {
                    if (_starnameslist.RemoveAllNamedStars())
                    {
                        //Console.WriteLine("Remove stars");
                        _requestrepaint = true;
                    }
                }
            }

            if (_requestrepaint)
            {
                //Console.WriteLine("{0} {1} Tick m{2} d{3} key {4} {5} ", _updateinterval.ElapsedMilliseconds % 10000, _msticks, _lastcameranorm.CameraMoved, _lastcameranorm.CameraDirChanged, _kbdActions.Any(), (_msticks > 100) ? "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" : "");
                _requestrepaint = false;
                glControl.Invalidate();                 // and kick paint - not via the function ON purpose, so we can distinguish between this and others reasons
            }
        }

        public void ChangeNamedStars()                  // background estimator finished.. repaint and indicate computed to foreground
        {
            _requestrepaint = true;
            _starnamescomputed = true;
            //Console.WriteLine("name");
        }

        private void RequestRepaint()       // ask if you've change objects
        {
            _requestrepaint = true;
            _lastcameranorm.ForceZoomChanged();     // and make it reestimate zoom sizes on objects...
        }

        public void HandleSpecialKeys(KeyboardActions _kbdActions)
        {
            if (_kbdActions.Action(KeyboardActions.ActionType.IncrStar))
            {
                _starnameslist.IncreaseStarLimit();
                RequestRepaint();
            }

            if (_kbdActions.Action(KeyboardActions.ActionType.DecrStar))
            {
                _starnameslist.DecreaseStarLimit();
                RequestRepaint();
            }

            if (_kbdActions.Action(KeyboardActions.ActionType.Record))
            {
                maprecorder.RecordStepDialog(posdir.Position, posdir.CameraDirection, zoomfov.Zoom);
            }
        }


        #endregion

        #region OpenGL Render and Viewport

        long lasttick = 0;
        private void glControl_Paint(object sender, PaintEventArgs e)
        {
#if DEBUG
            long curtime = _updateinterval.ElapsedMilliseconds;
            long timesince = curtime - lasttick;
            lasttick = curtime;

            if (timesince > 200)
                fps = 0;
            else
            {
                float newfps = 1000.0F / (float)timesince;
                fps = (fps * 0.9F) + (newfps * 0.1F);
            }
#endif
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);            // select the current matrix to the model view

            Matrix4 mm = posdir.ModelMatrix;
            GL.LoadMatrix(ref mm);                          // set the model view to this matrix.

            GL.Enable(EnableCap.PointSmooth);                                               // Render galaxy
            GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.PushMatrix();
            DrawStars();
            GL.PopMatrix();

            glControl.SwapBuffers();
            UpdateStatus();

//            Console.WriteLine("{0} Paint since {1} took {2}", curtime % 10000, timesince, _updateinterval.ElapsedMilliseconds - curtime);

            Tools.LogToFile(String.Format("{0} Paint since {1} took {2} {3}", curtime % 10000, timesince, _updateinterval.ElapsedMilliseconds - curtime , (timesince>60) ? "************************************":""));
        }

        private void DrawStars()
        {
            // Take references on objects that could be replaced by the background (?)
            List<IData3DSet> _datasets_galmapobjects = this._datasets_galmapobjects;
            List<IData3DSet> _datasets_notedsystems = this._datasets_notedsystems;
            List<IData3DSet> _datasets_bookmarkedsystems = this._datasets_bookedmarkedsystems;

            if (_datasets_maps == null)                     // happens during debug.. paint before form load
                return;                                     // needs to be in order of background to foreground objects

            Debug.Assert(_datasets_finegridlines != null);
            if (toolStripButtonFineGrid.Checked && _datasets_finegridlines != null )
            {
                foreach (var dataset in _datasets_finegridlines)        // these show thru the gal regions
                    dataset.DrawAll(glControl);
            }

            if (_datasets_galmapregions != null)
            {
                _datasets_galmapregions[0].DrawAll(glControl);          
            }

            foreach (var dataset in _datasets_maps)                     
                dataset.DrawAll(glControl);

            Debug.Assert(_datasets_coarsegridlines != null);
            if (toolStripButtonGrid.Checked && _datasets_coarsegridlines != null )
            {
                foreach (var dataset in _datasets_coarsegridlines)
                    dataset.DrawAll(glControl);
            }

            Debug.Assert(_datasets_gridlinecoords != null);
            if (toolStripButtonCoords.Checked && _datasets_gridlinecoords != null )
            {
                foreach (var dataset in _datasets_gridlinecoords)
                    dataset.DrawAll(glControl);
            }

            if (_datasets_galmapregions != null)
            {
                _datasets_galmapregions[1].DrawAll(glControl);          // next one is the outlines
                _datasets_galmapregions[2].DrawAll(glControl);          // and the names
            }

            _stargrids.DrawAll(glControl, showStarstoolStripMenuItem.Checked, showStationsToolStripMenuItem.Checked);

            Debug.Assert(_datasets_galmapobjects != null);
            if (_datasets_galmapobjects != null)
            {
                foreach (var dataset in _datasets_galmapobjects)
                    dataset.DrawAll(glControl);
            }

            Debug.Assert(_datasets_routetri != null);
            if (_datasets_routetri != null)
            {
                foreach (var dataset in _datasets_routetri)
                    dataset.DrawAll(glControl);
            }

            Debug.Assert(_datasets_visitedsystems != null);
            if (_datasets_visitedsystems != null)
            {
                foreach (var dataset in _datasets_visitedsystems)
                    dataset.DrawAll(glControl);
            }

            if (_starnameslist.Draw())          // rang out of bandwidth, ask for another paint
            {
                _requestrepaint = true;         // DONT invalidate.. this makes this thing go around and around and the main tick never gets a look in
            }

            if ( _starnamescomputed )            // done AFTER draw, so all stars from the queue between the foreground/background have been added to the master list   
            {
                _starnamescomputed = _starnamesbusy = false;             // okay, no longer busy, so we can do another estimate now..
            }

            Debug.Assert(_datasets_selectedsystems != null);
            if (_datasets_selectedsystems != null)
            {
                foreach (var dataset in _datasets_selectedsystems)
                    dataset.DrawAll(glControl);
            }

            Debug.Assert(_datasets_notedsystems != null);
            if (_datasets_notedsystems != null && showNoteMarksToolStripMenuItem.Checked)
            {
                foreach (var dataset in _datasets_notedsystems)                     // needs to be in order of background to foreground objects
                    dataset.DrawAll(glControl);
            }

            Debug.Assert(_datasets_bookedmarkedsystems != null);
            if (_datasets_bookedmarkedsystems != null && showBookmarksToolStripMenuItem.Checked)
            {
                foreach (var dataset in _datasets_bookedmarkedsystems)                     // needs to be in order of background to foreground objects
                    dataset.DrawAll(glControl);
            }
        }

        private void UpdateStatus()
        {
            statusLabel.Text = string.Format("x:{0,-6:0} y:{1,-6:0} z:{2,-6:0} Zoom:{3,-4:0.00} FOV:{4,-4:0} Use ? for help", posdir.Position.X, posdir.Position.Y, posdir.Position.Z, zoomfov.Zoom , zoomfov.FovDeg);
#if DEBUG
            statusLabel.Text += string.Format("   Direction x={0,-6:0.0} y={1,-6:0.0} z={2,-6:0.0} FPS {3,-6:0.0}", posdir.CameraDirection.X, posdir.CameraDirection.Y, posdir.CameraDirection.Z , fps);
#endif
        }


        #endregion

        #region Generate Data Sets

        private void GenerateDataSets()         // Called ONCE only during Load.. fixed data.
        {
            DatasetBuilder builder1 = new DatasetBuilder();
            _datasets_coarsegridlines = builder1.AddCoarseGridLines();

            DatasetBuilder builder2 = new DatasetBuilder();
            _datasets_finegridlines = builder2.AddFineGridLines();

            DatasetBuilder builder3 = new DatasetBuilder();
            _datasets_gridlinecoords = builder3.AddGridCoords();

            GenerateDataSetsBNG();
            UpdateDataSetsDueToZoomOrFlip(true);
        }

        private void UpdateDataSetsDueToZoomOrFlip(bool zoommoved)
        {
            //Console.WriteLine("Update due to zoom {0} or flip ", zoommoved);

            DatasetBuilder builder = new DatasetBuilder();

            if (zoommoved)                    // zoom affected objects only
            {
                if (toolStripButtonFineGrid.Checked)
                    builder.UpdateGridZoom(ref _datasets_coarsegridlines, zoomfov.Zoom);

                if (toolStripButtonCoords.Checked)
                    builder.UpdateGridCoordZoom(ref _datasets_gridlinecoords, zoomfov.Zoom);
            }

            builder.UpdateGalObjects(ref _datasets_galmapobjects, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), _lastcameranorm.Rotation);

            if (showBookmarksToolStripMenuItem.Checked)
                builder.UpdateBookmarks(ref _datasets_bookedmarkedsystems, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), _lastcameranorm.Rotation);

            if (showNoteMarksToolStripMenuItem.Checked)
                builder.UpdateBookmarks(ref _datasets_notedsystems, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), _lastcameranorm.Rotation);

            if (_clickedGMO != null || _clickedSystem != null)              // if markers
                builder.UpdateSelected(ref _datasets_selectedsystems, _clickedSystem, _clickedGMO, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), _lastcameranorm.Rotation);
        }

        private void GenerateDataSetsMaps()
        {
            DeleteDataset(ref _datasets_maps);
            _datasets_maps = null;

            FGEImage[] _selected = dropdownMapNames.DropDownItems.OfType<ToolStripButton>().Where(b => b.Checked).Select(b => b.Tag as FGEImage).ToArray();

            DatasetBuilder builder = new DatasetBuilder();
            _datasets_maps = builder.AddMapImages(_selected);
        }

        private void GenerateDataSetsVisitedSystems()
        {
            DeleteDataset(ref _datasets_visitedsystems);
            _datasets_visitedsystems = null;

            DatasetBuilder builder = new DatasetBuilder();

            List<VisitedSystemsClass> filtered = (_visitedSystems != null) ? _visitedSystems.Where(s => s.Time >= startTime && s.Time <= endTime).OrderBy(s => s.Time).ToList() : null;

            _datasets_visitedsystems = builder.BuildVisitedSystems(toolStripButtonDrawLines.Checked, filtered);
        }

        private void GenerateDataSetsRouteTri()
        {
            DeleteDataset(ref _datasets_routetri);
            _datasets_routetri = null;

            DatasetBuilder builder = new DatasetBuilder();

            _datasets_routetri = builder.BuildRouteTri(_centerSystem, _referenceSystems, _plannedRoute);
        }

        private void GenerateDataSetsSelectedSystems()
        {
            DeleteDataset(ref _datasets_selectedsystems);
            _datasets_selectedsystems = null;
            DatasetBuilder builder = new DatasetBuilder();
            _datasets_selectedsystems = builder.BuildSelected(_centerSystem, _clickedSystem, _clickedGMO, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), _lastcameranorm.Rotation);
        }

        private void GenerateDataSetsBNG()      // because the target is bound up with all three, best to do all three at once in ONE FUNCTION!
        {
            Bitmap maptarget = (Bitmap)EDDiscovery.Properties.Resources.bookmarktarget;
            Bitmap mapstar = (Bitmap)EDDiscovery.Properties.Resources.bookmarkgreen;
            Bitmap mapregion = (Bitmap)EDDiscovery.Properties.Resources.bookmarkyellow;
            Bitmap mapnotedbkmark = (Bitmap)EDDiscovery.Properties.Resources.bookmarkbrightred;
            Debug.Assert(mapnotedbkmark != null && maptarget != null);
            Debug.Assert(mapstar != null && mapregion != null);

            List<IData3DSet> oldbookmarks = _datasets_bookedmarkedsystems;
            DatasetBuilder builder1 = new DatasetBuilder();
            _datasets_bookedmarkedsystems = builder1.AddStarBookmarks(mapstar, mapregion, maptarget, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), _lastcameranorm.Rotation);
            DeleteDataset(ref oldbookmarks);

            List<IData3DSet> oldnotedsystems = _datasets_notedsystems;
            DatasetBuilder builder2 = new DatasetBuilder();
            _datasets_notedsystems = builder2.AddNotedBookmarks(mapnotedbkmark, maptarget, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), _lastcameranorm.Rotation, _visitedSystems);
            DeleteDataset(ref oldnotedsystems);

            DatasetBuilder builder3 = new DatasetBuilder();
            List<IData3DSet> oldgalmaps = _datasets_galmapobjects;
            _datasets_galmapobjects = builder3.AddGalMapObjectsToDataset(maptarget, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), _lastcameranorm.Rotation, _toolstripToggleNamingButton.Checked);
            DeleteDataset(ref oldgalmaps);

            DatasetBuilder builder4 = new DatasetBuilder();
            List<IData3DSet> oldgalreg = _datasets_galmapregions;
            _datasets_galmapregions = builder4.AddGalMapRegionsToDataset(_toolstripToggleRegionColouringButton.Checked);
            DeleteDataset(ref oldgalreg);

            if (_clickedGMO != null)              // if GMO marked.
                GenerateDataSetsSelectedSystems();          // as GMO marker is scaled and positioned so may need moving
        }

        private void DeleteDataset(ref List<IData3DSet> _datasets)
        {
            if (_datasets != null)
            {
                foreach (var ds in _datasets)
                {
                    if (ds is IDisposable)
                    {
                        ((IDisposable)ds).Dispose();
                    }
                }
            }
        }

        #endregion

        #region Set Position

        private void SetCenterSystemLabel()
        {
            if (_centerSystem != null)
                labelSystemCoords.Text = string.Format("{0} x:{1} y:{2} z:{3}", _centerSystem.name, _centerSystem.x.ToString("0.00"), _centerSystem.y.ToString("0.00"), _centerSystem.z.ToString("0.00"));
            else
                labelSystemCoords.Text = "No centre system";
        }

        public bool SetCenterSystemTo(VisitedSystemsClass system)
        {
            if (Is3DMapsRunning)
            {
                ISystem sys = system.curSystem;

                if (sys != null)
                {
                    return SetCenterSystemTo(sys);
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

        public bool SetCenterSystemTo(string name)
        {
            if (Is3DMapsRunning)                         // if null, we are not up and running
                return SetCenterSystemTo(FindSystem(name));
            else
                return false;
        }

        private bool SetCenterSystemTo(ISystem sys)
        {
            if (sys != null)
            {
                _centerSystem = sys;
                SetCenterSystemLabel();
                GenerateDataSetsSelectedSystems();
                posdir.StartCameraSlew(new Vector3((float)_centerSystem.x, (float)_centerSystem.y, (float)_centerSystem.z), -1F);
                return true;
            }
            else
                return false;
        }


        #endregion



        #region User Controls

        private void EndPicker_ValueChanged(object sender, EventArgs e)
        {
            endTime = endPicker.Value;
            GenerateDataSetsVisitedSystems();
            RequestRepaint();
        }

        private void StartPicker_ValueChanged(object sender, EventArgs e)
        {
            startTime = startPicker.Value;
            GenerateDataSetsVisitedSystems();
            RequestRepaint();
        }

        private void dropdownFilterHistory_Custom_Click(object sender, EventArgs e, ToolStripButton sel)
        {
            foreach (var item in dropdownFilterDate.DropDownItems.OfType<ToolStripButton>())
            {
                if (item != sel)
                {
                    item.Checked = false;
                }
            }

            if (startTime < startPicker.MinDate)
            {
                startTime = startPicker.MinDate;
            }

            SQLiteDBClass.PutSettingString("Map3DFilter", "Custom");                   // Custom is not saved, but clear last entry.
            startPickerHost.Visible = true;
            endPickerHost.Visible = true;
            startPicker.Value = startTime;
            endPicker.Value = endTime;
            GenerateDataSetsVisitedSystems();
            RequestRepaint();
        }

        private void dropdownFilterHistory_Item_Click(object sender, EventArgs e, ToolStripButton sel, Func<DateTime> startfunc, Func<DateTime> endfunc)
        {
            foreach (var item in dropdownFilterDate.DropDownItems.OfType<ToolStripButton>())
            {
                if (item != sel)
                {
                    item.Checked = false;
                }
            }
            SQLiteDBClass.PutSettingString("Map3DFilter", sel.Text);
            startTime = startfunc();
            endTime = endfunc == null ? DateTime.Now.AddDays(1) : endfunc();
            startPickerHost.Visible = false;
            endPickerHost.Visible = false;
            GenerateDataSetsVisitedSystems();
            RequestRepaint();
        }

        private void buttonLookAt_Click(object sender, EventArgs e)
        {
            CentreLook(false);
        }

        private void buttonCenter_Click(object sender, EventArgs e)
        {
            CentreLook(true);
        }

        void CentreLook(bool moveto )
        { 
            ISystem sys;
            GalacticMapObject gmo;
            Vector3 loc;

            string name = FindSystemOrGMO(textboxFrom.Text, out sys, out gmo, out loc);

            if (name != null)
            {
                textboxFrom.Text = name;        // normalise name (user may have different 

                if (sys != null && moveto)
                    SetCenterSystemTo(sys);
                else if (moveto)
                    posdir.StartCameraSlew(loc,-1F);
                else
                    posdir.CameraLookAt(loc,zoomfov.Zoom, 2F);
            }
            else
                MessageBox.Show("System or Object " + textboxFrom.Text + " not found");
        }

        private void toolStripButtonGoBackward_Click(object sender, EventArgs e)
        {
            if (_visitedSystems != null)
            {
                string name = VisitedSystemsClass.FindNextVisitedSystem(_visitedSystems, _centerSystem.name, -1, _centerSystem.name);
                SetCenterSystemTo(name);
            }
            else
                MessageBox.Show("No travel history is available");
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            SetCenterSystemTo(_homeSystem);
        }

        private void buttonHistory_Click(object sender, EventArgs e)
        {
            if (_historySelection == null)
                MessageBox.Show("No travel history is available");
            else
                SetCenterSystemTo(_historySelection);
        }

        private void toolStripButtonTarget_Click(object sender, EventArgs e)
        {
            string name;
            double x, y, z;

            if (TargetClass.GetTargetPosition(out name, out x, out y, out z))
            {
                posdir.StartCameraSlew(new Vector3((float)x, (float)y, (float)z),-1F);
            }
            else
            {
                MessageBox.Show("No target designated, create a bookmark or region mark, or use a Note mark, right click on it and set it as the target");
            }
        }
        
        private void toolStripButtonGoForward_Click(object sender, EventArgs e)
        {
            if (_visitedSystems != null)
            {
                string name = VisitedSystemsClass.FindNextVisitedSystem(_visitedSystems, _centerSystem.name, 1, _centerSystem.name);
                SetCenterSystemTo(name);
            }
            else
                MessageBox.Show("No travel history is available");
        }

        private void toolStripButtonAutoForward_Click(object sender, EventArgs e)
        {
        }

        private void toolStripLastKnownPosition_Click(object sender, EventArgs e)
        {
            if (_visitedSystems != null)
            {
                VisitedSystemsClass vs = _visitedSystems.FindLast(x => x.HasTravelCoordinates || (x.curSystem != null && x.curSystem.HasCoordinate));

                if (vs != null )
                    SetCenterSystemTo(FindSystem(vs.Name));
                else
                    MessageBox.Show("No stars with defined co-ordinates available in travel history");
            }
            else
                MessageBox.Show("No travel history is available");
        }

        private void toolStripButtonDrawLines_Click(object sender, EventArgs e)
        {
            GenerateDataSetsVisitedSystems();
            RequestRepaint();
        }

        private void showStarstoolStripMenuItem_Click(object sender, EventArgs e)
        {
            RequestRepaint();
        }

        private void showGalacticMapTypeMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tmsi = (ToolStripMenuItem)sender;

            if ( tmsi.Tag is int )
            {
                int v = (int)tmsi.Tag;
                if ( v == 0 )
                {
                    EDDiscoveryForm.galacticMapping.ToggleEnable();
                    
                    foreach (ToolStripMenuItem ti in toolStripDropDownButtonGalObjects.DropDownItems)
                    {
                        if (ti.Tag is GalMapType )
                            ti.Checked = ((GalMapType)ti.Tag).Enabled;
                    }
                }
            }
            else
            {
                EDDiscoveryForm.galacticMapping.ToggleEnable((GalMapType)tmsi.Tag);
            }

            GenerateDataSetsBNG();
            RequestRepaint();
        }

        private void enableColoursToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _stargrids.ForceWhite = !enableColoursToolStripMenuItem.Checked;
            RequestRepaint();
        }

        private void showStationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RequestRepaint();
        }

        private void toolStripButtonGrid_Click(object sender, EventArgs e)
        {
            RequestRepaint();
        }

        private void toolStripButtonFineGrid_Click(object sender, EventArgs e)
        {
            RequestRepaint();
        }

        private void toolStripButtonCoords_Click(object sender, EventArgs e)
        {
            RequestRepaint();
        }

        private void toolStripButtonEliteMovement_Click(object sender, EventArgs e)
        {
        }

        private void showDiscsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RequestRepaint();
        }

        private void showNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RequestRepaint();
        }

        private void showNoteMarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateDataSetsBNG();
            RequestRepaint();
        }

        private void showBookmarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateDataSetsBNG();
            RequestRepaint();
        }

        private void newRegionBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookmarkForm frm = new BookmarkForm();
            frm.InitialisePos(posdir.Position.X, posdir.Position.Y, posdir.Position.Z);
            DateTime tme = DateTime.Now;
            frm.RegionBookmark(tme.ToString());
            DialogResult res = frm.ShowDialog();

            if (res == DialogResult.OK)
            {
                BookmarkClass newcls = new BookmarkClass();

                newcls.Heading = frm.StarHeading;
                newcls.x = double.Parse(frm.x);
                newcls.y = double.Parse(frm.y);
                newcls.z = double.Parse(frm.z);
                newcls.Time = tme;
                newcls.Note = frm.Notes;
                newcls.Add();

                if (frm.IsTarget)          // asked for targetchanged..
                {
                    TargetClass.SetTargetBookmark("RM:" + newcls.Heading, newcls.id, newcls.x, newcls.y, newcls.z);
                    travelHistoryControl.RefreshTargetInfo();
                }

                GenerateDataSetsBNG();
                RequestRepaint();
            }
        }
        
        private void toolStripButtonPerspective_Click(object sender, EventArgs e)
        {
            if (!toolStripButtonPerspective.Checked)
                posdir.SetCameraDir(new Vector3(0, 0, 0));

            SetModelProjectionMatrix();

            RequestRepaint();
        }

        private void dotSystemCoords_Click(object sender, EventArgs e)
        {
            SetCenterSystemTo(_centerSystem);
        }

        private void dotSelectedSystemCoords_Click(object sender, EventArgs e)
        {
            if (_clickedSystem!=null)
                SetCenterSystemTo(_clickedSystem);
            else
                posdir.StartCameraSlew(_clickedposition,-1F);      // if nan, will ignore..
        }

        private void toolStripButtonHelp_Click(object sender, EventArgs e)
        {
            InfoForm dl = new InfoForm();
            string text = EDDiscovery.Properties.Resources.maphelp3d;
            dl.Info("3D Map Help", text, new Font("Microsoft Sans Serif", 10), new int[] { 50, 200, 400 });
            dl.Show();
        }

        private void dropdownMapNames_DropDownItemClicked(object sender, EventArgs e)
        {
            ToolStripButton tsb = (ToolStripButton)sender;
            SQLiteDBClass.PutSettingBool("map3DMaps" + tsb.Text, tsb.Checked);
            GenerateDataSetsMaps();
            RequestRepaint();
        }

        private void viewOnEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_clickedurl != null && _clickedurl.Length>0)
                System.Diagnostics.Process.Start(_clickedurl);
        }

        private void labelClickedSystemCoords_Click(object sender, EventArgs e)
        {
            if (_clickedSystem != null || !float.IsNaN(_clickedposition.X))
            {
                systemselectionMenuStrip.Show(labelClickedSystemCoords, 0, labelClickedSystemCoords.Height);
            }
        }

        void SetDropDownRecordImage()
        {
            if (maprecorder.InPlayBack)
                toolStripDropDownRecord.Image = (maprecorder.Paused) ? EDDiscovery.Properties.Resources.pauseblue : EDDiscovery.Properties.Resources.PlayNormal;
            else if (maprecorder.Recording)
                toolStripDropDownRecord.Image = (maprecorder.Paused) ? EDDiscovery.Properties.Resources.PauseNormalRed : EDDiscovery.Properties.Resources.RecordPressed;
            else
                toolStripDropDownRecord.Image = EDDiscovery.Properties.Resources.VideoRecorder;
        }

        private void recordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            maprecorder.ToggleRecord(false);
            SetDropDownRecordImage();
        }

        private void recordStepF5ToStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            maprecorder.ToggleRecord(true);
            SetDropDownRecordImage();
        }

        private void toolStripMenuItemClearRecording_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm you wish to clear the current recording", "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
                maprecorder.Clear();

            SetDropDownRecordImage();
        }

        private void playbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            maprecorder.TogglePlayBack();
            SetDropDownRecordImage();
        }

        private void pauseRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            maprecorder.TogglePause();
            SetDropDownRecordImage();
        }

        private void toolStripDropDownRecord_DropDownOpening(object sender, EventArgs e)
        {
            recordToolStripMenuItem.Text = maprecorder.Recording ? "Stop Recording (F5 to record a special step)" : maprecorder.Entries ? "Restart Recording" : "Start Recording";
            recordToolStripMenuItem.Image = maprecorder.Recording ? EDDiscovery.Properties.Resources.StopNormalRed : EDDiscovery.Properties.Resources.RecordPressed;
            recordToolStripMenuItem.Enabled = !maprecorder.InPlayBack && !maprecorder.RecordingStep;

            recordStepF5ToStepToolStripMenuItem.Text = maprecorder.Recording ? "Stop Recording (F5 to record a step)" : maprecorder.Entries ? "Restart Step Recording" : "Start Step Recording";
            recordStepF5ToStepToolStripMenuItem.Image = maprecorder.Recording ? EDDiscovery.Properties.Resources.StopNormalRed : EDDiscovery.Properties.Resources.RecordPressed;
            recordStepF5ToStepToolStripMenuItem.Enabled = !maprecorder.InPlayBack && !maprecorder.RecordingNormal;

            toolStripMenuItemClearRecording.Enabled = maprecorder.Entries;

            playbackToolStripMenuItem.Text = maprecorder.InPlayBack ? "Stop Playback" : "Start Playback";
            playbackToolStripMenuItem.Image = maprecorder.InPlayBack ? EDDiscovery.Properties.Resources.StopNormalBlue : EDDiscovery.Properties.Resources.PlayNormal;
            playbackToolStripMenuItem.Enabled = maprecorder.Entries;

            if (maprecorder.InPlayBack)
            {
                pauseRecordToolStripMenuItem.Text = maprecorder.Paused ? "Restart Playback" : "Pause Playback";
                pauseRecordToolStripMenuItem.Image = EDDiscovery.Properties.Resources.pauseblue;
            }
            else if (maprecorder.Recording)
            {
                pauseRecordToolStripMenuItem.Text = maprecorder.Paused ? "Restart Recording" : "Pause Recording";
                pauseRecordToolStripMenuItem.Image = EDDiscovery.Properties.Resources.PauseNormalRed;
            }
            else
            {
                pauseRecordToolStripMenuItem.Text = "Pause";
                pauseRecordToolStripMenuItem.Image = EDDiscovery.Properties.Resources.PauseNormalRed;
            }

            pauseRecordToolStripMenuItem.Enabled = maprecorder.Recording || maprecorder.InPlayBack;

            saveToFileToolStripMenuItem.Enabled = maprecorder.Entries;
        }

        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            maprecorder.SaveDialog();
        }

        private void LoadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            maprecorder.LoadDialog();
        }

        #endregion

        #region Mouse

        private void glControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            posdir.KillSlews();

            _mouseDownPos.X = e.X;
            _mouseDownPos.Y = e.Y;
            //Console.WriteLine("Mouseup down at " + e.X + "," + e.Y);

            if (e.Button.HasFlag(System.Windows.Forms.MouseButtons.Left))
            {
                _mouseStartRotate.X = e.X;
                _mouseStartRotate.Y = e.Y;
                //Console.WriteLine("Mouse start left");
            }

            if (e.Button.HasFlag(System.Windows.Forms.MouseButtons.Right))
            {
                _mouseStartTranslateXY.X = e.X;
                _mouseStartTranslateXY.Y = e.Y;
                _mouseStartTranslateXZ.X = e.X;
                _mouseStartTranslateXZ.Y = e.Y;
            }
        }

        private void glControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            bool notmovedmouse = Math.Abs(e.X - _mouseDownPos.X) + Math.Abs(e.Y - _mouseDownPos.Y) < 4;

            if (!notmovedmouse)     // if we moved it, its not a stationary click, ignore
                return;

            // system = cursystem!=null, curbookmark = null, notedsystem = false
            // bookmark on system, cursystem!=null, curbookmark != null, notedsystem = false
            // region bookmark. cursystem = null, curbookmark != null, notedsystem = false
            // clicked on note on a system, cursystem!=null,curbookmark=null, notedsystem=true
            // clicked on a gmo, gmo != null, rest null or false.

            ISystem cursystem = null;      
            BookmarkClass curbookmark = null;           
            bool notedsystem = false;
            GalacticMapObject gmo = null;             

            GetMouseOverItem(e.X, e.Y, out cursystem, out curbookmark, out notedsystem, out gmo);

            if (e.Button == System.Windows.Forms.MouseButtons.Left)            // left clicks associate with systems..
            {
                _mouseStartRotate = new Point(int.MinValue, int.MinValue);      // indicate finished .. we need to do this to make mousemove not respond to random stuff when resizing

                bool isanyselected = _clickedSystem != null || _clickedGMO != null;                // have we got a currently selected object

                _clickedGMO = gmo;
                _clickedSystem = cursystem;         // clicked system was found either by a bookmark looking up a name, or a position look up. best we can do.
                _clickedposition.X = float.NaN;
                _clickedurl = null;

                string name = null;

                if (_clickedSystem != null)         // will be set for systems clicked, bookmarks or noted systems
                {
                    _clickedposition = new Vector3((float)_clickedSystem.x, (float)_clickedSystem.y, (float)_clickedSystem.z);

                    name = _clickedSystem.name;

                    var edsm = new EDSM.EDSMClass();
                    _clickedurl = edsm.GetUrlToEDSMSystem(name, _clickedSystem.id_edsm);
                    viewOnEDSMToolStripMenuItem.Enabled = true;

                    System.Windows.Forms.Clipboard.SetText(_clickedSystem.name);
                }
                else if (curbookmark != null)                                   // region bookmark..
                {
                    _clickedposition = new Vector3((float)curbookmark.x, (float)curbookmark.y, (float)curbookmark.z);

                    name = (curbookmark.StarName != null) ? curbookmark.StarName : curbookmark.Heading;      // I know its only going to be a region bookmark, but heh

                    viewOnEDSMToolStripMenuItem.Enabled = false;
                }
                else if (gmo != null)
                {
                    _clickedposition = new Vector3((float)gmo.points[0].x, (float)gmo.points[0].y, (float)gmo.points[0].z);

                    name = gmo.name;

                    _clickedurl = gmo.galMapUrl;
                    viewOnEDSMToolStripMenuItem.Enabled = true;
                }

                if (name != null)       // this means we found something..
                    labelClickedSystemCoords.Text = string.Format("{0} x:{1} y:{2} z:{3}", name, _clickedposition.X.ToString("0.00"), _clickedposition.Y.ToString("0.00"), _clickedposition.Z.ToString("0.00"));
                else
                    labelClickedSystemCoords.Text = "None";

                if ( name != null || isanyselected)     // if we selected something, or something was selected
                {
                    GenerateDataSetsSelectedSystems();
                    RequestRepaint();
                }
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)                    // right clicks are about bookmarks.
            {
                _mouseStartTranslateXY = new Point(int.MinValue, int.MinValue);         // indicate rotation is finished.
                _mouseStartTranslateXZ = new Point(int.MinValue, int.MinValue);

                if (cursystem != null || curbookmark != null)      // if we have a system or a bookmark..
                {                                                   // try and find the associated bookmark..
                    BookmarkClass bkmark = (curbookmark != null) ? curbookmark : BookmarkClass.bookmarks.Find(x => x.StarName != null && x.StarName.Equals(cursystem.name));
                    string note = (cursystem != null) ? SystemNoteClass.GetSystemNote(cursystem.name) : null;   // may be null

                    BookmarkForm frm = new BookmarkForm();

                    if (notedsystem && bkmark == null)              // note on a system
                    {
                        long targetid = TargetClass.GetTargetNotedSystem();      // who is the target of a noted system (0=none)
                        long noteid = SystemNoteClass.GetSystemNoteClass(cursystem.name).id;

                        frm.InitialisePos(cursystem.x, cursystem.y, cursystem.z);
                        frm.NotedSystem(cursystem.name, note, noteid == targetid);       // note may be passed in null
                        frm.ShowDialog();

                        if ((frm.IsTarget && targetid != noteid) || (!frm.IsTarget && targetid == noteid)) // changed..
                        {
                            if (frm.IsTarget)
                                TargetClass.SetTargetNotedSystem(cursystem.name, noteid, cursystem.x, cursystem.y, cursystem.z);
                            else
                                TargetClass.ClearTarget();
                        }
                    }
                    else
                    {
                        bool regionmarker = false;
                        DateTime tme;

                        long targetid = TargetClass.GetTargetBookmark();      // who is the target of a bookmark (0=none)

                        if (bkmark == null)                         // new bookmark
                        {
                            frm.InitialisePos(cursystem.x, cursystem.y, cursystem.z);
                            tme = DateTime.Now;
                            frm.NewSystemBookmark(cursystem.name, note, tme.ToString());
                        }
                        else                                        // update bookmark
                        {
                            frm.InitialisePos(bkmark.x, bkmark.y, bkmark.z);
                            regionmarker = bkmark.isRegion;
                            tme = bkmark.Time;
                            frm.Update(regionmarker ? bkmark.Heading : bkmark.StarName, note, bkmark.Note, tme.ToString(), regionmarker, targetid == bkmark.id);
                        }

                        DialogResult res = frm.ShowDialog();

                        if (res == DialogResult.OK)
                        {
                            BookmarkClass newcls = new BookmarkClass();

                            if (regionmarker)
                                newcls.Heading = frm.StarHeading;
                            else
                                newcls.StarName = frm.StarHeading;

                            newcls.x = double.Parse(frm.x);
                            newcls.y = double.Parse(frm.y);
                            newcls.z = double.Parse(frm.z);
                            newcls.Time = tme;
                            newcls.Note = frm.Notes;

                            if (bkmark != null)
                            {
                                newcls.id = bkmark.id;
                                newcls.Update();
                            }
                            else
                                newcls.Add();

                            if ((frm.IsTarget && targetid != newcls.id) || (!frm.IsTarget && targetid == newcls.id)) // changed..
                            {
                                if (frm.IsTarget)
                                    TargetClass.SetTargetBookmark(regionmarker ? ("RM:" + newcls.Heading) : newcls.StarName, newcls.id, newcls.x, newcls.y, newcls.z);
                                else
                                    TargetClass.ClearTarget();
                            }
                        }
                        else if (res == DialogResult.Abort && bkmark != null)
                        {
                            if (targetid == bkmark.id)
                            {
                                TargetClass.ClearTarget();
                            }

                            bkmark.Delete();
                        }
                    }

                    travelHistoryControl.RefreshTargetInfo();       // because of all the ways it may have changed
                    GenerateDataSetsBNG();      // in case target changed, do all..
                    RequestRepaint();
                }
                else if (gmo != null)
                {
                    long targetid = TargetClass.GetTargetGMO();      // who is the target of a bookmark (0=none)

                    BookmarkForm frm = new BookmarkForm();

                    frm.Name = gmo.name;
                    frm.InitialisePos(gmo.points[0].x, gmo.points[0].y, gmo.points[0].z);
                    frm.GMO(gmo.name, gmo.description, targetid == gmo.id, gmo.galMapUrl);
                    DialogResult res = frm.ShowDialog();

                    if (res == DialogResult.OK)
                    {
                        if ((frm.IsTarget && targetid != gmo.id) || (!frm.IsTarget && targetid == gmo.id)) // changed..
                        {
                            if (frm.IsTarget)
                                TargetClass.SetTargetGMO("G:" + gmo.name, gmo.id, gmo.points[0].x, gmo.points[0].y, gmo.points[0].z);
                            else
                                TargetClass.ClearTarget();

                            GenerateDataSetsBNG();
                            RequestRepaint();
                            travelHistoryControl.RefreshTargetInfo();
                        }
                    }
                }
            }
        }

        private void glControl_DoubleClick(object sender, EventArgs e)
        {
            posdir.StartCameraSlew(_clickedposition,-1F);
        }

        private void glControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {                                                                   
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (_mouseStartRotate.X != int.MinValue) // on resize double click resize, we get a stray mousemove with left, so we need to make sure we actually had a down event
                {
                    posdir.KillSlews();
                    //Console.WriteLine("Mouse move left");
                    int dx = e.X - _mouseStartRotate.X;
                    int dy = e.Y - _mouseStartRotate.Y;

                    _mouseStartRotate.X = _mouseStartTranslateXZ.X = e.X;
                    _mouseStartRotate.Y = _mouseStartTranslateXZ.Y = e.Y;
                    //System.Diagnostics.Trace.WriteLine("dx" + dx.ToString() + " dy " + dy.ToString() + " Button " + e.Button.ToString());

                    posdir.RotateCameraDir(new Vector3((float)(-dy / 4.0f), (float)(dx / 4.0f), 0));
                }

                _mousehovertick.Stop();
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (_mouseStartTranslateXY.X != int.MinValue)
                {
                    posdir.KillSlews();

                    int dx = e.X - _mouseStartTranslateXY.X;
                    int dy = e.Y - _mouseStartTranslateXY.Y;

                    _mouseStartTranslateXY.X = _mouseStartTranslateXZ.X = e.X;
                    _mouseStartTranslateXY.Y = _mouseStartTranslateXZ.Y = e.Y;
                    //System.Diagnostics.Trace.WriteLine("dx" + dx.ToString() + " dy " + dy.ToString() + " Button " + e.Button.ToString());

                    posdir.MoveCameraPos(new Vector3(0, -dy * (1.0f / zoomfov.Zoom) * 2.0f, 0));
                }

                _mousehovertick.Stop();
            }
            else if (e.Button == (System.Windows.Forms.MouseButtons.Left | System.Windows.Forms.MouseButtons.Right))
            {
                if (_mouseStartTranslateXZ.X != int.MinValue)
                {
                    posdir.KillSlews();

                    int dx = e.X - _mouseStartTranslateXZ.X;
                    int dy = e.Y - _mouseStartTranslateXZ.Y;

                    _mouseStartTranslateXZ.X = _mouseStartRotate.X = _mouseStartTranslateXY.X = e.X;
                    _mouseStartTranslateXZ.Y = _mouseStartRotate.Y = _mouseStartTranslateXY.Y = e.Y;
                    //System.Diagnostics.Trace.WriteLine("dx" + dx.ToString() + " dy " + dy.ToString() + " Button " + e.Button.ToString());

                    Matrix4 transform = Matrix4.CreateRotationZ((float)(-posdir.CameraDirection.Y * Math.PI / 180.0f));
                    Vector3 translation = new Vector3(-dx * (1.0f / zoomfov.Zoom) * 2.0f, dy * (1.0f / zoomfov.Zoom) * 2.0f, 0.0f);
                    translation = Vector3.Transform(translation, transform);

                    posdir.MoveCameraPos(new Vector3(translation.X,0, translation.Y));
                }

                _mousehovertick.Stop();
            }
            else //
            {
                if (Math.Abs(e.X - _mouseHover.X) + Math.Abs(e.Y - _mouseHover.Y) > 8)
                    KillHover();                                // we move we kill the hover.

                                                                // no tool tip, not slewing, not ticking..
                if (_mousehovertooltip == null && !posdir.InSlews && !_mousehovertick.Enabled)
                {
                    //Console.WriteLine("{0} Start tick", Environment.TickCount);
                    _mouseHover = e.Location;
                    _mousehovertick.Start();
                }
            }
        }

        void KillHover()
        {
            if (_mousehovertooltip != null)                                 // kill the tool tip
            {
                //Console.WriteLine("{0} kill tool tip move mouse", Environment.TickCount);
                _mousehovertooltip.Dispose();
                _mousehovertooltip = null;
            }

            //Console.WriteLine("{0} kill hover", Environment.TickCount);
            _mousehovertick.Stop();
        }

        void MouseHoverTick(object sender, EventArgs e)
        {
            _mousehovertick.Stop();

            //Console.WriteLine("{0} Hover tick tripped slew {1}", Environment.TickCount, _cameraSlewProgress);

            ISystem hoversystem = null;
            BookmarkClass curbookmark = null;
            bool notedsystem = false;
            GalacticMapObject gmo = null;
            GetMouseOverItem(_mouseHover.X, _mouseHover.Y, out hoversystem, out curbookmark, out notedsystem, out gmo);

            string info = null, sysname = null;
            double xp = 0, yp = 0, zp = 0;

            if (hoversystem != null)
            {
                sysname = hoversystem.name;
                info = hoversystem.name + Environment.NewLine + string.Format("x:{0} y:{1} z:{2}", hoversystem.x.ToString("0.00"), hoversystem.y.ToString("0.00"), hoversystem.z.ToString("0.00"));
                xp = hoversystem.x;
                yp = hoversystem.y;
                zp = hoversystem.z;

                SystemClass sysclass = (hoversystem.id != 0) ? SystemClass.GetSystem(hoversystem.id) : SystemClass.GetSystem(hoversystem.name);

                if (sysclass != null)
                {
                    if (sysclass.allegiance != EDAllegiance.Unknown)
                        info += Environment.NewLine + "Allegiance: " + sysclass.allegiance;

                    if (sysclass.primary_economy != EDEconomy.Unknown)
                        info += Environment.NewLine + "Economy: " + sysclass.primary_economy;

                    if (sysclass.government != EDGovernment.Unknown)
                        info += Environment.NewLine + "Government: " + sysclass.allegiance;

                    if (sysclass.state != EDState.Unknown)
                        info += Environment.NewLine + "State: " + sysclass.state;

                    if (sysclass.allegiance != EDAllegiance.Unknown)
                        info += Environment.NewLine + "Allegiance: " + sysclass.allegiance;
                }

                if (hoversystem.population != 0)
                    info += Environment.NewLine + "Population: " + hoversystem.population;
            }
            else if (curbookmark != null && curbookmark.Heading != null)     // region bookmark (second check should be redundant but its protection).
            {
                info = curbookmark.Heading + Environment.NewLine + string.Format("x:{0} y:{1} z:{2}", curbookmark.x.ToString("0.00"), curbookmark.y.ToString("0.00"), curbookmark.z.ToString("0.00"));
                sysname = "<<Never match string! to make the comparison fail";
                xp = curbookmark.x;
                yp = curbookmark.y;
                zp = curbookmark.z;
            }
            else if ( gmo != null )
            {
                xp = gmo.points[0].x;
                yp = gmo.points[0].y;
                zp = gmo.points[0].z;
                info = gmo.name + Environment.NewLine + gmo.galMapType.Description + Environment.NewLine + Tools.WordWrap(gmo.description,60) + Environment.NewLine +
                    string.Format("x:{0} y:{1} z:{2}", xp.ToString("0.00"), yp.ToString("0.00"), zp.ToString("0.00"));
                sysname = "<<Never match string! to make the comparison fail";
            }
            
            if ( sysname != null )
            { 
                if (!sysname.Equals(_centerSystem.name))
                {
                    double distcsn = Math.Sqrt((xp - _centerSystem.x) * (xp - _centerSystem.x) + (yp - _centerSystem.y) * (yp - _centerSystem.y) + (zp - _centerSystem.z) * (zp - _centerSystem.z));
                    info += Environment.NewLine + "Distance from " + _centerSystem.name + ": " + distcsn.ToString("0.0");
                }
                // if exists, history not hover, history not centre
                if (_historySelection != null && !sysname.Equals(_historySelection.name) && !_historySelection.name.Equals(_centerSystem.name))
                {
                    double disthist = Math.Sqrt((xp - _historySelection.x) * (xp - _historySelection.x) + (yp - _historySelection.y) * (yp - _historySelection.y) + (zp - _historySelection.z) * (zp - _historySelection.z));
                    info += Environment.NewLine + "Distance from " + _historySelection.name + ": " + disthist.ToString("0.0");
                }
                // home not centre, home not history or history null
                if (!_homeSystem.name.Equals(_centerSystem.name) && (_historySelection == null || !_historySelection.name.Equals(_homeSystem.name)))
                {
                    double disthome = Math.Sqrt((xp - _homeSystem.x) * (xp - _homeSystem.x) + (yp - _homeSystem.y) * (yp - _homeSystem.y) + (zp - _homeSystem.z) * (zp - _homeSystem.z));
                    info += Environment.NewLine + "Distance from " + _homeSystem.name + ": " + disthome.ToString("0.0");
                }

                string note = SystemNoteClass.GetSystemNote(sysname);   // may be null
                if (note != null && note.Trim().Length>0 )
                {
                    info += Environment.NewLine + "Notes: " + note.Trim();
                }

                if (curbookmark != null && curbookmark.Note != null && curbookmark.Note.Trim().Length>0 )
                    info += Environment.NewLine + "Bookmark Notes: " + curbookmark.Note.Trim();

                _mousehovertooltip = new System.Windows.Forms.ToolTip();
                _mousehovertooltip.InitialDelay = 0;
                _mousehovertooltip.AutoPopDelay = 30000;
                _mousehovertooltip.ReshowDelay = 0;
                _mousehovertooltip.IsBalloon = true;
                _mousehovertooltip.SetToolTip(glControl, info);
   
            }
        }

        private void glControl_OnMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var kbdstate = OpenTK.Input.Keyboard.GetState();

            if (e.Delta != 0)
            {
                if (kbdstate[Key.LControl] || kbdstate[Key.RControl])
                {
                    if (zoomfov.ChangeFov(e.Delta < 0))
                    {
                        SetModelProjectionMatrix();
                        RequestRepaint();
                    }
                }
                else
                {
                    zoomfov.ChangeZoom(e.Delta > 0);
                }
            }
        }


#endregion

        #region FindObjectsOnScreen

        private bool GetPixel(Vector4d xyzw, ref Matrix4d resmat, ref Vector2d pixelpos, out double newcursysdistz)
        {
            Vector4d sysloc = Vector4d.Transform(xyzw, resmat);

            double w2 = glControl.Width / 2.0;
            double h2 = glControl.Height / 2.0;

            if (sysloc.Z > _znear)
            {
                pixelpos = new Vector2d(((sysloc.X / sysloc.W) + 1.0) * w2, ((sysloc.Y / sysloc.W) + 1.0) * h2);
                newcursysdistz = Math.Abs(sysloc.Z * zoomfov.Zoom);

                return true;
            }

            newcursysdistz = 0;
            return false;
        }

        private bool IsWithinRectangle( Matrix4d area ,  int x, int y, out double newcursysdistz, ref Matrix4d resmat )
        {
            Vector2d ptopleft = new Vector2d(0, 0), pbottomright = new Vector2d(0, 0);
            Vector2d pbottomleft = new Vector2d(0, 0), ptopright = new Vector2d(0, 0);
            double ztopleft, zbottomright,zbottomleft,ztopright;

            if (GetPixel(area.Row0, ref resmat, ref ptopleft, out ztopleft) &&
                GetPixel(area.Row1, ref resmat, ref ptopright, out ztopright) &&
                GetPixel(area.Row2, ref resmat, ref pbottomright, out zbottomright) && 
                GetPixel(area.Row3, ref resmat, ref pbottomleft, out zbottomleft ) )
            {
                //    Console.WriteLine("Row0 {0},{1}", ptopleft.X, ptopleft.Y); Console.WriteLine("Row1 {0},{1}", ptopright.X, ptopright.Y); Console.WriteLine("Row2 {0},{1}", pbottomright.X, pbottomright.Y); Console.WriteLine("Row3 {0},{1}", pbottomleft.X, pbottomleft.Y);  Console.WriteLine("x,y {0},{1} {2},{3}", x, y , x-pbottomleft.X, y-pbottomright.Y);

                GraphicsPath p = new GraphicsPath();            // a moment of inspiration, use the graphics path for the polygon hit test!
                p.AddLine(new PointF((float)ptopleft.X, (float)ptopleft.Y), new PointF((float)ptopright.X, (float)ptopright.Y));
                p.AddLine(new PointF((float)pbottomright.X, (float)pbottomright.Y), new PointF((float)pbottomleft.X, (float)pbottomleft.Y));
                p.CloseFigure();

                if ( p.IsVisible( new PointF(x,y) ) )
                {
                    newcursysdistz = ztopleft;
                    return true;
                }
            }

            newcursysdistz = 0;
            return false;
        }

        private float GetBitmapOnScreenSizeX() { return (float)Math.Min(Math.Max(2, 80.0 / zoomfov.Zoom), 1000); }
        private float GetBitmapOnScreenSizeY() { return (float)Math.Min(Math.Max(2, 100.0 / zoomfov.Zoom), 1000); }

        private BookmarkClass GetMouseOverBookmark(int x, int y, out double cursysdistz)
        {
            x = Math.Min(Math.Max(x, 5), glControl.Width - 5);
            y = Math.Min(Math.Max(glControl.Height - y, 5), glControl.Height - 5);

            Vector3d[] rotvert = TexturedQuadData.GetVertices(new Vector3d(0, 0, 0), _lastcameranorm.Rotation, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), 0, GetBitmapOnScreenSizeY() / 2);

            BookmarkClass curbk = null;
            cursysdistz = double.MaxValue;
            Matrix4d resmat = posdir.GetResMatd;

            foreach (BookmarkClass bc in BookmarkClass.bookmarks)
            {
                //Console.WriteLine("Checking bookmark " + ((bc.Heading != null) ? bc.Heading : bc.StarName));

                Matrix4d area = new Matrix4d(
                    new Vector4d(rotvert[0].X + bc.x, rotvert[0].Y + bc.y, rotvert[0].Z + bc.z, 1),    // top left
                    new Vector4d(rotvert[1].X + bc.x, rotvert[1].Y + bc.y, rotvert[1].Z + bc.z, 1),    
                    new Vector4d(rotvert[2].X + bc.x, rotvert[2].Y + bc.y, rotvert[2].Z + bc.z, 1),    
                    new Vector4d(rotvert[3].X + bc.x, rotvert[3].Y + bc.y, rotvert[3].Z + bc.z, 1)    // bot left
                    );

                //Console.WriteLine("{0},{1},{2}, {3},{4},{5} vs {6},{7}" , area.Row0.X , area.Row0.Y, area.Row0.Z, area.Row2.X, area.Row2.Y , area.Row2.Z , x,y);

                double newcursysdistz;
                if (IsWithinRectangle(area, x, y, out newcursysdistz, ref resmat))
                {
                    if (newcursysdistz < cursysdistz)
                    {
                        cursysdistz = newcursysdistz;
                        curbk = bc;
                    }
                }
            }

            return curbk;
        }

        private SystemClass GetMouseOverNotedSystem(int x, int y, out double cursysdistz )
        {
            x = Math.Min(Math.Max(x, 5), glControl.Width - 5);
            y = Math.Min(Math.Max(glControl.Height - y, 5), glControl.Height - 5);

            Vector3d[] rotvert = TexturedQuadData.GetVertices(new Vector3d(0, 0, 0), _lastcameranorm.Rotation, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), 0, GetBitmapOnScreenSizeY() / 2);

            SystemClass cursys = null;
            cursysdistz = double.MaxValue;

            if (_visitedSystems == null)
                return null;

            Matrix4d resmat = posdir.GetResMatd;

            foreach (VisitedSystemsClass vs in _visitedSystems)
            {
                SystemNoteClass notecs = SystemNoteClass.GetSystemNoteClass(vs.Name);

                if (notecs!=null)
                {
                    string note = notecs.Note.Trim();

                    if (note.Length > 0)
                    {
                        double lx = (vs.HasTravelCoordinates) ? vs.X : vs.curSystem.x;
                        double ly = (vs.HasTravelCoordinates) ? vs.Y : vs.curSystem.y;
                        double lz = (vs.HasTravelCoordinates) ? vs.Z : vs.curSystem.z;

                        Matrix4d area = new Matrix4d(
                            new Vector4d(rotvert[0].X + lx, rotvert[0].Y + ly, rotvert[0].Z + lz, 1),    // top left
                            new Vector4d(rotvert[1].X + lx, rotvert[1].Y + ly, rotvert[1].Z + lz, 1),
                            new Vector4d(rotvert[2].X + lx, rotvert[2].Y + ly, rotvert[2].Z + lz, 1),
                            new Vector4d(rotvert[3].X + lx, rotvert[3].Y + ly, rotvert[3].Z + lz, 1)    // bot left
                            );

                        double newcursysdistz;
                        if (IsWithinRectangle(area, x, y, out newcursysdistz,ref resmat))
                        { 
                            if (newcursysdistz < cursysdistz)
                            {
                                cursysdistz = newcursysdistz;
                                cursys = (SystemClass)vs.curSystem;
                            }
                        }
                    }
                }

            }

            return cursys;
        }

        private GalacticMapObject GetMouseOverGalaxyObject(int x, int y, out double cursysdistz)
        {
            x = Math.Min(Math.Max(x, 5), glControl.Width - 5);
            y = Math.Min(Math.Max(glControl.Height - y, 5), glControl.Height - 5);

            Vector3d[] rotvert = TexturedQuadData.GetVertices(new Vector3d(0, 0, 0), _lastcameranorm.Rotation, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY());

            cursysdistz = double.MaxValue;
            Matrix4d resmat = posdir.GetResMatd;
            GalacticMapObject curobj = null;

            if (EDDiscoveryForm.galacticMapping != null)
            {
                foreach (GalacticMapObject gmo in EDDiscoveryForm.galacticMapping.galacticMapObjects)
                {
                    PointData pd = (gmo.points.Count > 0) ? gmo.points[0] : null;     // lets be paranoid

                    if (gmo.galMapType.Enabled && gmo.galMapType.Group == GalMapType.GalMapGroup.Markers && pd != null)             // if it is Enabled and has a co-ord, and is a marker type (routes/regions rejected)
                    {
                        Matrix4d area = new Matrix4d(
                            new Vector4d(rotvert[0].X + pd.x, rotvert[0].Y + pd.y, rotvert[0].Z + pd.z, 1),    // top left
                            new Vector4d(rotvert[1].X + pd.x, rotvert[1].Y + pd.y, rotvert[1].Z + pd.z, 1),
                            new Vector4d(rotvert[2].X + pd.x, rotvert[2].Y + pd.y, rotvert[2].Z + pd.z, 1),
                            new Vector4d(rotvert[3].X + pd.x, rotvert[3].Y + pd.y, rotvert[3].Z + pd.z, 1)    // bot left
                            );

                        double newcursysdistz;
                        if (IsWithinRectangle(area, x, y, out newcursysdistz, ref resmat))
                        {
                            if (newcursysdistz < cursysdistz)
                            {
                                cursysdistz = newcursysdistz;
                                curobj = gmo;
                            }
                        }
                    }
                }
            }

            return curobj;
        }

        private ISystem GetMouseOverSystem(int x, int y, out double cursysdistz)
        {
            x = Math.Min(Math.Max(x, 5), glControl.Width - 5);
            y = Math.Min(Math.Max(glControl.Height - y, 5), glControl.Height - 5);

            StarGrid.TransFormInfo ti = new StarGrid.TransFormInfo(posdir.GetResMatd, _znear, glControl.Width, glControl.Height, zoomfov.Zoom);

            Vector3? posofsystem = _stargrids.FindOverSystem(x, y, out cursysdistz, ti, showStarstoolStripMenuItem.Checked, showStationsToolStripMenuItem.Checked);

            if ( posofsystem == null )
                posofsystem = _starnameslist.FindOverSystem(x, y, out cursysdistz, ti); // in case these are showing

            ISystem f = null;

//TBD try visited history list...

            if (posofsystem != null)
                f = FindSystem(new Vector3(posofsystem.Value.X, posofsystem.Value.Y, posofsystem.Value.Z));

            return f;
        }

        // system = cursystem!=null, curbookmark = null, notedsystem = false
        // bookmark on system, cursystem!=null, curbookmark != null, notedsystem = false
        // region bookmark. cursystem = null, curbookmark != null, notedsystem = false
        // clicked on note on a system, cursystem!=null,curbookmark=null, notedsystem=true
        // clicked on gal object, galmapobject !=null, cursystem=null,curbookmark=null,notedsystem = false

        private void GetMouseOverItem(int x, int y, out ISystem cursystem,  // can return both, if a system bookmark is clicked..
                                                    out BookmarkClass curbookmark, out bool notedsystem,
                                                    out GalacticMapObject galobj)
        {
            cursystem = null;
            curbookmark = null;
            notedsystem = false;
            galobj = null;

            if (_datasets_bookedmarkedsystems != null)              // only if bookedmarked is shown
            {
                double curdistbookmark;
                curbookmark = GetMouseOverBookmark(x, y, out curdistbookmark);       

                if (curbookmark != null)
                {
                    if ( curbookmark.StarName != null )            // if starname set, see if we can find it
                        cursystem = FindSystem(curbookmark.StarName);       // find, either in visited system, or in db

                    return;
                }
            }

            if (_datasets_notedsystems != null)
            {
                double curdistnoted;
                cursystem = GetMouseOverNotedSystem(x, y, out curdistnoted);

                if ( cursystem != null )
                { 
                    notedsystem = true;
                    return;
                }
            }

            double curdistgalmap;
            galobj = GetMouseOverGalaxyObject(x, y, out curdistgalmap);
            if (galobj != null)
                return;

            double curdistsystem;
            cursystem = GetMouseOverSystem(x, y, out curdistsystem);
        }

        #endregion

        #region Misc

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

        public ISystem FindSystem(string name, SQLiteConnectionED cn = null)    // nice wrapper for this
        {
            if (_visitedSystems != null)
            {
                VisitedSystemsClass sys = _visitedSystems.FindLast(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

                if (sys != null)
                    return sys.curSystem;
            }

            ISystem isys = SystemClass.GetSystem(name,cn);
            return isys;
        }

        public ISystem FindSystem(Vector3 pos, SQLiteConnectionED cn = null )
        {
            if (_visitedSystems != null)
            {
                VisitedSystemsClass vsc = VisitedSystemsClass.FindByPos(_visitedSystems, new EMK.LightGeometry.Point3D(pos.X, pos.Y, pos.Z), 0.1);

                if (vsc != null)
                    return vsc.curSystem;
            }

            return SystemClass.FindNearestSystem(pos.X, pos.Y, pos.Z, false, 0.1,cn);
        }

        private ISystem SafeSystem(ISystem s)
        {
            if (s == null)
            {
                s = FindSystem("Sol");

                if (s == null)
                    s = new SystemClass("Sol", 0, 0, 0);
            }

            return s;
        }

        public string FindSystemOrGMO(string name, out ISystem sys, out GalacticMapObject gmo , out Vector3 loc )
        {
            gmo = null;
            sys = FindSystem(textboxFrom.Text);
            loc = new Vector3(0, 0, 0);

            if (sys != null)
            {
                loc = new Vector3((float)sys.x, (float)sys.y, (float)sys.z);
                return sys.name;
            }

            gmo = EDDiscoveryForm.galacticMapping.Find(textboxFrom.Text, true, true);    // ignore if its off, find any part of string, find if disabled

            if ( gmo != null )
            {
                loc = new Vector3((float)gmo.points[0].x, (float)gmo.points[0].y, (float)gmo.points[0].z);
                return gmo.name;
            }

            return null;
        }

        #endregion


        #region Map Images

        private void LoadMapImages()
        {
            string datapath = System.IO.Path.Combine(Tools.GetAppDataDirectory(), "Maps");
            if (System.IO.Directory.Exists(datapath))
            {
                fgeimages = FGEImage.LoadImages(datapath);
                fgeimages.AddRange(FGEImage.LoadFixedImages(datapath));
            }

            dropdownMapNames.DropDownItems.Clear();

            foreach (var img in fgeimages)
            {
                ToolStripButton item = new ToolStripButton
                {
                    Text = img.FileName,
                    CheckOnClick = true,
                    DisplayStyle = ToolStripItemDisplayStyle.Text,
                    Tag = img
                };
                item.Click += new EventHandler(dropdownMapNames_DropDownItemClicked);
                item.Checked = SQLiteDBClass.GetSettingBool("map3DMaps" + img.FileName, false);
                dropdownMapNames.DropDownItems.Add(item);
            }
        }

        public void AddExpedition(string name, Func<DateTime> starttime, Func<DateTime> endtime)
        {
            _AddExpedition(name, starttime, endtime);
        }

        public void AddExpedition(string name, DateTime starttime, DateTime? endtime)
        {
            _AddExpedition(name, () => starttime, endtime == null ? null : new Func<DateTime>(() => (DateTime)endtime));
        }

        private ToolStripButton _AddExpedition(string name, Func<DateTime> starttime, Func<DateTime> endtime)
        {
            var item = new ToolStripButton
            {
                Text = name,
                CheckOnClick = true,
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };
            item.Click += (s, e) => dropdownFilterHistory_Item_Click(s, e, item, starttime, endtime);
            dropdownFilterDate.DropDownItems.Add(item);
            return item;
        }

        private void FillExpeditions()
        {
            Dictionary<string, Func<DateTime>> starttimes = new Dictionary<string, Func<DateTime>>()
            {
                { "All", () => new DateTime(2010, 1, 1, 0, 0, 0) },
                { "Last Week", () => DateTime.Now.AddDays(-7) },
                { "Last Month", () => DateTime.Now.AddMonths(-1) },
                { "Last Year", () => DateTime.Now.AddYears(-1) }
            };

            Dictionary<string, Func<DateTime>> endtimes = new Dictionary<string, Func<DateTime>>();

            foreach (var expedition in SavedRouteClass.GetAllSavedRoutes())
            {
                if (expedition.StartDate != null)
                {
                    var starttime = (DateTime)expedition.StartDate;
                    starttimes[expedition.Name] = () => starttime;

                    if (expedition.EndDate != null)
                    {
                        var endtime = (DateTime)expedition.EndDate;
                        endtimes[expedition.Name] = () => endtime;
                    }
                }
                else if (expedition.EndDate != null)
                {
                    var endtime = (DateTime)expedition.EndDate;
                    endtimes[expedition.Name] = () => endtime;
                    starttimes[expedition.Name] = starttimes["All"];
                }
            }

            startTime = starttimes["All"]();
            endTime = DateTime.Now.AddDays(1);

            string lastsel = SQLiteDBClass.GetSettingString("Map3DFilter", "");
            foreach (var kvp in starttimes)
            {
                var name = kvp.Key;
                var startfunc = kvp.Value;
                var endfunc = endtimes.ContainsKey(name) ? endtimes[name] : () => DateTime.Now.AddDays(1);
                var item = _AddExpedition(name, startfunc, endfunc);

                if (item.Text.Equals(lastsel))              // if a standard one, restore.  WE are not saving custom.
                {                                           // if custom is selected, we don't restore a tick.
                    item.Checked = true;
                    startTime = startfunc();
                    endTime = endfunc();
                }
            }

            var citem = new ToolStripButton
            {
                Text = "Custom",
                CheckOnClick = true,
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };
            citem.Click += (s, e) => dropdownFilterHistory_Custom_Click(s, e, citem);

            dropdownFilterDate.DropDownItems.Add(citem);

            startPicker = new DateTimePicker();
            endPicker = new DateTimePicker();
            startPicker.ValueChanged += StartPicker_ValueChanged;
            endPicker.ValueChanged += EndPicker_ValueChanged;
            startPickerHost = new ToolStripControlHost(startPicker) { Visible = false };
            endPickerHost = new ToolStripControlHost(endPicker) { Visible = false };
            startPickerHost.Size = new Size(150, 20);
            endPickerHost.Size = new Size(150, 20);
            toolStripShowAllStars.Items.Add(startPickerHost);
            toolStripShowAllStars.Items.Add(endPickerHost);
        }

        #endregion


    }



}

