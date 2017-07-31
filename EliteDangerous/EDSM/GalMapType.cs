﻿/*
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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using EliteDangerousCore;

namespace EliteDangerousCore.EDSM
{
    public class GalMapType
    {
        public enum GalMapGroup
        {
            Markers = 1,
            Routes,
            Regions,
            Quadrants,
        }

        public string Typeid;
        public string Description;
        public Bitmap Image;
        public GalMapGroup Group;
        public bool Enabled;

        public GalMapType(string id, string desc, GalMapGroup g, Bitmap b, int i)
        {
            Typeid = id;
            Description = desc;
            Group = g;
            Image = b;
            Enabled = false;
        }
        
        static public List<GalMapType> GetTypes()
        {
            List<GalMapType> type = new List<GalMapType>();

            int index = 0;

            type.Add(new GalMapType("historicalLocation", "η Historical Location", GalMapGroup.Markers, EliteDangerous.Properties.Resources.pointofinterest, index++));
            type.Add(new GalMapType("nebula", "α Nebula", GalMapGroup.Markers, EliteDangerous.Properties.Resources.nebula, index++));
            type.Add(new GalMapType("planetaryNebula", "β Planetary Nebula", GalMapGroup.Markers, EliteDangerous.Properties.Resources.PlanetaryNebula, index++));
            type.Add(new GalMapType("stellarRemnant", "γ Stellar Features", GalMapGroup.Markers, EliteDangerous.Properties.Resources.DefaultStar, index++));
            type.Add(new GalMapType("blackHole", "δ Black Hole", GalMapGroup.Markers, EliteDangerous.Properties.Resources.Blackhole, index++));
            type.Add(new GalMapType("starCluster", "σ Star Cluster", GalMapGroup.Markers, EliteDangerous.Properties.Resources.starcluster, index++));
            type.Add(new GalMapType("pulsar", "ζ Pulsar", GalMapGroup.Markers , EliteDangerous.Properties.Resources.pulsar, index++));
            type.Add(new GalMapType("minorPOI", "★ Minor POI or Star", GalMapGroup.Markers , EliteDangerous.Properties.Resources.pointofinterest, index++));
            type.Add(new GalMapType("beacon", "⛛ Beacon", GalMapGroup.Markers , EliteDangerous.Properties.Resources.pointofinterest, index++));
            type.Add(new GalMapType("surfacePOI", "∅ Surface POI", GalMapGroup.Markers , EliteDangerous.Properties.Resources.pointofinterest, index++));
            type.Add(new GalMapType("cometaryBody", "☄ Cometary Body", GalMapGroup.Markers , EliteDangerous.Properties.Resources.comet, index++));
            type.Add(new GalMapType("jumponiumRichSystem", "☢ Jumponium-Rich System", GalMapGroup.Markers, EliteDangerous.Properties.Resources.pointofinterest, index++));
            type.Add(new GalMapType("planetFeatures", "∅ Planetary Features", GalMapGroup.Markers, EliteDangerous.Properties.Resources.pointofinterest, index++));
            type.Add(new GalMapType("deepSpaceOutpost", "Deep space outpost", GalMapGroup.Markers, EliteDangerous.Properties.Resources.pointofinterest, index++));
            type.Add(new GalMapType("mysteryPOI", "Mystery POI", GalMapGroup.Markers, EliteDangerous.Properties.Resources.pointofinterest, index++));
            type.Add(new GalMapType("restrictedSectors", "Restricted Sectors", GalMapGroup.Markers, EliteDangerous.Properties.Resources.no_entry, index++));

            type.Add(new GalMapType("travelRoute", "Travel Route", GalMapGroup.Routes , null, index++));
            type.Add(new GalMapType("historicalRoute", "Historical Route", GalMapGroup.Routes , null, index++));
            type.Add(new GalMapType("minorRoute", "Minor Route", GalMapGroup.Routes, null, index++));
            type.Add(new GalMapType("neutronRoute", "Neutron highway", GalMapGroup.Routes, null, index++));

            type.Add(new GalMapType("region", "Region", GalMapGroup.Regions , null, index++));
            type.Add(new GalMapType("regionQuadrants", "Galactic Quadrants", GalMapGroup.Quadrants , null, index++));

            type.Add(new GalMapType("EDSMUnknown", "EDSM other POI type", GalMapGroup.Markers, EliteDangerous.Properties.Resources.pointofinterest, index++));

            return type;
        }
    }
}