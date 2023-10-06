using BlazorWASMAttackTable.Client.Elements.SubscriptionDisposal;
using BlazorWASMAttackTable.Client.Interactions.AttackTableInteractions;
using Microsoft.AspNetCore.Components;
using VirtualAttackTableLib.TargetShipParameter;

namespace BlazorWASMAttackTable.Client.Elements.AttackTableElements
{
    public class CollapsableFloatParameterCell<TParameter, TDefinitionKey> : SubscriptionDisposingElement
        where TParameter : AlteredUnitParameterInteraction<TDefinitionKey>
        where TDefinitionKey : notnull 
    {
        #region Fields
        private bool _highlight = false;
        #endregion

        #region Properties
        [Parameter, EditorRequired]
        public AttackTableShipEntry OwningEntry { get; set; } = null!;

        [Parameter, EditorRequired]
        public TParameter Interaction {
            get;
            set; } = null!;

        protected string BackgroundStyleEntry => Highlight ? "rgb(0, 100, 200, 0.5)" : "transparent";

        protected bool Highlight
        {
            get
            {
                return _highlight;
            }
            private set
            {
                if (_highlight != value)
                {
                    _highlight = value;
                    StateHasChanged();
                }
            }
        }
        #endregion

        #region Methods
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            Subscribe(OwningEntry.ParametersToHighlight.ValueChanged, OnOwningEntryParametersToHighlightChanged);
            OnOwningEntryParametersToHighlightChanged(OwningEntry.ParametersToHighlight.Value);
        }

        private void OnOwningEntryParametersToHighlightChanged(IReadOnlySet<IParameter> parametersToHighlight)
        {
            Highlight = parametersToHighlight.Contains(Interaction.Parameter);
        }
        #endregion
    }
}
