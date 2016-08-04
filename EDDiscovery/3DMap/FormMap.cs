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

namespace EDDiscovery2
{
    public partial class FormMap : Form
    {
        #region Variables

        public bool noWindowReposition { get; set; } = false;                       // set externally
        public TravelHistoryControl travelHistoryControl { get; set; } = null;      // set externally

        const int HELP_VERSION = 2;         // increment this to force help onto the screen of users first time.

        public EDDConfig.MapColoursClass MapColours { get; set; } = EDDConfig.Instance.MapColours;

        private List<IData3DSet> _datasets_finegridlines;
        private List<IData3DSet> _datasets_coarsegridlines;
        private List<IData3DSet> _datasets_gridlinecoords;
        private List<IData3DSet> _datasets_maps;
        private List<IData3DSet> _datasets_poi;
        private List<IData3DSet> _datasets_selectedsystems;
        private List<IData3DSet> _datasets_visitedsystems;
        private List<IData3DSet> _datasets_bookedmarkedsystems;
        private List<IData3DSet> _datasets_notedsystems;

        private const double ZoomMax = 500;
        private const double ZoomMin = 0.01;
        private const double ZoomFact = 1.2589254117941672104239541063958;
        private const double CameraSlewTime = 1.0;

        StarGrids _stargrids;

        //        List<SystemClassStarNames> _starnames = null;    // star list combines data base and travelled h
        //        Dictionary<string, SystemClassStarNames> _starnamelookup; // and a dictionary to above since its so slow to do a search

        private AutoCompleteStringCollection _systemNames;
        private ISystem _centerSystem;
        private ISystem _homeSystem;

        private ISystem _clickedSystem;        // left clicked on a system/bookmark system/noted system
        private Vector3 _clickedposition;                   // left clicked on a position

        private ISystem _historySelection;
        private bool _loaded = false;

        private float _zoom = 1.0f;

        private Vector3 _cameraPos = Vector3.Zero;
        private Vector3 _cameraDir = Vector3.Zero;

        private Vector3 _cameraActionMovement = Vector3.Zero;
        private Vector3 _cameraActionRotation = Vector3.Zero;
        private float _cameraFov = (float)(Math.PI / 2.0f);
        private float _cameraSlewProgress = 1.0f;
        private Vector3 _cameraSlewPosition;

        private KeyboardActions _kbdActions = new KeyboardActions();
        private long _oldTickCount = DateTime.Now.Ticks / 10000;
        private int _ticks = 0;

        StarNamesList _starnameslist;
        Timer _starnametimer = new Timer();

        private Point _mouseStartRotate;
        private Point _mouseStartTranslateXY;
        private Point _mouseStartTranslateXZ;
        private Point _mouseDownPos;
        private Point _mouseHover;
        Timer _mousehovertick = new Timer();
        System.Windows.Forms.ToolTip _mousehovertooltip = null;


        private float _defaultZoom;
        private List<SystemClass> _referenceSystems { get; set; }
        public List<VisitedSystemsClass> _visitedSystems { get; set; }
        private List<SystemClass> _plannedRoute { get; set; }

        public List<FGEImage> fgeimages = new List<FGEImage>();
        public List<FGEImage> selectedmaps = new List<FGEImage>();

        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        private DateTimePicker startPicker;
        private DateTimePicker endPicker;
        private ToolStripControlHost startPickerHost;
        private ToolStripControlHost endPickerHost;

        private float _znear;
        private bool _timerRunning;

        private bool _isActivated = false;

        public bool Is3DMapsRunning { get { return _stargrids != null;  } }

        #endregion

        #region Initialisation

        public FormMap()
        {
            InitializeComponent();
        }

        public void Prepare(VisitedSystemsClass historysel, string homesys, ISystem centersys, float zoom,
                                AutoCompleteStringCollection sysname, List<VisitedSystemsClass> visited)
        {
            _visitedSystems = visited;
            Prepare((historysel!= null) ? FindSystem(historysel.Name):null , homesys, centersys, zoom, sysname, visited);
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

            _defaultZoom = zoom;

            _referenceSystems = null;
            _plannedRoute = null;

            SetCenterSystemTo(_centerSystem, true);             // move to this..

            ResetCamera();
            toolStripShowAllStars.Renderer = new MyRenderer();
            toolStripButtonDrawLines.Checked = SQLiteDBClass.GetSettingBool("Map3DDrawLines", true);
            showStarstoolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool("Map3DAllStars", true);
            showStationsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool("Map3DButtonStations", true);
            toolStripButtonPerspective.Checked = SQLiteDBClass.GetSettingBool("Map3DPerspective", false);
            toolStripButtonGrid.Checked = SQLiteDBClass.GetSettingBool("Map3DCoarseGrid", true);
            toolStripButtonFineGrid.Checked = SQLiteDBClass.GetSettingBool("Map3DFineGrid", true);
            toolStripButtonCoords.Checked = SQLiteDBClass.GetSettingBool("Map3DCoords", true);
            toolStripButtonEliteMovement.Checked = SQLiteDBClass.GetSettingBool("Map3DEliteMove", false);
            toolStripButtonStarNames.Checked = SQLiteDBClass.GetSettingBool("Map3DStarNames", true);
            showNoteMarksToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool("Map3DShowNoteMarks", true);
            showBookmarksToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool("Map3DShowBookmarks", true);
            toolStripButtonAutoForward.Checked = SQLiteDBClass.GetSettingBool("Map3DAutoForward", false );

            textboxFrom.AutoCompleteCustomSource = _systemNames;

            _stargrids.FillVisitedSystems(_visitedSystems);     // to ensure its updated
            _stargrids.Start();
        }

        public void SetPlannedRoute(List<SystemClass> plannedr)
        {
            _plannedRoute = plannedr;
            GenerateDataSetsVisitedSystems();
            glControl.Invalidate();
        }

        public void SetReferenceSystems(List<SystemClass> refsys)
        {
            _referenceSystems = refsys;
            GenerateDataSetsVisitedSystems();
            glControl.Invalidate();
        }

