using CallbackList;

namespace BlazorWASMAttackTable.Client.Interactions.Options
{
    public interface IListForSingleOption : IListOfOptions
    {
        bool AllowNoOption { get; }
        IOption? SelectedOption { get; }

        void SelectNoOption();
    }

    public interface IListForSingleOption<out TOption> : IListForSingleOption
    {
        new IOption<TOption>? SelectedOption { get; }

        IOption? IListForSingleOption.SelectedOption => SelectedOption;

        ISubscribableCallbackListManager<Action<IOption<TOption>?>> SelectedOptionChanged { get; }
    }

    public class ListForSingleOption<TOption> : ListOfOptions<TOption>, IListForSingleOption<TOption>
        where TOption : notnull
    {
        #region Fields
        private OptionInteraction<TOption>? _selectedOptionInteraction;
        private Option<TOption>? _selectedOption;
        #endregion

        #region Properties
        public Option<TOption>? SelectedOption
        {
            get
            {
                return _selectedOption;
            }
            set
            {
                if (_selectedOption != value)
                {
                    _selectedOption = value;
                    SelectedOptionChanged.CreateFireCall()?.Invoke(_selectedOption);
                }
            }
        }

        IOption<TOption>? IListForSingleOption<TOption>.SelectedOption => SelectedOption;

        private OptionInteraction<TOption>? SelectedOptionInteraction
        {
            get
            {
                return _selectedOptionInteraction;
            }
            set
            {
                if (_selectedOptionInteraction != value)
                {
                    _selectedOptionInteraction = value;
                    SelectedOption = _selectedOptionInteraction?.Option;
                }
            }
        }

        public bool AllowNoOption
        {
            get;
            private init;
        }

        public CallbackListManager<Option<TOption>?> SelectedOptionChanged
        {
            get;
        } = new();

        ISubscribableCallbackListManager<Action<IOption<TOption>?>> IListForSingleOption<TOption>.SelectedOptionChanged => SelectedOptionChanged;
        #endregion

        #region Constructors
        public ListForSingleOption(IEnumerable<Option<TOption>> options, string interactionHeader, bool allowNoOption = false) : base(options, interactionHeader)
        {
            AllowNoOption = allowNoOption;

            if (AllowNoOption)
            {
                SelectedOptionInteraction = null;
            }
            else
            {
                if (!Options.Any())
                    throw new ArgumentException($"{nameof(ListForSingleOption<TOption>)} can't be constructed with {nameof(allowNoOption)} if it is created with no options ({nameof(Options)} had 0 elements).", nameof(allowNoOption));

                var firstOption = Options.First();

                SelectedOptionInteraction = firstOption;
                firstOption.SetIsSelected(true);
            }
        }
        #endregion

        #region Methods
        protected override void ToggleOption(OptionInteraction<TOption> toggledOption)
        {
            if (toggledOption.IsSelected)
            {
                // Can't deselect options if having no option selected is not allowed.
                if (!AllowNoOption) return;

                toggledOption.SetIsSelected(false);
                // I trust the docs say it actually is null both for reference types and Nullable<ValueType>.
                SelectedOption = default;

                return;
            }

            //if (!toggledOption.IsSelected)
            foreach (OptionInteraction<TOption> option in Options)
            {
                option.SetIsSelected(option == toggledOption);
            }

            SelectedOption = toggledOption.Option;
        }

        public void SelectNoOption()
        {
            if (AllowNoOption)
            {
                SelectedOption = null;
            }
        }
        #endregion
    }
}
