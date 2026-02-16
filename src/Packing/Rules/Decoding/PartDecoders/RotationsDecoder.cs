using EvolutionaryContainerPacking.Packing.Architecture.Geometry;

namespace EvolutionaryContainerPacking.Packing.Rules.Decoding.PartDecoders;

public class MultipleRotationsDecoder : PackingRulesUsingPartDecoder<Rotation>
{
    public MultipleRotationsDecoder(IReadOnlyList<Rotation> rotations) : base(rotations) { }
}

public class AllRotationsDecoder : PackingRulesUsingPartDecoder<Rotation>
{
    public AllRotationsDecoder() : base(Enum.GetValues<Rotation>()) { }
}

public class OneRotationDecoder : PackingRulesNonUsingPartDecoder<Rotation>
{
    public OneRotationDecoder(Rotation rotation) : base(rotation) { }
}