        public void UpdateVisitedSystems(List<VisitedSystemsClass> visited)
        {
            if (Is3DMapsRunning && visited != null )         // if null, we are not up and running.  visited should never be null, but being defensive
            {
                _visitedSystems = visited;
                _stargrids.FillVisitedSystems(_visitedSystems);          // update visited systems, will be displayed on next update of star grids
                GenerateDataSetsVisitedSystems();
                _starnameslist.RecalcStarNames();
                glControl.Invalidate();

                if (toolStripButtonAutoForward.Checked)             // auto forward?
                {
                    VisitedSystemsClass vs = _visitedSystems.FindLast(x => x.HasTravelCoordinates || (x.curSystem != null && x.curSystem.HasCoordinate));

                    if ( vs != null )
                        SetCenterSystemTo(vs.Name, true);
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
                GenerateDataSetsNotedSystems();
                glControl.Invalidate();
            }
        }

        public void UpdateBookmarks()
        {
            if (Is3DMapsRunning)         // if null, we are not up and running
            {
                GenerateDataSetsBookmarks();
                GenerateDataSetsNotedSystems();
                glControl.Invalidate();
            }
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

            GenerateDataSets();
            GenerateDataSetsMaps();
            GenerateDataSetsSelectedSystems();
            GenerateDataSetsVisitedSystems();

            _starnametimer.Interval = 100;
            _starnametimer.Tick += new EventHandler(NameStars);
            _starnametimer.Start();

            _mousehovertick.Tick += new EventHandler(MouseHoverTick);
            _mousehovertick.Interval = 250;
        }

        private void FormMap_Shown(object sender, EventArgs e)
        {
            int helpno = SQLiteDBClass.GetSettingInt("Map3DShownHelp", 0);                 // force up help, to make sure they know it exists
            if (helpno != HELP_VERSION)
            {
                toolStripButtonHelp_Click(null, null);
                SQLiteDBClass.PutSettingInt("Map3DShownHelp", HELP_VERSION);
            }
        }

        private void FormMap_Activated(object sender, EventArgs e)
        {
            _isActivated = true;
            _timerRunning = false;      // this causes ApplicationIdle to kick the first invalidate off..
            glControl.Invalidate();
        }

        private void FormMap_Deactivate(object sender, EventArgs e)
        {
            _isActivated = false;
            _timerRunning = true;
            UpdateTimer.Stop();
        }

        private void FormMap_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Visible)
            {
                SQLiteDBClass.PutSettingInt("Map3DFormWidth", this.Width);
                SQLiteDBClass.PutSettingInt("Map3DFormHeight", this.Height);
                SQLiteDBClass.PutSettingInt("Map3DFormTop", this.Top);
                SQLiteDBClass.PutSettingInt("Map3DFormLeft", this.Left);
                //Console.WriteLine("Save map " + this.Top + "," + this.Left + "," + this.Width + "," + this.Height);
            }

            _stargrids.Stop();

            e.Cancel = true;
            this.Hide();
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            GL.ClearColor((Color)System.Drawing.ColorTranslator.FromHtml("#0D0D10"));

            SetupViewport();

            _loaded = true;

            Application.Idle += Application_Idle;
            _timerRunning = false;              // timer is not running, app idle will kick it off..
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            if (!_timerRunning)                 // used when keyboard or camera slew, and at first load..
                glControl.Invalidate();
        }

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

        private void glControl_Resize(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            SetupViewport();
        }

#endregion

#region Viewport

        /// <summary>
        /// Setup the Viewport
        /// </summary>
        private void SetupViewport()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            int w = glControl.Width;
            int h = glControl.Height;

            if (w == 0 || h == 0) return;

            if (toolStripButtonPerspective.Checked)
            {
                Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(_cameraFov, (float)w / h, 1.0f, 1000000.0f);
                GL.LoadMatrix(ref perspective);
                _znear = 0.00000000000001f;     // very small positive value.. needed to be low so zoom=500 works.. may need revisiting
            }
            else
            {
                float orthoW = w * (_zoom + 1.0f);
                float orthoH = h * (_zoom + 1.0f);

                float orthoheight = 1000.0f * h / w;

                GL.Ortho(-1000.0f, 1000.0f, -orthoheight, orthoheight, -5000.0f, 5000.0f);
                _znear = -5000.0f;
            }

            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
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

            DatasetBuilder builder4 = new DatasetBuilder();
            _datasets_poi = builder4.AddPOIsToDataset();

