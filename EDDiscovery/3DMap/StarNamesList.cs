using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2._3DMap;
using EDDiscovery2.DB;
using OpenTK;
using System;
using System.Collections.Concurrent;
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
            newnametexture = null; newstar = null;
            nametexture = null; paintstar = null;
            newnamevertices = null;
            inview = false;
            todispose = false;
        }

        public long id { get; set; }                             // EDDB ID, or 0 if not known
        public string name { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public Vector3 position { get { return new Vector3((float)x, (float)y, (float)z);  } }
        public long population { get; set; }

        public TexturedQuadData newnametexture { get; set; }    // if a new texture is needed..
        public TexturedQuadData nametexture { get; set; }       // currently painted one
        public Vector3d[] newnamevertices;                      

        public PointData newstar { get; set; }                  // purposely drawing it like this, one at a time, due to sync issues between foreground/thread
        public PointData paintstar { get; set; }                // instead of doing a array paint.

        public bool inview { get; set; }
        public bool todispose { get; set; }

        public void Dispose()
        {
            if (newnametexture != null)
                newnametexture.Dispose();

            if (nametexture != null)
                nametexture.Dispose();
        }
    };


    public class StarNamesList
    {
        public bool Busy { get { return _starnamesbusy; } }

        Matrix4d _resmat;                           // to pass to thread..
        bool _dirorzoomchange;                      // to pass to thread..
        bool _discson;                              // to pass to thread..
        bool _nameson;                              // to pass to thread..
        float _znear;                               // to pass to thread..
        Color _namecolour;                          // colour of names

        int _starlimitly = 5000;                    // stars within this, div zoom.  F1/F2 adjusts this
        float _starnamesizely = 40F;                // star name width, div zoom
        float _starnameminly = 0.1F;                // ranging between per char
        float _starnamemaxly = 0.5F;

        Dictionary<Vector3, StarNames> _starnamesbackground;        // only used by background thread. 
        List<StarNames> _starnamestoforeground;                     // transfer list between the back/fore
        LinkedList<StarNames> _starnamesforeground;                 // foreground list, linked list since we remove entries in the middle at random

        static Font _starfont = new Font("MS Sans Serif", 16F);       // font size really determines the nicenest of the image, not its size on screen.. 12 point enough

        StarGrids _stargrids;
        FormMap _formmap;
        GLControl _glControl;
        CameraDirectionMovementTracker _lastcamera;

        Object deletelock = new Object();           // locked during delete..

        System.Threading.Thread nsThread;

        bool _starnamesbusy = false;                            // Are we in a compute cycle..

        public StarNamesList(StarGrids sg, FormMap _fm, GLControl gl)
        {
            _stargrids = sg;
            _formmap = _fm;
            _glControl = gl;

            _starnamesbackground = new Dictionary<Vector3, StarNames>();
            _starnamestoforeground = new List<StarNames>();
            _starnamesforeground = new LinkedList<StarNames>();
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

        public void Update(CameraDirectionMovementTracker lastcamera, bool dirorzoomchange, 
                            Matrix4d resmat, float _zn, bool names, bool discs , Color namecolour)     // FOREGROUND no thread
        {
            _starnamesbusy = true;      // from update to Transfertoforeground we are busy

            if (_starnamesforeground.Count > 10000)                       // if we have too many, clean up, to free memory
            {
                List<LinkedListNode<StarNames>> removelist = new List<LinkedListNode<StarNames>>();
                LinkedListNode<StarNames> pos = _starnamesforeground.First;

                while (pos != null)
                {
                    StarNames sys = pos.Value;
                    if (!sys.inview)                          // if not painting
                        removelist.Add(pos);
                    pos = pos.Next;
                }

                //Tools.LogToFile(String.Format("starnameupd Remove {0}", removelist.Count ));
                //Console.WriteLine("Remove {0}", removelist.Count);
                
                foreach (LinkedListNode<StarNames> rpos in removelist)
                {
                    StarNames sys = rpos.Value;
                    sys.Dispose();

                    _starnamesforeground.Remove(rpos);
                    _starnamesbackground.Remove(sys.position);
                }
            }

            _lastcamera = lastcamera;
            _resmat = resmat;
            _znear = _zn;
            _nameson = names;
            _discson = discs;
            _dirorzoomchange = dirorzoomchange;
            _namecolour = namecolour;

            nsThread = new System.Threading.Thread(NamedStars) { Name = "Calculate Named Stars", IsBackground = true };
            nsThread.Start();
        }

        private void NamedStars() // background thread.. run after Update.  Thread never deletes, only adds
        {
            try // just in case someone tears us down..
            {
                int lylimit = (int)(_starlimitly / _lastcamera.LastZoom);
                lylimit = Math.Max(lylimit, 20);
                int sqlylimit = lylimit * lylimit;                 // in squared distance limit from viewpoint

                StarGrid.TransFormInfo ti = new StarGrid.TransFormInfo(_resmat, _znear, _glControl.Width, _glControl.Height, sqlylimit, _lastcamera.CameraPos);

                SortedDictionary<float, StarGrid.InViewInfo> inviewlist = new SortedDictionary<float, StarGrid.InViewInfo>(new DuplicateKeyComparer<float>());       // who's in view, sorted by distance

                //Stopwatch sw1 = new Stopwatch();sw1.Start(); Tools.LogToFile(String.Format("starnamesest Estimate at {0} len {1}", ti.campos, sqlylimit));

                _stargrids.GetSystemsInView(ref inviewlist, 2000.0, ti);            // consider all grids under 2k from current pos.

                //Tools.LogToFile(String.Format("starnamesest Took {0} in view {1}", sw1.ElapsedMilliseconds, inviewlist.Count));

                float textscalingw = Math.Min(_starnamemaxly, Math.Max(_starnamesizely / _lastcamera.LastZoom, _starnameminly)); // per char
                float textscalingh = textscalingw * 4;
                float textoffset = .20F;
                float starsize = Math.Min(Math.Max(_lastcamera.LastZoom / 10F, 1.0F), 20F);     // Normal stars are at 1F.
                //Console.WriteLine("Per char {0} h {1} sc {2} ", textscalingw, textscalingh, starsize);

                lock (deletelock)                                          // can't delete during update, can paint..
                {
                    foreach (StarNames s in _starnamesbackground.Values)    // all items not processed
                        s.todispose = true;                                 // only items remaining will clear this

                    int limit = 1000;                   // max number of stars to show..
                    int painted = 0;

                    using (SQLiteConnectionED cn = new SQLiteConnectionED())
                    {
                        foreach (StarGrid.InViewInfo inview in inviewlist.Values)            // for all in viewport, sorted by distance from camera position
                        {
                            StarNames sys = null;
                            bool draw = false;

                            if (_starnamesbackground.ContainsKey(inview.position))                   // if already there..
                            {
                                sys = _starnamesbackground[inview.position];
                                sys.todispose = false;                         // forced redraw due to change in orientation, or due to disposal
                                draw = _dirorzoomchange || (_nameson && sys.newstar == null) || (_discson && sys.nametexture == null);
                                painted++;
                            }
                            else if (painted < limit)
                            {
                                ISystem sc = _formmap.FindSystem(inview.position, cn);               // with the open connection, find this star..

                                if (sc != null)     // if can't be resolved, ignore
                                {
                                    sys = new StarNames(sc);
                                    _starnamesbackground.Add(sys.position, sys); // add to our database
                                    _starnamestoforeground.Add(sys);  // send to foreground for adding
                                    //Tools.LogToFile(String.Format("starnamesest: push {0}", sys.Pos));
                                    draw = true;
                                    painted++;
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
                                    if (sys.nametexture == null)     // so see if newtexture is there
                                    {
                                        map = DatasetBuilder.DrawString(sys.name, _namecolour, _starfont);
                                        sys.newnametexture = TexturedQuadData.FromBitmap(map,
                                            new PointData(sys.x, sys.y, sys.z),
                                            _lastcamera.Rotation,
                                            width, textscalingh, textoffset + width / 2, 0);
                                    }
                                    else
                                    {
                                        sys.newnamevertices = TexturedQuadData.CalcVertices( new PointData(sys.x, sys.y, sys.z),
                                                                                                _lastcamera.Rotation,
                                                                        width, textscalingh, textoffset + width / 2, 0);

                                    }
                                }

                                if (_discson)
                                {
                                    sys.newstar = new PointData(sys.x, sys.y, sys.z, starsize, inview.AsColor);
                                }
                            }
                        }
                    }

                    foreach (StarNames s in _starnamesbackground.Values)              // only items above will remain.
                        s.inview = !s.todispose;                          // copy flag over, causes foreground to start removing them

                    //Tools.LogToFile(String.Format("starnamesest added all delta {0} topaint {1}", sw1.ElapsedMilliseconds, painted));
                }

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

        public bool TransferToForeground()              // FOREGROUND no thread. Return if any new stuff 
        {
            bool ret = _starnamestoforeground.Count > 0;

            foreach (StarNames sys in _starnamestoforeground)
                _starnamesforeground.AddLast(sys);

            _starnamestoforeground.Clear();

            _starnamesbusy = false;                     // now not busy, another cycle may start..

            return ret; 
        }

        public bool HideAll()                           // FOREGROUND no thread
        {
            bool changed = false;

            foreach (StarNames sys in _starnamestoforeground)
            {
                if (sys.inview)
                {
                    sys.inview = false;
                    changed = true;
                }
            }

            return changed;
        }


        public bool Draw()                              // FOREGROUND thread may be running.. so use only foreground object
        {
            bool needmoreticks = false;

            int updated = 0;

            foreach (StarNames sys in _starnamesforeground )
            {
                if (sys.newnametexture != null )         //250 seems okay on my machine, around the 50ms mark
                {
                    if (updated < 250)
                    {
                        if (sys.nametexture != null)
                            sys.nametexture.Dispose();

                        sys.nametexture = sys.newnametexture;      // copy over and take another reference.. 
                        sys.newnametexture = null;
                        updated++;
                    }
                    else
                    {
                        needmoreticks = true;
                    }
                }

                if (sys.newnamevertices != null)
                {
                    if (updated < 250)
                    {
                        sys.nametexture.UpdateVertices(sys.newnamevertices);
                        sys.newnamevertices = null;
                        updated++;
                    }
                    else
                    {
                        needmoreticks = true;
                    }
                }

                if (sys.newstar != null)              // same with newstar
                {
                    sys.paintstar = sys.newstar;
                    sys.newstar = null;
                }

                if ( sys.inview )                       // in view, send it to the renderer
                {
                    if (sys.paintstar != null && _discson)                  // if star disk, paint..
                    {
                        sys.paintstar.Draw(_glControl);
                    }

                    if (sys.nametexture != null && _nameson )           // being paranoid by treating these separately. Thread may finish painting one before the other.
                    {
                        sys.nametexture.Draw(_glControl);
                    }
                }
            }

            //if (updated > 0) Tools.LogToFile(String.Format("starnamesdraw: {0} Updated {1} Not Updated {2}", sw1.ElapsedMilliseconds, updated, notupdated));

            return needmoreticks;
        }

        public Vector3? FindOverSystem(int x, int y, out double cursysdistz, StarGrid.TransFormInfo ti) // FOREGROUND thread may be running
        {
            cursysdistz = double.MaxValue;
            Vector3? ret = null;

            double w2 = (double)ti.dwidth / 2.0;
            double h2 = (double)ti.dheight / 2.0;

            foreach (StarNames sys in _starnamesforeground)
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

            return ret;
        }


        public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable      // special compare for sortedlist
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);
                return (result == 0) ? 1 : result;      // for this, equals just means greater than, to allow duplicate distance values to be added.
            }
        }
    }

}

