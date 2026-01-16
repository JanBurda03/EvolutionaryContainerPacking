public readonly record struct ContainerData
{
    public int ID { get; init; }
    public long CurrentWeight { get; init; }

    public long OccupiedVolume { get; init; }

    public IReadOnlyList<Region> EMR { get; init; }

    public IReadOnlyList<PackedBox> PackedBoxes { get; init; }


    public ContainerProperties ContainerProperties { get; init; }

    public ContainerData(int id, long currentWeight, long occupiedVolume, IReadOnlyList<Region> emr, IReadOnlyList<PackedBox> packedBoxes, ContainerProperties containerProperties)
    {
        ID = id;
        CurrentWeight = currentWeight;
        OccupiedVolume = occupiedVolume;
        EMR = emr;
        ContainerProperties = containerProperties;
        PackedBoxes = packedBoxes;

    }

    public double GetRelativeVolume()
    {
        return (double)OccupiedVolume / ContainerProperties.Sizes.GetVolume();
    }

    public double GetRelativeWeight()
    {
        return (double)CurrentWeight / ContainerProperties.MaxWeight;
    }
}

