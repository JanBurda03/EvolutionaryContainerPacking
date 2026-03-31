namespace EvolutionaryContainerPacking.Packing.Architecture.Placements;
using EvolutionaryContainerPacking.Packing.Architecture.Boxes;
using EvolutionaryContainerPacking.Packing.Architecture.Geometry;
using EvolutionaryContainerPacking.Packing.Architecture.Containers;


/// <summary>
/// Provides built-in placement heuristics.
/// </summary>
public static class PlacementHeuristics
{
    /// <summary>
    /// Available heuristics indexed by name.
    /// </summary>
    private static readonly IReadOnlyDictionary<string, PlacementHeuristic> PlacementHeuristicsDictionary = new Dictionary<string, PlacementHeuristic>
        {
            {"Best Volume Fit", BestVolumeFit},
            {"Max Distance", MaxDistance},
            {"Min X", MinX},
            {"Min Y", MinY},
            {"Gravity", MinZ},
            {"Anti-Gravity", MaxZ},
            {"Bottom Left Back", BottomLeftBack}
        };

    /// <summary>
    /// List of available heuristic names.
    /// </summary>
    public static IReadOnlyList<string> PlacementHeuristicsList => PlacementHeuristicsDictionary.Keys.ToArray();


    /// <summary>
    /// Returns a heuristic by name.
    /// </summary>
    public static PlacementHeuristic GetPlacementHeuristic(string placementHeuristicName)
    {
        if (PlacementHeuristicsDictionary.TryGetValue(placementHeuristicName, out var placementHeuristic))
            return placementHeuristic;

        throw new ArgumentException($"Unknown heuristic: {placementHeuristicName}");
    }

    /// <summary>
    /// Returns multiple heuristics by their names.
    /// </summary>
    public static IReadOnlyList<PlacementHeuristic> GetPlacementHeuristics(string[] placementHeuristicsNames)
    {
        PlacementHeuristic[] placementHeuristics = new PlacementHeuristic[placementHeuristicsNames.Length];
        for (int i = 0; i < placementHeuristicsNames.Length; i++) 
        {
            placementHeuristics[i] = GetPlacementHeuristic(placementHeuristicsNames[i]);
        }
        return placementHeuristics;
    }

    /// <summary>
    /// Chooses placement that minimizes leftover region volume.
    /// </summary>
    public static Placement? BestVolumeFit(PendingBox box, IEnumerable<ContainerData> containers)
    {
        Placement? placement = null;
        long bestVolume = long.MaxValue;
        long newVolume;

        foreach (ContainerData c in containers)
        {
            if (ValidWeight(box, c))
            {
                foreach (Region region in c.EmptyMaximalRegions)
                {
                    if (ValidSides(box, region))
                    {
                        newVolume = region.GetVolume();

                        if (placement == null || bestVolume > newVolume)
                        {
                            bestVolume = newVolume;
                            placement = new Placement(c.ID, box.GetRotatedSizes().ToRegion(region.Start));
                        }
                    }
                }
            }
        }
        return placement;
    }

    /// <summary>
    /// Chooses placement farthest from container end corner.
    /// </summary>
    public static Placement? MaxDistance(PendingBox box, IEnumerable<ContainerData> containers)
    {
        Placement? best = null;
        double maxDistance = double.MinValue;

        foreach (ContainerData c in containers)
        {
            Coordinates cEnd = c.ContainerProperties.Sizes.ToRegion(new Coordinates(0, 0, 0)).End;

            if (ValidWeight(box, c))
            {
                foreach (Region region in c.EmptyMaximalRegions)
                {
                    if (ValidSides(box, region))
                    {
                        double distance = region.Start.GetEuclidanDistanceTo(cEnd);

                        if (distance > maxDistance)
                        {
                            maxDistance = distance;
                            best = new Placement(c.ID, box.GetRotatedSizes().ToRegion(region.Start));
                        }
                    }
                }
            }
        }

        return best;
    }

