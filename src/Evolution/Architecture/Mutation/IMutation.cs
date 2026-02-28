namespace EvolutionaryContainerPacking.Evolution.Architecture.Mutation;

/// <summary>
/// Defines a mutation strategy for evolutionary algorithms.
/// </summary>
/// <typeparam name="T">
/// Type of the individual representation.
/// </typeparam>
public interface IMutation<T>
{
    /// <summary>
    /// Mutates a single individual.
    /// </summary>
    T Mutate(T a);

    /// <summary>
    /// Mutates a collection of individuals.
    /// </summary>
    IReadOnlyList<T> Mutate(IReadOnlyList<T> a);

    /// <summary>
    /// Mutates a single evaluated individual.
    /// </summary>
    T Mutate(EvaluatedIndividual<T> a);

    /// <summary>
    /// Mutates a collection of evaluated individuals.
    /// </summary>
    IReadOnlyList<T> Mutate(IReadOnlyList<EvaluatedIndividual<T>> a);
}