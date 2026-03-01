namespace EvolutionaryContainerPacking.Evolution.Architecture.Memetic;

/// <summary>
/// Represents a memetic (local improvement) operator.
/// A memetic operator applies a local search or refinement step
/// to individuals after evolutionary operators (e.g., crossover, mutation).
/// </summary>
/// <typeparam name="T">Type of the individual.</typeparam>
public interface IMemetic<T>
{
    /// <summary>
    /// Applies local improvement to a single individual.
    /// </summary>
    /// <param name="individual">Individual to improve.</param>
    /// <returns>Locally improved evaluated individual.</returns>
    EvaluatedIndividual<T> Memetic(T individual);

    /// <summary>
    /// Applies local improvement to a collection of individuals.
    /// </summary>
    /// <param name="individuals">Individuals to improve.</param>
    /// <returns>Collection of locally improved evaluated individuals.</returns>
    IReadOnlyList<EvaluatedIndividual<T>> Memetic(IReadOnlyList<T> individuals);

    /// <summary>
    /// Applies local improvement to a single evaluated individual.
    /// </summary>
    /// <param name="individual">Evaluated individual to improve.</param>
    /// <returns>Locally improved evaluated individual.</returns>
    EvaluatedIndividual<T> Memetic(EvaluatedIndividual<T> individual);

    /// <summary>
    /// Applies local improvement to a collection of evaluated individuals.
    /// </summary>
    /// <param name="individuals">Evaluated individuals to improve.</param>
    /// <returns>Collection of locally improved evaluated individuals.</returns>
    IReadOnlyList<EvaluatedIndividual<T>> Memetic(IReadOnlyList<EvaluatedIndividual<T>> individuals);
}