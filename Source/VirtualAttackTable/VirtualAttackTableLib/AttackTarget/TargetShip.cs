using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAttackTableLib.TargetShipParameter;

namespace VirtualAttackTableLib.AttackTarget
{
    public class TargetShip
    {
        #region Fields
        #endregion

        #region Properties
        public TargetShipData Data
        {
            get => ShipDataParameterDefinition.Value;
            set => ShipDataParameterDefinition.Value = value;
        }
        private ArbitraryValueParameterDefinition<TargetShipData> ShipDataParameterDefinition { get; init; }
        private MultipleDefinitionParameter<SingleOptionDefinition, TargetShipData> ShipDataParameter { get; init; } = new();

        public MultipleDefinitionParameter<BearingDefinition, float> BearingRadians { get; private init; } = new();

        public MultipleDefinitionParameter<AbsoluteHeightDefinition, float> AbsoluteHeightMeters { get; private init; } = new();
        public MultipleDefinitionParameter<VisibleHeightDefinition, float> VisibleHeightRadians { get; private init; } = new();
        public MultipleDefinitionParameter<AbsoluteLengthDefinition, float> AbsoluteLengthMeters { get; private init; } = new();
        public MultipleDefinitionParameter<VisibleLengthDefinition, float> VisibleLengthRadians { get; private init; } = new();

        public MultipleDefinitionParameter<TargetRangeDefinition, float> TargetRangeMeters { get; private init; } = new();
        public AoBParameter AoBRadians { get; private init; } = new();

        public MultipleDefinitionParameter<HullTimeDefinition, float> HullTimeSeconds { get; private init; } = new();
        public MultipleDefinitionParameter<OneDegreeTimeDefinition, float> OneDegreeTimeSeconds { get; private init; } = new();

        public MultipleDefinitionParameter<TargetSpeedDefinition, float> TargetSpeedMpS { get; private init; } = new();
        public MultipleDefinitionParameter<AngularSpeedDefinition, float> AngularSpeedRpS { get; private init; } = new();

        public TorpedoSpeedMpSParameter TorpedoSpeedMpS { get; private init; } = new();

        public MultipleDefinitionParameter<BoatSpeedDefinition, float> BoatSpeedMpS { get; private init; } = new();

        public MultipleDefinitionParameter<LeadAngleDefinition, float> LeadAngleRadians { get; private init; } = new();
        #endregion

