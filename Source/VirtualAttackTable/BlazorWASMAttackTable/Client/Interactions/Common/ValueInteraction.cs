using CallbackList;

namespace BlazorWASMAttackTable.Client.Interactions.Common
{
    public class ValueInteraction<T>
    {
        #region Fields
        private T _value;
        #endregion

        #region Properties
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                SetValue(value);
            }
        }

        public CallbackListManager<T> ValueChanged
        {
            get;
        } = new();
        //public event Action<T>? ValueChanged;
        #endregion

        #region Constructors
        public ValueInteraction(T initialVaue)
        {
            _value = initialVaue;
        }
        #endregion

        #region Methods
        /// <summary>
        /// A method that can be overriden in <see cref="ValueInteraction{T}"/> inherited classes to alter assignment behavior (add constraints, etc).
        /// By default, overwrites the value and fires <see cref="ValueChanged"/> if the new value was different (checked with <see cref="object.Equals(object?, object?)"/>).
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetValue(T value)
        {
            if (!Equals(_value, value))
            {
                _value = value;
                ValueChanged.CreateFireCall()?.Invoke(Value);
            }
        }
        #endregion
    }
}
