public class PackingVectorUsingPartDecoder<T> : IPackingVectorPartDecoder<T>
{
    // decoder that selects item from the list based on the packing vector cell value
    public bool IsUsingPackingVector { get; init; }
    private readonly IReadOnlyList<T> _possibilities;
    public PackingVectorUsingPartDecoder(IReadOnlyList<T> possibilities)
    {
        _possibilities = possibilities;
        IsUsingPackingVector = true;
    }

    public IReadOnlyList<T> DecodeMultiple(PackingRules packingVector)
    {
        T[] decoded = new T[packingVector.Count];
        for (int i = 0; i < decoded.Length; i++)
        {
            decoded[i] = Decode(packingVector[i]);
        }
        return decoded;
    }

    public T Decode(PackingRulesCell packingVectorCell)
    {
        // packing vector cell has values between 0 and 1, so all the possible indexes are uniformly set as intervals in between these values
        int index = (int)((double)packingVectorCell * _possibilities.Count);
        return _possibilities[index];
    }
}