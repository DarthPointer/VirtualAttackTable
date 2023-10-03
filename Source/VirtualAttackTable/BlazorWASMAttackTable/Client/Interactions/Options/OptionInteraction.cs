using CallbackList;

namespace BlazorWASMAttackTable.Client.Interactions.Options
{
    public class OptionInteraction<TOption> : IOptionInteraction<TOption>
    {
        #region Fields
        private readonly Option<TOption> _option;
        #endregion

        #region Properties
        public bool IsSelected
        {
            get;
            private set;
        }

        public Option<TOption> Option => _option;

        public TOption Value => _option.Value;

        public string OptionName => _option.OptionName;
        public string OptionDescription => _option.OptionDescription;

        private Action ToggleCall
        {
            get;
            init;
        }

        public CallbackListManager StateChanged
        {
            get;
        } = new();

        private ListOfOptions<TOption> OwningList { get; }
        #endregion

        #region Constructors
        public OptionInteraction(Option<TOption> option, Action<OptionInteraction<TOption>> toggleCall, ListOfOptions<TOption> owningList)
        {
            _option = option;

            ToggleCall = () => toggleCall(this);

            IsSelected = false;

            OwningList = owningList;
        }
        #endregion

        #region Methods
        public void Toggle()
        {
            ToggleCall();
        }

        public void SetIsSelected(bool isSelected)
        {
            IsSelected = isSelected;
            StateChanged.CreateFireCall()?.Invoke();
        }

        public void Preview(bool preview)
        {
            OwningList.PreviewOption(Option, preview);
        }
        #endregion
    }

    public interface IOptionInteraction
    {
        public string OptionName { get; }
        public string OptionDescription { get; }
        public bool IsSelected { get; }
        void Toggle();
        void Preview(bool preview);

        //event Action? StateChanged;
        CallbackListManager StateChanged { get; }

        public void SetIsSelected(bool isSelected);
    }

    public interface IOptionInteraction<out TOption> : IOptionInteraction
    {
        TOption Value { get; }
    }
}
