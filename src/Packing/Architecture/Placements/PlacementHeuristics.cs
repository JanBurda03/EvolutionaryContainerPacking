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
            {"BestFit", BestFit},
            {"MaxDistance", MaxDistance},
            {"MinDistance", MinDistance}
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
    public static Placement? BestFit(PendingBox box, IEnumerable<ContainerData> containers)
    {
        Placement? placement = null;

        foreach (ContainerData c in containers)
        {
            if (ValidWeight(box, c))
            {
                foreach (Region region in c.EmptyMaximalRegions)
                {
                    if (ValidSides(box, region))
                    {
                        if (placement == null || ((Placement)placement).OccupiedRegion.GetVolume() > region.GetVolume())
                        {
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
        Placement? placement = null;
        int maxD = int.MaxValue;
        

        foreach (ContainerData c in containers)
        {
            Coordinates cEnd = c.ContainerProperties.Sizes.ToRegion(new Coordinates(0, 0, 0)).End;
            if (ValidWeight(box, c))
            {
                foreach (Region region in c.EmptyMaximalRegions)
                {
                    if (ValidSides(box, region))
                    {
                        if (placement == null || maxD < region.Start.GetEuclidanDistanceTo(cEnd))
                        {
                            placement = new Placement(c.ID, box.GetRotatedSizes().ToRegion(region.Start));
                        }
                    }
                }
            }
        }
        return placement;
    }

    /// <summary>
    /// Chooses placement closest to container origin.
    /// </summary>
    public static Placement? MinDistance(PendingBox box, IEnumerable<ContainerData> containers)
    {
        Placement? placement = null;
        int minD = int.MaxValue;
        Coordinates cStart = new Coordinates(0, 0, 0);


        foreach (ContainerData c in containers)
        {
            
            if (ValidWeight(box, c))
            {
                foreach (Region region in c.EmptyMaximalRegions)
                {
                    if (ValidSides(box, region))
                    {
                        if (placement == null || minD > region.Start.GetEuclidanDistanceTo(cStart))
                        {
                            placement = new Placement(c.ID, box.GetRotatedSizes().ToRegion(region.Start));
                        }
                    }
                }
            }
        }
        return placement;
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

