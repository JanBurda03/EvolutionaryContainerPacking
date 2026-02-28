namespace EvolutionaryContainerPacking.Evolution.Architecture.Crossover;

/// <summary>
/// Defines a crossover strategy for evolutionary algorithms.
/// </summary>
/// <typeparam name="T">
/// Type of the individual representation.
/// </typeparam>
public interface ICrossover<T>
{
    /// <summary>
    /// Performs crossover on two individuals.
    /// </summary>
    T Crossover(T a, T b);

    /// <summary>
    /// Performs crossover on two collections of individuals.
    /// </summary>
    IReadOnlyList<T> Crossover(IReadOnlyList<T> a, IReadOnlyList<T> b);

    /// <summary>
    /// Performs crossover on two evaluated individuals.
    /// </summary>
    T Crossover(EvaluatedIndividual<T> a, EvaluatedIndividual<T> b);

    /// <summary>
    /// Performs crossover on two collections of evaluated individuals.
    /// </summary>
    IReadOnlyList<T> Crossover(IReadOnlyList<EvaluatedIndividual<T>> a, IReadOnlyList<EvaluatedIndividual<T>> b);
}