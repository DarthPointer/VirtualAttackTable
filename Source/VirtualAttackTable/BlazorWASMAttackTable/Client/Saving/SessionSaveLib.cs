﻿using BlazorWASMAttackTable.Client.AsyncLoading;
using BlazorWASMAttackTable.Client.Interactions;
using BlazorWASMAttackTable.Client.Interactions.AttackTableInteractions;
using BlazorWASMAttackTable.Client.Interactions.Options;
using BlazorWASMAttackTable.Client.Saving.SaveObjectVersions;
using BlazorWASMAttackTable.Client.Saving.SaveObjectVersions.CurrentVersion;
using Newtonsoft.Json.Linq;
using VirtualAttackTableLib;
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

            Current currentVersionSaveObject = new() { Session = session };

            Save result = new()
            {
                versionCode = SaveVersion.Current.VersionCode,
                saveObject = JToken.FromObject(currentVersionSaveObject)
            };

            return result;
        }

        public static async Task<UserSessionInteraction> CreateSessionFromSave(Save save)
        {
            IReadOnlyList<TargetShipData> allTargetShipDatas = await AsyncLoadedData.TargetShipDatas.ItemTask;

            UserSessionInteraction result = new(allTargetShipDatas);

            Session sessionData = save.GetCurrentVersionSaveObject().Session;

            foreach (IdentificationState identificationState in sessionData.IdentificationTabs)
            {
                SetupIdentifyShipInteraction(result.StartNewIdentifyShipInteraction(), identificationState);
            }

            SetUnits(result.AttackTable.UnitsSelection, sessionData.ParameterUnits);

            foreach (TargetShipEntry targetShipEntry in sessionData.TargetShipEntries)
            {
                AddTargetShip(result.AttackTable, targetShipEntry, allTargetShipDatas);
            }

            return result;
        }

        #region Save
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
                TargetHeading = GetSelectedUnitName(unitsSelection.TargetHeading),

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

                BoatHeading = GetSelectedUnitName(unitsSelection.BoatHeading),
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
                TypeName = targetShip.Data.TypeName,

                Bearing = CreateFloatCellSave(targetShip.BearingRadians),
                TargetHeading = CreateFloatCellSave(targetShip.TargetHeadingRadians),

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

                BoatHeading = CreateFloatCellSave(targetShip.BoatHeadingRadians),
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
        #endregion

        #region Load
        private static void SetupIdentifyShipInteraction(IdentifyShipInteraction identifyShipInteraction, IdentificationState identificationState)
        {
            SelectOption(identifyShipInteraction.Superstructure, identificationState.Superstructure);
            SelectOption(identifyShipInteraction.EnginePlacement, identificationState.EnginePlacement);

            identifyShipInteraction.BowIsland.Value = identificationState.BowIsland;
            identifyShipInteraction.MidIsland.Value = identificationState.MidIsland;
            identifyShipInteraction.SternIsland.Value = identificationState.SternIsland;

            identifyShipInteraction.StructuresCode.Value = identificationState.StructuresCode;
        }

        private static void SelectOption<TValue>(IListForSingleOption<TValue> listForSingleOption, TValue? valueToSelect)
            where TValue : struct
        {
            if (valueToSelect == null)
            {
                listForSingleOption.SelectNoOption();
                return;
            }

            SelectOption(listForSingleOption, val => val.Equals(valueToSelect));
        }

        private static void SelectOption<TValue>(IListForSingleOption<TValue> listForSingleOption, Func<TValue, bool> predicate)
            where TValue : notnull
        {
            listForSingleOption.SelectNoOption();

            listForSingleOption.Options.FirstOrDefault(option => predicate(option.Value))?.Toggle();
        }

        private static void SetUnits(UnitsSelection unitsSelection, ParameterUnits parameterUnits)
        {
            SelectUnit(unitsSelection.Bearing, parameterUnits.Bearing);
            SelectUnit(unitsSelection.TargetHeading, parameterUnits.TargetHeading);

            SelectUnit(unitsSelection.AbsoluteHeight, parameterUnits.AbsoluteHeight);
            SelectUnit(unitsSelection.VisibleHeight, parameterUnits.VisibleHeight);
            SelectUnit(unitsSelection.AbsoluteLength, parameterUnits.AbsoluteLength);
            SelectUnit(unitsSelection.VisibleLength, parameterUnits.VisibleLength);

            SelectUnit(unitsSelection.TargetRange, parameterUnits.TargetRange);

            SelectUnit(unitsSelection.AoB, parameterUnits.AoB);

            SelectUnit(unitsSelection.HullTime, parameterUnits.HullTime);
            SelectUnit(unitsSelection.OneDegreeTime, parameterUnits.OneDegreeTime);

            SelectUnit(unitsSelection.TargetSpeed, parameterUnits.TargetSpeed);
            SelectUnit(unitsSelection.AngularTargetSpeed, parameterUnits.AngularSpeed);

            SelectUnit(unitsSelection.TorpedoSpeed, parameterUnits.TorpedoSpeed);

            SelectUnit(unitsSelection.BoatHeading, parameterUnits.BoatHeading);
            SelectUnit(unitsSelection.BoatSpeed, parameterUnits.BoatSpeed);

            SelectUnit(unitsSelection.LeadAngle, parameterUnits.LeadAngle);
        }

        private static void SelectUnit(IListForSingleOption<IUnit> unitSelection, string unitName)
        {
            SelectOption(unitSelection, unit => unit.UnitName == unitName);
        }

        private static void AddTargetShip(AttackTableInteraction attackTable, TargetShipEntry targetShipEntry, IReadOnlyList<TargetShipData> allTargetShipDatas)
        {
            string typeName = targetShipEntry.TypeName;

            TargetShipData targetShipData = allTargetShipDatas.FirstOrDefault(x => x.TypeName == typeName) ??
                throw new Exception($"Could not create a target ship of type {typeName} because no target ship data was found for it.");

            TargetShipAlteredUnitsWrap targetShip = attackTable.CreateNewTargetShip(targetShipData);

            SetupParameter(targetShip.Bearing, targetShipEntry.Bearing);
            SetupParameter(targetShip.TargetHeading, targetShipEntry.TargetHeading);

            SetupParameter(targetShip.AbsoluteHeight, targetShipEntry.AbsoluteHeight);
            SetupParameter(targetShip.VisibleHeight, targetShipEntry.VisibleHeight);
            SetupParameter(targetShip.AbsoluteLength, targetShipEntry.AbsoluteLength);
            SetupParameter(targetShip.VisibleLength, targetShipEntry.VisibleLength);

            SetupParameter(targetShip.TargetRange, targetShipEntry.TargetRange);

            SetupParameter(targetShip.AoB, targetShipEntry.AoB);
            targetShip.AoB.Parameter.AoBQuarter = targetShipEntry.AoBQuarter;

            SetupParameter(targetShip.HullTime, targetShipEntry.HullTime);
            SetupParameter(targetShip.OneDegreeTime, targetShipEntry.OneDegreeTime);

            SetupParameter(targetShip.TargetSpeed, targetShipEntry.TargetSpeed);
            SetupParameter(targetShip.AngularSpeed, targetShipEntry.AngularSpeed);

            SetupParameter<string>(targetShip.TorpedoSpeed, targetShipEntry.TorpedoSpeed);

            SetupParameter(targetShip.BoatHeading, targetShipEntry.BoatHeading);
            SetupParameter(targetShip.BoatSpeed, targetShipEntry.BoatSpeed);

            SetupParameter(targetShip.LeadAngle, targetShipEntry.LeadAngle);
        }

        private static void SetupParameter<TDefinitionKey>(AlteredUnitParameterInteraction<TDefinitionKey> parameter, FloatCell<TDefinitionKey> floatCell)
            where TDefinitionKey : struct
        {
            parameter.Parameter.SetArbitraryValue(floatCell.ArbitraryValue);
            SelectOption(parameter.DefinitionKeySelection, floatCell.ActiveDefinitionKey);
        }

        private static void SetupParameter<TDefinitionKey>(AlteredUnitParameterInteraction<string> parameter, FloatCell<string> floatCell)
        {
            parameter.Parameter.SetArbitraryValue(floatCell.ArbitraryValue);
            SelectOption(parameter.DefinitionKeySelection, key => key == floatCell.ActiveDefinitionKey);
        }
        #endregion
    }
}
