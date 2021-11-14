namespace Traveller.Core.Features;
public class SubSector : IWorldHolder
{
    public Metadata Metadata { get; init; }

    [JsonIgnore]
    public List<StarSystem> StarSystems { get; init; }

    public Position Position { get; init; }

    public SubSector(Position position, Metadata? metadata = null)
    {
        Position = position;
        Metadata = metadata ?? new Metadata();
        StarSystems = new List<StarSystem>();
    }
}
