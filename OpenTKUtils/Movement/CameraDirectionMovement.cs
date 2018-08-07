/*
 * Copyright © 2016 EDDiscovery development team
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
using OpenTK;

namespace OpenTKUtils
{
    public class CameraDirectionMovementTracker       // keeps track of previous and works out how to present bitmaps
    {
        public Vector3 LastCameraPos;
        public Vector3 LastCameraDir;
        public float LastZoom;
        public Vector3 LastCameraGrossDir;               // for gross direction camera adjustments

        public Vector3 Rotation = new Vector3(0, 0, 0);

        public bool CameraDirChanged;
        public bool CameraDirGrossChanged;
        public bool CameraMoved;
        public bool CameraZoomed;
        public bool AnythingChanged { get { return CameraDirChanged || CameraMoved || CameraZoomed; } }         //DIR is more sensitive than gross, so no need to use
                
        public void Update(Vector3 cameraDir, Vector3 cameraPos, float zoom, float grossdirchange)
        {
            CameraDirChanged = Vector3.Subtract(LastCameraDir, cameraDir).LengthSquared >= 1;

            if (CameraDirChanged)
            {
                LastCameraDir = cameraDir;
            }

            CameraDirGrossChanged = Vector3.Subtract(LastCameraGrossDir, cameraDir).LengthSquared >= grossdirchange;

            if ( CameraDirGrossChanged )
            {
                LastCameraGrossDir = cameraDir;
            }

            CameraMoved = Vector3.Subtract(LastCameraPos, cameraPos).LengthSquared >= 0.05; // small so you can see small slews

            if ( CameraMoved )
                LastCameraPos = cameraPos;

            float zoomfact = zoom / LastZoom;

            CameraZoomed = (zoomfact >= 1.05 || zoomfact <= 0.95);     // prevent too small zoom causing a repaint

            if ( CameraZoomed )
                LastZoom = zoom;

            Rotation = LastCameraDir;
            Rotation.X = -Rotation.X;       // invert to face
            Rotation.Z = 0;                 // no Z, not used much, and cause the other two axis to spin .. would need more work to understand
        }

        public void ForceMoveChange()
        {
            LastCameraPos = new Vector3(float.MinValue, 0, 0);
        }

        public void SetGrossChanged()       // tell it that we dealt with it and move gross back to last camera
        {
            LastCameraGrossDir = LastCameraDir;
        }
    }


}
