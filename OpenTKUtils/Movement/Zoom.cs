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
using System;
using System.Diagnostics;

namespace OpenTKUtils
{
    public class ZoomFov
    {
        public float Zoom { get { return _zoom; } }
        public float Fov { get { return _cameraFov; } }
        public float FovDeg { get { return (float)(_cameraFov/Math.PI*180); } }
        public bool InSlew { get { return (_zoomtimer.IsRunning); } }

        private float _cameraFov = (float)(Math.PI / 2.0f);     // Camera, in radians, 180/2 = 90 degrees

        private const float ZoomMax = 300F;
        private const float ZoomMin = 0.01F;        // Iain special ;-)
        private const float ZoomFact = 1.258925F;

        private float _defaultZoom = 1F;
        private float _zoom = 1.0f;

        private int _zoomtimeperstep = 0;
        private float _zoommultiplier = 0;
        private float _zoomtarget = 0;
        private Stopwatch _zoomtimer = new Stopwatch();
        long _zoomnextsteptime = 0;

        public void SetDefaultZoom(float x)
        {
            x = Math.Max(x, ZoomMin);
            x = Math.Min(x, ZoomMax);
            _defaultZoom = x;
        }

        public void SetToDefault()
        {
            _zoom = _defaultZoom;
        }

        public void StartZoom( float z , float timetozoom = 0)        // <0 means auto estimate
        {
            if (timetozoom == 0)
            {
                _zoom = z;
            }
            else if ( z != _zoom )
            {
                _zoomtarget = z;

                if ( timetozoom < 0 )       // auto estimate on log distance between them
                {
                    timetozoom = (float)(Math.Abs(Math.Log10(_zoomtarget / _zoom)) * 1.5);
                }

                _zoomtimeperstep = 50;          // go for 20hz tick
                int wantedsteps = (int)((timetozoom * 1000.0F) / _zoomtimeperstep);
                _zoommultiplier = (float)Math.Pow(10.0, Math.Log10(_zoomtarget / _zoom) / wantedsteps );      // I.S^n = F I = initial, F = final, S = scaling, N = no of steps

                _zoomtimer.Stop();
                _zoomtimer.Reset();
                _zoomtimer.Start();
                _zoomnextsteptime = 0;

                //Console.WriteLine("Zoom {0} to {1} in {2} steps {3} steptime {4} mult {5}", _zoom, _zoomtarget, timetozoom*1000, wantedsteps, _zoomtimeperstep , _zoommultiplier );
            }
        }

        public void KillSlew()
        {
            _zoomtimer.Stop();
        }

        public void DoZoomSlew()                           // do dynamic zoom adjustments..  true if a readjust zoom needed
        {
            if ( _zoomtimer.IsRunning && _zoomtimer.ElapsedMilliseconds >= _zoomnextsteptime )
            {
                float newzoom = (float)(_zoom * _zoommultiplier);
                bool stop = (_zoomtarget > _zoom) ? (newzoom >= _zoomtarget) : (newzoom <= _zoomtarget);

                //Console.WriteLine("{0} Zoom {1} -> {2} m {3} t {4} stop {5}", _zoomtimer.ElapsedMilliseconds, _zoom , newzoom, _zoommultiplier, _zoomtarget, stop);

                if (stop)
                {
                    _zoom = _zoomtarget;
                    _zoomtimer.Stop();
                }
                else
                {
                    _zoom = newzoom;
                    _zoomnextsteptime += _zoomtimeperstep;
                }
            }
        }

        public void HandleZoomAdjustmentKeys(KeyboardActions _kbdActions, int _msticks)
        {
            var adjustment = 1.0f + ((float)_msticks * 0.002f);

            if (_kbdActions.Action(KeyboardActions.ActionType.ZoomIn))
            {
                _zoom *= (float)adjustment;
                if (_zoom > ZoomMax)
                    _zoom = (float)ZoomMax;
            }

            if (_kbdActions.Action(KeyboardActions.ActionType.ZoomOut))
            {
                _zoom /= (float)adjustment;
                if (_zoom < ZoomMin)
                    _zoom = (float)ZoomMin;
            }

            float newzoom = 0;

            if (_kbdActions.Action(KeyboardActions.ActionType.Zoom1))
                newzoom = ZoomMax;
            if (_kbdActions.Action(KeyboardActions.ActionType.Zoom2))
                newzoom = 100;                                                      // Factor 3 scale
            if (_kbdActions.Action(KeyboardActions.ActionType.Zoom3))
                newzoom =33;
            if (_kbdActions.Action(KeyboardActions.ActionType.Zoom4))
                newzoom =11F;
            if (_kbdActions.Action(KeyboardActions.ActionType.Zoom5))
                newzoom =3.7F;
            if (_kbdActions.Action(KeyboardActions.ActionType.Zoom6))
                newzoom =1.23F;
            if (_kbdActions.Action(KeyboardActions.ActionType.Zoom7))
                newzoom =0.4F;
            if (_kbdActions.Action(KeyboardActions.ActionType.Zoom8))
                newzoom =0.133F;
            if (_kbdActions.Action(KeyboardActions.ActionType.Zoom9))
                newzoom = ZoomMin;

            if (newzoom != 0)
                StartZoom(newzoom, -1);
        }

        public bool ChangeFov(bool direction)        // direction true is scale up FOV - need to tell it its changed
        {
            float curfov = _cameraFov;

            if (direction)
                _cameraFov = (float)Math.Min(_cameraFov * ZoomFact, Math.PI * 0.8);
            else
                _cameraFov /= (float)ZoomFact;

            return curfov != _cameraFov;
        }

        public void ChangeZoom(bool direction)        // direction true is scale up zoom
        {
            if (direction)
            {
                _zoom *= (float)ZoomFact;
                if (_zoom > ZoomMax)
                    _zoom = (float)ZoomMax;
            }
            else
            { 
                _zoom /= (float)ZoomFact;
                if (_zoom < ZoomMin)
                    _zoom = (float)ZoomMin;
            }
        }


    }
}
