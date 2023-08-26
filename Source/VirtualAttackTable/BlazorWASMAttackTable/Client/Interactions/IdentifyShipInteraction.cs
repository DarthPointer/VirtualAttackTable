using BlazorWASMAttackTable.Client.Interactions.Common;
using BlazorWASMAttackTable.Client.Interactions.Options;
using VirtualAttackTableLib.AttackTarget;
using VirtualAttackTableLib.TargetIdentification;
using CallbackList;
using BlazorWASMAttackTable.Client.Interactions.IdentificationInteractions;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;

namespace BlazorWASMAttackTable.Client.Interactions
{
    public class IdentifyShipInteraction
    {
        #region Fields
        private ListForSingleOption<Superstructure> _superstructure = null!;
        private ListForSingleOption<EnginePlacement> _enginePlacement = null!;

        private ThreeStateToggle _bowIsland = null!;
        private ThreeStateToggle _midIsland = null!;
        private ThreeStateToggle _sternIsland = null!;

        private IReadOnlyList<SelectTargetShipDataWrapInteraction> _filteredShipDatas = null!;
        private ValueInteraction<string> _structuresCode = null!;
        #endregion

        #region Properties
        public ListForSingleOption<Superstructure> Superstructure
        {
            get
            {
                return _superstructure;
            }
            private init
            {
                _superstructure = value;
                _superstructure.SelectedOption.ValueChanged.Subscribe(OnFilterValueChanged);
            }
        }

        public ListForSingleOption<EnginePlacement> EnginePlacement
        {
            get
            {
                return _enginePlacement;
            }
            private init
            {
                _enginePlacement = value;
                _enginePlacement.SelectedOption.ValueChanged.Subscribe(OnFilterValueChanged);
            }
        }

        public ThreeStateToggle BowIsland
        {
            get
            {
                return _bowIsland;
            }
            private init
            {
                _bowIsland = value;
                _bowIsland.ValueChanged.Subscribe(OnFilterValueChanged);
                //_bowIsland.ValueChanged += OnFilterValueChanged;
            }
        }

        public ThreeStateToggle MidIsland
        {
            get
            {
                return _midIsland;
            }
            private init
            {
                _midIsland = value;
                _midIsland.ValueChanged.Subscribe(OnFilterValueChanged);
                //_midIsland.ValueChanged += OnFilterValueChanged;
            }
        }

        public ThreeStateToggle SternIsland
        {
            get
            {
                return _sternIsland;
            }
            private init
            {
                _sternIsland = value;
                _sternIsland.ValueChanged.Subscribe(OnFilterValueChanged);
                //_sternIsland.ValueChanged += OnFilterValueChanged;
            }
        }

        public ValueInteraction<string> StructuresCode
        {
            get
            {
                return _structuresCode;
            }
            private init
            {
                _structuresCode = value;
                _structuresCode.ValueChanged.Subscribe(OnFilterValueChanged);
            }
        }

        private IReadOnlyList<SelectTargetShipDataWrapInteraction> AllTargetShipDatas
        {
            get;
            init;
        }

        public IReadOnlyList<SelectTargetShipDataWrapInteraction> FilteredShipDatas
        {
            get
            {
                return _filteredShipDatas;
            }
            private set
            {
                if (value != _filteredShipDatas)
                {
                    _filteredShipDatas = value;
                    FilteredShipDatasChanged.CreateFireCall()?.Invoke();
                }
            }
        }

        private TargetShipData? SelectedData
        {
            get;
            set;
        }

        private SelectTargetShipDataWrapInteraction? SelectedDataWrap
        {
            get
            {
                return SelectedWrap.Value;
            }
            set
            {
                if (SelectedWrap.Value != value)
                {
                    if (SelectedWrap.Value != null)
                    {
                        SelectedWrap.Value.IsSelected.Value = false;
                    }

                    SelectedWrap.Value = value;

                    if (value != null)
                    {
                        value.IsSelected.Value = true;
                    }

                    SelectedData = value?.TargetShipData;
                }
            }
        }

        public ValueInteraction<SelectTargetShipDataWrapInteraction?> SelectedWrap { get; } = new(null);

        public CallbackListManager FilteredShipDatasChanged
        {
            get;
        } = new();
        //public event Action? FilteredShipDatasChanged;

        public CallbackListManager<TargetShipData> TargetShipDataAccepted
        {
            get;
        } = new();
        //public event Action<TargetShipData>? TargetShipDataAccepted;
        #endregion

        #region Constructors
        public IdentifyShipInteraction(IEnumerable<TargetShipData> datas)
        {
            Superstructure = new ListForSingleOption<Superstructure>(ListOfOptions.GenerateEnumOptions<Superstructure>(), "Superstructure", allowNoOption: true);
            EnginePlacement = new ListForSingleOption<EnginePlacement>(ListOfOptions.GenerateEnumOptions<EnginePlacement>(), "Engine Placement", allowNoOption: true);

            BowIsland = new("Bow Island", null);
            MidIsland = new("Mid Island", null);
            SternIsland = new("Stern Island", null);

            StructuresCode = new("");

            AllTargetShipDatas = datas.Select(data => new SelectTargetShipDataWrapInteraction(data, this)).ToList();

            // UpdateFilteredDatas sets FilteredShipDatas
            FilteredShipDatas = null!;
            UpdateFilteredDatas();
            //
        }
        #endregion

        #region Methods
        private void UpdateFilteredDatas()
        {
            FilteredShipDatas = AllTargetShipDatas.Where(TargetShipDataFitsFilters).ToList();
        }

        private bool TargetShipDataFitsFilters(SelectTargetShipDataWrapInteraction wrap)
        {
            TargetShipData targetShipData = wrap.TargetShipData;

            if (Superstructure.SelectedOption.Value != null && Superstructure.SelectedOption.Value.Value != targetShipData.Superstructure) return false;
            if (EnginePlacement.SelectedOption.Value != null && EnginePlacement.SelectedOption.Value.Value != targetShipData.EnginePlacement) return false;

            if (BowIsland.Value != null && BowIsland.Value != targetShipData.IslandsPositions.HasFlag(IslandsPositions.Bow)) return false;
            if (MidIsland.Value != null && MidIsland.Value != targetShipData.IslandsPositions.HasFlag(IslandsPositions.Mid)) return false;
            if (SternIsland.Value != null && SternIsland.Value != targetShipData.IslandsPositions.HasFlag(IslandsPositions.Stern)) return false;

            if (StructuresCode.Value != "" && !MatchStructureCode(StructuresCode.Value, targetShipData.Structures)) return false;

            return true;
        }

        private bool MatchStructureCode(string pattern, IEnumerable<StructureType> shipStructures)
        {
            return string.Concat(shipStructures.Select(StructureTypeStringConversion.GetStructureStringCode)) == pattern;
        }

        private void OnFilterValueChanged<T>(T filterValue)
        {
            UpdateFilteredDatas();
        }

        private void NotifyTargetShipDataAccepted(TargetShipData targetShipData)
        {
            TargetShipDataAccepted.CreateFireCall()?.Invoke(targetShipData);
        }

        public void SelectData(SelectTargetShipDataWrapInteraction wrap)
        {
            if (SelectedDataWrap != wrap)
            {
                SelectedDataWrap = wrap;
                return;
            }
        }

        public void AcceptDataFromSelectedWrap()
        {
            if (SelectedDataWrap != null && FilteredShipDatas.Contains(SelectedDataWrap))
            {
                NotifyTargetShipDataAccepted(SelectedDataWrap.TargetShipData);
            }
        }
        #endregion
    }
}
