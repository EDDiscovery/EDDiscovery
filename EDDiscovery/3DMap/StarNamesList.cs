/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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
using OpenTK;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using OpenTKUtils.GL1;
using OpenTKUtils.Common;

namespace EDDiscovery
{
    public class StarNames                                      // Holds stars which have been named..
    {
        public StarNames() { }

        public StarNames(ISystem other , Vector3 posf)          // we can't use the position in other, as its in doubles. and we must be able to accurately match
        {
            name = other.Name;
            pos = posf;
            population = other.Population;

            nametexture = null;
            newnametexture = null;

            paintstar = null;
            newstar = null;

            inview = false;
            updatedinview = true;
        }

        public long id { get; set; }                             
        public string name { get; set; }
        public Vector3 pos { get; set; }                        // need to keep it in the same floats as whats returned in the inview list for matching
        public long population { get; set; }

        public TexturedQuadData newnametexture { get; set; }    // if a new texture is needed..
        public TexturedQuadData nametexture { get; set; }       // currently painted one

        public PointData newstar { get; set; }                  // purposely drawing it like this, one at a time, due to sync issues between foreground/thread
        public PointData paintstar { get; set; }                // instead of doing a array paint.

        public bool inview { get; set; }
        public bool updatedinview { get; set; }

        public Vector3 rotation;                                // when we drew it, what was its position and zoom
        public float zoom;

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

        Matrix4 _resmat;                            // to pass to thread..
        bool _discson;                              // to pass to thread..
        bool _nameson;                              // to pass to thread..
        float _znear;                               // to pass to thread..
        Color _namecolour;                          // colour of names

        int _starlimitly = 5000;                    // stars within this, div zoom.  F1/F2 adjusts this
        float _starnamesizely = 40F;                // star name width, div zoom
        float _starnameminly = 0.1F;                // ranging between per char
        float _starnamemaxly = 0.5F;
        float _startextoffset = 0.2F;               //ly

        Dictionary<Vector3, StarNames> _starnamesbackground;        // only used by background thread. 
        List<StarNames> _starnamestoforeground;                     // transfer list between the back/fore
        LinkedList<StarNames> _starnamesforeground;                 // foreground list, linked list since we remove entries in the middle at random

        static Font _starfont = BaseUtils.FontLoader.GetFont("MS Sans Serif", 16F);       // font size really determines the nicenest of the image, not its size on screen.. 12 point enough

        StarGrids _stargrids;
        FormMap _formmap;
        GLControl _glControl;
        CameraDirectionMovementTracker _lastcamera;

        bool _starnamesbusy = false;                            // Are we in a compute cycle..
        bool _needrepaint = false;                  // set if thread changed anything

        int limitperdraw = 100;                     // setting this higher causes the DRAW to take longer as it has to submit the stuff to GL..
        int maxstars = 1000;                        // maximum stars on view
        int maxstarscached = 10000;                 // how many we hold onto before we clean up

        System.Threading.Thread nsThread;


        #region Initialisation
        
        public StarNamesList(StarGrids sg, FormMap _fm, GLControl gl)
        {
            _stargrids = sg;
            _formmap = _fm;
            _glControl = gl;

            _starnamesbackground = new Dictionary<Vector3, StarNames>();
            _starnamestoforeground = new List<StarNames>();
            _starnamesforeground = new LinkedList<StarNames>();
        }

        #endregion

        #region Public controls

        public void IncreaseStarLimit()
        {
            _starlimitly += 100;
        }

        public void DecreaseStarLimit()
        {
            if (_starlimitly > 100)
            {
                _starlimitly -= 100;
            }
        }

        #endregion

        #region UI Functions

        public void Update(CameraDirectionMovementTracker lastcamera, Matrix4 resmat, float _zn, bool names, bool discs , Color namecolour)     // FOREGROUND no thread
        {
            _starnamesbusy = true;      // from update to Transfertoforeground we are busy

            if (_starnamesforeground.Count >= maxstarscached)                       // if we have too many, clean up, to free memory
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
                    _starnamesbackground.Remove(sys.pos);
                }
            }

            _lastcamera = lastcamera;
            _resmat = resmat;
            _znear = _zn;
            _nameson = names;
            _discson = discs;
            _namecolour = namecolour;

