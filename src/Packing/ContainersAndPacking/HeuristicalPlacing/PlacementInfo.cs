public readonly record struct PlacementInfo
{
    // where to place the box - which container and which region in it
    public int ContainerID { get; init; }
    public Region OccupiedRegion { get; init; }

    public PlacementInfo(int containerID, Region occupiedRegion)
    {
        ContainerID = containerID;
        OccupiedRegion = occupiedRegion;
    }
}