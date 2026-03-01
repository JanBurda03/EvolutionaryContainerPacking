namespace EvolutionaryContainerPacking.Evolution.Architecture.Elitism;

using EvolutionaryContainerPacking.Evolution.Fitness;

/// <summary>
/// Provides selection of best (elitism) and worst (anti-elitism)
/// individuals from a population.
/// </summary>
/// <typeparam name="T">Type of the individual.</typeparam>
public class Elitism<T> : IElitism<T>
{
    private readonly FitnessComparer _fitnessComparer;
    private readonly FitnessComparer _reversedComparer;

    /// <summary>
    /// Initializes elitism with minimizing/maximizing criterion.
    /// </summary>
    /// <param name="minimizing">True if lower fitness is better.</param>
    public Elitism(bool minimizing)
    {
        _fitnessComparer = new FitnessComparer(minimizing);
        _reversedComparer = new FitnessComparer(!minimizing);
    }

    /// <summary>
    /// Initializes elitism with a custom fitness comparer.
    /// </summary>
    public Elitism(FitnessComparer fitnessComparer)
    {
        _fitnessComparer = fitnessComparer;
        _reversedComparer = new FitnessComparer(!fitnessComparer.Minimizing);
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
        return GetExtreme(population, _fitnessComparer);
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
    public IReadOnlyList<EvaluatedIndividual<T>> GetElites(IReadOnlyList<EvaluatedIndividual<T>> population, int numberOfElites)
    {
        return GetExtremes(population, numberOfElites, _fitnessComparer);
    }

    /// <summary>
    /// Returns the worst individual from the given population
    /// according to the configured fitness comparer.
    /// </summary>
    /// <param name="population">Population to evaluate.</param>
    /// <returns>Worst individual in the population.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the population is null or empty.
    /// </exception>
    public EvaluatedIndividual<T> GetWorst(IReadOnlyList<EvaluatedIndividual<T>> population)
    {
        return GetExtreme(population, _reversedComparer);
    }

    /// <summary>
    /// Returns the N worst individuals from the given population
    /// according to the configured fitness comparer.
    /// </summary>
    /// <param name="population">Population to evaluate.</param>
    /// <param name="numberOfWorst">Number of worst individuals to return.</param>
    /// <returns>Collection of worst individuals.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the population is null or contains fewer individuals than requested.
    /// </exception>
    public IReadOnlyList<EvaluatedIndividual<T>> GetWorst(IReadOnlyList<EvaluatedIndividual<T>> population, int numberOfWorst)
    {
        return GetExtremes(population, numberOfWorst, _reversedComparer);
    }



    /// <summary>
    /// Returns a single extreme individual according to comparer.
    /// </summary>
    private EvaluatedIndividual<T> GetExtreme(
        IReadOnlyList<EvaluatedIndividual<T>> population,
        FitnessComparer comparer)
    {
        if (population == null || population.Count == 0)
            throw new ArgumentException("Population must not be empty.");

        var best = population[0];

        for (int i = 1; i < population.Count; i++)
        {
            if (comparer.Compare(population[i], best) < 0)
                best = population[i];
        }

        return best;
    }

    /// <summary>
    /// Returns N extreme individuals according to comparer.
    /// </summary>
    private IReadOnlyList<EvaluatedIndividual<T>> GetExtremes(
        IReadOnlyList<EvaluatedIndividual<T>> population,
        int count,
        FitnessComparer comparer)
    {
        if (count <= 0)
            return Array.Empty<EvaluatedIndividual<T>>();

        if (population == null || population.Count < count)
            throw new ArgumentException("Population does not contain enough individuals.");

        // Initialize array with first 'count' individuals
        EvaluatedIndividual<T>[] extremes = new EvaluatedIndividual<T>[count];
        for (int i = 0; i < count; i++)
            extremes[i] = population[i];

        int worstIndex = -1;

        // Process remaining individuals
        for (int i = count; i < population.Count; i++)
        {
            if (worstIndex == -1)
                worstIndex = FindWorstIndex(extremes, comparer);

            if (comparer.Compare(population[i], extremes[worstIndex]) < 0)
            {
                extremes[worstIndex] = population[i];
                worstIndex = -1; // recompute next time
            }
        }

        return extremes;
    }

    /// <summary>
    /// Finds the index of the worst individual within the current extremes
    /// (according to given comparer).
    /// </summary>
    private int FindWorstIndex(
        IReadOnlyList<EvaluatedIndividual<T>> individuals,
        FitnessComparer comparer)
    {
        int worstIndex = 0;

        for (int i = 1; i < individuals.Count; i++)
        {
            // NOTE:
            // Here we invert logic:
            // if current worst is "better" than candidate,
            // then candidate becomes new worst.
            if (comparer.Compare(individuals[worstIndex], individuals[i]) < 0)
                worstIndex = i;
        }

        return worstIndex;
    }
}