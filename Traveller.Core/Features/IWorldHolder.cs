namespace Traveller.Core.Features;
public interface IWorldHolder
{
    public Metadata Metadata { get; init; }
    public List<StarSystem> StarSystems { get; init; }
    public Position Position { get; init; }
}
