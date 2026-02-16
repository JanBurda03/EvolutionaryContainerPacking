namespace EvolutionaryContainerPacking.Packing.Rules.Decoding.PartDecoders;

/// <summary>
/// A decoder that selects an item from a predefined list based on the packing rules cell value.
/// Converts continuous values in the packing vector (0..1) into discrete selections from the list.
/// </summary>
/// <typeparam name="T">The type of object produced by the decoder.</typeparam>
public class PackingRulesUsingPartDecoder<T> : IPackingRulesPartDecoder<T>
{
    /// <summary>
    /// Indicates that this decoder uses the packing rules vector.
    /// </summary>
    public bool IsUsingPackingRules { get; init; } = true;

    // List of possible items to select from.
    private readonly IReadOnlyList<T> _possibilities;

    /// <summary>
    /// Creates a new decoder that uses the packing rules vector to select items.
    /// </summary>
    /// <param name="possibilities">List of possible items that the decoder can select.</param>
    public PackingRulesUsingPartDecoder(IReadOnlyList<T> possibilities)
    {
        _possibilities = possibilities;
    }

    /// <summary>
    /// Decodes an entire packing rules vector into a list of selected items.
    /// Each cell is mapped to one item from <see cref="_possibilities"/>.
    /// </summary>
    /// <param name="packingRules">The packing rules vector.</param>
    /// <returns>List of decoded items corresponding to each cell.</returns>
    public IReadOnlyList<T> Decode(PackingRules packingRules)
    {
        T[] decoded = new T[packingRules.Count];
        for (int i = 0; i < decoded.Length; i++)
        {
            decoded[i] = Decode(packingRules[i]);
        }
        return decoded;
    }

    /// <summary>
    /// Decodes a single packing rules cell.
    /// Maps the cell's value (0..1) to an index in the <see cref="_possibilities"/> list.
    /// </summary>
    /// <param name="packingRulesCell">The packing rules cell.</param>
    /// <returns>The selected item from <see cref="_possibilities"/>.</returns>
    public T Decode(PackingRulesCell packingRulesCell)
    {
        // Map 0..1 range to discrete indexes
        int index = (int)((double)packingRulesCell * _possibilities.Count);

        // Ensure index is within bounds (edge case when cell == 1.0 is prevented by PackingRulesCell)
        return _possibilities[index];
    }
}
