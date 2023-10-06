using BlazorWASMAttackTable.Client.Interactions.Options;
using VirtualAttackTableLib;

namespace BlazorWASMAttackTable.Client.Interactions.AttackTableInteractions
{
    public class UnitsSelection
    {
        #region Custom Units
        private static readonly AngleUnit _grid1per16 = new AngleUnit() { UnitsPerRadian = Units.Degree.FromRadians(1) * 16, UnitName = "1/16°" };
        private static readonly AngleUnit _grid1per16QuadZoom = new AngleUnit() { UnitsPerRadian = Units.Degree.FromRadians(1) * 16 * 4, UnitName = "1/16° x4" };

        private static readonly AngleUnit _grid1per10 = new() { UnitsPerRadian = Units.Degree.FromRadians(1) * 10, UnitName = "1/10°" };
        private static readonly AngleUnit _grid1per10QuadZoom = new() { UnitsPerRadian = Units.Degree.FromRadians(1) * 10 * 4, UnitName = "1/10° x4" };
        #endregion

        #region Static Units Lists
        private static readonly IReadOnlyList<Option<AngleUnit>> _angleUnits = GenerateOptions(Units.Degree, Units.DegreeMinute, Units.Gradian, Units.Radian).ToList();

        private static readonly IReadOnlyList<Option<AngleUnit>> _wpPeriscopeVertical = GenerateOptions(_grid1per16, _grid1per16QuadZoom).ToList();
        private static readonly IReadOnlyList<Option<AngleUnit>> _wpPeriscopeHorizontal = GenerateOptions(_grid1per10, _grid1per10QuadZoom).ToList();

        private static readonly IReadOnlyList<Option<LengthUnit>> _lengthUnits = GenerateOptions(Units.Meter, Units.HectoMeter, Units.KiloMeter, Units.Yard).ToList();

        public static readonly IReadOnlyList<Option<TimeUnit>> _timeUnits = GenerateOptions(Units.Second, Units.Minute, Units.Hour).ToList();

        public static readonly IReadOnlyList<Option<SpeedUnit>> _speedUnits = GenerateOptions(Units.Knot, Units.MpS, Units.KilometersPerHour).ToList();

        public static readonly IReadOnlyList<Option<AngularSpeedUnit>> _angularSpeedUnits = GenerateOptions(Units.DegreePerSecond, Units.RadianPerSecond, Units.Hertz).ToList();
        #endregion

        #region Properties
        public AttackTableInteraction OwningAttackTable { get; }

        private List<IListForSingleOption> _unitOptionLists = new List<IListForSingleOption>();
        public IReadOnlyList<IListForSingleOption> UnitOptionLists => _unitOptionLists;

        public ListForSingleOption<AngleUnit> Bearing { get; }
        public ListForSingleOption<AngleUnit> TargetHeading { get; }

        public ListForSingleOption<LengthUnit> AbsoluteHeight { get; }
        public ListForSingleOption<AngleUnit> VisibleHeight { get; }
        public ListForSingleOption<LengthUnit> AbsoluteLength { get; }
        public ListForSingleOption<AngleUnit> VisibleLength { get; }

        public ListForSingleOption<LengthUnit> TargetRange { get; }

        public ListForSingleOption<AngleUnit> AoB { get; }

        public ListForSingleOption<TimeUnit> HullTime { get; }
        public ListForSingleOption<TimeUnit> OneDegreeTime { get; }

        public ListForSingleOption<SpeedUnit> TargetSpeed { get; }
        public ListForSingleOption<AngularSpeedUnit> AngularTargetSpeed { get; }

        public ListForSingleOption<SpeedUnit> TorpedoSpeed { get; }

        public ListForSingleOption<AngleUnit> BoatHeading { get; }
        public ListForSingleOption<SpeedUnit> BoatSpeed { get; }

        public ListForSingleOption<AngleUnit> LeadAngle { get; }
        #endregion

        #region Constructors
        public UnitsSelection(AttackTableInteraction owningAttackTable)
        {
            OwningAttackTable = owningAttackTable;

            Bearing = CreateUnitOptionsList(_angleUnits, "Bearing");
            TargetHeading = CreateUnitOptionsList(_angleUnits, "Target Heading");

            AbsoluteHeight = CreateUnitOptionsList(_lengthUnits, "Absolute Height");
            VisibleHeight = CreateUnitOptionsList(_wpPeriscopeVertical, "Visible Height");
            AbsoluteLength = CreateUnitOptionsList(_lengthUnits, "Absolute Length");
            VisibleLength = CreateUnitOptionsList(_wpPeriscopeHorizontal, "Visible Length");

            TargetRange = CreateUnitOptionsList(_lengthUnits, "Target Range");

            AoB = CreateUnitOptionsList(_angleUnits, "AoB");

            HullTime = CreateUnitOptionsList(_timeUnits, "Hull Time");
            OneDegreeTime = CreateUnitOptionsList(_timeUnits, "One Degree Time");

            TargetSpeed = CreateUnitOptionsList(_speedUnits, "Target Speed");
            AngularTargetSpeed = CreateUnitOptionsList(_angularSpeedUnits, "Angular Target Speed");

            TorpedoSpeed = CreateUnitOptionsList(_speedUnits, "Torpedo Speed");

            BoatHeading = CreateUnitOptionsList(_angleUnits, "Boat Heading");
            BoatSpeed = CreateUnitOptionsList(_speedUnits, "Boat Speed");

            LeadAngle = CreateUnitOptionsList(_angleUnits, "Lead Angle");
        }
        #endregion

        #region Methods
        private ListForSingleOption<T> CreateUnitOptionsList<T>(IEnumerable<Option<T>> options, string header)
            where T : IUnit
        {
            ListForSingleOption<T> result = new(options, header);
            _unitOptionLists.Add(result);

            return result;
        }


        private static IEnumerable<Option<T>> GenerateOptions<T>(params T[] units)
            where T : IUnit
        {
            return units.Select(unit => new Option<T>(unit, unit.UnitName));
        }
        #endregion
    }
}
