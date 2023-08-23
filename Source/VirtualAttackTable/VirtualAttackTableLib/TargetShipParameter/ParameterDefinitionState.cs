using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAttackTableLib.TargetShipParameter
{
    public enum ParameterDefinitionState
    {
        Valid = 0,
        DependencyLoop = 1,
        Exception = 2
    }
}
