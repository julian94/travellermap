using Traveller.Core.Features.Structures;

namespace Traveller.Core.Features;
public class World
{
    public string? Name { get; set; }
    public UWP? Uwp { get; set; }
    public TravelCode? TravelCode { get; set; }
    public Importance? Importance { get; set; }
    public Culture? Culture { get; set; }
    public Economic? Economic { get; set; }

    public int PopulationModifier { get; set; }

    public virtual bool Equals(World other) =>
        Name == other.Name &&
        Uwp.Equals(other.Uwp) &&
        TravelCode == other.TravelCode &&
        Importance == other.Importance &&
        Culture == other.Culture &&
        Economic == other.Economic &&
        PopulationModifier == other.PopulationModifier;
}
