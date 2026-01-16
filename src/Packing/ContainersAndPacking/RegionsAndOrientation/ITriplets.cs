public interface ITriplet
{
    public int X { get; init; }
    public int Y { get; init; }
    public int Z { get; init; }
}


public static class TripletExtensions
{
    public static bool AllGreaterThan<T>(this T a, T b) where T : ITriplet
    { 
        return a.X > b.X && a.Y > b.Y && a.Z > b.Z;
    }

    public static bool AllLessThan<T>(this T a, T b) where T : ITriplet
    { 
        return a.X < b.X && a.Y < b.Y && a.Z < b.Z;
    }

    public static bool AllGreaterOrEqualThan<T>(this T a, T b) where T : ITriplet
    { 
        return a.X >= b.X && a.Y >= b.Y && a.Z >= b.Z;
    }

    public static bool AllLessOrEqualThan<T>(this T a, T b) where T : ITriplet
    {
        return a.X <= b.X && a.Y <= b.Y && a.Z <= b.Z;
    }
}





