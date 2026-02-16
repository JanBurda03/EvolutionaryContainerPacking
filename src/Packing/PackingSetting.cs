namespace EvolutionaryContainerPacking.Packing;

/// <summary>
/// Configuration settings controlling the packing solver behavior.
/// Includes allowed placement heuristics, rotation permission, and optional packing order heuristic.
/// </summary>
public readonly record struct PackingSetting
{
    /// <summary>
    /// Initializes a new packing setting.
    /// </summary>
    /// <param name="selectedPlacementHeuristics">Placement heuristics names. Must not be empty.</param>
    /// <param name="allowRotations">Whether rotations are allowed.</param>
    /// <param name="selectedPackingOrderHeuristic">Optional packing order heuristic.</param>
    /// <exception cref="ArgumentException">Thrown if no placement heuristics are provided.</exception>
    public PackingSetting(string[] selectedPlacementHeuristics, bool allowRotations, string? selectedPackingOrderHeuristic)
    {
        if (selectedPlacementHeuristics == null || selectedPlacementHeuristics.Length == 0)
        {
            throw new ArgumentException("At least one placement heuristic must be provided.", nameof(selectedPlacementHeuristics));
        }

        SelectedPlacementHeuristics = selectedPlacementHeuristics;
        AllowRotations = allowRotations;
        SelectedPackingOrderHeuristic = selectedPackingOrderHeuristic;
    }

    /// <summary>
    /// Names of placement heuristics to be used for choosing container and region for boxes.
    /// </summary>
    public string[] SelectedPlacementHeuristics { get; init; }

    /// <summary>
    /// Indicates whether boxes are allowed to be rotated during packing.
    /// </summary>
    public bool AllowRotations { get; init; }

    /// <summary>
    /// Optional packing order heuristic for sorting boxes before packing.
    /// Can be null if no specific order heuristic is used.
    /// </summary>
    public string? SelectedPackingOrderHeuristic { get; init; }
}