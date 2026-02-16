namespace EvolutionaryContainerPacking.Packing.Architecture.Boxes;

using EvolutionaryContainerPacking.Packing.Architecture.Geometry;

/// <summary>
/// Common interface for all box types that have properties and rotation.
/// </summary>
public interface IBox
{
    /// <summary>
    /// Core properties of the box (size, weight, ID).
    /// </summary>
    BoxProperties BoxProperties { get; }

    /// <summary>
    /// Rotation applied to the box.
    /// </summary>
    Rotation Rotation { get; }
}