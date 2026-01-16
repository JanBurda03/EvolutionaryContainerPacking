public interface IEvolutionary<T>
{
    public IReadOnlyList<EvaluatedIndividual<T>> Population { get;}
    public EvaluatedIndividual<T> CurrentGenerationBest { get; }
    public EvaluatedIndividual<T> GlobalBest { get;}

    public void Evolve(int numberOfGenerations);
}