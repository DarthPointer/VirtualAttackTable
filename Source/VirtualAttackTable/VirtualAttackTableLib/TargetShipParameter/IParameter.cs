using CallbackList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAttackTableLib.TargetShipParameter
{
    public interface IParameter
    {
        IEnumerable<IParameter> GetDependencyParameters();

        CallbackListManager ParameterChanged { get; }
        //event Action? ParameterChanged;
    }
}
