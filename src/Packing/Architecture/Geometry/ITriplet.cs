namespace EvolutionaryContainerPacking.Packing.Architecture.Geometry;

/// <summary>
/// Represents a 3D triplet of integer values (X, Y, Z).
/// Used as a common abstraction for coordinates and sizes.
/// </summary>
public interface ITriplet
{
    /// <summary>
    /// X component.
    /// </summary>
    int X { get; init; }

    /// <summary>
    /// Y component.
    /// </summary>
    int Y { get; init; }

    /// <summary>
    /// Z component.
    /// </summary>
    int Z { get; init; }
}




