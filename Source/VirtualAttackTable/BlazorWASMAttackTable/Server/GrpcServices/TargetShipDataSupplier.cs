using BlazorWASMAttackTable.Shared.Protos;
using Grpc.Core;
using Newtonsoft.Json;
using VirtualAttackTableLib.AttackTarget;
using static BlazorWASMAttackTable.Shared.Protos.TargetShipdDataSupplierService;

namespace BlazorWASMAttackTable.Server.GrpcServices
{
    public class TargetShipDataSupplier : TargetShipdDataSupplierServiceBase
    {
        private const string TargetShipDatasAssetSubfolder = "ServerAssets/TargetShipDatas";

        private TargetShipdDataLoadResponse ConstantResponse { get; }

        public TargetShipDataSupplier() : base()
        {
            string datasFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TargetShipDatasAssetSubfolder);

            ConstantResponse = new();
            ConstantResponse.Datas.Add(GetDatasFromFolder(datasFolder).Select(data => data.ToBuffer()));
        }

        public override Task<TargetShipdDataLoadResponse> ProvideDatas(TargetShipdDataLoadRequest request, ServerCallContext context)
        {
            return Task.FromResult(ConstantResponse);
        }

        private static TargetShipData DataFromFile(string filePath)
        {
            return JsonConvert.DeserializeObject<TargetShipData>(File.ReadAllText(filePath)) ?? throw new Exception($"Failed to deserialize {filePath} as {typeof(TargetShipData)}.");
        }

        private static IEnumerable<TargetShipData> GetDatasFromFolder(string folder)
        {
            foreach (string filePath in Directory.EnumerateFiles(folder, "*.json"))
            {
                yield return DataFromFile(filePath);
            }
        }
    }
}
