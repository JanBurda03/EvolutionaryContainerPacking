public class TournamentSelection<T>:ISelection<T>
{
    private readonly FitnessComparer _fitnessComparer;
    private readonly Random _random;
    private readonly int _tournamentSize;

    public TournamentSelection(int tournamentSize, bool minimizing) : this(tournamentSize, new FitnessComparer(minimizing)) { }

    public TournamentSelection(int tournamentSize, FitnessComparer fitnessComparer)
    {
        if (tournamentSize < 1)
        {
            throw new ArgumentOutOfRangeException("Tournament size must be at least 1!");
        }

        _fitnessComparer = fitnessComparer;
        _random = new Random();
    }

    public IReadOnlyList<EvaluatedIndividual<T>> Select(IReadOnlyList<EvaluatedIndividual<T>> population, int number)
    {
        EvaluatedIndividual<T>[] selected = new EvaluatedIndividual<T>[number];

        for (int i = 0; i < number; i++)
        {
            selected[i] = Select(population);
        }
        return selected;
    }

    public EvaluatedIndividual<T> Select(IReadOnlyList<EvaluatedIndividual<T>> population)
    {
        // randomly selecting certain number of individuals given by the tournamentSize and returning the best one by fitness

        int bestIndex = _random.Next(population.Count);

        for (int j = 1; j < _tournamentSize; j++)
        {
            int challengerIndex = _random.Next(population.Count);

            if (_fitnessComparer.Compare(population[challengerIndex], population[bestIndex]) < 0)
            {
                bestIndex = challengerIndex;
            }
        }

        return population[bestIndex];
    }

    
}

