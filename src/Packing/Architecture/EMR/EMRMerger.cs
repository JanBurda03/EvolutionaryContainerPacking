using EvolutionaryContainerPacking.Packing.Architecture.Geometry;
namespace EvolutionaryContainerPacking.Packing.Architecture.EMR;

/// <summary>
/// Handles merging of regions that start at the same Z height,
/// applying touch rules in X and Y directions.
/// </summary>
public class EMRMerger
{
    /// <summary>
    /// Merges the upperRegion with all regions in sameBaseRegions.
    /// Uses a queue to process all chain merges via TouchX / TouchY rules.
    /// Returns a new list of merged regions (does not modify input list).
    /// </summary>
    /// <param name="upperRegion">The initial region above the box.</param>
    /// <param name="sameBaseRegions">List of regions that start at the same Z height.</param>
    /// <returns>List of merged regions.</returns>
    public List<Region> Merge(Region upperRegion, List<Region> sameBaseRegions)
    {
        // Queue for BFS-style merge processing
        var queue = new Queue<Region>();
        queue.Enqueue(upperRegion);

        // HashSet to store merged results (avoid duplicates)
        var result = new HashSet<Region>();

        // Process all regions in the queue
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            // Try merging with all original regions
            foreach (var other in sameBaseRegions)
            {
                if (TouchX(current, other))
                {
                    var merged = MergeX(current, other);

                    // Only enqueue new merged regions if not already processed or in queue
                    if (!result.Contains(merged) && !queue.Contains(merged))
                    {
                        queue.Enqueue(merged);
                    }
                }
                else if (TouchY(current, other))
                {
                    var merged = MergeY(current, other);

                    // Only enqueue new merged regions if not already processed or in queue
                    if (!result.Contains(merged) && !queue.Contains(merged))
                    {
                        queue.Enqueue(merged);
                    }
                }
            }

            // Add current region to result (once all merges considered)
            result.Add(current);
        }

        return result.ToList();
    }

    /// <summary>
    /// Determines if two regions touch in the X direction.
    /// Regions must share a side in X and have non-zero overlap in Y.
    /// </summary>
    private bool TouchX(Region a, Region b)
    {
        bool xTouch = a.End.X == b.Start.X || a.Start.X == b.End.X;
        bool yOverlap = Math.Min(a.End.Y, b.End.Y) > Math.Max(a.Start.Y, b.Start.Y);
        return xTouch && yOverlap;
    }

    /// <summary>
    /// Determines if two regions touch in the Y direction.
    /// Regions must share a side in Y and have non-zero overlap in X.
    /// </summary>
    private bool TouchY(Region a, Region b)
    {
        bool yTouch = a.End.Y == b.Start.Y || a.Start.Y == b.End.Y;
        bool xOverlap = Math.Min(a.End.X, b.End.X) > Math.Max(a.Start.X, b.Start.X);
        return yTouch && xOverlap;
    }

    /// <summary>
    /// Merges two regions horizontally (X axis).
    /// Keeps the overlap in Y as the new vertical bounds.
    /// Assumes regions touch in X.
    /// </summary>
    private Region MergeX(Region a, Region b)
    {
        int minX = Math.Min(a.Start.X, b.Start.X);
        int maxX = Math.Max(a.End.X, b.End.X);

        // Keep overlapping Y interval
        int minY = Math.Max(a.Start.Y, b.Start.Y);
        int maxY = Math.Min(a.End.Y, b.End.Y);

        return new Region(new Coordinates(minX, minY, a.Start.Z),
                          new Coordinates(maxX, maxY, a.End.Z));
    }

    /// <summary>
    /// Merges two regions vertically (Y axis).
    /// Keeps the overlap in X as the new horizontal bounds.
    /// Assumes regions touch in Y.
    /// </summary>
    private Region MergeY(Region a, Region b)
    {
        int minY = Math.Min(a.Start.Y, b.Start.Y);
        int maxY = Math.Max(a.End.Y, b.End.Y);

        // Keep overlapping X interval
        int minX = Math.Max(a.Start.X, b.Start.X);
        int maxX = Math.Min(a.End.X, b.End.X);

        return new Region(new Coordinates(minX, minY, a.Start.Z),
                          new Coordinates(maxX, maxY, a.End.Z));
    }
}