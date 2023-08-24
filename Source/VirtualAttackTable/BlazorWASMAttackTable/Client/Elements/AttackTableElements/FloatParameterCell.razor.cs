using BlazorWASMAttackTable.Client.Elements.SubscriptionDisposal;
using BlazorWASMAttackTable.Client.Interactions.AttackTableInteractions;
using Microsoft.AspNetCore.Components;
using VirtualAttackTableLib.TargetShipParameter;

namespace BlazorWASMAttackTable.Client.Elements.AttackTableElements
{
    public partial class FloatParameterCell<TParameter, TDefinitionKey>
        where TParameter : MultipleDefinitionParameter<TDefinitionKey, float>
        where TDefinitionKey : notnull
    {
        #region Fields
        private bool _badInput = false;
        private string _displayValue = "";

        private bool _highlight = false;
        #endregion

        #region Properties
        [Parameter, EditorRequired]
        public AlteredUnitParameterInteraction<TParameter, TDefinitionKey> Interaction { get; set; } = null!;

        [Parameter, EditorRequired]
        public AttackTableShipEntry OwningEntry { get; set; } = null!;

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

        private bool Highlight
        {
            get
            {
                return _highlight;
            }
            set
            {
                if (_highlight != value)
                {
                    _highlight = value;
                    StateHasChanged();
                }
            }
        }

        private bool HighlightingParametersForActiveDefinition { get; set; } = false;
        #endregion

        #region Methods
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            Subscribe(Interaction.ParameterChanged, OnParameterChanged);
            Subscribe(OwningEntry.ParametersToHighlight.ValueChanged, OnOwningEntryParametersToHighlightChanged);
            OnParameterChanged();
            OnOwningEntryParametersToHighlightChanged(OwningEntry.ParametersToHighlight.Value);
        }

        private void OnOwningEntryParametersToHighlightChanged(IReadOnlySet<IParameter> parametersToHighlight)
        {
            Highlight = parametersToHighlight.Contains(Interaction.Parameter);
        }

        private void OnParameterChanged()
        {
            BadInput = false;
            _displayValue = BlazorAttackTableLib.CustomUnitValueFormat(Interaction.CurrentValue);

            if (HighlightingParametersForActiveDefinition)
                OwningEntry.DefinitionToHighlightParameters = Interaction.Parameter.ActiveDefinition;

            StateHasChanged();
        }

        private void OnMouseEnterThisCell()
        {
            OwningEntry.DefinitionToHighlightParameters = Interaction.Parameter.ActiveDefinition;
            HighlightingParametersForActiveDefinition = true;
        }

        private void OnMouseLeaveThisCell()
        {
            OwningEntry.DefinitionToHighlightParameters = null;
            HighlightingParametersForActiveDefinition = false;
        }
        #endregion
    }
}
