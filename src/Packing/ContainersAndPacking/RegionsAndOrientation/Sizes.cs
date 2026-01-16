public readonly record struct Sizes : ITriplet
{
    public int X { get; init; }
    public int Y { get; init; }
    public int Z { get; init; }

    public Sizes(int X, int Y, int Z)
    {
        if (X <= 0 || Y <= 0 || Z <= 0) 
        {  
            throw new ArgumentOutOfRangeException("Sizes must be greater than 0!"); 
        }

        this.X = X;
        this.Y = Y;
        this.Z = Z;
    }

    public long GetVolume()
    {
        return (long)X * Y * Z;
    }

    public Region ToRegion(Coordinates start)
    {

        return new Region(new Coordinates(start.X, start.Y, start.Z), new Coordinates(X+start.X, Y+start.Y, Z + start.Z));
    }

    public Sizes GetRotatedSizes(Rotation rotation)
    {
        return rotation switch 
        {
            Rotation.XYZ => new Sizes(X, Y, Z),
            Rotation.XZY => new Sizes(X, Z, Y),
            Rotation.YXZ => new Sizes(Y, X, Z),
            Rotation.YZX => new Sizes(Y, Z, X),
            Rotation.ZXY => new Sizes(Z, X, Y),
            Rotation.ZYX => new Sizes(Z, Y, X),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}