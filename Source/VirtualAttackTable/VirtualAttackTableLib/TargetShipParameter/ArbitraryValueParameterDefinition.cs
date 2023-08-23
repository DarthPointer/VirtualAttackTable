using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAttackTableLib.TargetShipParameter
{
    public class ArbitraryValueParameterDefinition<TValue> : ParameterDefinition<TValue>
    {
        #region Fields
        private TValue _value = default!;
        #endregion

        #region Properties
        public TValue Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!Equals(value, _value))
                {
                    _value = value;
                    Update();
                }
            }
        }
        #endregion

        #region Constructors
        public ArbitraryValueParameterDefinition(IParameter owningParameter, TValue defaultValue = default!) :
            base(owningParameter)
        {
            Formula = GetValue;
            Value = defaultValue;
        }
        #endregion

        #region Methods
        private TValue GetValue() => Value;
        #endregion
    }
}
