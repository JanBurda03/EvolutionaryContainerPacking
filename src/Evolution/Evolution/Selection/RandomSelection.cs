public class RandomSelection<T> : ISelection<T>
{
    private readonly Random _random;

    public RandomSelection()
    {
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
        int index = _random.Next(population.Count);

        return population[index];
    }


}