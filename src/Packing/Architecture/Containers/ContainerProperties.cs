namespace EvolutionaryContainerPacking.Packing.Architecture.Containers;

using EvolutionaryContainerPacking.Packing.Architecture.Geometry;

/// <summary>
/// Represents the core properties of a container.
/// Immutable record storing container sizes and maximum allowed weight.
/// </summary>
/// <param name="Sizes">Sizes of the container.</param>
/// <param name="MaxWeight">Maximum total weight the container can hold.</param>
public record class ContainerProperties(Sizes Sizes, int MaxWeight);