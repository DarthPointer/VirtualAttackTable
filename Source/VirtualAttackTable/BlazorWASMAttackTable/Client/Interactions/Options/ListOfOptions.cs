namespace BlazorWASMAttackTable.Client.Interactions.Options
{
    public abstract class ListOfOptions<TOption> : IListOfOptions
    {
        #region Properties
        public string InteractionHeader
        {
            get;
            init;
        }

        IReadOnlyList<IOptionInteraction> IListOfOptions.Options => Options;

        public IReadOnlyList<OptionInteraction<TOption>> Options
        {
            get;
            private set;
        }
        #endregion

        #region Constructors
        public ListOfOptions(IEnumerable<Option<TOption>> options, string interactionHeader)
        {
            Options = options.Select(option => new OptionInteraction<TOption>(option, ToggleOption)).ToList();
            InteractionHeader = interactionHeader;
        }
        #endregion

        #region Methods
        protected abstract void ToggleOption(OptionInteraction<TOption> option);
        #endregion
    }

    public interface IListOfOptions
    {
        public string InteractionHeader { get; }

        IReadOnlyList<IOptionInteraction> Options { get; }
    }

    public static class ListOfOptions
    {
        public static IEnumerable<Option<TEnum>> GenerateEnumOptions<TEnum>()
            where TEnum : struct, Enum
        {
            return Enum.GetValues<TEnum>().
                Select(value => new Option<TEnum>(value, Enum.GetName(value)!));
        }
    }
}
