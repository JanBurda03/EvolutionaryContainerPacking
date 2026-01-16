internal interface IBoxPacker
{
    public void PackBoxes(IEnumerable<BoxToBePacked> boxesToBePacked);
    public IReadOnlyList<ContainerData> ContainersData { get; }
}