using EvolutionaryContainerPacking.Packing.Architecture.Placements;

namespace EvolutionaryContainerPacking.Packing.Rules.Decoding.PartDecoders;

public class MultiplePlacementsDecoder : PackingRulesUsingPartDecoder<PlacementHeuristic>
{
    public MultiplePlacementsDecoder(IReadOnlyList<PlacementHeuristic> heuristics) : base(heuristics) { }
}

public class OnePlacementDecoder : PackingRulesNonUsingPartDecoder<PlacementHeuristic>
{
    public OnePlacementDecoder(PlacementHeuristic heuristic) : base(heuristic) { }
}