            nsThread = new System.Threading.Thread(NamedStars) { Name = "Calculate Named Stars", IsBackground = true };
            nsThread.Start();
        }


        public bool TransferToForeground()              // FOREGROUND no thread. Return if above has changed anything..
        {
            foreach (StarNames sys in _starnamestoforeground)
                _starnamesforeground.AddLast(sys);

            _starnamestoforeground.Clear();

            _starnamesbusy = false;                     // now not busy, another cycle may start..

            return _needrepaint;
        }

        public bool HideAll()                           // FOREGROUND no thread
        {
            bool changed = false;

            foreach (StarNames sys in _starnamesforeground)
            {
                if (sys.inview)
                {
                    sys.inview = false;
                    changed = true;
                }
            }

            return changed;
        }

        #endregion

        #region Background worker

        private void NamedStars() // background thread.. run after Update.  Thread never deletes, only adds to its own structures
        {
            try // just in case someone tears us down..
            {
                int lylimit = (int)(_starlimitly / _lastcamera.LastZoom);
                lylimit = Math.Max(lylimit, 1);
                int sqlylimit = lylimit * lylimit;                 // in squared distance limit from viewpoint

                StarGrid.TransFormInfo ti = new StarGrid.TransFormInfo(_resmat, _znear, _glControl.Width, _glControl.Height, sqlylimit, _lastcamera.LastPosition);

                SortedDictionary<float, StarGrid.InViewInfo> inviewlist = new SortedDictionary<float, StarGrid.InViewInfo>(new DuplicateKeyComparer<float>());       // who's in view, sorted by distance

                //Stopwatch sw1 = new Stopwatch();sw1.Start(); Tools.LogToFile(String.Format("starnamesest Estimate at {0} len {1}", ti.campos, sqlylimit));

                _stargrids.GetSystemsInView(ref inviewlist, 2000.0F, ti);            // consider all grids under 2k from current pos.

                //Tools.LogToFile(String.Format("starnamesest Took {0} in view {1}", sw1.ElapsedMilliseconds, inviewlist.Count));

                float textscalingw = Math.Min(_starnamemaxly, Math.Max(_starnamesizely / _lastcamera.LastZoom, _starnameminly)); // per char
                float starsize = Math.Min(Math.Max(_lastcamera.LastZoom / 10F, 1.0F), 20F);     // Normal stars are at 1F.

                System.Diagnostics.Debug.WriteLine("Named Stars begin search");

                foreach (StarNames s in _starnamesbackground.Values)    // all items not processed
                    s.updatedinview = false;                                 // only items remaining will clear this

                _needrepaint = false;               // assume nothing changes

                int painted = 0;

                //string res = "";  // used to view whats added/removed/draw..

                foreach (StarGrid.InViewInfo inview in inviewlist.Values)            // for all in viewport, sorted by distance from camera position
                {
                    StarNames sys = null;
                    bool draw = false;

                    if (_starnamesbackground.ContainsKey(inview.position))                   // if already there..
                    {
                        sys = _starnamesbackground[inview.position];
                        sys.updatedinview = true;

                        draw = (_discson && sys.paintstar == null && sys.newstar == null) || 
                                (_nameson && ((sys.nametexture == null && sys.newnametexture == null) ));
                            
                        painted++;
                        //System.Diagnostics.Debug.WriteLine("In view there " + sys.name + " draw " + draw);
                    }
                    else if (painted < maxstars)
                    {
                        ISystem sc = _formmap.FindSystem(inview.position);               // with the open connection, find this star..

                        if (sc != null)     // if can't be resolved, ignore
                        {
                            sys = new StarNames(sc, inview.position);           // we keep position in here using same floats as inview so it will match
                            _starnamesbackground.Add(inview.position, sys);     // add to our database
                            _starnamestoforeground.Add(sys);  // send to foreground for adding
                            draw = true;
                            painted++;

                            //System.Diagnostics.Debug.WriteLine("Found " + inview.position);
                            //Tools.LogToFile(String.Format("starnamesest: push {0}", sys.Pos));
                            //res += "N";
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Cant find " + inview.position);
                        }
                    }
                    else
                    {
                        break;      // no point doing any more..  got our fill of items
                    }

                    if (draw)
                    {
                        _needrepaint = true;                                            // changed a item.. needs a repaint

                        if (_nameson)
                        {
                            float width = textscalingw * sys.name.Length;

                            Bitmap map = DatasetBuilder.DrawString(sys.name, _namecolour, _starfont);

                            sys.newnametexture = TexturedQuadData.FromBitmap(map,
                                                    new PointData(sys.pos.X, sys.pos.Y, sys.pos.Z),
                                                    _lastcamera.Rotation,
                                                    width, textscalingw * 4.0F, _startextoffset + width / 2, 0);

                            sys.rotation = _lastcamera.Rotation;            // remember when we were when we draw it
                            sys.zoom = _lastcamera.LastZoom;
                        }

                        if (_discson)
                        {
                            sys.newstar = new PointData(sys.pos.X, sys.pos.Y, sys.pos.Z, starsize, inview.AsColor);
                        }
                    }
                }

                System.Diagnostics.Debug.WriteLine("Process " + _starnamesbackground.Count);

                foreach (StarNames s in _starnamesbackground.Values)              // only items above will remain.
                {
                    //if (s.inview != s.updatedinview) res += (s.updatedinview) ? "+" : "-";

                    _needrepaint = _needrepaint || (s.inview != s.updatedinview);       // set if we change our mind on any of the items
                    s.inview = s.updatedinview;                          // copy flag over, causes foreground to start removing them
                }

                //if ( _needrepaint) Console.WriteLine(String.Format("starnamesest in view  {0} limit {1} repaint {2} {3}", inviewlist.Count, lylimit, _needrepaint, res));

                //Tools.LogToFile(String.Format("starnamesest added all delta {0} topaint {1}", sw1.ElapsedMilliseconds, painted));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception watcher: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }

            _formmap.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate              // kick the UI thread to process.
            {
                System.Diagnostics.Debug.WriteLine("Tell forground to change");
                _formmap.ChangeNamedStars();
            });
        }

        #endregion


        #region Painting

        public bool Draw( bool workallowed , float lastzoom , Vector3 lastrot )                              // FOREGROUND thread may be running.. so use only foreground object
        {
            float textscalingw = Math.Min(_starnamemaxly, Math.Max(_starnamesizely / lastzoom, _starnameminly)); // per char - sync with above in background
            float starsize = Math.Min(Math.Max(lastzoom / 10F, 1.0F), 20F);     // Normal stars are at 1F.

            bool needmoreticks = false;

            int updated = 0;
            //bool DEBUGscaled = false;

            foreach (StarNames sys in _starnamesforeground)
            {
                if (sys.newnametexture != null )         
                {
                    if (updated < limitperdraw && workallowed)
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

                if (sys.newstar != null)              // same with newstar
                {
                    sys.paintstar = sys.newstar;
                    sys.newstar = null;
                }

                if ( sys.inview )                       // in view, send it to the renderer
                {
                    bool zoomchanged = sys.zoom != lastzoom;

                    if (sys.paintstar != null && _discson)                  // if star disk, paint..
                    {
                        if (zoomchanged)
                        {
                            sys.paintstar.Size = starsize;
                            sys.zoom = lastzoom;
                        }

                        sys.paintstar.Draw(_glControl);
                    }

                    if (sys.nametexture != null && _nameson )       
                    {
                        if ( zoomchanged || sys.rotation != lastrot)  // if we have rotated since last vertex calc, either here or in background when created
                        {
                            float width = textscalingw * sys.name.Length;
                            sys.nametexture.UpdateVertices( TexturedQuadData.CalcVertices(sys.pos, lastrot , width, textscalingw * 4.0F, _startextoffset + width / 2, 0));
                            sys.zoom = lastzoom;
                            sys.rotation = lastrot;
                            //DEBUGscaled = true;
                        }

                        sys.nametexture.Draw(_glControl);
                    }
                }
            }

            //if ( DEBUGscaled || updated > 0)  Tools.LogToFile(String.Format("starnamesdraw: Updated {0} scaled {1}", updated, DEBUGscaled));

            return needmoreticks;
        }

        #endregion

        #region Finding in this name list

        public Vector3? FindOverSystem(int x, int y, out float cursysdistz, StarGrid.TransFormInfo ti) // FOREGROUND thread may be running
        {
            cursysdistz = float.MaxValue;
            Vector3? ret = null;

            float w2 = (float)ti.dwidth / 2.0F;
            float h2 = (float)ti.dheight / 2.0F;

            foreach (StarNames sys in _starnamesforeground)
            {
                if (sys.paintstar != null)
                {
                    Vector4 syspos = new Vector4(sys.pos.X, sys.pos.Y, sys.pos.Z, 1.0F);
                    Vector4 sysloc = Vector4.Transform(syspos, ti.resmat);

                    if (sysloc.Z > ti.znear)
                    {
                        Vector2 syssloc = new Vector2(((sysloc.X / sysloc.W) + 1.0F) * w2 - x, ((sysloc.Y / sysloc.W) + 1.0F) * h2 - y);
                        float sysdistsq = syssloc.X * syssloc.X + syssloc.Y * syssloc.Y;

                        if (sysdistsq < 7.0 * 7.0)
                        {
                            float sysdist = (float)Math.Sqrt(sysdistsq);

                            if ((sysdist + Math.Abs(sysloc.Z * ti.zoom)) < cursysdistz)
                            {
                                cursysdistz = sysdist + (float)Math.Abs(sysloc.Z * ti.zoom);
                                ret = sys.pos;
                            }
                        }
                    }
                }
            }

            return ret;
        }

        #endregion

        #region Misc

        public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable      // special compare for sortedlist
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);
                return (result == 0) ? 1 : result;      // for this, equals just means greater than, to allow duplicate distance values to be added.
            }
        }

        #endregion
    }

}

