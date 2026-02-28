namespace EvolutionaryContainerPacking.Evolution.Architecture.Selection;

/// <summary>
/// Random selection strategy.
/// Individuals are selected uniformly at random.
/// </summary>
public class RandomSelection<T> : ISelection<T>
{
    private readonly Random _random;

    public RandomSelection()
    {
        _random = new Random();
    }

    /// <summary>
    /// Selects a specified number of random individuals.
    /// </summary>
    public IReadOnlyList<EvaluatedIndividual<T>> Select(IReadOnlyList<EvaluatedIndividual<T>> population, int number)
    {
        EvaluatedIndividual<T>[] selected = new EvaluatedIndividual<T>[number];

        for (int i = 0; i < number; i++)
        {
            selected[i] = Select(population);
        }

        return selected;
    }

    /// <summary>
    /// Selects a single random individual.
    /// </summary>
    public EvaluatedIndividual<T> Select(IReadOnlyList<EvaluatedIndividual<T>> population)
    {
        int index = _random.Next(population.Count);
        return population[index];
    }
}