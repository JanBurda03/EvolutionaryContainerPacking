
namespace EvolutionaryContainerPacking.Packing.Architecture.Containers;

using EvolutionaryContainerPacking.Packing.Architecture.Boxes;

using EvolutionaryContainerPacking.Packing.Architecture.Geometry;

/// <summary>
/// Immutable snapshot of an opened container state.
/// Contains aggregated metrics, placed boxes, empty regions,
/// and container properties.
/// </summary>
public readonly record struct ContainerData
{
    /// <summary>
    /// Container identifier.
    /// </summary>
    public int ID { get; init; }

    /// <summary>
    /// Total weight of packed boxes.
    /// </summary>
    public long Weight { get; init; }

    /// <summary>
    /// Total occupied volume inside the container.
    /// </summary>
    public long OccupiedVolume { get; init; }

    /// <summary>
    /// Current list of empty maximal regions.
    /// </summary>
    public IReadOnlyList<Region> EmptyMaximalRegions { get; init; }

    /// <summary>
    /// Boxes placed inside the container.
    /// </summary>
    public IReadOnlyList<PlacedBox> PlacedBoxes { get; init; }

    /// <summary>
    /// Static container properties (sizes and weight limit).
    /// </summary>
    public ContainerProperties ContainerProperties { get; init; }

    /// <summary>
    /// Initializes a new container data snapshot.
    /// </summary>
    /// <param name="id">Container identifier.</param>
    /// <param name="weight">Total packed weight.</param>
    /// <param name="occupiedVolume">Total occupied volume.</param>
    /// <param name="emptyMaximalRegions">Current empty maximal regions.</param>
    /// <param name="placedBoxes">Boxes placed in the container.</param>
    /// <param name="containerProperties">Container properties.</param>
    public ContainerData(int id, long weight, long occupiedVolume, IReadOnlyList<Region> emptyMaximalRegions, IReadOnlyList<PlacedBox> placedBoxes, ContainerProperties containerProperties)
    {
        ID = id;
        Weight = weight;
        OccupiedVolume = occupiedVolume;
        EmptyMaximalRegions = emptyMaximalRegions;
        ContainerProperties = containerProperties;
        PlacedBoxes = placedBoxes;

    }

    /// <summary>
    /// Gets the relative volume utilization of the container (0–1).
    /// </summary>
    public double GetRelativeVolume()
    {
        return (double)OccupiedVolume / ContainerProperties.Sizes.GetVolume();
    }

    /// <summary>
    /// Gets the relative weight utilization of the container (0–1).
    /// </summary>
    public double GetRelativeWeight()
    {
        return (double)Weight / ContainerProperties.MaxWeight;
    }
}

