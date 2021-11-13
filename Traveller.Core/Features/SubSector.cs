namespace Traveller.Core.Features;
public class SubSector : IWorldHolder
{
    public Metadata Metadata { get; init; }

    [JsonIgnore]
    public List<StarSystem> StarSystems { get; init; }

    public Position Position { get; init; }
}
