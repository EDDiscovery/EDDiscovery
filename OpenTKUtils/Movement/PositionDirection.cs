/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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
using System;
using System.Diagnostics;

namespace OpenTKUtils
{
    public class PositionDirection
    {
        public Vector3 Position { get { Vector3 norm = new Vector3(_viewtargetpos); norm.Y = -norm.Y; return norm; } } // REAL world position of eye.  With no internal invert of Y.  Same as star positions

        public Vector3 CameraDirection { get { return _cameraDir; } }

        public bool InSlews { get { return (_cameraSlewProgress < 1.0f || _cameraDirSlewProgress < 1.0F); } }
        public bool InPerspectiveMode { get { return _perspectivemode; } set { _perspectivemode = value; } }
        public Matrix4 ModelMatrix { get { return _modelmatrix; } }
        public Matrix4 ProjectionMatrix { get { return _projectionmatrix; } }

        private bool _perspectivemode = false;

        private Vector3 _viewtargetpos = Vector3.Zero;          // point where we are viewing. Eye is offset from this by _cameraDir * 1000/_zoom. (prev _cameraPos)
                                                                // Y is upside down
        private Vector3 _cameraDir = Vector3.Zero;              // X = up/down, Y = rotate, Z = yaw, in degrees. 

        private float _cameraSlewProgress = 1.0f;               // 0 -> 1 slew progress
        private float _cameraSlewTime;                          // how long to take to do the slew
        private Vector3 _cameraSlewPosition;                    // where to slew to.

        private float _cameraDirSlewProgress = 1.0f;            // 0 -> 1 slew progress
        private float _cameraDirSlewTime;                       // how long to take to do the slew
        private Vector3 _cameraDirSlewPosition;                 // where to slew to.

        private Matrix4 _modelmatrix;
        private Matrix4 _projectionmatrix;

        #region Position

        public void SetCameraPos(Vector3 pos)
        {
            _viewtargetpos = pos;
        }

        public void MoveCameraPos(Vector3 pos)
        {
            _viewtargetpos += pos;
        }


