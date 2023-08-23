namespace BlazorWASMAttackTable.Client.Interactions.Options
{
    public interface IOption
    {
        string OptionName { get; }
    }

    public interface IOption<out TOption> : IOption
    {
        TOption Value { get; }
    }

    public class Option<TOption> : IOption<TOption>
    {
        public TOption Value { get; private init; }

        public string OptionName { get; private init; }

        public Option(TOption value, string optionName)
        {
            Value = value;
            OptionName = optionName;
        }
    }
}
