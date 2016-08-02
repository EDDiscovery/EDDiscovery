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


namespace EDDiscovery2
{
    public class StarGrid
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Z { get; set; }
        public int Percentage { get; set; }          // foreground flags
        public SystemClass.SystemAskType dBAsk { get; set; } // set for an explicit ask for unpopulated systems
        public int Count { get { return array1displayed ? array1vertices : array2vertices; } }
        public int CountJustMade { get { return array1displayed ? array2vertices : array1vertices; } }
        public bool Working { get; set; }

        bool array1displayed;
        private Vector3d[] array1;            // the star points
        int array1vertices;
        private Vector3d[] array2;            // the star points
        int array2vertices;

        public float Size { get; set; }
        public Color Color { get; set; }

        protected int VtxVboID;
        protected GLControl GLContext;

        public StarGrid(int id, double x, double z, Color c, float s)
        {
            Id = id; X = x; Z = z;
            Percentage = 0;
            Color = c;
            Size = s;
            array1vertices = 0;
            array2vertices = 0;
            array1displayed = true;
        }

        public double DistanceFrom(double x, double z)
        { return Math.Sqrt((x - X) * (x - X) + (z - Z) * (z - Z)); }

        public void FillFromDB()
        {
            if (array1displayed)
                array2vertices = SystemClass.GetSystemVector(Id, ref array2, dBAsk, Percentage);
            else
                array1vertices = SystemClass.GetSystemVector(Id, ref array1, dBAsk, Percentage);
        }

        public void FillFromVS(List<VisitedSystemsClass> cls)
        {
            if (array1displayed)
                array2vertices = FillFromVS(ref array2, cls);
            else
                array1vertices = FillFromVS(ref array1, cls);
        }

        private int FillFromVS( ref Vector3d[] array, List<VisitedSystemsClass> cls)
        {
            array = new Vector3d[cls.Count];     // can't have any more than this 
            int total = 0;

            foreach (VisitedSystemsClass vs in cls)
            {                                                               // all vs stars which are not in edsm and have co-ords.
                if (vs.curSystem != null && vs.curSystem.status != SystemStatusEnum.EDSC && vs.curSystem.HasCoordinate )
                {
                    array[total++] = new Vector3d(vs.curSystem.x, vs.curSystem.y, vs.curSystem.z);
                }
            }

            return total;
        }

        public Vector3d? FindPoint(int x, int y, ref double cursysdistz, Matrix4d resmat, float znear, float zoom, double vwidth, double vheight)
        {
            if (array1displayed)
                return FindPoint(array1, array1vertices, x, y, ref cursysdistz, resmat, znear, zoom, vwidth, vheight);
            else
                return FindPoint(array2, array2vertices, x, y, ref cursysdistz, resmat, znear, zoom, vwidth, vheight);
        }

        public Vector3d? FindPoint(Vector3d[] vert, int total , int x, int y, ref double cursysdistz, Matrix4d resmat, float znear, float zoom, double vwidth, double vheight)
        { 
            Vector3d? ret = null;

            for( int i = 0; i < total; i++)
            {
                Vector3d v = vert[i];

                Vector4d syspos = new Vector4d(v.X, v.Y, v.Z, 1.0);
                Vector4d sysloc = Vector4d.Transform(syspos, resmat);

                if (sysloc.Z > znear)
                {
                    Vector2d syssloc = new Vector2d(((sysloc.X / sysloc.W) + 1.0) * vwidth - x, ((sysloc.Y / sysloc.W) + 1.0) * vheight - y);
                    double sysdistsq = syssloc.X * syssloc.X + syssloc.Y * syssloc.Y;

                    if (sysdistsq < 7.0 * 7.0)
                    {
                        double sysdist = Math.Sqrt(sysdistsq);

                        if ((sysdist + Math.Abs(sysloc.Z * zoom)) < cursysdistz)
                        {
                            cursysdistz = sysdist + Math.Abs(sysloc.Z * zoom);
                            ret = new Vector3d(v.X, v.Y, v.Z);
                        }
                    }
                }
            }

            return ret;
        }

