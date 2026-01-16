
public readonly record struct Region
{
    public Coordinates Start { get; init; }
    public Coordinates End { get; init; }

    public Region(Coordinates start, Coordinates end) => (Start, End) = Standardize(start, end);

    public bool IntersectsWith(Region anotherRegion)
    {

        return OneDimIntersection((Start.X, End.X), (anotherRegion.Start.X, anotherRegion.End.X)) && OneDimIntersection((Start.Y, End.Y), (anotherRegion.Start.Y, anotherRegion.End.Y)) && OneDimIntersection((Start.Z, End.Z), (anotherRegion.Start.Z, anotherRegion.End.Z));


        bool OneDimIntersection((int start, int end) interval1, (int start, int end) interval2)
        {
            if (interval1.start >= interval1.end || interval2.start >= interval2.end)
                throw new ArgumentException("Invalid region definition, start coordintates must always be smaller that end coordinates!");

            return Math.Max(interval1.start, interval2.start) < Math.Min(interval1.end, interval2.end);
        }
    }


    public bool IsSubregionOf(Region region) => Start.AllGreaterOrEqualThan(region.Start) && End.AllLessOrEqualThan(region.End);

    public bool IsOverregionOf(Region region) => Start.AllLessOrEqualThan(region.Start) && End.AllGreaterOrEqualThan(region.End);


    private (Coordinates, Coordinates) Standardize(Coordinates a, Coordinates b)
    {
        int xStart = Math.Min(a.X, b.X);
        int yStart = Math.Min(a.Y, b.Y);
        int zStart = Math.Min(a.Z, b.Z);

        int xEnd = Math.Max(a.X, b.X);
        int yEnd = Math.Max(a.Y, b.Y);
        int zEnd = Math.Max(a.Z, b.Z);

        return (new Coordinates(xStart, yStart, zStart), new Coordinates(xEnd, yEnd, zEnd));
    }

    public Sizes GetSizes()
    { 
        return new Sizes(End.X - Start.X, End.Y - Start.Y, End.Z - Start.Z);
    }


    public long GetVolume()
    {
        return (long)(End.X - Start.X) * (End.Y - Start.Y) * (End.Z - Start.Z);
    }
}

