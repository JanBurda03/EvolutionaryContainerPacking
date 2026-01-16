public class MultiplePlacementsDecoder : PackingVectorUsingPartDecoder<PlacementHeuristic>
{
    public MultiplePlacementsDecoder(IReadOnlyList<PlacementHeuristic> heuristics) : base(heuristics) { }
}

public class OnePlacementDecoder : PackingVectorNonUsingPartDecoder<PlacementHeuristic>
{
    public OnePlacementDecoder(PlacementHeuristic heuristic) : base(heuristic) { }
}