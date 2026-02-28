namespace EvolutionaryContainerPacking.PackingTests.GeometryTests;
using Xunit;
using EvolutionaryContainerPacking.Packing.Architecture.Geometry;

public class CoordinateTest
{
    [Fact]
    public void TripletTest1()
    {
        Coordinates x = new Coordinates(0, 0, 0);
        Coordinates y = new Coordinates(2, 2, 0);

        Assert.True(x.AllLessOrEqualThan(y));
        Assert.False(y.AllGreaterThan(x));

        Assert.True(x.AllGreaterOrEqualThan(x));
        Assert.True(x.AllLessOrEqualThan(x));
        Assert.False(x.AllGreaterThan(x));
        Assert.False(x.AllLessThan(x));
    }

    [Fact]
    public void TripletTest2()
    {
        Coordinates x = new Coordinates(10, 10, 5);
        Coordinates y = new Coordinates(5, 5, 4);

        Assert.True(x.AllGreaterOrEqualThan(y));
        Assert.True(x.AllGreaterThan(y));
        Assert.False(y.AllGreaterOrEqualThan(x));
        Assert.False(y.AllGreaterThan(x));
    }


}
