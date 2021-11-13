using Traveller.Core.Features.Structures;

namespace Traveller.Core.Features;
public class World
{
    public string? Name { get; set; }
    public UWP? Uwp { get; set; }
    public Position Position {  get; set; }
    public TravelCode? TravelCode { get; set; }
    public Importance? Importance { get; set; }
    public Culture? Culture { get; set; }
    public Economic? Economic { get; set; }

    public int? GasGiants { get; set; }

    public World(Position position)
    {
        Position = position;
    }

    public virtual bool Equals(World world) =>
        Name == world.Name &&
        Uwp.Equals(world.Uwp) &&
        Position == world.Position &&
        TravelCode == world.TravelCode &&
        GasGiants == world.GasGiants;
}
