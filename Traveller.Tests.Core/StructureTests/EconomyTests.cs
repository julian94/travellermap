using NUnit.Framework;
using Traveller.Core.Features.Structures;

namespace Traveller.Core.Tests;
public class EconomyTests
{
    [Test]
    public void TestEconomy()
    {
        var raw = "(B34+3)";
        var economic = new Economic(raw);

        Assert.AreEqual(raw, economic.ToString());
        Assert.AreEqual(new EHex("B"), economic.Resources);
        Assert.AreEqual(new EHex("3"), economic.Labour);
        Assert.AreEqual(new EHex("4"), economic.Infrastructure);
        Assert.AreEqual(3, economic.Efficiency);
    }

    [Test]
    public void TestEquality()
    {
        var raw = "(B34+3)";
        var a = new Economic(raw);
        var b = new Economic(raw);

        Assert.IsTrue(a == b);
        Assert.AreEqual(a, b);
    }
}