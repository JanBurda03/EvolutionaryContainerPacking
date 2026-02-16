namespace EvolutionaryContainerPacking.Packing.Architecture.Placements;
using EvolutionaryContainerPacking.Packing.Architecture.Boxes;
using EvolutionaryContainerPacking.Packing.Architecture.Containers;

/// <summary>
/// Represents a placement heuristic.
/// </summary>
/// <param name="pendingBox">Box to be placed.</param>
/// <param name="containers">Available containers.</param>
/// <returns>
/// A valid <see cref="Placement"/> if placement is possible, otherwise null (try a different rotation or open a new container).
/// </returns>
public delegate Placement? PlacementHeuristic(PendingBox pendingBox, IEnumerable<ContainerData> containers);