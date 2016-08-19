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

        private float _cameraFov = (float)(Math.PI / 2.0f);     // Camera, in radians, 180/2 = 90 degrees

        private const double ZoomMax = 300;
        private const double ZoomMin = 0.01;
        private const double ZoomFact = 1.2589254117941672104239541063958;

        private float _defaultZoom;

        private float _zoom = 1.0f;
        
        public void SetDefaultZoom(float x)
        {
            _defaultZoom = x;
        }

        public void SetToDefault()
        {
            _zoom = _defaultZoom;
        }

        public bool SetZoom( float z , float timetozoom = 0)        // return if zoom sizes needs updating now.
        {
            _zoom = z;
            return true;
        }

        public bool DoZoom(int _msticks )                           // do dynamic zoom adjustments..  true if a repaint and readjust zoom objects needed
        {
            return false;
        }

        public bool HandleZoomAdjustmentKeys(KeyboardActions _kbdActions, int _msticks)
        {
            float curzoom = _zoom;
            var adjustment = 1.0f + ((float)_msticks * 0.01f);
            if (_kbdActions.ZoomIn)
            {
                _zoom *= (float)adjustment;
                if (_zoom > ZoomMax)
                    _zoom = (float)ZoomMax;
            }
            if (_kbdActions.ZoomOut)
            {
                _zoom /= (float)adjustment;
                if (_zoom < ZoomMin)
                    _zoom = (float)ZoomMin;
            }

            return _zoom != curzoom;
        }

        public bool ChangeFov(bool direction)        // direction true is scale up FOV
        {
            float curfov = _cameraFov;

            if (direction)
                _cameraFov = (float)Math.Min(_cameraFov * ZoomFact, Math.PI * 0.8);
            else
                _cameraFov /= (float)ZoomFact;

            return curfov != _cameraFov;
        }

        public bool ChangeZoom(bool direction)        // direction true is scale up zoom
        {
            float curzoom = _zoom;

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

            return _zoom != curzoom;
        }


    }
}
