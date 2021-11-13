using System.Text.RegularExpressions;

namespace Traveller.Core.Features.Structures;

public record struct UWP
{
    public EHex Starport { get; init; }
    public EHex Size { get; init; }
    public EHex Atmosphere { get; init; }
    public EHex Hydrology { get; init; }
    public EHex Population { get; init; }
    public EHex Government { get; init; }
    public EHex Law { get; init; }
    public EHex Tech { get; init; }

    public UWP(string raw)
    {
        if (!Regex.IsMatch(raw, "[ABCDEX][0-9A-Z]{6}-[0-9A-Z]")) throw new ArgumentException("Invalid UWP");
        var parts = raw.Replace("-", string.Empty).ToCharArray();
        var parsedParts = parts.Select(p => new EHex(p)).ToArray();
        Starport = parsedParts[0];
        Size = parsedParts[1];
        Atmosphere = parsedParts[2];
        Hydrology = parsedParts[3];
        Population = parsedParts[4];
        Government = parsedParts[5];
        Law = parsedParts[6];
        Tech = parsedParts[7];
    }

    public bool Equals(UWP other)
        => ToString().Equals(other.ToString());

    public override string ToString()
    {
        return $"{Starport}{Size}{Atmosphere}{Hydrology}{Population}{Government}{Law}-{Tech}";
    }
}
