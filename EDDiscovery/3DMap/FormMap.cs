/*
 * Copyright © 2015 - 2017 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using EDDiscovery;
using EDDiscovery._3DMap;
using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.EDSM;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OpenTKUtils.GL1;
using OpenTKUtils.Common;

namespace EDDiscovery
{
    public partial class FormMap : Forms.DraggableFormPos
    {
        #region Variables

        public bool Is3DMapsRunning { get { return stargrids != null; } }

        public EDDiscoveryForm discoveryForm { get; set; } = null;      // set externally

        const int HELP_VERSION = 5;         // increment this to force help onto the screen of users first time.

        private List<IData3DCollection> datasets_finegridlines;
        private List<IData3DCollection> datasets_coarsegridlines;
        private List<IData3DCollection> datasets_gridlinecoords;
        private List<IData3DCollection> datasets_maps;
        private List<IData3DCollection> datasets_selectedsystems;
        private List<IData3DCollection> datasets_systems;
        private List<IData3DCollection> datasets_routetri;
        private List<IData3DCollection> datasets_bookedmarkedsystems;
        private List<IData3DCollection> datasets_notedsystems;
        private List<IData3DCollection> datasets_galmapobjects;
        private List<IData3DCollection> datasets_galmapregions;

        StarGrids stargrids;                   // holds stars
        StarNamesList starnameslist;           // holds named stars

        private ISystem centerSystem;          // some systems remembered
        private ISystem homeSystem;
        private ISystem historySelection;

        private ISystem clickedSystem;         // left clicked on a system/bookmark system/noted system
        private GalacticMapObject clickedGMO;  // left clicked on a GMO
        private Vector3 clickedposition = new Vector3(float.NaN,float.NaN,float.NaN);       // above two also set this.. if its a RM, only this is set.
        private string clickedurl;             // what url is associated..

        private Position position = new Position();
        private Camera camera = new Camera();
        private Zoom zoom = new Zoom();            // zoom fov holders
        private Fov fov = new Fov();
        MatrixCalc matrixcalc = new MatrixCalc();

        private Timer systemtimer = new Timer();           // MAIN system tick controls most things
        private bool requestrepaint = false;               // set to request repaint on next tick
        private Stopwatch systemtickinterval = new Stopwatch();    // to accurately measure interval between system ticks
        private long lastsystemtickinterval = 0;                   // last update tick at previous update

        private CameraDirectionMovementTracker lastcameranorm = new CameraDirectionMovementTracker();        // these track movements and zoom for most systems
        private CameraDirectionMovementTracker lastcamerastarnames = new CameraDirectionMovementTracker();   // and for star names, which may be delayed due to background busy

        bool gridsupdated = false;                         // Remembers if we doing a grid update, so the star naming can wait for a another tick

        private long lastpainttick = 0;                    // FPS calculation
        private float fps = 0;
        private bool fpson = false;

        private Point mouseStartRotate = new Point(int.MinValue, int.MinValue);        // used to indicate not started for these using mousemove
        private Point mouseStartTranslateXY = new Point(int.MinValue, int.MinValue);
        private Point mouseStartTranslateXZ = new Point(int.MinValue, int.MinValue);
        private Point mouseDownPos;

        bool tooltipon = false;
        private Point tooltipPosition;                      
        System.Windows.Forms.ToolTip controltooltip = new ToolTip();

        public List<HistoryEntry> systemlist { get; set; }
        private List<ISystem> plannedRoute { get; set; }

        public List<BaseUtils.Map2d> fgeimages = new List<BaseUtils.Map2d>();

        public DateTime filterStartTime { get; set; }
        public DateTime filterEndTime { get; set; }

        private bool isActivated = false;
        private float _znear;

        private ToolStripMenuItem toolstripToggleNamingButton;     // for picking up this option quickly
        private ToolStripMenuItem toolstripToggleRegionColouringButton;     // for picking up this option quickly
        
        MapRecorder maprecorder = null;                     // the recorder 
        TimedMessage mapmsg = null;                         // and msg

        BaseUtils.KeyboardState kbdActions = new BaseUtils.KeyboardState();

        bool _allowresizematrixchange = false;           // prevent resize causing matrix calc before paint
        private OpenTK.GLControl glControl;
        private ExtendedControls.InfoForm helpDialog;

        #endregion

        #region External Interface

        public void Prepare(ISystem historysel, string homesys, ISystem centersys, float zoom,
                            List<HistoryEntry> visited)
        {
            ISystem homeSystem = (homesys != null) ? FindSystem(homesys,true) : null;
            Prepare(historysel, homeSystem, centersys, zoom, visited);
        }

        public void Prepare(ISystem historysel, ISystem homesys, ISystem centersys, float zoomf,
                            List<HistoryEntry> visited)
        {
            systemlist = visited;

            historySelection = SafeSystem(historysel);
            homeSystem = SafeSystem(homesys);
            centerSystem = SafeSystem(centersys);

            if (stargrids == null)
            {
                stargrids = new StarGrids();
                stargrids.Initialise();                        // bring up the class..
                starnameslist = new StarNamesList(stargrids, this, glControl);
            }

            zoom.Default = zoomf;

            plannedRoute = null;

            drawLinesBetweenStarsWithPositionToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("Map3DDrawLines", true);
            drawADiscOnStarsWithPositionToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("Map3DDrawTravelDisc", true);
            useWhiteForDiscsInsteadOfAssignedMapColourToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("Map3DDrawTravelWhiteDisc", true);
            showStarstoolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("Map3DAllStars", false);
            showStationsToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("Map3DButtonStations", false);
            toolStripButtonPerspective.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("Map3DPerspective", false);
            toolStripButtonGrid.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("Map3DCoarseGrid", true);
            toolStripButtonFineGrid.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("Map3DFineGrid", true);
            toolStripButtonCoords.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("Map3DCoords", true);
            toolStripButtonEliteMovement.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("Map3DEliteMove", false);
            showNamesToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("Map3DStarNaming", true);
            showDiscsToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("Map3DStarDiscs", true);
            showNoteMarksToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("Map3DShowNoteMarks", true);
            showBookmarksToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("Map3DShowBookmarks", true);
            toolStripButtonAutoForward.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("Map3DAutoForward", false);
            enableColoursToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("Map3DButtonColours", true);
            stargrids.ForceWhite = !enableColoursToolStripMenuItem.Checked;

            stargrids.FillSystemListGrid(systemlist);     // to ensure its updated
            stargrids.Start();

            if (toolStripDropDownButtonGalObjects.DropDownItems.Count == 0)
            {
                Debug.Assert(discoveryForm.galacticMapping.galacticMapTypes != null);

                foreach (GalMapType tp in discoveryForm.galacticMapping.galacticMapTypes)
                {
                    if (tp.Group == GalMapType.GalMapGroup.Markers || tp.Group == GalMapType.GalMapGroup.Regions)       // only markers for now..
                    {
                        toolStripDropDownButtonGalObjects.DropDownItems.Add(AddGalMapButton(tp.Description, tp, tp.Enabled));
                        if (tp.Group == GalMapType.GalMapGroup.Regions)
                        {
                            toolstripToggleRegionColouringButton = AddGalMapButton("Toggle Region Colouring", 2, EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("Map3DGMORegionColouring", true));
                            toolStripDropDownButtonGalObjects.DropDownItems.Add(toolstripToggleRegionColouringButton);
                        }
                    }
                }

                toolStripDropDownButtonGalObjects.DropDownItems.Add(AddGalMapButton("Toggle All", 0, null));

                toolstripToggleNamingButton = AddGalMapButton("Toggle Star Naming", 1, EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("Map3DGMONaming", true));
                toolStripDropDownButtonGalObjects.DropDownItems.Add(toolstripToggleNamingButton);
            }

            maprecorder.UpdateStoredVideosToolButton(toolStripDropDownRecord, LoadVideo, Icons.Controls.Map3D_Recorder_Save);

            discoveryForm.OnNewTarget -= UpdateTarget;  // in case called multi times
            discoveryForm.OnNewTarget += UpdateTarget;
            discoveryForm.OnNoteChanged -= UpdateNotes;
            discoveryForm.OnNoteChanged += UpdateNotes;
            discoveryForm.OnHistoryChange -= UpdateSystemListHC;   // refresh, update the system list..
            discoveryForm.OnHistoryChange += UpdateSystemListHC;   // refresh, update the system list..
            discoveryForm.OnNewEntry -= UpdateSystemList;   // any new entries, update the system list..
            discoveryForm.OnNewEntry += UpdateSystemList;
            discoveryForm.PrimaryCursor.OnTravelSelectionChanged -= PrimaryCursor_OnTravelSelectionChanged;
            discoveryForm.PrimaryCursor.OnTravelSelectionChanged += PrimaryCursor_OnTravelSelectionChanged;

            controltooltip.InitialDelay = 250;
            controltooltip.AutoPopDelay = 30000;
            controltooltip.ReshowDelay = 500;
            controltooltip.IsBalloon = true;

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

        public void SetPlannedRoute(List<ISystem> plannedr)
        {
            plannedRoute = plannedr;
            GenerateDataSetsRouteTri();
            RequestPaint();
        }

        private void PrimaryCursor_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            UpdateHistorySystem(he.System);
        }

        public void UpdateHistorySystem(ISystem historysel)
        {
            if (Is3DMapsRunning && historysel != null )         // if null, we are not up and running
            {
                historySelection = historysel;       
            }
        }

        public void UpdateNotes(Object sender,HistoryEntry he, bool committed)        // tested, 20 july 17, seen notes appear/disappear as edited.
        {
            if (Is3DMapsRunning && committed)         // if null, we are not up and running, and also if we are committing (don't update just because its being typed in)
            {
                GenerateDataSetsBNG();  // will create them at correct size
                RequestPaint();
            }
        }

        public void UpdateTarget(Object sender)
        { 
            if (Is3DMapsRunning)         // if null, we are not up and running
            {
                System.Diagnostics.Debug.WriteLine("3dmap Refresh target");

                GenerateDataSetsBNG();
                RequestPaint();
            }
        }

        public void UpdateSystemListHC(HistoryList hl)
        {
            UpdateSystemList(null, hl);
        }

        public void UpdateSystemList(HistoryEntry notused, HistoryList hl)
        {
            if (Is3DMapsRunning )
            {
                List<HistoryEntry> hfsd = hl.FilterByTravel;

                if (hfsd.Count > 0)
                {
                    systemlist = hfsd;
                    stargrids.FillSystemListGrid(systemlist);          // update visited systems, will be displayed on next update of star grids
                    GenerateDataSetsSystemList();
                    RequestPaint();

                    if (toolStripButtonAutoForward.Checked)             // auto forward?
                    {
                        HistoryEntry vs = systemlist.FindLast(x => x.System.HasCoordinate);

                        if (vs != null)
                            SetCenterSystemTo(vs.System.Name);
                    }
                }
            }
        }

        public void IconSelect(bool winborder)      // called to set up icons
        {
            panel_minimize.Visible = panel_close.Visible = !winborder;
            int spaceforclose = winborder ? 0 : (panel_close.Right - panel_minimize.Left) + 16;
            panelAuxControls.Left = ClientRectangle.Width - panelAuxControls.Width - spaceforclose;
        }


        #endregion

        #region Initialisation

        public FormMap()
        {
            RestoreFormPositionRegKey = "Map3DForm";
            InitializeComponent();
            maprecorder = new MapRecorder(this);
            // 
            // glControl
            // 
            this.glControlContainer.SuspendLayout();
            this.glControl = new GLControl();
            this.glControl.Dock = DockStyle.Fill;
            this.glControl.BackColor = System.Drawing.Color.Black;
            this.glControl.Name = "glControl";
            this.glControl.TabIndex = 0;
            this.glControl.VSync = true;
            this.glControl.Load += new System.EventHandler(this.glControl_Load);
            this.glControl.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl_Paint);
            this.glControl.DoubleClick += new System.EventHandler(this.glControl_DoubleClick);
            this.glControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseDown);
            this.glControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseMove);
            this.glControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseUp);
            this.glControl.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.glControl_OnMouseWheel);
            this.glControlContainer.Controls.Add(this.glControl);
            this.glControlContainer.ResumeLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            glControl.KeyDown += FormMap_KeyDown;
            glControl.KeyUp += FormMap_KeyUp;


            LoadMapImages();
            FillExpeditions();
            SetCenterSystemLabel();
            labelClickedSystemCoords.Text = "Click a star to select/copy, double-click to center";

            camera.Set(new Vector3(toolStripButtonPerspective.Checked ? 45 : 0, 0, 0));

            zoom.SetDefault();
            lastcameranorm.Update(camera.Current, position.Current, zoom.Current,1.0F); // set up here so ready for action.. below uses it.
            lastcamerastarnames.Update(camera.Current, position.Current, zoom.Current,1.0F); // set up here so ready for action.. below uses it.

            GenerateDataSets();
            GenerateDataSetsMaps();
            GenerateDataSetsSelectedSystems();
            GenerateDataSetsSystemList();
            GenerateDataSetsRouteTri();

           // mousehovertick.Tick += new EventHandler(MouseHoverTick);
          //  mousehovertick.Interval = 250;

            SetCenterSystemTo(centerSystem);                   // move to this..

            textboxFrom.SetAutoCompletor(SystemCache.ReturnSystemAdditionalListForAutoComplete);
        }

        private void FormMap_KeyUp(object sender, KeyEventArgs e)
        {
            kbdActions.KeyUp(e.Control, e.Shift, e.Alt, e.KeyCode);
        }

        private void FormMap_KeyDown(object sender, KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Key down" + e.KeyCode);
            kbdActions.KeyDown(e.Control, e.Shift, e.Alt, e.KeyCode);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            int helpno = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("Map3DShownHelp", 0);                 // force up help, to make sure they know it exists

            if (helpno != HELP_VERSION)
            {
                toolStripButtonHelp_Click(toolStripButtonHelp, EventArgs.Empty);
                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("Map3DShownHelp", HELP_VERSION);
            }

            SetModelProjectionMatrix();
            _allowresizematrixchange = true;
            StartSystemTimer();

            glControl.Focus();
        }

        private void StartSystemTimer()
        {
            if ( !systemtimer.Enabled )
            {
                systemtickinterval.Stop();
                systemtickinterval.Start();
                lastsystemtickinterval = systemtickinterval.ElapsedMilliseconds;
                systemtimer.Interval = 25;
                systemtimer.Tick += new EventHandler(SystemTick);
                systemtimer.Start();
            }
        }

        protected override void OnResize(EventArgs e)           // resizes changes glcontrol width/height, so needs a new viewport
        {
            base.OnResize(e);

            if (_allowresizematrixchange)
            {
                SetModelProjectionMatrix();
                RequestPaint();
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            isActivated = true;
            StartSystemTimer();                     // in case Close, then open, only get activated
            RequestPaint();
            glControl.Focus();
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);

            isActivated = false;
            VideoMessage();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            Trace.WriteLine($"{Environment.TickCount} Close form");

            systemtimer.Stop();
            systemtickinterval.Stop();
            VideoMessage();

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("Map3DAutoForward", toolStripButtonAutoForward.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("Map3DDrawLines", drawLinesBetweenStarsWithPositionToolStripMenuItem.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("Map3DDrawTravelDisc", drawADiscOnStarsWithPositionToolStripMenuItem.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("Map3DDrawTravelWhiteDisc", useWhiteForDiscsInsteadOfAssignedMapColourToolStripMenuItem.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("Map3DAllStars", showStarstoolStripMenuItem.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("Map3DButtonColours", enableColoursToolStripMenuItem.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("Map3DButtonStations", showStationsToolStripMenuItem.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("Map3DCoarseGrid", toolStripButtonGrid.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("Map3DFineGrid", toolStripButtonFineGrid.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("Map3DCoords", toolStripButtonCoords.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("Map3DEliteMove", toolStripButtonEliteMovement.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("Map3DStarDiscs", showDiscsToolStripMenuItem.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("Map3DStarNaming", showNamesToolStripMenuItem.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("Map3DShowNoteMarks", showNoteMarksToolStripMenuItem.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("Map3DShowBookmarks", showBookmarksToolStripMenuItem.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("Map3DPerspective", toolStripButtonPerspective.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("Map3DGMONaming", toolstripToggleNamingButton.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("Map3DGMORegionColouring", toolstripToggleRegionColouringButton.Checked);
            discoveryForm.galacticMapping.SaveSettings();

            stargrids.Stop();

            e.Cancel = true;
            this.Hide();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            helpDialog?.Dispose();
        }

        protected override void OnTopMostChanged(EventArgs e)
        {
            base.OnTopMostChanged(e);

            if (helpDialog != null)
                helpDialog.TopMost = this.TopMost;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (helpDialog != null)
                helpDialog.Visible = this.Visible;
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            GL.ClearColor((Color)System.Drawing.ColorTranslator.FromHtml("#0D0D10"));
        }

        private void SetModelProjectionMatrix()
        {
            matrixcalc.InPerspectiveMode = toolStripButtonPerspective.Checked;

            matrixcalc.CalculateProjectionMatrix(fov.Current, glControl.Width, glControl.Height, out _znear);
            matrixcalc.CalculateModelMatrix(position.Current,camera.Current,zoom.Current);

            GL.MatrixMode(MatrixMode.Projection);           // Select the project matrix for the following operations (current matrix)
            Matrix4 pm = matrixcalc.ProjectionMatrix;
            GL.LoadMatrix(ref pm);                          // replace projection matrix with this perspective matrix
            GL.Viewport(0, 0, glControl.Width, glControl.Height);                        // Use all of the glControl painting area

            UpdateStatus();                                 // because FOV could have changed.. FOV causes a call to this
        }

        #endregion

        #region Main Timer


        private void SystemTick(object sender, EventArgs e)                 // tick.. tock.. every X ms.  Drives everything now.
        {
            if (!Visible)
                return;

            long elapsed = systemtickinterval.ElapsedMilliseconds;         // stopwatch provides precision timing on last paint time.
            int msticks = (int)(elapsed - lastsystemtickinterval);
            lastsystemtickinterval = elapsed;

            if (isActivated && glControl.Focused)                           // if we can accept keys
            {
                if (kbdActions.IsAnyCurrentlyOrHasBeenPressed())                     // if any actions..
                {
                    //System.Diagnostics.Debug.WriteLine("Keys pressed");

                    float zoomlimited = Math.Min(Math.Max(zoom.Current, 0.01F), 15.0F);
                    if ( StandardKeyboardHandler.Movement(kbdActions, position, matrixcalc.InPerspectiveMode, camera.Current, msticks * (1.0f / zoomlimited), toolStripButtonEliteMovement.Checked) )
                    {
                        position.KillSlew();
                        camera.KillSlew();
                        zoom.KillSlew();
                    }

                    if ( StandardKeyboardHandler.Camera(kbdActions, camera, msticks) )
                    {
                        position.KillSlew();
                        camera.KillSlew();
                        zoom.KillSlew();
                    }

                    if (StandardKeyboardHandler.Zoom(kbdActions, zoom, msticks))
                    {
                        position.KillSlew();
                        camera.KillSlew();
                    }

                    HandleSpecialKeys(kbdActions);                     // special keys for us

                    kbdActions.ClearHasBeenPressed();
                }
            }
            else
            {
                kbdActions.Reset();
            }

            position.DoSlew(msticks);
            camera.DoSlew(msticks);
            zoom.DoSlew();

            if (maprecorder.InPlayBack)
            {
                MapRecorder.FlightEntry fe;
                if (maprecorder.PlayBack(out fe, position.InSlew || camera.InSlew || zoom.InSlew))
                {
                    //Console.WriteLine("{0} Playback {1} {2} {3} fly {4} pan {5} msg {6}", _systemtickinterval.ElapsedMilliseconds % 10000,
                    //    newpos, newdir, newzoom, timetofly, timetopan, message);

                    if ( fe.IsPosSet )
                        position.GoTo(fe.pos, (float)fe.timetofly / 1000.0F);

                    if ( fe.IsDirSet )
                        camera.Pan(fe.dir, (float)fe.timetopan / 1000.0F);

                    if ( fe.IsZoomSet)
                        zoom.GoTo(fe.zoom, (float)fe.timetozoom / 1000.0F);

                    if ( fe.IsMessageSet )
                        VideoMessage( fe.message, ( fe.messagetime == 0 ) ? 3000 : (int)fe.messagetime );
                }

                if (!maprecorder.InPlayBack)  // dropped out of playback?
                    SetDropDownRecordImage();
            }
            else if (maprecorder.Recording)
            {
                maprecorder.Record(position.Current, camera.Current, zoom.Current);
            }

            lastcameranorm.Update(camera.Current, position.Current, zoom.Current, 10.0F);       // Gross limit allows you not to repaint due to a small movement. I've set it to all the time for now, prefer the smoothness to the frame rate.

            //Console.WriteLine("Tick D/Z/M {0} {1} {2}", _lastcameranorm.CameraDirChanged, _lastcameranorm.CameraZoomed, _lastcameranorm.CameraMoved);
            //Tools.LogToFile(String.Format("Tick D/Z/M {0} {1} {2}", _lastcameranorm.CameraDirChanged, _lastcameranorm.CameraZoomed, _lastcameranorm.CameraMoved));

            if (lastcameranorm.AnythingChanged )
            {
                matrixcalc.CalculateModelMatrix(position.Current,camera.Current,zoom.Current);
                requestrepaint = true;

                if (lastcameranorm.CameraZoomed)       // zoom always updates
                {
                    lastcameranorm.SetGrossChanged();                  // make sure gross does not trip yet..     
                    UpdateDataSetsDueToZoomOrFlip(lastcameranorm.CameraZoomed);
                    //System.Diagnostics.Debug.WriteLine("{0}  ZOOM repaint", systemtickinterval.ElapsedMilliseconds);
                }
                else if (lastcameranorm.CameraDirGrossChanged )
                {
                    UpdateDataSetsDueToZoomOrFlip(lastcameranorm.CameraZoomed);
                }
                KillToolTip();
                UpdateStatus();
            }
            else
            {
                if ( fpson )
                    UpdateStatus();
            }

            gridsupdated = stargrids.Update(position.Current.X, position.Current.Z, zoom.Current, glControl);       // at intervals, inform star grids of position, and if it has

            if ( gridsupdated )                                        // if we did work.. repaint. do not do starname in same tick
            {
                requestrepaint = true;
            }
            else if (!starnameslist.Busy)                            // flag indicates background task is running..
            {
                bool names = showNamesToolStripMenuItem.Checked;
                bool discs = showDiscsToolStripMenuItem.Checked;

                if ((names | discs) && zoom.Current >= 0.99 )        // when shown, and with a good zoom, and something is there..
                {                                                   
                    lastcamerastarnames.Update(camera.Current, position.Current, zoom.Current, 1.0F);
                    
                                                                     // if camera moved/zoomed/dir, or display recalculated in this grid                                              
                    if (lastcamerastarnames.AnythingChanged || stargrids.IsDisplayChanged(position.Current.X, position.Current.Z))    
                    {
                        //Console.WriteLine("{0} Check at {1}", _systemtickinterval.ElapsedMilliseconds, position.Current);
                        starnameslist.Update(lastcamerastarnames, matrixcalc.GetResMat, _znear, names, discs,
                                enableColoursToolStripMenuItem.Checked ? Color.White : Color.Orange);
                    }
                }
                else
                {
                    if (starnameslist.HideAll())
                    {
                        requestrepaint = true;
                    }

                    lastcamerastarnames.ForceMoveChange(); // next time we are on, reestimate immediately.
                }
            }

            if (requestrepaint)
            {
                requestrepaint = false;
                glControl.Invalidate();                 // and kick paint 
                //System.Diagnostics.Debug.WriteLine("Repaint");
            }
        }

        public void ChangeNamedStars()                  // background estimator finished.. transfer to foreground and if new stars, repaint
        {
            if ( starnameslist.TransferToForeground() )      // move the stars found to the foreground list..  if any, repaint
                requestrepaint = true;
        }

        private void RequestPaint()                     // Call if any other system changed stuff .. 
        {
            requestrepaint = true;                     // repaint
        }

        private void RequestStarNameRecalc()                    // objects changed.. check size
        {
            lastcamerastarnames.ForceMoveChange();             // make it reestimate the objects by indicating a move
        }


        public void HandleSpecialKeys(BaseUtils.KeyboardState _kbdActions)
        {
            if (_kbdActions.HasBeenPressed(Keys.F3))
            {
                starnameslist.IncreaseStarLimit();
                RequestStarNameRecalc();
            }
            if (_kbdActions.HasBeenPressed(Keys.F4))
            {
                starnameslist.DecreaseStarLimit();
                RequestStarNameRecalc();
            }
            if (_kbdActions.HasBeenPressed(Keys.F5))
            {
                maprecorder.ToggleRecord(false);
                SetDropDownRecordImage();
            }
            if (_kbdActions.HasBeenPressed(Keys.F6))
            {
                maprecorder.ToggleRecord(true);
                SetDropDownRecordImage();
            }
            if (_kbdActions.HasBeenPressed(Keys.F7))
            {
                maprecorder.RecordStepDialog(position.Current, camera.Current, zoom.Current);
            }
            if (_kbdActions.HasBeenPressed(Keys.F8))
            {
                maprecorder.TogglePause();
                SetDropDownRecordImage();
            }
            if (_kbdActions.HasBeenPressed(Keys.F9))
            {
                maprecorder.TogglePlayBack();
                SetDropDownRecordImage();
            }
            if (_kbdActions.HasBeenPressed(Keys.F1))
            {
                toolStripButtonHelp_Click(null, null);
            }
            if (_kbdActions.HasBeenPressed(Keys.F) && _kbdActions.Ctrl)
            {
                fpson = !fpson;
                UpdateStatus();
            }
        }

        private void UpdateStatus()
        {
            string txt;
            
            if ( fpson )
                txt = string.Format("x:{0,-6:0} y:{1,-6:0} z:{2,-6:0} Zoom:{3,-4:0.00} FOV:{4,-3:0} FPS:{5,-4:0.0} Use F1 for help", position.Current.X, position.Current.Y, position.Current.Z, zoom.Current, fov.FovDeg, fps);
            else
                txt = string.Format("x:{0,-6:0} y:{1,-6:0} z:{2,-6:0} Zoom:{3,-4:0.00} FOV:{4,-3:0} Use F1 for help", position.Current.X, position.Current.Y, position.Current.Z, zoom.Current, fov.FovDeg);
#if DEBUG
            txt += string.Format("   Direction x={0,-6:0.0} y={1,-6:0.0} z={2,-6:0.0}", camera.Current.X, camera.Current.Y, camera.Current.Z);
#endif
            statusLabel.Text = txt;
        }

        void VideoMessage( string msg = null , int time = 0 )       // no paras, close it
        {
            mapmsg?.Close();
            mapmsg?.Dispose();
            mapmsg = null;

            if (!string.IsNullOrEmpty(msg) && time > 0)
            {
                TimedMessage newmsg = new TimedMessage();
                newmsg.Init("", msg, time, true, 0.9F, Color.Black, Color.White, BaseUtils.FontLoader.GetFont("MS Sans Serif", 20.0F));
                newmsg.Position(this, 0, 0, -1, -20, 0, 0);         // careful, it triggers a deactivate.. which tries to close it
                newmsg.Show(this);
                mapmsg = newmsg;                                    // now we can set this.. if we did it above, we would end with a race condition on a null pointer of this object
                glControl.Focus();
            }
        }

        #endregion

        #region OpenGL Render and Viewport

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            long curtime = systemtickinterval.ElapsedMilliseconds;
            long timesince = curtime - lastpainttick;
            lastpainttick = curtime;

            if (timesince > 0 && timesince < 100)                            // FPS calc - not less than 10hz. and check for silly 0
            {
                float newfps = 1000.0F / (float)timesince;
                fps = (fps * 0.9F) + (newfps * 0.1F);
            }

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);            // select the current matrix to the model view

            Matrix4 mm = matrixcalc.ModelMatrix;
            GL.LoadMatrix(ref mm);                          // set the model view to this matrix.

            GL.Enable(EnableCap.PointSmooth);                                               // Render galaxy
            GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.PushMatrix();
            DrawStars();
            GL.PopMatrix();

            glControl.SwapBuffers();

            //            Console.WriteLine("{0} Paint since {1} took {2}", curtime % 10000, timesince, _systemtickinterval.ElapsedMilliseconds - curtime);
            //Tools.LogToFile(String.Format("{0} Paint since {1} took {2} {3} {4}", curtime % 10000, timesince, _systemtickinterval.ElapsedMilliseconds - curtime, _fps, (timesince > 60) ? "************************************" : ""));
        }

        private void DrawStars()
        {
            // Take references on objects that could be replaced by the background (?)
            List<IData3DCollection> _datasets_galmapobjects = this.datasets_galmapobjects;
            List<IData3DCollection> _datasets_notedsystems = this.datasets_notedsystems;
            List<IData3DCollection> _datasets_bookmarkedsystems = this.datasets_bookedmarkedsystems;

            if (datasets_maps == null)                     // happens during debug.. paint before form load
                return;                                     //  needs to be in order of background to foreground objects

            //long t1 = _systemtickinterval.ElapsedMilliseconds;

            Debug.Assert(datasets_finegridlines != null);
            if (toolStripButtonFineGrid.Checked && datasets_finegridlines != null )
            {
                foreach (var dataset in datasets_finegridlines)      
                    dataset.DrawAll(glControl);
            }

            //long t2 = _systemtickinterval.ElapsedMilliseconds;

            if (datasets_galmapregions != null)
            {
                datasets_galmapregions[0].DrawAll(glControl);          
            }

           // long t3 = _systemtickinterval.ElapsedMilliseconds;

            foreach (var dataset in datasets_maps)                     
                dataset.DrawAll(glControl);

            //long t4 = _systemtickinterval.ElapsedMilliseconds;

            Debug.Assert(datasets_coarsegridlines != null);
            if (toolStripButtonGrid.Checked && datasets_coarsegridlines != null )
            {
                foreach (var dataset in datasets_coarsegridlines)
                    dataset.DrawAll(glControl);
            }

            //long t5 = _systemtickinterval.ElapsedMilliseconds;

            Debug.Assert(datasets_gridlinecoords != null);
            if (toolStripButtonCoords.Checked && datasets_gridlinecoords != null )
            {
                foreach (var dataset in datasets_gridlinecoords)
                    dataset.DrawAll(glControl);
            }

            if (datasets_galmapregions != null)
            {
                datasets_galmapregions[1].DrawAll(glControl);          // next one is the outlines
                datasets_galmapregions[2].DrawAll(glControl);          // and the names
            }

            //long t6 = _systemtickinterval.ElapsedMilliseconds;

            stargrids.DrawAll(glControl, showStarstoolStripMenuItem.Checked, showStationsToolStripMenuItem.Checked);

           // long t7 = _systemtickinterval.ElapsedMilliseconds;

            Debug.Assert(_datasets_galmapobjects != null);
            if (_datasets_galmapobjects != null)
            {
                foreach (var dataset in _datasets_galmapobjects)
                    dataset.DrawAll(glControl);
            }

            Debug.Assert(datasets_routetri != null);
            if (datasets_routetri != null)
            {
                foreach (var dataset in datasets_routetri)
                    dataset.DrawAll(glControl);
            }

            Debug.Assert(datasets_systems != null);
            if (datasets_systems != null)
            {
                foreach (var dataset in datasets_systems)
                    dataset.DrawAll(glControl);
            }

            //long t8 = _systemtickinterval.ElapsedMilliseconds;

            if (starnameslist.Draw(!gridsupdated, lastcameranorm.LastZoom , lastcameranorm.Rotation ))      // draw star names, pass if you can update the textures , pass back if it needs more work
            {
                requestrepaint = true;         // DONT invalidate.. this makes this thing go around and around and the main tick never gets a look in
            }

            //long t9 = _systemtickinterval.ElapsedMilliseconds;
            
            Debug.Assert(datasets_selectedsystems != null);
            if (datasets_selectedsystems != null)
            {
                foreach (var dataset in datasets_selectedsystems)
                    dataset.DrawAll(glControl);
            }

            Debug.Assert(_datasets_notedsystems != null);
            if (_datasets_notedsystems != null && showNoteMarksToolStripMenuItem.Checked)
            {
                foreach (var dataset in _datasets_notedsystems)                     // needs to be in order of background to foreground objects
                    dataset.DrawAll(glControl);
            }

            Debug.Assert(datasets_bookedmarkedsystems != null);
            if (datasets_bookedmarkedsystems != null && showBookmarksToolStripMenuItem.Checked)
            {
                foreach (var dataset in datasets_bookedmarkedsystems)                     // needs to be in order of background to foreground objects
                    dataset.DrawAll(glControl);
            }

            //long t10 = _systemtickinterval.ElapsedMilliseconds;
            //Tools.LogToFile(String.Format("gridwork {0} {1} {2} {3} {4} {5} {6} {7} {8} {9}", _gridsupdated, t2-t1,t3-t2,t4-t3,t5-t4,t6-t5,t7-t6,t8-t7,t9-t8,t10-t9));

        }

        #endregion

        #region Generate Data Sets

        private void GenerateDataSets()         // Called ONCE only during Load.. fixed data.
        {
            DatasetBuilder builder1 = new DatasetBuilder();
            datasets_coarsegridlines = builder1.AddCoarseGridLines();

            DatasetBuilder builder2 = new DatasetBuilder();
            datasets_finegridlines = builder2.AddFineGridLines();

            DatasetBuilder builder3 = new DatasetBuilder();
            datasets_gridlinecoords = builder3.AddGridCoords();

            GenerateDataSetsBNG();
            UpdateDataSetsDueToZoomOrFlip(true);
        }

        private float GetBitmapOnScreenSizeX() { return (float)Math.Min(Math.Max(2, 80.0 / zoom.Current), 400); }
        private float GetBitmapOnScreenSizeY() { return (float)Math.Min(Math.Max(2 * 1.2F, 100.0 / zoom.Current), 400); }
        private float GetBitmapOnScreenSizeXSel() { return (float)Math.Min(Math.Max(2, 80.0 / zoom.Current), 800); }
        private float GetBitmapOnScreenSizeYSel() { return (float)Math.Min(Math.Max(2 * 1.2F, 100.0 / zoom.Current), 800); }

        private void UpdateDataSetsDueToZoomOrFlip(bool zoommoved)
        {
            //Console.WriteLine("Update due to zoom {0} or flip ", zoommoved);

            DatasetBuilder builder = new DatasetBuilder();

            if (zoommoved)                    // zoom affected objects only
            {
                if (toolStripButtonFineGrid.Checked)
                    builder.UpdateGridZoom(ref datasets_coarsegridlines, zoom.Current);

                if (toolStripButtonCoords.Checked)
                    builder.UpdateGridCoordZoom(ref datasets_gridlinecoords, zoom.Current);
            }

            System.Diagnostics.Debug.WriteLine("Update gal rotate " + lastcameranorm.Rotation);
            builder.UpdateGalObjects(ref datasets_galmapobjects, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), lastcameranorm.Rotation);

            if (showBookmarksToolStripMenuItem.Checked)
                builder.UpdateBookmarks(ref datasets_bookedmarkedsystems, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), lastcameranorm.Rotation);

            if (showNoteMarksToolStripMenuItem.Checked)
                builder.UpdateBookmarks(ref datasets_notedsystems, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), lastcameranorm.Rotation);

            if (clickedGMO != null || clickedSystem != null)              // if markers
                builder.UpdateSelected(ref datasets_selectedsystems, clickedSystem, clickedGMO, GetBitmapOnScreenSizeXSel(), GetBitmapOnScreenSizeYSel(), lastcameranorm.Rotation);
        }

        private void GenerateDataSetsMaps()
        {
            DeleteDataset(ref datasets_maps);
            datasets_maps = null;

            BaseUtils.Map2d[] _selected = dropdownMapNames.DropDownItems.OfType<ToolStripButton>().Where(b => b.Checked).Select(b => b.Tag as BaseUtils.Map2d).ToArray();

            DatasetBuilder builder = new DatasetBuilder();
            datasets_maps = builder.AddMapImages(_selected);
        }

        private void GenerateDataSetsSystemList()
        {
            DeleteDataset(ref datasets_systems);
            datasets_systems = null;

            DatasetBuilder builder = new DatasetBuilder();

            List<HistoryEntry> filtered = systemlist.Where(s => s.EventTimeUTC.ToLocalTime() >= filterStartTime && s.EventTimeUTC.ToLocalTime() <= filterEndTime && s.MultiPlayer == false).OrderBy(s => s.EventTimeUTC).ToList();

            datasets_systems = builder.BuildSystems(drawLinesBetweenStarsWithPositionToolStripMenuItem.Checked,
                            drawADiscOnStarsWithPositionToolStripMenuItem.Checked,
                            useWhiteForDiscsInsteadOfAssignedMapColourToolStripMenuItem.Checked ? Color.White : Color.Transparent,
                            filtered);
        }

        private void GenerateDataSetsRouteTri()
        {
            DeleteDataset(ref datasets_routetri);
            datasets_routetri = null;

            DatasetBuilder builder = new DatasetBuilder();

            datasets_routetri = builder.BuildRouteTri(plannedRoute);
        }

        private void GenerateDataSetsSelectedSystems()
        {
            DeleteDataset(ref datasets_selectedsystems);
            datasets_selectedsystems = null;
            DatasetBuilder builder = new DatasetBuilder();
            datasets_selectedsystems = builder.BuildSelected(centerSystem, clickedSystem, clickedGMO, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), lastcameranorm.Rotation);
            Debug.Assert(datasets_selectedsystems != null);
        }

        private void GenerateDataSetsBNG()      // because the target is bound up with all three, best to do all three at once in ONE FUNCTION!
        {
            Bitmap maptarget = (Bitmap)Icons.Controls.Map3D_Bookmarks_Target;
            Bitmap mapstar = (Bitmap)Icons.Controls.Map3D_Bookmarks_Star;
            Bitmap mapsurface = (Bitmap)Icons.Controls.Map3d_Bookmarks_StarWithPlanets;
            Bitmap mapregion = (Bitmap)Icons.Controls.Map3D_Bookmarks_Region;
            Bitmap mapnotedbkmark = (Bitmap)Icons.Controls.Map3D_Bookmarks_Noted;
            Debug.Assert(mapnotedbkmark != null && maptarget != null);
            Debug.Assert(mapstar != null && mapregion != null);

            List<IData3DCollection> oldbookmarks = datasets_bookedmarkedsystems;
            DatasetBuilder builder1 = new DatasetBuilder();
            datasets_bookedmarkedsystems = builder1.AddStarBookmarks(mapstar, mapregion, maptarget, mapsurface, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), lastcameranorm.Rotation);
            DeleteDataset(ref oldbookmarks);

            List<IData3DCollection> oldnotedsystems = datasets_notedsystems;
            DatasetBuilder builder2 = new DatasetBuilder();
            datasets_notedsystems = builder2.AddNotedBookmarks(mapnotedbkmark, maptarget, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), lastcameranorm.Rotation, systemlist);
            DeleteDataset(ref oldnotedsystems);

            DatasetBuilder builder3 = new DatasetBuilder();
            List<IData3DCollection> oldgalmaps = datasets_galmapobjects;
            datasets_galmapobjects = builder3.AddGalMapObjectsToDataset(discoveryForm.galacticMapping, maptarget, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), lastcameranorm.Rotation, toolstripToggleNamingButton.Checked, enableColoursToolStripMenuItem.Checked ? Color.White : Color.Orange );
            DeleteDataset(ref oldgalmaps);

            DatasetBuilder builder4 = new DatasetBuilder();
            List<IData3DCollection> oldgalreg = datasets_galmapregions;
            datasets_galmapregions = builder4.AddGalMapRegionsToDataset(discoveryForm.galacticMapping, toolstripToggleRegionColouringButton.Checked);
            DeleteDataset(ref oldgalreg);

            if (clickedGMO != null)              // if GMO marked.
                GenerateDataSetsSelectedSystems();          // as GMO marker is scaled and positioned so may need moving
        }

        private void DeleteDataset(ref List<IData3DCollection> _datasets)
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
            if (centerSystem != null)
                labelSystemCoords.Text = string.Format("{0} x:{1} y:{2} z:{3}", centerSystem.Name, centerSystem.X.ToString("0.00"), centerSystem.Y.ToString("0.00"), centerSystem.Z.ToString("0.00"));
            else
                labelSystemCoords.Text = "No centre system";
        }

        public bool MoveTo(float x, float y, float z)
        {
            position.GoTo(new Vector3(x,y,z), -1F);
            return true;
        }

        public bool SetCenterSystemTo(string name)
        {
            if (Is3DMapsRunning)                         // if null, we are not up and running
                return SetCenterSystemTo(FindSystem(name,true));
            else
                return false;
        }

        public bool SetCenterSystemTo(ISystem sys)
        {
            if (sys != null)
            {
                centerSystem = sys;
                SetCenterSystemLabel();
                GenerateDataSetsSelectedSystems();
                position.GoTo(new Vector3((float)centerSystem.X, (float)centerSystem.Y, (float)centerSystem.Z), -1F);
                return true;
            }
            else
                return false;
        }


        #endregion



        #region User Controls

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

            if (name != null && !(float.IsNaN(loc.X) || float.IsNaN(loc.Y) || float.IsNaN(loc.Z)))
            {
                textboxFrom.Text = name;        // normalise name (user may have different)

                if (sys != null && moveto)
                    SetCenterSystemTo(sys);
                else if (moveto)
                    position.GoTo(loc, -1F);
                else
                    camera.LookAt(position.Current,loc,zoom.Current, 2F);
            }
            else
                ExtendedControls.MessageBoxTheme.Show(this, "System or Object " + textboxFrom.Text + " not found/no position");

            glControl.Focus();
        }

        private void toolStripButtonGoBackward_Click(object sender, EventArgs e)
        {
            if (systemlist != null)
            {
                HistoryEntry he = HistoryList.FindNextSystem(systemlist, centerSystem.Name, -1);
                SetCenterSystemTo((he == null) ? centerSystem.Name : he.System.Name);
            }
            else
                ExtendedControls.MessageBoxTheme.Show(this, "No travel history is available");
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            SetCenterSystemTo(homeSystem);
        }

        private void buttonHistory_Click(object sender, EventArgs e)
        {
            if (historySelection == null)
                ExtendedControls.MessageBoxTheme.Show(this, "No travel history is available");
            else
                SetCenterSystemTo(historySelection);
        }

        private void toolStripButtonTarget_Click(object sender, EventArgs e)
        {
            string name;
            double x, y, z;

            if (TargetClass.GetTargetPosition(out name, out x, out y, out z))
            {
                position.GoTo(new Vector3((float)x, (float)y, (float)z),-1F);
            }
            else
            {
                ExtendedControls.MessageBoxTheme.Show(this, "No target designated, create a bookmark or region mark, or use a Note mark, right click on it and set it as the target");
            }
        }
        
        private void toolStripButtonGoForward_Click(object sender, EventArgs e)
        {
            if (systemlist != null)
            {
                HistoryEntry he = HistoryList.FindNextSystem(systemlist, centerSystem.Name, 1);
                SetCenterSystemTo((he == null) ? centerSystem.Name : he.System.Name);
            }
            else
                ExtendedControls.MessageBoxTheme.Show(this, "No travel history is available");
        }

        private void toolStripButtonAutoForward_Click(object sender, EventArgs e)
        {
        }

        private void toolStripLastKnownPosition_Click(object sender, EventArgs e)
        {
            if (systemlist != null)
            {
                HistoryEntry he = HistoryList.FindLastKnownPosition(systemlist);

                if (he != null )
                    SetCenterSystemTo(FindSystem(he.System.Name,true));
                else
                    ExtendedControls.MessageBoxTheme.Show(this, "No stars with defined co-ordinates available in travel history");
            }
            else
                ExtendedControls.MessageBoxTheme.Show(this, "No travel history is available");
        }

        private void drawLinesBetweenStarsWithPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateDataSetsSystemList();
            RequestPaint();
        }

        private void drawADiscOnStarsWithPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateDataSetsSystemList();
            RequestPaint();
        }

        private void useWhiteForDiscsInsteadOfAssignedMapColourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateDataSetsSystemList();
            RequestPaint();
        }

        private void showStarstoolStripMenuItem_Click(object sender, EventArgs e)
        {
            RequestPaint();
        }

        private void showGalacticMapTypeMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tmsi = (ToolStripMenuItem)sender;

            if ( tmsi.Tag is int )
            {
                int v = (int)tmsi.Tag;
                if ( v == 0 )
                {
                    discoveryForm.galacticMapping.ToggleEnable();
                    
                    foreach (ToolStripMenuItem ti in toolStripDropDownButtonGalObjects.DropDownItems)
                    {
                        if (ti.Tag is GalMapType )
                            ti.Checked = ((GalMapType)ti.Tag).Enabled;
                    }
                }
            }
            else
            {
                discoveryForm.galacticMapping.ToggleEnable((GalMapType)tmsi.Tag);
            }

            GenerateDataSetsBNG();
            RequestPaint();
        }

        private void enableColoursToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stargrids.ForceWhite = !enableColoursToolStripMenuItem.Checked;
            GenerateDataSetsBNG();          // because gmo's are coloured accordingly

            RequestPaint();
        }

        private void showStationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RequestPaint();
        }

        private void toolStripButtonGrid_Click(object sender, EventArgs e)
        {
            RequestPaint();
        }

        private void toolStripButtonFineGrid_Click(object sender, EventArgs e)
        {
            RequestPaint();
        }

        private void toolStripButtonCoords_Click(object sender, EventArgs e)
        {
            RequestPaint();
        }

        private void toolStripButtonEliteMovement_Click(object sender, EventArgs e)
        {
        }

        private void showDiscsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RequestStarNameRecalc();
        }

        private void showNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RequestStarNameRecalc();
        }

        private void showNoteMarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateDataSetsBNG();
            RequestPaint();
        }

        private void showBookmarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateDataSetsBNG();
            RequestPaint();
        }

        private void newRegionBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookmarkForm frm = new BookmarkForm();
            frm.InitialisePos(position.Current.X, position.Current.Y, position.Current.Z);
            DateTime bookmarktime = DateTime.UtcNow;
            frm.NewRegionBookmark(bookmarktime);
            DialogResult res = frm.ShowDialog(this);

            if (res == DialogResult.OK)
            {
                BookmarkClass newcls = GlobalBookMarkList.Instance.AddOrUpdateBookmark(null,false,frm.StarHeading, double.Parse(frm.x), double.Parse(frm.y), double.Parse(frm.z), bookmarktime, frm.Notes);

                if (frm.IsTarget)          // asked for targetchanged..
                {
                    TargetClass.SetTargetBookmark("RM:" + newcls.Heading, newcls.id, newcls.x, newcls.y, newcls.z);
                    discoveryForm.NewTargetSet(this);
                }

                GenerateDataSetsBNG();
                RequestPaint();
            }
        }
        
        private void toolStripButtonPerspective_Click(object sender, EventArgs e)
        {
            camera.Set(new Vector3(toolStripButtonPerspective.Checked ? 45 : 0, 0, 0));
            
            SetModelProjectionMatrix();

            RequestPaint();
        }

        private void dotSystemCoords_Click(object sender, EventArgs e)
        {
            SetCenterSystemTo(centerSystem);
        }

        private void dotSelectedSystemCoords_Click(object sender, EventArgs e)
        {
            if (clickedSystem!=null)
                SetCenterSystemTo(clickedSystem);
            else 
                position.GoTo(clickedposition,-1F);      // if nan, will ignore..
        }

        private void toolStripButtonHelp_Click(object sender, EventArgs e)
        {
            if (helpDialog == null)
            {
                helpDialog = new ExtendedControls.InfoForm() { TopMost = this.TopMost };
                helpDialog.Info("3D Map Help", FindForm().Icon, Properties.Resources.maphelp3d, new int[] { 50, 200, 400 });
                helpDialog.Show();
                helpDialog.Disposed += (s, ea) => helpDialog = null;
            }
            else
            {
                helpDialog.Activate();
                helpDialog.BringToFront();
            }
        }

        private void dropdownMapNames_DropDownItemClicked(object sender, EventArgs e)
        {
            ToolStripButton tsb = (ToolStripButton)sender;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("map3DMaps" + tsb.Text, tsb.Checked);
            GenerateDataSetsMaps();
            RequestPaint();
        }

        private void viewOnEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clickedurl != null && clickedurl.Length>0)
                System.Diagnostics.Process.Start(clickedurl);
        }

        private void labelClickedSystemCoords_Click(object sender, EventArgs e)
        {
            if (clickedSystem != null || !float.IsNaN(clickedposition.X))
            {
                systemselectionMenuStrip.Show(labelClickedSystemCoords, 0, labelClickedSystemCoords.Height);
            }
        }

        void SetDropDownRecordImage()
        {
            if (maprecorder.InPlayBack)
                toolStripDropDownRecord.Image = (maprecorder.Paused) ? Icons.Controls.Map3D_Recorder_PausePlay : Icons.Controls.Map3D_Recorder_Play;
            else if (maprecorder.Recording)
                toolStripDropDownRecord.Image = (maprecorder.Paused) ? Icons.Controls.Map3D_Recorder_PauseRecord : Icons.Controls.Map3D_Recorder_Record;
            else
                toolStripDropDownRecord.Image = Icons.Controls.Map3D_Recorder_Menu;
        }

        private void recordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            maprecorder.ToggleRecord(false);
            SetDropDownRecordImage();
        }

        private void recordStepToStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            maprecorder.ToggleRecord(true);
            SetDropDownRecordImage();
        }

        private void newRecordStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            maprecorder.RecordStepDialog(position.Current, camera.Current, zoom.Current);
        }

        private void toolStripMenuItemClearRecording_Click(object sender, EventArgs e)
        {
            if (ExtendedControls.MessageBoxTheme.Show(this, "Confirm you wish to clear the current recording", "WARNING", MessageBoxButtons.OKCancel) == DialogResult.OK)
                maprecorder.Clear();

            SetDropDownRecordImage();
        }

        private void playbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            position.KillSlew();
            camera.KillSlew();
            zoom.KillSlew();
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
            recordToolStripMenuItem.Text = maprecorder.Recording ? "Stop Recording (F5)" : maprecorder.Entries ? "Resume Recording (F5)" : "Start Recording (F5)";
            recordToolStripMenuItem.Image = maprecorder.Recording ? Icons.Controls.Map3D_Recorder_StopRecord : Icons.Controls.Map3D_Recorder_Record;
            recordToolStripMenuItem.Enabled = !maprecorder.InPlayBack && !maprecorder.RecordingStep;

            recordStepToStepToolStripMenuItem.Text = maprecorder.Recording ? "Stop Step Recording (F6)" : maprecorder.Entries ? "Resume Step Recording (F6)" : "Start Step Recording (F6)";
            recordStepToStepToolStripMenuItem.Image = maprecorder.Recording ? Icons.Controls.Map3D_Recorder_StopRecord : Icons.Controls.Map3D_Recorder_RecordStep;
            recordStepToStepToolStripMenuItem.Enabled = !maprecorder.InPlayBack && !maprecorder.RecordingNormal;

            newRecordStepToolStripMenuItem.Enabled = maprecorder.Recording;

            toolStripMenuItemClearRecording.Enabled = maprecorder.Entries;

            playbackToolStripMenuItem.Text = maprecorder.InPlayBack ? "Stop Playback (F9)" : "Start Playback (F9)";
            playbackToolStripMenuItem.Image = maprecorder.InPlayBack ? Icons.Controls.Map3D_Recorder_StopPlay : Icons.Controls.Map3D_Recorder_Play;
            playbackToolStripMenuItem.Enabled = maprecorder.Entries;

            if (maprecorder.InPlayBack)
            {
                pauseRecordToolStripMenuItem.Text = maprecorder.Paused ? "Resume Playback (F8)" : "Pause Playback (F8)";
                pauseRecordToolStripMenuItem.Image = Icons.Controls.Map3D_Recorder_PausePlay;
            }
            else if (maprecorder.Recording)
            {
                pauseRecordToolStripMenuItem.Text = maprecorder.Paused ? "Resume Recording (F8)" : "Pause Recording (F8)";
                pauseRecordToolStripMenuItem.Image = Icons.Controls.Map3D_Recorder_PauseRecord;
            }
            else
            {
                pauseRecordToolStripMenuItem.Text = "Pause (F8)";
                pauseRecordToolStripMenuItem.Image = Icons.Controls.Map3D_Recorder_Pause;
            }

            pauseRecordToolStripMenuItem.Enabled = maprecorder.Recording || maprecorder.InPlayBack;

            saveToFileToolStripMenuItem.Enabled = maprecorder.Entries;
        }

        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            maprecorder.SaveDialog();
            maprecorder.UpdateStoredVideosToolButton(toolStripDropDownRecord, LoadVideo, Icons.Controls.Map3D_Recorder_Save);
        }

        private void LoadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            maprecorder.LoadDialog();
        }

        private void LoadVideo(object sender, EventArgs e)
        {
            ToolStripMenuItem tmsi = sender as ToolStripMenuItem;
            string file = (string)tmsi.Tag;
            if ( !maprecorder.ReadFromFile(file) )
            {
                ExtendedControls.MessageBoxTheme.Show(this, "Failed to load flight " + file + ". Check file path and file contents");
            }
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panelTop_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void panelTop_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            OnCaptionMouseUp((Control)sender, e);
        }

        #endregion

        #region Mouse

        private void glControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            position.KillSlew();
            camera.KillSlew();

            mouseDownPos.X = e.X;
            mouseDownPos.Y = e.Y;
            //Console.WriteLine("Mouseup down at " + e.X + "," + e.Y);

            if (e.Button.HasFlag(System.Windows.Forms.MouseButtons.Left))
            {
                mouseStartRotate.X = e.X;
                mouseStartRotate.Y = e.Y;
                //Console.WriteLine("Mouse start left");
            }

            if (e.Button.HasFlag(System.Windows.Forms.MouseButtons.Right))
            {
                mouseStartTranslateXY.X = e.X;
                mouseStartTranslateXY.Y = e.Y;
                mouseStartTranslateXZ.X = e.X;
                mouseStartTranslateXZ.Y = e.Y;
            }

            //System.Diagnostics.Debug.WriteLine("{0} Mouse down", Environment.TickCount % 10000);
        }

        private void glControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            bool notmovedmouse = Math.Abs(e.X - mouseDownPos.X) + Math.Abs(e.Y - mouseDownPos.Y) < 4;

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

            GetMouseOverItem(e.X, e.Y, out cursystem, out curbookmark, out notedsystem, out gmo, true);
            //System.Diagnostics.Debug.WriteLine("{0} Mouse over {1}", Environment.TickCount % 10000, cursystem);

            if (e.Button == System.Windows.Forms.MouseButtons.Left)            // left clicks associate with systems..
            {
                mouseStartRotate = new Point(int.MinValue, int.MinValue);      // indicate finished .. we need to do this to make mousemove not respond to random stuff when resizing

                bool isanyselected = clickedSystem != null || clickedGMO != null;                // have we got a currently selected object

                clickedGMO = gmo;
                clickedSystem = cursystem;         // clicked system was found either by a bookmark looking up a name, or a position look up. best we can do.
                clickedposition.X = float.NaN;
                clickedurl = null;

                string name = null;

                if (clickedSystem != null)         // will be set for systems clicked, bookmarks or noted systems
                {
                    clickedposition = new Vector3((float)clickedSystem.X, (float)clickedSystem.Y, (float)clickedSystem.Z);

                    name = clickedSystem.Name;

                    var edsm = new EDSMClass();
                    clickedurl = edsm.GetUrlToEDSMSystem(name, clickedSystem.EDSMID);
                    viewOnEDSMToolStripMenuItem.Enabled = true;

                    try
                    {
                        System.Windows.Forms.Clipboard.SetText(clickedSystem.Name);
                    }
                    catch
                    {
                        Trace.WriteLine("Copying text to clipboard failed");
                    }

                }
                else if (curbookmark != null)                                   // region bookmark..
                {
                    clickedposition = new Vector3((float)curbookmark.x, (float)curbookmark.y, (float)curbookmark.z);

                    name = (curbookmark.StarName != null) ? curbookmark.StarName : curbookmark.Heading;      // I know its only going to be a region bookmark, but heh

                    viewOnEDSMToolStripMenuItem.Enabled = false;
                }
                else if (gmo != null)
                {
                    clickedposition = new Vector3((float)gmo.points[0].X, (float)gmo.points[0].Y, (float)gmo.points[0].Z);

                    name = gmo.name;

                    clickedurl = gmo.galMapUrl;
                    viewOnEDSMToolStripMenuItem.Enabled = true;
                }

                if (name != null)       // this means we found something..
                    labelClickedSystemCoords.Text = string.Format("{0} x:{1} y:{2} z:{3}", name, clickedposition.X.ToString("0.00"), clickedposition.Y.ToString("0.00"), clickedposition.Z.ToString("0.00"));
                else
                    labelClickedSystemCoords.Text = "None";

                if ( name != null || isanyselected)     // if we selected something, or something was selected
                {
                    GenerateDataSetsSelectedSystems();
                    RequestPaint();
                }

                DisplayTip(clickedSystem, curbookmark, gmo);
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)                    // right clicks are about bookmarks.
            {
                mouseStartTranslateXY = new Point(int.MinValue, int.MinValue);         // indicate rotation is finished.
                mouseStartTranslateXZ = new Point(int.MinValue, int.MinValue);

                if (cursystem != null || curbookmark != null)      // if we have a system or a bookmark...
                {
                    //Moved the code so that it could be shared with SavedRouteExpeditionControl
                    UserControls.TargetHelpers.ShowBookmarkForm(this,discoveryForm , cursystem, curbookmark, notedsystem);
                    GenerateDataSetsBNG();      // in case target changed, do all..
                    RequestPaint();
                }
                else if (gmo != null)
                {
                    long targetid = TargetClass.GetTargetGMO();      // who is the target of a bookmark (0=none)

                    BookmarkForm frm = new BookmarkForm();

                    frm.Name = gmo.name;
                    frm.InitialisePos(gmo.points[0].X, gmo.points[0].Y, gmo.points[0].Z);
                    frm.GMO(gmo.name, gmo.description, targetid == gmo.id, gmo.galMapUrl);
                    DialogResult res = frm.ShowDialog(this);

                    if (res == DialogResult.OK)
                    {
                        if ((frm.IsTarget && targetid != gmo.id) || (!frm.IsTarget && targetid == gmo.id)) // changed..
                        {
                            if (frm.IsTarget)
                                TargetClass.SetTargetGMO("G:" + gmo.name, gmo.id, gmo.points[0].X, gmo.points[0].Y, gmo.points[0].Z);
                            else
                                TargetClass.ClearTarget();

                            GenerateDataSetsBNG();
                            RequestPaint();
                            discoveryForm.NewTargetSet(this);
                        }
                    }
                }
            }
        }

        private void glControl_DoubleClick(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("{0} Double click", Environment.TickCount%10000);
            position.GoTo(clickedposition,-1F);
        }

        private void glControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {                                                                   
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (mouseStartRotate.X != int.MinValue) // on resize double click resize, we get a stray mousemove with left, so we need to make sure we actually had a down event
                {
                    position.KillSlew(); camera.KillSlew();
                    //Console.WriteLine("Mouse move left");
                    int dx = e.X - mouseStartRotate.X;
                    int dy = e.Y - mouseStartRotate.Y;

                    mouseStartRotate.X = mouseStartTranslateXZ.X = e.X;
                    mouseStartRotate.Y = mouseStartTranslateXZ.Y = e.Y;
                    //System.Diagnostics.Trace.WriteLine("dx" + dx.ToString() + " dy " + dy.ToString() + " Button " + e.Button.ToString());

                    camera.Rotate(new Vector3((float)(-dy / 4.0f), (float)(dx / 4.0f), 0));
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (mouseStartTranslateXY.X != int.MinValue)
                {
                    position.KillSlew(); camera.KillSlew();

                    int dx = e.X - mouseStartTranslateXY.X;
                    int dy = e.Y - mouseStartTranslateXY.Y;

                    mouseStartTranslateXY.X = mouseStartTranslateXZ.X = e.X;
                    mouseStartTranslateXY.Y = mouseStartTranslateXZ.Y = e.Y;
                    //System.Diagnostics.Trace.WriteLine("dx" + dx.ToString() + " dy " + dy.ToString() + " Button " + e.Button.ToString());

                    position.Translate(new Vector3(0, -dy * (1.0f / zoom.Current) * 2.0f, 0));
                }
            }
            else if (e.Button == (System.Windows.Forms.MouseButtons.Left | System.Windows.Forms.MouseButtons.Right))
            {
                if (mouseStartTranslateXZ.X != int.MinValue)
                {
                    position.KillSlew(); camera.KillSlew();

                    int dx = e.X - mouseStartTranslateXZ.X;
                    int dy = e.Y - mouseStartTranslateXZ.Y;

                    mouseStartTranslateXZ.X = mouseStartRotate.X = mouseStartTranslateXY.X = e.X;
                    mouseStartTranslateXZ.Y = mouseStartRotate.Y = mouseStartTranslateXY.Y = e.Y;
                    //System.Diagnostics.Trace.WriteLine("dx" + dx.ToString() + " dy " + dy.ToString() + " Button " + e.Button.ToString());

                    Matrix3 transform = Matrix3.CreateRotationZ((float)(-camera.Current.Y * Math.PI / 180.0f));
                    Vector3 translation = new Vector3(-dx * (1.0f / zoom.Current) * 2.0f, dy * (1.0f / zoom.Current) * 2.0f, 0.0f);
                    translation = Vector3.Transform(translation, transform);

                    position.Translate(new Vector3(translation.X,0, translation.Y));
                }
            }
            else 
            {
                if (tooltipon)
                {
                    if (Math.Abs(e.X - tooltipPosition.X) + Math.Abs(e.Y - tooltipPosition.Y) > 8)
                    {
                        KillToolTip();
                    }
                }
                else
                    tooltipPosition = e.Location;
            }
        }

        void KillToolTip()
        {
            controltooltip.SetToolTip(glControl, "");
            controltooltip.Hide(glControl);
            tooltipon = false;
        }

        void DisplayTip(ISystem hoversystem, BookmarkClass curbookmark, GalacticMapObject gmo)
        { 
            string info = null, sysname = null;
            Vector3d pos = new Vector3d(0,0,0);

            if (hoversystem != null)
            {
                sysname = hoversystem.Name;

                info = hoversystem.Name + Environment.NewLine + string.Format("x:{0} y:{1} z:{2}", hoversystem.X.ToString("0.00"), hoversystem.Y.ToString("0.00"), hoversystem.Z.ToString("0.00"));

                pos = new Vector3d(hoversystem.X, hoversystem.Y, hoversystem.Z);

                if (hoversystem.Allegiance != EDAllegiance.Unknown)
                    info += Environment.NewLine + "Allegiance: " + hoversystem.Allegiance;

                if (hoversystem.PrimaryEconomy != EDEconomy.Unknown)
                    info += Environment.NewLine + "Economy: " + hoversystem.PrimaryEconomy;

                if (hoversystem.Government != EDGovernment.Unknown)
                    info += Environment.NewLine + "Government: " + hoversystem.Allegiance;

                if (hoversystem.State != EDState.Unknown)
                    info += Environment.NewLine + "State: " + hoversystem.State;

                if (hoversystem.Allegiance != EDAllegiance.Unknown)
                    info += Environment.NewLine + "Allegiance: " + hoversystem.Allegiance;

                if (hoversystem.Population != 0)
                    info += Environment.NewLine + "Population: " + hoversystem.Population;
            }
            else if (curbookmark != null && curbookmark.Heading != null)     // region bookmark (second check should be redundant but its protection).
            {
                info = curbookmark.Heading + Environment.NewLine + string.Format("x:{0} y:{1} z:{2}", curbookmark.x.ToString("0.00"), curbookmark.y.ToString("0.00"), curbookmark.z.ToString("0.00"));
                sysname = "<<Never match string! to make the comparison fail";
                pos = new Vector3d(curbookmark.x, curbookmark.y, curbookmark.z);
            }
            else if ( gmo != null )
            {
                pos = new Vector3d(gmo.points[0].X, gmo.points[0].Y, gmo.points[0].Z);
                info = gmo.name + Environment.NewLine + gmo.galMapType.Description + Environment.NewLine + gmo.description.WordWrap(60) + Environment.NewLine +
                    string.Format("x:{0} y:{1} z:{2}", pos.X.ToString("0.00"), pos.Y.ToString("0.00"), pos.Z.ToString("0.00"));
                sysname = "<<Never match string! to make the comparison fail";
            }
            
            if ( sysname != null )
            { 
                if (!sysname.Equals(centerSystem.Name))
                {
                    Vector3d cs = new Vector3d(centerSystem.X, centerSystem.Y, centerSystem.Z);
                    info += Environment.NewLine + "Distance from " + centerSystem.Name + ": " + (cs-pos).Length.ToString("0.0");
                }
                // if ex, history not hover, history not centre
                if (historySelection != null && !sysname.Equals(historySelection.Name) && !historySelection.Name.Equals(centerSystem.Name))
                {
                    Vector3d hs = new Vector3d(historySelection.X, historySelection.Y, historySelection.Z);
                    info += Environment.NewLine + "Distance from " + historySelection.Name + ": " + (hs-pos).Length.ToString("0.0");
                }
                // home not centre, home not history or history null
                if (!homeSystem.Name.Equals(centerSystem.Name) && (historySelection == null || !historySelection.Name.Equals(homeSystem.Name)))
                {
                    double dist = ((new Vector3d(homeSystem.X, homeSystem.Y, homeSystem.Z)) - pos).Length;
                    info += Environment.NewLine + "Distance from " + homeSystem.Name + ": " + dist.ToString("0.0");
                }

                if (clickedSystem != null && clickedSystem != centerSystem && clickedSystem != historySelection && clickedSystem != homeSystem && clickedSystem!=hoversystem)
                {
                    double dist = ((new Vector3d(clickedSystem.X, clickedSystem.Y, clickedSystem.Z)) - pos).Length;
                    info += Environment.NewLine + "Distance from " + clickedSystem.Name + ": " + dist.ToString("0.0");
                }

                if (clickedGMO != null && clickedGMO != gmo )
                {
                    double dist = ((new Vector3d(clickedGMO.points[0].X, clickedGMO.points[0].Y, clickedGMO.points[0].Z)) - pos).Length;
                    info += Environment.NewLine + "Distance from " + clickedGMO.name + ": " + dist.ToString("0.0");
                }

                SystemNoteClass sn = SystemNoteClass.GetNoteOnSystem(sysname, hoversystem == null ? 0 : hoversystem.EDSMID);   // may be null
                if (!string.IsNullOrWhiteSpace(sn?.Note))
                {
                    info += Environment.NewLine + "Notes: " + sn.Note.Trim();
                }

                if (!string.IsNullOrWhiteSpace(curbookmark?.Note))
                    info += Environment.NewLine + "Bookmark Notes: " + curbookmark.Note.Trim();

                controltooltip.SetToolTip(glControl, info);
                tooltipon = true;
            }
        }

        private void glControl_OnMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                if (kbdActions.Ctrl)
                {
                    if (fov.Scale(e.Delta < 0))
                    {
                        SetModelProjectionMatrix();
                        RequestPaint();
                    }
                }
                else
                {
                    zoom.Scale(e.Delta > 0);
                }
            }
        }


#endregion

        #region FindObjectsOnScreen

        private bool GetPixel(Vector4 xyzw, ref Matrix4 resmat, ref Vector2 pixelpos, out float newcursysdistz)
        {
            Vector4 sysloc = Vector4.Transform(xyzw, resmat);

            float w2 = glControl.Width / 2.0F;
            float h2 = glControl.Height / 2.0F;

            if (sysloc.Z > _znear)
            {
                pixelpos = new Vector2(((sysloc.X / sysloc.W) + 1.0F) * w2, ((sysloc.Y / sysloc.W) + 1.0F) * h2);
                newcursysdistz = (float)Math.Abs(sysloc.Z * zoom.Current);

                return true;
            }

            newcursysdistz = 0;
            return false;
        }

        private bool IsWithinRectangle( Matrix4 area ,  int x, int y, out float newcursysdistz, ref Matrix4 resmat )
        {
            Vector2 ptopleft = new Vector2(0, 0), pbottomright = new Vector2(0, 0);
            Vector2 pbottomleft = new Vector2(0, 0), ptopright = new Vector2(0, 0);
            float ztopleft, zbottomright,zbottomleft,ztopright;

            if (GetPixel(area.Row0, ref resmat, ref ptopleft, out ztopleft) &&
                GetPixel(area.Row1, ref resmat, ref ptopright, out ztopright) &&
                GetPixel(area.Row2, ref resmat, ref pbottomright, out zbottomright) && 
                GetPixel(area.Row3, ref resmat, ref pbottomleft, out zbottomleft ) )
            {
                //    Console.WriteLine("Row0 {0},{1}", ptopleft.X, ptopleft.Y); Console.WriteLine("Row1 {0},{1}", ptopright.X, ptopright.Y); Console.WriteLine("Row2 {0},{1}", pbottomright.X, pbottomright.Y); Console.WriteLine("Row3 {0},{1}", pbottomleft.X, pbottomleft.Y);  Console.WriteLine("x,y {0},{1} {2},{3}", x, y , x-pbottomleft.X, y-pbottomright.Y);

                GraphicsPath p = new GraphicsPath();            // a moment of inspiration, use the graphics path for the polygon hit test!
                p.AddLine(new PointF(ptopleft.X, ptopleft.Y), new PointF(ptopright.X, ptopright.Y));
                p.AddLine(new PointF(pbottomright.X, pbottomright.Y), new PointF(pbottomleft.X, pbottomleft.Y));
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

        private BookmarkClass GetMouseOverBookmark(int x, int y, out float cursysdistz)
        {
            x = Math.Min(Math.Max(x, 5), glControl.Width - 5);
            y = Math.Min(Math.Max(glControl.Height - y, 5), glControl.Height - 5);

            Vector3[] rotvert = TexturedQuadData.GetVertices(new Vector3(0, 0, 0), lastcameranorm.Rotation, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), 0, GetBitmapOnScreenSizeY() / 2);

            BookmarkClass curbk = null;
            cursysdistz = float.MaxValue;
            Matrix4 resmat = matrixcalc.GetResMat;

            foreach (BookmarkClass bc in GlobalBookMarkList.Instance.Bookmarks)
            {
                //Console.WriteLine("Checking bookmark " + ((bc.Heading != null) ? bc.Heading : bc.StarName));

                Vector3 bp = new Vector3((float)bc.x, (float)bc.y, (float)bc.z);

                Matrix4 area = new Matrix4(
                    new Vector4(rotvert[0].X + bp.X, rotvert[0].Y + bp.Y, rotvert[0].Z + bp.Z, 1),    // top left
                    new Vector4(rotvert[1].X + bp.X, rotvert[1].Y + bp.Y, rotvert[1].Z + bp.Z, 1),    
                    new Vector4(rotvert[2].X + bp.X, rotvert[2].Y + bp.Y, rotvert[2].Z + bp.Z, 1),    
                    new Vector4(rotvert[3].X + bp.X, rotvert[3].Y + bp.Y, rotvert[3].Z + bp.Z, 1)    // bot left
                    );

                //Console.WriteLine("{0},{1},{2}, {3},{4},{5} vs {6},{7}" , area.Row0.X , area.Row0.Y, area.Row0.Z, area.Row2.X, area.Row2.Y , area.Row2.Z , x,y);

                float newcursysdistz;
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

        private ISystem GetMouseOverNotedSystem(int x, int y, out float cursysdistz )
        {
            x = Math.Min(Math.Max(x, 5), glControl.Width - 5);
            y = Math.Min(Math.Max(glControl.Height - y, 5), glControl.Height - 5);

            Vector3[] rotvert = TexturedQuadData.GetVertices(new Vector3(0, 0, 0), lastcameranorm.Rotation, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY(), 0, GetBitmapOnScreenSizeY() / 2);

            ISystem cursys = null;
            cursysdistz = float.MaxValue;

            if (systemlist == null)
                return null;

            Matrix4 resmat = matrixcalc.GetResMat;

            foreach (HistoryEntry vs in systemlist)
            {
                if ( vs.System.HasCoordinate)
                { 
                    SystemNoteClass notecs = SystemNoteClass.GetNoteOnSystem(vs.System.Name, vs.System.EDSMID);

                    if (notecs!=null )
                    {
                        string note = notecs.Note.Trim();

                        if (note.Length > 0)
                        {
                            float lx = (float)(vs.System.X);
                            float ly = (float)(vs.System.Y);
                            float lz = (float)(vs.System.Z);

                            Matrix4 area = new Matrix4(
                                new Vector4(rotvert[0].X + lx, rotvert[0].Y + ly, rotvert[0].Z + lz, 1),    // top left
                                new Vector4(rotvert[1].X + lx, rotvert[1].Y + ly, rotvert[1].Z + lz, 1),
                                new Vector4(rotvert[2].X + lx, rotvert[2].Y + ly, rotvert[2].Z + lz, 1),
                                new Vector4(rotvert[3].X + lx, rotvert[3].Y + ly, rotvert[3].Z + lz, 1)    // bot left
                                );

                            float newcursysdistz;
                            if (IsWithinRectangle(area, x, y, out newcursysdistz, ref resmat))
                            {
                                if (newcursysdistz < cursysdistz)
                                {
                                    cursysdistz = newcursysdistz;
                                    cursys = vs.System;
                                }
                            }
                        }
                    }
                }

            }

            return cursys;
        }

        private GalacticMapObject GetMouseOverGalaxyObject(int x, int y, out float cursysdistz)
        {
            x = Math.Min(Math.Max(x, 5), glControl.Width - 5);
            y = Math.Min(Math.Max(glControl.Height - y, 5), glControl.Height - 5);

            Vector3[] rotvert = TexturedQuadData.GetVertices(new Vector3(0, 0, 0), lastcameranorm.Rotation, GetBitmapOnScreenSizeX(), GetBitmapOnScreenSizeY());

            cursysdistz = float.MaxValue;
            Matrix4 resmat = matrixcalc.GetResMat;
            GalacticMapObject curobj = null;

            if (discoveryForm.galacticMapping != null && discoveryForm.galacticMapping.Loaded)
            {
                foreach (GalacticMapObject gmo in discoveryForm.galacticMapping.galacticMapObjects)
                {
                    if (gmo.galMapType.Enabled && gmo.galMapType.Group == GalMapType.GalMapGroup.Markers && gmo.points.Count > 0)             // if it is Enabled and has a co-ord, and is a marker type (routes/regions rejected)
                    {
                        Vector3 pd = gmo.points[0].Convert();

                        Matrix4 area = new Matrix4(
                            new Vector4(rotvert[0].X + pd.X, rotvert[0].Y + pd.Y, rotvert[0].Z + pd.Z, 1),    // top left
                            new Vector4(rotvert[1].X + pd.X, rotvert[1].Y + pd.Y, rotvert[1].Z + pd.Z, 1),
                            new Vector4(rotvert[2].X + pd.X, rotvert[2].Y + pd.Y, rotvert[2].Z + pd.Z, 1),
                            new Vector4(rotvert[3].X + pd.X, rotvert[3].Y + pd.Y, rotvert[3].Z + pd.Z, 1)    // bot left
                            );

                        float newcursysdistz;
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

        private ISystem GetMouseOverSystem(int x, int y, bool checksysdb , out float cursysdistz)
        {
            x = Math.Min(Math.Max(x, 5), glControl.Width - 5);
            y = Math.Min(Math.Max(glControl.Height - y, 5), glControl.Height - 5);

            StarGrid.TransFormInfo ti = new StarGrid.TransFormInfo(matrixcalc.GetResMat, _znear, glControl.Width, glControl.Height, zoom.Current);

            Vector3? posofsystem = stargrids.FindOverSystem(x, y, out cursysdistz, ti, showStarstoolStripMenuItem.Checked, showStationsToolStripMenuItem.Checked);

            if ( posofsystem == null )
                posofsystem = starnameslist.FindOverSystem(x, y, out cursysdistz, ti); // in case these are showing

            ISystem f = null;

            if (posofsystem != null )
                f = FindSystem(new Vector3(posofsystem.Value.X, posofsystem.Value.Y, posofsystem.Value.Z),checksysdb);

            return f;
        }

        // system = cursystem!=null, curbookmark = null, notedsystem = false
        // bookmark on system, cursystem!=null, curbookmark != null, notedsystem = false
        // region bookmark. cursystem = null, curbookmark != null, notedsystem = false
        // clicked on note on a system, cursystem!=null,curbookmark=null, notedsystem=true
        // clicked on gal object, galmapobject !=null, cursystem=null,curbookmark=null,notedsystem = false

        private void GetMouseOverItem(int x, int y, out ISystem cursystem,  // can return both, if a system bookmark is clicked..
                                                    out BookmarkClass curbookmark, out bool notedsystem,
                                                    out GalacticMapObject galobj , bool checksysdb)
        {
            cursystem = null;
            curbookmark = null;
            notedsystem = false;
            galobj = null;

            if (datasets_bookedmarkedsystems != null)              // only if bookedmarked is shown
            {
                float curdistbookmark;
                curbookmark = GetMouseOverBookmark(x, y, out curdistbookmark);       

                if (curbookmark != null)
                {
                    if ( curbookmark.StarName != null )            // if starname set, see if we can find it
                        cursystem = FindSystem(curbookmark.StarName,true);       // find, either in visited system, or in db

                    return;
                }
            }

            if (datasets_notedsystems != null)
            {
                float curdistnoted;
                cursystem = GetMouseOverNotedSystem(x, y, out curdistnoted);

                if ( cursystem != null )
                { 
                    notedsystem = true;
                    return;
                }
            }

            float curdistgalmap;
            galobj = GetMouseOverGalaxyObject(x, y, out curdistgalmap);
            if (galobj != null)
                return;

            float curdistsystem;
            cursystem = GetMouseOverSystem(x, y, checksysdb , out curdistsystem);
        }

        #endregion

        #region Misc

        private ISystem FindSystem(string name, bool checksystemdb)    // find system either in systemlist or db if allowed.  only allowed in formmap due to cursor
        {
            if (systemlist != null)
            {
                HistoryEntry sys = systemlist.FindLast(x => x.System.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

                if (sys != null)
                    return sys.System;
            }

            if (checksystemdb)
            {
                Cursor = Cursors.WaitCursor;
                ISystem sys = SystemCache.FindSystem(name);
                Cursor = Cursors.Default;
                return sys;
            }
            else
                return null;
        }

        private ISystem FindSystem(Vector3 pos, bool checksystemdb) // find system either in systemlist or db if allowed.  only allowed in formmap due to cursor
        {
            if (systemlist != null)
            {
                HistoryEntry vsc = HistoryList.FindByPos(systemlist, pos.X, pos.Y, pos.Z, 0.1);

                if (vsc != null)
                    return vsc.System;
            }

            if (checksystemdb)
            {
                Cursor = Cursors.WaitCursor;
                ISystem sys = SystemCache.GetSystemByPosition(pos.X, pos.Y, pos.Z, 5000);
                Cursor = Cursors.Default;
                return sys;
            }
            else
                return null;
        }

        public ISystem FindSystemInSystemlist(Vector3 pos)  // allowable to be called from external
        {
            if (systemlist != null)
            {
                HistoryEntry vsc = HistoryList.FindByPos(systemlist, pos.X, pos.Y, pos.Z, 0.1);

                if (vsc != null)
                    return vsc.System;
            }

            return null;
        }

        private ISystem SafeSystem(ISystem s)
        {
            if (s == null)
            {
                s = FindSystem("Sol",true);

                if (s == null)
                    s = new SystemClass("Sol", 0, 0, 0);
            }

            return s;
        }

        public string FindSystemOrGMO(string name, out ISystem sys, out GalacticMapObject gmo , out Vector3 loc )
        {
            gmo = null;
            sys = FindSystem(textboxFrom.Text,true);
            loc = new Vector3(0, 0, 0);

            if (sys != null)
            {
                loc = new Vector3((float)sys.X, (float)sys.Y, (float)sys.Z);
                return sys.Name;
            }

            gmo = discoveryForm.galacticMapping.Find(textboxFrom.Text, true, true);    // ignore if its off, find any part of string, find if disabled

            if (gmo != null)
            {
                if (gmo.points.Count > 0)
                {
                    loc = gmo.points[0].Convert();
                    return gmo.name;
                }
                else
                    gmo = null;
            }

            return null;
        }

        #endregion


        #region Map Images and Expeditions

        private void LoadMapImages()
        {
            fgeimages = BaseUtils.Map2d.LoadImages(EDDOptions.Instance.MapsAppDirectory());
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
                item.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("map3DMaps" + img.FileName, false);
                dropdownMapNames.DropDownItems.Add(item);
            }
        }

        public void AddExpedition(string name, Func<DateTime> starttime, Func<DateTime> endtime)
        {
            AddExpeditionInt(name, starttime, endtime);
        }

        public void AddExpedition(string name, DateTime starttime, DateTime? endtime)
        {
            AddExpeditionInt(name, () => starttime, endtime == null ? null : new Func<DateTime>(() => (DateTime)endtime));
        }

        private ToolStripButton AddExpeditionInt(string name, Func<DateTime> starttime, Func<DateTime> endtime)
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
                { "All", () => new DateTime(2014, 12, 16, 0, 0, 0) },
                { "Last Week", () => DateTime.Now.AddDays(-7) },
                { "Last Month", () => DateTime.Now.AddMonths(-1) },
                { "Last Year", () => DateTime.Now.AddYears(-1) }
            };

            Dictionary<string, Func<DateTime>> endtimes = new Dictionary<string, Func<DateTime>>();

            foreach (var expedition in SavedRouteClass.GetAllSavedRoutes())
            {
                if (expedition.StartDateUTC != null)
                {
                    var starttime = (DateTime)expedition.StartDateUTC;
                    starttimes[expedition.Name] = () => starttime;

                    if (expedition.EndDateUTC != null)
                    {
                        var endtime = (DateTime)expedition.EndDateUTC;
                        endtimes[expedition.Name] = () => endtime;
                    }
                }
                else if (expedition.EndDateUTC != null)
                {
                    var endtime = (DateTime)expedition.EndDateUTC;
                    endtimes[expedition.Name] = () => endtime;
                    starttimes[expedition.Name] = starttimes["All"];
                }
            }

            filterStartTime = starttimes["All"]();
            filterEndTime = DateTime.UtcNow.AddYears(1);

            string lastsel = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("Map3DFilter", "");
            foreach (var kvp in starttimes)
            {
                var name = kvp.Key;
                var startfunc = kvp.Value;
                var endfunc = endtimes.ContainsKey(name) ? endtimes[name] : () => DateTime.Now.AddDays(1);
                var item = AddExpeditionInt(name, startfunc, endfunc);

                if (item.Text.Equals(lastsel))              // if a standard one, restore.  WE are not saving custom.
                {                                           // if custom is selected, we don't restore a tick.
                    item.Checked = true;
                    filterStartTime = startfunc();
                    filterEndTime = endfunc();
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

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("Map3DFilter", "Custom");                   // Custom is not saved, but clear last entry.

            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();
            int width = 300;

            f.Add(new ExtendedControls.ConfigurableForm.Entry("From", typeof(ExtendedControls.ExtDateTimePicker), filterStartTime.ToStringUS(), new Point(10, 40), new Size(width - 20, 24), null));
            f.Add(new ExtendedControls.ConfigurableForm.Entry("To", typeof(ExtendedControls.ExtDateTimePicker), filterEndTime.ToStringUS(), new Point(10, 80), new Size(width - 20, 24), null));
            f.AddOK(new Point(width - 20 - 80, 120));
            f.AddCancel(new Point(width - 200, 120));

            f.Trigger += (dialogname, controlname, tag) =>
            {
                if (controlname == "OK" || controlname == "Cancel" || controlname == "Close")
                {
                    f.ReturnResult(controlname == "OK" ? DialogResult.OK : DialogResult.Cancel);
                }
            };

            DialogResult res = f.ShowDialogCentred(this.FindForm(), this.FindForm().Icon, "Time".T(EDTx.Time), closeicon:true);

            if (res == DialogResult.OK)
            {
                filterStartTime = f.GetDateTime("From").Value;
                filterEndTime = f.GetDateTime("To").Value;
                GenerateDataSetsSystemList();
                RequestPaint();
            }
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

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("Map3DFilter", sel.Text);
            filterStartTime = startfunc();
            filterEndTime = endfunc == null ? DateTime.Now.AddDays(1) : endfunc();
            GenerateDataSetsSystemList();
            RequestPaint();
        }


        #endregion

    }



}

