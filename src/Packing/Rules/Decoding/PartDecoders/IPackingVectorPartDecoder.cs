namespace EvolutionaryContainerPacking.Packing.Rules.Decoding.PartDecoders;

/// <summary>
/// Decoder for a single part of the packing rules vector.
/// Converts <see cref="PackingRulesCell"/> values (or a <see cref="PackingRules"/> vector)
/// into domain-specific objects of type <typeparamref name="T"/> (e.g., rotations, heuristics).
/// </summary>
/// <typeparam name="T">The type of object that this decoder produces from packing rules cells.</typeparam>
public interface IPackingRulesPartDecoder<T>
{
    /// <summary>
    /// Decodes a single packing rules cell into a domain object of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="cell">The cell to decode.</param>
    /// <returns>The decoded object.</returns>
    public T Decode(PackingRulesCell cell);

    /// <summary>
    /// Decodes a full packing rules vector into a list of objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="packingRules">The packing rules vector.</param>
    /// <returns>A list of decoded objects.</returns>
    public IReadOnlyList<T> Decode(PackingRules packingRules);

    /// <summary>
    /// Indicates whether this decoder uses the packing rules vector at all.
    /// If false, the decoder may produce a fixed value.
    /// </summary>
    public bool IsUsingPackingRules { get; init; }
}