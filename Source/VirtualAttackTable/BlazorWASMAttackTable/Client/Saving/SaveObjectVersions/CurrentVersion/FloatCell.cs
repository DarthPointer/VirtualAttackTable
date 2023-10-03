namespace BlazorWASMAttackTable.Client.Saving.SaveObjectVersions.CurrentVersion
{
    public struct FloatCell<TDefinitionKey>
        where TDefinitionKey : notnull
    {
        public TDefinitionKey ActiveDefinitionKey { get; set; }
        
        public float ArbitraryValue { get; set; }
    }
}
