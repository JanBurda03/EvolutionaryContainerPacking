namespace EvolutionaryContainerPacking.Packing.Rules.Decoding.Sorting;

using EvolutionaryContainerPacking.Packing.Architecture.Boxes;
using EvolutionaryContainerPacking.Packing.Architecture.Geometry;

/// <summary>
/// Provides predefined ordering heuristics for pending boxes.
/// </summary>
public static class OrderHeuristics
{
    private static readonly IReadOnlyDictionary<string, IComparer<PendingBox>> OrderHeuristicsDictionary =
        new Dictionary<string, IComparer<PendingBox>>
        {
            {"HighVolumeFirstHeuristic", new HighVolumeFirstHeuristic() },
            {"HighAreaBaseFirstHeuristic", new HighAreaBaseFirstHeuristic()},
            {"LongestFirstHeuristic", new LongestFirstHeuristic()},
            {"HighWeightFirstHeuristic", new HighWeightFirstHeuristic()},
            {"HighSurfaceAreaFirstHeuristic", new HighSurfaceAreaFirstHeuristic()}
        };

    /// <summary>
    /// Gets the list of available heuristic names.
    /// </summary>
    public static IReadOnlyList<string> OrderHeuristicsList =>
        OrderHeuristicsDictionary.Keys.ToArray();

    /// <summary>
    /// Returns the comparer corresponding to the given heuristic name.
    /// </summary>
    /// <param name="orderHeuristic">Name of the heuristic.</param>
    /// <returns>Comparer implementing the selected heuristic.</returns>
    public static IComparer<PendingBox> GetOrderHeuristic(string orderHeuristic)
    {
        if (OrderHeuristicsDictionary.TryGetValue(orderHeuristic, out var comparer))
            return comparer;

        throw new ArgumentException($"Unknown heuristic: {orderHeuristic}");
    }

    /// <summary>
    /// Orders boxes by descending volume.
    /// </summary>
    private class HighVolumeFirstHeuristic : IComparer<PendingBox>
    {
        public int Compare(PendingBox a, PendingBox b)
        {
            long sa = a.BoxProperties.Sizes.GetVolume();
            long sb = b.BoxProperties.Sizes.GetVolume();
            return sb.CompareTo(sa);
        }
    }

    /// <summary>
    /// Orders boxes by descending base area using rotated sizes.
    /// </summary>
    private class HighAreaBaseFirstHeuristic : IComparer<PendingBox>
    {
        public int Compare(PendingBox a, PendingBox b)
        {
            Sizes sa = a.GetRotatedSizes();
            Sizes sb = b.GetRotatedSizes();

            return ((long)sb.X * sb.Y).CompareTo((long)sa.X * sa.Y);
        }
    }

    /// <summary>
    /// Orders boxes by descending longest edge.
    /// </summary>
    private class LongestFirstHeuristic : IComparer<PendingBox>
    {
        public int Compare(PendingBox a, PendingBox b)
        {
            Sizes sa = a.BoxProperties.Sizes;
            Sizes sb = b.BoxProperties.Sizes;

            return Math.Max(Math.Max(sb.X, sb.Y), sb.Z)
                .CompareTo(Math.Max(Math.Max(sa.X, sa.Y), sa.Z));
        }
    }

    /// <summary>
    /// Orders boxes by descending weight.
    /// </summary>
    private class HighWeightFirstHeuristic : IComparer<PendingBox>
    {
        public int Compare(PendingBox a, PendingBox b)
        {
            int wa = a.BoxProperties.Weight;
            int wb = b.BoxProperties.Weight;

            return wb.CompareTo(wa);
        }
    }

    /// <summary>
    /// Orders boxes by descending surface area of the bounding box.
    /// </summary>
    private class HighSurfaceAreaFirstHeuristic : IComparer<PendingBox>
    {
        public int Compare(PendingBox a, PendingBox b)
        {
            Sizes sa = a.BoxProperties.Sizes;
            Sizes sb = b.BoxProperties.Sizes;

            long surfaceA = ((long)sa.X * sa.Y + (long)sa.X * sa.Z + (long)sa.Y * sa.Z);
            long surfaceB = ((long)sb.X * sb.Y + (long)sb.X * sb.Z + (long)sb.Y * sb.Z);

            return surfaceB.CompareTo(surfaceA);
        }
    }
}



