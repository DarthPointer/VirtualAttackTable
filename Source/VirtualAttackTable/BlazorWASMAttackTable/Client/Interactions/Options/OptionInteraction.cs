using CallbackList;

namespace BlazorWASMAttackTable.Client.Interactions.Options
{
    public class OptionInteraction<TOption> : IOptionInteraction
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
        //public event Action? StateChanged;
        #endregion

        #region Constructors
        public OptionInteraction(Option<TOption> option, Action<OptionInteraction<TOption>> toggleCall)
        {
            _option = option;

            ToggleCall = () => toggleCall(this);

            IsSelected = false;
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
        #endregion
    }

    public interface IOptionInteraction
    {
        public string OptionName { get; }
        public string OptionDescription { get; }
        public bool IsSelected { get; }
        void Toggle();

        //event Action? StateChanged;
        CallbackListManager StateChanged { get; }
    }
}
