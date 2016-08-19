using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;

namespace EDDiscovery2._3DMap
{
    class PositionDirection
    {
        public Vector3 PositionExternal { get { Vector3 norm = new Vector3(_viewtargetpos); norm.Y = -norm.Y; return norm; } } // REAL world position of eye.
        public Vector3 CameraDirection { get { return _cameraDir; } }
        public bool InSlews { get { return (_cameraSlewProgress < 1.0f || _cameraDirSlewProgress < 1.0F); } }
        public bool InPerspectiveMode { get { return _perspectivemode; } }

        private bool _perspectivemode = false;

        private Vector3 _viewtargetpos = Vector3.Zero;          // point where we are viewing. Eye is offset from this by _cameraDir * 1000/_zoom. (prev _cameraPos)
                                                                // Y is upside down
        private Vector3 _cameraDir = Vector3.Zero;              // X = up/down, Y = rotate, Z = yaw, in degrees. 

        private Vector3 _cameraActionMovement = Vector3.Zero;
        private Vector3 _cameraActionRotation = Vector3.Zero;

        private float _cameraSlewProgress = 1.0f;               // 0 -> 1 slew progress
        private float _cameraSlewTime;                          // how long to take to do the slew
        private Vector3 _cameraSlewPosition;                    // where to slew to.

        private float _cameraDirSlewProgress = 1.0f;               // 0 -> 1 slew progress
        private float _cameraDirSlewTime;                          // how long to take to do the slew
        private Vector3 _cameraDirSlewPosition;                    // where to slew to.


        public void StartCameraSlew(Vector3 pos, float timeslewsec = 0)       // may pass a Nan Position - no action. Y is normal sense
        {
            Vector3 invviewpos = new Vector3(pos);
            invviewpos.Y = -invviewpos.Y;
            StartCameraSlewNI(invviewpos, timeslewsec);
        }
        
        // time <0 estimate, 0 instance >0 time
        private void StartCameraSlewNI(Vector3 pos, float timeslewsec = 0)     // may pass a Nan Position - no action.  pos.Y is same sense as _viewtargetpos (i.e inverted)
        {
            if (!float.IsNaN(pos.X))
            {
                double dist = Math.Sqrt((_viewtargetpos.X - pos.X) * (_viewtargetpos.X - pos.X) + (-_viewtargetpos.Y - pos.Y) * (-_viewtargetpos.Y - pos.Y) + (_viewtargetpos.Z - pos.Z) * (_viewtargetpos.Z - pos.Z));

                if (dist >= 1)
                {
                    if (timeslewsec == 0)
                    {
                        _viewtargetpos = pos;
                    }
                    else
                    {
                        _cameraSlewPosition = pos;
                        _cameraSlewProgress = 0.0f;
                        _cameraSlewTime = (timeslewsec < 0) ? ((float)Math.Max(2.0, dist / 10000.0)) : timeslewsec;            //10000 ly/sec, with a minimum slew
                    }
                }
            }
        }

        public void StartCameraPan(Vector3 pos, float timeslewsec = 0)       // may pass a Nan Position - no action
        {
            if (!float.IsNaN(pos.X))
            {
                if (timeslewsec == 0)
                {
                    _cameraDir = pos;
                }
                else
                {
                    _cameraDirSlewPosition = pos;
                    _cameraDirSlewProgress = 0.0f;
                    _cameraDirSlewTime = (timeslewsec == 0) ? (1.0F) : timeslewsec;
                }
            }
        }


        public void KillSlews()
        {
            _cameraSlewProgress = 1.0f;
            _cameraDirSlewProgress = 1.0f;
        }


