public interface IElitism<T>
{
    public IReadOnlyList<EvaluatedIndividual<T>> GetElites(IReadOnlyList<EvaluatedIndividual<T>> population, int numberOfElites);
}

