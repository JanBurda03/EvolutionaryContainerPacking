public readonly record struct PackingSetting
{    
    public PackingSetting(string[] selectedPlacementHeuristics, bool allowRotations, string? selectedPackingOrderHeuristic)
    {
        SelectedPlacementHeuristics = selectedPlacementHeuristics;
        AllowRotations = allowRotations;
        SelectedPackingOrderHeuristic = selectedPackingOrderHeuristic;
    }
    public string[] SelectedPlacementHeuristics { get; init; }
    public bool AllowRotations { get; init; }
    public string? SelectedPackingOrderHeuristic { get; init; }
}