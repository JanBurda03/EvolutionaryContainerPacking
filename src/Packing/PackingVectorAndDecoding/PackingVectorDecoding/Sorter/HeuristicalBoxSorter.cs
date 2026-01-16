public class HeuristicalBoxSorter : IBoxToBePackedSorter
{
    // sorter that uses only heuristics for sorting, not the packing vector
    public bool IsUsingPackingVector { get; init; }
    private readonly IComparer<BoxToBePacked> _boxComparer;
    public HeuristicalBoxSorter(IComparer<BoxToBePacked> boxComparer)
    {
        _boxComparer = boxComparer;
        IsUsingPackingVector = false;
    }
    public IReadOnlyList<BoxToBePacked> Sort(IReadOnlyList<BoxToBePacked> unsortedBoxes, PackingRules _)
    {
        var copy = unsortedBoxes.ToList();
        copy.Sort(_boxComparer);
        return copy;
    }
}