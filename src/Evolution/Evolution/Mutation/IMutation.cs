public interface IMutation<T>
{
    public T Mutate(T a);

    public IReadOnlyList<T> Mutate(IReadOnlyList<T> a);

    public T Mutate(EvaluatedIndividual<T> a);

    public IReadOnlyList<T> Mutate(IReadOnlyList<EvaluatedIndividual<T>> a);
}