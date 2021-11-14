using Traveller.Core.Features.Structures;

namespace Traveller.Core.Features;

public class StarSystem
{
    public Position Position { get; set; }
    public World MainWorld { get; set; }

    public StarSystem(Position position, World mainWorld)
    {
        Position = position;
        MainWorld = mainWorld;
    }

    public List<World>? OtherWorlds { get; set; }

    public string? Stars { get; set; }
    public int? PlanetoidBelts { get; set; }
    public int? Worlds { get; set; }
    public int? GasGiants { get; set; }

    public virtual bool Equals(StarSystem other) =>
        Position == other.Position &&
        MainWorld.Equals(other.MainWorld) &&
        OtherWorlds == other.OtherWorlds &&
        Stars == other.Stars &&
        PlanetoidBelts == other.PlanetoidBelts &&
        Worlds == other.Worlds &&
        GasGiants == other.GasGiants;
}
