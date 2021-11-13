using Traveller.Core.Features.Structures;

namespace Traveller.Core.Features;

public class System
{
    public Position Position { get; set; }
    public World MainWorld { get; set; }

    public List<World>? OtherWorlds { get; set; }

    public string? Stars { get; set; }
    public int? PlanetoidBelts { get; set; }
    public int? GasGiants { get; set; }
}
