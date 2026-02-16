namespace EvolutionaryContainerPacking.Packing.Architecture.Boxes;

using EvolutionaryContainerPacking.Packing.Architecture.Placements;
using EvolutionaryContainerPacking.Packing.Architecture.Geometry;

/// <summary>
/// Represents a box that is pending placement in a container.
/// Contains box properties, current rotation, and a placement heuristic.
/// </summary>
public readonly record struct PendingBox : IBox
{
    // <summary>
    /// Core properties of the box (size, weight, ID).
    /// </summary>
    public BoxProperties BoxProperties { get; init; }

    /// <summary>
    /// Current rotation of the box for placement.
    /// </summary>
    public Rotation Rotation { get; init; }

    /// <summary>
    /// Heuristic used to determine where the box will be placed.
    /// </summary>
    public PlacementHeuristic PlacementHeuristic { get; init; }

    public PendingBox(BoxProperties boxProperties,  Rotation rotation, PlacementHeuristic placementHeuristic)
    {
        BoxProperties = boxProperties;
        Rotation = rotation;
        PlacementHeuristic = placementHeuristic;
    }

    /// <summary>
    /// Converts this pending box into a packed box using the specified placement information.
    /// </summary>
    /// <param name="placement">The placement info defining where and how the box is packed.</param>
    /// <returns>A PackedBox representing the box in the container.</returns>
    /// <exception cref="Exception">Thrown if the occupied region size does not match the rotated box size.</exception>
    public PlacedBox Pack(Placement placement)
    {
        // Validates that the box fits exactly in the occupied region
        if (placement.OccupiedRegion.GetSizes() != this.GetRotatedSizes())
        {
            throw new Exception("The sizes of the occupied region do not correspond to the sizes of the rotated box!");
        }

        return new PlacedBox(placement, BoxProperties, Rotation);
    }
}