        // time <0 estimate, 0 instance >0 time
        public void StartCameraSlew(Vector3 normpos, float timeslewsec = 0)       // may pass a Nan Position - no action. Y is normal sense
        {
            if (!float.IsNaN(normpos.X))
            {
                Vector3 pos = new Vector3(normpos);
                pos.Y = -pos.Y;     // invert to internal Y

                double dist = Math.Sqrt((_viewtargetpos.X - pos.X) * (_viewtargetpos.X - pos.X) + (_viewtargetpos.Y - pos.Y) * (_viewtargetpos.Y - pos.Y) + (_viewtargetpos.Z - pos.Z) * (_viewtargetpos.Z - pos.Z));
                Debug.Assert(!double.IsNaN(dist));      // had a bug due to incorrect signs!

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
                        _cameraSlewTime = (timeslewsec < 0) ? ((float)Math.Max(1.0, dist / 10000.0)) : timeslewsec;            //10000 ly/sec, with a minimum slew
                        //Console.WriteLine("Slew start to {0} in {1}",  _cameraSlewPosition , _cameraSlewTime);
                    }
                }
            }
        }

        #endregion

        #region Camera Direction


        public void SetCameraDir(Vector3 pos)
        {
            _cameraDir = pos;
        }

        public void RotateCameraDir(Vector3 rot)
        {
            _cameraDir += rot;

            if (_cameraDir.X < 0 && _cameraDir.X > -90)
                _cameraDir.X = 0;

            if (_cameraDir.X > 180 || _cameraDir.X <= -90)
                _cameraDir.X = 180;
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

        public void CameraLookAt(Vector3 normtarget, float _zoom, float time = 0)            // real world 
        {
            Vector3 target = new Vector3(normtarget.X, -normtarget.Y, normtarget.Z);
            Vector3 eye = _viewtargetpos;
            Vector3 camera = AzEl(eye, target);
            camera.Y = 180 - camera.Y;      // adjust to this system
            StartCameraPan(camera, time);
        }

        #endregion

        #region Slews

        public void KillSlews()
        {
            _cameraSlewProgress = 1.0f;
            _cameraDirSlewProgress = 1.0f;
        }


        public void DoCameraSlew(int _msticks)
        {
            if (_cameraSlewProgress < 1.0f)
            {
                Debug.Assert(_cameraSlewTime > 0);
                var newprogress = _cameraSlewProgress + _msticks / (_cameraSlewTime * 1000);

                if (newprogress >= 1.0f)
                {
                    _viewtargetpos = new Vector3(_cameraSlewPosition.X, _cameraSlewPosition.Y, _cameraSlewPosition.Z);
                    //Console.WriteLine("{0} Slew complete at {1} {2}", slewt.ElapsedMilliseconds % 10000, _viewtargetpos, _cameraSlewPosition);
                }
                else
                {
                    var slewstart = Math.Sin((_cameraSlewProgress - 0.5) * Math.PI);
                    var slewend = Math.Sin((newprogress - 0.5) * Math.PI);
                    Debug.Assert((1 - 0 - slewstart) != 0);
                    var slewfact = (slewend - slewstart) / (1.0 - slewstart);

                    var totvector = new Vector3((float)(_cameraSlewPosition.X - _viewtargetpos.X), (float)(_cameraSlewPosition.Y - _viewtargetpos.Y), (float)(_cameraSlewPosition.Z - _viewtargetpos.Z));
                    _viewtargetpos += Vector3.Multiply(totvector, (float)slewfact);
                    //Console.WriteLine("{0} Slew to {1}", slewt.ElapsedMilliseconds % 10000, _viewtargetpos);
                }

                _cameraSlewProgress = (float)newprogress;
            }

            if (_cameraDirSlewProgress < 1.0f)
            {
                var newprogress = _cameraDirSlewProgress + _msticks / (_cameraDirSlewTime * 1000);

                if (newprogress >= 1.0f)
                {
                    _cameraDir = _cameraDirSlewPosition;
                }
                else
                {
                    var slewstart = Math.Sin((_cameraDirSlewProgress - 0.5) * Math.PI);
                    var slewend = Math.Sin((newprogress - 0.5) * Math.PI);
                    Debug.Assert((1 - 0 - slewstart) != 0);
                    var slewfact = (slewend - slewstart) / (1.0 - slewstart);

                    var totvector = new Vector3((float)(_cameraDirSlewPosition.X - _cameraDir.X), (float)(_cameraDirSlewPosition.Y - _cameraDir.Y), (float)(_cameraDirSlewPosition.Z - _cameraDir.Z));
                    _cameraDir += Vector3.Multiply(totvector, (float)slewfact);
                }

                _cameraDirSlewProgress = (float)newprogress;
            }
        }

        #endregion

        #region Keys

        public void HandleTurningAdjustments(KeyboardActions _kbdActions, int _msticks )
        {
            Vector3 _cameraActionRotation = Vector3.Zero;

            var angle = (float)_msticks * 0.075f;
            if (_kbdActions.Action(KeyboardActions.ActionType.YawLeft))
            {
                _cameraActionRotation.Z = -angle;
            }
            if (_kbdActions.Action(KeyboardActions.ActionType.YawRight))
            {
                _cameraActionRotation.Z = angle;
            }
            if (_kbdActions.Action(KeyboardActions.ActionType.Dive))
            {
                _cameraActionRotation.X = -angle;
            }
            if (_kbdActions.Action(KeyboardActions.ActionType.Pitch))
            {
                _cameraActionRotation.X = angle;
            }
            if (_kbdActions.Action(KeyboardActions.ActionType.RollLeft))
            {
                _cameraActionRotation.Y = -angle;
            }
            if (_kbdActions.Action(KeyboardActions.ActionType.RollRight))
            {
                _cameraActionRotation.Y = angle;
            }

            if (_cameraActionRotation.LengthSquared > 0)
            {
                _cameraDir.X = BoundedAngle(_cameraDir.X + _cameraActionRotation.X);
                _cameraDir.Y = BoundedAngle(_cameraDir.Y + _cameraActionRotation.Y);
                _cameraDir.Z = BoundedAngle(_cameraDir.Z + _cameraActionRotation.Z);        // rotate camera by asked value

                // Limit camera pitch
                if (_cameraDir.X < 0 && _cameraDir.X > -90)
                    _cameraDir.X = 0;
                if (_cameraDir.X > 180 || _cameraDir.X <= -90)
                    _cameraDir.X = 180;
            }
        }

        public void HandleMovementAdjustments(KeyboardActions _kbdActions, int _msticks, float _zoom, bool elitemovement)
        {
            Vector3 _cameraActionMovement = Vector3.Zero;

            float zoomlimited = Math.Min(Math.Max(_zoom, 0.01F), 15.0F);
            var distance = _msticks * (1.0f / zoomlimited);

            if ( _kbdActions.Shift )
                distance *= 2.0F;

            //Console.WriteLine("Distance " + distance + " zoom " + _zoom + " lzoom " + zoomlimited );
            if (_kbdActions.Action(KeyboardActions.ActionType.Left))
            {
                _cameraActionMovement.X = -distance;
            }
            else if (_kbdActions.Action(KeyboardActions.ActionType.Right))
            {
                _cameraActionMovement.X = distance;
            }

            if (_kbdActions.Action(KeyboardActions.ActionType.PgUp))    // pgup/r
            {
                if (InPerspectiveMode)
                    _cameraActionMovement.Z = distance;
            }
            else if (_kbdActions.Action(KeyboardActions.ActionType.PgDown) ) //pgdown/f
            {
                if (InPerspectiveMode)
                    _cameraActionMovement.Z = -distance;
            }

            if (_kbdActions.Action(KeyboardActions.ActionType.Up))          // w/UP
            {
                if (InPerspectiveMode)
                    _cameraActionMovement.Y = distance;
                else
                    _cameraActionMovement.Z = distance;
            }
            else if (_kbdActions.Action(KeyboardActions.ActionType.Down))        // S/Down
            {
                if (InPerspectiveMode)
                    _cameraActionMovement.Y = -distance;
                else
                    _cameraActionMovement.Z = -distance;
            }

            if (_cameraActionMovement.LengthSquared > 0)
            {
                if (!InPerspectiveMode)
                    elitemovement = false;

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
            }
        }

        #endregion

        #region Views

        public void CalculateEyePosition(float _zoom , out Vector3 eye, out Vector3 normal )
        {
            Matrix3 transform = Matrix3.Identity;                   // identity nominal matrix, dir is in degrees
            transform *= Matrix3.CreateRotationZ((float)(_cameraDir.Z * Math.PI / 180.0f));
            transform *= Matrix3.CreateRotationX((float)(_cameraDir.X * Math.PI / 180.0f));
            transform *= Matrix3.CreateRotationY((float)(_cameraDir.Y * Math.PI / 180.0f));
            // transform ends as the camera direction vector

            // calculate where eye is, relative to target. its 1000/zoom, rotated by camera rotation
            Vector3 eyerel = Vector3.Transform(new Vector3(0.0f, -1000.0f / _zoom, 0.0f), transform);

            // rotate the up vector (0,0,1) by the eye camera dir to get a vector upwards from the current camera dir
            normal = Vector3.Transform(new Vector3(0.0f, 0.0f, 1.0f), transform);

            eye = _viewtargetpos + eyerel;              // eye is here, the target pos, plus the eye relative position
        }

        public void CalculateModelMatrix(float _zoom)       // We compute the model matrix, not opengl, because we need it before we do a Paint for other computations
        {
            Matrix4 flipy = Matrix4.CreateScale(new Vector3(1, -1, 1));
            Matrix4 preinverted;

            if (InPerspectiveMode)
            {
                Vector3 eye, normal;
                CalculateEyePosition(_zoom, out eye, out normal);
                preinverted = Matrix4.LookAt(eye, _viewtargetpos, normal);   // from eye, look at target, with up giving the rotation of the look
                _modelmatrix = Matrix4.Mult(flipy, preinverted);    //ORDER VERY important this one took longer to work out the order! replaces GL.Scale(1.0, -1.0, 1.0);
            }
            else
            {                                                               // replace open gl computation with our own.
                Matrix4 scale = Matrix4.CreateScale(_zoom);
                Matrix4 offset = Matrix4.CreateTranslation(-_viewtargetpos.X, -_viewtargetpos.Y, -_viewtargetpos.Z);
                Matrix4 rotcam = Matrix4.Identity;
                rotcam *= Matrix4.CreateRotationY((float)(-_cameraDir.Y * Math.PI / 180.0f));
                rotcam *= Matrix4.CreateRotationX((float)((_cameraDir.X - 90) * Math.PI / 180.0f));
                rotcam *= Matrix4.CreateRotationZ((float)(_cameraDir.Z * Math.PI / 180.0f));

                preinverted = Matrix4.Mult(offset,scale);
                preinverted = Matrix4.Mult(preinverted, rotcam);
                _modelmatrix = preinverted;
            }

        }

        public Matrix4 GetResMat
        {
            get
            {
                Matrix4 resmat = Matrix4.Mult(_modelmatrix, _projectionmatrix);
                return resmat;
            }
        }

        public void CalculateProjectionMatrix(float fov, int w, int h, out float _znear)
        {
            if (InPerspectiveMode)
            {                                                                   // Fov, perspective, znear, zfar
                _znear = 1.0F;
                _projectionmatrix = Matrix4.CreatePerspectiveFieldOfView(fov, (float)w / h, 1.0f, 1000000.0f);
            }
            else
            {
                _znear = -5000.0f;
                float orthoheight = 1000.0f * h / w;
                _projectionmatrix = Matrix4.CreateOrthographic(2000F, orthoheight * 2.0F, -5000.0F, 5000.0F);
            }
        }

        private float BoundedAngle(float angle)
        {
            return ((angle + 360 + 180) % 360) - 180;
        }

        private float DegreesToRadians(float angle)
        {
            return (float)(Math.PI * angle / 180.0);
        }

        private static Vector3 AzEl(Vector3 curpos, Vector3 target)     // az and elevation between curpos and target
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

#endregion

    }
}
