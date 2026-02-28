namespace EvolutionaryContainerPacking.Evolution.Architecture.Selection;

/// <summary>
/// Defines a selection strategy for evolutionary algorithms.
/// </summary>
/// <typeparam name="T">
/// Type of the individual representation.
/// </typeparam>
public interface ISelection<T>
{
    /// <summary>
    /// Selects a specified number of individuals from the population.
    /// </summary>
    IReadOnlyList<EvaluatedIndividual<T>> Select(IReadOnlyList<EvaluatedIndividual<T>> population, int number);

    /// <summary>
    /// Selects a single individual from the population.
    /// </summary>
    EvaluatedIndividual<T> Select(
        IReadOnlyList<EvaluatedIndividual<T>> population);
}