namespace EvolutionaryContainerPacking.Tests.DecoderTests;

using EvolutionaryContainerPacking.Packing;
using EvolutionaryContainerPacking.Packing.Architecture.Geometry;
using EvolutionaryContainerPacking.Packing.Architecture.Containers;
using EvolutionaryContainerPacking.Packing.Architecture.Boxes;
using EvolutionaryContainerPacking.Packing.Architecture.Placements;
using EvolutionaryContainerPacking.Packing.Rules;
using EvolutionaryContainerPacking.Packing.Rules.Decoding;
using EvolutionaryContainerPacking.Packing.Rules.Decoding.PartDecoders;
using EvolutionaryContainerPacking.Packing.Rules.Decoding.Sorting;

public class DecoderTests
{
    [Fact]
    public void DecoderTest()
    {
        PackingInput input = new PackingInput(
            new ContainerProperties(new Sizes(1, 1, 1), 42),
            [new BoxProperties(0, new Sizes(1, 1, 1), 1)]
            );

        PackingRulesDecoder decoder;

        IPackingRulesPartDecoder<Rotation> rotationDecoder;
        Rotation expectedRotation;

        IPackingRulesPartDecoder<PlacementHeuristic> placementDecoder;
        PlacementHeuristic expectedHeuristics;

        IPendingBoxSorter pendingBoxSorter;

        IReadOnlyList<PendingBox> decoded;

        var packingVector = PackingRules.CreateRandom(3);

        for (int i = 0; i < 10000; i++)
        {
            for (int a = 0; a < 2; a++)
            {
                if (a == 0)
                {
                    pendingBoxSorter = new HeuristicalBoxSorter(OrderHeuristics.GetOrderHeuristic("High Volume First Heuristic"));
                }
                else
                {
                    pendingBoxSorter = new PackingRulesUsingBoxSorter();

                }

                for (int b = 0; b < 2; b++)
                {
                    if (b == 0)
                    {
                        rotationDecoder = new OneRotationDecoder(Rotation.XYZ);
                        expectedRotation = Rotation.XYZ;


                    }
                    else
                    {
                        rotationDecoder = new MultipleRotationsDecoder([Rotation.ZYX, Rotation.ZXY]);

                        if (packingVector[a] < 0.5)
                        {
                            expectedRotation = Rotation.ZYX;
                        }
                        else
                        {
                            expectedRotation = Rotation.ZXY;
                        }
                    }


                    for (int c = 0; c < 2; c++)
                    {
                        if (c == 0)
                        {
                            placementDecoder = new OnePlacementDecoder(PlacementHeuristics.BestVolumeFit);
                            expectedHeuristics = PlacementHeuristics.BestVolumeFit;

                        }
                        else
                        {
                            placementDecoder = new MultiplePlacementsDecoder([PlacementHeuristics.MinDistance, PlacementHeuristics.MaxDistance]);
                            if (packingVector[a + b] < 0.5)
                            {
                                expectedHeuristics = PlacementHeuristics.MinDistance;
                            }
                            else
                            {
                                expectedHeuristics = PlacementHeuristics.MaxDistance;
                            }
                        }

                        decoder = new PackingRulesDecoder(placementDecoder, rotationDecoder, pendingBoxSorter, input);

                        Assert.Equal(a + b + c, decoder.PackingRulesMinimalLength);

                        decoded = decoder.Decode(packingVector);

                        Assert.Single(decoded);

                        Assert.Equal(expectedRotation, decoded[0].Rotation);
                        Assert.Equal(expectedHeuristics, decoded[0].PlacementHeuristic);



                    }
                }
            }
        }
    }
}