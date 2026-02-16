namespace EvolutionaryContainerPacking.Packing.Architecture.Geometry;

/// <summary>
/// Represents a 3D coordinate in space.
/// </summary>
public readonly record struct Coordinates : ITriplet
{
    /// <summary>
    /// X coordinate.
    /// </summary>
    public int X { get; init; }

    /// <summary>
    /// Y coordinate.
    /// </summary>
    public int Y { get; init; }

    /// <summary>
    /// Z coordinate.
    /// </summary>
    public int Z { get; init; }

    /// <summary>
    /// Initializes a new 3D coordinate.
    /// </summary>
    /// <param name="X">X coordinate value.</param>
    /// <param name="Y">Y coordinate value.</param>
    /// <param name="Z">Z coordinate value.</param>
    public Coordinates(int X, int Y, int Z)
    {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
    }

    /// <summary>
    /// Computes the Euclidean distance to another coordinate.
    /// </summary>
    /// <param name="coordinates">Target coordinate.</param>
    /// <returns>Euclidean distance between the two points.</returns>
    public double GetEuclidanDistanceTo(Coordinates coordinates)
    {
        int dx = X - coordinates.X;
        int dy = Y - coordinates.Y;
        int dz = Z - coordinates.Z;
        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }

}