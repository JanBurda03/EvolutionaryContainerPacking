using Xunit;


public class IntersectionTests
{
    [Fact]
    public void IntersectionTest1()
    {
        var space1 = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 10));
        var space2 = new Region(new Coordinates(5, 5, 5), new Coordinates(15, 15, 15));

        Assert.True(space1.IntersectsWith(space2));
        Assert.True(space2.IntersectsWith(space1));
    }


    [Fact]
    public void IntersectionTest2()
    {
        var space1 = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 10));
        var space2 = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 10));

        Assert.True(space1.IntersectsWith(space2));
        Assert.True(space2.IntersectsWith(space1));
    }


    [Fact]
    public void IntersectionTest3()
    {
        var space1 = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 10));
        var space2 = new Region(new Coordinates(2, 2, 2), new Coordinates(8, 8, 8));

        Assert.True(space1.IntersectsWith(space2));
        Assert.True(space2.IntersectsWith(space1));
    }



    [Fact]
    public void IntersectionTest4()
    {
        var space1 = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 10));
        var space2 = new Region(new Coordinates(0, 0, 0), new Coordinates(1, 1, 1));

        Assert.True(space1.IntersectsWith(space2));
        Assert.True(space2.IntersectsWith(space1));
    }



    [Fact]
    public void IntersectionTest5()
    {
        var space1 = new Region(new Coordinates(5, 5, 5), new Coordinates(10, 10, 10));
        var space2 = new Region(new Coordinates(0, 0, 0), new Coordinates(5, 5, 5));

        Assert.False(space1.IntersectsWith(space2));
        Assert.False(space2.IntersectsWith(space1));
    }


    [Fact]
    public void IntersectionTest6()
    {
        var space1 = new Region(new Coordinates(0, 0, 1), new Coordinates(10, 10, 10));
        var space2 = new Region(new Coordinates(5, 5, 0), new Coordinates(15, 15, 1));

        Assert.False(space1.IntersectsWith(space2));
        Assert.False(space2.IntersectsWith(space1));
    }



    [Fact]
    public void IntersectionTest7()
    {
        var space1 = new Region(new Coordinates(2, 2, 2), new Coordinates(10, 10, 10));
        var space2 = new Region(new Coordinates(1, 2, 3), new Coordinates(2, 2, 4));

        Assert.False(space1.IntersectsWith(space2));
        Assert.False(space2.IntersectsWith(space1));
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

public class TripletTest
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
