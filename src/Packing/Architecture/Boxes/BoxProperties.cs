namespace EvolutionaryContainerPacking.Packing.Architecture.Boxes;

using EvolutionaryContainerPacking.Packing.Architecture.Geometry;

/// <summary>
/// Represents the core properties of a box.
/// Immutable record storing identifier, dimensions, and weight.
/// </summary>
/// <param name="ID">Unique identifier of the box.</param>
/// <param name="Sizes">Sizes of the box.</param>
/// <param name="Weight">Weight of the box.</param>
public record class BoxProperties(int ID, Sizes Sizes, int Weight);