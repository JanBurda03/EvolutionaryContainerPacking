public class PackingRulesUniformCrossover: ICrossover<PackingRules>
{
    private readonly double _averageElementsChangedToFirst; 
    private readonly Random _random;

    public PackingRulesUniformCrossover(double averageElementsChangedToFirst)
    {

        _averageElementsChangedToFirst = averageElementsChangedToFirst;
        _random = new Random();
    }

    public PackingRules Crossover(PackingRules a, PackingRules b)
    {
        if (a.Count != b.Count)
            throw new ArgumentException("PackingRules must have the same length!");

        double prob = _averageElementsChangedToFirst/a.Count;


        double[] result = new double[a.Count];
        for (int i = 0; i < a.Count; i++)
        {
            // uniform crossover means that every cell in the new individual is chosen from one of the two parents with certain probability
            if (_random.NextDouble() < prob)
            {
                result[i] = a[i];
            }
            else
            {
                result[i] = b[i];
            }

        }
        return new PackingRules(result);
    }

    public IReadOnlyList<PackingRules> Crossover(IReadOnlyList<PackingRules> la, IReadOnlyList<PackingRules> lb)
    {
        if (la.Count != lb.Count)
            throw new ArgumentException("Lists must have the same length!");

        PackingRules[] crossovered = new PackingRules[lb.Count];

        for (int x = 0; x < lb.Count; x++)
        {
            crossovered[x] = Crossover(la[x], lb[x]);
        }
        return crossovered;
    }

    public PackingRules Crossover(EvaluatedIndividual<PackingRules> x, EvaluatedIndividual<PackingRules> y)
    {
        return Crossover(x.Individual, y.Individual);
    }

    public IReadOnlyList<PackingRules> Crossover(IReadOnlyList<EvaluatedIndividual<PackingRules>> x, IReadOnlyList<EvaluatedIndividual<PackingRules>> y)
    {
        var xi = EvaluatedIndividual<PackingRules>.GetIndividuals(x);
        var yi = EvaluatedIndividual<PackingRules>.GetIndividuals(y);
        return Crossover(xi, yi);
    }


}