namespace BlazorWASMAttackTable.Client.Interactions.Options
{
    public class FakeOptionInteraction : IOption
    {
        public string OptionName { get; }

        public string OptionDescription { get; }

        public Action TriggerAction { get; }

        public FakeOptionInteraction(string optionName, string optionDescription, Action triggerAction)
        {
            OptionName = optionName;
            OptionDescription = optionDescription;
            TriggerAction = triggerAction;
        }
    }
}