        #region Constructors
        public TargetShip(TargetShipData initialShip)
        {
            List<IParameterDefinition> parameterDefinitions = new();

            ShipDataParameterDefinition = new ArbitraryValueParameterDefinition<TargetShipData>(ShipDataParameter);
            parameterDefinitions.Add(ShipDataParameter.AddDefinition(SingleOptionDefinition.O, ShipDataParameterDefinition));

            Data = initialShip;

            parameterDefinitions.Add(BearingRadians.AddDefinition(BearingDefinition.Arbitrary,
                new ArbitraryValueParameterDefinition<float>(BearingRadians, defaultValue: 0)));


            parameterDefinitions.Add(AbsoluteHeightMeters.AddDefinition(AbsoluteHeightDefinition.Maximum, GetMaximumAbsoluteHeightMeters, new List<IParameter> { ShipDataParameter }));
            parameterDefinitions.Add(AbsoluteHeightMeters.AddDefinition(AbsoluteHeightDefinition.Arbitrary, new ArbitraryValueParameterDefinition<float>(AbsoluteHeightMeters, defaultValue: GetMaximumAbsoluteHeightMeters())));

            parameterDefinitions.Add(VisibleHeightRadians.AddDefinition(VisibleHeightDefinition.Arbitrary, new ArbitraryValueParameterDefinition<float>(VisibleHeightRadians, defaultValue: 0)));

            parameterDefinitions.Add(AbsoluteLengthMeters.AddDefinition(AbsoluteLengthDefinition.Maximum, GetMaximumLengthMeters, new List<IParameter> { ShipDataParameter }));
            parameterDefinitions.Add(AbsoluteLengthMeters.AddDefinition(AbsoluteLengthDefinition.Arbitrary, new ArbitraryValueParameterDefinition<float>(AbsoluteLengthMeters, defaultValue: GetMaximumLengthMeters())));

            parameterDefinitions.Add(VisibleLengthRadians.AddDefinition(VisibleLengthDefinition.Arbitrary, new ArbitraryValueParameterDefinition<float>(VisibleLengthRadians, defaultValue: 0)));


            parameterDefinitions.Add(TargetRangeMeters.AddDefinition(TargetRangeDefinition.Arbitrary, new ArbitraryValueParameterDefinition<float>(TargetRangeMeters, defaultValue: 0)));
            parameterDefinitions.Add(TargetRangeMeters.AddDefinition(TargetRangeDefinition.ByVisibleHight, GetTargetRangeByVisibleHeight, new List<IParameter> { AbsoluteHeightMeters, VisibleHeightRadians }));

            parameterDefinitions.Add(AoBRadians.AddDefinition(AoBDefinition.Arbitrary, new ArbitraryAoBDefinition(AoBRadians, defaultValue: 0)));
            parameterDefinitions.Add(AoBRadians.AddDefinition(AoBDefinition.ByRangeAndVisibleLength, GetAoBByRangeAndVisibleLength, new List<IParameter> { TargetRangeMeters, AbsoluteLengthMeters, VisibleLengthRadians }));


            parameterDefinitions.Add(HullTimeSeconds.AddDefinition(HullTimeDefinition.Arbitrary, new ArbitraryValueParameterDefinition<float>(HullTimeSeconds, defaultValue: 0)));

            parameterDefinitions.Add(OneDegreeTimeSeconds.AddDefinition(OneDegreeTimeDefinition.Arbitrary, new ArbitraryValueParameterDefinition<float>(OneDegreeTimeSeconds, defaultValue: 0)));

            parameterDefinitions.Add(TargetSpeedMpS.AddDefinition(TargetSpeedDefinition.Arbitrary, new ArbitraryValueParameterDefinition<float>(TargetSpeedMpS, defaultValue: 0)));
            parameterDefinitions.Add(TargetSpeedMpS.AddDefinition(TargetSpeedDefinition.Linear, GetTargetSpeedLinear, new List<IParameter> { HullTimeSeconds, AbsoluteLengthMeters }));
            parameterDefinitions.Add(TargetSpeedMpS.AddDefinition(TargetSpeedDefinition.Angular, GetTargetSpeedAngular, new List<IParameter> { TargetRangeMeters, AngularSpeedRpS, AoBRadians }));
            parameterDefinitions.Add(TargetSpeedMpS.AddDefinition(TargetSpeedDefinition.LinearConstantBoatVelocity, GetTargetSpeedConstantBoatVelocity, new List<IParameter> { AbsoluteLengthMeters, HullTimeSeconds, BoatSpeedMpS, BearingRadians, AoBRadians }));

            parameterDefinitions.Add(AngularSpeedRpS.AddDefinition(AngularSpeedDefinition.Arbitrary, new ArbitraryValueParameterDefinition<float>(AngularSpeedRpS, defaultValue: 0)));
            parameterDefinitions.Add(AngularSpeedRpS.AddDefinition(AngularSpeedDefinition.ByOneDegreeTime, GetAngularSpeedByOneDegreeTime, new List<IParameter> { OneDegreeTimeSeconds }));

            parameterDefinitions.Add(BoatSpeedMpS.AddDefinition(BoatSpeedDefinition.Arbitrary, new ArbitraryValueParameterDefinition<float>(BoatSpeedMpS, defaultValue: 0)));

            parameterDefinitions.Add(LeadAngleRadians.AddDefinition(LeadAngleDefinition.Arbitrary, new ArbitraryValueParameterDefinition<float>(LeadAngleRadians, defaultValue: 0)));
            parameterDefinitions.Add(LeadAngleRadians.AddDefinition(LeadAngleDefinition.ByAngularSpeed, GetLeadAngleByAngularSpeed, new List<IParameter> { TargetRangeMeters, AngularSpeedRpS, TorpedoSpeedMpS }));

            foreach (IParameterDefinition parameterDefinition in parameterDefinitions)
            {
                parameterDefinition.Update();
            }
        }
        #endregion

        #region Methods
        private float GetMaximumAbsoluteHeightMeters()
        {
            return Data.MaxHeightMeters;
        }

        private float GetMaximumLengthMeters()
        {
            return Data.LengthMeters;
        }

        private float GetTargetRangeByVisibleHeight()
        {
            return AttackArithmetics.RangeMetersByHeight(AbsoluteHeightMeters.CurrentValue, VisibleHeightRadians.CurrentValue);
        }

        private float GetAoBByRangeAndVisibleLength()
        {
            return AttackArithmetics.AoBRadiansByTrigonometry(TargetRangeMeters.CurrentValue, AbsoluteLengthMeters.CurrentValue, VisibleLengthRadians.CurrentValue, AoBRadians.AoBQuarter);
        }

        private float GetTargetSpeedLinear()
        {
            return AttackArithmetics.SpeedMpSStaticLinear(AbsoluteLengthMeters.CurrentValue, HullTimeSeconds.CurrentValue);
        }

        private float GetTargetSpeedAngular()
        {
            return AttackArithmetics.SpeedMpSStaticAngular(TargetRangeMeters.CurrentValue, AngularSpeedRpS.CurrentValue, AoBRadians.CurrentValue);
        }

        private float GetTargetSpeedConstantBoatVelocity()
        {
            return AttackArithmetics.SpeedMpSConstantBoatVelocityLinear(AbsoluteLengthMeters.CurrentValue, HullTimeSeconds.CurrentValue, BoatSpeedMpS.CurrentValue,
                BearingRadians.CurrentValue, AoBRadians.CurrentValue);
        }

        private float GetAngularSpeedByOneDegreeTime()
        {
            return AttackArithmetics.AngularSpeedRpSByOneDegreeTime(OneDegreeTimeSeconds.CurrentValue);
        }

        private float GetLeadAngleByAngularSpeed()
        {
            return AttackArithmetics.LeadAngleRadiansFastAttack(TargetRangeMeters.CurrentValue, AngularSpeedRpS.CurrentValue, TorpedoSpeedMpS.CurrentValue);
        }
        #endregion
    }
}
