using NUnit.Framework;
using Traveller.Core.Features.Structures;

namespace Traveller.Core.Tests;
public class EHexTests
{
    [Test]
    [TestCase("0")]
    [TestCase("1")]
    [TestCase("2")]
    [TestCase("3")]
    [TestCase("4")]
    [TestCase("5")]
    [TestCase("6")]
    [TestCase("7")]
    [TestCase("8")]
    [TestCase("9")]
    [TestCase("A")]
    [TestCase("B")]
    [TestCase("C")]
    [TestCase("D")]
    [TestCase("E")]
    [TestCase("F")]
    [TestCase("G")]
    [TestCase("H")]
    [TestCase("J")]
    [TestCase("K")]
    [TestCase("L")]
    [TestCase("M")]
    [TestCase("N")]
    [TestCase("P")]
    [TestCase("Q")]
    [TestCase("R")]
    [TestCase("S")]
    [TestCase("T")]
    [TestCase("U")]
    [TestCase("V")]
    [TestCase("W")]
    [TestCase("X")]
    [TestCase("Y")]
    [TestCase("Z")]

    public void TestEHexCreation(string input)
    {
        var result = new EHex(input);
        Assert.AreEqual(input, result.ToString());
    }

}