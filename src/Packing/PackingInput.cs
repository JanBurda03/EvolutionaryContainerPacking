public record class PackingInput
{
    public ContainerProperties ContainerProperties { get; init; }
    public IReadOnlyList<BoxProperties> BoxPropertiesList { get; init; }

    public PackingInput(ContainerProperties containerProperties, IReadOnlyList<BoxProperties> boxPropertiesList)
    {
        ContainerProperties = containerProperties;
        BoxPropertiesList = boxPropertiesList;
    }

    public int GetLowerBound()
    {
        double weight = BoxPropertiesList.Sum(x => x.Weight);
        double volume = BoxPropertiesList.Sum(x => x.Sizes.GetVolume());

        return (int)Math.Ceiling(
            Math.Max(
            weight / ContainerProperties.MaxWeight,
            volume / ContainerProperties.Sizes.GetVolume()
            )
            );
    }
}

public record class ContainerProperties(Sizes Sizes, int MaxWeight);

public record class BoxProperties(int ID, Sizes Sizes, int Weight);