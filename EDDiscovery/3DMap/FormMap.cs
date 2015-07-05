using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2._3DMap;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery2
{
        public partial class FormMap : Form
        {
            bool loaded = false;

            float x = 0;
            float y = 0;
            float z = 0;
            float zoom = 1;
            float xang = 0;
            float yang = 0;

            private Point MouseStartRotate;
            private Point MouseStartTranslate;
            private bool bMouseDown = false;
            private SystemClass CenterSystem;

            public List<SystemClass> StarList;
            public List<SystemPosition> visitedSystems;
            private Dictionary<string, SystemClass> VisitedStars;

            private List<Data3DSetClass> datasets;




            public FormMap()
            {
                InitializeComponent();
            }

            /// <summary>
            /// Loads Control
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void glControl1_Load(object sender, EventArgs e)
            {
                loaded = true;
                GL.ClearColor(Color.Black); // Yey! .NET Colors can be used directly!

                SetupViewport();
                
            }

            /// <summary>
            /// Setup the Viewport
            /// </summary>
            private void SetupViewport()
            {

                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();            

                int w = glControl1.Width;
                int h = glControl1.Height;

                float orthoW = w * (zoom + 1);
                float orthoH = h * (zoom + 1);


                int orthoheight = 1000 * h / w;

                GL.Ortho(-1000.0, 1000.0, -orthoheight, orthoheight, -5000.0, 5000.0);
                //GL.Ortho(0, orthoW, 0, orthoH, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
                GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
            }


            public void InitData()
            {
                SystemClass star;

                VisitedStars = new Dictionary<string, SystemClass>();

                if (visitedSystems != null)
                {
                    foreach (SystemPosition sp in visitedSystems)
                    {
                        star = SystemData.GetSystem(sp.Name);
                        if (star != null && star.HasCoordinate)
                        {
                            VisitedStars[star.SearchName] = star;
                        }
                    }
                }

            }


            private void GenerateDataSets()
            {
                Data3DSetClass dataset;
                InitGenerateDataSet();

                datasets = new List<Data3DSetClass>();

                dataset = new Data3DSetClass("stars", PrimitiveType.Points, Color.White, 1.0f);
                foreach (SystemClass si in StarList)
                {
                    if (si.HasCoordinate)
                    {
                        dataset.AddPoint(si.x - CenterSystem.x, si.y - CenterSystem.y, CenterSystem.z - si.z);
                    }
                }
                datasets.Add(dataset);


                dataset = new Data3DSetClass("visitedstars", PrimitiveType.Points, Color.Red, 2.0f);

                if (visitedSystems != null)
                {
                    SystemClass star;
                    foreach (SystemClass sp in VisitedStars.Values)
                    {
                        star = sp;
                        if (star != null)
                        {
                            dataset.AddPoint(star.x - CenterSystem.x, star.y - CenterSystem.y, CenterSystem.z - star.z);
                        }
                    }
                }
                datasets.Add(dataset);


                dataset = new Data3DSetClass("Center", PrimitiveType.Points, Color.Yellow, 5.0f);

                //GL.Enable(EnableCap.ProgramPointSize);
                dataset.AddPoint(0, 0, 0);
                datasets.Add(dataset);
            }


            private void GenerateDataSetsAllegiance()
            {

                Dictionary<int, Data3DSetClass> datadict = new Dictionary<int, Data3DSetClass>();

                InitGenerateDataSet();

                datasets = new List<Data3DSetClass>();

                datadict[(int)EDAllegiance.Alliance] = new Data3DSetClass(EDAllegiance.Alliance.ToString(), PrimitiveType.Points, Color.Green, 1.0f);
                datadict[(int)EDAllegiance.Anarchy] = new Data3DSetClass(EDAllegiance.Anarchy.ToString(), PrimitiveType.Points, Color.Purple, 1.0f);
                datadict[(int)EDAllegiance.Empire] = new Data3DSetClass(EDAllegiance.Empire.ToString(), PrimitiveType.Points, Color.Blue, 1.0f);
                datadict[(int)EDAllegiance.Federation] = new Data3DSetClass(EDAllegiance.Federation.ToString(), PrimitiveType.Points, Color.Red, 1.0f);
                datadict[(int)EDAllegiance.Independent] = new Data3DSetClass(EDAllegiance.Independent.ToString(), PrimitiveType.Points, Color.Yellow, 1.0f);
                datadict[(int)EDAllegiance.None] = new Data3DSetClass(EDAllegiance.None.ToString(), PrimitiveType.Points, Color.LightGray, 1.0f);
                datadict[(int)EDAllegiance.Unknown] = new Data3DSetClass(EDAllegiance.Unknown.ToString(), PrimitiveType.Points, Color.DarkGray , 1.0f);

                foreach (SystemClass si in StarList)
                {
                    if (si.HasCoordinate)
                    {
                        datadict[(int)si.allegiance].AddPoint(si.x - CenterSystem.x, si.y - CenterSystem.y, CenterSystem.z - si.z);
                    }
                }

                foreach (var ds in datadict.Values)
                    datasets.Add(ds);

                datadict[(int)EDAllegiance.None].Visible = false;
                datadict[(int)EDAllegiance.Unknown].Visible = false;

            }


            private void GenerateDataSetsGovernment()
            {

                Dictionary<int, Data3DSetClass> datadict = new Dictionary<int, Data3DSetClass>();

                InitGenerateDataSet();

                datasets = new List<Data3DSetClass>();

                datadict[(int)EDGovernment.Anarchy] = new Data3DSetClass(EDGovernment.Anarchy.ToString(), PrimitiveType.Points, Color.Yellow, 1.0f);
                datadict[(int)EDGovernment.Colony] = new Data3DSetClass(EDGovernment.Colony.ToString(), PrimitiveType.Points, Color.YellowGreen, 1.0f);
                datadict[(int)EDGovernment.Democracy] = new Data3DSetClass(EDGovernment.Democracy.ToString(), PrimitiveType.Points, Color.Green, 1.0f);
                datadict[(int)EDGovernment.Imperial] = new Data3DSetClass(EDGovernment.Imperial.ToString(), PrimitiveType.Points, Color.DarkGreen, 1.0f);
                datadict[(int)EDGovernment.Corporate] = new Data3DSetClass(EDGovernment.Corporate.ToString(), PrimitiveType.Points, Color.LawnGreen, 1.0f);
                datadict[(int)EDGovernment.Communism] = new Data3DSetClass(EDGovernment.Communism.ToString(), PrimitiveType.Points, Color.DarkOliveGreen, 1.0f);
                datadict[(int)EDGovernment.Feudal] = new Data3DSetClass(EDGovernment.Feudal.ToString(), PrimitiveType.Points, Color.LightBlue, 1.0f);
                datadict[(int)EDGovernment.Dictatorship] = new Data3DSetClass(EDGovernment.Dictatorship.ToString(), PrimitiveType.Points, Color.Blue, 1.0f);
                datadict[(int)EDGovernment.Theocracy] = new Data3DSetClass(EDGovernment.Theocracy.ToString(), PrimitiveType.Points, Color.DarkBlue, 1.0f);
                datadict[(int)EDGovernment.Cooperative] = new Data3DSetClass(EDGovernment.Cooperative.ToString(), PrimitiveType.Points, Color.Purple, 1.0f);
                datadict[(int)EDGovernment.Patronage] = new Data3DSetClass(EDGovernment.Patronage.ToString(), PrimitiveType.Points, Color.LightCyan, 1.0f);
                datadict[(int)EDGovernment.Confederacy] = new Data3DSetClass(EDGovernment.Confederacy.ToString(), PrimitiveType.Points, Color.Red, 1.0f);
                datadict[(int)EDGovernment.Prison_Colony] = new Data3DSetClass(EDGovernment.Prison_Colony.ToString(), PrimitiveType.Points, Color.Orange, 1.0f);
                datadict[(int)EDGovernment.None] = new Data3DSetClass(EDGovernment.None.ToString(), PrimitiveType.Points, Color.Gray, 1.0f);
                datadict[(int)EDGovernment.Unknown] = new Data3DSetClass(EDGovernment.Unknown.ToString(), PrimitiveType.Points, Color.DarkGray, 1.0f);

                foreach (SystemClass si in StarList)
                {
                    if (si.HasCoordinate)
                    {
                        datadict[(int)si.primary_economy].AddPoint(si.x - CenterSystem.x, si.y - CenterSystem.y, CenterSystem.z - si.z);
                    }
                }

                foreach (var ds in datadict.Values)
                    datasets.Add(ds);

                datadict[(int)EDGovernment.None].Visible = false;
                datadict[(int)EDGovernment.Unknown].Visible = false;

            }



            private void InitGenerateDataSet()
            {

                if (StarList == null)
                    StarList = SQLiteDBClass.globalSystems;


                if (VisitedStars == null)
                    InitData();
            }

            private void DrawStars()
            {
                foreach (var dataset in datasets)
                {
                    dataset.DrawPoints();
                }
            }

            /*
            private void DrawStars()
            {
                if (StarList == null)
                    StarList = SQLiteDBClass.globalSystems;


                if (VisitedStars == null)
                    InitData();

                GL.PointSize(1.0f);

                GL.Begin(PrimitiveType.Points);
                GL.Color3(Color.White);

                foreach (SystemClass si in StarList)
                {
                    if (si.HasCoordinate)
                    {
                        GL.Vertex3(si.x-CenterSystem.x, si.y-CenterSystem.y, CenterSystem.z-si.z);
                    }
                }
                GL.End();



                GL.PointSize(2.0f);
                GL.Begin(PrimitiveType.Points);
                GL.Color3(Color.Red);
               

                if (visitedSystems != null)
                {
                    SystemClass star;

                    foreach (SystemClass sp in VisitedStars.Values)
                    {
                        star = sp;
                        if (star != null)
                        {
                            GL.Vertex3(star.x - CenterSystem.x, star.y - CenterSystem.y, CenterSystem.z- star.z);
                        }
                    }
                }
                GL.End();

                GL.PointSize(5.0f);
                GL.Begin(PrimitiveType.Points);
                GL.Enable(EnableCap.ProgramPointSize);
                GL.Color3(Color.Yellow);
                GL.Vertex3(0,0,0);
                GL.End();
            }

            */

            /// <summary>
            /// Calculate Translation of  (X, Y, Z) - according to mouse input
            /// </summary>
            private void SetupCursorXYZ()
            {
//                x =   (PointToClient(Cursor.Position).X - glControl1.Width/2 ) * (zoom + 1);
//                y = (-PointToClient(Cursor.Position).Y + glControl1.Height/2) * (zoom + 1);

//                System.Diagnostics.Trace.WriteLine("X:" + x + " Y:" + y + " Z:" + zoom);
            }


            /// <summary>
            /// We need to setup each time our viewport and Ortho.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void glControl1_Resize(object sender, EventArgs e)
            {
                if (!loaded)
                    return;

                SetupViewport();
            }

            int nr = 0;

            /// <summary>
            /// Paint The control.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void glControl1_Paint(object sender, PaintEventArgs e)
            {
                if (!loaded) // Play nice
                    return;

                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();


                nr++;
                GL.Translate(x, y, 0); // position triangle according to our x variable
                GL.Rotate(xang,  0, 1, 0);
                GL.Rotate(yang, 1, 0, 0);
                GL.Scale(zoom, zoom, zoom);
//                GL.Enable(EnableCap.PointSmooth);
//                GL.Enable(EnableCap.Blend);


                GL.Enable(EnableCap.PointSmooth);
                GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);


                //GL.Color3(Color.Yellow);
                //GL.Begin(BeginMode.Triangles);
                //GL.Vertex2(10, 20);
                //GL.Vertex2(100, 20);
                //GL.Vertex2(100, 50);
                //GL.End();

                DrawStars();

                glControl1.SwapBuffers();

            }



            /// <summary>
            /// The triangle will always move with the cursor. 
            /// But is is not a problem to make it only if mousebutton pressed. 
            /// And do some simple ath with old Translation and new translation.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void glControl1_MouseMove(object sender, MouseEventArgs e)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    int dx = e.X - MouseStartRotate.X;
                    int dy = e.Y - MouseStartRotate.Y;

                    MouseStartRotate.X = e.X;
                    MouseStartRotate.Y = e.Y;
                    System.Diagnostics.Trace.WriteLine("dx" + dx.ToString() + " dy " + dy.ToString() + " Button " + e.Button.ToString());


                    xang+= (float)(dx /5.0);
                    yang+= (float)(-dy / 5.0);

                    SetupCursorXYZ();

                    glControl1.Invalidate();
                }
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    int dx = e.X - MouseStartTranslate.X;
                    int dy = e.Y - MouseStartTranslate.Y;

                    MouseStartTranslate.X = e.X;
                    MouseStartTranslate.Y = e.Y;
                    System.Diagnostics.Trace.WriteLine("dx" + dx.ToString() + " dy " + dy.ToString() + " Button " + e.Button.ToString());


                    x += dx * zoom;
                    y += -dy * zoom;

                    SetupCursorXYZ();

                    glControl1.Invalidate();
                }



            }


            private void OnMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
            {
                if (e.Delta > 0 && zoom < 10) zoom *= 1.1f;
                if (e.Delta < 0 && zoom > 0.01) zoom /= 1.1f;

                SetupCursorXYZ();

                SetupViewport();
                glControl1.Invalidate();
            }

            private void glControl1_MouseDown(object sender, MouseEventArgs e)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    MouseStartRotate.X = e.X;
                    MouseStartRotate.Y = e.Y;
                }

                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    MouseStartTranslate.X = e.X;
                    MouseStartTranslate.Y = e.Y;
                }
            
            }

            private void glControl1_MouseUp(object sender, MouseEventArgs e)
            {
                bMouseDown = false;
            }

            private void FormMap_Load(object sender, EventArgs e)
            {
                textBox_From.AutoCompleteCustomSource = EDDiscoveryForm.SystemNames;
                ShowCenterSystem();
                //GenerateDataSets();
                //GenerateDataSetsAllegiance();
                GenerateDataSetsGovernment();
            }


            
            private void buttonCenter_Click(object sender, EventArgs e)
            {
                if (CenterSystem == null)
                    CenterSystem = SystemData.GetSystem("sol");
                SystemClass sys = SystemData.GetSystem(textBox_From.Text);

                if (sys != null)
                {
                    CenterSystem = sys;
                    ShowCenterSystem();
                }
                else
                {
                    ShowCenterSystem();
                }

                glControl1.Invalidate();
            }

            private void ShowCenterSystem()
            {
                if (CenterSystem == null)
                    CenterSystem = SystemData.GetSystem("sol");

                if (CenterSystem == null)
                {
                    CenterSystem = new SystemClass();
                    CenterSystem.name = "Sol";
                    CenterSystem.SearchName = "sol";
                    CenterSystem.x = 0;
                    CenterSystem.y = 0;
                    CenterSystem.z = 0;

                }

                label1.Text = CenterSystem.name + " x:" + CenterSystem.x.ToString("0.00") + " y:" + CenterSystem.y.ToString("0.00") + " z:" + CenterSystem.z.ToString("0.00");
            }
        }
    }

