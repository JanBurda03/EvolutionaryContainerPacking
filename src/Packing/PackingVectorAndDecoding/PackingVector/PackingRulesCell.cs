
public readonly record struct PackingRulesCell:IComparable<PackingRulesCell>
{
    private const int MaxValue = ushort.MaxValue; // 65535
    private readonly ushort _value;

    public PackingRulesCell(double value)
    {
        if (value < 0.0 || value >= 1.0)
            throw new ArgumentOutOfRangeException(nameof(value), "Value must be in range [0, 1)");
        _value = (ushort)(value * MaxValue);
    }

    public int CompareTo(PackingRulesCell other)
    {
        return _value.CompareTo(other._value);
    }

    public double ToDouble() => _value / (double)MaxValue;

    // any packing vector cell can be implicitly converted into double
    public static implicit operator double(PackingRulesCell v) => v.ToDouble();
    // conversion from double to packing vector must always be explicit because of the constaint of the number being between 0 and 1
    public static explicit operator PackingRulesCell(double d) => new PackingRulesCell(d);




    public static PackingRulesCell operator +(PackingRulesCell a, PackingRulesCell b)
    {
        double result = a.ToDouble() + b.ToDouble();
        result = Math.Min(Math.Max(result, 0.0), 0.999999); 
        return new PackingRulesCell(result);
    }

    public static PackingRulesCell operator -(PackingRulesCell a, PackingRulesCell b)
    {
        double result = a.ToDouble() - b.ToDouble();
        result = Math.Min(Math.Max(result, 0.0), 0.999999);
        return new PackingRulesCell(result);
    }

    public static PackingRulesCell operator *(PackingRulesCell a, int k)
    {
        double result = a.ToDouble() * k;
        result = Math.Min(Math.Max(result, 0.0), 0.999999);
        return new PackingRulesCell(result);
    }

    public static PackingRulesCell operator *(int k, PackingRulesCell a)
    {
        return a * k;
    }

    public static PackingRulesCell operator *(PackingRulesCell a, double k)
    {
        double result = a.ToDouble() * k;
        result = Math.Min(Math.Max(result, 0.0), 0.999999);
        return new PackingRulesCell(result);
    }

    public static PackingRulesCell operator *(double k, PackingRulesCell a)
    {
        return a * k;
    }
}