            UpdateDataSetsDueToZoom();
        }

        private void UpdateDataSetsDueToZoom()
        {
            DatasetBuilder builder = new DatasetBuilder();
            if (toolStripButtonFineGrid.Checked)
                builder.UpdateGridZoom(ref _datasets_coarsegridlines, _zoom);

            if (toolStripButtonCoords.Checked)
                builder.UpdateGridCoordZoom(ref _datasets_gridlinecoords, _zoom);

            GenerateDataSetsBookmarks();
            GenerateDataSetsNotedSystems();
        }

        private void GenerateDataSetsMaps()
        {
            //Console.WriteLine("STARS Data set due to " + Environment.StackTrace);
            DeleteDataset(ref _datasets_maps);
            DatasetBuilder builder = new DatasetBuilder();
            _datasets_maps = builder.BuildMaps(ref selectedmaps);
        }

        private void GenerateDataSetsVisitedSystems()
        {
            //Console.WriteLine("Data set due to " + Environment.StackTrace);
            DeleteDataset(ref _datasets_visitedsystems);
            DatasetBuilder builder = new DatasetBuilder();
            _datasets_visitedsystems = builder.BuildVisitedSystems(toolStripButtonDrawLines.Checked, _centerSystem, _visitedSystems,
                                                                    _referenceSystems, _plannedRoute);
        }

        private void GenerateDataSetsSelectedSystems()
        {
            //Console.WriteLine("Data set due to " + Environment.StackTrace);
            DeleteDataset(ref _datasets_selectedsystems);
            DatasetBuilder builder = new DatasetBuilder();
            _datasets_selectedsystems = builder.BuildSelected(_centerSystem,_clickedSystem);
        }

        private void GenerateDataSetsBookmarks()         // Called during Load, and if we ever add systems..
        {
            DeleteDataset(ref _datasets_bookedmarkedsystems);
            _datasets_bookedmarkedsystems = null;

            if (showBookmarksToolStripMenuItem.Checked)
            {
                Bitmap mapstar = (Bitmap)EDDiscovery.Properties.Resources.bookmarkgreen;
                Bitmap mapregion = (Bitmap)EDDiscovery.Properties.Resources.bookmarkyellow;
                Bitmap maptarget = (Bitmap)EDDiscovery.Properties.Resources.bookmarktarget;
                Debug.Assert(mapstar != null && mapregion != null);

                DatasetBuilder builder = new DatasetBuilder();
                _datasets_bookedmarkedsystems = builder.AddStarBookmarks(mapstar, mapregion, maptarget, GetBookmarkSize(), GetBookmarkSize(), toolStripButtonPerspective.Checked);
            }
        }

        private void GenerateDataSetsNotedSystems()         // Called during Load, and if we ever add systems..
        {
            DeleteDataset(ref _datasets_notedsystems);
            _datasets_notedsystems = null;

            if (showNoteMarksToolStripMenuItem.Checked)
            {
                Bitmap map = (Bitmap)EDDiscovery.Properties.Resources.bookmarkbrightred;
                Bitmap maptarget = (Bitmap)EDDiscovery.Properties.Resources.bookmarktarget;
                Debug.Assert(map != null);

                DatasetBuilder builder = new DatasetBuilder();
                _datasets_notedsystems = builder.AddNotedBookmarks(map, maptarget, GetBookmarkSize(), GetBookmarkSize(), toolStripButtonPerspective.Checked , _visitedSystems );
            }
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

        private void NameStars(object sender, EventArgs e) // tick.. way it works is a tick occurs, timer off, thread runs, thread calls 
                                                           // delegate, work done in UI, timer reticks.  KEEP it that way!
        {
            if (Visible && toolStripButtonStarNames.Checked && _zoom >= 0.99)  // only when shown, and enabled, and with a good zoom
            {
                if (_starnameslist.Update(_zoom, _cameraPos, _cameraDir, GetResMat(), _znear))       // if its kicked off the thread..
                    _starnametimer.Stop();                                              // stop the timer.
            }
            else
            {
                if (_starnameslist.RemoveAllNamedStars())
                {
                    glControl.Invalidate();
                    GC.Collect();
                }
            }
        }

        public void ChangeNamedStars()                                  // for the star naming thread.. kick it off
        {
            glControl.Invalidate();
            _starnametimer.Start();                                      // and do another tick..
        }

        private List<FGEImage> GetSelectedMaps()
        {
            FGEImage[] _selected = dropdownMapNames.DropDownItems.OfType<ToolStripButton>().Where(b => b.Checked).Select(b => b.Tag as FGEImage).ToArray();
            HashSet<string> newselected = new HashSet<string>(_selected.Select(f => f.FileName));
            HashSet<string> oldselected = new HashSet<string>(selectedmaps.Select(f => f.FileName));
            List<FGEImage> selected = new List<FGEImage>();

            foreach (var sel in selectedmaps)
            {
                if (newselected.Contains(sel.FileName))
                {
                    selected.Add(sel);
                }
            }

            foreach (var sel in _selected)
            {
                if (!oldselected.Contains(sel.FileName))
                {
                    selected.Add(sel);
                }
            }

            return selected;
        }
#endregion

#region Set Orientation

        private void SetCenterSystemLabel()
        {
            if (_centerSystem != null)
                labelSystemCoords.Text = string.Format("{0} x:{1} y:{2} z:{3}", _centerSystem.name, _centerSystem.x.ToString("0.00"), _centerSystem.y.ToString("0.00"), _centerSystem.z.ToString("0.00"));
            else
                labelSystemCoords.Text = "No centre system";
        }

        public bool SetCenterSystemTo(VisitedSystemsClass system, bool moveto)
        {
            if (Is3DMapsRunning)
            {
                ISystem sys = system.curSystem;

                if (sys != null)
                {
                    return SetCenterSystemTo(sys, moveto);
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

        public bool SetCenterSystemTo(string name, bool moveto)
        {
            if (Is3DMapsRunning)                         // if null, we are not up and running
                return SetCenterSystemTo(FindSystem(name), moveto);
            else
                return false;
        }

        private bool SetCenterSystemTo(ISystem sys, bool moveto)        
        {
            if (sys != null)
            {
                _centerSystem = sys;
                SetCenterSystemLabel();
                GenerateDataSetsSelectedSystems();

                if (moveto)
                    StartCameraSlew(new Vector3((float)_centerSystem.x, (float)_centerSystem.y, (float)_centerSystem.z));

                glControl.Invalidate();

                return true;
            }
            else
                return false;
        }
        

#endregion

#region Keyboard

        private void HandleInputs()
        {
            ReceiveKeyboardActions();
            HandleTurningAdjustments();
            HandleMovementAdjustments();
            HandleZoomAdjustment();
        }

        private void ReceiveKeyboardActions()
        {
            _kbdActions.Reset();

            if ( !_isActivated || !glControl.Focused)
                    return;

            try
            {
                var state = OpenTK.Input.Keyboard.GetState();

                if (state[Key.F1])
                    _starnameslist.IncreaseStarLimit();

                if (state[Key.F2])
                    _starnameslist.DecreaseStarLimit();

                _kbdActions.Left = (state[Key.Left] || state[Key.A]);
                _kbdActions.Right = (state[Key.Right] || state[Key.D]);

                if (toolStripButtonPerspective.Checked)
                {
                    _kbdActions.Up = (state[Key.PageUp] || state[Key.R]);
                    _kbdActions.Down = (state[Key.PageDown] || state[Key.F]);
                    _kbdActions.Forwards = state[Key.Up] || state[Key.W];                    // WASD is fore/back/left/right, R/F is up down
                    _kbdActions.Backwards = state[Key.Down] || state[Key.S];
                }
                else
                {
                    _kbdActions.Up = state[Key.W] || state[Key.Up];
                    _kbdActions.Down = state[Key.S] || state[Key.Down];
                }

                _kbdActions.ZoomIn = (state[Key.Plus] || state[Key.Z]);                 // additional Useful keys
                _kbdActions.ZoomOut = (state[Key.Minus] || state[Key.X]);
                _kbdActions.YawLeft = state[Key.Keypad4];
                _kbdActions.YawRight = state[Key.Keypad6];
                _kbdActions.Pitch = state[Key.Keypad8];
                _kbdActions.Dive = (state[Key.Keypad5] || state[Key.Keypad2]);
                _kbdActions.RollLeft = state[Key.Keypad7] || state[Key.Q];
                _kbdActions.RollRight = state[Key.Keypad9] || state[Key.E];

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"ReceiveKeybaordActions Exception: {ex.Message}");
                return;
            }
        }

        private void HandleTurningAdjustments()
        {
            _cameraActionRotation = Vector3.Zero;

            var angle = (float)_ticks * 0.075f;
            if (_kbdActions.YawLeft)
            {
                _cameraActionRotation.Z = -angle;
            }
            if (_kbdActions.YawRight)
            {
                _cameraActionRotation.Z = angle;
            }
            if (_kbdActions.Dive)
            {
                _cameraActionRotation.X = -angle;
            }
            if (_kbdActions.Pitch)
            {
                _cameraActionRotation.X = angle;
            }
            if (_kbdActions.RollLeft)
            {
                _cameraActionRotation.Y = -angle;
            }
            if (_kbdActions.RollRight)
            {
                _cameraActionRotation.Y = angle;
            }

        }

        private void HandleMovementAdjustments()
        {
            _cameraActionMovement = Vector3.Zero;
            float zoomlimited = Math.Min(Math.Max(_zoom, 0.01F), 15.0F);
            var distance = _ticks * (1.0f / zoomlimited);

            if ((Control.ModifierKeys & Keys.Shift) != 0)
                distance *= 2.0F;

            //Console.WriteLine("Distance " + distance + " zoom " + _zoom + " lzoom " + zoomlimited );
            if (_kbdActions.Left)
            {
                _cameraActionMovement.X = -distance;
            }
            if (_kbdActions.Right)
            {
                _cameraActionMovement.X = distance;
            }
            if (_kbdActions.Forwards)
            {
                _cameraActionMovement.Y = distance;
            }
            if (_kbdActions.Backwards)
            {
                _cameraActionMovement.Y = -distance;
            }
            if (_kbdActions.Up)
            {
                _cameraActionMovement.Z = distance;
            }
            if (_kbdActions.Down)
            {
                _cameraActionMovement.Z = -distance;
            }
        }

        private void HandleZoomAdjustment()
        {
            float curzoom = _zoom;
            var adjustment = 1.0f + ((float)_ticks * 0.01f);
            if (_kbdActions.ZoomIn)
            {
                _zoom *= (float)adjustment;
                if (_zoom > ZoomMax) _zoom = (float)ZoomMax;
            }
            if (_kbdActions.ZoomOut)
            {
                _zoom /= (float)adjustment;
                if (_zoom < ZoomMin) _zoom = (float)ZoomMin;
            }

            if (_zoom != curzoom)
                UpdateDataSetsDueToZoom();
        }

#endregion

#region Render

        private void Render()
        {
            if (!_loaded) // Play nice
                return;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);

            if (toolStripButtonPerspective.Checked)
            {
                CameraLookAt();
            }
            else
            {
                TransformWorldOrientatation();

                TransformCamera();
            }
            FlipYAxisOnWorld();
            RenderGalaxy();

            glControl.SwapBuffers();
            UpdateStatus();
        }

        private void TransformWorldOrientatation()
        {
            GL.LoadIdentity();
            GL.Rotate(-90.0, 1, 0, 0);
        }

        private void TransformCamera()
        {
            GL.Scale(_zoom, _zoom, _zoom);
            GL.Rotate(_cameraDir.Z, 0.0, 0.0, -1.0);
            GL.Rotate(_cameraDir.X, -1.0, 0.0, 0.0);
            GL.Rotate(_cameraDir.Y, 0.0, -1.0, 0.0);
            GL.Translate(-_cameraPos.X, -_cameraPos.Y, -_cameraPos.Z);
        }

        private void CameraLookAt()
        {
            Vector3 target = _cameraPos;
            Matrix4 transform = Matrix4.Identity;
            transform *= Matrix4.CreateRotationZ((float)(_cameraDir.Z * Math.PI / 180.0f));
            transform *= Matrix4.CreateRotationX((float)(_cameraDir.X * Math.PI / 180.0f));
            transform *= Matrix4.CreateRotationY((float)(_cameraDir.Y * Math.PI / 180.0f));
            Vector3 eyerel = Vector3.Transform(new Vector3(0.0f, -1000.0f / _zoom, 0.0f), transform);
            Vector3 up = Vector3.Transform(new Vector3(0.0f, 0.0f, 1.0f), transform);
            Vector3 eye = _cameraPos + eyerel;
            Matrix4 lookat = Matrix4.LookAt(eye, target, up);
            GL.LoadMatrix(ref lookat);
        }

        private void FlipYAxisOnWorld()
        {
            GL.Scale(1.0, -1.0, 1.0);
        }

        private void RenderGalaxy()
        {
            GL.Enable(EnableCap.PointSmooth);
            GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.PushMatrix();
            DrawStars();
            GL.PopMatrix();
        }

        private void DrawStars()
        {
            if (_datasets_maps == null)     // happens during debug.. paint before form load
                return;

            foreach (var dataset in _datasets_maps)                     // needs to be in order of background to foreground objects
                dataset.DrawAll(glControl);

            if (toolStripButtonFineGrid.Checked)
            {
                foreach (var dataset in _datasets_finegridlines)
                    dataset.DrawAll(glControl);
            }

            if (toolStripButtonGrid.Checked)
            {
                foreach (var dataset in _datasets_coarsegridlines)
                    dataset.DrawAll(glControl);
            }

            if (toolStripButtonCoords.Checked)
            {
                foreach (var dataset in _datasets_gridlinecoords)
                    dataset.DrawAll(glControl);
            }

             _stargrids.DrawAll(glControl, showStarstoolStripMenuItem.Checked, showStationsToolStripMenuItem.Checked);
            
            foreach (var dataset in _datasets_poi)
                dataset.DrawAll(glControl);

            foreach (var dataset in _datasets_visitedsystems)
                dataset.DrawAll(glControl);

            _starnameslist.Draw();

            foreach (var dataset in _datasets_selectedsystems)
                dataset.DrawAll(glControl);

            if (_datasets_notedsystems != null)
            {
                foreach (var dataset in _datasets_notedsystems)                     // needs to be in order of background to foreground objects
                    dataset.DrawAll(glControl);
            }

            if (_datasets_bookedmarkedsystems != null)
            {
                foreach (var dataset in _datasets_bookedmarkedsystems)                     // needs to be in order of background to foreground objects
                    dataset.DrawAll(glControl);
            }

        }

        private void UpdateStatus()
        {
            if (toolStripButtonPerspective.Checked)
                statusLabel.Text = "Use W(fwd) S(back) A(lft) D(Rgt), R(up) F(dn) keys with the mouse. ";
            else
                statusLabel.Text = "Use W, A, S, D keys with the mouse. ";

            statusLabel.Text += string.Format("x={0,-6:0} y={1,-6:0} z={2,-6:0} Zoom={3,-4:0.00}", _cameraPos.X, -(_cameraPos.Y), _cameraPos.Z, _zoom);
#if DEBUG
            statusLabel.Text += string.Format("   Direction x={0,-6:0.0} y={1,-6:0.0} z={2,-6:0.0}", _cameraDir.X, _cameraDir.Y, _cameraDir.Z);
#endif
        }

        private void CalculateTimeDelta()
        {
            var tickCount = DateTime.Now.Ticks / 10000;
            _ticks = (int)(tickCount - _oldTickCount);
            _oldTickCount = tickCount;
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            CalculateTimeDelta();
            //Console.WriteLine("Paint at " + Environment.TickCount + " Delta " + _ticks);

            if (!_kbdActions.Any() && (_cameraSlewProgress == 0.0f || _cameraSlewProgress >= 1.0f))
            {
                _ticks = 1;
                _oldTickCount = DateTime.Now.Ticks / 10000;
            }

            _stargrids.Update(_cameraPos.X, _cameraPos.Z, glControl);
            HandleInputs();
            DoCameraSlew();
            UpdateCamera();
            Render();

            if (_kbdActions.Any() || _cameraSlewProgress < 1.0f)
            {
                if (_timerRunning)                              // keyboard actions and camera slew rely on application idle to kick it.
                {
                    UpdateTimer.Stop();
                    _timerRunning = false;
                }
            }
            else
            {
                if (!_timerRunning )                            // restart timer
                {
                    UpdateTimer.Interval = 100;
                    UpdateTimer.Start();
                    _timerRunning = true;
                }
                else
                {
                    UpdateTimer.Stop();
                    UpdateTimer.Start();
                }
            }
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            glControl.Invalidate();                             // timer tick kicks paint..
        }

#endregion

#region Camera Slew and Update

        private void ResetCamera()
        {
            _cameraPos = new Vector3((float)_centerSystem.x, -(float)_centerSystem.y, (float)_centerSystem.z);
            _cameraDir = Vector3.Zero;

            _zoom = _defaultZoom;
            UpdateDataSetsDueToZoom();
        }

        private void StartCameraSlew(Vector3 pos)
        {
            _cameraSlewPosition = pos;
            _oldTickCount = Environment.TickCount;
            _ticks = 0;
            _cameraSlewProgress = 0.0f;
        }

        private void DoCameraSlew()
        {
            if (_kbdActions.Any())
            {
                _cameraSlewProgress = 1.0f;
            }

            if (_cameraSlewProgress < 1.0f)
            {
                _cameraActionMovement = Vector3.Zero;
                var newprogress = _cameraSlewProgress + _ticks / (CameraSlewTime * 1000);
                var totvector = new Vector3((float)(_cameraSlewPosition.X - _cameraPos.X), (float)(-_cameraSlewPosition.Y - _cameraPos.Y), (float)(_cameraSlewPosition.Z - _cameraPos.Z));

                if (newprogress >= 1.0f)
                {
                    _cameraPos = new Vector3(_cameraSlewPosition.X,-_cameraSlewPosition.Y, _cameraSlewPosition.Z);
                }
                else
                {
                    var slewstart = Math.Sin((_cameraSlewProgress - 0.5) * Math.PI);
                    var slewend = Math.Sin((newprogress - 0.5) * Math.PI);
                    var slewfact = (slewend - slewstart) / (1.0 - slewstart);
                    _cameraPos += Vector3.Multiply(totvector, (float)slewfact);
                }
                _cameraSlewProgress = (float)newprogress;
            }
        }

        private void UpdateCamera()
        {
            _cameraDir.X = BoundedAngle(_cameraDir.X + _cameraActionRotation.X);
            _cameraDir.Y = BoundedAngle(_cameraDir.Y + _cameraActionRotation.Y);
            _cameraDir.Z = BoundedAngle(_cameraDir.Z + _cameraActionRotation.Z);        // rotate camera by asked value

            // Limit camera pitch
            if (_cameraDir.X < 0 && _cameraDir.X > -90)
                _cameraDir.X = 0;
            if (_cameraDir.X > 180 || _cameraDir.X <= -90)
                _cameraDir.X = 180;

#if DEBUG
            bool istranslating = (_cameraActionMovement.X != 0 || _cameraActionMovement.Y != 0 || _cameraActionMovement.Z != 0);
//            if (istranslating)
//                Console.WriteLine("move Camera " + _cameraActionMovement.X + "," + _cameraActionMovement.Y + "," + _cameraActionMovement.Z
//                    + " point " + _cameraDir.X + "," + _cameraDir.Y + "," + _cameraDir.Z);
#endif

            var rotZ = Matrix4.CreateRotationZ(DegreesToRadians(_cameraDir.Z));
            var rotX = Matrix4.CreateRotationX(DegreesToRadians(_cameraDir.X));
            var rotY = Matrix4.CreateRotationY(DegreesToRadians(_cameraDir.Y));

            bool em = toolStripButtonEliteMovement.Checked && toolStripButtonPerspective.Checked;     // elite movement

            Vector3 requestedmove = new Vector3(_cameraActionMovement.X, _cameraActionMovement.Y, (em) ? 0 : _cameraActionMovement.Z);

            var translation = Matrix4.CreateTranslation(requestedmove);
            var cameramove = Matrix4.Identity;
            cameramove *= translation;
            cameramove *= rotZ;
            cameramove *= rotX;
            cameramove *= rotY;

            Vector3 trans = cameramove.ExtractTranslation();

            if (em)                                             // if in elite movement, Y is not affected
            {                                                   // by ASDW.
                trans.Y = 0;                                    // no Y translation even if camera rotated the vector into Y components
                _cameraPos += trans;
                _cameraPos.Y -= _cameraActionMovement.Z;        // translation appears in Z axis due to way the camera rotation is set up
            }
            else
                _cameraPos += trans;
#if DEBUG
//            if (istranslating)
//                Console.WriteLine("   em " + em + " Camera now " + _cameraPos.X + "," + _cameraPos.Y + "," + _cameraPos.Z);
#endif
        }

        private float BoundedAngle(float angle)
        {
            return ((angle + 360 + 180) % 360) - 180;
        }

        private float DegreesToRadians(float angle)
        {
            return (float)(Math.PI * angle / 180.0);
        }

#endregion

#region User Controls

        private void EndPicker_ValueChanged(object sender, EventArgs e)
        {
            endTime = endPicker.Value;
            GenerateDataSetsVisitedSystems();
            glControl.Invalidate();
        }

        private void StartPicker_ValueChanged(object sender, EventArgs e)
        {
            startTime = startPicker.Value;
            GenerateDataSetsVisitedSystems();
            glControl.Invalidate();
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
            glControl.Invalidate();
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
            glControl.Invalidate();
        }

        private void textboxFrom_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonCenter_Click(sender, e);
        }

        private void buttonCenter_Click(object sender, EventArgs e)
        {
            ISystem sys = FindSystem(textboxFrom.Text);

            if (sys != null)
            {
                textboxFrom.Text = sys.name;        // normalise name (user may have different 
                SetCenterSystemTo(sys, true);
            }
            else
                MessageBox.Show("System " + textboxFrom.Text + " not found");
        }

        private void toolStripButtonGoBackward_Click(object sender, EventArgs e)
        {
            if (_visitedSystems != null)
            {
                string name = VisitedSystemsClass.FindNextVisitedSystem(_visitedSystems, _centerSystem.name, -1, _centerSystem.name);
                SetCenterSystemTo(name, true);
            }
            else
                MessageBox.Show("No travel history is available");
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            SetCenterSystemTo(_homeSystem, true);
        }

        private void buttonHistory_Click(object sender, EventArgs e)
        {
            if (_historySelection == null)
                MessageBox.Show("No travel history is available");
            else
                SetCenterSystemTo(_historySelection, true);
        }

        private void toolStripButtonTarget_Click(object sender, EventArgs e)
        {
            string name;
            double x, y, z;

            if (TargetClass.GetTargetPosition(out name, out x, out y, out z))
            {
                StartCameraSlew(new Vector3((float)x, (float)y, (float)z));
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
                SetCenterSystemTo(name, true);
            }
            else
                MessageBox.Show("No travel history is available");
        }

        private void toolStripButtonAutoForward_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DAutoForward", toolStripButtonAutoForward.Checked);
        }

        private void toolStripLastKnownPosition_Click(object sender, EventArgs e)
        {
            if (_visitedSystems != null)
            {
                VisitedSystemsClass vs = _visitedSystems.FindLast(x => x.HasTravelCoordinates || (x.curSystem != null && x.curSystem.HasCoordinate));

                if (vs != null )
                    SetCenterSystemTo(FindSystem(vs.Name), true);
                else
                    MessageBox.Show("No stars with defined co-ordinates available in travel history");
            }
            else
                MessageBox.Show("No travel history is available");
        }

        private void toolStripButtonDrawLines_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DDrawLines", toolStripButtonDrawLines.Checked);
            GenerateDataSetsVisitedSystems();
            glControl.Invalidate();
        }

        private void showStarstoolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DAllStars", showStarstoolStripMenuItem.Checked);
            glControl.Invalidate();
        }

        private void showStationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DButtonStations", showStationsToolStripMenuItem.Checked);
            glControl.Invalidate();
        }

        private void toolStripButtonGrid_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DCoarseGrid", toolStripButtonGrid.Checked);
            glControl.Invalidate();
        }

        private void toolStripButtonFineGrid_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DFineGrid", toolStripButtonFineGrid.Checked);
            UpdateDataSetsDueToZoom();
            glControl.Invalidate();
        }

        private void toolStripButtonCoords_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DCoords", toolStripButtonCoords.Checked);
            UpdateDataSetsDueToZoom();
            glControl.Invalidate();
        }

        private void toolStripButtonEliteMovement_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DEliteMove", toolStripButtonEliteMovement.Checked);
        }

        private void toolStripButtonStarNames_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DStarNames", toolStripButtonStarNames.Checked);
        }

        private void showNoteMarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DShowNoteMarks", showNoteMarksToolStripMenuItem.Checked);
            GenerateDataSetsNotedSystems();
            glControl.Invalidate();
        }

        private void showBookmarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DShowBookmarks", showBookmarksToolStripMenuItem.Checked);
            GenerateDataSetsBookmarks();
            glControl.Invalidate();
        }

        private void newRegionBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookmarkForm frm = new BookmarkForm();
            frm.InitialisePos(_cameraPos.X, -(_cameraPos.Y), _cameraPos.Z);
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

                GenerateDataSetsBookmarks();
                glControl.Invalidate();
            }
        }
        
        private void toolStripButtonPerspective_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DPerspective", toolStripButtonPerspective.Checked);
            SetupViewport();
            GenerateDataSetsNotedSystems();
            GenerateDataSetsBookmarks();
            glControl.Invalidate();
        }

        private void dotSystemCoords_Click(object sender, EventArgs e)
        {
            SetCenterSystemTo(_centerSystem, true);
        }

        private void dotSelectedSystemCoords_Click(object sender, EventArgs e)
        {
            SetCenterSystemTo(_clickedSystem, true);
        }

        private void toolStripButtonHelp_Click(object sender, EventArgs e)
        {
            InfoForm dl = new InfoForm();
            string text = EDDiscovery.Properties.Resources.maphelp3d;
            dl.Info("3D Map Help", text, new Font("Microsoft Sans Serif", 10), new int[] { 50, 200, 400 });
            dl.Show();
        }

        private void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (_timerRunning)
                glControl.Invalidate();
        }

        private void glControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (_timerRunning)
                glControl.Invalidate();
        }

        private void dropdownMapNames_DropDownItemClicked(object sender, EventArgs e)
        {
            ToolStripButton tsb = (ToolStripButton)sender;
            SQLiteDBClass.PutSettingBool("map3DMaps" + tsb.Text, tsb.Checked);
            GenerateDataSetsMaps();
            glControl.Invalidate();
        }

        private void viewOnEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_clickedSystem != null)
            {
                var edsm = new EDSM.EDSMClass();
                edsm.ShowSystemInEDSM(_clickedSystem.name);
            }
        }

        private void labelClickedSystemCoords_Click(object sender, EventArgs e)
        {
            if (_clickedSystem != null)
            {
                systemselectionMenuStrip.Show(labelClickedSystemCoords, 0, labelClickedSystemCoords.Height);
            }
        }

