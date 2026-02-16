namespace EvolutionaryContainerPacking.Packing.Architecture.EMR;

using EvolutionaryContainerPacking.Packing.Architecture.Geometry;

/// <summary>
/// Splits an empty maximal region into smaller regions after a box has been placed inside.
/// Only handles splitting in X and Y dimensions; assumes region is fully on base (Z-level).
/// </summary>
public class EMRSpliter
{
    /// <summary>
    /// Splits the original region by removing the space occupied by a box.
    /// Produces zero or more new regions representing the remaining empty space.
    /// </summary>
    /// <param name="original">The original empty maximal region.</param>
    /// <param name="occupied">The region occupied by a newly packed box.</param>
    /// <returns>A list of remaining regions after removing the occupied area.</returns>
    /// <exception cref="Exception">
    /// Thrown if either the original or occupied region is floating).
    /// </exception>
    public IEnumerable<Region> SplitRegion(Region original, Region occupied)
    {
        List<Region> spaces = new List<Region>();


        if (original.Start.Z != occupied.Start.Z)
        {
            if (original.Start.Z < occupied.Start.Z)
            {
                throw new Exception($"Newly placed object must always be fully on base, new occupied is {occupied} and the empty space was {original}");
            }

            else
            {
                throw new Exception($"Empty maximal space must always be fully on base, new occupied is {occupied} and the empty space was {original}");
            }
        }


        int a_start = original.Start.X;
        int b_start = original.Start.Y;

        int a_end = original.End.X;
        int b_end = original.End.Y;

        int x_start = occupied.Start.X;
        int y_start = occupied.Start.Y;

        int x_end = occupied.End.X;
        int y_end = occupied.End.Y;
        
        if (x_start > a_start && x_start < a_end)
        {
            spaces.Add(new Region(new Coordinates(a_start, b_start, original.Start.Z), new Coordinates(x_start, b_end, original.End.Z)));
        }
         
        if (y_start > b_start && y_start < b_end)
        {
            spaces.Add(new Region(new Coordinates(a_start, b_start, original.Start.Z), new Coordinates(a_end, y_start, original.End.Z)));
        }

        if (x_end < a_end && x_end > a_start)
        {
            spaces.Add(new Region(new Coordinates(x_end, b_start, original.Start.Z), new Coordinates(a_end, b_end, original.End.Z)));
        }

        if (y_end < b_end && y_end > b_start)
        {
            spaces.Add(new Region(new Coordinates(a_start, y_end, original.Start.Z), new Coordinates(a_end, b_end, original.End.Z)));

        }

        return spaces;
    }
}