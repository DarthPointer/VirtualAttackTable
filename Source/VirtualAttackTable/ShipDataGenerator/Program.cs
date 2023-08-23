using IronSoftware.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using SixLabors.ImageSharp.Formats;
using System.Diagnostics;
using System.IO.Enumeration;
using VirtualAttackTableLib;
using VirtualAttackTableLib.AttackTarget;
using VirtualAttackTableLib.TargetIdentification;
using VYaml.Serialization;
using static System.Formats.Asn1.AsnWriter;

namespace ShipDataGenerator
{
    internal class Program
    {
        private const string SHIP_PREFABS_FOLDER = "Resources";
        private const string SHIP_IMAGES_FOLDER = "Texture2D";

        private const string HF_PREFAB_PATTERN = "heavy_freighter*.prefab";
        private const string HT_PREFAB_PATTERN = "heavy_tanker*.prefab";
        private const string LM_PREFAB_PATTERN = "light_merchant*.prefab";
        private const string MT_PREFAB_PATTERN = "medium_tanker*.prefab";
        private const string PL_PREFAB_PATTERN = "passenger_ship*.prefab";

        /// <summary>
        /// Filename pattern for metadata of .png assets.
        /// </summary>
        private const string PNG_META_PATTERN = "*.png.meta";

        private const string JSON_EXT = ".json";
        private const string PNG_EXT = ".png";

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Error.WriteLine($"The program requires 2 parameters to run (source and target directories), but {args.Length} were provided.");
            }

            string fromDirectory = args[0];
            string toDirectory = args[1];

            if (fromDirectory.IndexOfAny(Path.GetInvalidPathChars()) > -1)
            {
                Console.Error.WriteLine($"Source directory specified \"{fromDirectory}\" is not a valid path.");
                return;
            }

            if (!Directory.Exists(fromDirectory))
            {
                Console.Error.WriteLine($"Source directory specified \"{fromDirectory}\" does not exist.");
                return;
            }

            string prefabsDirectory = Path.Combine(fromDirectory, SHIP_PREFABS_FOLDER);
            string imagesDirectory = Path.Combine(fromDirectory, SHIP_IMAGES_FOLDER);

            if (!Directory.Exists(prefabsDirectory))
            {
                Console.Error.WriteLine($"Ship prefabs were expected to be located in \"{prefabsDirectory}\", but this directory does not exist.");
                return;
            }

            if (!Directory.Exists(prefabsDirectory))
            {
                Console.Error.WriteLine($"Ship images were expected to be located in \"{imagesDirectory}\", but this directory does not exist.");
                return;
            }

            if (toDirectory.IndexOfAny(Path.GetInvalidPathChars()) > -1)
            {
                Console.Error.WriteLine($"Target directory specified \"{toDirectory}\" is not a valid path.");
            }

            TransferPrefabs(prefabsDirectory, toDirectory);

            TransferImages(imagesDirectory, toDirectory);

