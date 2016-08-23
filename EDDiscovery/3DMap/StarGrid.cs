using EDDiscovery.DB;
using EDDiscovery2.DB;
using OpenTK;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Threading;
using System.Windows.Forms;
using EDDiscovery;

namespace EDDiscovery2
{
    public class StarGrid
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Z { get; set; }
        public int Percentage { get; set; }          // foreground flags
        public double CalculatedDistance { get; set; }  // foreground.. what distance did we calc on..
        public SystemClass.SystemAskType dBAsk { get; set; } // set for an explicit ask for unpopulated systems
        public int Count { get { return array1displayed ? array1vertices : array2vertices; } }
        public int CountJustMade { get { return array1displayed ? array2vertices : array1vertices; } }
        public bool Displayed = false;              // records if it was ever displayed
        public bool Working = false;

        bool array1displayed;
        private Vector3[] array1;            // the star points
        private uint[] carray1;                // the star colours
        int array1vertices;
        private Vector3[] array2;            // the star points
        private uint[] carray2;                // the star colours
        int array2vertices;

        public float Size { get; set; }
        public Color Color { get; set; }

        protected int VtxVboID;
        protected int VtxColorVboId;
        protected GLControl GLContext;

        Object lockdisplaydata = new Object();

        public StarGrid(int id, double x, double z, Color c, float s)
        {
            Id = id; X = x; Z = z;
            Percentage = 0;
            CalculatedDistance = 10E6;        // some large value, but not too large so ABS takes us overrange
            Color = c;
            Size = s;
            array1vertices = 0;
            array2vertices = 0;
            array1displayed = true;
        }

        public double DistanceFrom(double x, double z)
        { return Math.Sqrt((x - X) * (x - X) + (z - Z) * (z - Z)); }

        public void FillFromDB()        // does not affect the display object
        {
            if (array1displayed)
                array2vertices = SystemClass.GetSystemVector(Id, ref array2, ref carray2, dBAsk, Percentage);       // MAY return array/carray is null
            else
                array1vertices = SystemClass.GetSystemVector(Id, ref array1, ref carray1, dBAsk, Percentage);
        }

        public void FillFromVS(List<VisitedSystemsClass> cls) // does not affect the display object
        {
            if (array1displayed)
                array2vertices = FillFromVS(ref array2, ref carray2, cls, this.Color);
            else
                array1vertices = FillFromVS(ref array1, ref carray1, cls, this.Color);
        }

        private int FillFromVS( ref Vector3[] array, ref uint[] carray, List<VisitedSystemsClass> cls, Color basecolour)
        {
            carray = new uint[cls.Count];
            array = new Vector3[cls.Count];     // can't have any more than this 
            int total = 0;

            uint cx = BitConverter.ToUInt32(new byte[] { basecolour.R, basecolour.G, basecolour.B, basecolour.A }, 0);

            foreach (VisitedSystemsClass vs in cls)
            {                                                               // all vs stars which are not in edsm and have co-ords.
                if (vs.curSystem != null && vs.curSystem.status != SystemStatusEnum.EDSC && vs.curSystem.HasCoordinate )
                {
                    carray[total] = cx;      // Visited systems grid does not use the carray as its overriden, but starnames does
                    array[total++] = new Vector3((float)vs.curSystem.x, (float)vs.curSystem.y, (float)vs.curSystem.z);
                    //Console.WriteLine("Added {0} due to not being in star database", vs.Name);
                }
            }

            return total;
        }

        public class TransFormInfo
        {
            public TransFormInfo(Matrix4d r, float z, int dw, int dh, float zm)
            { resmat = r; znear = z; dwidth = dw; dheight = dh; zoom = zm; }

            public TransFormInfo(Matrix4d r, float z, int dw, int dh, double sq, Vector3 cm)
            { resmat = r; znear = z; dwidth = dw; dheight = dh; sqlylimit = sq; campos = cm; }

            public Matrix4d resmat;
            public float znear;
            public int dwidth;
            public int dheight;

            public float zoom;      // Find Point only
            public double sqlylimit;      //  GetSystemInView only
            public Vector3 campos;//  GetSystemInView only
        }

