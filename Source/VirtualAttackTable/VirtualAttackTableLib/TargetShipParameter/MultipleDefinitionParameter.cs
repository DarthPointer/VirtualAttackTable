using CallbackList;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAttackTableLib.TargetShipParameter
{
    public class MultipleDefinitionParameter<TDefinitionKey, TValue> : IParameter
        where TDefinitionKey : notnull
    {
        #region Fields
        [DisallowNull]
        private TDefinitionKey? _activeDefinitionKey = default!;
        private ParameterDefinition<TValue> _activeDefinition;
        #endregion

        #region Properties
        [DisallowNull]
        public TDefinitionKey? ActiveDefinitionKey
        {
            get
            {
                return _activeDefinitionKey;
            }
            set
            {
                if (!value.Equals(_activeDefinitionKey) && Definitions.ContainsKey(value))
                {
                    _activeDefinitionKey = value;
                    ActiveDefinition = Definitions[value];
                }
            }
        }

        private ISubscriptionHandle? ActiveDefinitionValueChangedSubscription { get; set; }

        public ParameterDefinition<TValue> ActiveDefinition
        {
            get
            {
                return _activeDefinition;
            }
            private set
            {
                if (_activeDefinition != value)
                {
                    ActiveDefinitionValueChangedSubscription?.Unsubscribe();
                    //_activeDefinition.StateOrValueChanged -= NotifyParameterChanged;

                    _activeDefinition = value;
                    ActiveDefinitionValueChangedSubscription = value.StateOrValueChanged.Subscribe(NotifyParameterChanged);
                    //value.StateOrValueChanged += NotifyParameterChanged;
                    NotifyParameterChanged();
                }
            }
        }

        public TValue CurrentValue => ActiveDefinition.CurrentValue;

        public ParameterDefinitionState CurrentState => ActiveDefinition.CurrentState;

        private Dictionary<TDefinitionKey, ParameterDefinition<TValue>> Definitions
        {
            get;
            init;
        } = new();

        public IReadOnlyDictionary<TDefinitionKey, ParameterDefinition<TValue>> AllDefinitions => Definitions;

        /// <summary>
        /// Fired when a different <see cref="ActiveDefinition"/> is set or its <see cref="ParameterDefinition{TValue}.CurrentState"/> or <see cref="ParameterDefinition{TValue}.CurrentValue"/> are changed.
        /// </summary>
        public CallbackListManager ParameterChanged
        {
            get;
        } = new();
        //public event Action? ParameterChanged;
        #endregion

        #region Constructors
        public MultipleDefinitionParameter()
        {
            _activeDefinition = new(() => default!, this);
        }
        #endregion

        #region Methods
        public IEnumerable<IParameter> GetDependencyParameters()
        {
            return ActiveDefinition.DependencyParameters;
        }

        public ParameterDefinition<TValue> AddDefinition(TDefinitionKey key, Func<TValue> formula, IEnumerable<IParameter>? dependencyParameters = null, TValue fallbackValue = default!)
        {
            return AddDefinition(key, new(formula, this, dependencyParameters, fallbackValue));
        }

        public ParameterDefinition<TValue> AddDefinition(TDefinitionKey key, ParameterDefinition<TValue> parameterDefinition)
        {
            if (Definitions.ContainsKey(key))
                throw new ArgumentException($"{GetType()} already contains a definition by key {key}", nameof(key));

            bool firstDefinition = Definitions.Count == 0;

            Definitions.Add(key, parameterDefinition);

            if (firstDefinition)
            {
                ActiveDefinitionKey = key;
                ActiveDefinition = parameterDefinition;
            }

            return parameterDefinition;
        }

        public IEnumerable<TDefinitionKey> GetPresentDefinitionKeys() => Definitions.Keys;

        public void SetArbitraryValue(float value)
        {
            if (Definitions.Values.OfType<ArbitraryValueParameterDefinition<float>>().FirstOrDefault() is ArbitraryValueParameterDefinition<float> arbitraryDefinition)
            {
                arbitraryDefinition.Value = value;
            }
        }

        protected void NotifyParameterChanged()
        {
            ParameterChanged.CreateFireCall()?.Invoke();
        }
        #endregion
    }
}
