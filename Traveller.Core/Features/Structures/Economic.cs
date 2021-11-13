namespace Traveller.Core.Features.Structures;
public record struct Economic
{
    public readonly EHex Resources;
    public readonly EHex Labour;
    public readonly EHex Infrastructure;
    public readonly int Efficiency;


    public Economic(string economic)
    {
        var trimmed = economic.TrimStart('(').TrimEnd(')').Trim();
        Resources = new EHex(trimmed[0]);
        Labour = new EHex(trimmed[1]);
        Infrastructure = new EHex(trimmed[2]);
        Efficiency = int.Parse(trimmed[3..]);
    }

    public override string ToString() =>
        "(" +
        Resources.ToString() +
        Labour.ToString() +
        Infrastructure.ToString() +
        (Efficiency >= 0 ? "+" : string.Empty) +
        Efficiency.ToString() +
        ")";
}