        public Vector3? FindPoint(int x, int y, ref double cursysdistz, TransFormInfo ti) // UI call .. operate on  display
        {
            Debug.Assert(Application.MessageLoop);

            if (array1displayed)
                return FindPoint(ref array1, array1vertices, x, y, ref cursysdistz, ti);
            else
                return FindPoint(ref array2, array2vertices, x, y, ref cursysdistz, ti);
        }
                                                                                            // operate on  display
        public Vector3? FindPoint(ref Vector3[] vert, int total , int x, int y, ref double cursysdistz, TransFormInfo ti)
        { 
            Vector3? ret = null;
            double w2 = (double)ti.dwidth / 2.0;
            double h2 = (double)ti.dheight / 2.0;

            for ( int i = 0; i < total; i++)
            {
                Vector3 v = vert[i];

                Vector4d syspos = new Vector4d(v.X, v.Y, v.Z, 1.0);
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
                            ret = new Vector3(v.X, v.Y, v.Z);
                        }
                    }
                }
            }

            return ret;
        }

        public class InViewInfo
        {
            public InViewInfo(Vector3 pos, uint c) { position = pos; colour = c; }
            public Color AsColor { get { return Color.FromArgb((int)((colour >> 24) & 0xff), (int)(colour & 0xff), (int)((colour >> 8) & 0xff), (int)((colour >> 16) & 0xff)); } }

            public Vector3 position;
            public uint colour;
        }

        // operate on  display - Called in a thread..
        public void GetSystemsInView(ref SortedDictionary<float, InViewInfo> list , TransFormInfo ti, uint forcecol = 0)
        {
            if (array1displayed)
                GetSystemsInView(ref array1, ref carray1, array1vertices, ref list, ti , forcecol);
            else
                GetSystemsInView(ref array2, ref carray2, array2vertices, ref list, ti , forcecol);
        }

        // operate on  display - can be called BY A THREAD
        public void GetSystemsInView(ref Vector3[] vert, ref uint[] cols , int total, ref SortedDictionary<float, InViewInfo> list , TransFormInfo ti , uint forcecol )
        {
            int margin = -150;
            float sqdist = 0F;
            double w2 = (double)ti.dwidth / 2.0;
            double h2 = (double)ti.dheight / 2.0;

            lock (lockdisplaydata)                                                  // must lock it..
            {
                for (int i = 0; i < total; i++)
                {
                    Vector3 v = vert[i];

                    Vector4d syspos = new Vector4d(v.X, v.Y, v.Z, 1.0);
                    Vector4d sysloc = Vector4d.Transform(syspos, ti.resmat);

                    if (sysloc.Z > ti.znear)
                    {
                        Vector2d syssloc = new Vector2d(((sysloc.X / sysloc.W) + 1.0) * w2, ((sysloc.Y / sysloc.W) + 1.0) * h2);

                        if ((syssloc.X >= margin && syssloc.X <= ti.dwidth - margin) && (syssloc.Y >= margin && syssloc.Y <= ti.dheight - margin))
                        {
                            sqdist = ((float)v.X - ti.campos.X) * ((float)v.X - ti.campos.X) + ((float)v.Y - ti.campos.Y) * ((float)v.Y - ti.campos.Y) + ((float)v.Z - ti.campos.Z) * ((float)v.Z - ti.campos.Z);

                            if (sqdist <= ti.sqlylimit)
                                list.Add(sqdist, new InViewInfo( new Vector3(v.X,v.Y,v.Z),(forcecol!=0) ? forcecol : cols[i]));
                        }
                    }
                }
            }
        }

        public void Dispose()           // throw away any previous buffer..
        {
            DeleteContext();
            array1 = null;
            carray1 = null;
            array2 = null;
            carray2 = null;
        }

        public void DeleteContext()
        {
            if (GLContext != null)
            {
                if (GLContext.InvokeRequired)
                {
                    GLContext.Invoke(new Action(this.DeleteContext));
                }
                else
                {
                    GL.DeleteBuffer(VtxVboID);
                    GL.DeleteBuffer(VtxColorVboId);
                    GLContext = null;
                    VtxVboID = 0;
                    VtxColorVboId = 0;
                }
            }
        }

        public void Display(GLControl control)          // UI thread..
        {
            Debug.Assert(Application.MessageLoop);

            lock ( lockdisplaydata )                                        // not allowed to swap..
            {
                Displayed = true;                                               // true if displayed

                array1displayed = !array1displayed;

                DeleteContext();

                if (array1displayed)
                {
                    if (array1vertices > 0)
                    {
                        GL.GenBuffers(1, out VtxVboID);
                        GL.BindBuffer(BufferTarget.ArrayBuffer, VtxVboID);
                        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(array1vertices * BlittableValueType.StrideOf(array1)), array1, BufferUsageHint.StaticDraw);

                        GL.GenBuffers(1, out VtxColorVboId);
                        GL.BindBuffer(BufferTarget.ArrayBuffer, VtxColorVboId);
                        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(array1vertices * BlittableValueType.StrideOf(carray1)), carray1, BufferUsageHint.StaticDraw);
                        GLContext = control;
                    }

                    carray2 = null;
                    array2 = null;
                    array2vertices = 0;
                }
                else
                {
                    if (array2vertices > 0)
                    {
                        GL.GenBuffers(1, out VtxVboID);
                        GL.BindBuffer(BufferTarget.ArrayBuffer, VtxVboID);

                        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(array2vertices * BlittableValueType.StrideOf(array2)), array2, BufferUsageHint.StaticDraw);

                        GL.GenBuffers(1, out VtxColorVboId);
                        GL.BindBuffer(BufferTarget.ArrayBuffer, VtxColorVboId);
                        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(array2vertices * BlittableValueType.StrideOf(carray2)), carray2, BufferUsageHint.StaticDraw);
                        GLContext = control;
                    }

                    carray1 = null;
                    array1 = null;
                    array1vertices = 0;
                }
            }
        }

        public void Draw(GLControl control)         // UI thread.
        {
            Debug.Assert(Application.MessageLoop);

            if (GLContext != null)
            {
                Debug.Assert(GLContext == control);
                Debug.Assert(VtxVboID > 0);

                if (control.InvokeRequired)
                {
                    control.Invoke(new Action<GLControl>(this.Draw), control);
                }
                else
                {
                    int numpoints = (array1displayed) ? array1vertices : array2vertices;

                    if (numpoints > 0)
                    {
                        GL.EnableClientState(ArrayCap.VertexArray);
                        GL.BindBuffer(BufferTarget.ArrayBuffer, VtxVboID);
                        GL.VertexPointer(3, VertexPointerType.Float, 0, 0);
                        GL.PointSize(Size);

                        if (Color == Color.Transparent)
                        {
                            GL.EnableClientState(ArrayCap.ColorArray);
                            GL.BindBuffer(BufferTarget.ArrayBuffer, VtxColorVboId);
                            GL.ColorPointer(4, ColorPointerType.UnsignedByte, 0, 0);
                            GL.DrawArrays(PrimitiveType.Points, 0, numpoints);
                            GL.DisableClientState(ArrayCap.ColorArray);
                        }
                        else
                        {
                            GL.Color4(Color);
                            GL.DrawArrays(PrimitiveType.Points, 0, numpoints);
                        }

                        GL.DisableClientState(ArrayCap.VertexArray);
                    }
                }
            }
        }
    }

    public class StarGrids
    {
        #region Vars
        public bool ForceWhite { get; set; } = false;

        private BlockingCollection<StarGrid> computed = new BlockingCollection<StarGrid>();
        private List<StarGrid> grids = new List<StarGrid>();        // unpopulated grid stars
        private StarGrid populatedgrid;
        private StarGrid visitedsystemsgrid;
        private System.Threading.Thread computeThread;
        private bool computeExit = false;
        private EventWaitHandle ewh = new EventWaitHandle(false, EventResetMode.AutoReset);
        private double curx = 0, curz = 0;

        private int midpercentage = 80;
        private double middistance = 20000;
        private int farpercentage = 50;
        private double fardistance = 40000;
        private double MinRecalcDistance = 5000;            // only recalc a grid if we are more than this away from its prev calc pos

        private Color popcolour = Color.Blue;

#endregion

#region Initialise

        public void Initialise()
        {
            for (int z = 0; z < GridId.gridzrange; z++)
            {
                for (int x = 0; x < GridId.gridxrange; x++)
                {
                    int id = GridId.IdFromComponents(x, z);
                    double xp = 0, zp = 0;
                    bool ok = GridId.XZ(id, out xp, out zp);
                    Debug.Assert(ok);
                    StarGrid grd = new StarGrid(id, xp, zp, Color.Transparent, 1.0F);           //A=0 means use default colour array
                    if (xp == 0 && zp == 0)                                     // sol grid, unpopulated stars please
                        grd.dBAsk = SystemClass.SystemAskType.UnPopulatedStars;

                    grids.Add(grd);
                }
            }

            visitedsystemsgrid = new StarGrid(-1, 0, 0, Color.Orange, 1.0F);    // grid ID -1 means it won't be filled by the Update task
            grids.Add(visitedsystemsgrid);

            int solid = GridId.Id(0, 0);
            populatedgrid = new StarGrid(solid, 0, 0, Color.Transparent , 1.0F);      // Duplicate grid id but asking for populated stars
            populatedgrid.dBAsk = SystemClass.SystemAskType.PopulatedStars;
            grids.Add(populatedgrid);                                       // add last, so displayed last, so overwrites anything else

            long total = SystemClass.GetTotalSystems();

            total = Math.Min(total, 10000000);                  // scaling limit at 10mil
            long offset = (total - 1000000) / 100000;           // scale down slowly.. experimental!
            midpercentage -= (int)(offset / 2);
            farpercentage -= (int)(offset / 3);

            //midpercentage = 10;           // agressive debugging options
            //farpercentage = 1;

            Console.WriteLine("Grids " + grids.Count + "Database Stars " + total + " mid " + midpercentage + " far " + farpercentage);
        }

        public void Start()
        {
            if (computeThread == null)
            {
                computeThread = new System.Threading.Thread(ComputeThread) { Name = "Fill stars", IsBackground = true };
                computeThread.Start();
            }
        }

        public void Stop()
        {
            if (computeThread!=null && computeThread.IsAlive)
            {
                Console.WriteLine("{0} Ask for compute exit", Environment.TickCount);
                computeExit = true;
                ewh.Set();              // wake it up!
                computeThread.Join();
                computeThread = null;
                computeExit = false;
                Console.WriteLine("{0} compute exit", Environment.TickCount);
            }
        }

        public void FillVisitedSystems(List<VisitedSystemsClass> cls)
        {
            if (cls != null)
            {
                visitedsystemsgrid.FillFromVS(cls);     // recompute it into the other array
                computed.Add(visitedsystemsgrid);       // add to list to display when ready
            }
        }

        public int GetPercentage(double dist)  
        {
            if (dist < middistance)
                return 100;
            else if (dist < fardistance)
                return midpercentage;
            else
                return farpercentage;
        }

        public bool IsDisplayed(double xp, double zp )
        {
            int gridid = GridId.Id(xp, zp);

            StarGrid grid = grids.Find(x => x.Id == gridid);

            if (grid != null)
                return grid.Displayed;
            else
                return false;
        }

#endregion

#region Update

        
        public bool Update(double xp, double zp, float zoom , GLControl gl  )            // Foreground UI thread, tells it if anything has changed..
        {
            Debug.Assert(Application.MessageLoop);

            StarGrid grd = null;

            bool displayed = false;

            while (computed.TryTake(out grd))                               // remove from the computed queue and mark done
            {
                grd.Display(gl);                                            // swap to using this one..
                Thread.MemoryBarrier();                                     // finish above before clearing working
                grd.Working = false;
                displayed = true;
            }

            Thread.MemoryBarrier();                                         // finish above 

            curx = xp;
            curz = zp;

            ewh.Set();                                                      // tick thread again to consider..

            return displayed;                                               // return if we updated anything..
        }

        void ComputeThread()
        {
            //Console.WriteLine("Start COMPUTE");

            while (true)
            {
                ewh.WaitOne();

                while (true)
                {
                    if (computeExit)
                        return;

                    double mindist = double.MaxValue;
                    double maxdist = 0;
                    StarGrid selmin = null;
                    StarGrid selmax = null;

                    foreach (StarGrid gcheck in grids)
                    {
                        if (gcheck.Id >= 0 && !gcheck.Working)                                     // if not a special grid
                        {
                            double dist = gcheck.DistanceFrom(curx, curz);

                            if (Math.Abs(dist - gcheck.CalculatedDistance) > MinRecalcDistance) // if its too small a change, ignore.. histerisis
                            {
                                int percentage = GetPercentage(dist);

                                if (percentage > gcheck.Percentage)                // if increase, it has priority..
                                {
                                    if (dist < mindist)                             // if best.. pick
                                    {
                                        mindist = dist;
                                        selmin = gcheck;
                                        //Console.WriteLine("Select {0} incr perc {1} to {2} dist {3,8:0.0}", gcheck.Id, gcheck.Percentage, percentage, dist);
                                    }
                                }
                                else if (selmin == null && percentage < gcheck.Percentage)   // if not selected a min one, pick the further one to decrease
                                {
                                    if (dist > maxdist)
                                    {
                                        maxdist = dist;
                                        selmax = gcheck;
                                        //Console.WriteLine("Select {0} decr perc {1} to {2} dist {3,8:0.0}", gcheck.Id, gcheck.Percentage, percentage, dist);
                                    }
                                }
                            }
                        }
                    }

                    if (selmin == null)
                        selmin = selmax;

                    if (selmin != null)
                    {
                        selmin.Working = true;                                          // stops another go by this thread, only cleared by UI when it has displayed
                        int prevpercent = selmin.Percentage;
                        double prevdist = selmin.CalculatedDistance;

                        selmin.CalculatedDistance = selmin.DistanceFrom(curx, curz);
                        selmin.Percentage = GetPercentage(selmin.CalculatedDistance);
                        selmin.FillFromDB();

                        Thread.MemoryBarrier();                                         // finish above before trying to trigger another go..

                        Debug.Assert(Math.Abs(prevdist - selmin.CalculatedDistance) > MinRecalcDistance);

                        Debug.Assert(!computed.Contains(selmin));

                        Tools.LogToFile(String.Format("Grid repaint {0} {1}%->{2}% dist {3,8:0.0}->{4,8:0.0} s{5}", selmin.Id, prevpercent, selmin.Percentage, prevdist, selmin.CalculatedDistance, selmin.CountJustMade));

                        computed.Add(selmin);
                    }
                    else
                        break;              // nothing to do, wait for kick
                }
            }
        }

#endregion

#region Draw

        public void DrawAll(GLControl control, bool showstars , bool showstations )
        {
            if (ForceWhite)
                populatedgrid.Color = (showstations) ? popcolour : Color.FromArgb(255, 212, 212, 212);
            else
                populatedgrid.Color = (showstations) ? popcolour : Color.Transparent;

            if (showstars)
            {
                foreach (StarGrid grd in grids)
                {
                    if (grd != populatedgrid && grd != visitedsystemsgrid)
                        grd.Color = (ForceWhite) ? Color.FromArgb(255, 212, 212, 212) : Color.Transparent;

                    grd.Draw(control);              // populated grid is in this list, so will be drawn..
                }
            }
            else if ( showstations )                // if only stations on, draw it specially.
            {
                populatedgrid.Draw(control);
            }
        }

#endregion

#region misc

        public Vector3? FindOverSystem(int x, int y, out double cursysdistz, StarGrid.TransFormInfo ti , 
                                        bool showstars, bool showstations) // UI Call.
        {
            Debug.Assert(Application.MessageLoop);

            cursysdistz = double.MaxValue;
            Vector3? ret = null;

            if (showstars)                        // populated grid is in this list, so will be checked
            {
                foreach (StarGrid grd in grids)
                {
                    Vector3? cur = grd.FindPoint(x, y, ref cursysdistz, ti);
                    if (cur != null)        // if found one, better than cursysdistz, use it
                        ret = cur;
                }
            }
            else if ( showstations )
            {
                ret = populatedgrid.FindPoint(x, y, ref cursysdistz, ti);
            }

            return ret;
        }
                                        // used by Starnames in a thread..
        public void GetSystemsInView(ref SortedDictionary<float, StarGrid.InViewInfo> list, double gridlylimit , StarGrid.TransFormInfo ti)
        {
            int idpos = GridId.Id(ti.campos.X, ti.campos.Z);

            foreach (StarGrid grd in grids)                 // either we are inside the grid, or close to the centre of another grid..
            {
                if (grd.Id==idpos || grd.DistanceFrom(ti.campos.X, ti.campos.Z) < gridlylimit)                         // only consider grids which are nearer than this..
                {
                    grd.GetSystemsInView(ref list,ti, (ForceWhite) ? 0xff00ffff : 0);
                    //Console.WriteLine("Check grid {0} {1} gives {2}" ,grd.X,grd.Z,list.Count);
                }
            }

            visitedsystemsgrid.GetSystemsInView(ref list, ti);          // this can be anywhere in space.. so must check
        }


        public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable      // special compare for sortedlist
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);
                return (result == 0) ? 1 : result;      // for this, equals just means greater than, to allow duplicate distance values to be added.
            }
        }

        private int CountStars()
        {
            int t = 0;
            foreach (StarGrid grd in grids)                 // either we are inside the grid, or close to the centre of another grid..
                t += grd.Count;
            return t;
        }

#endregion
    }


}
