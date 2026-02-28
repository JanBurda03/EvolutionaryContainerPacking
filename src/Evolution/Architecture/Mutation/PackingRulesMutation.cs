namespace EvolutionaryContainerPacking.Evolution.Architecture.Mutation;

using EvolutionaryContainerPacking.Packing.Rules;

/// <summary>
/// Mutation strategy for <see cref="PackingRules"/>.
/// Each element is changed with probability derived from
/// the desired average number of modified elements.
/// </summary>
public class PackingRulesMutation : IMutation<PackingRules>
{
    private readonly double _averageElementsChanged;
    private readonly Random _random;

    public PackingRulesMutation(double averageElementsChanged)
    {
        _averageElementsChanged = averageElementsChanged;
        _random = new Random();
    }

    /// <summary>
    /// Mutates a single packing rule vector.
    /// </summary>
    public PackingRules Mutate(PackingRules p)
    {
        double prob = Math.Min(1.0, _averageElementsChanged / p.Count);

        double[] result = new double[p.Count];

        for (int i = 0; i < p.Count; i++)
        {
            if (_random.NextDouble() < prob)
            {
                result[i] = _random.NextDouble();
            }
            else
            {
                result[i] = p[i];
            }
        }

        return new PackingRules(result);
    }

    /// <summary>
    /// Mutates a collection of packing rules.
    /// </summary>
    public IReadOnlyList<PackingRules> Mutate(IReadOnlyList<PackingRules> pl)
    {
        PackingRules[] mutated =
            new PackingRules[pl.Count];

        for (int i = 0; i < pl.Count; i++)
        {
            mutated[i] = Mutate(pl[i]);
        }

        return mutated;
    }

    /// <summary>
    /// Mutates a single evaluated individual.
    /// </summary>
    public PackingRules Mutate(EvaluatedIndividual<PackingRules> p)
    {
        return Mutate(p.Individual);
    }

    /// <summary>
    /// Mutates a collection of evaluated individuals.
    /// </summary>
    public IReadOnlyList<PackingRules> Mutate(IReadOnlyList<EvaluatedIndividual<PackingRules>> pl)
    {
        return Mutate(EvaluatedIndividual<PackingRules>.GetIndividuals(pl));
    }
}