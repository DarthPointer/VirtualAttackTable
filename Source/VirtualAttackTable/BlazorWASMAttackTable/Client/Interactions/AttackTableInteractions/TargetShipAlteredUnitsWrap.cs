using BlazorWASMAttackTable.Client.Interactions.Options;
using VirtualAttackTableLib.AttackTarget;
using VirtualAttackTableLib.TargetShipParameter;

namespace BlazorWASMAttackTable.Client.Interactions.AttackTableInteractions
{
    public class TargetShipAlteredUnitsWrap
    {
        #region Proeprties
        public TargetShip TargetShip { get; }

        public AlteredUnitParameterInteraction<BearingDefinition> Bearing { get; }

        public AlteredUnitParameterInteraction<AbsoluteHeightDefinition> AbsoluteHeight { get; }
        public AlteredUnitParameterInteraction<VisibleHeightDefinition> VisibleHeight { get; }
        public AlteredUnitParameterInteraction<AbsoluteLengthDefinition> AbsoluteLength { get; }
        public AlteredUnitParameterInteraction<VisibleLengthDefinition> VisibleLength { get; }

        public AlteredUnitParameterInteraction<TargetRangeDefinition> TargetRange { get; }

        public AlteredUnitAoBParameterInteraction AoB { get; }

        public AlteredUnitParameterInteraction<HullTimeDefinition> HullTime { get; }
        public AlteredUnitParameterInteraction<OneDegreeTimeDefinition> OneDegreeTime { get; }

        public AlteredUnitParameterInteraction<TargetSpeedDefinition> TargetSpeed { get; }
        public AlteredUnitParameterInteraction<AngularSpeedDefinition> AngularSpeed { get; }

        public AlteredUnitParameterInteraction<string> TorpedoSpeed { get; }

        public AlteredUnitParameterInteraction<BoatSpeedDefinition> BoatSpeed { get; }

        public AlteredUnitParameterInteraction<LeadAngleDefinition> LeadAngle { get; }

        public ArbitraryValueParameterDefinition<float> ArbitraryAbsoluteHeightMeters { get; }

        public ArbitraryValueParameterDefinition<float> ArbitraryAbsoluteLengthMeters { get; }
        #endregion

        #region Constructors
        public TargetShipAlteredUnitsWrap(TargetShip targetShip, UnitsSelection unitsSelection)
        {
            TargetShip = targetShip;

            Bearing = new(TargetShip.BearingRadians, GenerateOptions(TargetShip.BearingRadians.GetPresentDefinitionKeys()), unitsSelection.Bearing);

            AbsoluteHeight = new(TargetShip.AbsoluteHeightMeters, GenerateOptions(TargetShip.AbsoluteHeightMeters.GetPresentDefinitionKeys()), unitsSelection.AbsoluteHeight);
            VisibleHeight = new(TargetShip.VisibleHeightRadians, GenerateOptions(TargetShip.VisibleHeightRadians.GetPresentDefinitionKeys()), unitsSelection.VisibleHeight);
            AbsoluteLength = new(TargetShip.AbsoluteLengthMeters, GenerateOptions(TargetShip.AbsoluteLengthMeters.GetPresentDefinitionKeys()), unitsSelection.AbsoluteLength);
            VisibleLength = new(TargetShip.VisibleLengthRadians, GenerateOptions(TargetShip.VisibleLengthRadians.GetPresentDefinitionKeys()), unitsSelection.VisibleLength);

            ArbitraryAbsoluteHeightMeters = AbsoluteHeight.Parameter.AllDefinitions.OfType<ArbitraryValueParameterDefinition<float>>().First();
            ArbitraryAbsoluteLengthMeters = AbsoluteLength.Parameter.AllDefinitions.OfType<ArbitraryValueParameterDefinition<float>>().First();

            TargetRange = new(TargetShip.TargetRangeMeters, GenerateOptions(TargetShip.TargetRangeMeters.GetPresentDefinitionKeys()), unitsSelection.TargetRange);

            AoB = new(TargetShip.AoBRadians, GenerateOptions(TargetShip.AoBRadians.GetPresentDefinitionKeys()), unitsSelection.AoB);

            HullTime = new(TargetShip.HullTimeSeconds, GenerateOptions(TargetShip.HullTimeSeconds.GetPresentDefinitionKeys()), unitsSelection.HullTime);
            OneDegreeTime = new(TargetShip.OneDegreeTimeSeconds, GenerateOptions(TargetShip.OneDegreeTimeSeconds.GetPresentDefinitionKeys()), unitsSelection.OneDegreeTime);

            TargetSpeed = new(TargetShip.TargetSpeedMpS, GenerateOptions(TargetShip.TargetSpeedMpS.GetPresentDefinitionKeys()), unitsSelection.TargetSpeed);
            AngularSpeed = new(TargetShip.AngularSpeedRpS, GenerateOptions(TargetShip.AngularSpeedRpS.GetPresentDefinitionKeys()), unitsSelection.AngularSpeed);

            TorpedoSpeed = new(TargetShip.TorpedoSpeedMpS, GenerateStringOptions(TargetShip.TorpedoSpeedMpS.GetPresentDefinitionKeys()), unitsSelection.TorpedoSpeed);

            BoatSpeed = new(TargetShip.BoatSpeedMpS, GenerateOptions(TargetShip.BoatSpeedMpS.GetPresentDefinitionKeys()), unitsSelection.BoatSpeed);

            LeadAngle = new(TargetShip.LeadAngleRadians, GenerateOptions(TargetShip.LeadAngleRadians.GetPresentDefinitionKeys()), unitsSelection.LeadAngle);
        }
        #endregion

        #region Methods
        private static IEnumerable<Option<TEnumKey>> GenerateOptions<TEnumKey>(IEnumerable<TEnumKey> keys)
            where TEnumKey : struct, Enum
        {
            return keys.Select(GenerateOption);
        }

        private static Option<TEnumKey> GenerateOption<TEnumKey>(TEnumKey key)
            where TEnumKey : struct, Enum
        {
            return new(key,
                Enum.GetName(key) ?? ((int)((object)key)).ToString());
        }

        private static IEnumerable<Option<string>> GenerateStringOptions(IEnumerable<string> keys)
        {
            return keys.Select(key => new Option<string>(key, key));
        }
        #endregion
    }
}
