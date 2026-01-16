public readonly record struct BoxToBePacked
{
    public BoxProperties BoxProperties { get; init; }

    public Rotation Rotation { get; init; }

    public PlacementHeuristic PlacementHeuristic { get; init; }

    public BoxToBePacked(BoxProperties boxProperties,  Rotation rotation, PlacementHeuristic placementHeuristic)
    {
        BoxProperties = boxProperties;
        Rotation = rotation;
        PlacementHeuristic = placementHeuristic;
    }

    public PackedBox ToPackedBox(PlacementInfo placementInfo)
    {

        if (placementInfo.OccupiedRegion.GetSizes() != GetRotatedSizes())
        {
            throw new Exception("The sizes of the occupied region do not correspond to the sizes of the rotated box!");
        }
        return new PackedBox(placementInfo, BoxProperties, Rotation);
    }

    public Sizes GetRotatedSizes()
    {
        return BoxProperties.Sizes.GetRotatedSizes(Rotation);
    }

    public long GetVolume()
    {
        return BoxProperties.Sizes.GetVolume();
    }
}
