using BlazorWASMAttackTable.Client.Interactions.Common;

namespace BlazorWASMAttackTable.Client.Interactions.AttackTableInteractions
{
    public class CollapseColumnsFlags
    {
        public ValueInteraction<bool> CollapseBearing { get; } = new(false);
        public ValueInteraction<bool> CollapseAbsoluteHeight { get; } = new(false);
        public ValueInteraction<bool> CollapseVisibleHeight { get; } = new(false);
        public ValueInteraction<bool> CollapseAbsoluteLength { get; } = new(false);
        public ValueInteraction<bool> CollapseVisibleLength { get; } = new(false);
        public ValueInteraction<bool> CollapseTargetRange { get; } = new(false);
        public ValueInteraction<bool> CollapseAoB { get; } = new(false);
        public ValueInteraction<bool> CollapseHullTime { get; } = new(false);
        public ValueInteraction<bool> CollapseOneDegreeTime { get; } = new(false);
        public ValueInteraction<bool> CollapseTargetSpeed { get; } = new(false);
        public ValueInteraction<bool> CollapseAngularTargetSpeed { get; } = new(false);
        public ValueInteraction<bool> CollapseTorpedoSpeed { get; } = new(false);
        public ValueInteraction<bool> CollapseBoatSpeed { get; } = new(false);
        public ValueInteraction<bool> CollapseLeadAngle { get; } = new(false);
    }
}
