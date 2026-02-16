
public class EmptyMaximalRegionsTests
{

    [Fact]
    public void Test1()
    {
        var initial = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 5));
        var ems = new EmptyMaximalRegions(initial);

        var box1 = new Region(new Coordinates(2, 2, 0), new Coordinates(5, 5, 4));
        var box2 = new Region(new Coordinates(0, 0, 0), new Coordinates(2, 2, 4));
        ems.Update(box1);
        ems.Update(box2);

        var updatedSpaces = ems.EmptyMaximalRegionsList;



        Assert.Contains(new Region(new Coordinates(0, 2, 0), new Coordinates(2, 10, 5)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(2, 0, 0), new Coordinates(10, 2, 5)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(5, 0, 0), new Coordinates(10, 10, 5)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(0, 5, 0), new Coordinates(10, 10, 5)), updatedSpaces);

        Assert.Contains(new Region(new Coordinates(2, 2, 4), new Coordinates(5, 5, 5)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(0, 0, 4), new Coordinates(2, 2, 5)), updatedSpaces);

        Assert.Equal(updatedSpaces.Count, 6);



    }

    [Fact]
    public void Test2()
    {
        var initial = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 5));
        var ems = new EmptyMaximalRegions(initial);

        var box1 = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 4));
        var box2 = new Region(new Coordinates(0, 0, 4), new Coordinates(2, 2, 5));
        ems.Update(box1);
        ems.Update(box2);

        var updatedSpaces = ems.EmptyMaximalRegionsList;



        Assert.Contains(new Region(new Coordinates(0, 2, 4), new Coordinates(10, 10, 5)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(2, 0, 4), new Coordinates(10, 10, 5)), updatedSpaces);


        Assert.Equal(updatedSpaces.Count, 2);



    }

    [Fact]
    public void Test3()
    {
        var initial = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 5));
        var ems = new EmptyMaximalRegions(initial);

        var box1 = new Region(new Coordinates(4, 4, 0), new Coordinates(8, 8, 4));    
        ems.Update(box1);

        var updatedSpaces = ems.EmptyMaximalRegionsList;



        Assert.Contains(new Region(new Coordinates(0, 0, 0), new Coordinates(4, 10, 5)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(0, 0, 0), new Coordinates(10, 4, 5)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(8, 0, 0), new Coordinates(10, 10, 5)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(0, 8, 0), new Coordinates(10, 10, 5)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(4, 4, 4), new Coordinates(8, 8, 5)), updatedSpaces);


        Assert.Equal(updatedSpaces.Count, 5);



    }

    [Fact]
    public void LevitatingBox()
    {
        var initial = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 5));
        var ems = new EmptyMaximalRegions(initial);

        var box1 = new Region(new Coordinates(4, 4, 2), new Coordinates(8, 8, 4));

        try
        {
            ems.Update(box1);
            Assert.True(false);
        }
        catch 
        {
            Assert.True(true);
        }

    }

    [Fact]
    public void IntersectingBox()
    {
        var initial = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 5));
        var ems = new EmptyMaximalRegions(initial);

        var box1 = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 4));
        var box2 = new Region(new Coordinates(0, 0, 3), new Coordinates(2, 2, 5));
        ems.Update(box1);
        

        try
        {
            ems.Update(box2);
            Assert.True(false);
        }
        catch
        {
            Assert.True(true);
        }

    }





}


