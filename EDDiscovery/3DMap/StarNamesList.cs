using EDDiscovery.DB;
using EDDiscovery2._3DMap;
using EDDiscovery2.DB;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            inview = false;
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

        public bool inview { get; set; }
        public bool todispose { get; set; }
    };


    public class StarNamesList
    {
        Matrix4d _resmat;                  // to pass to thread..
        bool _flippedorzoomed;                  // to pass to thread..
        bool _discson;                            // to pass to thread..
        bool _nameson;                            // to pass to thread..
        float _znear;                            // to pass to thread..

        int _starlimitly = 5000;                    // stars within this, div zoom.  F1/F2 adjusts this
        float _starnamesizely = 40F;                // star name width, div zoom
        float _starnameminly = 0.1F;                // ranging between per char
        float _starnamemaxly = 0.5F;

        Dictionary<Vector3, StarNames> _starnames;

        static Font _starfont = new Font("MS Sans Serif", 16F);       // font size really determines the nicenest of the image, not its size on screen.. 12 point enough

        StarGrids _stargrids;
        FormMap _formmap;
        GLControl _glControl;

        Object deletelock = new Object();           // locked during delete..

        System.Threading.Thread nsThread;

        public StarNamesList(StarGrids sg, FormMap _fm, GLControl gl)
        {
            _stargrids = sg;
            _formmap = _fm;
            _glControl = gl;

            _starnames = new Dictionary<Vector3, StarNames>();
        }

        public bool RemoveAllNamedStars()                               // indicate all no paint, pass back if changed anything.
        {
            bool changed = false;

            foreach (StarNames sys in _starnames.Values)
            {
                if (sys.painttexture != null || sys.paintstar != null)
                {
                    sys.inview = false;
                    changed = true;
                }
            }

            return changed;
        }

        public void IncreaseStarLimit()
        {
            _starlimitly += 500;
        }

        public void DecreaseStarLimit()
        {
            if (_starlimitly > 500)
            {
                _starlimitly -= 500;
            }
        }

        CameraDirectionMovement _lastcamera;

        public void Update(CameraDirectionMovement lastcamera, bool flippedorzoomed, Matrix4d resmat, float _zn, bool names, bool discs)     // UI thread..
        {
            _lastcamera = lastcamera;
            _resmat = resmat;
            _znear = _zn;
            _nameson = names;
            _discson = discs;
            _flippedorzoomed = flippedorzoomed;

            //Console.WriteLine("Tick start thread");
            nsThread = new System.Threading.Thread(NamedStars) { Name = "Calculate Named Stars", IsBackground = true };
            nsThread.Start();
        }

        private void NamedStars() // background thread.. run after timer tick
        {
            try // just in case someone tears us down..
            {
                int lylimit = (int)(_starlimitly / _lastcamera.LastZoom);
                lylimit = Math.Max(lylimit, 20);
                //Console.Write("Look down " + _camera_paint_lookdown + " look forward " + _camera_paint_lookforward);
                //Console.Write("Repaint " + _repaintall + " Stars " + _starlimitly + " within " + lylimit + "  ");
                int sqlylimit = lylimit * lylimit;                 // in squared distance limit from viewpoint

                Vector3 modcampos = _lastcamera.CameraPos;
                modcampos.Y = -modcampos.Y;

                StarGrid.TransFormInfo ti = new StarGrid.TransFormInfo(_resmat, _znear, _glControl.Width, _glControl.Height, sqlylimit, modcampos);

                SortedDictionary<float, StarGrid.InViewInfo> inviewlist = new SortedDictionary<float, StarGrid.InViewInfo>(new DuplicateKeyComparer<float>());       // who's in view, sorted by distance

                _stargrids.GetSystemsInView(ref inviewlist, 2000.0, ti);            // consider all grids under 2k from current pos.

                float textscalingw = Math.Min(_starnamemaxly, Math.Max(_starnamesizely / _lastcamera.LastZoom, _starnameminly)); // per char
                float textscalingh = textscalingw * 4;
                float textoffset = .20F;
                float starsize = Math.Min(Math.Max(_lastcamera.LastZoom / 10F, 1.0F), 20F);     // Normal stars are at 1F.
                //Console.WriteLine("Per char {0} h {1} sc {2} ", textscalingw, textscalingh, starsize);

                lock (deletelock)                                          // can't delete during update, can paint..
                {
                    foreach (StarNames s in _starnames.Values)              // all items not processed
                        s.todispose = true;                                 // only items remaining will clear this

                    int limit = 1000;                   // max number of stars to show..
                    int painted = 0;

                    using (SQLiteConnectionED cn = new SQLiteConnectionED())
                    {
                        foreach (StarGrid.InViewInfo inview in inviewlist.Values)            // for all in viewport, sorted by distance from camera position
                        {
                            StarNames sys = null;
                            bool draw = false;

                            if (_starnames.ContainsKey(inview.position))                   // if already there..
                            {
                                sys = _starnames[inview.position];
                                sys.todispose = false;                         // forced redraw due to change in orientation, or due to disposal
                                draw = _flippedorzoomed || (_nameson && sys.newstar == null) || (_discson && sys.newtexture == null);
                                painted++;
                            }
                            else if (painted < limit)
                            {
                                ISystem sc = _formmap.FindSystem(inview.position, cn);               // with the open connection, find this star..

                                if (sc != null)     // if can't be resolved, ignore
                                {
                                    sys = new StarNames(sc);
                                    lock (_starnames)
                                    {
                                        _starnames.Add(inview.position, sys);               // need to lock over add.. in case display is working
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
                                if (_nameson)
                                {
                                    float width = textscalingw * sys.name.Length;

                                    Bitmap map;                     // now, delete is the only one who removed newtexture
                                                                    // and we are protected against delete..
                                    if (sys.newtexture == null)     // so see if newtexture is there
                                        map = DatasetBuilder.DrawString(sys.name, Color.Orange, _starfont);
                                    else
                                        map = sys.newtexture.Texture;

                                    sys.newtexture = TexturedQuadData.FromBitmap(map,
                                        new PointData(sys.x, sys.y, sys.z),
                                        _lastcamera.Rotation,
                                        width, textscalingh, textoffset + width / 2, 0);
                                }

                                if (_discson)
                                {
                                    sys.newstar = new PointData(sys.x, sys.y, sys.z, starsize, Color.FromArgb(255, inview.colour & 255, (inview.colour >> 8) & 255, (inview.colour >> 16) & 255));
                                }
                            }
                        }
                    }

                    foreach (StarNames s in _starnames.Values)              // only items above will remain.
                        s.inview = !s.todispose;                          // copy flag over, causes foreground to start removing them
                }

                //Console.WriteLine("  " + (Environment.TickCount % 10000) + "Paint " + painted);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception watcher: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }

            _formmap.Invoke((System.Windows.Forms.MethodInvoker)delegate              // kick the UI thread to process.
            {
                _formmap.ChangeNamedStars();
            });
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
            //Stopwatch sw = new Stopwatch();  sw.Start();

            if (_starnames.Count > 10000)
            {
                if (Monitor.TryEnter(deletelock))                 // if we can get in, we are not in the update above, so can clean
                {                                                  // its a lazy delete, no rush..
                    List<Vector3> cleanuplist = new List<Vector3>();

                    foreach (Vector3 key in _starnames.Keys)
                    {
                        StarNames sys = _starnames[key];

                        if (!sys.inview)                          // if not painting
                            cleanuplist.Add(key);                 // add to clean up
                    }

                    Console.WriteLine("Clean up star names from " + _starnames.Count + " to " + (_starnames.Count - cleanuplist.Count));

                    foreach (Vector3 key in cleanuplist)
                    {
                        StarNames sys = _starnames[key];
                        if (sys.painttexture != null)
                            sys.painttexture.Dispose();

                        _starnames.Remove(key);
                    }

                    Monitor.Exit(deletelock);
                    GC.Collect();
                }
            }

            lock (_starnames)                                   // lock so they can't add anything while we draw
            {
                //int painted = 0;
                foreach (StarNames sys in _starnames.Values)
                {
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

                    if ( sys.inview )                       // in view, send it to the renderer
                    { 
                        if (sys.paintstar != null)                  // if star disk, paint..
                            sys.paintstar.Draw(_glControl);

                        if (sys.painttexture != null)           // being paranoid by treating these separately. Thread may finish painting one before the other.
                        {
                            sys.painttexture.Draw(_glControl);
                            //painted++;
                        }
                    }
                }

                //Console.WriteLine("Painted {0} out of {1}", painted, _starnames.Count);
            }

            //long e = sw.ElapsedMilliseconds; if (e > 1) Console.WriteLine("Elapsed " + e);
        }
        
        public Vector3? FindOverSystem(int x, int y, out double cursysdistz, StarGrid.TransFormInfo ti)
        {
            cursysdistz = double.MaxValue;
            Vector3? ret = null;

            double w2 = (double)ti.dwidth / 2.0;
            double h2 = (double)ti.dheight / 2.0;

            lock (_starnames)                                   // lock so they can't add anything while we draw
            {
                foreach (StarNames sys in _starnames.Values)
                {
                    if (sys.paintstar != null)
                    {
                        Vector4d syspos = new Vector4d((float)sys.x, (float)sys.y, (float)sys.z, 1.0);
                        Vector4d sysloc = Vector4d.Transform(syspos, ti.resmat);

                        if (sysloc.Z > ti.znear)
                        {
                            Vector2d syssloc = new Vector2d(((sysloc.X / sysloc.W) + 1.0) * w2 - x, ((sysloc.Y / sysloc.W) + 1.0) * h2 - y);
                            double sysdistsq = syssloc.X * syssloc.X + syssloc.Y * syssloc.Y;

                            if (sysdistsq < 7.0 * 7.0)
                            {
                                double sysdist = Math.Sqrt(sysdistsq);

                                if ((sysdist + Math.Abs(sysloc.Z * ti.zoom)) < cursysdistz)
                                {
                                    cursysdistz = sysdist + Math.Abs(sysloc.Z * ti.zoom);
                                    ret = new Vector3((float)sys.x, (float)sys.y, (float)sys.z);
                                }
                            }
                        }
                    }
                }
            }

            return ret;
        }
    }

}
