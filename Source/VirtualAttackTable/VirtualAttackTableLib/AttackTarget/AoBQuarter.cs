using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAttackTableLib.AttackTarget
{
    public enum AoBQuarter
    {
        AheadRight = 0,
        AsternRight = 1,
        AsternLeft = 2,
        AheadLeft = 3
    }

    public static class AoBQuarterExtension
    {
        public static bool IsAhead(this AoBQuarter aoBQuarter)
        {
            return aoBQuarter == AoBQuarter.AheadRight || aoBQuarter == AoBQuarter.AheadLeft;
        }

        public static bool IsAstern(this AoBQuarter aoBQuarter)
        {
            return aoBQuarter == AoBQuarter.AsternRight || aoBQuarter == AoBQuarter.AsternLeft;
        }

        public static bool IsLeft(this AoBQuarter aoBQuarter)
        {
            return aoBQuarter == AoBQuarter.AheadLeft || aoBQuarter == AoBQuarter.AsternLeft;
        }

        public static bool IsRight(this AoBQuarter aoBQuarter)
        {
            return aoBQuarter == AoBQuarter.AheadRight || aoBQuarter == AoBQuarter.AsternRight;
        }
    }
}
