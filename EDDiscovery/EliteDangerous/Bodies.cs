using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.EliteDangerous
{
    public enum StarBodieEnum
    {
        StarUnknown = 0,
        O = 1,
        B,
        A,
        F,
        G,
        K,
        M,

        // Dwarf
        L,
        T,
        Y,

        // proto stars
        AeBe,
        TTS,


        // wolf rayet
        W,
        WN,
        WNE,   // wiki not journal may not be in game
        WNL,   // wiki not journal may not be in game
        WNC,
        WC, 
        WCR,   // wiki not journal may not be in game
        WCL,   // wiki not journal may not be in game
        WO,

                // Carbon
        CS,
        C,
        CN,
        CJ,
        CHD,


        MS,  //seen in log
        S,   // seen in log

                // white dwarf
        D,
        DA,
        DAB,
        DAO,
        DAZ,
        DAV,
        DB,
        DBZ,
        DBV,
        DO,
        DOV,
        DQ,
        DC,
        DCV,
        DX,

        N,   // Neutron

        H,      // currently speculative, not confirmed with actual data...

        X,    // currently speculative, not confirmed with actual data... in journal

        Supermassiveblackhole,
        A_Bluewhitesupergiant,
        F_Whitesupergiant,
        M_Redsupergiant,
        M_Redgiant,
        K_Orangegiant,
        Rogueplanet,
            
    }

    public enum  PlanetBodieEnum
    {
        PlanetUnknown = 0,

    }


    class Bodies
    {
    }
}
