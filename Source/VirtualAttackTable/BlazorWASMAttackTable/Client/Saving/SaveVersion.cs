using BlazorWASMAttackTable.Client.Saving.SaveObjectVersions;
using BlazorWASMAttackTable.Client.Saving.SaveObjectVersions.CurrentVersion;

namespace BlazorWASMAttackTable.Client.Saving
{
    public class SaveVersion
    {
        public static SaveVersion Current { get; }

        public static IReadOnlyDictionary<string, SaveVersion> Versions { get; }

        #region Properties
        public required string VersionCode { get; init; }

        public required Type SaveRootClass { get; init; }
        #endregion

        static SaveVersion()
        {
            Dictionary<string, SaveVersion> versions = new();

            Current = new()
            {
                VersionCode = "0.3.0",
                SaveRootClass = typeof(Current)
            };

            AddVersion(versions, Current);

            Versions = versions;
        }

        private static void AddVersion(Dictionary<string, SaveVersion> versions, SaveVersion version)
        {
            versions.Add(version.VersionCode, version);
        }
    }
}
