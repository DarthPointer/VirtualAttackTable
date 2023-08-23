using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAttackTableLib.TargetShipParameter
{
    public class FixedValueParameterDefinition<TValue> : ParameterDefinition<TValue>
    {
        public FixedValueParameterDefinition(IParameter owningParameter, TValue value) :
            base(owningParameter)
        {
            Formula = () => value;
            Update();
        }
    }
}
