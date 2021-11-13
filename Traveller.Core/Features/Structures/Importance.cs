namespace Traveller.Core.Features.Structures;
public record struct Importance
{
    public readonly int RawImportance;

    public Importance(int importance) => this.RawImportance = importance;

    public Importance(string importance)
    {
        var trimmed = importance.Trim().TrimStart('{').TrimEnd('}').Trim();
        RawImportance = int.Parse(trimmed);
    }

    public override string ToString() => "{" + RawImportance + "}";

    public bool Equals(Importance other) => other.RawImportance == RawImportance;
    public static bool operator >(Importance left, Importance right) =>
        left.RawImportance > right.RawImportance;
    public static bool operator <(Importance left, Importance right) =>
        left.RawImportance < right.RawImportance;
}
