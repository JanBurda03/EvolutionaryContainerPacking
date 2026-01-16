public readonly record struct PackedBox
{
    public PlacementInfo PlacementInfo { get; init; }

    public BoxProperties BoxProperties { get; init; }

    public Rotation Rotation { get; init; }

    public PackedBox(PlacementInfo placementInfo, BoxProperties boxProperties, Rotation rotation)
    {
        PlacementInfo = placementInfo;
        BoxProperties = boxProperties;
        Rotation = rotation;
    }

    public Sizes GetSizes()
    {
        return BoxProperties.Sizes;
    }

    public long GetVolume()
    {
        return BoxProperties.Sizes.GetVolume();
    }
}