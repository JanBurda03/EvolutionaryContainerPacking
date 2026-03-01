namespace EvolutionaryContainerPacking.Tests.GeometryTests;

using EvolutionaryContainerPacking.Packing.Architecture.Geometry;
using Xunit;

public class SubspaceTests
{
    [Fact]
    public void SubspaceTest1()
    {
        var y = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 10));
        var x = new Region(new Coordinates(1, 1, 1), new Coordinates(9, 9, 9));

        Assert.True(x.IsSubregionOf(y));
        Assert.True(y.IsOverregionOf(x));
    }

    [Fact]
    public void SubspaceTest2()
    {
        var y = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 10));
        var x = new Region(new Coordinates(0, 0, 0), new Coordinates(9, 9, 9));

        Assert.True(x.IsSubregionOf(y));
        Assert.True(y.IsOverregionOf(x));
    }


    [Fact]
    public void SubspaceTest3()
    {
        var y = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 10));
        var x = new Region(new Coordinates(1, 1, 1), new Coordinates(10, 10, 10));

        Assert.True(x.IsSubregionOf(y));
        Assert.True(y.IsOverregionOf(x));
    }

    [Fact]
    public void SubspaceTest4()
    {
        var y = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 10));
        var x = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 10));

        Assert.True(x.IsSubregionOf(y));
        Assert.True(y.IsOverregionOf(x));
    }

    [Fact]
    public void SubspaceTest5()
    {
        var y = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 10));
        var x = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 11));

        Assert.False(x.IsSubregionOf(y));
        Assert.False(y.IsOverregionOf(x));
    }

    [Fact]
    public void SubspaceTest6()
    {
        var y = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 5));
        var x = new Region(new Coordinates(2, 2, 0), new Coordinates(5, 5, 4));

        Assert.True(x.IsSubregionOf(y));
        Assert.True(y.IsOverregionOf(x));
    }


}