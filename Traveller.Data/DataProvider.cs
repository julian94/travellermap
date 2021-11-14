using Traveller.Core.Features;
using Traveller.Parser;

namespace Traveller.Resources;

public class DataProvider
{
    public DataProvider()
    {
        /* Todo:
         * 1: Read in and parse all Sectors and milieus.
         *      a: Enumerate folders and files into a tree structure.
         *      b: Fetch the Main xml metadata file, names match folders.
         *      c: Parse sectors, one by one.
         * 2: Present the data in an easy to use data structure.
         * 3: Add test mileu and tests to verify proper functioning.
         */

        Galaxies = new();
    }

    public List<Galaxy> Galaxies { get; init; }
}