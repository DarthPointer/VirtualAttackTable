using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAttackTableLib.TargetShipParameter
{
    public class TorpedoSpeedMpSParameter : MultipleDefinitionParameter<string, float>
    {
        #region Properties
        private ArbitraryValueParameterDefinition<float> ArbitrarySpeedDefinition
        {
            get;
            init;
        }

        public float ArbitrarySpeed
        {
            get => ArbitrarySpeedDefinition.Value;
            set => ArbitrarySpeedDefinition.Value = value;
        }
        #endregion

        #region Constructors
        public TorpedoSpeedMpSParameter() : base()
        {
            ArbitrarySpeedDefinition = new(this);
            AddDefinition("arbitrary", ArbitrarySpeedDefinition);

            AddDefinition("G7e", new FixedValueParameterDefinition<float>(this, Units.Knot.ToMpS(30)));
            AddDefinition("G7a fast", new FixedValueParameterDefinition<float>(this, Units.Knot.ToMpS(44)));
            AddDefinition("G7a medium", new FixedValueParameterDefinition<float>(this, Units.Knot.ToMpS(40)));
            AddDefinition("G7a slow", new FixedValueParameterDefinition<float>(this, Units.Knot.ToMpS(30)));
        }
        #endregion
    }
}
