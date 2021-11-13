namespace Traveller.Core.Features.Structures;
public record struct Culture
{
    public readonly EHex Homogeneity;
    public readonly EHex Accaptance;
    public readonly EHex Strangeness;
    public readonly EHex Symbols;


    public Culture(string culture)
    {
        var trimmed = culture.TrimStart('[').TrimEnd(']').Trim();
        Homogeneity = new EHex(trimmed[0]);
        Accaptance = new EHex(trimmed[1]);
        Strangeness = new EHex(trimmed[2]);
        Symbols = new EHex(trimmed[3]);
    }

    public override string ToString() =>
        "[" +
        Homogeneity.ToString() +
        Accaptance.ToString() +
        Strangeness.ToString() +
        Symbols.ToString() +
        "]";
}
