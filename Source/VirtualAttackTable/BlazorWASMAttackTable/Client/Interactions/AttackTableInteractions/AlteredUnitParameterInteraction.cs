using BlazorWASMAttackTable.Client.Interactions.Options;
using CallbackList;
using VirtualAttackTableLib;
using VirtualAttackTableLib.TargetShipParameter;

namespace BlazorWASMAttackTable.Client.Interactions.AttackTableInteractions
{
    public class AlteredUnitParameterInteraction<TParameter, TDefinitionKey>
        where TParameter : MultipleDefinitionParameter<TDefinitionKey, float>
        where TDefinitionKey : notnull
    {
        #region Fields
        private float _baseUnitValue;
        private TDefinitionKey? _activeDefinitionKey;
        private ParameterDefinitionState _definitionState;
        #endregion

        #region Properties
        private IListForSingleOption<IUnit> AlteredUnitSource { get; }
        protected IUnit CurrentUnit => AlteredUnitSource.SelectedOption.Value?.Value ??
            throw (new InvalidOperationException($"{GetType()} can not get current unit because " +
                $"its unit source has {nameof(AlteredUnitSource.SelectedOption)} of null."));
        public TParameter Parameter { get; }

        public bool CanSetArbitraryValue => CurrentArbitraryDefinition != null;
        public virtual float CurrentValue => CurrentUnit.FromBase(Parameter.CurrentValue);

        public ListForSingleOption<TDefinitionKey> DefinitionKeySelection { get; }

        private ArbitraryValueParameterDefinition<float>? CurrentArbitraryDefinition => Parameter.ActiveDefinition as ArbitraryValueParameterDefinition<float>;

        private float BaseUnitValue
        {
            get
            {
                return _baseUnitValue;
            }
            set
            {
                if (_baseUnitValue != value)
                {
                    _baseUnitValue = value;
                    NotifyParameterChanged();
                }
            }
        }

        private TDefinitionKey? ActiveDefinitionKey
        {
            get
            {
                return _activeDefinitionKey;
            }
            set
            {
                if (!Equals(_activeDefinitionKey, value))
                {
                    _activeDefinitionKey = value;
                    NotifyParameterChanged();
                }
            }
        }

        private ParameterDefinitionState DefinitionState
        {
            get
            {
                return _definitionState;
            }
            set
            {
                if (_definitionState != value)
                {
                    _definitionState = value;
                    NotifyParameterChanged();
                }
            }
        }

        public CallbackListManager ParameterChanged { get; } = new();
        #endregion

        #region Constructors
        public AlteredUnitParameterInteraction(TParameter parameter,
            IEnumerable<Option<TDefinitionKey>> options, IListForSingleOption<IUnit> unitSource)
        {
            AlteredUnitSource = unitSource;
            AlteredUnitSource.SelectedOption.ValueChanged.Subscribe(OnUnitChanged);

            Parameter = parameter;
            Parameter.ParameterChanged.Subscribe(OnParameterChanged);

            DefinitionKeySelection = new(options, "Definition");
            DefinitionKeySelection.Options.FirstOrDefault(opt => opt.Value.Equals(parameter.ActiveDefinitionKey))?.Toggle();
            DefinitionKeySelection.SelectedOption.ValueChanged.Subscribe(SetDefinitionKey);
        }
        #endregion

        #region Methods
        public virtual void SetArbitraryValue(float value)
        {
            if (CanSetArbitraryValue)
            {
                SetArbitraryBaseUnitsValue(CurrentUnit.ToBase(value));
            }
        }

        protected void SetArbitraryBaseUnitsValue(float baseUnits)
        {
            if (CanSetArbitraryValue)
            {
                _baseUnitValue = baseUnits;
                CurrentArbitraryDefinition!.Value = _baseUnitValue;
            }
        }

        private void SetDefinitionKey(IOption<TDefinitionKey>? _)
        {
            Parameter.ActiveDefinitionKey = DefinitionKeySelection.SelectedOption.Value!.Value;
        }

        private void OnUnitChanged(IOption<IUnit>? _)
        {
            NotifyParameterChanged();
        }

        private void OnParameterChanged()
        {
            BaseUnitValue = Parameter.CurrentValue;
            ActiveDefinitionKey = Parameter.ActiveDefinitionKey;
            DefinitionState = Parameter.CurrentState;
        }

        private void NotifyParameterChanged() => ParameterChanged.CreateFireCall()?.Invoke();
        #endregion
    }

    public class AlteredUnitParameterInteraction<TDefinitionKey> : AlteredUnitParameterInteraction<MultipleDefinitionParameter<TDefinitionKey, float>, TDefinitionKey>
        where TDefinitionKey : notnull
    {
        public AlteredUnitParameterInteraction(MultipleDefinitionParameter<TDefinitionKey, float> parameter, IEnumerable<Option<TDefinitionKey>> options, IListForSingleOption<IUnit> unitSource) : base(parameter, options, unitSource)
        { }
    }
}
