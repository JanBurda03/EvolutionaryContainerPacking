namespace EvolutionaryContainerPacking.Packing.Architecture.Geometry;

/// <summary>
/// Represents an axis-aligned 3D region defined by start and end coordinates.
/// The region is automatically standardized so that Start <= End in all axes.
/// </summary>
public readonly record struct Region
{
    /// <summary>
    /// Lower (minimum) corner of the region.
    /// </summary>
    public Coordinates Start { get; init; }

    /// <summary>
    /// Upper (maximum) corner of the region.
    /// </summary>
    public Coordinates End { get; init; }

    /// <summary>
    /// Initializes a new region defined by two coordinates.
    /// The coordinates are automatically ordered so that Start <= End.
    /// </summary>
    /// <param name="start">First corner.</param>
    /// <param name="end">Opposite corner.</param>
    public Region(Coordinates start, Coordinates end) => (Start, End) = Standardize(start, end);


    /// <summary>
    /// Determines whether this region intersects with another region.
    /// </summary>
    /// <param name="anotherRegion">Region to test against.</param>
    /// <returns>True if regions overlap in all three dimensions.</returns>
    public bool IntersectsWith(Region anotherRegion)
    {

        return 
            OneDimIntersection((Start.X, End.X), (anotherRegion.Start.X, anotherRegion.End.X)) && 
            OneDimIntersection((Start.Y, End.Y), (anotherRegion.Start.Y, anotherRegion.End.Y)) && 
            OneDimIntersection((Start.Z, End.Z), (anotherRegion.Start.Z, anotherRegion.End.Z));


        bool OneDimIntersection((int start, int end) interval1, (int start, int end) interval2)
        {
            return Math.Max(interval1.start, interval2.start) < Math.Min(interval1.end, interval2.end);
        }
    }



    /// <summary>
    /// Determines whether this region is completely contained within another region.
    /// </summary>
    public bool IsSubregionOf(Region region) => Start.AllGreaterOrEqualThan(region.Start) && End.AllLessOrEqualThan(region.End);

    /// <summary>
    /// Determines whether this region completely contains another region.
    /// </summary>
    public bool IsOverregionOf(Region region) => Start.AllLessOrEqualThan(region.Start) && End.AllGreaterOrEqualThan(region.End);

    /// <summary>
    /// Ensures coordinates are ordered so that Start represents
    /// the minimum corner and End the maximum corner.
    /// </summary>
    private (Coordinates, Coordinates) Standardize(Coordinates a, Coordinates b)
    {
        int xStart = Math.Min(a.X, b.X);
        int yStart = Math.Min(a.Y, b.Y);
        int zStart = Math.Min(a.Z, b.Z);

        int xEnd = Math.Max(a.X, b.X);
        int yEnd = Math.Max(a.Y, b.Y);
        int zEnd = Math.Max(a.Z, b.Z);

        return (new Coordinates(xStart, yStart, zStart), new Coordinates(xEnd, yEnd, zEnd));
    }

    /// <summary>
    /// Returns the size of the region along each axis.
    /// </summary>
    public Sizes GetSizes()
    { 
        return new Sizes(End.X - Start.X, End.Y - Start.Y, End.Z - Start.Z);
    }

    /// <summary>
    /// Computes the volume of the region.
    /// </summary>
    public long GetVolume()
    {
        return (long)(End.X - Start.X) * (End.Y - Start.Y) * (End.Z - Start.Z);
    }
}

