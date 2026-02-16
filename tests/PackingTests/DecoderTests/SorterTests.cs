public class SorterTests
{
    [Fact]

    public void SorterTest1()
    {
        PackingRules vector = new PackingRules([0.8, 0.75, 0.7]);

        BoxToBePacked[] boxes = new BoxToBePacked[vector.Count];
        for (int i = 0; i < boxes.Length; i++)
        {

            boxes[i] = new BoxToBePacked(
                new BoxProperties(i, new Sizes(1, 1, 1), 1),
                Rotation.XYZ,
                PlacementHeuristics.BestFit
                );
        }

        var sorter = new PackingVectorUsingBoxSorter();

        var sorted = sorter.Sort(boxes, vector);

        for (int i = 0; i < boxes.Length; i++)
        {
            Assert.Equal(boxes[boxes.Length - 1 - i], sorted[i]);
        }
    }


    [Fact]

    public void SorterTest2()
    {
        PackingRules vector = new PackingRules([0.99, 0.8, 0.75, 0.7, 0.5, 0.3, 0.1, 0]);

        BoxToBePacked[] boxes = new BoxToBePacked[vector.Count];
        for (int i = 0; i < boxes.Length; i++)
        {

            boxes[i] = new BoxToBePacked(
                new BoxProperties(i, new Sizes(1, 1, 1), 1),
                Rotation.XYZ,
                PlacementHeuristics.BestFit
                );
        }

        var sorter = new PackingVectorUsingBoxSorter();

        var sorted = sorter.Sort(boxes, vector);

        for (int i = 0; i < boxes.Length; i++)
        {
            Assert.Equal(boxes[boxes.Length - 1 - i], sorted[i]);
        }
    }
}