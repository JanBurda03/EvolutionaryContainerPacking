namespace EvolutionaryContainerPacking.App.Output;

using EvolutionaryContainerPacking.Packing.Architecture.Geometry;

/// <summary>
/// Represents export-ready data of a placed box inside a container.
/// </summary>
/// <param name="BoxID">Unique identifier of the box.</param>
/// <param name="Rotation">Applied rotation of the box.</param>
/// <param name="OccupiedRegion">Region occupied by the box within the container.</param>
public record class PlacedBoxExport(
    int BoxID,
    string Rotation,
    Region OccupiedRegion
);