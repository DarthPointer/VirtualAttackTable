using BlazorWASMAttackTable.Client.Interactions.Options;
using VirtualAttackTableLib.AttackTarget;
using VirtualAttackTableLib.TargetShipParameter;

namespace BlazorWASMAttackTable.Client.Interactions.AttackTableInteractions
{
    public class TargetShipAlteredUnitsWrap
    {
        #region Definition Descriptions
        private readonly Dictionary<AbsoluteHeightDefinition, string> _absoluteHeightDefinitionDescriptions = new() { { AbsoluteHeightDefinition.Maximum, "Height between the waterline and highest tip of mast/kingpost."} };
        private readonly Dictionary<AbsoluteLengthDefinition, string> _absoluteLengthDefinitionDescriptions = new() { { AbsoluteLengthDefinition.Maximum, "Full length of a ship (between tips of bow and stern)."} };

        private readonly Dictionary<TargetRangeDefinition, string> _targetRangeDefinitionDescriptions = new() { { TargetRangeDefinition.ByVisibleHight, "Compares visible angular and absolute height between reference points." } };

        private readonly Dictionary<AoBDefinition, string> _aoBDefinitionDescriptions = new() { { AoBDefinition.ByRangeAndVisibleLength, "Compares visible angular and absolute length between reference points with target range." } };

        private readonly Dictionary<TargetSpeedDefinition, string> _targetSpeedDefinitionDescriptions = new() {
            { TargetSpeedDefinition.Linear, "Based on specified absolute length between reference points and time for the target's hull to travel that distance (\"Hull Time\"). Assumes the boat is static or moves along the used line of sight." },
            { TargetSpeedDefinition.Angular, "Based on target range, angular speed and AoB" },
            { TargetSpeedDefinition.LinearConstantBoatVelocity, "Depends on Bearing, AoB, Absolute Length, Hull Time and Boat Speed. Assumes the Boat to run at constant course and speed." } };

        private readonly Dictionary<AngularSpeedDefinition, string> _angularSpeedDefinitionDescriptions = new() { { AngularSpeedDefinition.ByOneDegreeTime, "Approximates the angular speed of target relative to the Boat based on its \"One Degree Time\"" } };

        private readonly Dictionary<LeadAngleDefinition, string> _leadAngleDefinitionDescriptions = new() { { LeadAngleDefinition.ByAngularSpeed, "APPROXIMATION. Based on Target Range, Angular Speed and Torpedo Speed. This is a simplified formula that assumes AoB of 90 degrees and 0 Gyroangle and will be highly inaccurate at sharp AoBs and Launch Angles." } };
        #endregion

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

            AbsoluteHeight = new(TargetShip.AbsoluteHeightMeters, GenerateOptions(TargetShip.AbsoluteHeightMeters.GetPresentDefinitionKeys(), _absoluteHeightDefinitionDescriptions), unitsSelection.AbsoluteHeight);
            VisibleHeight = new(TargetShip.VisibleHeightRadians, GenerateOptions(TargetShip.VisibleHeightRadians.GetPresentDefinitionKeys()), unitsSelection.VisibleHeight);
            AbsoluteLength = new(TargetShip.AbsoluteLengthMeters, GenerateOptions(TargetShip.AbsoluteLengthMeters.GetPresentDefinitionKeys(), _absoluteLengthDefinitionDescriptions), unitsSelection.AbsoluteLength);
            VisibleLength = new(TargetShip.VisibleLengthRadians, GenerateOptions(TargetShip.VisibleLengthRadians.GetPresentDefinitionKeys()), unitsSelection.VisibleLength);

            ArbitraryAbsoluteHeightMeters = AbsoluteHeight.Parameter.AllDefinitions.Values.OfType<ArbitraryValueParameterDefinition<float>>().First();
            ArbitraryAbsoluteLengthMeters = AbsoluteLength.Parameter.AllDefinitions.Values.OfType<ArbitraryValueParameterDefinition<float>>().First();

            TargetRange = new(TargetShip.TargetRangeMeters, GenerateOptions(TargetShip.TargetRangeMeters.GetPresentDefinitionKeys(), _targetRangeDefinitionDescriptions), unitsSelection.TargetRange);

            AoB = new(TargetShip.AoBRadians, GenerateOptions(TargetShip.AoBRadians.GetPresentDefinitionKeys(), _aoBDefinitionDescriptions), unitsSelection.AoB);

            HullTime = new(TargetShip.HullTimeSeconds, GenerateOptions(TargetShip.HullTimeSeconds.GetPresentDefinitionKeys()), unitsSelection.HullTime);
            OneDegreeTime = new(TargetShip.OneDegreeTimeSeconds, GenerateOptions(TargetShip.OneDegreeTimeSeconds.GetPresentDefinitionKeys()), unitsSelection.OneDegreeTime);

            TargetSpeed = new(TargetShip.TargetSpeedMpS, GenerateOptions(TargetShip.TargetSpeedMpS.GetPresentDefinitionKeys(), _targetSpeedDefinitionDescriptions), unitsSelection.TargetSpeed);
            AngularSpeed = new(TargetShip.AngularSpeedRpS, GenerateOptions(TargetShip.AngularSpeedRpS.GetPresentDefinitionKeys(), _angularSpeedDefinitionDescriptions), unitsSelection.AngularTargetSpeed);

            TorpedoSpeed = new(TargetShip.TorpedoSpeedMpS, GenerateStringOptions(TargetShip.TorpedoSpeedMpS.GetPresentDefinitionKeys()), unitsSelection.TorpedoSpeed);

            BoatSpeed = new(TargetShip.BoatSpeedMpS, GenerateOptions(TargetShip.BoatSpeedMpS.GetPresentDefinitionKeys()), unitsSelection.BoatSpeed);

            LeadAngle = new(TargetShip.LeadAngleRadians, GenerateOptions(TargetShip.LeadAngleRadians.GetPresentDefinitionKeys(), _leadAngleDefinitionDescriptions), unitsSelection.LeadAngle);
        }
        #endregion

        #region Methods
        private static IEnumerable<Option<TEnumKey>> GenerateOptions<TEnumKey>(IEnumerable<TEnumKey> keys, Dictionary<TEnumKey, string>? descriptions = null)
            where TEnumKey : struct, Enum
        {
            return keys.Select(option => GenerateOption(option, descriptions));
        }

        private static Option<TEnumKey> GenerateOption<TEnumKey>(TEnumKey key, Dictionary<TEnumKey, string>? descriptions = null)
            where TEnumKey : struct, Enum
        {
            string description = "";

            if (descriptions?.TryGetValue(key, out string? mayBeDesc) == true)
            {
                description = mayBeDesc;
            }

            return new(key,
                Enum.GetName(key) ?? ((int)((object)key)).ToString(),
                description);
        }

        private static IEnumerable<Option<string>> GenerateStringOptions(IEnumerable<string> keys)
        {
            return keys.Select(key => new Option<string>(key, key));
        }
        #endregion
    }
}