#endregion

#region Mouse

        private void glControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _cameraSlewProgress = 1.0f;

            _mouseDownPos.X = e.X;
            _mouseDownPos.Y = e.Y;
            //Console.WriteLine("Mouseup down at " + e.X + "," + e.Y);

            if (e.Button.HasFlag(System.Windows.Forms.MouseButtons.Left))
            {
                _mouseStartRotate.X = e.X;
                _mouseStartRotate.Y = e.Y;
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
            bool notmovedmouse = Math.Abs(e.X - _mouseDownPos.X) + Math.Abs(e.Y - _mouseDownPos.Y) < 8;

            // system = cursystem!=null, curbookmark = null, notedsystem = false
            // bookmark on system, cursystem!=null, curbookmark != null, notedsystem = false
            // region bookmark. cursystem = null, curbookmark != null, notedsystem = false
            // clicked on note on a system, cursystem!=null,curbookmark=null, notedsystem=true

            ISystem cursystem = null;      
            BookmarkClass curbookmark = null;           
            bool notedsystem = false;                   

            if (notmovedmouse)
            {
                GetMouseOverItem(e.X, e.Y, out cursystem, out curbookmark, out notedsystem);
            }

            if ( e.Button == System.Windows.Forms.MouseButtons.Left)            // left clicks associate with systems..
            {
                _clickedSystem = cursystem;

                if (_clickedSystem != null)         // will be set for systems clicked, bookmarks or noted systems
                {
                    labelClickedSystemCoords.Text = string.Format("{0} x:{1} y:{2} z:{3}", _clickedSystem.name, _clickedSystem.x.ToString("0.00"), _clickedSystem.y.ToString("0.00"), _clickedSystem.z.ToString("0.00"));

                    SystemClass sysclass = (_clickedSystem.id != 0) ? SystemClass.GetSystem(_clickedSystem.id) : SystemClass.GetSystem(_clickedSystem.name);

                    if (sysclass != null)
                    {
                        selectionAllegiance.Text = "Allegiance: " + sysclass.allegiance;
                        selectionEconomy.Text = "Economy: " + sysclass.primary_economy;
                        selectionGov.Text = "Gov: " + sysclass.government;
                        selectionState.Text = "State: " + sysclass.state;
                    }

                    GenerateDataSetsSelectedSystems();
                    glControl.Invalidate();

                    viewOnEDSMToolStripMenuItem.Enabled = true;
                    System.Windows.Forms.Clipboard.SetText(_clickedSystem.name);
                }
                else if (curbookmark != null)                                   // else just remember the position for later.
                    _clickedposition = new Vector3((float)curbookmark.x, (float)curbookmark.y, (float)curbookmark.z);
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)                    // right clicks are about bookmarks.
            {
                if (cursystem != null || curbookmark != null )      // if we have a system or a bookmark..
                {                                                   // try and find the associated bookmark..
                    BookmarkClass bkmark = (curbookmark != null) ? curbookmark : BookmarkClass.bookmarks.Find(x => x.StarName != null && x.StarName.Equals(cursystem.name));
                    string note = (cursystem != null) ? SystemNoteClass.GetSystemNote(cursystem.name) : null;   // may be null

                    BookmarkForm frm = new BookmarkForm();

                    if (notedsystem && bkmark == null)              // note on a system
                    {
                        long targetid = TargetClass.GetTargetNotedSystem();      // who is the target of a noted system (0=none)
                        long noteid = SystemNoteClass.GetSystemNoteClass(cursystem.name).id;

                        frm.InitialisePos(cursystem.x, cursystem.y, cursystem.z);
                        frm.SystemInfo(cursystem.name, note,noteid == targetid);       // note may be passed in null
                        frm.ShowDialog();

                        if ((frm.IsTarget && targetid != noteid) || (!frm.IsTarget && targetid == noteid)) // changed..
                        {
                            if (frm.IsTarget)
                                TargetClass.SetTargetNotedSystem(cursystem.name, noteid, cursystem.x, cursystem.y, cursystem.z);
                            else
                                TargetClass.ClearTarget();

                            GenerateDataSetsBookmarks();
                            GenerateDataSetsNotedSystems();
                            glControl.Invalidate();
                            travelHistoryControl.RefreshTargetInfo();
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
                            frm.New(cursystem.name, note, tme.ToString());
                        }
                        else                                        // update bookmark
                        {
                            frm.InitialisePos(bkmark.x, bkmark.y, bkmark.z);
                            regionmarker = bkmark.isRegion;
                            tme = bkmark.Time;
                            frm.Update(regionmarker ? bkmark.Heading : bkmark.StarName, note, bkmark.Note, tme.ToString(), regionmarker , targetid == bkmark.id );
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
                                
                                GenerateDataSetsBookmarks();
                                GenerateDataSetsNotedSystems();
                                glControl.Invalidate();
                                travelHistoryControl.RefreshTargetInfo();
                            }
                        }
                        else if (res == DialogResult.Abort && bkmark != null)
                        {
                            if (targetid == bkmark.id)
                            {
                                TargetClass.ClearTarget();
                                GenerateDataSetsBookmarks();
                                GenerateDataSetsNotedSystems();
                                glControl.Invalidate();
                                travelHistoryControl.RefreshTargetInfo();
                            }

                            bkmark.Delete();
                        }
                    }

                    GenerateDataSetsBookmarks();
                    glControl.Invalidate();
                }

                //else if ( notmovedmouse )  if I ever get click at point working..
                //  MessageBox.Show("Right click at " + e.X + "," + e.Y);
            }
        }

        private void glControl_DoubleClick(object sender, EventArgs e)
        {
            if (_clickedSystem != null)
            {
                SetCenterSystemTo(_clickedSystem, true);            // no action if clicked system null
                travelHistoryControl.SetTravelHistoryPosition(_clickedSystem.name);
            }
            else if ( _clickedposition != null )
            {
                StartCameraSlew(_clickedposition);
            }
        }

        /// <summary>
        /// The triangle will always move with the cursor. 
        /// But is is not a problem to make it only if mousebutton pressed. 
        /// And do some simple ath with old Translation and new translation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // TODO: Use OpenTK Input control, implement rotation where the camera locks onto the mouse, like with EDs map.
            // This may be a good starting point to figuring it out:
            // http://www.opentk.com/node/3738
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _cameraSlewProgress = 1.0f;

                int dx = e.X - _mouseStartRotate.X;
                int dy = e.Y - _mouseStartRotate.Y;

                _mouseStartRotate.X = _mouseStartTranslateXZ.X = e.X;
                _mouseStartRotate.Y = _mouseStartTranslateXZ.Y = e.Y;
                //System.Diagnostics.Trace.WriteLine("dx" + dx.ToString() + " dy " + dy.ToString() + " Button " + e.Button.ToString());


                _cameraDir.Y += (float)(dx / 4.0f);
                _cameraDir.X += (float)(-dy / 4.0f);

                // Limit camera pitch
                if (_cameraDir.X < 0 && _cameraDir.X > -90)
                    _cameraDir.X = 0;
                if (_cameraDir.X > 180 || _cameraDir.X <= -90)
                    _cameraDir.X = 180;
                //SetupCursorXYZ();

                glControl.Invalidate();
            }
            // TODO: Turn this into Up and Down along Y Axis, like real ED map 
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                _cameraSlewProgress = 1.0f;

                int dx = e.X - _mouseStartTranslateXY.X;
                int dy = e.Y - _mouseStartTranslateXY.Y;

                _mouseStartTranslateXY.X = _mouseStartTranslateXZ.X = e.X;
                _mouseStartTranslateXY.Y = _mouseStartTranslateXZ.Y = e.Y;
                //System.Diagnostics.Trace.WriteLine("dx" + dx.ToString() + " dy " + dy.ToString() + " Button " + e.Button.ToString());


                //_cameraPos.X += -dx * (1.0f /_zoom) * 2.0f;
                _cameraPos.Y += -dy * (1.0f / _zoom) * 2.0f;

                glControl.Invalidate();
            }
            if (e.Button == (System.Windows.Forms.MouseButtons.Left | System.Windows.Forms.MouseButtons.Right))
            {
                _cameraSlewProgress = 1.0f;

                int dx = e.X - _mouseStartTranslateXZ.X;
                int dy = e.Y - _mouseStartTranslateXZ.Y;

                _mouseStartTranslateXZ.X = _mouseStartRotate.X = _mouseStartTranslateXY.X = e.X;
                _mouseStartTranslateXZ.Y = _mouseStartRotate.Y = _mouseStartTranslateXY.Y = e.Y;
                //System.Diagnostics.Trace.WriteLine("dx" + dx.ToString() + " dy " + dy.ToString() + " Button " + e.Button.ToString());

                Matrix4 transform = Matrix4.CreateRotationZ((float)(-_cameraDir.Y * Math.PI / 180.0f));
                Vector3 translation = new Vector3(-dx * (1.0f / _zoom) * 2.0f, dy * (1.0f / _zoom) * 2.0f, 0.0f);
                translation = Vector3.Transform(translation, transform);

                _cameraPos.X += translation.X;
                _cameraPos.Z += translation.Y;

                glControl.Invalidate();
            }
            else //
            {
                if (_mousehovertooltip != null)
                {
                    if (Math.Abs(e.X - _mouseHover.X) + Math.Abs(e.Y - _mouseHover.Y) > 8)
                    {
                        _mousehovertooltip.Dispose();
                        _mousehovertooltip = null;
                    }
                }
                else if (_mousehovertick.Enabled)
                {
                    if (Math.Abs(e.X - _mouseHover.X) + Math.Abs(e.Y - _mouseHover.Y) > 8)
                    {
                        _mousehovertick.Stop();
                    }
                }
                else
                {
                    _mouseHover = e.Location;
                    _mousehovertick.Start();
                }
            }
        }

        private void glControl_OnMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var kbdstate = OpenTK.Input.Keyboard.GetState();

            if (kbdstate[Key.LControl] || kbdstate[Key.RControl])
            {
                if (e.Delta > 0)
                {
                    _cameraFov *= (float)ZoomFact;
                    if (_cameraFov >= Math.PI * 0.8)
                    {
                        _cameraFov = (float)(Math.PI * 0.8);
                    }
                }
                if (e.Delta < 0)
                {
                    _cameraFov /= (float)ZoomFact;
                }
            }
            else
            {
                float curzoom = _zoom;

                if (e.Delta > 0)
                {
                    _zoom *= (float)ZoomFact;
                    if (_zoom > ZoomMax) _zoom = (float)ZoomMax;
                }
                if (e.Delta < 0)
                {
                    _zoom /= (float)ZoomFact;
                    if (_zoom < ZoomMin) _zoom = (float)ZoomMin;
                }

                if ( curzoom != _zoom )
                    UpdateDataSetsDueToZoom();

                //SetupCursorXYZ();
            }

            SetupViewport();
            glControl.Invalidate();
        }

        void MouseHoverTick(object sender, EventArgs e)
        {
            _mousehovertick.Stop();

            ISystem hoversystem = null;
            BookmarkClass curbookmark = null;
            bool notedsystem = false;
            GetMouseOverItem(_mouseHover.X, _mouseHover.Y, out hoversystem, out curbookmark, out notedsystem);

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

                if (curbookmark != null && curbookmark.Note != null)
                    info += Environment.NewLine + "Bookmark Notes: " + curbookmark.Note.Trim();

                _mousehovertooltip = new System.Windows.Forms.ToolTip();
                _mousehovertooltip.InitialDelay = 0;
                _mousehovertooltip.AutoPopDelay = 30000;
                _mousehovertooltip.ReshowDelay = 0;
                _mousehovertooltip.IsBalloon = true;
                _mousehovertooltip.SetToolTip(glControl, info);
            }
        }

