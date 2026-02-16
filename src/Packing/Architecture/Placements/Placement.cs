namespace EvolutionaryContainerPacking.Packing.Architecture.Placements;

using EvolutionaryContainerPacking.Packing.Architecture.Geometry;


/// <summary>
/// Represents a concrete placement of a box inside a container.
/// </summary>
public readonly record struct Placement
{
    /// <summary>
    /// Identifier of the container where the box is placed.
    /// </summary>
    public int ContainerID { get; init; }

    /// <summary>
    /// The region occupied by the box within the container.
    /// </summary>
    public Region OccupiedRegion { get; init; }

    /// <summary>
    /// Initializes a new placement instance.
    /// </summary>
    /// <param name="containerID">Target container identifier.</param>
    /// <param name="occupiedRegion">Region occupied inside the container.</param>
    public Placement(int containerID, Region occupiedRegion)
    {
        ContainerID = containerID;
        OccupiedRegion = occupiedRegion;
    }
}