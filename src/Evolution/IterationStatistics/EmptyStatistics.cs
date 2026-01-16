public class EmptyStatistics<T> : IIterationStatistics<T>
{
    public void Update(int currentIteration, IReadOnlyList<EvaluatedIndividual<T>> population, EvaluatedIndividual<T> best)
    {

    }
}
