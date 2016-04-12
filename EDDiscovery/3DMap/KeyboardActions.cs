using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2._3DMap
{
    public class KeyboardActions
    {
        public bool Left { get; set; }
        public bool Right { get; set; }
        public bool Up { get; set; }
        public bool Down { get; set; }
        public bool Forwards { get; set; }
        public bool Backwards { get; set; }
        public bool Pitch { get; set; }
        public bool Dive { get; set; }
        public bool YawLeft { get; set; }
        public bool YawRight { get; set; }
        public bool RollLeft { get; set;  }
        public bool RollRight { get; set; }
        public bool ZoomIn { get; set; }
        public bool ZoomOut { get; set; }

        public void Reset()
        {
            Left = false;
            Right = false;
            Up = false;
            Down = false;
            Forwards = false;
            Backwards = false;
            Pitch = false;
            Dive = false;
            YawLeft = false;
            YawRight = false;
            RollLeft = false;
            RollRight = false;
            ZoomIn = false;
            ZoomOut = false;
        }

        public bool Any()
        {
            return Left || Right || Up || Down || Forwards || Backwards || Pitch || Dive || YawLeft || YawRight || RollLeft || RollRight || ZoomIn || ZoomOut;
        }
    }
}
