using BlazorWASMAttackTable.Client.Elements.SubscriptionDisposal;
using BlazorWASMAttackTable.Client.Interactions.AttackTableInteractions;
using BlazorWASMAttackTable.Client.Interactions.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using VirtualAttackTableLib.TargetShipParameter;

namespace BlazorWASMAttackTable.Client.Elements.AttackTableElements
{
    public partial class FloatParameterCell<TParameter, TDefinitionKey>
        where TParameter : MultipleDefinitionParameter<TDefinitionKey, float>
        where TDefinitionKey : notnull
    {
        private const string  DEPENDENCY_LOOP = "Dependency Loop";

        #region Fields
        private bool _badInput = false;
        private string _displayValue = "";

        private bool _highlightingParametersForActiveDefinition;
        private IParameterDefinition? _previewedDefinition;
        #endregion

        #region Properties
        [Parameter, EditorRequired]
        public AlteredUnitParameterInteraction<TParameter, TDefinitionKey> Interaction { get; set; } = null!;

        private AlteredUnitParameterInteraction<TParameter, TDefinitionKey>? PrevInteraction { get; set; }

        [Parameter, EditorRequired]
        public AttackTableShipEntry OwningEntry { get; set; } = null!;

        private string DisplayValue
        {
            get
            {
                return DependencyLoop ? DEPENDENCY_LOOP : _displayValue;
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

        private bool DependencyLoop => Interaction.Parameter.CurrentState == ParameterDefinitionState.DependencyLoop;

        private bool HighlightingParametersForActiveDefinition
        {
            get
            {
                return _highlightingParametersForActiveDefinition;
            }
            set
            {
                if (_highlightingParametersForActiveDefinition != value)
                {
                    _highlightingParametersForActiveDefinition = value;
                    UpdateDefinitionToHighlight();
                }
            }
        }

        private IParameterDefinition? PreviewedDefinition
        {
            get
            {
                return _previewedDefinition;
            }
            set
            {
                if (_previewedDefinition != value)
                {
                    _previewedDefinition = value;
                    UpdateDefinitionToHighlight();
                }
            }
        }

        private IJSInProcessObjectReference AssistanceModule { get; set; } = null!;
        #endregion

        #region Methods
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                AssistanceModule = await JS.InvokeAsync<IJSInProcessObjectReference>("import", "./Elements/AttackTableElements/FloatParameterCell.razor.js");
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            Subscribe(Interaction.ParameterChanged, OnParameterChanged);
            Subscribe(Interaction.DefinitionKeySelection.PreviewedOption.ValueChanged, OnDefinitionPreviewedValueChanged);
            //Subscribe(Interaction.DefinitionKeySelection.SelectedOption.ValueChanged, OnSelectedDefinitionChanged);

            // Blazor likes to issue reinitializations even when there is no good reason to do it.
            if (Interaction != PrevInteraction)
            {
                PrevInteraction = Interaction;
                OnParameterChanged();
            }
        }

        private void OnParameterChanged()
        {
            BadInput = false;
            _displayValue = BlazorAttackTableLib.CustomUnitValueFormat(Interaction.CurrentValue);

            SetOwnDefinitionToHighlightIfPresent();

            StateHasChanged();
        }

        private void OnMouseEnterThisCell()
        {
            HighlightingParametersForActiveDefinition = true;
        }

        private void OnMouseLeaveThisCell()
        {
            HighlightingParametersForActiveDefinition = false;
        }

        /// <summary>
        /// A hack to track the cases of mouse cursor being out of the cell's root content box when clicking.
        /// It is needed to set <see cref="HighlightingParametersForActiveDefinition"/> to <see langword="false"/> if the click closes 
        /// the dropdown while being outside the content box.
        /// </summary>
        /// <param name="mouseArgs"></param>
        private void OnMouseClick(MouseEventArgs mouseArgs)
        {
            HighlightingParametersForActiveDefinition = AssistanceModule.Invoke<bool>("IsPointOver", RootDiv, mouseArgs.ScreenX, mouseArgs.ScreenX);
        }
        
        private void OnDefinitionPreviewedValueChanged(Option<TDefinitionKey>? previewedOption)
        {
            if (previewedOption == null)
            {
                PreviewedDefinition = null;
                return;
            }
            PreviewedDefinition = Interaction.Parameter.AllDefinitions[previewedOption.Value];
        }

        private void UpdateDefinitionToHighlight()
        {
            if (PreviewedDefinition != null)
            {
                OwningEntry.DefinitionToHighlightParameters = PreviewedDefinition;
                return;
            }

            if (HighlightingParametersForActiveDefinition)
            {
                OwningEntry.DefinitionToHighlightParameters = Interaction.Parameter.ActiveDefinition;
                return;
            }

            else if (Interaction.Parameter.AllDefinitions.Values.Contains(OwningEntry.DefinitionToHighlightParameters))
            {
                OwningEntry.DefinitionToHighlightParameters = null;
                return;
            }
        }

        private void SetOwnDefinitionToHighlightIfPresent()
        {
            if (PreviewedDefinition != null)
            {
                OwningEntry.DefinitionToHighlightParameters = PreviewedDefinition;
                return;
            }

            if (HighlightingParametersForActiveDefinition)
            {
                OwningEntry.DefinitionToHighlightParameters = Interaction.Parameter.ActiveDefinition;
                return;
            }
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (AssistanceModule is not null)
            {
                await AssistanceModule.DisposeAsync();
            }
        }
        #endregion
    }
}
