public class PackingVectorUsingBoxSorter : IBoxToBePackedSorter
{
    // using packing vector to sort, each packing vector cell correspond to the index of one box to be packed; the cells are then sorted by their values (between 0 a 1) together with the boxes assigned to them
    public bool IsUsingPackingVector { get; init; }
    private readonly PackingPairComparer _packingPairComparer;
    public PackingVectorUsingBoxSorter()
    {
        _packingPairComparer = new PackingPairComparer();
        IsUsingPackingVector = true;
    }
    public IReadOnlyList<BoxToBePacked> Sort(IReadOnlyList<BoxToBePacked> unsortedBoxes, PackingRules packingVector)
    {

        if(packingVector.Count < unsortedBoxes.Count) 
        {
            throw new ArgumentOutOfRangeException("Packing vector is too short!");
        }

        (PackingRulesCell Value, BoxToBePacked Box)[] pairs = new (PackingRulesCell, BoxToBePacked)[unsortedBoxes.Count];
        for (int i = 0; i < unsortedBoxes.Count; i++)
        {
            pairs[i] = (packingVector[i], unsortedBoxes[i]);
        }


        Array.Sort(pairs, _packingPairComparer);

        BoxToBePacked[] sortedBoxesToBePacked = new BoxToBePacked[pairs.Length];
        for (int i = 0; i < sortedBoxesToBePacked.Length; i++)
        {
            sortedBoxesToBePacked[i] = pairs[i].Box;
        }

        return sortedBoxesToBePacked;
    }

    protected class PackingPairComparer : IComparer<(PackingRulesCell value, BoxToBePacked box)>
    {
        public int Compare((PackingRulesCell value, BoxToBePacked box) a, (PackingRulesCell value, BoxToBePacked box) b)
        {
            return a.value.CompareTo(b.value);
        }
    }
}

