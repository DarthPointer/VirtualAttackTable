using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAttackTableLib.AttackTarget
{
    public static class AttackArithmetics
    {
        public static float RangeMetersByHeight(float absoluteHeightMeters, float visibleHeightRadians)
        {
            if (visibleHeightRadians <= 0) return float.NaN;

            return absoluteHeightMeters / MathF.Sin(visibleHeightRadians);
        }

        public static float QuarterAoBRadiansByTrigonometry(float rangeMeters, float absoluteLengthMeters, float visibleLengthRadians)
        {
            if (absoluteLengthMeters <= 0) return float.NaN;
            if (rangeMeters <= 0) return float.NaN;
            if (visibleLengthRadians <= 0) return float.NaN;

            float normalVisibleLengthRadians = MathF.Asin(absoluteLengthMeters/rangeMeters);
            float ratio = visibleLengthRadians / normalVisibleLengthRadians;
            if (ratio > 1)
            {
                // TO DO: Notify ratio exceeding 1. Can help much if it goes something abnormal like > 1.2 implying source data being really bad.
                ratio = 1;
            }
            return MathF.Asin(ratio);
        }

        public static float AoBRadiansByTrigonometry(float rangeMeters, float absoluteLengthMeters, float visibleLengthRadians,
            AoBQuarter aoBQuarter)
        {
            float quarterAoBRadians = QuarterAoBRadiansByTrigonometry(rangeMeters, absoluteLengthMeters, visibleLengthRadians);

            return aoBQuarter switch
            {
                AoBQuarter.AheadRight => quarterAoBRadians,
                AoBQuarter.AsternRight => MathF.PI - quarterAoBRadians,
                AoBQuarter.AsternLeft => MathF.PI + quarterAoBRadians,
                AoBQuarter.AheadLeft => MathF.PI*2 - quarterAoBRadians,
                _ => float.NaN
            };
        }

        public static float LeadAngleRadiansFastAttack(float rangeMeters, float angularSpeedRpS, float torpedoSpeedMpS)
        {
            if (torpedoSpeedMpS <= 0) return float.NaN;

            // The lower the target speed is and the closer the real AoB is to 90 deg, the better this approximation works.
            float targetTangentialSpeedMpS = SpeedMpSStaticAngular(rangeMeters, angularSpeedRpS, MathF.PI / 2);
            float leadAngleSin = targetTangentialSpeedMpS / torpedoSpeedMpS;
            if (leadAngleSin > 1)
            {
                // TO DO: Notify leadAngleSin exceeding 1. Not like if shooting by this formula at large lead angles was a good idea, but still.
                leadAngleSin = 1;
            }
            return MathF.Asin(leadAngleSin);
        }

        public static float SpeedMpSStaticAngular(float rangeMeters, float angularSpeedRpS, float aoBRadians)
        {
            if (rangeMeters <= 0 ) return float.NaN;

            return rangeMeters * angularSpeedRpS / MathF.Sin(aoBRadians);
        }

        public static float SpeedMpSStaticLinear(float absoluteLengthMeters, float timeSeconds)
        {
            if (absoluteLengthMeters <= 0) return float.NaN;

            return absoluteLengthMeters / timeSeconds;
        }

        public static float SpeedMpSConstantBoatVelocityLinear(
            float absoluteLengthMeters, float timeSeconds, float boatSpeedMpS, float bearingRadians, float aoBRadians)
        {
            float tangentialBoatSpeedMpS = boatSpeedMpS * MathF.Sin(bearingRadians);
            float intersectionPointSpeed = -tangentialBoatSpeedMpS / MathF.Sin(aoBRadians);

            return SpeedMpSStaticLinear(absoluteLengthMeters, timeSeconds) + intersectionPointSpeed;
        }

        public static float AngularSpeedRpSByOneDegreeTime(float oneDegreeTimeSeconds)
        {
            if (oneDegreeTimeSeconds <= 0) return float.NaN;

            return Units.Degree.ToRadians(1)/oneDegreeTimeSeconds;
        }
    }
}
