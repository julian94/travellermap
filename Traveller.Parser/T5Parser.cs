namespace Traveller.Parser;
public class T5Parser : IParser
{
    public const string SupportedExtension = "tab";
    public static bool CanParse(string extension) => SupportedExtension.Equals(extension.Trim().TrimStart('.'));

    public static bool TryParseSector(string inputSector, string? inputMetadata, out Sector result)
    {
        /* Example header and first line, NOT AUTHORATIVE!
         * Fields may appear in ANY ORDER, though consistent on a file to file basis.
         * Sector	SS	Hex     Name	UWP	        Bases	Remarks	    Zone	PBG	    Allegiance	Stars	    {Ix}	(Ex)	[Cx]	Nobility	W	RU
         * Troj	    A	0103	Taltern	E530240-6	N	    De Lo Po	A	    202	    NaHu	    M2 V M2 V	{ -3 }	(410-5)	[1111]	            7   0
        */
        var parts = new List<string>(inputSector.Split('\n')); // Note both LF and CR+LF are valid line endings. This might not catch both types.
        var isTabDelimited = IsTabDelimited(parts[0]);

        result = new Sector();
        List<Dictionary<Field, string>> worldPartList;

        if (isTabDelimited)
        {
            worldPartList = ParseTabWorlds(parts);
        }
        else
        {
            worldPartList = ParseColumnWorlds(parts);
        }

        foreach (var world in worldPartList)
        {
            // Add worlds to sector.
            result.AddWorld(ParseLine(world));
        }

        // Todo: Parse Metadata.

        return true; // Return false at an earlier point if an error occurs.
    }

    public static List<Dictionary<Field, string>> ParseTabWorlds(List<string> parts)
    {
        var worldPartList = new List<Dictionary<Field, string>>();
        var headers = ParseTabHeader(parts[0]);

        for (var i = 1; i < parts.Count && !parts[i].Equals(string.Empty); i++)
        {
            var lineParts = parts[i].Split('\t');
            var worldParts = new Dictionary<Field, string>();
            for (var j = 0; j < headers.Count; j++)
            {
                worldParts[headers[j]] = lineParts[j];
            }
            worldPartList.Add(worldParts);
        }

        return worldPartList;
    }

    public static Dictionary<Field, string> ParseTabWorld(List<Field> headers, string line)
{
        var lineParts = line.Split('\t');
        var worldParts = new Dictionary<Field, string>();
        for (var j = 0; j < headers.Count; j++)
        {
            worldParts[headers[j]] = lineParts[j];
        }
        return worldParts;
    }

    public static List<Dictionary<Field, string>> ParseColumnWorlds(List<string> parts)
    {
        var worldPartList = new List<Dictionary<Field, string>>();
        var headers = ParseColumnHeader(parts[0], parts[1]);

        for (var i = 1; i < parts.Count && !string.IsNullOrWhiteSpace(parts[i]); i++)
        {
            var line = parts[i];
            var worldParts = ParseColumnWorld(headers, line);
            worldPartList.Add(worldParts);
        }

        return worldPartList;
    }

    public static Dictionary<Field, string> ParseColumnWorld(List<(Field, int)> headers, string line)
    {
        var worldParts = new Dictionary<Field, string>();
        var startIndex = 0;
        for (var j = 0; j < headers.Count; j++)
        {
            (var header, var length) = headers[j];
            worldParts[header] = line.Substring(startIndex, length);

            /* Using column separation of 1
                * People might be using a bigger or varying separation,
                * so we should probably be retrieving the startIndex instead of calculating it.
                * 
                * For now 1 is good enough.
                */
            startIndex += length + 1;
        }
        return worldParts;
    }

    public static bool IsTabDelimited(string header)
    {
        if ((header.Contains(' ') && header.Contains('\t')) ||
            (!header.Contains(' ') && !header.Contains('\t'))) throw new ArgumentException("A T5 format must be either tab or column delimited.");
        return header.Contains('\t');
    }

    public static List<Field> ParseTabHeader(string header)
    {
        var parts = header.Split('\t');
        var fields = new List<Field>();
        foreach (var part in parts)
        {
            if (Enum.TryParse<Field>(StripExtensionClosures(part), out var field)) fields.Add(field);
            else throw new ArgumentException($"Unrecognized Field: {part}");
        }

        return fields;
    }
    public static List<(Field, int)> ParseColumnHeader(string header, string headerDashes)
    {
        /* Example
            Hex  Name                 UWP       Remarks                   {Ix}   (Ex)    [Cx]   N    B  Z PBG W  A    Stellar       
            ---- -------------------- --------- ------------------------- ------ ------- ------ ---- -- - --- -- ---- --------------
            0101 Tikal                E767213-A Ga                        { 0 }  (000-0) [0000]         - 000 1                     
         */

        var nameParts = header.Split();
        var sizeParts = headerDashes.Split();
        var fields = new List<(Field, int)>();
        
        for (int i = 0; i < nameParts.Length; i++)
        {
            if (Enum.TryParse<Field>(StripExtensionClosures(nameParts[i]), out var field)) fields.Add((field, sizeParts[i].Length));
            else throw new ArgumentException($"Unrecognized Field: {nameParts[i]}");
        }

        return fields;
    }

    public static World ParseLine(Dictionary<Field, string> parts)
    {
        var world = new World(new Position(parts[Field.Hex]))
        {
            Name = parts[Field.Name],
            Uwp = new UWP(parts[Field.UWP]),
            GasGiants = 0,
            TravelCode = parts[Field.Zone] == " " ? TravelCode.G : Enum.Parse<TravelCode>(parts[Field.Zone]),
        };

        return world;
    }

    public static string StripExtensionClosures(string field) =>
        field
        .TrimStart('{')
        .TrimStart('[')
        .TrimStart('(')
        .TrimEnd(')')
        .TrimEnd(']')
        .TrimEnd('}');

}
public enum Field
{
    Sector,
    SS,
    Hex,
    Name,
    UWP,
    Bases,
    Remarks,
    Zone,
    PBG,
    Allegiance,
    Stars,
    Ix,
    Ex,
    Cx,
    Nobility,
    W,
    RU,
}
