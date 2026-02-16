namespace EvolutionaryContainerPacking.Packing.Rules;

/// <summary>
/// Represents a vector of numbers in range [0, 1).
/// Packing rules encode a deterministic solution to the packing problem.
/// </summary>
public readonly struct PackingRules
{
    private PackingRulesCell[] Vector { get; init; }

    /// <summary>
    /// Gets the number of cells in the vector.
    /// </summary>
    public int Count => Vector.Length;

    /// <summary>
    /// Gets the cell at the specified index.
    /// </summary>
    /// <param name="index">Index of the cell.</param>
    public PackingRulesCell this[int index] => Vector[index];

    /// <summary>
    /// Creates a new packing rules vector from normalized values.
    /// </summary>
    /// <param name="vector">Source values.</param>
    /// /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when any packing cell value is outside the valid range [0, 1).
    /// </exception>
    public PackingRules(IReadOnlyList<double> vector)
    {
        Vector = new PackingRulesCell[vector.Count];
        for (int i = 0; i < vector.Count; i++)
        {
            Vector[i] = (PackingRulesCell)vector[i];
        }
    }

    /// <summary>
    /// Returns a subvector of this packing rules vector.
    /// </summary>
    /// <param name="start">Start index.</param>
    /// <param name="length">Number of elements.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the range is invalid.
    /// </exception>
    public PackingRules Slice(int start, int length)
    {
        if (start < 0 || length < 0 || start + length > Count)
        {
            throw new ArgumentOutOfRangeException();
        }

        double[] slice = new double[length];
        for (int i = 0; i < length; i++)
        {
            slice[i] = Vector[start + i];
        }
        return new PackingRules(slice);
    }

    /// <summary>
    /// Creates a vector with random values.
    /// </summary>
    /// <param name="length">Vector length.</param>
    public static PackingRules CreateRandom(int length)
    {
        Random random = Random.Shared;
        var vector = new double[length];
        for (int i = 0; i < length; i++)
        {
            vector[i] = random.NextDouble();
        }
        return new PackingRules(vector);
    }

    /// <summary>
    /// Creates an empty vector.
    /// </summary>
    public static PackingRules CreateEmpty()
    {
        return new PackingRules(Array.Empty<double>());
    }

    /// <summary>
    /// Creates a vector filled with zeros.
    /// </summary>
    /// <param name="length">Vector length.</param>
    public static PackingRules CreateZeros(int length)
    {
        return new PackingRules(new double[length]);
    }

    /// <summary>
    /// Converts the vector to an array of doubles.
    /// </summary>
    public static implicit operator double[](PackingRules vector)
    {
        double[] doubles = new double[vector.Count];
        for (int i = 0; i < vector.Count; i++)
        {
            doubles[i] = vector[i];
        }
        return doubles;
    }

    /// <summary>
    /// Converts the vector to a list of doubles.
    /// </summary>
    public static implicit operator List<double>(PackingRules vector)
    {
        var doubles = new List<double>(vector.Count);
        for (int i = 0; i < vector.Count; i++)
        {
            doubles.Add(vector[i]);
        }
        return doubles;
    }

    /// <summary>
    /// Converts an array of doubles to a packing rules vector.
    /// </summary>
    public static explicit operator PackingRules(double[] doubles)
    {
        return new PackingRules(doubles);
    }

    /// <summary>
    /// Converts a list of doubles to a packing rules vector.
    /// </summary>
    public static explicit operator PackingRules(List<double> doubles)
    {
        return new PackingRules(doubles);
    }
}
