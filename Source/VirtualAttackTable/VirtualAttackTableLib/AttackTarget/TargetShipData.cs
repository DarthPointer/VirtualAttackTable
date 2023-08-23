using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAttackTableLib.TargetIdentification;

namespace VirtualAttackTableLib.AttackTarget
{
    public class TargetShipData
    {
        #region Properties
        public required float GRT
        {
            get;
            init;
        }

        public required float MaxHeightMeters
        {
            get;
            init;
        }

        public required FloatRange VerticalImageRange
        {
            get;
            init;
        }

        public required float LengthMeters
        {
            get;
            init;
        }

        public required FloatRange HorizontalImageRange
        {
            get;
            init;
        }

        public required float DraughtMeters
        {
            get;
            init;
        }

        public required float MaxSpeedMpS
        {
            get;
            init;
        }

        public required string TypeName
        {
            get;
            init;
        }

        public required EnginePlacement EnginePlacement
        {
            get;
            init;
        }

        public required IslandsPositions IslandsPositions
        {
            get;
            init;
        }

        public required Superstructure Superstructure
        {
            get;
            init;
        }

        public required IReadOnlyList<StructureType> Structures
        {
            get;
            init;
        }
        #endregion

        #region Constructors
        public TargetShipData()
        {
        }
        #endregion
    }
}
