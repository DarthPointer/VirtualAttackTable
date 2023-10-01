using VirtualAttackTableLib.AttackTarget;

namespace BlazorWASMAttackTable.Client.Saving.SaveObjectVersions.CurrentVersion
{
    public class TargetShipEntry
    {
        public string TypeName { get; set; } = string.Empty;

        public FloatCell<BearingDefinition> Bearing { get; set; }

        public FloatCell<AbsoluteHeightDefinition> AbsoluteHeight { get; set; }
        public FloatCell<VisibleHeightDefinition> VisibleHeight { get; set; }
        public FloatCell<AbsoluteLengthDefinition> AbsoluteLength { get; set; }
        public FloatCell<VisibleLengthDefinition> VisibleLength { get; set; }

        public FloatCell<TargetRangeDefinition> TargetRange { get; set; }

        public FloatCell<AoBDefinition> AoB { get; set; }
        public AoBQuarter AoBQuarter { get; set; }

        public FloatCell<HullTimeDefinition> HullTime { get; set; }
        public FloatCell<OneDegreeTimeDefinition> OneDegreeTime { get; set; }

        public FloatCell<TargetSpeedDefinition> TargetSpeed { get; set; }
        public FloatCell<AngularSpeedDefinition> AngularSpeed { get; set; }

        public FloatCell<string> TorpedoSpeed { get; set; }

        public FloatCell<BoatSpeedDefinition> BoatSpeed { get; set; }

        public FloatCell<LeadAngleDefinition> LeadAngle { get; set; }
    }
}
