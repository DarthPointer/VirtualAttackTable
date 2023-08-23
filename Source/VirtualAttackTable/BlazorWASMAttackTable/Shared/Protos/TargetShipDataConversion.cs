using VirtualAttackTableLib.AttackTarget;
using VirtualAttackTableLib.TargetIdentification;

namespace BlazorWASMAttackTable.Shared.Protos
{
    public static class TargetShipDataConversion
    {
        public static TargetShipData ToData(this TargetShipdDataLoadElement element)
        {
            return new TargetShipData()
            {
                GRT = element.GRT,

                MaxHeightMeters = element.MaxHeightMeters,
                VerticalImageRange = element.VerticalImageRange.ToFloatRange(),
                LengthMeters = element.LengthMeters,
                HorizontalImageRange = element.HorizontalImageRange.ToFloatRange(),

                DraughtMeters = element.DraughtMeters,

                MaxSpeedMpS = element.MaxSpeedMpS,

                TypeName = element.TypeName,

                EnginePlacement = (EnginePlacement)element.EnginePlacement,
                IslandsPositions = (IslandsPositions)element.IslandsPositions,
                Superstructure = (Superstructure)element.Superstructure,

                Structures = element.Structures.Select(code => (StructureType)code).ToList()
            };
        }

        public static TargetShipdDataLoadElement ToBuffer(this TargetShipData targetShipData)
        {
            TargetShipdDataLoadElement result = new()
            {
                GRT = targetShipData.GRT,

                MaxHeightMeters = targetShipData.MaxHeightMeters,
                VerticalImageRange = targetShipData.VerticalImageRange.ToBuffer(),
                LengthMeters = targetShipData.LengthMeters,
                HorizontalImageRange = targetShipData.HorizontalImageRange.ToBuffer(),

                DraughtMeters = targetShipData.DraughtMeters,

                MaxSpeedMpS = targetShipData.MaxSpeedMpS,

                TypeName = targetShipData.TypeName,

                EnginePlacement = (int)targetShipData.EnginePlacement,
                IslandsPositions = (int)targetShipData.IslandsPositions,
                Superstructure = (int)targetShipData.Superstructure,
            };
            result.Structures.Add(targetShipData.Structures.Select(structure => (int)structure));

            return result;
        }
    }

    public static class FloatRangeConversion
    {
        public static VirtualAttackTableLib.FloatRange ToFloatRange(this FloatRange floatRange)
        {
            return new() { Start = floatRange.Min, End = floatRange.Max };
        }

        public static FloatRange ToBuffer(this VirtualAttackTableLib.FloatRange floatRange)
        {
            return new() { Min = floatRange.Start, Max = floatRange.End };
        }
    }
}
