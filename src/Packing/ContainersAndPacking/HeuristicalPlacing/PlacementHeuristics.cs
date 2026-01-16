public delegate PlacementInfo? PlacementHeuristic(BoxToBePacked boxToBePlaced, IEnumerable<ContainerData> containersData);


public static class PlacementHeuristics
{
    // Placement heuristic is a function that chooses in which container and in what region should the box be packed based on the data from each container


    private static readonly IReadOnlyDictionary<string, PlacementHeuristic> PlacementHeuristicsDictionary = new Dictionary<string, PlacementHeuristic>
        {
            //{"FirstFit", FirstFit},
            {"BestFit", BestFit},
            {"MaxDistance", MaxDistance},
            {"MinDistance", MinDistance}
        };
    public static IReadOnlyList<string> PlacementHeuristicsList => PlacementHeuristicsDictionary.Keys.ToArray();

    public static PlacementHeuristic GetPlacementHeuristic(string placementHeuristicName)
    {
        if (PlacementHeuristicsDictionary.TryGetValue(placementHeuristicName, out var placementHeuristic))
            return placementHeuristic;

        throw new ArgumentException($"Unknown heuristic: {placementHeuristicName}");
    }

    public static IReadOnlyList<PlacementHeuristic> GetMultiplePlacementHeuristics(string[] placementHeuristicsNames)
    {
        PlacementHeuristic[] placementHeuristics = new PlacementHeuristic[placementHeuristicsNames.Length];
        for (int i = 0; i < placementHeuristicsNames.Length; i++) 
        {
            placementHeuristics[i] = GetPlacementHeuristic(placementHeuristicsNames[i]);
        }
        return placementHeuristics;
    }


    public static PlacementInfo? FirstFit(BoxToBePacked boxToBePlaced, IEnumerable<ContainerData> containersData)
    {
        foreach (ContainerData containerData in containersData)
        {  
            if (ValidWeight(boxToBePlaced, containerData))
            {
                foreach(Region region in containerData.EMR)
                {
                    if (ValidSides(boxToBePlaced, region))
                    {
                        return new PlacementInfo(containerData.ID, boxToBePlaced.GetRotatedSizes().ToRegion(region.Start));
                    }
                }
            }     
        }
        return null;
    }

    public static PlacementInfo? BestFit(BoxToBePacked boxToBePlaced, IEnumerable<ContainerData> containersData)
    {
        PlacementInfo? info = null;

        foreach (ContainerData containerData in containersData)
        {
            if (ValidWeight(boxToBePlaced, containerData))
            {
                foreach (Region region in containerData.EMR)
                {
                    if (ValidSides(boxToBePlaced, region))
                    {
                        if (info == null || ((PlacementInfo)info).OccupiedRegion.GetVolume() > region.GetVolume())
                        {
                            info = new PlacementInfo(containerData.ID, boxToBePlaced.GetRotatedSizes().ToRegion(region.Start));
                        }
                    }
                }
            }
        }
        return info;
    }

    public static PlacementInfo? MaxDistance(BoxToBePacked boxToBePlaced, IEnumerable<ContainerData> containersData)
    {
        PlacementInfo? info = null;
        int maxDistance = int.MaxValue;
        

        foreach (ContainerData containerData in containersData)
        {
            Coordinates containerEnd = containerData.ContainerProperties.Sizes.ToRegion(new Coordinates(0, 0, 0)).End;
            if (ValidWeight(boxToBePlaced, containerData))
            {
                foreach (Region region in containerData.EMR)
                {
                    if (ValidSides(boxToBePlaced, region))
                    {
                        if (info == null || maxDistance < region.Start.GetEuclidanDistanceTo(containerEnd))
                        {
                            info = new PlacementInfo(containerData.ID, boxToBePlaced.GetRotatedSizes().ToRegion(region.Start));
                        }
                    }
                }
            }
        }
        return info;
    }

    public static PlacementInfo? MinDistance(BoxToBePacked boxToBePlaced, IEnumerable<ContainerData> containersData)
    {
        PlacementInfo? info = null;
        int minDistance = int.MaxValue;
        Coordinates containerStart = new Coordinates(0, 0, 0);


        foreach (ContainerData containerData in containersData)
        {
            
            if (ValidWeight(boxToBePlaced, containerData))
            {
                foreach (Region region in containerData.EMR)
                {
                    if (ValidSides(boxToBePlaced, region))
                    {
                        if (info == null || minDistance > region.Start.GetEuclidanDistanceTo(containerStart))
                        {
                            info = new PlacementInfo(containerData.ID, boxToBePlaced.GetRotatedSizes().ToRegion(region.Start));
                        }
                    }
                }
            }
        }
        return info;
    }

    private static bool ValidWeight(BoxToBePacked boxToBePlaced, ContainerData containerData)
    {
        return containerData.CurrentWeight + boxToBePlaced.BoxProperties.Weight <= containerData.ContainerProperties.MaxWeight;

    }

    private static bool ValidSides(BoxToBePacked boxToBePlaced, Region region)
    {
        return boxToBePlaced.GetRotatedSizes().AllLessOrEqualThan(region.GetSizes());
    }

}

