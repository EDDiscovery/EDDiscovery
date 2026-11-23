/*
 * Copyright 2019-2023 Robbyxp1 @ github.com
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
 */

using EliteDangerousCore;
using GLOFC;
using GLOFC.Controller;
using GLOFC.GL4.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls.Map3D
{
    public partial class Map
    {
        #region UI

        private void MouseClickOnMap(GLBaseControl s, GLMouseEventArgs e)
        {
            int distmovedsq = gl3dcontroller.MouseMovedSq(e);        //3dcontroller is monitoring mouse movements
            if (distmovedsq < 4)
            {
                //  System.Diagnostics.Debug.WriteLine("map click");
                Object item = FindObjectOnMap(e.ViewportLocation);

                if (item != null)
                {
                    if (e.Button == GLMouseEventArgs.MouseButtons.Left)
                    {
                        if (item is HistoryEntry)
                            travelpath.SetSystem((item as HistoryEntry).System);
                        var nl = NameLocationDescription(item, null);
                        System.Diagnostics.Debug.WriteLine("Click on and slew to " + nl.DescriptiveName);
                        SetEntryText(nl.DescriptiveName);
                        gl3dcontroller.SlewToPosition(nl.Location, -1);
                    }
                    else if (e.Button == GLMouseEventArgs.MouseButtons.Right)
                    {
                        if (rightclickmenu != null)
                        {
                            rightclickmenu.Tag = item;
                            rightclickmenu.Show(displaycontrol, new Point(e.Location.X, e.Location.Y - 32));
                        }
                    }
                }
            }
        }

        private void OtherKeys(GLOFC.Controller.KeyboardMonitor kb)
        {
            // See OFC PositionCamera for keys used
            // F keys are reserved for KeyPresses action pack

            if ((parts & Parts.PerspectiveChange) != 0 && kb.HasBeenPressed(Keys.P, GLOFC.Controller.KeyboardMonitor.ShiftState.None))
            {
                gl3dcontroller.ChangePerspectiveMode(!gl3dcontroller.MatrixCalc.InPerspectiveMode);
            }

            if ((parts & Parts.YHoldButton) != 0 && kb.HasBeenPressed(Keys.O, KeyboardMonitor.ShiftState.None))
            {
                gl3dcontroller.YHoldMovement = !gl3dcontroller.YHoldMovement;
            }

            if (kb.HasBeenPressed(Keys.D1, GLOFC.Controller.KeyboardMonitor.ShiftState.None))
            {
                GalaxyDisplay = !GalaxyDisplay;
            }
            if (kb.HasBeenPressed(Keys.D2, GLOFC.Controller.KeyboardMonitor.ShiftState.None))
            {
                Grid = !Grid;
            }
            if (kb.HasBeenPressed(Keys.D3, GLOFC.Controller.KeyboardMonitor.ShiftState.None))
            {
                StarDotsSpritesDisplay = !StarDotsSpritesDisplay;
            }
            if (kb.HasBeenPressed(Keys.D4, GLOFC.Controller.KeyboardMonitor.ShiftState.None))
            {
                TravelPathTapeDisplay = TravelPathTapeStars = TravelPathTextDisplay = !TravelPathTapeDisplay;
            }
            if (kb.HasBeenPressed(Keys.D5, GLOFC.Controller.KeyboardMonitor.ShiftState.None))
            {
                GalObjectDisplay = !GalObjectDisplay;
            }
            if (kb.HasBeenPressed(Keys.D6, GLOFC.Controller.KeyboardMonitor.ShiftState.None))
            {
                GalaxyStars = GalaxyStars >= 0 ? 0 : 3;
            }
            if (kb.HasBeenPressed(Keys.D7, GLOFC.Controller.KeyboardMonitor.ShiftState.None))
            {
                if (EDSMRegionsEnable)
                    EDSMRegionsOutlineEnable = !EDSMRegionsOutlineEnable;
                else
                    EliteRegionsOutlineEnable = !EliteRegionsOutlineEnable;
            }
            if (kb.HasBeenPressed(Keys.D8, GLOFC.Controller.KeyboardMonitor.ShiftState.None))
            {
                if (EDSMRegionsEnable)
                    EDSMRegionsShadingEnable = !EDSMRegionsShadingEnable;
                else
                    EliteRegionsShadingEnable = !EliteRegionsShadingEnable;
            }
            if (kb.HasBeenPressed(Keys.D9, GLOFC.Controller.KeyboardMonitor.ShiftState.None))
            {
                if (EDSMRegionsEnable)
                    EDSMRegionsTextEnable = !EDSMRegionsTextEnable;
                else
                    EliteRegionsTextEnable = !EliteRegionsTextEnable;
            }
        }

        #endregion
    }
}
