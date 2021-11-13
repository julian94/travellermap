using System.Text.Json;

namespace Traveller.Parser;
public class JsonParser: IParser
{
    public const string JsonExtension = "json";
    public static bool CanParse(string extension) => JsonExtension.Equals(extension.Trim().TrimStart('.'));

    public static bool TryParseSector(string inputSector, string? inputMetadata, out Sector result)
    {
        result = null;
        return false;
    }
}
