namespace EvolutionaryContainerPacking.Packing.Architecture.Geometry;

/// <summary>
/// Provides component-wise comparison operations for ITriplet types.
/// </summary>
public static class TripletExtensions
{
    /// <summary>
    /// Returns true if all components of <paramref name="a"/> 
    /// are strictly greater than corresponding components of <paramref name="b"/>.
    /// </summary>
    public static bool AllGreaterThan<T>(this T a, T b) where T : ITriplet
    {
        return a.X > b.X && a.Y > b.Y && a.Z > b.Z;
    }

    /// <summary>
    /// Returns true if all components of <paramref name="a"/> 
    /// are strictly less than corresponding components of <paramref name="b"/>.
    /// </summary>
    public static bool AllLessThan<T>(this T a, T b) where T : ITriplet
    {
        return a.X < b.X && a.Y < b.Y && a.Z < b.Z;
    }

    /// <summary>
    /// Returns true if all components of <paramref name="a"/> 
    /// are greater than or equal to corresponding components of <paramref name="b"/>.
    /// </summary>
    public static bool AllGreaterOrEqualThan<T>(this T a, T b) where T : ITriplet
    {
        return a.X >= b.X && a.Y >= b.Y && a.Z >= b.Z;
    }

    /// <summary>
    /// Returns true if all components of <paramref name="a"/> 
    /// are less than or equal to corresponding components of <paramref name="b"/>.
    /// </summary>
    public static bool AllLessOrEqualThan<T>(this T a, T b) where T : ITriplet
    {
        return a.X <= b.X && a.Y <= b.Y && a.Z <= b.Z;
    }
}