namespace Traveller.Core.Features;
public class Quadrant : IWorldHolder
{
    public Metadata Metadata { get; init; }

    [JsonIgnore]
    public List<StarSystem> StarSystems { get; init; }

    public Position Position { get; init; }

    public List<SubSector> SubSectors {  get; init; }

    public Quadrant(Position position, Metadata? metadata = null)
    {
        Position = position;
        Metadata = metadata ?? new Metadata();
        SubSectors = new List<SubSector>();
        StarSystems = new List<StarSystem>();
    }
}
