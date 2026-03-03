namespace EvolutionaryContainerPacking.App.Output;

/// <summary>
/// Represents export-ready data of a container,
/// including its current state and packed boxes.
/// </summary>
/// <param name="ContainerID">Unique identifier of the container.</param>
/// <param name="CurrentWeight">Total weight of all packed boxes.</param>
/// <param name="OccupiedVolume">Total occupied volume inside the container.</param>
/// <param name="PackedBoxes">Collection of packed boxes within the container.</param>
public record class ContainerExport(
    int ContainerID,
    long CurrentWeight,
    long OccupiedVolume,
    IReadOnlyList<PlacedBoxExport> PackedBoxes
);