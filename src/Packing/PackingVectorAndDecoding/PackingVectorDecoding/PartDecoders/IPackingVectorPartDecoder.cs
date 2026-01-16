public interface IPackingVectorPartDecoder<T>: IPackingVectorUsing
{
    public T Decode(PackingRulesCell cell);
    public IReadOnlyList<T> DecodeMultiple(PackingRules packingVector);
}

