
public readonly struct PackingRules
{
    private PackingRulesCell[] Vector { get; init; }

    public int Count => Vector.Length;

    public PackingRulesCell this[int index] => Vector[index];

    public PackingRules(IReadOnlyList<double> vector)
    {
        Vector = new PackingRulesCell[vector.Count];
        for (int i = 0; i < vector.Count; i++)
        {
            Vector[i] = (PackingRulesCell)vector[i];
        }
    }

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

    public static PackingRules CreateEmpty()
    {
        return new PackingRules(Array.Empty<double>());
    }

    public static PackingRules CreateZeros(int length)
    {
        return new PackingRules(new double[length]);
    }

    public static implicit operator double[](PackingRules vector)
    {
        double[] doubles = new double[vector.Count];
        for (int i = 0; i < vector.Count; i++)
        {
            doubles[i] = vector[i];
        }
        return doubles;
    }

    public static implicit operator List<double>(PackingRules vector)
    {
        var doubles = new List<double>(vector.Count);
        for (int i = 0; i < vector.Count; i++)
        {
            doubles.Add(vector[i]);
        }
        return doubles;
    }


    public static explicit operator PackingRules(double[] doubles)
    {
        return new PackingRules(doubles);
    }

    public static explicit operator PackingRules(List<double> doubles)
    {
        return new PackingRules(doubles);
    }











    public static PackingRules operator +(PackingRules a, PackingRules b)
    {
        if (a.Count != b.Count)
            throw new InvalidOperationException("Vector dimensions must match.");

        var result = new double[a.Count];
        for (int i = 0; i < a.Count; i++)
        {
            result[i] = a[i] + b[i];
        }
        return new PackingRules(result);
    }

    public static PackingRules operator -(PackingRules a, PackingRules b)
    {
        if (a.Count != b.Count)
            throw new InvalidOperationException("Vector dimensions must match.");

        var result = new double[a.Count];
        for (int i = 0; i < a.Count; i++)
        {
            result[i] = a[i] - b[i];
        }
        return new PackingRules(result);
    }

    public static PackingRules operator *(PackingRules a, int k)
    {
        var result = new double[a.Count];
        for (int i = 0; i < a.Count; i++)
        {
            result[i] = a[i] * k;
        }
        return new PackingRules(result);
    }


    public static PackingRules operator *(int k, PackingRules a)
    {
        return a * k;

    }

    public static PackingRules operator *(PackingRules a, double k)
    {
        var result = new double[a.Count];
        for (int i = 0; i < a.Count; i++)
        {
            result[i] = a[i] * k;
        }
        return new PackingRules(result);
    }


    public static PackingRules operator *(double k, PackingRules a)
    {
        return a * k;

    }


}