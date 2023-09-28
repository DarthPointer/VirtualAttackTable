using BlazorWASMAttackTable.Client.Interactions.Common;
using BlazorWASMAttackTable.Client.Interactions.Options;

namespace BlazorWASMAttackTable.Client.Elements.AttackTableElements
{
    public class CollapsableTableColumnHeaderWrapData
    {
        public required ValueInteraction<bool> IsCollapsed { get; init; }

        public required IListForSingleOption UnitSelection { get; init; }

        public required string IcoPath { get; init; }
    }
}
