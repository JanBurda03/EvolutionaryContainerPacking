public class PackingVectorNonUsingPartDecoder<T> : IPackingVectorPartDecoder<T>
{
    // decoder that return the same value no matter the packing vector input
    private readonly T _possibility;
    public bool IsUsingPackingVector { get; init; }
    public PackingVectorNonUsingPartDecoder(T possibility)
    {
        _possibility = possibility;
        IsUsingPackingVector = false;
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

    public T Decode(PackingRulesCell cell)
    {
        return _possibility;
    }
}