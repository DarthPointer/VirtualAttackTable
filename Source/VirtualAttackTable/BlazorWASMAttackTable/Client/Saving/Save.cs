using BlazorWASMAttackTable.Client.Saving.SaveObjectVersions;
using BlazorWASMAttackTable.Client.Saving.SaveObjectVersions.CurrentVersion;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlazorWASMAttackTable.Client.Saving
{
    public class Save
    {
        public string versionCode = string.Empty;

        public JToken saveObject = new JObject();

        public Current GetCurrentVersionSaveObject()
        {
            Type loadedSaveVersionClass = SaveVersion.Versions[versionCode].SaveRootClass;

            ISaveObject isaveObject = (saveObject.ToObject(loadedSaveVersionClass) as ISaveObject) ??
                throw new Exception($"Failed to get current version save object because the serialized object saved as version {versionCode} could not be loaded as {loadedSaveVersionClass}.");

            return isaveObject.ToCurrent();
        }


    }
}
