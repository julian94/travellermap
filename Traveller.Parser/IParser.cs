namespace Traveller.Parser;
public interface IParser
{
    public static bool CanParse(string extension) => false;

    public static bool TryParseSector(string inputSector, string? inputMetadata, out Sector result)
    {
        result = null;
        return false;
    }
}
