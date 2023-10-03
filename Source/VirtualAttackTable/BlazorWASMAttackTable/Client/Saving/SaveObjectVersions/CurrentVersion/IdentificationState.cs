using VirtualAttackTableLib.TargetIdentification;

namespace BlazorWASMAttackTable.Client.Saving.SaveObjectVersions.CurrentVersion
{
    public class IdentificationState
    {
        public EnginePlacement? EnginePlacement { get; set; }
        public Superstructure? Superstructure { get; set; }

        public bool? BowIsland { get; set; }
        public bool? MidIsland { get; set; }
        public bool? SternIsland { get; set; }

        public string StructuresCode { get; set; } = string.Empty;

        public string? SelectedShip { get; set; } = string.Empty;
    }
}
