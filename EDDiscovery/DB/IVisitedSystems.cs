﻿using System;

namespace EDDiscovery2.DB
{
    public interface IVisitedSystems
    {
        long id { get; set; }
        string Name { get; set; }
        DateTime Time { get; set; }
        int Commander { get; set; }
        long Source { get; set; }
        string Unit { get; set; }
        bool EDSM_sync { get; set; }
        int MapColour { get; set; }
        double X { get; set; }
        double Y { get; set; }
        double Z { get; set; }


        bool Update();
    }
}