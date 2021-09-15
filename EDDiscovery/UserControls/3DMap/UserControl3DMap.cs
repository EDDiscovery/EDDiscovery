/*
 * Copyright © 2021 - 2021 EDDiscovery development team
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

using EliteDangerousCore;
using GLOFC;
using GLOFC.Controller;
using GLOFC.GL4;
using GLOFC.WinForm;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControl3DMap : UserControlCommonBase
    {
        private GLWinFormControl glwfc;
        private Controller3D gl3dcontroller;
        private Timer systemtimer = new Timer();

        private GLRenderProgramSortedList rObjects = new GLRenderProgramSortedList();
        private GLItemsList items = new GLItemsList();

        public UserControl3DMap() 
        {
            InitializeComponent();
        }

        public override void Init()
        {
            BaseUtils.Translator.Instance.Translate(this);      // translate before we add anything else to the panel

            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;

            glwfc = new GLOFC.WinForm.GLWinFormControl(panelOuter);
            glwfc.EnsureCurrentPaintResize = true;      // set, ensures context is set up for internal code on paint and any Paints chained to it
        }

        public override void LoadLayout()
        {
            base.LoadLayout();

            glwfc.EnsureCurrentContext();

            gl3dcontroller = new Controller3D();
            gl3dcontroller.PaintObjects = ControllerDraw;       // hook paint objects, itself called by glwfc paint, to controller draw
            gl3dcontroller.MatrixCalc.PerspectiveNearZDistance = 1f;
            gl3dcontroller.MatrixCalc.PerspectiveFarZDistance = 1000f;
            gl3dcontroller.ZoomDistance = 20F;
            gl3dcontroller.Start(glwfc, new Vector3(0, 0, 0), new Vector3(110f, 0, 0f), 1F);

            gl3dcontroller.KeyboardTravelSpeed = (ms, eyedist) =>
            {
                return (float)ms / 100.0f;
            };

            items.Add(new GLColorShaderWithWorldCoord(), "COSW");
            items.Add(new GLColorShaderWithObjectTranslation(), "COSOT");
            items.Add(new GLTexturedShaderWithObjectTranslation(), "TEXOT");

            items.Add(new GLTexture2D(global::EDDiscovery.Icons.Controls.Map3D_Bookmarks_ShowBookmarks as Bitmap, SizedInternalFormat.Rgba8), "ica");
            items.Add(new GLTexture2D(global::EDDiscovery.Icons.Controls.Map3D_Bookmarks_Menu as Bitmap, SizedInternalFormat.Rgba8), "icb");

            #region coloured lines

            if (true)
            {
                GLRenderState lines = GLRenderState.Lines(1);

                rObjects.Add(items.Shader("COSW"),
                             GLRenderableItem.CreateVector4Color4(items, PrimitiveType.Lines, lines,
                                                        GLShapeObjectFactory.CreateLines(new Vector3(-100, -0, -100), new Vector3(-100, -0, 100), new Vector3(10, 0, 0), 21),
                                                        new Color4[] { Color.Red, Color.Red, Color.DarkRed, Color.DarkRed })
                                   );


                rObjects.Add(items.Shader("COSW"),
                             GLRenderableItem.CreateVector4Color4(items, PrimitiveType.Lines, lines,
                                   GLShapeObjectFactory.CreateLines(new Vector3(-100, -0, -100), new Vector3(100, -0, -100), new Vector3(0, 0, 10), 21),
                                                        new Color4[] { Color.Red, Color.Red, Color.DarkRed, Color.DarkRed }));
            }
            if (true)
            {
                GLRenderState lines = GLRenderState.Lines(1);

                rObjects.Add(items.Shader("COSW"),
                             GLRenderableItem.CreateVector4Color4(items, PrimitiveType.Lines, lines,
                                   GLShapeObjectFactory.CreateLines(new Vector3(-100, 10, -100), new Vector3(-100, 10, 100), new Vector3(10, 0, 0), 21),
                                                             new Color4[] { Color.Yellow, Color.Orange, Color.Yellow, Color.Orange })
                                   );

                rObjects.Add(items.Shader("COSW"),
                             GLRenderableItem.CreateVector4Color4(items, PrimitiveType.Lines, lines,
                                   GLShapeObjectFactory.CreateLines(new Vector3(-100, 10, -100), new Vector3(100, 10, -100), new Vector3(0, 0, 10), 21),
                                                             new Color4[] { Color.Yellow, Color.Orange, Color.Yellow, Color.Orange })
                                   );
            }

            #endregion

            #region Coloured triangles
            if (true)
            {
                GLRenderState rc = GLRenderState.Tri();
                rc.CullFace = false;

                rObjects.Add(items.Shader("COSOT"), "scopen",
                            GLRenderableItem.CreateVector4Color4(items, PrimitiveType.Triangles, rc,
                                            GLCubeObjectFactory.CreateSolidCubeFromTriangles(5f),
                                            new Color4[] { Color4.Red, Color4.Green, Color4.Blue, Color4.White, Color4.Cyan, Color4.Orange },
                                            new GLRenderDataTranslationRotation(new Vector3(10, 3, 20))
                            ));

                rObjects.Add(items.Shader("COSOT"), "scopen2",
                            GLRenderableItem.CreateVector4Color4(items, PrimitiveType.Triangles, rc,
                                            GLCubeObjectFactory.CreateSolidCubeFromTriangles(5f),
                                            new Color4[] { Color4.Red, Color4.Red, Color4.Red, Color4.Red, Color4.Red, Color4.Red },
                                            new GLRenderDataTranslationRotation(new Vector3(-10, -3, -20))
                            ));
            }

            #endregion
            #region textures
            if (true)
            {
                GLRenderState rq = GLRenderState.Quads();

                rObjects.Add(items.Shader("TEXOT"),
                            GLRenderableItem.CreateVector4Vector2(items, PrimitiveType.Quads, rq,
                            GLShapeObjectFactory.CreateQuad(1.0f, 1.0f, new Vector3(-90f.Radians(), 0, 0)), GLShapeObjectFactory.TexQuad,
                            new GLRenderDataTranslationRotationTexture(items.Tex("ica"), new Vector3(0, 0, 0))
                            ));

                rObjects.Add(items.Shader("TEXOT"),
                    GLRenderableItem.CreateVector4Vector2(items, PrimitiveType.Quads, rq,
                            GLShapeObjectFactory.CreateQuad(1.0f, 1.0f, new Vector3(0, 0, 0)), GLShapeObjectFactory.TexQuad,
                            new GLRenderDataTranslationRotationTexture(items.Tex("icb"), new Vector3(4, 0, 0))
                            ));

            }

            #endregion


            #region Matrix Calc Uniform

            items.Add(new GLMatrixCalcUniformBlock(), "MCUB");     // def binding of 0

            #endregion

            // start clock
            systemtimer.Interval = 500;
            systemtimer.Tick += new EventHandler(SystemTick);
            systemtimer.Start();

        }

        public override void Closing()
        {
            System.Diagnostics.Debug.WriteLine($"3dmap {displaynumber} stop");
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            systemtimer.Stop();

            glwfc.EnsureCurrentContext();           // must make sure current context before we call all the dispose functions
            items.Dispose();                        // dispose of data
        }

        // called by GLControl_Paint->Controller 3d paint, context is ensured
        private void ControllerDraw(Controller3D mc, ulong unused)
        {
            System.Diagnostics.Debug.WriteLine($"3dmap {displaynumber} draw");

            GLMatrixCalcUniformBlock mcub = (GLMatrixCalcUniformBlock)items.UB("MCUB");
            mcub.SetText(gl3dcontroller.MatrixCalc);

            rObjects.Render(glwfc.RenderState, gl3dcontroller.MatrixCalc);

            var azel = gl3dcontroller.PosCamera.EyePosition.AzEl(gl3dcontroller.PosCamera.Lookat, true);

            this.Text = "Looking at " + gl3dcontroller.MatrixCalc.TargetPosition + " from " + gl3dcontroller.MatrixCalc.EyePosition + " cdir " + gl3dcontroller.PosCamera.CameraDirection + " azel " + azel + " zoom " + gl3dcontroller.PosCamera.ZoomFactor + " dist " + gl3dcontroller.MatrixCalc.EyeDistance + " FOV " + gl3dcontroller.MatrixCalc.FovDeg;
            System.Diagnostics.Debug.WriteLine($"3dmap {displaynumber} draw over");
        }

        // Context is not set, but should not be needed to be set unless the keyboard/mouse stuff does some GL commands
        private void SystemTick(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"3dmap {displaynumber} tick");
            gl3dcontroller.HandleKeyboardSlews(true);       // nothing here calls GL control, 
            gl3dcontroller.RecalcMatrixIfMoved();
            gl3dcontroller.Redraw();        // invalidate it to redraw, but if its not the current tab, it won't draw.
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            if (he.IsFSDCarrierJump)
            {
            }
        }

        private void Discoveryform_OnHistoryChange(HistoryList obj)
        {
        }


    }
}
