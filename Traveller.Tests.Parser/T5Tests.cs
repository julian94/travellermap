using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Traveller.Core.Features;
using Traveller.Parser;

namespace Traveller.Tests.Parser
{
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

        private static (string, string, Dictionary<Field, string>, World) GetTabWorld() =>
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
                new World(new Position("2223"))
                {
                    Name = "Drinax",
                    GasGiants = 4,
                    TravelCode = TravelCode.G,
                    Uwp = new UWP("A43645A-E"),
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
            (_, _, var worldParts, var world) = GetTabWorld();
            var parsedWorld = T5Parser.ParseLine(worldParts);
            Assert.IsNotNull(parsedWorld);

            Assert.AreEqual(world.Uwp.ToString(), parsedWorld.Uwp.ToString());
            Assert.IsTrue(world.Uwp.Equals(parsedWorld.Uwp));
            //Assert.AreEqual(world.Uwp, parsedWorld.Uwp);

            Assert.IsTrue(world.Equals(parsedWorld));
        }
    }
}