internal class Container
{
    public int ID { get; init; }

    public long CurrentWeight { get; private set; }

    public long OccupiedVolume { get; private set; }


    private readonly EmptyMaximalRegions _emptyMaximalRegions;

    private readonly ContainerProperties _containerProperties;

    private List<PackedBox> _packedBoxes;

    private ContainerData? _data;
    public ContainerData Data
    {
        get
        {
            if (_data == null)
            {
                _data = new ContainerData(
                    ID,
                    CurrentWeight,
                    OccupiedVolume,
                    _emptyMaximalRegions.EmptyMaximalRegionsList,
                    PackedBoxes,
                    _containerProperties
                );
            }
            return (ContainerData)_data;
        }
    }

    public IReadOnlyList<PackedBox> PackedBoxes => _packedBoxes;

    public Container(int iD, ContainerProperties containerProperties)
    {
        ID = iD;
        CurrentWeight = 0;
        OccupiedVolume = 0;
        _containerProperties = containerProperties;
        _emptyMaximalRegions = new EmptyMaximalRegions(_containerProperties.Sizes.ToRegion(new Coordinates(0,0,0)));
        _data = null;
        _packedBoxes = new List<PackedBox>();
    }

    public void PackBox(BoxToBePacked boxToBePacked, PlacementInfo placementInfo)
    {
        if (ID != placementInfo.ContainerID) 
        {
            throw new Exception($"The item is supposed to be packed to container {placementInfo.ContainerID}, this is container {ID}");
        }

        // data must be reseted, because they are not updated after adding the new container
        _data = null;

        PackedBox packedBox = boxToBePacked.ToPackedBox(placementInfo);

        CurrentWeight += packedBox.BoxProperties.Weight;

        if (CurrentWeight > _containerProperties.MaxWeight)
        {
            throw new Exception("Maximum weight has been exceeded!");
        }

        _emptyMaximalRegions.UpdateEMR(placementInfo.OccupiedRegion);

        _packedBoxes.Add(packedBox);

        OccupiedVolume += placementInfo.OccupiedRegion.GetVolume();
    }

    
}


