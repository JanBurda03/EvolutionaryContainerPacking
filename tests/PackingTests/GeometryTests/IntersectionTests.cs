namespace EvolutionaryContainerPacking.PackingTests.GeometryTests;

using EvolutionaryContainerPacking.Packing.Architecture.Geometry;
using Xunit;

public class IntersectionTests
{
    [Fact]
    public void IntersectionTest1()
    {
        var region1 = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 10));
        var region2 = new Region(new Coordinates(5, 5, 5), new Coordinates(15, 15, 15));

        Assert.True(region1.IntersectsWith(region2));
        Assert.True(region2.IntersectsWith(region1));
    }


    [Fact]
    public void IntersectionTest2()
    {
        var region1 = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 10));
        var region2 = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 10));

        Assert.True(region1.IntersectsWith(region2));
        Assert.True(region2.IntersectsWith(region1));
    }


    [Fact]
    public void IntersectionTest3()
    {
        var region1 = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 10));
        var region2 = new Region(new Coordinates(2, 2, 2), new Coordinates(8, 8, 8));

        Assert.True(region1.IntersectsWith(region2));
        Assert.True(region2.IntersectsWith(region1));
    }



    [Fact]
    public void IntersectionTest4()
    {
        var region1 = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 10));
        var region2 = new Region(new Coordinates(0, 0, 0), new Coordinates(1, 1, 1));

        Assert.True(region1.IntersectsWith(region2));
        Assert.True(region2.IntersectsWith(region1));
    }



    [Fact]
    public void IntersectionTest5()
    {
        var region1 = new Region(new Coordinates(5, 5, 5), new Coordinates(10, 10, 10));
        var region2 = new Region(new Coordinates(0, 0, 0), new Coordinates(5, 5, 5));

        Assert.False(region1.IntersectsWith(region2));
        Assert.False(region2.IntersectsWith(region1));
    }


    [Fact]
    public void IntersectionTest6()
    {
        var region1 = new Region(new Coordinates(0, 0, 1), new Coordinates(10, 10, 10));
        var region2 = new Region(new Coordinates(5, 5, 0), new Coordinates(15, 15, 1));

        Assert.False(region1.IntersectsWith(region2));
        Assert.False(region2.IntersectsWith(region1));
    }



    [Fact]
    public void IntersectionTest7()
    {
        var region1 = new Region(new Coordinates(2, 2, 2), new Coordinates(10, 10, 10));
        var region2 = new Region(new Coordinates(1, 2, 3), new Coordinates(2, 2, 4));

        Assert.False(region1.IntersectsWith(region2));
        Assert.False(region2.IntersectsWith(region1));
    }

    [Fact]
    public void IntersectionTest8()
    {
        var region1 = new Region(new Coordinates(30, 10, 0), new Coordinates(35, 30, 20));
        var region2 = new Region(new Coordinates(20, 20, 0), new Coordinates(40, 30, 10));


        Assert.True(region1.IntersectsWith(region2));
        Assert.True(region2.IntersectsWith(region1));
    }

}
