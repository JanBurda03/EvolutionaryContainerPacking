namespace EvolutionaryContainerPacking.Packing.Rules.Decoding.Sorting;

using EvolutionaryContainerPacking.Packing.Rules;


using EvolutionaryContainerPacking.Packing.Architecture.Boxes;

/// <summary>
/// Sorts pending boxes using values from packing rules.
/// Boxes are ordered by values at corresponding indices.
/// </summary>
public class PackingRulesUsingBoxSorter : IPendingBoxSorter
{
    /// <summary>
    /// Indicates that this sorter relies on packing rules.
    /// </summary>
    public bool IsUsingPackingRules { get; init; } = true;

    private readonly PackingPairComparer _packingPairComparer;

    /// <summary>
    /// Initializes a new instance of the sorter.
    /// </summary>
    public PackingRulesUsingBoxSorter()
    {
        _packingPairComparer = new PackingPairComparer();
    }

    /// <summary>
    /// Orders boxes according to corresponding values in packing rules.
    /// </summary>
    /// <param name="unsortedBoxes">Boxes to be ordered.</param>
    /// <param name="packingRules">Packing rules providing sorting priorities.</param>
    /// <returns>Boxes ordered by packing rule values.</returns>
    public IReadOnlyList<PendingBox> Sort(IReadOnlyList<PendingBox> unsortedBoxes, PackingRules packingRules)
    {
        // Validates that the packing rules covers all boxes.
        if (packingRules.Count < unsortedBoxes.Count)
        {
            throw new ArgumentOutOfRangeException("Packing vector is too short!");
        }

        // Creates value-box pairs for sorting.
        (PackingRulesCell Value, PendingBox Box)[] pairs =
            new (PackingRulesCell, PendingBox)[unsortedBoxes.Count];

        for (int i = 0; i < unsortedBoxes.Count; i++)
        {
            pairs[i] = (packingRules[i], unsortedBoxes[i]);
        }

        Array.Sort(pairs, _packingPairComparer);

        // Extracts sorted boxes from sorted pairs.
        PendingBox[] sortedBoxesToBePacked = new PendingBox[pairs.Length];

        for (int i = 0; i < sortedBoxesToBePacked.Length; i++)
        {
            sortedBoxesToBePacked[i] = pairs[i].Box;
        }

        return sortedBoxesToBePacked;
    }

    /// <summary>
    /// Compares pairs by their packing rule value.
    /// </summary>
    protected class PackingPairComparer : IComparer<(PackingRulesCell value, PendingBox box)>
    {
        public int Compare(
            (PackingRulesCell value, PendingBox box) a,
            (PackingRulesCell value, PendingBox box) b
        )
        {
            return a.value.CompareTo(b.value);
        }
    }
}


