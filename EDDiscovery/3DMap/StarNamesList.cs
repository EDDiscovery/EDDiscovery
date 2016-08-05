using EDDiscovery.DB;
using EDDiscovery2._3DMap;
using EDDiscovery2.DB;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace EDDiscovery2
{
    public class StarNames    // Holds stars which have been named..
    {
        public StarNames() { }

        public StarNames(ISystem other)
        {
            id = other.id;
            name = other.name;
            x = other.x; y = other.y; z = other.z;
            population = other.population;
            newtexture = null; newstar = null;
            painttexture = null; paintstar = null;
            candisposepainttexture = false;
            todispose = false;
        }

        public long id { get; set; }                             // EDDB ID, or 0 if not known
        public string name { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public long population { get; set; }
        public TexturedQuadData newtexture { get; set; }
        public PointData newstar { get; set; }                  // purposely drawing it like this, one at a time, due to sync issues between foreground/thread
        public TexturedQuadData painttexture { get; set; }
        public PointData paintstar { get; set; }                // instead of doing a array paint.

        public bool candisposepainttexture { get; set; }
        public bool todispose { get; set; }
    };


    public class StarNamesList
    {
        Matrix4d _starname_resmat;                  // to pass to thread..
        bool _starname_repaintall;                  // to pass to thread..
        Vector3 _starname_camera_lastpos;           // last estimated camera pos
        Vector3 _starname_camera_lastdir;           // and direction..
        bool _starname_camera_paint_lookdown = false; // true, we are above the stars
        bool _starname_camera_paint_lookforward = false; // true, we are pointing +z ie towards saga* from sol
        float _starname_curstars_zoom = ZoomOff;    // and what zoom.. 
        const float ZoomOff = -1000000F;            // zoom off flag
        int _starlimitly = 5000;                     // stars within this, div zoom.  F1/F2 adjusts this
        int _starnamesizely = 1200;                  // star name width, div zoom
        int _starnameminly = 2;                      // ranging between
        int _starnamemaxly = 80;
        Font _starnamebitmapfnt;
        int _starnamebitmapwidth, _starnamebitmapheight;
        Dictionary<Vector3d, StarNames> _starnames;

        StarGrids _stargrids;
        FormMap _formmap;
        GLControl _glControl;

        float _zoom;
        Vector3 _viewtargetpos;
        float _znear;

        Object deletelock = new Object();           // locked during delete..

        System.Threading.Thread nsThread;

        public StarNamesList( StarGrids sg, FormMap _fm , GLControl gl )
        {
            _stargrids = sg;
            _formmap = _fm;
            _glControl = gl;

            _starnames = new Dictionary<Vector3d, StarNames>();

            string fontname = "MS Sans Serif";                  // calculate once for bitmap 
            _starnamebitmapfnt = new Font(fontname, 20F);

            Bitmap text_bmp = new Bitmap(100, 30);
            using (Graphics g = Graphics.FromImage(text_bmp))
            {
                SizeF sz = g.MeasureString("Blah blah EX22 LYXX2", _starnamebitmapfnt);
                _starnamebitmapwidth = (int)sz.Width + 4;
                _starnamebitmapheight = (int)sz.Height + 4;
            }

            _starname_curstars_zoom = ZoomOff;             // reset zoom to make it recalc the named stars..
        }

        public bool RemoveAllNamedStars()                               // indicate all no paint, pass back if changed anything.
        {
            bool changed = false;

            if (_starname_curstars_zoom != ZoomOff)
            {
                foreach (StarNames sys in _starnames.Values)
                {
                    if (sys.painttexture != null)
                    {
                        sys.candisposepainttexture = true;
                        changed = true;
                    }
                }

                _starname_curstars_zoom = ZoomOff;
            }

            return changed;
        }

        public void RecalcStarNames()                                  // force recalc..
        {
            _starname_curstars_zoom = ZoomOff;
        }

        public void IncreaseStarLimit()
        {
            _starlimitly += 500;
            RecalcStarNames();
        }

        public void DecreaseStarLimit()
        {
            if (_starlimitly > 500)
            { 
                _starlimitly -= 500;
                RecalcStarNames();
            }
        }

        public bool Update(float z, Vector3 c, Vector3 _cameraDir, Matrix4d resmat , float _zn)     // UI thread..
        {
            _zoom = z;
            _viewtargetpos = c;
            _znear = _zn;

            if (!_stargrids.IsDisplayed(_viewtargetpos.X, _viewtargetpos.Z))
                return false;                            // okay, if we have not got to the position of displaying this grid, just wait

            Vector3 modcampos = _viewtargetpos;
            modcampos.Y = -modcampos.Y;
            bool lookdown = (_cameraDir.X < 90F);          // lookdown when X < 90
            bool lookforward = (_cameraDir.Y > -90F && _cameraDir.Y < 90F);  // forward looking

            if (_cameraDir.Z < -90F || _cameraDir.Z > 90F)       // this has the effect of turning our world up side down!
            {
                lookdown = !lookdown;
                lookforward = !lookforward;
            }

            bool repaintall = (lookdown != _starname_camera_paint_lookdown) ||          // have we flipped?
                              (lookforward != _starname_camera_paint_lookforward) ||
                               Math.Abs(_starname_curstars_zoom - _zoom) > 0.5F;          // if its worth doing a recalc..

            if (Vector3.Subtract(_starname_camera_lastpos, modcampos).LengthSquared > 3 * 3 * 3 ||
                Vector3.Subtract(_starname_camera_lastdir, _cameraDir).LengthSquared > 1 * 1 * 1 ||
                repaintall)
            {
                //Console.WriteLine("Rescan stars zoom " + _zoom);
                _starname_camera_lastpos = modcampos;
                _starname_camera_lastdir = _cameraDir;
                _starname_curstars_zoom = _zoom;
                _starname_camera_paint_lookdown = lookdown;
                _starname_camera_paint_lookforward = lookforward;

                _starname_resmat = resmat;
                _starname_repaintall = repaintall;          // pass to thread

                //Console.WriteLine("Tick start thread");
                nsThread = new System.Threading.Thread(NamedStars) { Name = "Calculate Named Stars", IsBackground = true };
                nsThread.Start();
                return true;
            }
            else
                return false;
        }

        private void NamedStars() // background thread.. run after timer tick
        {
            try // just in case someone tears us down..
            {
                int lylimit = (int)(_starlimitly / _zoom);
                lylimit = Math.Max(lylimit, 20);
                //Console.Write("Look down " + _starname_camera_paint_lookdown + " look forward " + _starname_camera_paint_lookforward);
                //Console.Write("Repaint " + _starname_repaintall + " Stars " + _starlimitly + " within " + lylimit + "  ");
                int sqlylimit = lylimit * lylimit;                 // in squared distance limit from viewpoint

                Vector3 modcampos = _viewtargetpos;
                modcampos.Y = -modcampos.Y;

                StarGrid.TransFormInfo ti = new StarGrid.TransFormInfo(_starname_resmat, _znear, _glControl.Width, _glControl.Height, sqlylimit, modcampos);

                SortedDictionary<float, Vector3d> inviewlist = new SortedDictionary<float, Vector3d>(new DuplicateKeyComparer<float>());       // who's in view, sorted by distance

                _stargrids.GetSystemsInView(ref inviewlist, 2000.0, ti);            // consider all grids under 2k from current pos.


                float textoffset = 0.35F;
                float textwidthly = Math.Min(_starnamemaxly, Math.Max(_starnamesizely / _zoom, _starnameminly)) + textoffset;
                float textheightly = textwidthly / 10;

                if (!_starname_camera_paint_lookdown)         // flip bitmap to make it look at you..
                {
                    if (!_starname_camera_paint_lookforward)
                    {
                        textoffset = -textoffset;
                        textwidthly = -textwidthly;
                    }
                    else
                        textheightly = -textheightly;
                }
                else if (!_starname_camera_paint_lookforward)
                {
                    textheightly = -textheightly;
                    textoffset = -textoffset;
                    textwidthly = -textwidthly;
                }

                float starsize = Math.Min(Math.Max(_zoom / 5F, 1.0F), 20F);     // Normal stars are at 1F.
                //Console.WriteLine((Environment.TickCount % 10000) + "Text " + _starnamesizely + " text " + textwidthly.ToString("0.0") + "," + textheightly.ToString("0.0") + " star size " + starsize.ToString("0.0") + " Lylimit " + lylimit);

                lock (deletelock)                                          // can't delete during update, can paint..
                {
                    foreach (StarNames s in _starnames.Values)              // all items not processed
                        s.todispose = true;                                 // only items remaining will clear this

                    int limit = 1000;                   // max number of stars to show..
                    int painted = 0;

                    using (SQLiteConnectionED cn = new SQLiteConnectionED())
                    {
                        foreach (Vector3d pos in inviewlist.Values)            // for all in viewport, sorted by distance from camera position
                        {
                            StarNames sys = null;
                            bool draw = false;

                            if (_starnames.ContainsKey(pos))                   // if already there..
                            {
                                sys = _starnames[pos];
                                sys.todispose = false;                         // forced redraw due to change in orientation, or due to disposal
                                draw = _starname_repaintall || sys.newstar == null || sys.newtexture == null;
                                painted++;
                            }
                            else if (painted < limit)
                            {
                                ISystem sc = _formmap.FindSystem(pos, cn);               // with the open connection, find this star..

                                if (sc != null)     // if can't be resolved, ignore
                                {
                                    sys = new StarNames(sc);
                                    lock (_starnames)
                                    {
                                        _starnames.Add(pos, sys);               // need to lock over add.. in case display is working
                                    }
                                    draw = true;
                                    painted++;
                                }
                                else
                                {
                                    // Console.WriteLine("Failed to find " + pos.X + "," + pos.Y + "," + pos.Z);
                                }
                            }
                            else
                            {
                                break;      // no point doing any more..  Either the closest ones have been found, or a new one was painted
                            }

                            if (draw)
                            {
                                Bitmap map = DatasetBuilder.DrawString(sys.name, _starnamebitmapfnt, _starnamebitmapwidth, _starnamebitmapheight, Color.Orange);

                                sys.newtexture = TexturedQuadData.FromBitmapHorz(map,
                                                 new PointF((float)sys.x + textoffset, (float)sys.z - textheightly / 2), new PointF((float)sys.x + textwidthly, (float)sys.z - textheightly / 2),
                                                 new PointF((float)sys.x + textoffset, (float)sys.z + textheightly / 2), new PointF((float)sys.x + textwidthly, (float)sys.z + textheightly / 2), (float)sys.y);

                                sys.newstar = new PointData(sys.x, sys.y, sys.z, starsize, (sys.population != 0) ? EDDConfig.Instance.MapColours.NamedStar : EDDConfig.Instance.MapColours.NamedStarUnpopulated);
                            }
                        }
                    }

                    foreach (StarNames s in _starnames.Values)              // only items above will remain.
                        s.candisposepainttexture = s.todispose;             // copy flag over, causes foreground to start removing them
                }

                //Console.WriteLine("  " + (Environment.TickCount % 10000) + "Paint " + painted);

                _formmap.Invoke((System.Windows.Forms.MethodInvoker)delegate              // kick the UI thread to process.
                {
                    _formmap.ChangeNamedStars();
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception watcher: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }
        }

        public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable      // special compare for sortedlist
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);
                return (result == 0) ? 1 : result;      // for this, equals just means greater than, to allow duplicate distance values to be added.
            }
        }

        public void Draw()
        {
            if (_starnames.Count > 10000)
            {
                if (Monitor.TryEnter(deletelock))                 // if we can get in, we are not in the update above, so can clean
                {                                                  // its a lazy delete, no rush..
                    List<Vector3d> cleanuplist = new List<Vector3d>();

                    foreach (Vector3d key in _starnames.Keys)
                    {
                        StarNames sys = _starnames[key];

                        if (sys.painttexture == null)             // if not painting
                            cleanuplist.Add(key);                 // add to clean up
                    }

                    Console.WriteLine("Clean up star names from " + _starnames.Count + " to " + (_starnames.Count - cleanuplist.Count));

                    foreach (Vector3d key in cleanuplist)
                        _starnames.Remove(key);

                    Monitor.Exit(deletelock);
                    GC.Collect();
                }
            }

            lock (_starnames)                                   // lock so they can't add anything while we draw
            {
                foreach (StarNames sys in _starnames.Values)
                {
                    if (sys.candisposepainttexture)             // flag is controlled by thread.. don't clear here..
                    {
                        if (sys.painttexture != null)           // paint texture controlled by this foreground
                        {
                            sys.painttexture.Dispose();
                            sys.painttexture = null;
                        }
                        if (sys.paintstar != null)              // star controlled by this foreground
                        {
                            sys.paintstar = null;
                        }
                    }

                    if (sys.newtexture != null)            // new is controlled by thread..
                    {
                        if (sys.painttexture != null)
                            sys.painttexture.Dispose();

                        sys.painttexture = sys.newtexture;      // copy over and take another reference.. 
                        sys.newtexture = null;
                    }
                    if (sys.newstar != null)              // same with newstar
                    {
                        sys.paintstar = sys.newstar;
                        sys.newstar = null;
                    }

                    if (sys.paintstar != null)                  // if star disk, paint..
                        sys.paintstar.Draw(_glControl);

                    if (sys.painttexture != null)           // being paranoid by treating these separately. Thread may finish painting one before the other.
                        sys.painttexture.Draw(_glControl);
                }
            }
        }

    }
}
