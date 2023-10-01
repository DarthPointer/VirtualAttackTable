using VirtualAttackTableLib.AttackTarget;

namespace BlazorWASMAttackTable.Client.AsyncLoading
{
    public static class AsyncLoadedData
    {
        public static AsyncLoadedItem<IReadOnlyList<TargetShipData>> TargetShipDatas { get; } = new();
    }
}
