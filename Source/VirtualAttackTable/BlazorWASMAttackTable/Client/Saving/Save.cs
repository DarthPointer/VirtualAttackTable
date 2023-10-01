using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlazorWASMAttackTable.Client.Saving
{
    public class Save
    {
        public string versionCode = string.Empty;

        public JToken session = new JObject();
    }
}
