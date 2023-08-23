namespace BlazorWASMAttackTable.Client.Interactions.Common
{
    public class ThreeStateToggle : ValueInteraction<bool?>
    {
        #region Properties
        public string Header
        {
            get;
            private init;
        }
        #endregion

        #region Constructors
        public ThreeStateToggle(string header, bool? initialValue) : base(initialValue)
        {
            Header = header;
        }
        #endregion

        #region Methods
        public void Toggle()
        {
            Value = Value switch
            {
                null => true,
                true => false,
                false => null,
            };
        }
        #endregion
    }
}
