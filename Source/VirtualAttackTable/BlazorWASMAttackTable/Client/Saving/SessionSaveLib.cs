using BlazorWASMAttackTable.Client.Interactions;
using BlazorWASMAttackTable.Client.Interactions.AttackTableInteractions;
using BlazorWASMAttackTable.Client.Interactions.Options;
using BlazorWASMAttackTable.Client.Saving.SaveObjectVersions.CurrentVersion;
using Newtonsoft.Json.Linq;
using VirtualAttackTableLib.AttackTarget;
using VirtualAttackTableLib.TargetShipParameter;

namespace BlazorWASMAttackTable.Client.Saving
{
    public static class SessionSaveLib
    {
        public static Save SessionToSave(UserSessionInteraction userSessionInteraction)
        {
            Session session = new()
            {
                IdentificationTabs = userSessionInteraction.IdentifyShipInteractions.Select(CreateIdentificationSave).ToList(),

                ParameterUnits = CreateParameterUnitsSave(userSessionInteraction.AttackTable.UnitsSelection),

                TargetShipEntries = userSessionInteraction.AttackTable.TargetShips.Select(wrap => CreateTargetShipEntrySave(wrap.TargetShip)).ToList()
            };

            Save result = new()
            {
                versionCode = SaveVersion.Current.VersionCode,
                session = JToken.FromObject(session)
            };

            return result;
        }

        private static IdentificationState CreateIdentificationSave(IdentifyShipInteraction identifyShipInteraction)
        {
            return new()
            {
                EnginePlacement = identifyShipInteraction.EnginePlacement.SelectedOption.Value?.Value,
                Superstructure = identifyShipInteraction.Superstructure.SelectedOption.Value?.Value,

                BowIsland = identifyShipInteraction.BowIsland.Value,
                MidIsland = identifyShipInteraction.MidIsland.Value,
                SternIsland = identifyShipInteraction.SternIsland.Value,

                StructuresCode = identifyShipInteraction.StructuresCode.Value,

                SelectedShip = identifyShipInteraction.SelectedWrap.Value?.TargetShipData.TypeName
            };
        }

        private static ParameterUnits CreateParameterUnitsSave(UnitsSelection unitsSelection)
        {
            return new()
            {
                Bearing = GetSelectedUnitName(unitsSelection.Bearing),

                AbsoluteHeight = GetSelectedUnitName(unitsSelection.AbsoluteHeight),
                VisibleHeight = GetSelectedUnitName(unitsSelection.VisibleHeight),
                AbsoluteLength = GetSelectedUnitName(unitsSelection.AbsoluteLength),
                VisibleLength = GetSelectedUnitName(unitsSelection.VisibleLength),

                TargetRange = GetSelectedUnitName(unitsSelection.TargetRange),

                AoB = GetSelectedUnitName(unitsSelection.AoB),

                HullTime = GetSelectedUnitName(unitsSelection.HullTime),
                OneDegreeTime = GetSelectedUnitName(unitsSelection.OneDegreeTime),

                TargetSpeed = GetSelectedUnitName(unitsSelection.TargetSpeed),
                AngularSpeed = GetSelectedUnitName(unitsSelection.AngularTargetSpeed),

                TorpedoSpeed = GetSelectedUnitName(unitsSelection.TorpedoSpeed),

                BoatSpeed = GetSelectedUnitName(unitsSelection.BoatSpeed),

                LeadAngle = GetSelectedUnitName(unitsSelection.LeadAngle)
            };
        }

        private static string GetSelectedUnitName(IListForSingleOption unitSelection)
        {
            return unitSelection.SelectedOption.Value!.OptionName;
        }

        private static TargetShipEntry CreateTargetShipEntrySave(TargetShip targetShip)
        {
            return new()
            {
                Bearing = CreateFloatCellSave(targetShip.BearingRadians),

                AbsoluteHeight = CreateFloatCellSave(targetShip.AbsoluteHeightMeters),
                VisibleHeight = CreateFloatCellSave(targetShip.VisibleHeightRadians),
                AbsoluteLength = CreateFloatCellSave(targetShip.AbsoluteLengthMeters),
                VisibleLength = CreateFloatCellSave(targetShip.VisibleLengthRadians),

                TargetRange = CreateFloatCellSave(targetShip.TargetRangeMeters),

                AoB = CreateFloatCellSave(targetShip.AoBRadians),
                AoBQuarter = targetShip.AoBRadians.AoBQuarter,

                HullTime = CreateFloatCellSave(targetShip.HullTimeSeconds),
                OneDegreeTime = CreateFloatCellSave(targetShip.OneDegreeTimeSeconds),

                TargetSpeed = CreateFloatCellSave(targetShip.TargetSpeedMpS),
                AngularSpeed = CreateFloatCellSave(targetShip.AngularSpeedRpS),

                TorpedoSpeed = CreateFloatCellSave(targetShip.TorpedoSpeedMpS),

                BoatSpeed = CreateFloatCellSave(targetShip.BoatSpeedMpS),

                LeadAngle = CreateFloatCellSave(targetShip.LeadAngleRadians)
            };
        }

        private static FloatCell<TDefinitionKey> CreateFloatCellSave<TDefinitionKey>(MultipleDefinitionParameter<TDefinitionKey, float> parameter)
            where TDefinitionKey : notnull
        {
            return new()
            {
                ActiveDefinitionKey = parameter.ActiveDefinitionKey!,
                ArbitraryValue = parameter.AllDefinitions.Values.First(def => def is ArbitraryValueParameterDefinition<float>).CurrentValue
            };
        }
    }
}
