using BlazorWASMAttackTable.Client.Interactions.AttackTableInteractions;
using BlazorWASMAttackTable.Client.Interactions.Common;
using VirtualAttackTableLib.AttackTarget;

namespace BlazorWASMAttackTable.Client.Interactions
{
    public class AttackTableInteraction
    {
        #region Fields
        #endregion

        #region Properties
        public ListInteraction<TargetShipAlteredUnitsWrap> TargetShips
        {
            get;
            private init;
        }

        public CollapseColumnsFlags CollapseColumnsFlags { get; } = new();

        public UnitsSelection UnitsSelection { get; }
        #endregion

        #region Constructors
        public AttackTableInteraction()
        {
            TargetShips = new();
            UnitsSelection = new(this);
        }
        #endregion

        #region Methods
        public void CreateNewTargetShip(TargetShipData targetShipData)
        {
            TargetShips.Add(new(new(targetShipData), UnitsSelection));
        }

        public void RemoveTargetShip(TargetShipAlteredUnitsWrap targetShip)
        {
            TargetShips.Remove(targetShip);
        }
        #endregion
    }
}
