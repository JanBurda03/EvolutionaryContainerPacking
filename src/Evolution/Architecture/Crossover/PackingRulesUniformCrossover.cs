namespace EvolutionaryContainerPacking.Evolution.Architecture.Crossover;

using EvolutionaryContainerPacking.Packing.Rules;

/// <summary>
/// Implements uniform crossover for PackingRules.
/// Each element in the offspring is chosen from one of the parents
/// according to a given probability.
/// </summary>
public class PackingRulesUniformCrossover : ICrossover<PackingRules>
{
    private readonly double _percentageOfElementsChangedToFirst;
    private readonly Random _random;

    /// <summary>
    /// Initializes the crossover with a probability controlling
    /// inheritance from the first parent.
    /// </summary>
    /// <param name="percentageOfElementsChangedToFirst">
    /// Percentage of elements inherited from the first parent.
    /// </param>
    public PackingRulesUniformCrossover(double percentageOfElementsChangedToFirst)
    {
        if (percentageOfElementsChangedToFirst < 0 || percentageOfElementsChangedToFirst > 1)
        {
            throw new Exception("Percentage of elements inherited from the first parent must always be between 0 and 1");
        }

        _percentageOfElementsChangedToFirst = percentageOfElementsChangedToFirst;
        _random = new Random();
    }

    /// <summary>
    /// Performs uniform crossover on two PackingRules.
    /// </summary>
    public PackingRules Crossover(PackingRules a, PackingRules b)
    {
        if (a.Count != b.Count)
            throw new ArgumentException("PackingRules must have the same length!");

        double prob = Math.Min(1.0, _percentageOfElementsChangedToFirst / a.Count);

        double[] result = new double[a.Count];
        for (int i = 0; i < a.Count; i++)
        {
            // Each gene is chosen from parent a with probability 'prob', else from b
            result[i] = _random.NextDouble() < prob ? a[i] : b[i];
        }
        return new PackingRules(result);
    }

    /// <summary>
    /// Performs uniform crossover on two lists of PackingRules.
    /// </summary>
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

    /// <summary>
    /// Performs uniform crossover on two evaluated individuals.
    /// </summary>
    public PackingRules Crossover(EvaluatedIndividual<PackingRules> x, EvaluatedIndividual<PackingRules> y)
    {
        return Crossover(x.Individual, y.Individual);
    }

    /// <summary>
    /// Performs uniform crossover on two lists of evaluated individuals.
    /// </summary>
    public IReadOnlyList<PackingRules> Crossover(IReadOnlyList<EvaluatedIndividual<PackingRules>> x, IReadOnlyList<EvaluatedIndividual<PackingRules>> y)
    {
        var xi = EvaluatedIndividual<PackingRules>.GetIndividuals(x);
        var yi = EvaluatedIndividual<PackingRules>.GetIndividuals(y);
        return Crossover(xi, yi);
    }
}