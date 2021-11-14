using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Traveller.Core.Features;
using Traveller.Core.Features.Structures;
using Traveller.Parser;

namespace Traveller.Tests.Parser;

public class T5Tests
{
    public string Milieu;
    public string Data;
    public string Metadata;

    [SetUp]
    public void Setup()
    {
        Milieu = File.ReadAllText("./TestFiles/M1105.xml");
        Data = File.ReadAllText("./TestFiles/Trojan Reach.tab");
        Metadata = File.ReadAllText("./TestFiles/Trojan Reach.xml");
    }

    [Test]
    public void VerifySetup()
    {
        Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\" ?>", Milieu.Split('\n')[0].Trim('\r'));
        Assert.AreEqual("Sector	SS	Hex	Name	UWP	Bases	Remarks	Zone	PBG	Allegiance	Stars	{Ix}	(Ex)	[Cx]	Nobility	W	RU", Data.Split('\n')[0].Trim('\r'));
        Assert.AreEqual("<?xml version=\"1.0\"?>", Metadata.Split('\n')[0].Trim('\r'));
    }

    [Test]
    public void ProducesSector()
    {
        Assert.IsTrue(T5Parser.TryParseSector(Data, Metadata, out var sector));
        Assert.IsNotNull(sector);
    }

    [Test]
    public void TestSectorIsCorrect()
    {
        Assert.IsTrue(T5Parser.TryParseSector(Data, Metadata, out var sector));
        Assert.IsNotNull(sector);

        Assert.AreEqual(4, sector.Quadrants.Count);
        Assert.AreEqual(16, sector.SubSectors.Count);

        Assert.AreEqual(327, sector.StarSystems.Count);

        var picard = (from system in sector.StarSystems where system.MainWorld.Name == "Picard" select system).First();

        Assert.IsNotNull(picard);
        Assert.AreEqual("D679646-7", picard.MainWorld.Uwp.ToString());
        Assert.AreEqual("0417", picard.Position.ToString());
    }

    [Test]
    [TestCase("Sector	SS	Hex	Name	UWP	Bases	Remarks	Zone	PBG	Allegiance	Stars	{Ix}	(Ex)	[Cx]	Nobility	W	RU", true)]
    [TestCase("Hex  Name                 UWP       Remarks                   {Ix}   (Ex)    [Cx]   N    B  Z PBG W  A    Stellar       ", false)]
    public void TestTabColumnDetection(string input, bool isTab)
    {
        Assert.AreEqual(isTab, T5Parser.IsTabDelimited(input));
    }


    [Test]
    [TestCase("{Ix}","Ix")]
    [TestCase("(Ex)", "Ex")]
    [TestCase("[Cx]", "Cx")]
    [TestCase("{ Ix }", "Ix")]
    [TestCase("Sector", "Sector")]
    [TestCase("SS", "SS")]
    [TestCase("Name", "Name")]
    [TestCase("", "")]
    public void TestClosureStripper(string input, string expected)
    {
        Assert.AreEqual(expected, T5Parser.StripExtensionClosures(input));
    }

    private static (string, string, Dictionary<Field, string>, StarSystem) GetTabWorld() =>
        (
            "Sector	SS	Hex	Name	UWP	Bases	Remarks	Zone	PBG	Allegiance	Stars	{Ix}	(Ex)	[Cx]	Nobility	W	RU",
            "Troj	K	2223	Drinax	A43645A-E		Ni		714	NaHu	M1 V	{ 1 }	(B34+3)	[657G]		9	396",
            new Dictionary<Field, string>
            {
                { Field.Sector, "Troj" },
                { Field.SS, "K" },
                { Field.Hex, "2223" },
                { Field.Name, "Drinax" },
                { Field.UWP, "A43645A-E" },
                { Field.Bases, string.Empty },
                { Field.Remarks, "Ni" },
                { Field.Zone, string.Empty },
                { Field.PBG, "714" },
                { Field.Allegiance, "NaHu" },
                { Field.Stars, "M1 V" },
                { Field.Ix, "{ 1 }" },
                { Field.Ex, "(B34+3)" },
                { Field.Cx, "[657G]" },
                { Field.Nobility, string.Empty },
                { Field.W, "9" },
                { Field.RU, "396"},
            },
            new StarSystem(
                new Position("2223"),
                new World()
                {
                    Name = "Drinax",
                    Uwp = new UWP("A43645A-E"),
                    TravelCode = TravelCode.G,
                    Importance = new Importance("{ 1 }"),
                    Economic = new Economic("(B34+3)"),
                    Culture = new Culture("[657G]"),
                    PopulationModifier = 7,
                }
            )
            {
                Stars = "M1 V",
                PlanetoidBelts = 1,
                Worlds = 9,
                GasGiants = 4,
            }
        );

    [Test]
    public void TestTokeniseTabWorld()
    {
        (var rawHeader, var drinaxRaw, var drinaxParts, _) = GetTabWorld();

        var header = T5Parser.ParseTabHeader(rawHeader);
        Assert.IsNotNull(header);
        var parsedParts = T5Parser.TokeniseTabWorld(header, drinaxRaw);
        Assert.IsNotNull(parsedParts);
            
        foreach (var part in drinaxParts)
        {
            Assert.AreEqual(part.Value, parsedParts[part.Key]);
        }
    }

    [Test]
    public void TestTokeniseTabWorlds()
    {
        (var rawHeader, var drinaxRaw, var drinaxParts, _) = GetTabWorld();

        var lines = new List<string>
        {
            rawHeader, drinaxRaw, string.Empty,
        };

        var parsedParts = T5Parser.TokeniseTabWorlds(lines);
        Assert.IsNotNull(parsedParts);
        Assert.IsNotEmpty(parsedParts);

        var world = parsedParts[0];
        Assert.IsNotNull(world);

        foreach (var part in drinaxParts)
        {
            Assert.AreEqual(part.Value, world[part.Key]);
        }
    }

    [Test]
    public void TestParseWorld()
    {
        (_, _, var worldParts, var system) = GetTabWorld();

        Assert.IsTrue(T5Parser.TryParseLine(worldParts, out var parsedSystem));

        Assert.IsNotNull(parsedSystem);

        Assert.IsTrue(system.Equals(parsedSystem));

        Assert.AreEqual(system.MainWorld.Uwp.ToString(), parsedSystem.MainWorld.Uwp.ToString());
        Assert.IsTrue(system.MainWorld.Uwp.Equals(parsedSystem.MainWorld.Uwp));

        Assert.IsTrue(system.MainWorld.Equals(parsedSystem.MainWorld));

        Assert.AreEqual(system.MainWorld.Importance, parsedSystem.MainWorld.Importance);
        Assert.AreEqual(system.MainWorld.Economic, parsedSystem.MainWorld.Economic);
        Assert.AreEqual(system.MainWorld.Culture, parsedSystem.MainWorld.Culture);
    }
}
