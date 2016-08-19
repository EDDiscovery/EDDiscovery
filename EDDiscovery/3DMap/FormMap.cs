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

        const int HELP_VERSION = 4;         // increment this to force help onto the screen of users first time.

        public EDDConfig.MapColoursClass MapColours { get; set; } = EDDConfig.Instance.MapColours;

        private List<IData3DSet> _datasets_finegridlines;
        private List<IData3DSet> _datasets_coarsegridlines;
        private List<IData3DSet> _datasets_gridlinecoords;
        private List<IData3DSet> _datasets_maps;
        private List<IData3DSet> _datasets_selectedsystems;
        private List<IData3DSet> _datasets_visitedsystems;
        private List<IData3DSet> _datasets_bookedmarkedsystems;
        private List<IData3DSet> _datasets_notedsystems;
        private List<IData3DSet> _datasets_galmapobjects;

        StarGrids _stargrids;                   // holds stars

        StarNamesList _starnameslist;           // holds named stars
        Timer _starupdatetimer = new Timer();     // kicks off star naming
        CameraDirectionMovement _lastcameranorm = new CameraDirectionMovement();
        CameraDirectionMovement _lastcamerastarnames = new CameraDirectionMovement();
        bool _starnamesbusy = false;

        private AutoCompleteStringCollection _systemNames;

        private ISystem _centerSystem;
        private ISystem _homeSystem;
        private ISystem _historySelection;

        private ISystem _clickedSystem;         // left clicked on a system/bookmark system/noted system
        private GalacticMapObject _clickedGMO;  // left clicked on a GMO
        private Vector3 _clickedposition = new Vector3(float.NaN,float.NaN,float.NaN);       // above two also set this.. if its a RM, only this is set.
        private string _clickedurl;             // what url is associated..

        private float _zoom = 1.0f;

        private Vector3 _viewtargetpos = Vector3.Zero;          // point where we are viewing. Eye is offset from this by _cameraDir * 1000/_zoom. (prev _cameraPos)
        private Vector3 _cameraDir = Vector3.Zero;
        private float _cameraFov = (float)(Math.PI / 2.0f);     // Camera, in radians, 180/2 = 90 degrees

        private const double ZoomMax = 300;
        private const double ZoomMin = 0.01;
        private const double ZoomFact = 1.2589254117941672104239541063958;

        private Vector3 _cameraActionMovement = Vector3.Zero;
        private Vector3 _cameraActionRotation = Vector3.Zero;

        private float _cameraSlewProgress = 1.0f;               // 0 -> 1 slew progress
        private float _cameraSlewTime;                          // how long to take to do the slew
        private Vector3 _cameraSlewPosition;                    // where to slew to.

        private KeyboardActions _kbdActions = new KeyboardActions();
        private Stopwatch _updateinterval = new Stopwatch();
        private long _lastmstick;
        private int _msticks;

        private Point _mouseStartRotate = new Point(int.MinValue, int.MinValue);        // used to indicate not started for these using mousemove
        private Point _mouseStartTranslateXY = new Point(int.MinValue, int.MinValue);
        private Point _mouseStartTranslateXZ = new Point(int.MinValue, int.MinValue);
        private Point _mouseDownPos;
        private Point _mouseHover;

        Timer _mousehovertick = new Timer();
        System.Windows.Forms.ToolTip _mousehovertooltip = null;

        private float _defaultZoom;
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

        private float _znear;

        private bool _isActivated = false;

        private ToolStripMenuItem _toolstripToggleNamingButton;     // for picking up this option quickly

        public bool Is3DMapsRunning { get { return _stargrids != null; } }

        #endregion

        #region Initialisation

        List<ToolStripMenuItem> galmaptsm = new List<ToolStripMenuItem>();

        public FormMap()
        {
            InitializeComponent();
        }

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

            _defaultZoom = zoom;

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
                    if ( tp.Group == GalMapType.GalMapGroup.Markers )       // only markers for now..
                        toolStripDropDownButtonGalObjects.DropDownItems.Add(AddGalMapButton(tp.Description, tp,null));
                }

                toolStripDropDownButtonGalObjects.DropDownItems.Add(AddGalMapButton("Toggle All", null,null));
                _toolstripToggleNamingButton = AddGalMapButton("Toggle Naming", null, SQLiteDBClass.GetSettingBool("Map3DGMONaming", true));
                toolStripDropDownButtonGalObjects.DropDownItems.Add(_toolstripToggleNamingButton);
            }
        }
        
        public ToolStripMenuItem AddGalMapButton( string name, GalMapType tp, bool? checkedanyway)
        {
            ToolStripMenuItem tsmi = new ToolStripMenuItem();
            tsmi.Text = name; 
            tsmi.Size = new Size(195, 22);
            tsmi.Tag = tp;
            if (tp != null || checkedanyway.HasValue)
            {
                tsmi.CheckState = CheckState.Checked;
                tsmi.CheckOnClick = true;
                tsmi.Checked = (tp!=null) ? tp.Enabled : checkedanyway.Value;
            }
            tsmi.Click += new System.EventHandler(this.showGalacticMapTypeMenuItem_Click);
            return tsmi;
        }


        public void SetPlannedRoute(List<SystemClass> plannedr)
        {
            _plannedRoute = plannedr;
            GenerateDataSetsVisitedSystems();
            Repaint();
        }

        public void SetReferenceSystems(List<SystemClass> refsys)
        {
            _referenceSystems = refsys;
            GenerateDataSetsVisitedSystems();
            Repaint();
        }

        public void UpdateVisitedSystems(List<VisitedSystemsClass> visited)
        {
            if (Is3DMapsRunning && visited != null )         // if null, we are not up and running.  visited should never be null, but being defensive
            {
                _visitedSystems = visited;
                _stargrids.FillVisitedSystems(_visitedSystems);          // update visited systems, will be displayed on next update of star grids
                GenerateDataSetsVisitedSystems();
                _lastcameranorm.ForceZoomChanged();              // this will make it recalc..
                Repaint();

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
                Repaint();
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
                        StartCameraSlew(new Vector3((float)x, (float)y, (float)z));
                }

                Repaint();
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

            _viewtargetpos = new Vector3((float)_centerSystem.x, -(float)_centerSystem.y, (float)_centerSystem.z);
            _cameraDir = Vector3.Zero;
            _zoom = _defaultZoom;
            _lastcameranorm.Update(_cameraDir, _viewtargetpos, _zoom); // set up here so ready for action.. below uses it.
            _lastcameranorm.ForceZoomChanged();                      
            _lastcamerastarnames.Update(_cameraDir, _viewtargetpos, _zoom); // set up here so ready for action.. below uses it.
            _lastcamerastarnames.ForceZoomChanged();                    

            GenerateDataSets();
            GenerateDataSetsMaps();
            GenerateDataSetsSelectedSystems();
            GenerateDataSetsVisitedSystems();

            _updateinterval.Start();
            _lastmstick = _updateinterval.ElapsedMilliseconds;

            _starupdatetimer.Interval = 50;
            _starupdatetimer.Tick += new EventHandler(UpdateStars);
            _starupdatetimer.Start();

            _mousehovertick.Tick += new EventHandler(MouseHoverTick);
            _mousehovertick.Interval = 250;

            SetCenterSystemTo(_centerSystem);                   // move to this..
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
            Repaint();
        }

        private void FormMap_Deactivate(object sender, EventArgs e)
        {
            _isActivated = false;
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
            Repaint();
        }

        private void FormMap_Resize(object sender, EventArgs e)         // resizes changes glcontrol width/height, so needs a new viewport
        {
            SetupViewport();
            Repaint();
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
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
            UpdateDataSetsDueToZoom();
        }

        private void UpdateDataSetsDueToZoom()
        {
            DatasetBuilder builder = new DatasetBuilder();
            if (toolStripButtonFineGrid.Checked)
                builder.UpdateGridZoom(ref _datasets_coarsegridlines, _zoom);

            if (toolStripButtonCoords.Checked)
                builder.UpdateGridCoordZoom(ref _datasets_gridlinecoords, _zoom);

            builder.UpdateGalObjects(ref _datasets_galmapobjects, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), _lastcameranorm.Rotation);

            if (showBookmarksToolStripMenuItem.Checked)
                builder.UpdateBookmarks(ref _datasets_bookedmarkedsystems, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), _lastcameranorm.Rotation);

            if (showNoteMarksToolStripMenuItem.Checked)
                builder.UpdateBookmarks(ref _datasets_notedsystems, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), _lastcameranorm.Rotation);

            if (_clickedGMO != null || _clickedSystem != null)              // if markers
                builder.UpdateSelected(ref _datasets_selectedsystems, _clickedSystem, _clickedGMO, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), _lastcameranorm.Rotation);
        }

        private void UpdateDataSetsDueToFlip()
        {
            DatasetBuilder builder = new DatasetBuilder();
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

            _datasets_visitedsystems = builder.BuildVisitedSystems(toolStripButtonDrawLines.Checked, _centerSystem, filtered,
                                                                    _referenceSystems, _plannedRoute);
        }

        private void GenerateDataSetsSelectedSystems()
        {
            DeleteDataset(ref _datasets_selectedsystems);
            _datasets_selectedsystems = null;
            DatasetBuilder builder = new DatasetBuilder();
            _datasets_selectedsystems = builder.BuildSelected(_centerSystem,_clickedSystem,_clickedGMO, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), _lastcameranorm.Rotation);
        }

        private void GenerateDataSetsBNG()      // because the target is bound up with all three, best to do all three at once in ONE FUNCTION!
        {
            Bitmap maptarget = (Bitmap)EDDiscovery.Properties.Resources.bookmarktarget;

            List<IData3DSet> oldbookmarks = _datasets_bookedmarkedsystems;

            Bitmap mapstar = (Bitmap)EDDiscovery.Properties.Resources.bookmarkgreen;
            Bitmap mapregion = (Bitmap)EDDiscovery.Properties.Resources.bookmarkyellow;

            Debug.Assert(mapstar != null && mapregion != null);

            DatasetBuilder builder1 = new DatasetBuilder();
            _datasets_bookedmarkedsystems = builder1.AddStarBookmarks(mapstar, mapregion, maptarget, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), _lastcameranorm.Rotation);
            DeleteDataset(ref oldbookmarks);

            List<IData3DSet> oldnotedsystems = _datasets_notedsystems;
            Bitmap map = (Bitmap)EDDiscovery.Properties.Resources.bookmarkbrightred;
            Debug.Assert(map != null);

            DatasetBuilder builder2 = new DatasetBuilder();
            _datasets_notedsystems = builder2.AddNotedBookmarks(map, maptarget, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), _lastcameranorm.Rotation, _visitedSystems);
            DeleteDataset(ref oldnotedsystems);

            DatasetBuilder builder3= new DatasetBuilder();

            List<IData3DSet> oldgalmaps = _datasets_galmapobjects;
            _datasets_galmapobjects = builder3.AddGalMapObjectsToDataset(maptarget, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), _lastcameranorm.Rotation , _toolstripToggleNamingButton.Checked);
            DeleteDataset(ref oldgalmaps);

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

        bool _requestrepaint = false;

        private void UpdateStars(object sender, EventArgs e) // tick.. tock.. every X ms.  Drives everything now.
        {
            if (!Visible)
                return;

            long elapsed = _updateinterval.ElapsedMilliseconds;         // stopwatch provides precision timing on last paint time.
            _msticks = (int)(elapsed - _lastmstick);
            _lastmstick = elapsed;

            HandleInputs();
            DoCameraSlew();
            UpdateCamera();

            if (_kbdActions.Any() || (_cameraSlewProgress < 1.0f))          // if we have any future work, start the kick timer..
            {
                //Console.WriteLine("keyboard/slew ");
                _requestrepaint = true;
            }

            if (_stargrids.Update(_viewtargetpos.X, _viewtargetpos.Z, _zoom, glControl))       // at intervals, inform star grids of position, and if it has
            {
                //Console.WriteLine("grids");
                _requestrepaint = true;
            }

            _lastcameranorm.Update(_cameraDir, _viewtargetpos, _zoom);

            if (_lastcameranorm.CameraFlipped)                              // if we flip the camera around, flip some of the graphics
            {
                UpdateDataSetsDueToFlip();
                _requestrepaint = true;
                //Console.WriteLine("flip ");
            }

            //Console.WriteLine("Tick m{0} d{1}", _lastcameranorm.CameraMoved , _lastcameranorm.CameraDirChanged);

            if (!_starnamesbusy)                            // flag indicates work is happening in the background
            {
                bool names = showNamesToolStripMenuItem.Checked;
                bool discs = showDiscsToolStripMenuItem.Checked;

                _lastcamerastarnames.Update(_cameraDir, _viewtargetpos, _zoom);

                if ( (names | discs) && _zoom >= 0.99)  // only when shown, and enabled, and with a good zoom
                {
                    bool flippedorzoom = _lastcamerastarnames.CameraFlipped || _lastcamerastarnames.CameraZoomed;
                    bool movedorcameradirchanged = _lastcamerastarnames.CameraMoved || _lastcamerastarnames.CameraDirChanged;

                    if (_stargrids.IsDisplayed(_viewtargetpos.X, _viewtargetpos.Z) && (flippedorzoom || movedorcameradirchanged))                              // if changed something
                    {
                        _starnamesbusy = true;
                        _starnameslist.Update(_lastcamerastarnames, flippedorzoom, GetResMat(), _znear, names, discs);
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
                _requestrepaint = false;
                glControl.Invalidate();                 // and kick paint - not via the function ON purpose, so we can distinguish between this and others reasons
            }
        }

        public void ChangeNamedStars()                                  // star names finished.. repaint and mark not busy
        {
            _requestrepaint = true;
            _starnamesbusy = false;
            //Console.WriteLine("name");
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
                StartCameraSlew(new Vector3((float)_centerSystem.x, (float)_centerSystem.y, (float)_centerSystem.z));
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
                {
                    _starnameslist.IncreaseStarLimit();
                    _lastcamerastarnames.ForceZoomChanged();              // this will make it recalc..
                }


                if (state[Key.F2])
                {
                    _starnameslist.DecreaseStarLimit();
                    _lastcamerastarnames.ForceZoomChanged();              // this will make it recalc..
                }

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

            var angle = (float)_msticks * 0.075f;
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
            var distance = _msticks * (1.0f / zoomlimited);

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
            var adjustment = 1.0f + ((float)_msticks * 0.01f);
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

#region OpenGL Render and Viewport

        private void SetupViewport()
        {
            GL.MatrixMode(MatrixMode.Projection);           // Select the project matrix for the following operations (current matrix)

            int w = glControl.Width;
            int h = glControl.Height;

            if (w == 0 || h == 0) return;

            if (toolStripButtonPerspective.Checked)
            {                                                                   // Fov, perspective, znear, zfar
                Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(_cameraFov, (float)w / h, 1.0f, 1000000.0f);
                GL.LoadMatrix(ref perspective);             // replace projection matrix with this perspective matrix
                _znear = 1.0f;
            }
            else
            {
                float orthoW = w * (_zoom + 1.0f);
                float orthoH = h * (_zoom + 1.0f);

                float orthoheight = 1000.0f * h / w;

                GL.LoadIdentity();                              // set to 1/1/1/1.
                
                // multiply identity matrix with orth matrix, left/right vert clipping plane, bot/top horiz clippling planes, distance between near/far clipping planes
                GL.Ortho(-1000.0f, 1000.0f, -orthoheight, orthoheight, -5000.0f, 5000.0f);
                _znear = -5000.0f;
            }

            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            //Stopwatch sw1 = new Stopwatch(); sw1.Start();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);            // select the current matrix to the model view

            if (toolStripButtonPerspective.Checked)
            {
                Vector3 target = _viewtargetpos;

                Matrix4 transform = Matrix4.Identity;                   // identity nominal matrix, dir is in degrees
                transform *= Matrix4.CreateRotationZ((float)(_cameraDir.Z * Math.PI / 180.0f));
                transform *= Matrix4.CreateRotationX((float)(_cameraDir.X * Math.PI / 180.0f));
                transform *= Matrix4.CreateRotationY((float)(_cameraDir.Y * Math.PI / 180.0f));
                                                                        // transform ends as the camera direction vector

                // calculate where eye is, relative to target. its 1000/zoom, rotated by camera rotation
                Vector3 eyerel = Vector3.Transform(new Vector3(0.0f, -1000.0f / _zoom, 0.0f), transform);

                // rotate the up vector (0,0,1) by the eye camera dir to get a vector upwards from the current camera dir
                Vector3 up = Vector3.Transform(new Vector3(0.0f, 0.0f, 1.0f), transform);

                Vector3 eye = _viewtargetpos + eyerel;              // eye is here, the target pos, plus the eye relative position
                Matrix4 lookat = Matrix4.LookAt(eye, target, up);   // from eye, look at target, with up giving the rotation of the look
                GL.LoadMatrix(ref lookat);                          // set the model view to this matrix.
            }
            else
            {
                GL.LoadIdentity();                  // model view matrix is 1/1/1/1.
                GL.Rotate(-90.0, 1, 0, 0);          // Rotate the world - current matrix, rotated -90 degrees around the vector (1,0,0)
                GL.Scale(_zoom, _zoom, _zoom);      // scale all the axis to zoom
                GL.Rotate(_cameraDir.Z, 0.0, 0.0, -1.0);    // rotate the axis around the camera dir
                GL.Rotate(_cameraDir.X, -1.0, 0.0, 0.0);
                GL.Rotate(_cameraDir.Y, 0.0, -1.0, 0.0);
                GL.Translate(-_viewtargetpos.X, -_viewtargetpos.Y, -_viewtargetpos.Z);  // and translate the model view by the view target pos
            }

            GL.Scale(1.0, -1.0, 1.0);               // Flip Y axis on world by inverting the model view matrix

            // Render galaxy
            GL.Enable(EnableCap.PointSmooth);
            GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.PushMatrix();
            DrawStars();
            GL.PopMatrix();

            glControl.SwapBuffers();
            UpdateStatus();

            //long elapsed = sw1.ElapsedMilliseconds;
            //Console.WriteLine("{0} Time {1} {2}", Environment.TickCount, elapsed, (elapsed>50) ? "***********************":"");
        }

        private void DrawStars()
        {
            // Take references on objects that could be replaced by the background (?)
            List<IData3DSet> _datasets_galmapobjects = this._datasets_galmapobjects;
            List<IData3DSet> _datasets_notedsystems = this._datasets_notedsystems;
            List<IData3DSet> _datasets_bookmarkedsystems = this._datasets_bookedmarkedsystems;

            if (_datasets_maps == null)     // happens during debug.. paint before form load
                return;

            foreach (var dataset in _datasets_maps)                     // needs to be in order of background to foreground objects
                dataset.DrawAll(glControl);

            Debug.Assert(_datasets_finegridlines != null);
            if (toolStripButtonFineGrid.Checked && _datasets_finegridlines != null )
            {
                foreach (var dataset in _datasets_finegridlines)
                    dataset.DrawAll(glControl);
            }

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

            _stargrids.DrawAll(glControl, showStarstoolStripMenuItem.Checked, showStationsToolStripMenuItem.Checked);

            Debug.Assert(_datasets_galmapobjects != null);
            if (_datasets_galmapobjects != null)
            {
                foreach (var dataset in _datasets_galmapobjects)
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
                _requestrepaint = true;
                //Console.WriteLine("Ask for star paint");
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
            statusLabel.Text = string.Format("x:{0,-6:0} y:{1,-6:0} z:{2,-6:0} Zoom:{3,-4:0.00} FOV:{4,-4:0} Use ? for help", _viewtargetpos.X, -(_viewtargetpos.Y), _viewtargetpos.Z, _zoom , _cameraFov/Math.PI*180);
#if DEBUG
            statusLabel.Text += string.Format("   Direction x={0,-6:0.0} y={1,-6:0.0} z={2,-6:0.0}", _cameraDir.X, _cameraDir.Y, _cameraDir.Z);
#endif
        }

        private void Repaint()
        {
            glControl.Invalidate();                 // and kick paint
            //Console.WriteLine("MANUAL REPAINT!!!!!!!!!!!!! {0}" , Tools.StackTrace(Environment.StackTrace, ".Repaint()", 1));
        }

        #endregion

        #region Camera Slew and Update

        private void StartCameraSlew(Vector3 pos)       // may pass a Nan Position - no action
        {
            if (!float.IsNaN(pos.X))
            {
                double dist = Math.Sqrt((_viewtargetpos.X - pos.X) * (_viewtargetpos.X - pos.X) + (-_viewtargetpos.Y - pos.Y) * (-_viewtargetpos.Y - pos.Y) + (_viewtargetpos.Z - pos.Z) * (_viewtargetpos.Z - pos.Z));

                if (dist >= 1)
                {
                    _cameraSlewPosition = pos;
                    _cameraSlewProgress = 0.0f;
                    _cameraSlewTime = (float)Math.Max(2.0, dist / 10000.0);            //10000 ly/sec, with a minimum slew
                    //Console.WriteLine("Slew " + dist + " in " + _cameraSlewTime);
                    KillHover();
                    _requestrepaint = true;
                }
            }
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
                Debug.Assert(_cameraSlewTime > 0);
                var newprogress = _cameraSlewProgress + _msticks / (_cameraSlewTime * 1000);
                var totvector = new Vector3((float)(_cameraSlewPosition.X - _viewtargetpos.X), (float)(-_cameraSlewPosition.Y - _viewtargetpos.Y), (float)(_cameraSlewPosition.Z - _viewtargetpos.Z));

                if (newprogress >= 1.0f)
                {
                    _viewtargetpos = new Vector3(_cameraSlewPosition.X,-_cameraSlewPosition.Y, _cameraSlewPosition.Z);
                }
                else
                {
                    var slewstart = Math.Sin((_cameraSlewProgress - 0.5) * Math.PI);
                    var slewend = Math.Sin((newprogress - 0.5) * Math.PI);
                    Debug.Assert((1 - 0 - slewstart) != 0);
                    var slewfact = (slewend - slewstart) / (1.0 - slewstart);
                    _viewtargetpos += Vector3.Multiply(totvector, (float)slewfact);
                }

                _requestrepaint = true;
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
                _viewtargetpos += trans;
                _viewtargetpos.Y -= _cameraActionMovement.Z;        // translation appears in Z axis due to way the camera rotation is set up
            }
            else
                _viewtargetpos += trans;
#if DEBUG
//            if (istranslating)
//                Console.WriteLine("   em " + em + " Camera now " + _viewtargetpos.X + "," + _viewtargetpos.Y + "," + _viewtargetpos.Z);
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
            Repaint();
        }

        private void StartPicker_ValueChanged(object sender, EventArgs e)
        {
            startTime = startPicker.Value;
            GenerateDataSetsVisitedSystems();
            Repaint();
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
            Repaint();
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
            Repaint();
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
                SetCenterSystemTo(sys);
            }
            else
            {
                GalacticMapObject gmo = EDDiscoveryForm.galacticMapping.Find(textboxFrom.Text, true, true);    // ignore if its off, find any part of string, find if disabled

                if (gmo != null)
                {
                    textboxFrom.Text = gmo.name;
                    StartCameraSlew(new Vector3((float)gmo.points[0].x, (float)gmo.points[0].y, (float)gmo.points[0].z));
                }
                else
                    MessageBox.Show("System or Object " + textboxFrom.Text + " not found");
            }
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
                SetCenterSystemTo(name);
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
                    SetCenterSystemTo(FindSystem(vs.Name));
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
            Repaint();
        }

        private void showStarstoolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DAllStars", showStarstoolStripMenuItem.Checked);
            Repaint();
        }

        private void showGalacticMapTypeMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tmsi = (ToolStripMenuItem)sender;
            GalMapType tp = (GalMapType)tmsi.Tag;

            if (tp == null )
            {
                if (tmsi.Text.Contains("Naming"))
                {
                    SQLiteDBClass.PutSettingBool("Map3DGMONaming", tmsi.Checked);
                }
                else
                {
                    EDDiscoveryForm.galacticMapping.ToggleEnable();

                    foreach (ToolStripMenuItem ti in toolStripDropDownButtonGalObjects.DropDownItems)
                    {
                        if (ti.Tag != null)
                            ti.Checked = ((GalMapType)ti.Tag).Enabled;
                    }
                }
            }
            else
            {
                EDDiscoveryForm.galacticMapping.ToggleEnable(tp);
            }

            GenerateDataSetsBNG();
            Repaint();
        }

        private void enableColoursToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DButtonColours", enableColoursToolStripMenuItem.Checked);
            _stargrids.ForceWhite = !enableColoursToolStripMenuItem.Checked;
            _lastcamerastarnames.ForceZoomChanged();              // this will make it recalc..
            Repaint();
        }

        private void showStationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DButtonStations", showStationsToolStripMenuItem.Checked);
            Repaint();
        }

        private void toolStripButtonGrid_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DCoarseGrid", toolStripButtonGrid.Checked);
            Repaint();
        }

        private void toolStripButtonFineGrid_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DFineGrid", toolStripButtonFineGrid.Checked);
            UpdateDataSetsDueToZoom();
            Repaint();
        }

        private void toolStripButtonCoords_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DCoords", toolStripButtonCoords.Checked);
            UpdateDataSetsDueToZoom();
            Repaint();
        }

        private void toolStripButtonEliteMovement_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DEliteMove", toolStripButtonEliteMovement.Checked);
        }

        private void showDiscsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DStarDiscs", showDiscsToolStripMenuItem.Checked);
            _lastcamerastarnames.ForceZoomChanged();
        }

        private void showNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DStarNaming", showNamesToolStripMenuItem.Checked);
            _lastcamerastarnames.ForceZoomChanged();
        }

        private void showNoteMarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DShowNoteMarks", showNoteMarksToolStripMenuItem.Checked);
            GenerateDataSetsBNG();
            Repaint();
        }

        private void showBookmarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DShowBookmarks", showBookmarksToolStripMenuItem.Checked);
            GenerateDataSetsBNG();
            Repaint();
        }

        private void newRegionBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookmarkForm frm = new BookmarkForm();
            frm.InitialisePos(_viewtargetpos.X, -(_viewtargetpos.Y), _viewtargetpos.Z);
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
                Repaint();
            }
        }
        
        private void toolStripButtonPerspective_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("Map3DPerspective", toolStripButtonPerspective.Checked);
            SetupViewport();
            GenerateDataSetsBNG();
            Repaint();
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
                StartCameraSlew(_clickedposition);      // if nan, will ignore..
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
            Repaint();
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
                    _requestrepaint = true;
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

                            GenerateDataSetsBNG();
                            _requestrepaint = true;
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

                                GenerateDataSetsBNG();
                                _requestrepaint = true;
                                travelHistoryControl.RefreshTargetInfo();
                            }
                        }
                        else if (res == DialogResult.Abort && bkmark != null)
                        {
                            if (targetid == bkmark.id)
                            {
                                TargetClass.ClearTarget();
                                GenerateDataSetsBNG();
                                _requestrepaint = true;
                                travelHistoryControl.RefreshTargetInfo();
                            }

                            bkmark.Delete();
                        }
                    }

                    GenerateDataSetsBNG();      // in case target changed, do all..
                    _requestrepaint = true;
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
                            _requestrepaint = true;
                            travelHistoryControl.RefreshTargetInfo();
                        }
                    }
                }
            }
        }

        private void glControl_DoubleClick(object sender, EventArgs e)
        {
            StartCameraSlew(_clickedposition);
        }

        private void glControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {                                                                   
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (_mouseStartRotate.X != int.MinValue) // on resize double click resize, we get a stray mousemove with left, so we need to make sure we actually had a down event
                {
                    _cameraSlewProgress = 1.0f;
                    //Console.WriteLine("Mouse move left");
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

                    _requestrepaint = true;
                }

                _mousehovertick.Stop();
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (_mouseStartTranslateXY.X != int.MinValue)
                {
                    _cameraSlewProgress = 1.0f;

                    int dx = e.X - _mouseStartTranslateXY.X;
                    int dy = e.Y - _mouseStartTranslateXY.Y;

                    _mouseStartTranslateXY.X = _mouseStartTranslateXZ.X = e.X;
                    _mouseStartTranslateXY.Y = _mouseStartTranslateXZ.Y = e.Y;
                    //System.Diagnostics.Trace.WriteLine("dx" + dx.ToString() + " dy " + dy.ToString() + " Button " + e.Button.ToString());

                    _viewtargetpos.Y += -dy * (1.0f / _zoom) * 2.0f;

                    _requestrepaint = true;
                }

                _mousehovertick.Stop();
            }
            else if (e.Button == (System.Windows.Forms.MouseButtons.Left | System.Windows.Forms.MouseButtons.Right))
            {
                if (_mouseStartTranslateXZ.X != int.MinValue)
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

                    _viewtargetpos.X += translation.X;
                    _viewtargetpos.Z += translation.Y;

                    _requestrepaint = true;
                }

                _mousehovertick.Stop();
            }
            else //
            {
                if (Math.Abs(e.X - _mouseHover.X) + Math.Abs(e.Y - _mouseHover.Y) > 8)
                    KillHover();                                // we move we kill the hover.

                                                                // no tool tip, not slewing, not ticking..
                if (_mousehovertooltip == null && _cameraSlewProgress >= 1.0F && !_mousehovertick.Enabled)
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

            if (kbdstate[Key.LControl] || kbdstate[Key.RControl])
            {
                if (e.Delta < 0)
                {
                    _cameraFov = (float)Math.Min(_cameraFov * ZoomFact, Math.PI * 0.8);
                }
                else if (e.Delta > 0)
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

                if (curzoom != _zoom)
                    UpdateDataSetsDueToZoom();
            }

            SetupViewport();
            _requestrepaint = true;
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

        private bool GetPixel(Vector4d xyzw, ref Matrix4d resmat, ref Vector2d pixelpos, out double newcursysdistz)
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

        private float GetBitmapOnScreenSizeX() { return (float)Math.Min(Math.Max(2, 80.0 / _zoom), 1000); }
        private float GetBitmapOnScreenSizeY() { return (float)Math.Min(Math.Max(2, 100.0 / _zoom), 1000); }

        private BookmarkClass GetMouseOverBookmark(int x, int y, out double cursysdistz)
        {
            x = Math.Min(Math.Max(x, 5), glControl.Width - 5);
            y = Math.Min(Math.Max(glControl.Height - y, 5), glControl.Height - 5);

            Vector3d[] rotvert = TexturedQuadData.GetVertices(new Vector3d(0, 0, 0), _lastcameranorm.Rotation, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), 0, GetBitmapOnScreenSizeY() / 2);

            BookmarkClass curbk = null;
            cursysdistz = double.MaxValue;
            Matrix4d resmat = GetResMat();

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

            Matrix4d resmat = GetResMat();

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
            Matrix4d resmat = GetResMat();
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

            StarGrid.TransFormInfo ti = new StarGrid.TransFormInfo(GetResMat(), _znear, glControl.Width, glControl.Height, _zoom);

            Vector3? posofsystem = _stargrids.FindOverSystem(x, y, out cursysdistz, ti, showStarstoolStripMenuItem.Checked, showStationsToolStripMenuItem.Checked);

            if ( posofsystem == null )
                posofsystem = _starnameslist.FindOverSystem(x, y, out cursysdistz, ti); // in case these are showing

            ISystem f = null;

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

        #endregion

    }



}