            GenerateShipDataJsons(toDirectory);
        }

        /// <summary>
        /// Save .jsons with configs of MonobeHaviours with relevant gameplay data configs.
        /// </summary>
        /// <param name="prefabsDirectory"></param>
        /// <param name="toDirectory"></param>
        private static void TransferPrefabs(string prefabsDirectory, string toDirectory)
        {
            Console.WriteLine("Transfering ship prefabs");

            Directory.CreateDirectory(toDirectory);
            foreach (string currentShipPrefabPath in GetSourcePrefabsPaths(prefabsDirectory))
            {
                List<dynamic> docs = YamlSerializer.DeserializeMultipleDocuments<dynamic>(File.ReadAllBytes(currentShipPrefabPath)).ToList();

                string shipAssetsName = Path.GetFileNameWithoutExtension(currentShipPrefabPath);

                Console.WriteLine(shipAssetsName);
                
                JArray jsonDocs = (JArray)JToken.FromObject(docs);

                File.WriteAllText(path: Path.Combine(toDirectory, shipAssetsName + JSON_EXT), contents:

                    // The prefab contains a set of configs for ship's objects, we only need one of them.
                    jsonDocs.Children<JObject>().
                    // The elements created from the YAML "documents" have one property with its key being the type and the value being the actual data,
                    // which should be represented with a JObject token.
                    SelectMany(element => element.Children<JProperty>().Select(prop => prop.Value as JObject)).
                    // Then we need the element data that has the "NameOfShipType" property which indicates it is the MonoBehaviour we need.
                    First(doc => doc?.ContainsKey("NameOfShipType") == true)?.ToString());
            }
        }

        /// <summary>
        /// Transfer .png assets of images for the parsed prefabs.
        /// </summary>
        /// <param name="imagesDirectory">The directory with .pngs to fetch from.</param>
        /// <param name="toDirectory">The directory to copy needed .pngs into for parsed prefabs already located there after using <see cref="TransferPrefabs(string, string)"/>.</param>
        private static void TransferImages(string imagesDirectory, string toDirectory)
        {
            Console.WriteLine("Mapping images");
            Dictionary<Guid, string> imagesMap = MapImages(imagesDirectory);
            Console.WriteLine("Images mapped");

            Console.WriteLine("Transferring images");
            foreach (string prefabJsonPath in Directory.EnumerateFiles(toDirectory, "*" + JSON_EXT, SearchOption.TopDirectoryOnly))
            {
                JObject? prefabJobject = JToken.Parse(File.ReadAllText(prefabJsonPath)) as JObject;
                JObject? imagePropValue = prefabJobject?.GetValue("image") as JObject;
                Guid? imageGuid = (imagePropValue?.GetValue("guid") as JValue)?.ToObject<Guid>();

                if (imageGuid != null)
                {
                    string imageTargetPath = Path.ChangeExtension(prefabJsonPath, PNG_EXT).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                    string imageSourcePath = imagesMap[imageGuid.Value].Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

                    Console.WriteLine($"Image {imageSourcePath} fetched by guid {imageGuid}");

                    File.Copy(sourceFileName: imagesMap[imageGuid.Value], destFileName: imageTargetPath, overwrite: true);
                }
            }
        }

        /// <summary>
        /// Create a dictionary to access required images from the folder by asset GUID.
        /// </summary>
        /// <param name="imagesDirectory"></param>
        /// <returns></returns>
        private static Dictionary<Guid, string> MapImages(string imagesDirectory)
        {
            return
            
            (from filePath in Directory.EnumerateFiles(imagesDirectory, PNG_META_PATTERN, SearchOption.TopDirectoryOnly)
            select (
                propValue: GetGuidJPropertyValueFromMeta(filePath),
                filePath
                )
                into guidMapping

            where guidMapping.propValue is JValue select (guid: guidMapping.propValue.ToObject<Guid>(), guidMapping.filePath)).
            
            ToDictionary(mapping => mapping.guid, mapping => mapping.filePath.Substring(0, mapping.filePath.LastIndexOf('.')));
        }

        private static JToken? GetGuidJPropertyValueFromMeta(string metaPath)
        {
            return ((JObject)JToken.FromObject(
                YamlSerializer.Deserialize<dynamic>(
                File.ReadAllBytes(metaPath)))).
                GetValue("guid", StringComparison.Ordinal);
        }

        private static void GenerateShipDataJsons(string targetDirectory)
        {
            foreach (string jsonPath in Directory.EnumerateFiles(targetDirectory, '*' + JSON_EXT, SearchOption.TopDirectoryOnly))
            {
                JObject prefabJson = (JObject)JToken.Parse(File.ReadAllText(jsonPath));

                float GRT = prefabJson.GetValue("Displacement")!.ToObject<float>();
                float maxHeightMeters = prefabJson.GetValue("height")!.ToObject<float>();
                float lengthMeters = prefabJson.GetValue("ShipLength")!.ToObject<float>();

                var (left, top, right, bottom, imageWidth, imageHeight) = GetShipImageEdges(Path.ChangeExtension(jsonPath, PNG_EXT));

                FloatRange verticalImageRange = new() { Start = (float)top / imageHeight, End = (float)bottom / imageHeight };
                FloatRange horizontalImageRange = new() { Start = (float)left / imageWidth, End = (float)right / imageWidth };

                Console.WriteLine($"{Path.GetFileNameWithoutExtension(jsonPath)}: ship inside left {left} top {top} right {right} bottom {bottom}");

                float draughtMeters = prefabJson.GetValue("draught")!.ToObject<float>();

                float maxSpeedMpS = Units.Knot.ToMpS(prefabJson.GetValue("MaxSpeed")!.ToObject<float>());

                string typeName = prefabJson.GetValue("NameOfShipType")!.ToObject<string>()!;

                EnginePlacement enginePlacement = (EnginePlacement)prefabJson.GetValue("enginePlacement")!.ToObject<int>();
                Superstructure superstructure = (Superstructure)prefabJson.GetValue("superStructure")!.ToObject<int>();

                IslandsPositions islandsPositions = IslandsPositions.None;

                if (prefabJson.GetValue("FrontIsland")!.ToObject<float>() > 0) islandsPositions |= IslandsPositions.Bow;
                if (prefabJson.GetValue("MidIsland")!.ToObject<float>() > 0) islandsPositions |= IslandsPositions.Mid;
                if (prefabJson.GetValue("BackIsland")!.ToObject<float>() > 0) islandsPositions |= IslandsPositions.Stern;

                string masts = prefabJson.GetValue("Masts")!.ToObject<string>()!;
                List<StructureType> structures = masts.Select(StructureTypeStringConversion.GetStructureType).
                ToList();

                TargetShipData targetShipData = new()
                {
                    GRT = GRT,

                    MaxHeightMeters = maxHeightMeters,
                    VerticalImageRange = verticalImageRange,

                    LengthMeters = lengthMeters,
                    HorizontalImageRange = horizontalImageRange,

                    DraughtMeters = draughtMeters,

                    MaxSpeedMpS = maxSpeedMpS,

                    TypeName = typeName,

                    EnginePlacement = enginePlacement,
                    IslandsPositions = islandsPositions,
                    Superstructure = superstructure,
                    Structures = structures
                };

                File.WriteAllText(jsonPath, JsonConvert.SerializeObject(targetShipData, Formatting.Indented));
            }
        }


        private static IEnumerable<string> GetSourcePrefabsPaths(string prefabsDirectory)
        {
            // Perhaps there is *the best* way to make it "fast" and "safe"? Gotta google it up when I start polishing the code.
            List<string> result = Directory.EnumerateFiles(prefabsDirectory, HF_PREFAB_PATTERN, SearchOption.TopDirectoryOnly).ToList();
            result.AddRange(Directory.EnumerateFiles(prefabsDirectory, HT_PREFAB_PATTERN, SearchOption.TopDirectoryOnly));
            result.AddRange(Directory.EnumerateFiles(prefabsDirectory, LM_PREFAB_PATTERN, SearchOption.TopDirectoryOnly));
            result.AddRange(Directory.EnumerateFiles(prefabsDirectory, MT_PREFAB_PATTERN, SearchOption.TopDirectoryOnly));
            result.AddRange(Directory.EnumerateFiles(prefabsDirectory, PL_PREFAB_PATTERN, SearchOption.TopDirectoryOnly));

            return result;
        }

        private static (int left, int top, int right, int bottom, int imageWidth, int imageHeight) GetShipImageEdges(string imagePath)
        {
            // The lib docs explicitly say to not alter that for default configuration, but the morons could not provide how-to examples in the docpages of the class.
            // Having to resetup the entire shitfest of worker and factory objects to change one bool flag is utterly deranged and not something I'm willing to do.
            SixLabors.ImageSharp.Configuration.Default.PreferContiguousImageBuffers = true;

            using Image<Rgba32> image = Image.Load<Rgba32>(imagePath);

            // I disregard the flag result. In theory I'm not getting images that could make the memory spans incontiguous.
            image.DangerousTryGetSinglePixelMemory(out Memory<Rgba32> pixels);

            Span<Rgba32> imageBytes = pixels.Span;

            int bottom = GetShipBottom(imageBytes, image.Width, image.Height);
            int left = GetShipLeft(imageBytes, image.Width, bottom);
            int right = GetShipRight(imageBytes, image.Width, bottom);
            int top = GetShipTop(imageBytes, image.Width, left, right, bottom);
            return (left, top, right, bottom, image.Width, image.Height);
        }

        private static int GetShipBottom(Span<Rgba32> imageBytes, int imageWidth, int imageHeight)
        {
            int bottomByMiddle = imageHeight;
            int middleX = imageWidth / 2;

            while (bottomByMiddle > 0)
            {
                bottomByMiddle--;

                if (imageBytes[bottomByMiddle * imageWidth + middleX].A > 0) break;
            }

            int bottom = bottomByMiddle;
            bool bottomConfirmed = false;

            while (bottom < imageHeight - 1)
            {
                int testBottom = bottom + 1;
                int rowOffset = testBottom * imageWidth;

                for (int x = 0; x < imageWidth; x++)
                {
                    bottomConfirmed = imageBytes[rowOffset + x].A == 0;
                    if (bottomConfirmed) break;
                }

                if (bottomConfirmed) break;

                bottom++;
            }

            return bottom;
        }

        private static int GetShipLeft(Span<Rgba32> imageBytes, int imageWidth, int shipBottom)
        {
            // The leftmost column with confirmed ship pixels.
            int shipLeft = imageWidth / 2;
            // The rightmost column with confirmed no ship pixels.
            int emptyRight = 0;

            while (emptyRight != shipLeft - 1)
            {
                int testColumn = (emptyRight + shipLeft) / 2;

                int startRowOffset = shipBottom * imageWidth;

                bool shipPixelsDetected = false;
                // Starting at precalculated bottom, scanning upwards until we find a pixel or run above the image.
                for (int i = startRowOffset + testColumn; i > 0; i -= imageWidth)
                {
                    shipPixelsDetected = imageBytes[i].A > 0;

                    if (shipPixelsDetected) break;
                }

                if (shipPixelsDetected)
                    shipLeft = testColumn;
                else
                    emptyRight = testColumn;
            }

            return shipLeft;
        }

        private static int GetShipRight(Span<Rgba32> imageBytes, int imageWidth, int shipBottom)
        {
            // The rightmost column with confirmed ship pixels.
            int shipRight = imageWidth / 2;
            // The leftmost column with confirmed no ship pixels.
            int emptyLeft = imageWidth - 1;

            while (emptyLeft != shipRight + 1)
            {
                int testColumn = (emptyLeft + shipRight) / 2;

                int startRowOffset = shipBottom * imageWidth;

                bool shipPixelsDetected = false;
                // Starting at precalculated bottom, scanning upwards until we find a pixel or run above the image.
                for (int i = startRowOffset + testColumn; i > 0; i -= imageWidth)
                {
                    shipPixelsDetected = imageBytes[i].A > 0;

                    if (shipPixelsDetected) break;
                }

                if (shipPixelsDetected)
                    shipRight = testColumn;
                else
                    emptyLeft = testColumn;
            }

            return shipRight;
        }

        private static int GetShipTop(Span<Rgba32> imageBytes, int imageWidth, int shipLeft, int shipRight, int shipBottom)
        {
            // The highest row with confirmed ship pixels.
            int shipTop = shipBottom;
            // The lowest row with no confirmed ship pixels.
            int emptyBottom = 0;

            while (emptyBottom != shipTop - 1)
            {
                int testRow = (emptyBottom + shipTop) / 2;

                int testRowOffset = testRow * imageWidth;

                bool shipPixelsDetected = false;
                // Scanning within precalculated ship width.
                for (int x = shipLeft; x <= shipRight; x++)
                {
                    shipPixelsDetected = imageBytes[testRowOffset + x].A > 0;

                    if (shipPixelsDetected) break;
                }

                if (shipPixelsDetected)
                    shipTop = testRow;
                else
                    emptyBottom = testRow;
            }

            return shipTop;
        }

        private static bool KVPIsShipTypeName(dynamic kvp) => kvp.Key == "NameOfShipType";
    }
}