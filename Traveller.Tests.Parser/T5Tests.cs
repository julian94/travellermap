using System.IO;
using NUnit.Framework;
using Traveller.Core;
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
        [TestCase("Sector	SS	Hex	Name	UWP	Bases	Remarks	Zone	PBG	Allegiance	Stars	{Ix}	(Ex)	[Cx]	Nobility	W	RU", true)]
        [TestCase("Hex  Name                 UWP       Remarks                   {Ix}   (Ex)    [Cx]   N    B  Z PBG W  A    Stellar       ", false)]
        public void TestTabColumnDetection(string input, bool isTab)
        {
            Assert.AreEqual(isTab, T5Parser.IsTabDelimited(input));
        }
    }
}