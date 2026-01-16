public class PackingRulesMutation : IMutation<PackingRules>
{
    private readonly double _averageElementsChanged;
    private readonly Random _random;

    public PackingRulesMutation(double averageElementsChanged)
    {
        _averageElementsChanged = averageElementsChanged;
        _random = new Random();
    }

    public PackingRules Mutate(PackingRules p)
    {
        double prob = _averageElementsChanged / p.Count;


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

    public IReadOnlyList<PackingRules> Mutate(IReadOnlyList<PackingRules> pl)
    {
        PackingRules[] mutated = new PackingRules[pl.Count];

        for (int x = 0; x< pl.Count; x++) 
        {
            mutated[x] = Mutate(pl[x]);
        }
        return mutated;
    }

    public PackingRules Mutate(EvaluatedIndividual<PackingRules> p)
    {
        return Mutate(p.Individual);
    }

    public IReadOnlyList<PackingRules> Mutate(IReadOnlyList<EvaluatedIndividual<PackingRules>> pl)
    {
        return Mutate(EvaluatedIndividual<PackingRules>.GetIndividuals(pl));
    }
}