#endregion

#region FindObjectsOnScreen

        Matrix4d GetResMat()
        {
            Matrix4d proj;
            Matrix4d mview;
            GL.GetDouble(GetPName.ProjectionMatrix, out proj);
            GL.GetDouble(GetPName.ModelviewMatrix, out mview);
            return Matrix4d.Mult(mview, proj);
        }

        private bool GetPixel(Vector4d xyzw, Matrix4d resmat, ref Vector2d pixelpos, out double newcursysdistz)
        {
            Vector4d sysloc = Vector4d.Transform(xyzw, resmat);

            double w2 = glControl.Width / 2.0;
            double h2 = glControl.Height / 2.0;

            if (sysloc.Z > _znear)
            {
                pixelpos = new Vector2d(((sysloc.X / sysloc.W) + 1.0) * w2, ((sysloc.Y / sysloc.W) + 1.0) * h2);
                newcursysdistz = Math.Abs(sysloc.Z * _zoom);

                return true;
            }

            newcursysdistz = 0;
            return false;
        }

        private bool IsWithinRectangle( Matrix4d area ,  int x, int y, out double newcursysdistz)
        {
            Vector2d ptopleft = new Vector2d(0, 0), pbottomright = new Vector2d(0, 0);
            Vector2d pbottomleft = new Vector2d(0, 0), ptopright = new Vector2d(0, 0);
            double ztopleft, zbottomright,zbottomleft,ztopright;
            Matrix4d resmat = GetResMat();

            if (GetPixel(area.Row0, resmat, ref ptopleft, out ztopleft) &&
                    GetPixel(area.Row3, resmat, ref pbottomright, out zbottomright) &&
                    GetPixel(area.Row1, resmat, ref ptopright, out ztopright) &&
                    GetPixel(area.Row2, resmat, ref pbottomleft, out zbottomleft) )
            {
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

        private double GetBookmarkSize() { return Math.Min(Math.Max(2, 100.0 / _zoom), 1000); }

        Matrix4d GetBookMarkOutline(double x, double y, double z , double bksize)
        {
            if (toolStripButtonPerspective.Checked)
            {
                return new Matrix4d(new Vector4d(x - bksize / 2, y + bksize, z, 1),         // top left
                                new Vector4d(x + bksize / 2, y + bksize, z, 1),         // top right
                                new Vector4d(x - bksize / 2, y, z, 1),                  // bottom left
                                new Vector4d(x + bksize / 2, y, z, 1));                 // bottom right
            }
            else
            {
                return new Matrix4d(new Vector4d(x - bksize / 2, y, z + bksize, 1),         // top left
                                new Vector4d(x + bksize / 2, y, z + bksize, 1),         // top right
                                new Vector4d(x - bksize / 2, y, z, 1),                  // bottom left
                                new Vector4d(x + bksize / 2, y, z, 1));                 // bottom right
            }
        }

        private BookmarkClass GetMouseOverBookmark(int x, int y, out double cursysdistz)
        {
            x = Math.Min(Math.Max(x, 5), glControl.Width - 5);
            y = Math.Min(Math.Max(glControl.Height - y, 5), glControl.Height - 5);

            double bksize = GetBookmarkSize();

            BookmarkClass curbk = null;
            cursysdistz = double.MaxValue;

            foreach (BookmarkClass bc in BookmarkClass.bookmarks)
            {
                //Console.WriteLine("Checking bookmark " + ((bc.Heading != null) ? bc.Heading : bc.StarName));

                double newcursysdistz;
                if (IsWithinRectangle(GetBookMarkOutline(bc.x, bc.y, bc.z, bksize), x, y, out newcursysdistz))
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

            double bksize = GetBookmarkSize();

            SystemClass cursys = null;
            cursysdistz = double.MaxValue;

            if (_visitedSystems == null)
                return null;

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

                        double newcursysdistz;
                        if (IsWithinRectangle(GetBookMarkOutline(lx,ly,lz, bksize), x, y, out newcursysdistz))
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

        private ISystem GetMouseOverSystem(int x, int y, out double cursysdistz)
        {
            x = Math.Min(Math.Max(x, 5), glControl.Width - 5);
            y = Math.Min(Math.Max(glControl.Height - y, 5), glControl.Height - 5);

            StarGrid.TransFormInfo ti = new StarGrid.TransFormInfo(GetResMat(), _znear, glControl.Width, glControl.Height, _zoom);

            Vector3d? posofsystem = _stargrids.FindOverSystem(x, y, out cursysdistz, ti );

            ISystem f = null;

            if (posofsystem != null)
                f = FindSystem(new Vector3d(posofsystem.Value.X, posofsystem.Value.Y, posofsystem.Value.Z));

            return f;
        }

        // system = cursystem!=null, curbookmark = null, notedsystem = false
        // bookmark on system, cursystem!=null, curbookmark != null, notedsystem = false
        // region bookmark. cursystem = null, curbookmark != null, notedsystem = false
        // clicked on note on a system, cursystem!=null,curbookmark=null, notedsystem=true

        private void GetMouseOverItem(int x, int y, out ISystem cursystem,  // can return both, if a system bookmark is clicked..
                                                    out BookmarkClass curbookmark , out bool notedsystem )
        {
            cursystem = null;
            curbookmark = null;
            notedsystem = false;

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

        public ISystem FindSystem(Vector3d pos, SQLiteConnectionED cn = null )
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

        #endregion

    }



}

