public class EvolutionStatistics<T>: IIterationStatistics<T>
{
    // class used for gaining additional data from the evolution, for example for creating graphs
    public IReadOnlyList<IterationData<T>> IterationScore => _iterationScore.AsReadOnly();

    private readonly List<IterationData<T>> _iterationScore = new List<IterationData<T>>();
    public void Update(int currentIteration, IReadOnlyList<EvaluatedIndividual<T>> population, EvaluatedIndividual<T> best)
    {
        var currentScore = EvaluatedIndividual<T>.GetFitnesses(population);

        double average = currentScore.Sum() / currentScore.Count;

        _iterationScore.Add(new IterationData<T>(currentIteration, best.Fitness, average));

        Console.WriteLine($"Best Score of Iteration {currentIteration} is {best.Fitness} with Average of {average}");
    }


}


