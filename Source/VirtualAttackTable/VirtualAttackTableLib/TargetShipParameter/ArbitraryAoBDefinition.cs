using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAttackTableLib.AttackTarget;

namespace VirtualAttackTableLib.TargetShipParameter
{
    public class ArbitraryAoBDefinition : ArbitraryValueParameterDefinition<float>
    {
        #region Properties
        private AoBParameter AoBParameter { get; }

        private bool LastQuarterWasLeft { get; set; }
        #endregion


        #region Constructors
        public ArbitraryAoBDefinition(AoBParameter owningParameter, float defaultValue = 0) : base(owningParameter, defaultValue)
        {
            AoBParameter = owningParameter;
            LastQuarterWasLeft = AoBParameter.AoBQuarter.IsLeft();
        }
        #endregion

        #region Methods
        protected override void DefinitionCustomPreUpdate()
        {
            if (LastQuarterWasLeft != AoBParameter.AoBQuarter.IsLeft())
            {
                LastQuarterWasLeft = AoBParameter.AoBQuarter.IsLeft();

                Value = MathF.PI * 2 - Value;
            }
        }

        
        #endregion
    }
}
