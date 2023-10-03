namespace BlazorWASMAttackTable.Client.Saving.SaveObjectVersions.CurrentVersion
{
    public class Session
    {
        public List<IdentificationState> IdentificationTabs { get; set; } = new();

        public ParameterUnits ParameterUnits { get; set; } = new();

        public List<TargetShipEntry> TargetShipEntries { get; set; } = new();
    }
}
