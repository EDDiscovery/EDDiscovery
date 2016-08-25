using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2
{
    public class CameraDirectionMovementTracker       // keeps track of previous and works out how to present bitmaps
    {
        public Vector3 LastCameraPos;
        public Vector3 LastCameraDir;
        public float LastZoom;

        public Vector3 Rotation = new Vector3(0, 0, 0);

        public bool CameraDirChanged;
        public bool CameraMoved;
        public bool CameraZoomed;
        public bool AnythingChanged { get { return CameraDirChanged || CameraMoved || CameraZoomed; } }

        public void Update(Vector3 cameraDir, Vector3 cameraPos, float zoom)
        {
            CameraDirChanged = Vector3.Subtract(LastCameraDir, cameraDir).LengthSquared >= 1;

            if (CameraDirChanged)
            {
                LastCameraDir = cameraDir;
                //Console.WriteLine("Dir {0},{1},{2}", CameraDir.X, CameraDir.Y, CameraDir.Z);
            }

            CameraMoved = Vector3.Subtract(LastCameraPos, cameraPos).LengthSquared >= 0.05; // small so you can see small slews

            if ( CameraMoved )
                LastCameraPos = cameraPos;

            CameraZoomed = Math.Abs(LastZoom - zoom) > 0.0000001;

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
    }


}
