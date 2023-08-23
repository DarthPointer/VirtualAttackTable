using BlazorWASMAttackTable.Client.Interactions.Common;
using BlazorWASMAttackTable.Shared.Protos;
using CallbackList;
using VirtualAttackTableLib.AttackTarget;

namespace BlazorWASMAttackTable.Client.Interactions
{
    public class UserSessionInteraction
    {
        #region Fields
        #endregion

        #region Properties
        public AttackTableInteraction AttackTable
        {
            get;
            private init;
        }

        public ListInteraction<IdentifyShipInteraction> IdentifyShipInteractions
        {
            get;
            private init;
        }

        private Dictionary<IdentifyShipInteraction, ISubscriptionHandle> IdentifyShipInteractionSubscriptions { get; } = new();

        private IReadOnlyList<TargetShipData> AllTargetShipDatasCache { get; }
        #endregion

        #region Constructors
        private UserSessionInteraction(IEnumerable<TargetShipData> allTargetShipDatas)
        {
            AttackTable = new();
            IdentifyShipInteractions = new();

            AllTargetShipDatasCache = allTargetShipDatas.ToList();
        }
        #endregion

        #region Methods
        public IdentifyShipInteraction StartNewIdentifyShipInteraction()
        {
            IdentifyShipInteraction newInteraction = new IdentifyShipInteraction(AllTargetShipDatasCache);

            IdentifyShipInteractionSubscriptions.Add(
                newInteraction,
                newInteraction.TargetShipDataAccepted.Subscribe(AddNewTargetShip));
            IdentifyShipInteractions.Add(newInteraction);

            return newInteraction;
        }

        public void DropIdentifyShipInteraction(IdentifyShipInteraction identifyShipInteraction)
        {
            if (IdentifyShipInteractions.Remove(identifyShipInteraction))
            {
                IdentifyShipInteractionSubscriptions[identifyShipInteraction].Unsubscribe();
            }
        }

        public void AddNewTargetShip(TargetShipData targetShipData)
        {
            AttackTable.CreateNewTargetShip(targetShipData);
        }

        public async static Task<UserSessionInteraction> CreateAsync(TargetShipdDataSupplierService.TargetShipdDataSupplierServiceClient targetShipdDataSupplierServiceClient)
        {
            IEnumerable<TargetShipData> allTargetShipDatas = (await targetShipdDataSupplierServiceClient.ProvideDatasAsync(new())).Datas.Select(buffer => buffer.ToData());

            return new UserSessionInteraction(allTargetShipDatas);
        }
        #endregion
    }
}
