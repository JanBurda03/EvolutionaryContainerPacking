namespace EvolutionaryContainerPacking.Packing.Architecture.Containers;

using EvolutionaryContainerPacking.Packing.Architecture.EMR;
using EvolutionaryContainerPacking.Packing.Architecture.Boxes;
using EvolutionaryContainerPacking.Packing.Architecture.Placements;

/// <summary>
/// Represents an opened container during the packing process.
/// Tracks placed boxes, occupied volume, weight, and empty maximal regions.
/// </summary>
public class OpenedContainer
{
    /// <summary>
    /// Unique identifier of the container.
    /// </summary>
    public int ID { get; init; }

    /// <summary>
    /// Current total weight of packed boxes.
    /// </summary>
    public long Weight { get; private set; }

    /// <summary>
    /// Total occupied volume inside the container.
    /// </summary>
    public long OccupiedVolume { get; private set; }

    private readonly EmptyMaximalRegions _emptyMaximalRegions;
    private readonly ContainerProperties _containerProperties;
    private List<PlacedBox> _placedBoxes;
    private ContainerData? _data;

    /// <summary>
    /// Snapshot data of the container state.
    /// Lazily created and invalidated after each packing operation.
    /// </summary>
    public ContainerData Data
    {
        get
        {
            if (_data == null)
            {
                _data = new ContainerData(
                    ID,
                    Weight,
                    OccupiedVolume,
                    _emptyMaximalRegions.EmptyMaximalRegionsList,
                    PlacedBoxes,
                    _containerProperties
                );
            }
            return (ContainerData)_data;
        }
    }

    /// <summary>
    /// List of boxes currently placed in the container.
    /// </summary>
    public IReadOnlyList<PlacedBox> PlacedBoxes => _placedBoxes;

    /// <summary>
    /// Initializes a new opened container with given ID and properties.
    /// </summary>
    /// <param name="id">Container identifier.</param>
    /// <param name="containerProperties">Container size and weight limits.</param>
    public OpenedContainer(int id, ContainerProperties containerProperties)
    {
        ID = id;
        Weight = 0;
        OccupiedVolume = 0;
        _containerProperties = containerProperties;
        _emptyMaximalRegions = new EmptyMaximalRegions(_containerProperties.Sizes);
        _data = null;
        _placedBoxes = new List<PlacedBox>();
    }

    /// <summary>
    /// Packs a pending box into the container at the specified placement.
    /// Updates weight, occupied volume, and empty maximal regions.
    /// </summary>
    /// <param name="pendingBox">Box to be packed.</param>
    /// <param name="placement">Placement information defining the occupied region and container ID.</param>
    /// <exception cref="Exception">
    /// Thrown if the placement refers to a different container
    /// or if the maximum weight is exceeded.
    /// </exception>
    public void PackBox(PendingBox pendingBox, Placement placement)
    {
        if (ID != placement.ContainerID) 
        {
            throw new Exception($"The item is supposed to be packed to container {placement.ContainerID}, this is container {ID}");
        }

        // data must be reseted, because they are not updated after adding the new container
        _data = null;

        PlacedBox placedBox = pendingBox.Pack(placement);

        Weight += placedBox.BoxProperties.Weight;

        if (Weight > _containerProperties.MaxWeight)
        {
            throw new Exception("Maximum weight has been exceeded!");
        }

        _emptyMaximalRegions.Update(placement.OccupiedRegion);

        _placedBoxes.Add(placedBox);

        OccupiedVolume += placement.OccupiedRegion.GetVolume();
    }
}


