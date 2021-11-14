namespace Traveller.Core.Features;
public class Sector : IWorldHolder
{
    public Metadata Metadata {  get; init; }
    public List<StarSystem> StarSystems {  get; init; }
    public Position Position {  get; init; }
    public List<Quadrant> Quadrants { get; init; }

    public Sector(Position position, Metadata? metadata = null)
    {
        Position = position;
        Metadata = metadata ?? new Metadata();
        StarSystems = new();
        Quadrants = new();
        InitialiseQuadrants();
        InitialiseSubSectors();
    }

    private void InitialiseQuadrants()
    {
        Quadrants.Add(new Quadrant(Position + Constants.Positions.QuadrantA));
        Quadrants.Add(new Quadrant(Position + Constants.Positions.QuadrantB));
        Quadrants.Add(new Quadrant(Position + Constants.Positions.QuadrantC));
        Quadrants.Add(new Quadrant(Position + Constants.Positions.QuadrantD));
    }

    private void InitialiseSubSectors()
    {
        Quadrants[0].SubSectors.Add(new SubSector(Position + Constants.Positions.SubSectorA));
        Quadrants[0].SubSectors.Add(new SubSector(Position + Constants.Positions.SubSectorB));
        Quadrants[0].SubSectors.Add(new SubSector(Position + Constants.Positions.SubSectorC));
        Quadrants[0].SubSectors.Add(new SubSector(Position + Constants.Positions.SubSectorD));

        Quadrants[1].SubSectors.Add(new SubSector(Position + Constants.Positions.SubSectorE));
        Quadrants[1].SubSectors.Add(new SubSector(Position + Constants.Positions.SubSectorF));
        Quadrants[1].SubSectors.Add(new SubSector(Position + Constants.Positions.SubSectorG));
        Quadrants[1].SubSectors.Add(new SubSector(Position + Constants.Positions.SubSectorH));

        Quadrants[2].SubSectors.Add(new SubSector(Position + Constants.Positions.SubSectorI));
        Quadrants[2].SubSectors.Add(new SubSector(Position + Constants.Positions.SubSectorJ));
        Quadrants[2].SubSectors.Add(new SubSector(Position + Constants.Positions.SubSectorK));
        Quadrants[2].SubSectors.Add(new SubSector(Position + Constants.Positions.SubSectorL));

        Quadrants[3].SubSectors.Add(new SubSector(Position + Constants.Positions.SubSectorM));
        Quadrants[3].SubSectors.Add(new SubSector(Position + Constants.Positions.SubSectorN));
        Quadrants[3].SubSectors.Add(new SubSector(Position + Constants.Positions.SubSectorO));
        Quadrants[3].SubSectors.Add(new SubSector(Position + Constants.Positions.SubSectorP));
    }

    public List<SubSector> SubSectors
    {
        get
        {
            var subSectors = new List<SubSector>();
            foreach (var quadrant in Quadrants) subSectors.AddRange(quadrant.SubSectors);
            return subSectors;
        }
    }

    public void AddStarSystem(StarSystem system)
    {
        if (!system.Position.IsInSector(Position)) return;
        
        StarSystems.Add(system);

        foreach (var quadrant in Quadrants)
            if (system.Position.IsInQuadrant(quadrant.Position)) 
                quadrant.StarSystems.Add(system);

        foreach (var subSector in SubSectors)
            if (system.Position.IsInSubSector(subSector.Position))
                subSector.StarSystems.Add(system);
    }
}