    /// <summary>
    /// Chooses placement minimizing X coordinate.
    /// </summary>
    public static Placement? MinX(PendingBox box, IEnumerable<ContainerData> containers)
    {
        Placement? best = null;
        int minX = int.MaxValue;

        foreach (ContainerData c in containers)
        {
            if (ValidWeight(box, c))
            {
                foreach (Region region in c.EmptyMaximalRegions)
                {
                    if (ValidSides(box, region))
                    {
                        int x = region.Start.X;
                        if (best == null || x < minX)
                        {
                            minX = x;
                            best = new Placement(c.ID, box.GetRotatedSizes().ToRegion(region.Start));
                        }
                    }
                }
            }
        }

        return best;
    }

    /// <summary>
    /// Chooses placement minimizing Y coordinate.
    /// </summary>
    public static Placement? MinY(PendingBox box, IEnumerable<ContainerData> containers)
    {
        Placement? best = null;
        int minY = int.MaxValue;

        foreach (ContainerData c in containers)
        {
            if (ValidWeight(box, c))
            {
                foreach (Region region in c.EmptyMaximalRegions)
                {
                    if (ValidSides(box, region))
                    {
                        int y = region.Start.Y;
                        if (best == null || y < minY)
                        {
                            minY = y;
                            best = new Placement(c.ID, box.GetRotatedSizes().ToRegion(region.Start));
                        }
                    }
                }
            }
        }

        return best;
    }

    /// <summary>
    /// Chooses placement minimizing Z coordinate.
    /// </summary>
    public static Placement? MinZ(PendingBox box, IEnumerable<ContainerData> containers)
    {
        Placement? best = null;
        int minZ = int.MaxValue;

        foreach (ContainerData c in containers)
        {
            if (ValidWeight(box, c))
            {
                foreach (Region region in c.EmptyMaximalRegions)
                {
                    if (ValidSides(box, region))
                    {
                        int z = region.Start.Z;
                        if (best == null || z < minZ)
                        {
                            minZ = z;
                            best = new Placement(c.ID, box.GetRotatedSizes().ToRegion(region.Start));
                        }
                    }
                }
            }
        }

        return best;
    }

    /// <summary>
    /// Chooses placement maximizing Z coordinate.
    /// </summary>
    public static Placement? MaxZ(PendingBox box, IEnumerable<ContainerData> containers)
    {
        Placement? best = null;
        int maxZ = int.MinValue;

        foreach (ContainerData c in containers)
        {
            if (ValidWeight(box, c))
            {
                foreach (Region region in c.EmptyMaximalRegions)
                {
                    if (ValidSides(box, region))
                    {
                        int z = region.Start.Z;
                        if (best == null || z > maxZ)
                        {
                            maxZ = z;
                            best = new Placement(c.ID, box.GetRotatedSizes().ToRegion(region.Start));
                        }
                    }
                }
            }
        }

        return best;
    }

    /// <summary>
    /// Chooses placement using Bottom-Left-Back rule (minimal Z, then Y, then X).
    /// </summary>
    public static Placement? BottomLeftBack(PendingBox box, IEnumerable<ContainerData> containers)
    {
        Placement? best = null;
        Coordinates bestCoord = new Coordinates(int.MaxValue, int.MaxValue, int.MaxValue);

        foreach (ContainerData c in containers)
        {
            if (ValidWeight(box, c))
            {
                foreach (Region region in c.EmptyMaximalRegions)
                {
                    if (ValidSides(box, region))
                    {
                        var coord = region.Start;
                        if (best == null ||
                            coord.Z < bestCoord.Z ||
                            (coord.Z == bestCoord.Z && coord.Y < bestCoord.Y) ||
                            (coord.Z == bestCoord.Z && coord.Y == bestCoord.Y && coord.X < bestCoord.X))
                        {
                            bestCoord = coord;
                            best = new Placement(c.ID, box.GetRotatedSizes().ToRegion(coord));
                        }
                    }
                }
            }
        }

        return best;
    }

    /// <summary>
    /// Checks whether containers current weight + boxs weigh exceed the max weight of the container.
    /// </summary>
    private static bool ValidWeight(PendingBox box, ContainerData container)
    {
        return container.Weight + box.BoxProperties.Weight <= container.ContainerProperties.MaxWeight;

    }

    /// <summary>
    /// Checks whether box fits into the region.
    /// </summary>
    private static bool ValidSides(PendingBox box, Region region)
    {
        return box.GetRotatedSizes().AllLessOrEqualThan(region.GetSizes());
    }

}

