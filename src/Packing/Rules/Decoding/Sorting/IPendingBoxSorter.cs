namespace EvolutionaryContainerPacking.Packing.Rules.Decoding.Sorting;

using EvolutionaryContainerPacking.Packing.Architecture.Boxes;

/// <summary>
/// Provides a strategy for ordering pending boxes before packing.
/// </summary>
public interface IPendingBoxSorter
{
    /// <summary>
    /// Indicates whether packing rules are used during sorting.
    /// </summary>
    public bool IsUsingPackingRules { get; init; }

    /// <summary>
    /// Returns boxes ordered according to the implemented strategy.
    /// </summary>
    /// <param name="unsortedBoxes">Boxes to be ordered.</param>
    /// <param name="packingRules">Packing constraints.</param>
    /// <returns>Ordered collection of boxes.</returns>
    public IReadOnlyList<PendingBox> Sort(IReadOnlyList<PendingBox> unsortedBoxes, PackingRules packingRules);
}