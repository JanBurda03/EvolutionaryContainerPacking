namespace EvolutionaryContainerPacking.Packing.Rules.Decoding.Sorting;

using EvolutionaryContainerPacking.Packing.Architecture.Boxes;

public class HeuristicalBoxSorter : IPendingBoxSorter
{
    public bool IsUsingPackingRules { get; init; } = false;
    private readonly IComparer<PendingBox> _heuristicalBoxComparer;
    public HeuristicalBoxSorter(IComparer<PendingBox> boxComparer)
    {
        _heuristicalBoxComparer = boxComparer;
    }
    public IReadOnlyList<PendingBox> Sort(IReadOnlyList<PendingBox> unsortedBoxes, PackingRules _)
    {
        var copy = unsortedBoxes.ToList();
        copy.Sort(_heuristicalBoxComparer);
        return copy;
    }
}