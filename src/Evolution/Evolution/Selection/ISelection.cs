public interface ISelection<T>
{
    public IReadOnlyList<EvaluatedIndividual<T>> Select(IReadOnlyList<EvaluatedIndividual<T>> population, int number);
    public EvaluatedIndividual<T> Select(IReadOnlyList<EvaluatedIndividual<T>> population);
}