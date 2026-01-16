public interface ICrossover<T>
{
    public T Crossover(T a, T b);

    public IReadOnlyList<T> Crossover(IReadOnlyList<T> a, IReadOnlyList<T> b);

    public T Crossover(EvaluatedIndividual<T> a, EvaluatedIndividual<T> b);

    public IReadOnlyList<T> Crossover(IReadOnlyList<EvaluatedIndividual<T>> a, IReadOnlyList<EvaluatedIndividual<T>> b);
}