        public bool DoCameraSlew(int _msticks)
        {
            bool repaint = false;

            if (_cameraSlewProgress < 1.0f)
            {
                _cameraActionMovement = Vector3.Zero;
                Debug.Assert(_cameraSlewTime > 0);
                var newprogress = _cameraSlewProgress + _msticks / (_cameraSlewTime * 1000);
                var totvector = new Vector3((float)(_cameraSlewPosition.X - _viewtargetpos.X), (float)(_cameraSlewPosition.Y - _viewtargetpos.Y), (float)(_cameraSlewPosition.Z - _viewtargetpos.Z));

                if (newprogress >= 1.0f)
                {
                    _viewtargetpos = new Vector3(_cameraSlewPosition.X, _cameraSlewPosition.Y, _cameraSlewPosition.Z);
                    //Console.WriteLine("{0} Slew complete at {1} {2}" , _updateinterval.ElapsedMilliseconds % 10000,_viewtargetpos, _cameraSlewPosition);
                }
                else
                {
                    var slewstart = Math.Sin((_cameraSlewProgress - 0.5) * Math.PI);
                    var slewend = Math.Sin((newprogress - 0.5) * Math.PI);
                    Debug.Assert((1 - 0 - slewstart) != 0);
                    var slewfact = (slewend - slewstart) / (1.0 - slewstart);
                    _viewtargetpos += Vector3.Multiply(totvector, (float)slewfact);
                }

                repaint = true;
                _cameraSlewProgress = (float)newprogress;
            }

            if (_cameraDirSlewProgress < 1.0f)
            {
                var newprogress = _cameraDirSlewProgress + _msticks / (_cameraDirSlewTime * 1000);
                var totvector = new Vector3((float)(_cameraDirSlewPosition.X - _cameraDir.X), (float)(_cameraDirSlewPosition.Y - _cameraDir.Y), (float)(_cameraDirSlewPosition.Z - _cameraDir.Z));

                if (newprogress >= 1.0f)
                {
                    _cameraDir = _cameraDirSlewPosition;
                    //Console.WriteLine("{0} Pan complete", _updateinterval.ElapsedMilliseconds % 10000);
                }
                else
                {
                    var slewstart = Math.Sin((_cameraDirSlewProgress - 0.5) * Math.PI);
                    var slewend = Math.Sin((newprogress - 0.5) * Math.PI);
                    Debug.Assert((1 - 0 - slewstart) != 0);
                    var slewfact = (slewend - slewstart) / (1.0 - slewstart);
                    _cameraDir += Vector3.Multiply(totvector, (float)slewfact);
                    //Console.WriteLine("Vector {0} Dir {1} progress {2}", totvector, _cameraDir, newprogress);
                }

                repaint = true;
                _cameraDirSlewProgress = (float)newprogress;
            }

            return repaint;
        }

        public void HandleTurningAdjustments(KeyboardActions _kbdActions, int _msticks )
        {
            _cameraActionRotation = Vector3.Zero;

            var angle = (float)_msticks * 0.075f;
            if (_kbdActions.YawLeft)
            {
                _cameraActionRotation.Z = -angle;
            }
            if (_kbdActions.YawRight)
            {
                _cameraActionRotation.Z = angle;
            }
            if (_kbdActions.Dive)
            {
                _cameraActionRotation.X = -angle;
            }
            if (_kbdActions.Pitch)
            {
                _cameraActionRotation.X = angle;
            }
            if (_kbdActions.RollLeft)
            {
                _cameraActionRotation.Y = -angle;
            }
            if (_kbdActions.RollRight)
            {
                _cameraActionRotation.Y = angle;
            }

        }

        public void HandleMovementAdjustments(KeyboardActions _kbdActions, int _msticks, float _zoom)
        {
            _cameraActionMovement = Vector3.Zero;
            float zoomlimited = Math.Min(Math.Max(_zoom, 0.01F), 15.0F);
            var distance = _msticks * (1.0f / zoomlimited);

            if ((Control.ModifierKeys & Keys.Shift) != 0)
                distance *= 2.0F;

            //Console.WriteLine("Distance " + distance + " zoom " + _zoom + " lzoom " + zoomlimited );
            if (_kbdActions.Left)
            {
                _cameraActionMovement.X = -distance;
            }
            if (_kbdActions.Right)
            {
                _cameraActionMovement.X = distance;
            }
            if (_kbdActions.Forwards)
            {
                _cameraActionMovement.Y = distance;
            }
            if (_kbdActions.Backwards)
            {
                _cameraActionMovement.Y = -distance;
            }
            if (_kbdActions.Up)
            {
                _cameraActionMovement.Z = distance;
            }
            if (_kbdActions.Down)
            {
                _cameraActionMovement.Z = -distance;
            }
        }

