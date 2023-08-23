using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VirtualAttackTableLib.TargetIdentification.StructureType;

namespace VirtualAttackTableLib.TargetIdentification
{
    public enum StructureType
    {
        Mast = 0,
        Crane = 1,
        Funnel = 2
    }

    public static class StructureTypeStringConversion
    {
        public static string GetStructureStringCode(StructureType type)
        {
            return type switch
            {
                Mast => "m",
                Crane => "k",
                Funnel => "f",

                _ => throw new ArgumentException($"{type} is not a valid {nameof(StructureType)} value to retrieve string code.", nameof(type))
            };
        }

        public static StructureType GetStructureType(char structuresCode)
        {
            return structuresCode switch
            {
                'm' => StructureType.Mast,
                'f' => StructureType.Funnel,
                'k' => StructureType.Crane,
                _ => throw new Exception($"\'{structuresCode}\' is not a valid structure type symbol.")
            };
        }
    }
}
