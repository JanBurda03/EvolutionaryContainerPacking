namespace EvolutionaryContainerPacking.Tests.EMRTests;
using EvolutionaryContainerPacking.Packing.Architecture.EMR;
using EvolutionaryContainerPacking.Packing.Architecture.Geometry;
public class EmptyMaximalRegionsTests
{

    [Fact]
    public void TwoNeighboursTest()
    {
        var initial = new Region(new Coordinates(0, 0, 0), new Coordinates(10, 10, 5));
        var ems = new EmptyMaximalRegions(initial);

        
        var box1 = new Region(new Coordinates(0, 0, 0), new Coordinates(2, 2, 4));
        var box2 = new Region(new Coordinates(2, 0, 0), new Coordinates(5, 5, 4));

        ems.Update(box1);
        var updatedSpaces = ems.EmptyMaximalRegionsList;

        Assert.Contains(new Region(new Coordinates(0, 2, 0), new Coordinates(10, 10, 5)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(2, 0, 0), new Coordinates(10, 10, 5)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(0, 0, 4), new Coordinates(2, 2, 5)), updatedSpaces);
        Assert.Equal(3, updatedSpaces.Count);


        ems.Update(box2);
        updatedSpaces = ems.EmptyMaximalRegionsList;



        Assert.Contains(new Region(new Coordinates(0, 2, 0), new Coordinates(2, 10, 5)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(5, 0, 0), new Coordinates(10, 10, 5)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(0, 5, 0), new Coordinates(10, 10, 5)), updatedSpaces);

        Assert.Contains(new Region(new Coordinates(2, 0, 4), new Coordinates(5, 5, 5)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(0, 0, 4), new Coordinates(5, 2, 5)), updatedSpaces);

        Assert.Equal(5, updatedSpaces.Count);



    }

    [Fact]
    public void OneStandingOnAnotherTest()
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


        Assert.Equal(2, updatedSpaces.Count);



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


    [Fact]
    public void MergingTest()
    {
        var initial = new Region(new Coordinates(0, 0, 0), new Coordinates(3, 3, 2));
        var ems = new EmptyMaximalRegions(initial);

        var box1 = new Region(new Coordinates(0, 0, 0), new Coordinates(1, 2, 1));
        var box2 = new Region(new Coordinates(2, 0, 0), new Coordinates(3, 2, 1));
        var box3 = new Region(new Coordinates(1, 1, 0), new Coordinates(2, 2, 1));

        ems.Update(box1);

        var updatedSpaces = ems.EmptyMaximalRegionsList;

        Assert.Contains(new Region(new Coordinates(0, 2, 0), new Coordinates(3, 3, 2)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(1, 0, 0), new Coordinates(3, 3, 2)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(0, 0, 1), new Coordinates(1, 2, 2)), updatedSpaces);
        Assert.Equal(3, updatedSpaces.Count);

        ems.Update(box2);

        updatedSpaces = ems.EmptyMaximalRegionsList;

        Assert.Contains(new Region(new Coordinates(0, 2, 0), new Coordinates(3, 3, 2)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(1, 0, 0), new Coordinates(2, 3, 2)), updatedSpaces);

        Assert.Contains(new Region(new Coordinates(0, 0, 1), new Coordinates(1, 2, 2)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(2, 0, 1), new Coordinates(3, 2, 2)), updatedSpaces);
        Assert.Equal(4, updatedSpaces.Count);

        ems.Update(box3);

        updatedSpaces = ems.EmptyMaximalRegionsList;

        Assert.Contains(new Region(new Coordinates(1, 0, 0), new Coordinates(2, 1, 2)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(0, 2, 0), new Coordinates(3, 3, 2)), updatedSpaces);

        Assert.Contains(new Region(new Coordinates(0, 0, 1), new Coordinates(1, 2, 2)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(2, 0, 1), new Coordinates(3, 2, 2)), updatedSpaces);
        Assert.Contains(new Region(new Coordinates(0, 1, 1), new Coordinates(3, 2, 2)), updatedSpaces);
        Assert.Equal(5, updatedSpaces.Count);

    }





}


