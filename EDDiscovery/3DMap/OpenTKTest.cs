using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2.DB;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EDDiscovery2
{
    class OpenTKTest
    {
        public List<SystemClass> StarList;
        public List<VisitedSystemsClass> visitedSystems=null;
        private Dictionary<string, SystemClass> VisitedStars;

        float x = 0;
        float y = 0;
        //float z = 0;


        public void InitData()
        {
            SystemClass star;

            VisitedStars = new Dictionary<string, SystemClass>();

            if (visitedSystems != null)
            {
                foreach (VisitedSystemsClass sp in visitedSystems)
                {
                    star = SystemData.GetSystem(sp.Name);
                    if (star != null && star.HasCoordinate)
                    {
                        VisitedStars[star.SearchName] = star;
                    }
                }
            }
           
        }

        private void DrawStars()
        {
        if (StarList == null)
                StarList =  SQLiteDBClass.globalSystems;


        if (VisitedStars == null)
            InitData();

            GL.Begin(PrimitiveType.Points);
            GL.Color3(Color.White);
            GL.PointSize(2.0f);

            foreach (SystemClass si in StarList)
            {
                if (si.HasCoordinate)
                {
                  GL.Vertex3(si.x, si.y, si.z);
                }
            }
            GL.End();




            GL.Begin(PrimitiveType.Points);
            GL.Color3(Color.Red);
            GL.PointSize(2.0f);

            if (visitedSystems != null)
            {
                SystemClass star;

                foreach (SystemClass sp in VisitedStars.Values)
                {
                    star = sp;
                    if (star != null)
                    {
                        GL.Vertex3(star.x, star.y, star.z);
                    }
                }
            }
            GL.End();
        }


        static int framenr = 0;
        public void Main()
        {
            using (var game = new GameWindow())
            {
                game.Load += (sender, e) =>
                {
                    // setup settings, load textures, sounds
                    game.VSync = VSyncMode.On;
                };

                game.Resize += (sender, e) =>
                {
                    GL.Viewport(0, 0, game.Width, game.Height);
                };

                game.UpdateFrame += (sender, e) =>
                {
                    // add game logic, input handling
                    if (game.Keyboard[Key.Escape])
                    {
                        game.Exit();
                    }
                };

                game.RenderFrame += (sender, e) =>
                {
                    // render graphics
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.Ortho(-1000.0, 1000.0, -1000.0, 1000.0, 0.0, 2000.0);

                    GL.Translate(x, y, 0); // position triangle according to our x variable

                    //GL.Color3(Color.MidnightBlue);
                    //GL.Vertex2(-1.0f, 1.0f);
                    //GL.Color3(Color.SpringGreen);
                    //GL.Vertex2(0.0f, -1.0f);
                    //GL.Color3(Color.Ivory);
                    //GL.Vertex2(1.0f, 1.0f);


                    framenr++;
                    System.Diagnostics.Trace.WriteLine("RenderFrame " + framenr.ToString());
                    DrawStars();

                    game.SwapBuffers();
                };

                // Run the game at 30 updates per second
                game.Run(30.0);
            }
        }
    }
}
