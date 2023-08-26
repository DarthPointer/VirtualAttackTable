using BlazorWASMAttackTable.Client.Interactions.Common;
using CallbackList;
using System.Reflection;

namespace BlazorWASMAttackTable.Client.Interactions.Options
{
    public interface IListForSingleOption : IListOfOptions
    {
        bool AllowNoOption { get; }

        IReadOnlyValueInteraction<IOption?> SelectedOption { get; }

        void SelectNoOption();
    }

    public interface IListForSingleOption<out TOption> : IListForSingleOption
    {
        new IReadOnlyValueInteraction<IOption<TOption>?> SelectedOption { get; }

        IReadOnlyValueInteraction<IOption?> IListForSingleOption.SelectedOption => SelectedOption;
    }

    public class ListForSingleOption<TOption> : ListOfOptions<TOption>, IListForSingleOption<TOption>
        where TOption : notnull
    {
        #region Fields
        private OptionInteraction<TOption>? _selectedOptionInteraction;
        #endregion

        #region Properties
        public IReadOnlyValueInteraction<IOption<TOption>?> SelectedOption => SelectedOptionInternal;

        private ValueInteraction<Option<TOption>?> SelectedOptionInternal { get; } = new(null);

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
                    SelectedOptionInternal.Value = _selectedOptionInteraction?.Option;
                }
            }
        }

        public bool AllowNoOption
        {
            get;
            private init;
        }

        public ValueInteraction<Option<TOption>?> PreviewedOption { get; } = new(null);
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
                    throw new ArgumentException($"{nameof(ListForSingleOption<TOption>)} can't be constructed with {nameof(allowNoOption)} = false if it is created with no options ({nameof(Options)} had 0 elements).", nameof(allowNoOption));

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
                SelectedOptionInternal.Value = null;

                return;
            }

            //if (!toggledOption.IsSelected)
            foreach (OptionInteraction<TOption> option in Options)
            {
                option.SetIsSelected(option == toggledOption);
            }

            SelectedOptionInternal.Value = toggledOption.Option;
        }

        public void SelectNoOption()
        {
            if (AllowNoOption)
            {
                SelectedOptionInternal.Value = null;
            }
        }

        public override void PreviewOption(Option<TOption> option, bool preview)
        {
            if (preview)
            {
                PreviewedOption.Value = option;
                return;
            }

            if (!preview && PreviewedOption.Value == option)
            {
                PreviewedOption.Value = null;
                return;
            }
        }
        #endregion
    }
}
