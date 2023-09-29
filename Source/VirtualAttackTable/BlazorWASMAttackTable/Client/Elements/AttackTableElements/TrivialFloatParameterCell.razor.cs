using Microsoft.AspNetCore.Components;
using BlazorWASMAttackTable.Client.Interactions.AttackTableInteractions;
using VirtualAttackTableLib.TargetShipParameter;
using Microsoft.VisualBasic;

namespace BlazorWASMAttackTable.Client.Elements.AttackTableElements
{
    public partial class TrivialFloatParameterCell<TParameter, TDefinitionKey>
        where TParameter : AlteredUnitParameterInteraction<TDefinitionKey>
        where TDefinitionKey : notnull
    {
    }
}