        public void Dispose()           // throw away any previous buffer..
        {
            DeleteContext();
            array1 = null;
            array2 = null;
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
                    GLContext = null;
                    VtxVboID = 0;
                }
            }
        }

        public void Display(GLControl control)
        {
            array1displayed = !array1displayed;

            DeleteContext();

            if ( array1displayed )
            {
                if (array1vertices > 0)
                {
                    GL.GenBuffers(1, out VtxVboID);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, VtxVboID);
                    GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(array1vertices * BlittableValueType.StrideOf(array1)), array1, BufferUsageHint.StaticDraw);
                    GLContext = control;
                }

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
                    GLContext = control;
                }

                array1 = null;
                array1vertices = 0;
            }
        }

        public void Draw(GLControl control)
        {
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
                    GL.EnableClientState(ArrayCap.VertexArray);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, VtxVboID);
                    GL.VertexPointer(3, VertexPointerType.Double, 0, 0);
                    GL.PointSize(Size);
                    GL.Color3(Color);
                    GL.DrawArrays(PrimitiveType.Points, 0, numpoints);
                    GL.DisableClientState(ArrayCap.VertexArray);
                    //Console.WriteLine("Draw " + Id + " " + numpoints);
                }
            }
        }
    }

    public class StarGrids
    {
#region Vars
        private BlockingCollection<StarGrid> tocompute = new BlockingCollection<StarGrid>();
        private BlockingCollection<StarGrid> computed = new BlockingCollection<StarGrid>();
        private List<StarGrid> grids = new List<StarGrid>();        // unpopulated grid stars
        private StarGrid populatedgrid;
        private StarGrid visitedsystemsgrid;
        private System.Threading.Thread computeThread;
        private bool computeExit = false;

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
                    StarGrid grd = new StarGrid(id, xp, zp, Color.White, 1.0F);
                    if (xp == 0 && zp == 0)                                     // sol grid, unpopulated stars please
                        grd.dBAsk = SystemClass.SystemAskType.UnPopulatedStars;

                    grids.Add(grd);
                }
            }

            visitedsystemsgrid = new StarGrid(-1, 0, 0, Color.Orange, 1.0F);    // grid ID -1 means it won't be filled by the Update task
            grids.Add(visitedsystemsgrid);

            int solid = GridId.Id(0, 0);
            populatedgrid = new StarGrid(solid, 0, 0, Color.Blue, 1.0F);      // Duplicate grid id but asking for populated stars
            populatedgrid.dBAsk = SystemClass.SystemAskType.PopulatedStars;
            grids.Add(populatedgrid);                                       // add last, so displayed last, so overwrites anything else

            Console.WriteLine("Total grids " + grids.Count);

            computeThread = new System.Threading.Thread(ComputeThread) { Name = "Fill stars", IsBackground = true };
            computeThread.Start();
        }

        public void ShutDown()
        {
            if (computeThread.IsAlive)
            {
                computeExit = true;
                tocompute.Add(null);                                 // add to the compute list.. null is a marker saying shut down
                computeThread.Join();
            }

            foreach (StarGrid grd in grids)
                grd.Dispose();
        }

        public void FillVisitedSystems(List<VisitedSystemsClass> cls)
        {
            if (cls != null)
            {
                visitedsystemsgrid.FillFromVS(cls);     // recompute it into the other array
                computed.Add(visitedsystemsgrid);       // add to list to display when ready
            }
        }

        #endregion

        #region Update

        public void Update(double xp, double zp, GLControl gl )            // Foreground UI thread
        {
            {
                StarGrid grd = null;
                while (computed.TryTake(out grd))               // remove from the computed queue and mark done
                {
                    grd.Working = false;
                    grd.Display(gl);                            // swap to using this one..
                }
            }

            SortedList<double, StarGrid> toupdate = new SortedList<double, StarGrid>(new DuplicateKeyComparer<double>());     // we sort in distance order

            foreach (StarGrid gcheck in grids)
            {
                if (gcheck.Working == false && gcheck.Id >= 0)         // if not in the tocompute queue.. and not a special grid
                {
                    double dist = gcheck.DistanceFrom(xp, zp);
                    int percentage = GetPercentage(dist);

                    if (gcheck.Percentage != percentage)                // if different, compute.  Initially percentage will be zero so make it compute
                    {
                        gcheck.Percentage = percentage;
                        toupdate.Add(dist, gcheck);                     // add to sorted list
                    }
                }
            }

            foreach (StarGrid gsubmit in toupdate.Values)              // and we submit to the queue in distance order, so we build outwards
            {
                gsubmit.Working = true;                                 // working..
                tocompute.Add(gsubmit);                                 // add to the compute list..
                //Console.WriteLine("Compute " + gsubmit.Id + " at " + gsubmit.X + " , " + gsubmit.Z + " %" + gsubmit.Percentage);
            }
        }

        public int GetPercentage(double dist)
        {
            if (dist < 10000)
                return 100;
            if (dist < 30000)
                return 50;
            return 10;
        }

#endregion

#region Compute

        void ComputeThread()
        {
            while (true)
            {
                StarGrid grd = tocompute.Take();

                if (grd == null || computeExit == true)
                    break;

                grd.FillFromDB();

                Console.WriteLine("Computed " + grd.Id.ToString("0000") + "  total " + grd.CountJustMade.ToString("00000") + " at " + grd.X + " , " + grd.Z + " % " + grd.Percentage);
                computed.Add(grd);
            }
        }

        #endregion

        #region Draw

        public void DrawAll(GLControl control, bool showstars , bool showstations )
        {
            populatedgrid.Color = (showstations) ? Color.Blue : Color.White;

            if (showstars)
            {
                foreach (StarGrid grd in grids)
                {
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

        public Vector3d? FindOverSystem(int x, int y, out double cursysdistz, Matrix4d resmat , float znear , float zoom , double vwidth , double vheight )
        {
            cursysdistz = double.MaxValue;
            Vector3d? ret = null;

            foreach (StarGrid grd in grids)
            {
                Vector3d? cur = grd.FindPoint(x, y, ref cursysdistz, resmat, znear, zoom, vwidth, vheight);
                if (cur != null)        // if found one, better than cursysdistz, use it
                    ret = cur;
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

#endregion
    }


}
