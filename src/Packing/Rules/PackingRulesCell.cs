namespace EvolutionaryContainerPacking.Packing.Rules;

/// <summary>
/// Represents a single packing rules cell stored as a normalized value in range [0, 1).
/// Internally the value is stored as an unsigned 16-bit integer for compactness.
/// </summary>
public readonly record struct PackingRulesCell:IComparable<PackingRulesCell>
{
    private const int MaxValue = ushort.MaxValue; // 65535
    private readonly ushort _value;

    /// <summary>
    /// Creates a new cell from a normalized value.
    /// </summary>
    /// <param name="value">Value in range [0, 1).</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when value is outside the valid range.
    /// </exception>
    public PackingRulesCell(double value)
    {
        if (value < 0.0 || value >= 1.0)
            throw new ArgumentOutOfRangeException(nameof(value), "Value must be in range [0, 1)");
        _value = (ushort)(value * MaxValue);
    }

    /// <summary>
    /// Compares this cell with another cell.
    /// </summary>
    /// <param name="other">Cell to compare with.</param>
    /// <returns>
    /// Less than zero if this cell is smaller, zero if equal, greater than zero if larger.
    /// </returns>
    public int CompareTo(PackingRulesCell other)
    {
        return _value.CompareTo(other._value);
    }

    /// <summary>
    /// Returns the cell value as a double.
    /// </summary>
    /// <returns>Value in range [0, 1).</returns>
    public double ToDouble() => _value / (double)MaxValue;

    /// <summary>
    /// Converts the cell to double.
    /// </summary>
    public static implicit operator double(PackingRulesCell v) => v.ToDouble();

    /// <summary>
    /// Converts a double to a packing rules cell.
    /// </summary>
    /// <param name="d">Value in range [0, 1).</param>
    public static explicit operator PackingRulesCell(double d) => new PackingRulesCell(d);
}