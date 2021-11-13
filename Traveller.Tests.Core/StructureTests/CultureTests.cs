using NUnit.Framework;
using Traveller.Core.Features.Structures;

namespace Traveller.Core.Tests;
public class CultureTests
{
    [Test]
    public void TestCulture()
    {
        var raw = "[657G]";
        var culture = new Culture(raw);

        Assert.AreEqual(raw, culture.ToString());
        Assert.AreEqual(new EHex("6"), culture.Homogeneity);
        Assert.AreEqual(new EHex("5"), culture.Accaptance);
        Assert.AreEqual(new EHex("7"), culture.Strangeness);
        Assert.AreEqual(new EHex("G"), culture.Symbols);
    }

}