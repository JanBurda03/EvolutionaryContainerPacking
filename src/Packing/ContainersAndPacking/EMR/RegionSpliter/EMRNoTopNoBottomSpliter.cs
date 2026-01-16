internal class EMRNoTopNoBottomSpliter : IRegionSpliter
{
    public IEnumerable<Region> SplitRegion(Region original, Region occupied)
    {
        List<Region> spaces = new List<Region>();


        if (original.Start.Z != occupied.Start.Z)
        {
            if (original.Start.Z < occupied.Start.Z)
            {
                throw new Exception($"Newly placed object must always be fully on base, new occupied is {occupied} and the empty space was {original}");
            }

            else 
            {
                throw new Exception($"Empty maximal space must always be fully on base, new occupied is {occupied} and the empty space was {original}");
            }
        }
        

        int a_start = original.Start.X;
        int b_start = original.Start.Y;

        int a_end = original.End.X;
        int b_end = original.End.Y;

        int x_start = occupied.Start.X;
        int y_start = occupied.Start.Y;

        int x_end = occupied.End.X;
        int y_end = occupied.End.Y;

        if (x_start > a_start)
        {
            spaces.Add(new Region(new Coordinates(a_start, b_start, original.Start.Z), new Coordinates(x_start, b_end, original.End.Z)));
        }

        if (y_start > b_start)
        {
            spaces.Add(new Region(new Coordinates(a_start, b_start, original.Start.Z), new Coordinates(a_end, y_start, original.End.Z)));
        }

        if (x_end < a_end)
        {
            spaces.Add(new Region(new Coordinates(x_end, b_start, original.Start.Z), new Coordinates(a_end, b_end, original.End.Z)));
        }

        if (y_end < b_end)
        {
            spaces.Add(new Region(new Coordinates(a_start, y_end, original.Start.Z), new Coordinates(a_end, b_end, original.End.Z)));

        }

        return spaces;
    }
}