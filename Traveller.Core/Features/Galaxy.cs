﻿namespace Traveller.Core.Features;
public class Galaxy : IWorldHolder
{
    public Metadata Metadata { get; init; }

    [JsonIgnore]
    public List<StarSystem> StarSystems { get; init; }

    public Position Position { get; init; }

    public List<Sector> Sectors { get; init; }
}
