namespace EvolutionaryContainerPacking.Tests.DecoderTests;

using EvolutionaryContainerPacking.Packing;
using EvolutionaryContainerPacking.Packing.Architecture.Geometry;
using EvolutionaryContainerPacking.Packing.Architecture.Containers;
using EvolutionaryContainerPacking.Packing.Architecture.Boxes;
using EvolutionaryContainerPacking.Packing.Architecture.Placements;
using EvolutionaryContainerPacking.Packing.Rules;
using EvolutionaryContainerPacking.Packing.Rules.Decoding.Sorting;

public class SorterTests
{
    [Fact]

    public void SorterTest1()
    {
        PackingRules vector = new PackingRules([0.8, 0.75, 0.7]);

        PendingBox[] boxes = new PendingBox[vector.Count];
        for (int i = 0; i < boxes.Length; i++)
        {

            boxes[i] = new PendingBox(
                new BoxProperties(i, new Sizes(1, 1, 1), 1),
                Rotation.XYZ,
                PlacementHeuristics.BestVolumeFit
                );
        }

        var sorter = new PackingRulesUsingBoxSorter();

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

        PendingBox[] boxes = new PendingBox[vector.Count];
        for (int i = 0; i < boxes.Length; i++)
        {

            boxes[i] = new PendingBox(
                new BoxProperties(i, new Sizes(1, 1, 1), 1),
                Rotation.XYZ,
                PlacementHeuristics.BestVolumeFit
                );
        }

        var sorter = new PackingRulesUsingBoxSorter();

        var sorted = sorter.Sort(boxes, vector);

        for (int i = 0; i < boxes.Length; i++)
        {
            Assert.Equal(boxes[boxes.Length - 1 - i], sorted[i]);
        }
    }
}