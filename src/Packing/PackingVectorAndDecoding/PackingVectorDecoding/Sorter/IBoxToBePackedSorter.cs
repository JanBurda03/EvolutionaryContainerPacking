public interface IBoxToBePackedSorter:IPackingVectorUsing
{
    public IReadOnlyList<BoxToBePacked> Sort(IReadOnlyList<BoxToBePacked> unsortedBoxes, PackingRules packingVector);
}