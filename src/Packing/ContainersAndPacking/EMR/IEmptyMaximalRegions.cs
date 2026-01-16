public interface IEmptyMaximalRegions
{
    public IReadOnlyList<Region> EmptyMaximalRegionsList { get; }

    public void UpdateEMR(Region space);
}
