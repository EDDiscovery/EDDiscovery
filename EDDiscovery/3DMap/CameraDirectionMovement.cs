using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2
{
    public class CameraDirectionMovementTracker       // keeps track of previous and works out how to present bitmaps
    {
        public Vector3 CameraPos;
        public Vector3 CameraDir;
        public float LastZoom;

        public Vector3 Rotation = new Vector3(0, 0, 0);

        public bool CameraFlipped;
        public bool CameraDirChanged;
        public bool CameraMoved;
        public bool CameraZoomed;

        public void Update(Vector3 cameraDir, Vector3 cameraPos, float zoom)
        {
            CameraDirChanged = Vector3.Subtract(CameraDir, cameraDir).LengthSquared > 1 * 1 * 1;

            if (CameraDirChanged)
            {
                CameraDir = cameraDir;
                //Console.WriteLine("Dir {0},{1},{2}", CameraDir.X, CameraDir.Y, CameraDir.Z);
            }

            CameraMoved = Vector3.Subtract(CameraPos, cameraPos).LengthSquared > 3 * 3 * 3;

            if ( CameraMoved )
                CameraPos = cameraPos;

            CameraZoomed = Math.Abs(LastZoom - zoom) > 0.5F;          // if its worth doing a recalc..

            if ( CameraZoomed )
                LastZoom = zoom;

            Rotation = CameraDir;
            Rotation.X = -Rotation.X;       // invert to face
            Rotation.Z = 0;                 // no Z, not used much, and cause the other two axis to spin .. would need more work to understand
            CameraFlipped = CameraDirChanged;

            //TBD
           // Rotation = new Vector3(0, 0, 0);
        }

        public void ForceZoomChanged()
        {
            LastZoom = -1000000;
        }
    }


}