        public void UpdateCamera(bool elitemovement )
        {
            if (!InPerspectiveMode)
                elitemovement = false;

            _cameraDir.X = BoundedAngle(_cameraDir.X + _cameraActionRotation.X);
            _cameraDir.Y = BoundedAngle(_cameraDir.Y + _cameraActionRotation.Y);
            _cameraDir.Z = BoundedAngle(_cameraDir.Z + _cameraActionRotation.Z);        // rotate camera by asked value

            // Limit camera pitch
            if (_cameraDir.X < 0 && _cameraDir.X > -90)
                _cameraDir.X = 0;
            if (_cameraDir.X > 180 || _cameraDir.X <= -90)
                _cameraDir.X = 180;

#if DEBUG
            bool istranslating = (_cameraActionMovement.X != 0 || _cameraActionMovement.Y != 0 || _cameraActionMovement.Z != 0);
            //            if (istranslating)
            //                Console.WriteLine("move Camera " + _cameraActionMovement.X + "," + _cameraActionMovement.Y + "," + _cameraActionMovement.Z
            //                    + " point " + _cameraDir.X + "," + _cameraDir.Y + "," + _cameraDir.Z);
#endif

            var rotZ = Matrix4.CreateRotationZ(DegreesToRadians(_cameraDir.Z));
            var rotX = Matrix4.CreateRotationX(DegreesToRadians(_cameraDir.X));
            var rotY = Matrix4.CreateRotationY(DegreesToRadians(_cameraDir.Y));

            Vector3 requestedmove = new Vector3(_cameraActionMovement.X, _cameraActionMovement.Y, (elitemovement) ? 0 : _cameraActionMovement.Z);

            var translation = Matrix4.CreateTranslation(requestedmove);
            var cameramove = Matrix4.Identity;
            cameramove *= translation;
            cameramove *= rotZ;
            cameramove *= rotX;
            cameramove *= rotY;

            Vector3 trans = cameramove.ExtractTranslation();

            if (elitemovement)                                   // if in elite movement, Y is not affected
            {                                                   // by ASDW.
                trans.Y = 0;                                    // no Y translation even if camera rotated the vector into Y components
                _viewtargetpos += trans;
                _viewtargetpos.Y -= _cameraActionMovement.Z;        // translation appears in Z axis due to way the camera rotation is set up
            }
            else
                _viewtargetpos += trans;
#if DEBUG
            //            if (istranslating)
            //                Console.WriteLine("   em " + em + " Camera now " + _viewtargetpos.X + "," + _viewtargetpos.Y + "," + _viewtargetpos.Z);
#endif
        }

        public void CalculateEyePosition(float _zoom , out Vector3 eye, out Vector3 normal )
        {
            Vector3 target = _viewtargetpos;

            Matrix4 transform = Matrix4.Identity;                   // identity nominal matrix, dir is in degrees
            transform *= Matrix4.CreateRotationZ((float)(_cameraDir.Z * Math.PI / 180.0f));
            transform *= Matrix4.CreateRotationX((float)(_cameraDir.X * Math.PI / 180.0f));
            transform *= Matrix4.CreateRotationY((float)(_cameraDir.Y * Math.PI / 180.0f));
                                                                    // transform ends as the camera direction vector

            // calculate where eye is, relative to target. its 1000/zoom, rotated by camera rotation
            Vector3 eyerel = Vector3.Transform(new Vector3(0.0f, -1000.0f / _zoom, 0.0f), transform);

            // rotate the up vector (0,0,1) by the eye camera dir to get a vector upwards from the current camera dir
            normal = Vector3.Transform(new Vector3(0.0f, 0.0f, 1.0f), transform);

            eye = _viewtargetpos + eyerel;              // eye is here, the target pos, plus the eye relative position
        }

        public void SetMatrix(float _zoom)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);            // select the current matrix to the model view

            if (InPerspectiveMode)
            {
                Vector3 eye, normal;
                CalculateEyePosition(_zoom, out eye, out normal);
                Matrix4 lookat = Matrix4.LookAt(eye, _viewtargetpos, normal);   // from eye, look at target, with up giving the rotation of the look
                GL.LoadMatrix(ref lookat);                          // set the model view to this matrix.
            }
            else
            {
                GL.LoadIdentity();                  // model view matrix is 1/1/1/1.
                GL.Rotate(-90.0, 1, 0, 0);          // Rotate the world - current matrix, rotated -90 degrees around the vector (1,0,0)
                GL.Scale(_zoom, _zoom, _zoom);      // scale all the axis to zoom
                GL.Rotate(_cameraDir.Z, 0.0, 0.0, -1.0);    // rotate the axis around the camera dir
                GL.Rotate(_cameraDir.X, -1.0, 0.0, 0.0);
                GL.Rotate(_cameraDir.Y, 0.0, -1.0, 0.0);
                GL.Translate(-_viewtargetpos.X, -_viewtargetpos.Y, -_viewtargetpos.Z);  // and translate the model view by the view target pos
            }

