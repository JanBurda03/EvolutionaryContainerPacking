// heuristics that are used to order the boxes to be packed
public static class OrderHeuristics
{
    private static readonly IReadOnlyDictionary<string, IComparer<BoxToBePacked>> OrderHeuristicsDictionary = new Dictionary<string, IComparer<BoxToBePacked>>
        {
            {"HighVolumeFirstHeuristic", new HighVolumeFirstHeuristic() },
            {"HighAreaBaseFirstHeuristic", new HighAreaBaseFirstHeuristic()},
            {"LongestFirstHeuristic", new LongestFirstHeuristic()}
           
        };

    public static IReadOnlyList<string> OrderHeuristicsList => OrderHeuristicsDictionary.Keys.ToArray();

    public static IComparer<BoxToBePacked> GetOrderHeuristic(string orderHeuristic)
    {
        if (OrderHeuristicsDictionary.TryGetValue(orderHeuristic, out var comparer))
            return comparer;

        throw new ArgumentException($"Unknown heuristic: {orderHeuristic}");
    }



    public class HighVolumeFirstHeuristic : IComparer<BoxToBePacked>
    {
        public int Compare(BoxToBePacked a, BoxToBePacked b)
        {
            long sa = a.BoxProperties.Sizes.GetVolume();
            long sb = b.BoxProperties.Sizes.GetVolume();
            return sb.CompareTo(sa);
        }
    }

    public class HighAreaBaseFirstHeuristic : IComparer<BoxToBePacked>
    {
        public int Compare(BoxToBePacked a, BoxToBePacked b)
        {
            Sizes sa = a.GetRotatedSizes();
            Sizes sb = b.GetRotatedSizes();

            return ((long)sb.X * sb.Y).CompareTo((long)sa.X * sa.Y);
        }
    }

    public class LongestFirstHeuristic : IComparer<BoxToBePacked>
    {
        public int Compare(BoxToBePacked a, BoxToBePacked b)
        {
            Sizes sa = a.BoxProperties.Sizes;
            Sizes sb = b.BoxProperties.Sizes;

            return (Math.Max(Math.Max(sb.X, sb.Y), sb.Z)).CompareTo(Math.Max(Math.Max(sa.X, sa.Y), sa.Z));
        }
    }
}




