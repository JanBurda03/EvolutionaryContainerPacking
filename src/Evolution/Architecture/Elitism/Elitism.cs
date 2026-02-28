namespace EvolutionaryContainerPacking.Evolution.Architecture.Elitism;

using EvolutionaryContainerPacking.Evolution.Fitness;

/// <summary>
/// Provides selection of elite individuals from a population.
/// </summary>
/// <typeparam name="T">Type of the individual.</typeparam>
public class Elitism<T> : IElitism<T>
{
    private readonly FitnessComparer _fitnessComparer;

    /// <summary>
    /// Initializes elitism with a minimizing/maximizing criterion.
    /// </summary>
    /// <param name="minimizing">True if lower fitness is better.</param>
    public Elitism(bool minimizing)
    {
        _fitnessComparer = new FitnessComparer(minimizing);
    }

    /// <summary>
    /// Initializes elitism with a custom fitness comparer.
    /// </summary>
    public Elitism(FitnessComparer fitnessComparer)
    {
        _fitnessComparer = fitnessComparer;
    }

    /// <summary>
    /// Returns the single best individual from the population.
    /// </summary>
    public EvaluatedIndividual<T> GetElite(IReadOnlyList<EvaluatedIndividual<T>> population)
    {
        if (population == null || population.Count == 0)
            throw new ArgumentException("Population does not contain enough individuals");

        var best = population[0];
        for (int i = 1; i < population.Count; i++)
        {
            if (_fitnessComparer.Compare(population[i], best) < 0)
            {
                best = population[i];
            }
        }
        return best;
    }

    /// <summary>
    /// Returns the top N elite individuals from the population.
    /// </summary>
    /// <param name="population">Population to select from.</param>
    /// <param name="numberOfElites">Number of elites to return.</param>
    public IReadOnlyList<EvaluatedIndividual<T>> GetElites(IReadOnlyList<EvaluatedIndividual<T>> population, int numberOfElites)
    {
        if (numberOfElites <= 0) 
            return Array.Empty<EvaluatedIndividual<T>>();

        if (population == null || population.Count <= numberOfElites)
            throw new ArgumentException("Population does not contain enough individuals");

        // initially fill elite array with first individuals
        EvaluatedIndividual<T>[] elites = new EvaluatedIndividual<T>[numberOfElites];
        for (int i = 0; i < numberOfElites; i++)
        {
            elites[i] = population[i];
        }

        int worstIndex = -1;

        // iterate remaining population to update elites
        for (int i = numberOfElites; i < population.Count; i++)
        {
            if (worstIndex == -1)
                worstIndex = FindWorstIndex(elites);

            if (_fitnessComparer.Compare(population[i], elites[worstIndex]) < 0)
            {
                elites[worstIndex] = population[i];
                worstIndex = -1; // reset to recalc worst
            }
        }

        return elites;
    }

    /// <summary>
    /// Finds the index of the worst individual among elites.
    /// </summary>
    private int FindWorstIndex(IReadOnlyList<EvaluatedIndividual<T>> elites)
    {
        int worstIndex = 0;
        for (int j = 1; j < elites.Count; j++)
        {
            if (_fitnessComparer.Compare(elites[worstIndex], elites[j]) < 0)
                worstIndex = j;
        }
        return worstIndex;
    }
}