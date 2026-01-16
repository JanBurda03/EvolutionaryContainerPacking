internal interface IRegionSpliter
{
    public IEnumerable<Region> SplitRegion(Region original, Region occupied);
}
