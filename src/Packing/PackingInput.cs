namespace EvolutionaryContainerPacking.Packing;

using EvolutionaryContainerPacking.Packing.Architecture.Boxes;
using EvolutionaryContainerPacking.Packing.Architecture.Containers;

/// <summary>
/// Represents the input data for a packing problem.
/// Contains container properties and the list of boxes to be packed.
/// </summary>
public record class PackingInput
{
    /// <summary>
    /// The container specifications, including sizes and maximum weight.
    /// </summary>
    public ContainerProperties ContainerProperties { get; init; }

    /// <summary>
    /// The list of boxes to be packed into the containers.
    /// </summary>
    public IReadOnlyList<BoxProperties> BoxPropertiesList { get; init; }

    /// <summary>
    /// Initializes a new instance of PackingInput with a container and a list of boxes.
    /// </summary>
    /// <param name="containerProperties">Properties of the container.</param>
    /// <param name="boxPropertiesList">List of box properties.</param>
    public PackingInput(ContainerProperties containerProperties, IReadOnlyList<BoxProperties> boxPropertiesList)
    {
        ContainerProperties = containerProperties;
        BoxPropertiesList = boxPropertiesList;
    }
}
