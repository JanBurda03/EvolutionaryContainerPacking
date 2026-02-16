
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

        IPackingVectorPartDecoder<Rotation> rotationDecoder;
        Rotation expectedRotation;

        IPackingVectorPartDecoder<PlacementHeuristic> placementDecoder;
        PlacementHeuristic expectedHeuristics;

        IBoxToBePackedSorter boxToBePackedSorter;

        IReadOnlyList<BoxToBePacked> decoded;

        var packingVector = PackingRules.CreateRandom(3);

        for (int i = 0; i < 10000; i++)
        {
            for (int a = 0; a < 2; a++)
            {
                if (a == 0)
                {
                    boxToBePackedSorter = new HeuristicalBoxSorter(new OrderHeuristics.HighVolumeFirstHeuristic());
                }
                else
                {
                    boxToBePackedSorter = new PackingVectorUsingBoxSorter();

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
                            placementDecoder = new OnePlacementDecoder(PlacementHeuristics.FirstFit);
                            expectedHeuristics = PlacementHeuristics.FirstFit;

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

                        decoder = new PackingRulesDecoder(placementDecoder, rotationDecoder, boxToBePackedSorter, input);

                        Assert.Equal(a + b + c, decoder.PackingVectorMinimalLength);

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