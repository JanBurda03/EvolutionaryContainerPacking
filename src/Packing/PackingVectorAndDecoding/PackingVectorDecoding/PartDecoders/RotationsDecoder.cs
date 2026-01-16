public class MultipleRotationsDecoder : PackingVectorUsingPartDecoder<Rotation>
{
    public MultipleRotationsDecoder(IReadOnlyList<Rotation> rotations) : base(rotations) { }
}

public class AllRotationsDecoder : PackingVectorUsingPartDecoder<Rotation>
{
    public AllRotationsDecoder() : base(Enum.GetValues<Rotation>()) { }
}

public class OneRotationDecoder : PackingVectorNonUsingPartDecoder<Rotation>
{
    public OneRotationDecoder(Rotation rotation) : base(rotation) { }
}