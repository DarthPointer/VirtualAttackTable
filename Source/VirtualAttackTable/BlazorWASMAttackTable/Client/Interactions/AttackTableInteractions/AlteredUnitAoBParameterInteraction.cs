using BlazorWASMAttackTable.Client.Interactions.Options;
using VirtualAttackTableLib;
using VirtualAttackTableLib.AttackTarget;
using VirtualAttackTableLib.TargetShipParameter;

namespace BlazorWASMAttackTable.Client.Interactions.AttackTableInteractions
{
    public class AlteredUnitAoBParameterInteraction : AlteredUnitParameterInteraction<AoBDefinition>
    {
        #region Properties
        new public AoBParameter Parameter { get; }

        private float AoBPortStarboardAngleRadians => Parameter.AoBQuarter.IsRight() ? Parameter.CurrentValue : MathF.PI * 2 - Parameter.CurrentValue;

        public override float CurrentValue => CurrentUnit.FromBase(AoBPortStarboardAngleRadians);
        #endregion

        #region Constructors
        public AlteredUnitAoBParameterInteraction(AoBParameter parameter, IEnumerable<Option<AoBDefinition>> options, IListForSingleOption<IUnit> unitSource) : base(parameter, options, unitSource)
        {
            Parameter = parameter;
        }
        #endregion

        #region Methods
        public override void SetArbitraryValue(float value)
        {
            if (CanSetArbitraryValue)
            {
                float baseUnitsValue = GetAngleRadiansFromArbitraryAoBRadians(CurrentUnit.ToBase(value));
                SetArbitraryBaseUnitsValue(baseUnitsValue);
            }
        }

        private float GetAngleRadiansFromArbitraryAoBRadians(float aoB)
        {
            return Parameter.AoBQuarter.IsRight() ? aoB : MathF.PI * 2 - aoB;
        }
        #endregion
    }
}
