using CallbackList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAttackTableLib.TargetShipParameter
{
    public interface IParameterDefinition
    {
        void Update();

        IReadOnlyList<IParameter> DependencyParameters { get; }
    }

    public class ParameterDefinition<TValue> : IParameterDefinition
    {
        #region Fields
        #endregion

        #region Properties
        protected Func<TValue> Formula
        {
            get;
            init;
        }

        public IReadOnlyList<IParameter> DependencyParameters
        {
            get;
            private init;
        }

        private TValue FallbackValue
        {
            get;
            init;
        }

        private IParameter OwningParameter
        {
            get;
            init;
        }

        public TValue CurrentValue
        {
            get;
            private set;
        } = default!;

        public ParameterDefinitionState CurrentState
        {
            get;
            private set;
        }

        public CallbackListManager StateOrValueChanged
        {
            get;
        } = new();
        //public event Action? StateOrValueChanged;
        #endregion

        #region Constructors
        public ParameterDefinition(Func<TValue> formula, IParameter owningParameter, IEnumerable<IParameter>? dependencyParameters = null, TValue fallbackValue = default!) :
            this(owningParameter, dependencyParameters, fallbackValue)
        {
            Formula = formula;
        }

        /// <summary>
        /// Ctor for use by inheriting classes that want to set custom fixed <see cref="Formula"/> and avoid explicit boilerplate for "formula" parameter of the public ctor.
        /// </summary>
        /// <param name="owningParameter"></param>
        /// <param name="dependencyParameters"></param>
        /// <param name="fallbackValue"></param>
        protected ParameterDefinition(IParameter owningParameter, IEnumerable<IParameter>? dependencyParameters = null, TValue fallbackValue = default!)
        {
            Formula = () => default!;

            OwningParameter = owningParameter;
            DependencyParameters = (dependencyParameters ?? Enumerable.Empty<IParameter>()).ToList();
            FallbackValue = fallbackValue;

            foreach (IParameter dependencyParameter in DependencyParameters)
            {
                dependencyParameter.ParameterChanged.Subscribe(Update);
                //dependencyParameter.ParameterChanged += Update;
            }
        }
        #endregion

        #region Methods
        private void SetStateAndValue(ParameterDefinitionState newState, TValue newValue)
        {
            bool stateOrValueChanged = (CurrentState != newState) || (!Equals(CurrentValue, newValue));

            CurrentValue = newValue;
            CurrentState = newState;

            if (stateOrValueChanged) StateOrValueChanged.CreateFireCall()?.Invoke();
        }

        public void Update()
        {
            try
            {
                DefinitionCustomPreUpdate();

                ParameterDefinitionState newState = CheckForSelfLoop();
                TValue newValue = newState switch
                {
                    ParameterDefinitionState.Valid => Formula(),
                    ParameterDefinitionState.DependencyLoop => FallbackValue,

                    _ => throw new Exception($"{newState} was not expected as the result of a self loop check.")
                };

                SetStateAndValue(newState, newValue);

                return;
            }
            catch
            {
                SetStateAndValue(ParameterDefinitionState.Exception, FallbackValue);
                return;
            }
        }

        protected virtual void DefinitionCustomPreUpdate()
        { }

        private ParameterDefinitionState CheckForSelfLoop()
        {
            HashSet<IParameter> loopParameters = new(){ OwningParameter };

            foreach (IParameter dependencyParameter in DependencyParameters)
            {
                if (CheckForSelfLoop(dependencyParameter, loopParameters) == ParameterDefinitionState.DependencyLoop)
                    return ParameterDefinitionState.DependencyLoop;
            }

            return ParameterDefinitionState.Valid;
        }

        private ParameterDefinitionState CheckForSelfLoop(IParameter parameterToCheck, HashSet<IParameter> loopParameters)
        {
            if (loopParameters.Contains(parameterToCheck)) return ParameterDefinitionState.DependencyLoop;

            loopParameters.Add(parameterToCheck);

            foreach (IParameter dependencyParameter in parameterToCheck.GetDependencyParameters())
            {
                if (CheckForSelfLoop(dependencyParameter, loopParameters) == ParameterDefinitionState.DependencyLoop)
                    return ParameterDefinitionState.DependencyLoop;
            }

            loopParameters.Remove(parameterToCheck);

            return ParameterDefinitionState.Valid;
        }
        #endregion
    }
}
