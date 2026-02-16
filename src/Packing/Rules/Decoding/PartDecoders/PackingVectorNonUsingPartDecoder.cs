namespace EvolutionaryContainerPacking.Packing.Rules.Decoding.PartDecoders;

/// <summary>
/// A decoder that ignores the packing rules vector and always returns the same fixed value.
/// Useful for parts of the packing vector that are not used, e.g., fixed rotation or single heuristic.
/// </summary>
/// <typeparam name="T">The type of object produced by the decoder.</typeparam>
public class PackingRulesNonUsingPartDecoder<T> : IPackingRulesPartDecoder<T>
{
    // The fixed value to return for every cell.
    private readonly T _possibility;

    /// <summary>
    /// Indicates that this decoder does not use the packing rules vector.
    /// Always returns false.
    /// </summary>
    public bool IsUsingPackingRules { get; init; } = false;

    /// <summary>
    /// Creates a decoder that always returns the same value.
    /// </summary>
    /// <param name="possibility">The fixed value to return.</param>
    public PackingRulesNonUsingPartDecoder(T possibility)
    {
        _possibility = possibility;
    }

    /// <summary>
    /// Decodes an entire packing rules vector.
    /// Returns a list of fixed values (same length as the vector).
    /// </summary>
    /// <param name="packingRules">The packing rules vector (ignored).</param>
    /// <returns>List of fixed values.</returns>
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
    /// Always returns the fixed value.
    /// </summary>
    /// <param name="cell">The cell to decode (ignored).</param>
    /// <returns>The fixed value.</returns>
    public T Decode(PackingRulesCell cell)
    {
        return _possibility;
    }
}
