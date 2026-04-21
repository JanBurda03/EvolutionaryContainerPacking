namespace EvolutionaryContainerPacking.Evolution.Architecture.Elitism;

using EvolutionaryContainerPacking.Evolution.Fitness;

/// <summary>
/// Provides selection of best individuals and splitting a population into elites and non-elites.
/// </summary>
/// <typeparam name="T">Type of the individual.</typeparam>
public class Elitism<T> : IElitism<T>
{
    private readonly FitnessComparer _fitnessComparer;

    /// <summary>
    /// Initializes elitism with minimizing/maximizing criterion.
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
    /// Returns the best individual from the given population
    /// according to the configured fitness comparer.
    /// </summary>
    /// <param name="population">Population to evaluate.</param>
    /// <returns>Best individual in the population.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the population is null or empty.
    /// </exception>
    public EvaluatedIndividual<T> GetElite(IReadOnlyList<EvaluatedIndividual<T>> population)
    {
        if (population == null || population.Count == 0)
            throw new ArgumentException("Population must not be empty.");

        var best = population[0];

        for (int i = 1; i < population.Count; i++)
        {
            if (_fitnessComparer.Compare(population[i], best) < 0)
                best = population[i];
        }

        return best;
    }

    /// <summary>
    /// Returns the N best individuals from the given population
    /// according to the configured fitness comparer.
    /// </summary>
    /// <param name="population">Population to evaluate.</param>
    /// <param name="numberOfElites">Number of best individuals to return.</param>
    /// <returns>Collection of best individuals.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the population is null or contains fewer individuals than requested.
    /// </exception>
    public IReadOnlyList<EvaluatedIndividual<T>> GetElite(IReadOnlyList<EvaluatedIndividual<T>> population, int numberOfElites)
    {
        if (population == null)
            throw new ArgumentNullException(nameof(population));

        if (numberOfElites < 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfElites), "Number of elites must not be negative.");

        if (numberOfElites > population.Count)
            throw new ArgumentException("Population does not contain enough individuals.");

        if (numberOfElites == 0)
            return Array.Empty<EvaluatedIndividual<T>>();

        var extremes = new EvaluatedIndividual<T>[numberOfElites];

        for (int i = 0; i < numberOfElites; i++)
            extremes[i] = population[i];

        int worstIndex = -1;

        for (int i = numberOfElites; i < population.Count; i++)
        {
            if (worstIndex == -1)
                worstIndex = FindWorstIndex(extremes);

            if (_fitnessComparer.Compare(population[i], extremes[worstIndex]) < 0)
            {
                extremes[worstIndex] = population[i];
                worstIndex = -1;
            }
        }

        return extremes;
    }

    /// <summary>
    /// Splits the population into elite and non-elite individuals.
    /// </summary>
    /// <param name="population">The population to split.</param>
    /// <param name="numberOfElites">How many elites should be selected.</param>
    /// <returns>
    /// Tuple containing list of elite individuals and list of non-elite individuals.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the population is null or contains fewer individuals than requested.
    /// </exception>
    public (IReadOnlyList<EvaluatedIndividual<T>> Elites, IReadOnlyList<EvaluatedIndividual<T>> NonElites)
        ElitesNonElitesSplit(IReadOnlyList<EvaluatedIndividual<T>> population, int numberOfElites)
    {
        if (population == null)
            throw new ArgumentNullException(nameof(population));

        if (numberOfElites < 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfElites), "Number of elites must not be negative.");

        if (numberOfElites > population.Count)
            throw new ArgumentException("Population does not contain enough individuals.");

        var sortedPopulation = population.ToArray();
        Array.Sort(sortedPopulation, (a, b) => _fitnessComparer.Compare(a, b));

        var elites = sortedPopulation.Take(numberOfElites).ToArray();
        var nonElites = sortedPopulation.Skip(numberOfElites).ToArray();

        return (elites, nonElites);
    }

    /// <summary>
    /// Finds the index of the worst individual within the current extremes.
    /// </summary>
    private int FindWorstIndex(IReadOnlyList<EvaluatedIndividual<T>> individuals)
    {
        int worstIndex = 0;

        for (int i = 1; i < individuals.Count; i++)
        {
            if (_fitnessComparer.Compare(individuals[worstIndex], individuals[i]) < 0)
                worstIndex = i;
        }

        return worstIndex;
    }
}