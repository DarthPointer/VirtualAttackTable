namespace BlazorWASMAttackTable.Client.Interactions.Options
{
    public abstract class ListOfOptions<TOption> : IListOfOptions<TOption>
    {
        #region Properties
        public string InteractionHeader
        {
            get;
            init;
        }

        IReadOnlyList<IOptionInteraction<TOption>> IListOfOptions<TOption>.Options => Options;
        

        public IReadOnlyList<OptionInteraction<TOption>> Options { get; }

        public IReadOnlyList<FakeOptionInteraction> FakeOptionInteractions { get; }
        #endregion

        #region Constructors
        public ListOfOptions(IEnumerable<Option<TOption>> options, string interactionHeader, IEnumerable<FakeOptionInteraction>? fakeOptionInteractions = null)
        {
            Options = options.Select(option => new OptionInteraction<TOption>(option, ToggleOption, this)).ToList();
            InteractionHeader = interactionHeader;

            FakeOptionInteractions = fakeOptionInteractions?.ToList() ?? new List<FakeOptionInteraction> { };
        }
        #endregion

        #region Methods
        protected abstract void ToggleOption(OptionInteraction<TOption> option);

        public virtual void PreviewOption(Option<TOption> option, bool preview) { }
        #endregion
    }

    public interface IListOfOptions
    {
        public string InteractionHeader { get; }

        IReadOnlyList<IOptionInteraction> Options { get; }

        IReadOnlyList<FakeOptionInteraction> FakeOptionInteractions { get; }
    }

    public interface IListOfOptions<out TOption> : IListOfOptions
    {
        new IReadOnlyList<IOptionInteraction<TOption>> Options { get; }

        IReadOnlyList<IOptionInteraction> IListOfOptions.Options => Options;
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
