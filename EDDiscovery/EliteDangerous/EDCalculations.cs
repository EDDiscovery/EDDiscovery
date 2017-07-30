using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.EliteDangerous
{
    static class EDCalculations
    {
        //Based on http://elite-dangerous.wikia.com/wiki/Frame_Shift_Drive
        public static double CalculateMaxJumpDistance(double f, double cargo, double lc, double mShip, double mOpt, double pc, double mfpj, out double jumps)
        {
            double fr = f % mfpj;
            jumps = Math.Floor(f / mfpj);

            mShip += fr + cargo;
            double d = 0.0;
            if (f > 0.0)
                d = Math.Pow(fr / (lc * 0.001), 1 / pc) * mOpt / mShip;

            for (int idx = 0; idx < jumps; idx++)
            {
                mShip += mfpj;
                d += Math.Pow(mfpj / (lc * 0.001), 1 / pc) * mOpt / mShip;
            }
            return d;
        }

    }
}
