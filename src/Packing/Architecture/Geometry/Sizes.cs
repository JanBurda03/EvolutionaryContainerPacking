namespace EvolutionaryContainerPacking.Packing.Architecture.Geometry;

/// <summary>
/// Represents positive 3D sizes (X, Y, Z).
/// Immutable value type used for box and container sizes.
/// </summary>
public readonly record struct Sizes : ITriplet
{
    /// <summary>
    /// Size along the X axis.
    /// </summary>
    public int X { get; init; }

    /// <summary>
    /// Size along the Y axis.
    /// </summary>
    public int Y { get; init; }

    /// <summary>
    /// Size along the Z axis.
    /// </summary>
    public int Z { get; init; }

    /// <summary>
    /// Initializes a new size triplet.
    /// All dimensions must be greater than zero.
    /// </summary>
    /// <param name="X">Size along X axis.</param>
    /// <param name="Y">Size along Y axis.</param>
    /// <param name="Z">Size along Z axis.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if any dimension is less than or equal to zero.
    /// </exception>
    public Sizes(int X, int Y, int Z)
    {
        if (X <= 0 || Y <= 0 || Z <= 0) 
        {  
            throw new ArgumentOutOfRangeException("Sizes must be greater than 0!"); 
        }

        this.X = X;
        this.Y = Y;
        this.Z = Z;
    }

    /// <summary>
    /// Computes the volume defined by the dimensions.
    /// </summary>
    /// <returns>Volume as X * Y * Z.</returns>
    public long GetVolume()
    {
        return (long)X * Y * Z;
    }

    /// <summary>
    /// Creates a region starting at the specified coordinate and with corresponding sizes.
    /// </summary>
    /// <param name="start">Starting coordinate of the region.</param>
    /// <returns>Region spanning from start to start + size.</returns>
    public Region ToRegion(Coordinates start)
    {

        return new Region(
            new Coordinates(start.X, start.Y, start.Z), 
            new Coordinates(X+start.X, Y+start.Y, Z + start.Z));
    }

    /// <summary>
    /// Creates a region starting at the origin (0,0,0).
    /// </summary>
    /// <returns>Region spanning from origin to (X, Y, Z).</returns>
    public Region ToRegion()
    {

        return new Region(
            new Coordinates(0, 0, 0), 
            new Coordinates(X, Y, Z));
    }

    /// <summary>
    /// Returns dimensions after applying a given rotation.
    /// </summary>
    /// <param name="rotation">Rotation permutation.</param>
    /// <returns>New Sizes representing rotated dimensions.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if rotation value is not supported.
    /// </exception>
    public Sizes GetRotatedSizes(Rotation rotation)
    {
        return rotation switch 
        {
            Rotation.XYZ => new Sizes(X, Y, Z),
            Rotation.XZY => new Sizes(X, Z, Y),
            Rotation.YXZ => new Sizes(Y, X, Z),
            Rotation.YZX => new Sizes(Y, Z, X),
            Rotation.ZXY => new Sizes(Z, X, Y),
            Rotation.ZYX => new Sizes(Z, Y, X),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}