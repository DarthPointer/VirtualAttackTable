using BlazorWASMAttackTable.Client.Interactions.Common;
using VirtualAttackTableLib.AttackTarget;

namespace BlazorWASMAttackTable.Client.Interactions.IdentificationInteractions
{
    public class SelectTargetShipDataWrapInteraction
    {
        #region Properties
        public TargetShipData TargetShipData
        {
            get;
            private init;
        }

        public ValueInteraction<bool> IsSelected
        {
            get;
            private init;
        }

        private IdentifyShipInteraction OwningInteraction
        {
            get;
            init;
        }
        #endregion

        #region Constructors
        public SelectTargetShipDataWrapInteraction(TargetShipData targetShipData, IdentifyShipInteraction owningInteraction)
        {
            TargetShipData = targetShipData;
            IsSelected = new(false);
            OwningInteraction = owningInteraction;
        }
        #endregion

        #region Methods
        public void TriggerSelect()
        {
            OwningInteraction.SelectData(this);
        }

        public void AcceptThisForAttackTable()
        {
            OwningInteraction.SelectData(this);
            OwningInteraction.AcceptDataFromSelectedWrap();
        }
        #endregion
    }
}
