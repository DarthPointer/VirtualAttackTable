using BlazorWASMAttackTable.Client.Elements.SubscriptionDisposal;
using BlazorWASMAttackTable.Client.Interactions.AttackTableInteractions;
using Microsoft.AspNetCore.Components;
using VirtualAttackTableLib.TargetShipParameter;

namespace BlazorWASMAttackTable.Client.Elements.AttackTableElements
{
    public partial class FloatParameterCell<TParameter, TDefinitionKey> : SubscriptionDisposingElement
        where TParameter : MultipleDefinitionParameter<TDefinitionKey, float>
        where TDefinitionKey : notnull
    {
        #region Fields
        private bool _badInput = false;
        private string _displayValue = "";
        #endregion

        #region Properties
        [Parameter, EditorRequired]
        public AlteredUnitParameterInteraction<TParameter, TDefinitionKey> Interaction { get; set; } = null!;

        private string DisplayValue
        {
            get
            {
                return _displayValue;
            }
            set
            {
                if (_displayValue != value && Interaction.CanSetArbitraryValue)
                {
                    bool inputParsed = float.TryParse(value, out float floatVal);
                    if (inputParsed)
                    {
                        Interaction.SetArbitraryValue(floatVal);
                    }

                    BadInput = !inputParsed;
                    _displayValue = value;
                }
            }
        }

        private bool BadInput
        {
            get
            {
                return _badInput;
            }
            set
            {
                if (_badInput != value)
                {
                    _badInput = value;
                    StateHasChanged();
                }
            }
        }
        #endregion

        #region Methods
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            Subscribe(Interaction.ParameterChanged, OnParameterChanged);
            OnParameterChanged();
        }

        

        private void OnParameterChanged()
        {
            BadInput = false;
            _displayValue = BlazorAttackTableLib.CustomUnitValueFormat(Interaction.CurrentValue);

            StateHasChanged();
        }
        #endregion
    }
}
