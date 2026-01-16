public class Elitism<T> : IElitism<T>
{
    private readonly FitnessComparer _fitnessComparer;

    public Elitism(bool minimizing)
    {
        _fitnessComparer = new FitnessComparer(minimizing);
    }

    public Elitism(FitnessComparer fitnessComparer)
    {
        _fitnessComparer = fitnessComparer;
    }

    public IReadOnlyList<EvaluatedIndividual<T>> GetElites(IReadOnlyList<EvaluatedIndividual<T>> population, int numberOfElites)
    {
        if (numberOfElites <= 0)
        {
            return Array.Empty<EvaluatedIndividual<T>>();
        }

        if (population.Count <= numberOfElites)
        {
            throw new ArgumentException("Population does not contain enough individuals");
        }


        // array for the best individuals
        EvaluatedIndividual<T>[] elites = new EvaluatedIndividual<T>[numberOfElites];

        // initially filling the elite array
        for (int i  = 0; i < numberOfElites; i++)
        {
            elites[i] = population[i];
        }

        int worstIndex = -1;

        for (int i = numberOfElites; i < population.Count; i++)
        {
            // worstIndex -1 means that it is not known which of the elites has the worst fitness 
            // we need to know worstIndex, because the worst is the one that is going to be replaced next
            if (worstIndex == -1)
            {
                worstIndex = FindWorstIndex(elites);
            }

            // if the next individual has better fitness that the worst of elites, it is replaced
            if (_fitnessComparer.Compare(population[i], elites[worstIndex]) < 0)
            {
                elites[worstIndex] = population[i];

                worstIndex = -1;
            }
            
        }

        return elites;
    }

    private int FindWorstIndex(IReadOnlyList<EvaluatedIndividual<T>> elites)
    {
        // finding the worst index by comparing all elites
        int worstIndex = 0;
        for (int j = 1; j < elites.Count; j++)
        {
            if (_fitnessComparer.Compare(elites[worstIndex], elites[j]) < 0)
                worstIndex = j;
        }
        return worstIndex;
    }

}