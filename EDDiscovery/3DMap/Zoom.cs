using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2._3DMap
{
    class ZoomFov
    {
        public float Zoom { get { return _zoom; } }
        public float Fov { get { return _cameraFov; } }
        public float FovDeg { get { return (float)(_cameraFov/Math.PI*180); } }
        public bool InSlew { get { return (_zoomprogress < 1.0F); } }

        private float _cameraFov = (float)(Math.PI / 2.0f);     // Camera, in radians, 180/2 = 90 degrees

        private const double ZoomMax = 300;
        private const double ZoomMin = 0.01;
        private const double ZoomFact = 1.2589254117941672104239541063958;

        private float _defaultZoom = 1F;

        private float _zoom = 1.0f;

        private float _zoomprogress = 1.0F;
        private float _zoomslewtime = 0;
        private float _zoomstart = 0;
        private float _zoomtarget = 0;
        private float _zoomlastpaint = 0;

        public void SetDefaultZoom(float x)
        {
            _defaultZoom = x;
        }

        public void SetToDefault()
        {
            _zoom = _defaultZoom;
        }

        public void StartZoom( float z , float timetozoom = 0)        // return if zoom sizes needs updating now.
        {
            if (timetozoom == 0)
            {
                _zoom = z;
            }
            else if ( z != _zoom )
            {
                _zoomprogress = 0;
                _zoomtarget = z;
                _zoomslewtime = timetozoom;
                _zoomstart = _zoomlastpaint = _zoom;
                Console.WriteLine("Zoom {0} to {1} in {2}", _zoomstart, _zoomtarget, _zoomslewtime);
            }
        }

        public void KillSlew()
        {
            _zoomprogress = 1.0F;
        }

        public void DoZoomSlew(int _msticks )                           // do dynamic zoom adjustments..  true if a readjust zoom needed
        {
            if ( _zoomprogress < 1.0F )
            {
                float oldzoom = _zoom;

                _zoomprogress = _zoomprogress + _msticks / (_zoomslewtime * 1000);

                if (_zoomprogress >= 1.0F)
                {
                    _zoom = _zoomtarget;
                }
                else
                {
                    _zoom = _zoomstart + _zoomprogress * (_zoomtarget - _zoomstart);
                }
            }
        }

        public void HandleZoomAdjustmentKeys(KeyboardActions _kbdActions, int _msticks)
        {
            var adjustment = 1.0f + ((float)_msticks * 0.01f);

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

            if (_kbdActions.Action(KeyboardActions.ActionType.ZoomDefault))
                StartZoom(_defaultZoom, 1.5F);

            if (_kbdActions.Action(KeyboardActions.ActionType.ZoomWide))
                StartZoom(0.1F, 1.5F);
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
