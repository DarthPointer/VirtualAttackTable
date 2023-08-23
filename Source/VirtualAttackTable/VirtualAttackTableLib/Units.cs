using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAttackTableLib
{
    public static class Units
    {
        #region Length
        public static readonly LengthUnit Meter = new() { UnitsPerMeter = 1, UnitName = "m" };
        public static readonly LengthUnit HectoMeter = new() { UnitsPerMeter = 0.01f, UnitName = "Hm" };
        public static readonly LengthUnit KiloMeter = new() { UnitsPerMeter = 0.001f, UnitName = "km" };
        public static readonly LengthUnit Yard = new() { UnitsPerMeter = 1.09361f, UnitName = "yd" };
        #endregion

        #region Time
        public static readonly TimeUnit Second = new() { UnitsPerSecond = 1, UnitName = "s" };
        public static readonly TimeUnit Minute = new() { UnitsPerSecond = 1f / 60, UnitName = "min" };
        public static readonly TimeUnit Hour = new() { UnitsPerSecond = 1f / 3600, UnitName = "hr" };
        #endregion

        #region Speed
        public static readonly SpeedUnit MpS = new() { UnitsPerMpS = 1, UnitName = "m/s" };
        /// <summary>
        /// aka Nau Mile per Hour
        /// </summary>
        public static readonly SpeedUnit Knot = new() { UnitsPerMpS = 1.94384f, UnitName = "kt" };
        public static readonly SpeedUnit KilometersPerHour = new SpeedUnit() { UnitsPerMpS = 3.6f, UnitName = "km/h" };
        #endregion

        #region Angle
        public static readonly AngleUnit Radian = new() { UnitsPerRadian = 1, UnitName = "R" };

        public static readonly AngleUnit Degree = new() { UnitsPerRadian = 57.2958f, UnitName = "deg" };
        public static readonly AngleUnit DegreeMinute = new() { UnitsPerRadian = 3437.747f, UnitName = "amin" };

        public static readonly AngleUnit Gradian = new() { UnitsPerRadian = 63.662f, UnitName = "grad" };
        #endregion

        #region AngularSpeed
        public static readonly AngularSpeedUnit RadianPerSecond = new() { UnitsPerRpS = 1, UnitName = "R/s" };
        public static readonly AngularSpeedUnit Hertz = new() { UnitsPerRpS = 0.159155f, UnitName = "Hz" };

        public static readonly AngularSpeedUnit DegreePerSecond = new() { UnitsPerRpS = Degree.FromRadians(1), UnitName = "Degree per Second" };
        #endregion
    }

    public interface IUnit
    {
        string UnitName { get; }

        float ToBase(float units);
        float FromBase(float baseUnits);
    }

    public class LengthUnit : IUnit
    {
        public required string UnitName { get; init; }
        public required float UnitsPerMeter { get; init; }

        float IUnit.ToBase(float units) => ToMeters(units);
        public float ToMeters(float units) => units / UnitsPerMeter;

        float IUnit.FromBase(float baseUnits) => FromMeters(baseUnits);
        public float FromMeters(float meters) => meters * UnitsPerMeter;
    }

    public class TimeUnit : IUnit
    {
        public required string UnitName { get; init; }
        public required float UnitsPerSecond { get; init; }

        float IUnit.ToBase(float units) => ToSeconds(units);
        public float ToSeconds(float units) => units / UnitsPerSecond;

        float IUnit.FromBase(float baseUnits) => FromSeconds(baseUnits);
        public float FromSeconds(float seconds) => seconds * UnitsPerSecond;
    }

    public class SpeedUnit : IUnit
    {
        public required string UnitName { get; init; }
        public required float UnitsPerMpS { get; init; }

        float IUnit.ToBase(float units) => ToMpS(units);
        public float ToMpS(float units) => units / UnitsPerMpS;

        float IUnit.FromBase(float baseUnits) => FromMpS(baseUnits);
        public float FromMpS(float MpS) => MpS * UnitsPerMpS;
    }

    public class AngleUnit : IUnit
    {
        public required string UnitName { get; init; }
        public required float UnitsPerRadian { get; init; }

        float IUnit.ToBase(float units) => ToRadians(units);
        public float ToRadians(float units) => units / UnitsPerRadian;

        float IUnit.FromBase(float baseUnits) => FromRadians(baseUnits);
        public float FromRadians(float radians) => radians * UnitsPerRadian;
    }

    public class AngularSpeedUnit: IUnit
    {
        public required string UnitName { get; init; }
        public required float UnitsPerRpS { get; init; }

        float IUnit.ToBase(float units) => ToRpS(units);
        public float ToRpS(float units) => units / UnitsPerRpS;

        float IUnit.FromBase(float baseUnits) => FromRpS(baseUnits);
        public float FromRpS(float rps) => rps * UnitsPerRpS;
    }
}
