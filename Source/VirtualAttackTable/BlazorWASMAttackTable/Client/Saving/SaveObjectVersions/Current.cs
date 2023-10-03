using BlazorWASMAttackTable.Client.Saving.SaveObjectVersions.CurrentVersion;

namespace BlazorWASMAttackTable.Client.Saving.SaveObjectVersions
{
    public class Current : ISaveObject
    {
        #region Properties
        public Session Session { get; set; } = null!;
        #endregion

        Current ISaveObject.ToCurrent()
        {
            return this;
        }
    }
}
