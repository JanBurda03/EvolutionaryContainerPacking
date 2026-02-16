namespace EvolutionaryContainerPacking.Packing.Architecture.Boxes;

using EvolutionaryContainerPacking.Packing.Architecture.Placements;
using EvolutionaryContainerPacking.Packing.Architecture.Geometry;

/// <summary>
/// Represents a box that has been placed inside a container.
/// Contains placement info, rotation, and original box properties.
/// </summary>
public readonly record struct PlacedBox : IBox
{
    /// <summary>
    /// Placement information of the box within a container.
    /// </summary>
    public Placement Placement { get; init; }

    /// <summary>
    /// Original properties of the box (sizes, weight, ID).
    /// </summary>
    public BoxProperties BoxProperties { get; init; }

    /// <summary>
    /// Rotation applied to the box when placed.
    /// </summary>
    public Rotation Rotation { get; init; }

    /// <summary>
    /// Initializes a new packed box with placement, properties, and rotation.
    /// </summary>
    /// <param name="placement">Placement information.</param>
    /// <param name="boxProperties">Original box properties.</param>
    /// <param name="rotation">Rotation applied to the box.</param>
    public PlacedBox(Placement placement, BoxProperties boxProperties, Rotation rotation)
    {
        Placement = placement;
        BoxProperties = boxProperties;
        Rotation = rotation;
    }
}