            GL.Scale(1.0, -1.0, 1.0);               // Flip Y axis on world by inverting the model view matrix
        }

        public void SetViewport(bool pmode , ZoomFov zoomfov, int w, int h, out float _znear)
        {
            _perspectivemode = pmode;
            _znear = 1;
            if (w > 0 && h > 0)
            {
                GL.MatrixMode(MatrixMode.Projection);           // Select the project matrix for the following operations (current matrix)

                if (InPerspectiveMode)
                {                                                                   // Fov, perspective, znear, zfar
                    Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(zoomfov.Fov, (float)w / h, 1.0f, 1000000.0f);
                    GL.LoadMatrix(ref perspective);             // replace projection matrix with this perspective matrix
                    _znear = 1.0f;
                }
                else
                {
                    float orthoW = w * (zoomfov.Zoom + 1.0f);
                    float orthoH = h * (zoomfov.Zoom + 1.0f);

                    float orthoheight = 1000.0f * h / w;

                    GL.LoadIdentity();                              // set to 1/1/1/1.

                    // multiply identity matrix with orth matrix, left/right vert clipping plane, bot/top horiz clippling planes, distance between near/far clipping planes
                    GL.Ortho(-1000.0f, 1000.0f, -orthoheight, orthoheight, -5000.0f, 5000.0f);
                    _znear = -5000.0f;
                }

                GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
            }
        }

        public void RotateCamera( Vector3 rot )
        {
            _cameraDir += rot;

            if (_cameraDir.X < 0 && _cameraDir.X > -90)
                _cameraDir.X = 0;

            if (_cameraDir.X > 180 || _cameraDir.X <= -90)
                _cameraDir.X = 180;
        }

        public void SetCameraPos(Vector3 pos)
        {
            _viewtargetpos = pos;
        }

        public void MoveCamera(Vector3 pos)
        {
            _viewtargetpos += pos;
        }

        public void SetCameraDir(Vector3 pos)
        {
            _cameraDir = pos;
        }

        public void CameraLookAt(Vector3 target, float _zoom , float time = 0)            // real world 
        {
            Vector3 upsidedown = new Vector3(target.X, -target.Y, target.Z);
            CameraLookAtI(upsidedown, _zoom, time);
        }

        private void CameraLookAtI(Vector3 target, float _zoom, float time = 0)            // target in same sense at _viewtargetpos
        {
            Vector3 eye = _viewtargetpos;
            Vector3 camera = AzEl(eye, target);
            camera.Y = 180 - camera.Y;      // adjust to this system
            StartCameraPan(camera, time);
        }
        
        private float BoundedAngle(float angle)
        {
            return ((angle + 360 + 180) % 360) - 180;
        }

        private float DegreesToRadians(float angle)
        {
            return (float)(Math.PI * angle / 180.0);
        }

        private static Vector3 AzEl(Vector3 curpos, Vector3 target)
        {
            Vector3 delta = Vector3.Subtract(target, curpos);
            //Console.WriteLine("{0}->{1} d {2}", curpos, target, delta);

            float radius = delta.Length;

            if (radius < 0.1)
                return new Vector3(180, 0, 0);     // point forward, level

            float inclination = (float)Math.Acos(delta.Y / radius);
            float azimuth = (float)Math.Atan(delta.Z / delta.X);

            inclination *= (float)(180 / Math.PI);
            azimuth *= (float)(180 / Math.PI);

            if (delta.X < 0)      // atan wraps -90 (south)->+90 (north), then -90 to +90 around the y axis, going anticlockwise
                azimuth += 180;
            azimuth += 90;        // adjust to 0 at bottom, 180 north, to 360

            return new Vector3(inclination, azimuth, 0);
        }


    }
}
