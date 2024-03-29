﻿using CallbackList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAttackTableLib.AttackTarget;

namespace VirtualAttackTableLib.TargetShipParameter
{
    public class AoBParameter : MultipleDefinitionParameter<AoBDefinition, float>
    {
        #region Fields
        private AoBQuarter _aoBQuarter;
        #endregion

        #region Properties
        public AoBQuarter AoBQuarter
        {
            get
            {
                return _aoBQuarter;
            }
            set
            {
                if (_aoBQuarter != value && ActiveDefinitionKey != AoBDefinition.ByHeadingsAndBearing)
                {
                    _aoBQuarter = value;
                    OnAoBQuarterChanged();
                }
            }
        }

        public bool SwitchQuarterBySide =>
            ActiveDefinitionKey == AoBDefinition.Arbitrary ||
            ActiveDefinitionKey == AoBDefinition.ByHeadingsAndBearing;
        #endregion

        #region Methods
        private void OnAoBQuarterChanged()
        {
            foreach(IParameterDefinition definition in AllDefinitions.Values)
            {
                definition.Update();
            }

            OnParameterChanged();
        }

        protected override void ProcessParameterChanged()
        {
            if (ActiveDefinitionKey == AoBDefinition.ByHeadingsAndBearing)
            {
                _aoBQuarter = CurrentValue > MathF.PI ? AoBQuarter.AheadLeft : AoBQuarter.AheadRight;
            }
        }
        #endregion
    }
}
