namespace EvolutionaryContainerPacking.Evolution.Architecture.Selection;

using EvolutionaryContainerPacking.Evolution.Fitness;

/// <summary>
/// Tournament selection strategy.
/// The best individual is chosen from a random subset of the population.
/// </summary>
public class TournamentSelection<T> : ISelection<T>
{
    private readonly FitnessComparer _fitnessComparer;
    private readonly Random _random;
    private readonly int _tournamentSize;

    public TournamentSelection(int tournamentSize, bool minimizing): this(tournamentSize, new FitnessComparer(minimizing))
    {}

    public TournamentSelection(int tournamentSize, FitnessComparer fitnessComparer)
    {
        if (tournamentSize < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(tournamentSize),
                "Tournament size must be at least 1.");
        }

        _tournamentSize = tournamentSize;
        _fitnessComparer = fitnessComparer;
        _random = new Random();
    }

    /// <summary>
    /// Selects a specified number of individuals using tournament selection.
    /// </summary>
    public IReadOnlyList<EvaluatedIndividual<T>> Select(
        IReadOnlyList<EvaluatedIndividual<T>> population,
        int number)
    {
        EvaluatedIndividual<T>[] selected =
            new EvaluatedIndividual<T>[number];

        for (int i = 0; i < number; i++)
        {
            selected[i] = Select(population);
        }

        return selected;
    }

    /// <summary>
    /// Selects a single individual using tournament selection.
    /// </summary>
    public EvaluatedIndividual<T> Select(
        IReadOnlyList<EvaluatedIndividual<T>> population)
    {
        int bestIndex = _random.Next(population.Count);

        for (int j = 1; j < _tournamentSize; j++)
        {
            int challengerIndex = _random.Next(population.Count);

            if (_fitnessComparer.Compare(
                    population[challengerIndex],
                    population[bestIndex]) < 0)
            {
                bestIndex = challengerIndex;
            }
        }

        return population[bestIndex];
    }
}

