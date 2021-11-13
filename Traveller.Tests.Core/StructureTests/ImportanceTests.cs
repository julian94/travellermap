using NUnit.Framework;
using Traveller.Core.Features.Structures;

namespace Traveller.Core.Tests;
public class ImportanceTests
{
    [Test]
    public void TestImportance()
    {
        var raw = "{ 1 }";
        var expectedString = "{1}";
        var expectedInt = 1;
        var importance = new Importance(raw);

        Assert.AreEqual(expectedString, importance.ToString());
        Assert.AreEqual(expectedInt, importance.RawImportance);
    }

    [Test]
    public void TestComparison()
    {
        var rawA = "{ 1 }";
        var rawB = "{-1 }";
        var importanceA = new Importance(rawA);
        var importanceB = new Importance(rawB);

        Assert.IsTrue(importanceA > importanceB);
        Assert.IsTrue(importanceB < importanceA);
    }

    [Test]
    public void TestEquality()
    {
        var raw = "{ 1 }";
        var importanceA = new Importance(raw);
        var importanceB = new Importance(raw);

        Assert.IsTrue(importanceA == importanceB);
        Assert.AreEqual(importanceA, importanceB);
    }
}